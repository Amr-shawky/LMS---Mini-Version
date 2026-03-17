using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.ViewModels.Track;
using LMS___Mini_Version.ViewModels.Intern;
using LMS___Mini_Version.ViewModels.Enrollment;
using LMS___Mini_Version.ViewModels.Payment;

namespace LMS___Mini_Version.Mapping
{
    public static class MappingExtensions
    {
        //  TRACK MAPPINGS
        public static TrackDto ToDto(this Track entity) => new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Fees = entity.Fees,
            IsActive = entity.IsActive,
        };

        public static TrackSummaryViewModel ToSummaryViewModel(this TrackDto dto) => new()
        {
            Id = dto.Id,
            Name = dto.Name,
            Fees = dto.Fees,
            IsActive = dto.IsActive
        };

        public static TrackDetailViewModel ToDetailViewModel(this TrackDto dto) => new()
        {
            Id = dto.Id,
            Name = dto.Name,
            Fees = dto.Fees,
            IsActive = dto.IsActive,
        };

        public static Track ToEntity(this CreateTrackViewModel vm) => new()
        {
            Name = vm.Name,
            Fees = vm.Fees,
            IsActive = vm.IsActive,
        };

        //  INTERN MAPPINGS

        public static InternDto ToDto(this Intern entity) => new()
        {
            Id = entity.Id,
            FullName = entity.FullName,
            Email = entity.Email,
            BirthYear = entity.BirthYear,
            Status = entity.Status,
            TrackId = entity.TrackId,
        };

        public static InternSummaryViewModel ToSummaryViewModel(this InternDto dto) => new()
        {
            Id = dto.Id,
            FullName = dto.FullName,
            Email = dto.Email,
            Status = dto.Status,
        };

        public static InternDetailViewModel ToDetailViewModel(this InternDto dto) => new()
        {
            Id = dto.Id,
            FullName = dto.FullName,
            Email = dto.Email,
            BirthYear = dto.BirthYear,
            Status = dto.Status,
            TrackId = dto.TrackId,
        };

        public static Intern ToEntity(this CreateInternViewModel vm) => new()
        {
            FullName = vm.FullName,
            Email = vm.Email,
            BirthYear = vm.BirthYear,
            Status = vm.Status,
            TrackId = vm.TrackId
        };

        //  ENROLLMENT MAPPINGS

        public static EnrollmentDto ToDto(this Enrollment entity) => new()
        {
            Id = entity.Id,
            Status = entity.Status
        };

        public static EnrollmentViewModel ToViewModel(this EnrollmentDto dto) => new()
        {
            Id = dto.Id,
            InternName = dto.InternName,
            TrackName = dto.TrackName,
            Status = dto.Status.ToString()
        };

        //  PAYMENT MAPPINGS

        public static PaymentDto ToDto(this Payment entity) => new()
        {
            Id = entity.Id,
            EnrollmentId = entity.EnrollmentId,
            Amount = entity.Amount,
            PaymentDate = entity.PaymentDate,
            Method = entity.Method,
            Status = entity.Status
        };

        public static PaymentViewModel ToViewModel(this PaymentDto dto) => new()
        {
            Id = dto.Id,
            EnrollmentId = dto.EnrollmentId,
            Amount = dto.Amount,
            PaymentDate = dto.PaymentDate,
            Method = dto.Method.ToString(),
            Status = dto.Status.ToString()
        };
    }
}