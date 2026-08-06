using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;

namespace LMS___Mini_Version.Persistence
{
    public static class DbInitializer
    {
        public static void Seed(AppDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Tracks.Any()) return;

            var tracks = new List<Track>
            {
                new Track { Name = ".NET Backend Development", Fees = 8000, IsActive = true, MaxCapacity = 30 },
                new Track { Name = "Angular Frontend Development", Fees = 7000, IsActive = true, MaxCapacity = 25 },
                new Track { Name = "Cyber Security & Ethical Hacking", Fees = 9500, IsActive = true, MaxCapacity = 20 },
                new Track { Name = "Mobile App Development (Flutter)", Fees = 7500, IsActive = false, MaxCapacity = 20 },
                new Track { Name = "UI/UX Design Strategy", Fees = 6000, IsActive = true, MaxCapacity = 15 },
                new Track { Name = "Artificial Intelligence & ML", Fees = 12000, IsActive = true, MaxCapacity = 15 }
            };

            context.Tracks.AddRange(tracks);
            context.SaveChanges();

            var interns = new List<Intern>
            {
                new Intern { FullName = "Amr Mohamed", Email = "amr@test.com", BirthYear = 2002, Status = "Accepted", TrackId = 1 },
                new Intern { FullName = "Ahmed Ali", Email = "ahmed.ali@example.com", BirthYear = 2000, Status = "Accepted", TrackId = 1 },
                new Intern { FullName = "Sara Ahmed", Email = "sara.dev@gmail.com", BirthYear = 2001, Status = "Pending", TrackId = 1 },
                new Intern { FullName = "Mohamed Hassan", Email = "m.hassan@outlook.com", BirthYear = 1999, Status = "Accepted", TrackId = 2 },
                new Intern { FullName = "Mona Mahmoud", Email = "mona.m@company.com", BirthYear = 2003, Status = "Accepted", TrackId = 2 },
                new Intern { FullName = "Laila Ibrahim", Email = "laila.i@web.com", BirthYear = 2000, Status = "Pending", TrackId = 2 },
                new Intern { FullName = "Omar Khaled", Email = "omar.k@security.net", BirthYear = 1998, Status = "Accepted", TrackId = 3 },
                new Intern { FullName = "Ziad Waleed", Email = "ziad.w@cyber.org", BirthYear = 2001, Status = "Accepted", TrackId = 3 },
                new Intern { FullName = "Mariam Saeed", Email = "mariam.s@edu.eg", BirthYear = 2002, Status = "Rejected", TrackId = 3 },
                new Intern { FullName = "Yassin Amr", Email = "yassin.amr@flutter.io", BirthYear = 2004, Status = "Pending", TrackId = 4 },
                new Intern { FullName = "Nour El-Din", Email = "nour.design@behance.net", BirthYear = 1997, Status = "Accepted", TrackId = 5 },
                new Intern { FullName = "Hoda Farouk", Email = "hoda.f@ai.com", BirthYear = 2000, Status = "Accepted", TrackId = 6 },
                new Intern { FullName = "Mostafa Radwan", Email = "mostafa.r@test.com", BirthYear = 2001, Status = "Accepted", TrackId = 1 },
                new Intern { FullName = "Fatma Zein", Email = "fatma.z@dev.com", BirthYear = 1999, Status = "Accepted", TrackId = 2 },
                new Intern { FullName = "Kareem Adel", Email = "kareem.a@safe.com", BirthYear = 2002, Status = "Pending", TrackId = 3 }
            };

            context.Interns.AddRange(interns);
            context.SaveChanges();

            // Seed some enrollments
            var enrollments = new List<Enrollment>
            {
                new Enrollment { InternId = 1, TrackId = 1, EnrollmentDate = new DateTime(2026, 1, 15), Status = EnrollmentStatus.Active },
                new Enrollment { InternId = 2, TrackId = 1, EnrollmentDate = new DateTime(2026, 1, 16), Status = EnrollmentStatus.Active },
                new Enrollment { InternId = 4, TrackId = 2, EnrollmentDate = new DateTime(2026, 1, 20), Status = EnrollmentStatus.Active },
                new Enrollment { InternId = 5, TrackId = 2, EnrollmentDate = new DateTime(2026, 1, 21), Status = EnrollmentStatus.Pending },
                new Enrollment { InternId = 7, TrackId = 3, EnrollmentDate = new DateTime(2026, 2, 1), Status = EnrollmentStatus.Active },
            };

            context.Enrollments.AddRange(enrollments);
            context.SaveChanges();

            // Seed payments for the enrollments above
            var payments = new List<Payment>
            {
                new Payment { EnrollmentId = 1, Amount = 8000, PaymentDate = new DateTime(2026, 1, 15), Method = PaymentMethod.CreditCard, Status = PaymentStatus.Completed },
                new Payment { EnrollmentId = 2, Amount = 8000, PaymentDate = new DateTime(2026, 1, 16), Method = PaymentMethod.Cash, Status = PaymentStatus.Completed },
                new Payment { EnrollmentId = 3, Amount = 7000, PaymentDate = new DateTime(2026, 1, 20), Method = PaymentMethod.BankTransfer, Status = PaymentStatus.Completed },
                new Payment { EnrollmentId = 4, Amount = 7000, PaymentDate = new DateTime(2026, 1, 21), Method = PaymentMethod.Online, Status = PaymentStatus.Pending },
                new Payment { EnrollmentId = 5, Amount = 9500, PaymentDate = new DateTime(2026, 2, 1), Method = PaymentMethod.CreditCard, Status = PaymentStatus.Completed },
            };

            context.Payments.AddRange(payments);
            context.SaveChanges();
        }
    }
}