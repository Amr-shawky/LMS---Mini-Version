using LMS___Mini_Version.Persistence;
using MediatR;
namespace LMS___Mini_Version.features.shared.Behaviors
{
    public class TransactionBehavior<TRequest, TResponse>: IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly AppDbContext _dbContext;

        public TransactionBehavior(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (request is not ITransactionalRequest)
            {
                return await next();
            }
            if (_dbContext.Database.CurrentTransaction is not null)
            {
                return await next();
            }

            await using var transaction =await _dbContext.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var response = await next();
                if (response is not IResponseStatus result)
                {
                    throw new InvalidOperationException("Transactional requests must return IResponseStatus.");
                }
                
                if (!result.Success)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return response;
                }
                await transaction.CommitAsync(cancellationToken);
                return response;
            }
            catch
            {
                await transaction.RollbackAsync( CancellationToken.None);
                throw;
            }
        }
    }
}
