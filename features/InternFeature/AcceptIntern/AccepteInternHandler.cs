using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.InternFeature.ApproveRegisterIntern;
using LMS___Mini_Version.features.shared;
using MediatR;
using System.Runtime.InteropServices;

namespace LMS___Mini_Version.features.InternFeature.AcceptIntern
{
    public class AccepteInternHandler : IRequestHandler<AccepteInternCommand, RequestResponse<int>>
    {
        private readonly IGeneralRepository<Intern> _internRepository;
        public AccepteInternHandler(IGeneralRepository<Intern> generalRepository)
        {
            _internRepository = generalRepository;
        }
        public async Task<RequestResponse<int>> Handle(AccepteInternCommand request, CancellationToken cancellationToken)
        {
            var result = _internRepository.GetAll().Where(i => i.Id == request.internId).Select(i => 
                new  { intern=i,TrackId=i.TrackId , IsActive=i.Track.IsActive }).FirstOrDefault();
            Intern? intern = result?.intern;
            if (intern is null)
                return RequestResponse<int>.Fail(404, errorCode: ErrorCode.InternNotFound);

            if(result.IsActive == false)
                return RequestResponse<int>.Fail(400, errorCode: ErrorCode.InternTrackNotActive);

            intern.Status = "Accepted";
            _internRepository.Update(intern);
            return RequestResponse<int>.OK(result.TrackId);

        }
    }
}
