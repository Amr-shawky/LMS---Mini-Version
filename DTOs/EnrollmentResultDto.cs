namespace LMS___Mini_Version.DTOs;

public class EnrollmentResultDto
{
    public bool IsSuccess { get; set; }
    
    public string ErrorMessage { get; set; } = string.Empty;
    public EnrollmentDto? Enrollment { get; set; }
    public PaymentDto? Payment { get; set; }

    public static EnrollmentResultDto Fail(string errorMessage) => new()
    {
        IsSuccess = false,
        ErrorMessage = errorMessage
    };
    public static EnrollmentResultDto Succeed
        (EnrollmentDto enrollment, PaymentDto? payment) => new()
    {
        IsSuccess = true,
        Enrollment = enrollment,
        Payment = payment
    };

   
}