namespace LMS___Mini_Version.Domain.Constants
{
    /// <summary>
    /// Intern.Status is stored as a string; these are the values the business uses.
    /// </summary>
    public static class InternStatuses
    {
        public const string Pending = "Pending";
        public const string Accepted = "Accepted";
        public const string Rejected = "Rejected";

        public static bool IsPending(string? status)
            => string.Equals(status, Pending, StringComparison.OrdinalIgnoreCase);
    }
}
