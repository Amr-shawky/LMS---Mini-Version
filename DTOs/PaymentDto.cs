namespace LMS___Mini_Version.DTOs
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public int EnrollmentId { get; set; }
        public decimal Amount { get; set; }
        public DateOnly PaymentDate { get; set; }
        public string Method { get; set; }
        public string Status { get; set; }

    }
}
