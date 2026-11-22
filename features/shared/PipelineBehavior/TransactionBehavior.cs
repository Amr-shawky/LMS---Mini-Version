using LMS___Mini_Version.Domain.Repositories;
using MediatR;

namespace LMS___Mini_Version.features.shared.PipelineBehavior
{
    public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>, ITransactionalRequest
        where TResponse : RequestResponse
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransactionBehavior(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken ct, RequestHandlerDelegate<TResponse> next)
        {
            // in case of nested orchestrator => outermost one calls the inner
            if (_unitOfWork.HasActiveTransaction)
                return await next();

            await _unitOfWork.BeginTransactionAsync(ct);

            try
            {
                var response = await next(); // response of outermost orchestrator
                if (response.Success) await _unitOfWork.CommitAsync(ct);
                else
                    await _unitOfWork.RollbackAsync(ct);
                return response;
            }
            catch
            {
                await _unitOfWork.RollbackAsync(ct);
                throw;
            }
        }
    }
}
