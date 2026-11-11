using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;

namespace LMS___Mini_Version.features.TrackFeature.CheckTrack.Dtos
{
    public class CheckTrackDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int MaxCapacity { get; set; }
        public int ReservedSeats { get; set; }
        public int AvailableSeats { get; set; }
    }
}
