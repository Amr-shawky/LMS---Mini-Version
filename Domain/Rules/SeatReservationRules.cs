using System.Linq.Expressions;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;

namespace LMS___Mini_Version.Domain.Rules
{
    /// <summary>
    /// Single source of truth for track seat calculations, shared by
    /// the availability query and the approve-and-register command.
    /// </summary>
    public static class SeatReservationRules
    {
        /// <summary>
        /// Pending and Active enrollments hold a seat; Completed and Cancelled ones do not.
        /// The same statuses mark an enrollment as still "open" for duplicate checks.
        /// </summary>
        public static readonly Expression<Func<Enrollment, bool>> ReservesSeat =
            e => e.Status == EnrollmentStatus.Pending || e.Status == EnrollmentStatus.Active;

        /// <summary>
        /// Never negative, even when existing data already exceeds capacity.
        /// </summary>
        public static int CalculateAvailableSeats(int maxCapacity, int reservedSeats)
            => Math.Max(0, maxCapacity - reservedSeats);
    }
}
