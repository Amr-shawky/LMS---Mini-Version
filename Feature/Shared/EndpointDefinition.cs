using exam_system.Features.Shared;
using System.Transactions;

namespace LMS___Mini_Version.Feature.Shared
{
    public abstract class EndpointDefinition
    {
        public abstract void RegisterEndpoints(IEndpointRouteBuilder app);

        // ─────────────────────────────────────────────────────────────
        // 1) Response helpers (no transaction involved)
        // ─────────────────────────────────────────────────────────────
        
        protected static IResult Response<T>(RequestResponse<T> result)
            => Results.Ok((EndpointResponse<T>)result);

        protected static IResult Response<T>(PagingViewModel<T> result)
            => Results.Ok(EndpointResponse<PagingViewModel<T>>.Ok(result));

        // ─────────────────────────────────────────────────────────────
        // 2) Public overloads — each one only decides HOW to shape the response.
        //    The transaction logic lives in ONE place: RunInTransactionAsync.
        // ─────────────────────────────────────────────────────────────

        /// <summary>Handler already returns an IResult → return it as is.</summary>
        /// <remarks>T is unused; kept only so existing call sites keep compiling.</remarks>
        protected static Task<IResult> ExecuteWithTransactionAsync<T>(
            Func<Task<IResult>> handler,
            HttpContext httpContext)
            => RunInTransactionAsync(handler, httpContext);

        /// <summary>Handler returns a plain value → wrap it in a success response.</summary>
        protected static async Task<IResult> ExecuteWithTransactionAsync<T>(
            Func<Task<T>> handler,
            HttpContext httpContext)
        {
            var result = await RunInTransactionAsync(handler, httpContext);
            return Results.Ok(EndpointResponse<T>.Ok(result));
        }

        /// <summary>Handler returns nothing → respond with a generic success message.</summary>
        protected static async Task<IResult> ExecuteWithTransactionAsync(
            Func<Task> handler,
            HttpContext httpContext)
        {
            await RunInTransactionAsync(async () =>
            {
                await handler();
                return true;
            }, httpContext);

            return Results.Ok(EndPointResponse<bool>.Success(true, "Operation completed successfully"));
        }

        /// <summary>
        /// Handler returns a RequestResult (business success/failure without exceptions).
        /// A business failure ROLLS BACK instead of committing.
        /// </summary>
        protected static async Task<IResult> ExecuteWithTransactionAsync<T>(
            Func<Task<RequestResponse<T>>> handler,
            HttpContext httpContext)
        {
            var result = await RunInTransactionAsync(
                handler,
                httpContext,
                shouldCommit: r => r.IsSuccess);

            return result.IsSuccess
                ? Results.Ok(EndPointResponse<T>.Success(result.Data, result.Message))
                : Results.Ok(EndPointResponse<T>.Failure(result.ErrorCode, result.Message));
        }

        // ─────────────────────────────────────────────────────────────
        // 3) The single place that owns Begin / Commit / Rollback
        // ─────────────────────────────────────────────────────────────

        private static async Task<TResult> RunInTransactionAsync<TResult>(
            Func<Task<TResult>> handler,
            HttpContext httpContext,
            Func<TResult, bool>? shouldCommit = null)
        {
            var transactionManager = httpContext.RequestServices.GetRequiredService<TransactionManager>();
            var logger = httpContext.RequestServices.GetRequiredService<ILogger<EndpointDefinition>>();

            transactionManager.BeginTransaction();

            try
            {
                var result = await handler();

                if (shouldCommit is null || shouldCommit(result))
                    await transactionManager.CommitTransactionAsync();
                else
                    await transactionManager.RollbackTransactionAsync();   // business failure, no exception

                return result;
            }
            catch (Exception ex)
            {
                // FIX: the original checked a local `transaction` variable that was never assigned,
                // so this block never ran. We now roll back unconditionally.
                await transactionManager.RollbackTransactionAsync();

                logger.LogError(ex, "Transaction rolled back due to exception in endpoint: {Endpoint}",
                    httpContext.Request.Path);

                throw;   // let the global exception middleware build the error response
            }
        }
    }
}
