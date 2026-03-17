namespace LMS___Mini_Version.Domain.Entities
{
    public class Payment
    {

        public int Id { get; set; }
        public int EnrollmentId { get; set; }
        public decimal Amount { get; set; }
        public DateOnly PaymentDate { get; set; }
        public string Method { get; set; }
        public string Status { get; set; }
    }
}
