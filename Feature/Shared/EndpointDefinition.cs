using exam_system.Features.Shared;
using LMS___Mini_Version.Domain.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LMS___Mini_Version.Feature.Shared
{
    public abstract class EndpointDefinition
    {
        public abstract void RegisterEndpoints(IEndpointRouteBuilder app);

        // ─────────────────────────────────────────────────────────────
        // 1) Response helpers (no transaction involved)
        // ─────────────────────────────────────────────────────────────
        
        protected static IResult Response<T>(RequestResponse<T> result)
            => Results.Ok(EndpointResponse<T>.FromResult(result));

        protected static IResult Response<T>(PaginatedResult<T> result)
            => Results.Ok(EndpointResponse<PaginatedResult<T>>.Ok(result));

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

        /// <summary>Handler already returns an IResult without requiring generic type argument.</summary>
        protected static Task<IResult> ExecuteWithTransactionAsync(
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

            return Results.Ok(EndpointResponse<bool>.Ok(true, "Operation completed successfully"));
        }

        /// <summary>
        /// Handler returns a RequestResponse (business success/failure without exceptions).
        /// A business failure ROLLS BACK instead of committing.
        /// </summary>
        protected static async Task<IResult> ExecuteWithTransactionAsync<T>(
            Func<Task<RequestResponse<T>>> handler,
            HttpContext httpContext)
        {
            var result = await RunInTransactionAsync(
                handler,
                httpContext,
                shouldCommit: r => r.Success);

            return Results.Ok(EndpointResponse<T>.FromResult(result));
        }

        // ─────────────────────────────────────────────────────────────
        // 3) The single place that owns transactions via IUnitOfWork
        // ─────────────────────────────────────────────────────────────

        private static async Task<TResult> RunInTransactionAsync<TResult>(
            Func<Task<TResult>> handler,
            HttpContext httpContext,
            Func<TResult, bool>? shouldCommit = null)
        {
            var unitOfWork = httpContext.RequestServices.GetRequiredService<IUnitOfWork>();
            var logger = httpContext.RequestServices.GetRequiredService<ILogger<EndpointDefinition>>();

            TResult result = default!;

            try
            {
                await unitOfWork.ExecuteAsync(async () =>
                {
                    result = await handler();

                    if (shouldCommit != null && !shouldCommit(result))
                    {
                        // Business failure: signal rollback to UnitOfWork
                        throw new BusinessRollbackException();
                    }
                });

                return result;
            }
            catch (BusinessRollbackException)
            {
                // Transaction was rolled back by UnitOfWork for business failure; return result
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Transaction rolled back due to exception in endpoint: {Endpoint}",
                    httpContext.Request.Path);

                throw;   // let the global exception middleware build the error response
            }
        }

        private sealed class BusinessRollbackException : Exception
        {
        }
    }
}
