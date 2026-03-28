// ╔══════════════════════════════════════════════════════════════╗
// ║  🎯 ASSIGNMENT: Create GetPaymentByIdQuery here             ║
// ║                                                              ║
// ║  File: Features/Payments/Queries/GetPaymentByIdQuery.cs     ║
// ║  See CQRS_Practice_Assignment.md → Task 6 for details      ║
// ╚══════════════════════════════════════════════════════════════╝

using LMS___Mini_Version.DTOs;
using MediatR;

public record GetPaymentByIdQuery(int Id) : IRequest<PaymentDto>;