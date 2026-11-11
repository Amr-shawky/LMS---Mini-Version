
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Enums;

namespace LMS___Mini_Version.Persistence
{
    public static class DbInitializer
    {
        public static void Seed(AppDbContext context)
        {
            // Suitable for demo databases not managed by migrations.
            context.Database.EnsureCreated();

            // ==========================================
            // 1. TRACKS
            // ==========================================

            const string Net = ".NET Backend Development";
            const string Angular = "Angular Frontend Development";
            const string Cyber = "Cyber Security & Ethical Hacking";
            const string Flutter = "Mobile App Development (Flutter)";
            const string UIUX = "UI/UX Design Strategy";
            const string AI = "Artificial Intelligence & ML";
            const string React = "React Frontend Development";
            const string Data = "Data Engineering";
            const string DevOps = "DevOps & Cloud Engineering";
            const string Testing = "Software Testing & QA";

            var trackSeeds = new (string Name, int Fees, bool IsActive, int Capacity)[]
            {
                (Net, 8000, true, 30),
                (Angular, 7000, true, 25),
                (Cyber, 9500, true, 20),
                (Flutter, 7500, false, 20),
                (UIUX, 6000, true, 15),
                (AI, 12000, true, 15),
                (React, 7200, true, 25),
                (Data, 11000, true, 20),
                (DevOps, 10500, true, 20),
                (Testing, 6500, true, 30)
            };

            var existingTrackNames = context.Tracks
                .Select(t => t.Name)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var newTracks = trackSeeds
                .Where(t => !existingTrackNames.Contains(t.Name))
                .Select(t => new Track
                {
                    Name = t.Name,
                    Fees = t.Fees,
                    IsActive = t.IsActive,
                    MaxCapacity = t.Capacity
                })
                .ToList();

            context.Tracks.AddRange(newTracks);
            context.SaveChanges();

            var trackLookup = context.Tracks
                .ToDictionary(t => t.Name, StringComparer.OrdinalIgnoreCase);

            // ==========================================
            // 2. INTERNS
            // ==========================================

            var internSeeds = new (string Name, string Email, int BirthYear, string Status, string Track)[]
            {
                // Original interns
                ("Amr Mohamed", "amr@test.com", 2002, "Accepted", Net),
                ("Ahmed Ali", "ahmed.ali@example.com", 2000, "Accepted", Net),
                ("Sara Ahmed", "sara.dev@gmail.com", 2001, "Pending", Net),

                ("Mohamed Hassan", "m.hassan@outlook.com", 1999, "Accepted", Angular),
                ("Mona Mahmoud", "mona.m@company.com", 2003, "Accepted", Angular),
                ("Laila Ibrahim", "laila.i@web.com", 2000, "Pending", Angular),

                ("Omar Khaled", "omar.k@security.net", 1998, "Accepted", Cyber),
                ("Ziad Waleed", "ziad.w@cyber.org", 2001, "Accepted", Cyber),
                ("Mariam Saeed", "mariam.s@edu.eg", 2002, "Rejected", Cyber),

                ("Yassin Amr", "yassin.amr@flutter.io", 2004, "Pending", Flutter),
                ("Nour El-Din", "nour.design@behance.net", 1997, "Accepted", UIUX),
                ("Hoda Farouk", "hoda.f@ai.com", 2000, "Accepted", AI),

                ("Mostafa Radwan", "mostafa.r@test.com", 2001, "Accepted", Net),
                ("Fatma Zein", "fatma.z@dev.com", 1999, "Accepted", Angular),
                ("Kareem Adel", "kareem.a@safe.com", 2002, "Pending", Cyber),

                // Additional .NET interns
                ("Hady Tarek", "hady.tarek@example.com", 2001, "Accepted", Net),
                ("Ali Salah", "ali.salah@example.com", 2000, "Accepted", Net),
                ("Hadeer Mostafa", "hadeer.m@example.com", 2002, "Pending", Net),
                ("Jana Samir", "jana.samir@example.com", 2003, "Accepted", Net),

                // Additional Angular interns
                ("Belal Fathy", "belal.fathy@example.com", 1999, "Accepted", Angular),
                ("Nada Hossam", "nada.hossam@example.com", 2002, "Accepted", Angular),
                ("Malak Ashraf", "malak.ashraf@example.com", 2004, "Pending", Angular),

                // Additional Cyber Security interns
                ("Youssef Magdy", "youssef.magdy@example.com", 2000, "Accepted", Cyber),
                ("Menna Osama", "menna.osama@example.com", 2001, "Accepted", Cyber),

                // Flutter interns
                ("Seif Adel", "seif.adel@example.com", 2002, "Pending", Flutter),
                ("Basant Samy", "basant.samy@example.com", 2001, "Pending", Flutter),
                ("Mahmoud Galal", "mahmoud.galal@example.com", 2000, "Rejected", Flutter),

                // UI/UX interns
                ("Dalia Nabil", "dalia.nabil@example.com", 2000, "Accepted", UIUX),
                ("Salma Adel", "salma.adel@example.com", 2002, "Pending", UIUX),
                ("Rawan Tamer", "rawan.tamer@example.com", 2003, "Accepted", UIUX),

                // Artificial Intelligence interns
                ("Karim Mostafa", "karim.mostafa@example.com", 1999, "Accepted", AI),
                ("Yara Sami", "yara.sami@example.com", 2001, "Accepted", AI),
                ("Ahmed Fawzy", "ahmed.fawzy@example.com", 2002, "Pending", AI),

                // React interns
                ("Moustafa Nader", "moustafa.nader@example.com", 2001, "Accepted", React),
                ("Rania Kamal", "rania.kamal@example.com", 2000, "Accepted", React),
                ("Saif Hatem", "saif.hatem@example.com", 2003, "Pending", React),

                // Data Engineering interns
                ("Eman Ali", "eman.ali@example.com", 2001, "Accepted", Data),
                ("Hamza Adel", "hamza.adel@example.com", 1999, "Accepted", Data),
                ("Habiba Salah", "habiba.salah@example.com", 2002, "Pending", Data),

                // DevOps interns
                ("Mohab Amin", "mohab.amin@example.com", 2000, "Accepted", DevOps),
                ("Farah Mahmoud", "farah.mahmoud@example.com", 2002, "Accepted", DevOps),
                ("Tarek Emad", "tarek.emad@example.com", 1998, "Pending", DevOps),

                // Software Testing interns
                ("Reem Ahmed", "reem.ahmed@example.com", 2003, "Accepted", Testing),
                ("Abdelrahman Nabil", "abdelrahman.nabil@example.com", 2001, "Accepted", Testing),
                ("Doaa Youssef", "doaa.youssef@example.com", 2002, "Pending", Testing),
                ("Essam Hussein", "essam.hussein@example.com", 2000, "Accepted", Testing)
            };

            var existingEmails = context.Interns
                .Select(i => i.Email)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var newInterns = internSeeds
                .Where(i => !existingEmails.Contains(i.Email))
                .Select(i => new Intern
                {
                    FullName = i.Name,
                    Email = i.Email,
                    BirthYear = i.BirthYear,
                    Status = i.Status,
                    TrackId = trackLookup[i.Track].Id
                })
                .ToList();

            context.Interns.AddRange(newInterns);
            context.SaveChanges();

            var internLookup = context.Interns
                .ToDictionary(i => i.Email, StringComparer.OrdinalIgnoreCase);

            // ==========================================
            // 3. ENROLLMENTS
            // ==========================================

            // Helper to keep enrollment definitions readable.
            static (string Email, string Track, DateTime Date, EnrollmentStatus Status)
                E(string email, string track, int month, int day, EnrollmentStatus status)
                => (email, track, new DateTime(2026, month, day), status);

            var enrollmentSeeds = new[]
            {
                // Original enrollments
                E("amr@test.com", Net, 1, 15, EnrollmentStatus.Active),
                E("ahmed.ali@example.com", Net, 1, 16, EnrollmentStatus.Active),
                E("m.hassan@outlook.com", Angular, 1, 20, EnrollmentStatus.Active),
                E("mona.m@company.com", Angular, 1, 21, EnrollmentStatus.Pending),
                E("omar.k@security.net", Cyber, 2, 1, EnrollmentStatus.Active),

                // .NET enrollments
                E("mostafa.r@test.com", Net, 2, 3, EnrollmentStatus.Active),
                E("hady.tarek@example.com", Net, 2, 7, EnrollmentStatus.Active),
                E("ali.salah@example.com", Net, 2, 8, EnrollmentStatus.Active),
                E("hadeer.m@example.com", Net, 2, 9, EnrollmentStatus.Pending),
                E("jana.samir@example.com", Net, 2, 10, EnrollmentStatus.Active),
                E("sara.dev@gmail.com", Net, 4, 1, EnrollmentStatus.Pending),

                // Angular enrollments
                E("fatma.z@dev.com", Angular, 2, 4, EnrollmentStatus.Active),
                E("belal.fathy@example.com", Angular, 2, 11, EnrollmentStatus.Active),
                E("nada.hossam@example.com", Angular, 2, 12, EnrollmentStatus.Active),
                E("malak.ashraf@example.com", Angular, 2, 13, EnrollmentStatus.Pending),
                E("laila.i@web.com", Angular, 4, 2, EnrollmentStatus.Pending),

                // Cyber Security enrollments
                E("kareem.a@safe.com", Cyber, 2, 5, EnrollmentStatus.Pending),
                E("youssef.magdy@example.com", Cyber, 2, 14, EnrollmentStatus.Active),
                E("menna.osama@example.com", Cyber, 2, 15, EnrollmentStatus.Active),
                E("ziad.w@cyber.org", Cyber, 4, 3, EnrollmentStatus.Active),

                // UI/UX enrollments
                E("dalia.nabil@example.com", UIUX, 3, 1, EnrollmentStatus.Active),
                E("salma.adel@example.com", UIUX, 3, 2, EnrollmentStatus.Pending),
                E("rawan.tamer@example.com", UIUX, 3, 3, EnrollmentStatus.Active),
                E("nour.design@behance.net", UIUX, 4, 4, EnrollmentStatus.Active),

                // AI enrollments
                E("karim.mostafa@example.com", AI, 3, 4, EnrollmentStatus.Active),
                E("yara.sami@example.com", AI, 3, 5, EnrollmentStatus.Active),
                E("ahmed.fawzy@example.com", AI, 3, 6, EnrollmentStatus.Pending),
                E("hoda.f@ai.com", AI, 4, 5, EnrollmentStatus.Active),

                // React enrollments
                E("moustafa.nader@example.com", React, 3, 8, EnrollmentStatus.Active),
                E("rania.kamal@example.com", React, 3, 9, EnrollmentStatus.Active),
                E("saif.hatem@example.com", React, 3, 10, EnrollmentStatus.Pending),

                // Data Engineering enrollments
                E("eman.ali@example.com", Data, 3, 11, EnrollmentStatus.Active),
                E("hamza.adel@example.com", Data, 3, 12, EnrollmentStatus.Active),
                E("habiba.salah@example.com", Data, 3, 13, EnrollmentStatus.Pending),

                // DevOps enrollments
                E("mohab.amin@example.com", DevOps, 3, 14, EnrollmentStatus.Active),
                E("farah.mahmoud@example.com", DevOps, 3, 15, EnrollmentStatus.Active),
                E("tarek.emad@example.com", DevOps, 3, 16, EnrollmentStatus.Pending),

                // Software Testing enrollments
                E("reem.ahmed@example.com", Testing, 3, 17, EnrollmentStatus.Active),
                E("abdelrahman.nabil@example.com", Testing, 3, 18, EnrollmentStatus.Active),
                E("doaa.youssef@example.com", Testing, 3, 19, EnrollmentStatus.Pending),
                E("essam.hussein@example.com", Testing, 3, 20, EnrollmentStatus.Active),

                // Multiple enrollments per intern
                E("amr@test.com", React, 5, 1, EnrollmentStatus.Pending),
                E("amr@test.com", DevOps, 5, 10, EnrollmentStatus.Active),
                E("ahmed.ali@example.com", Cyber, 5, 3, EnrollmentStatus.Pending),
                E("m.hassan@outlook.com", React, 5, 7, EnrollmentStatus.Active),
                E("mona.m@company.com", UIUX, 5, 7, EnrollmentStatus.Active),
                E("fatma.z@dev.com", AI, 6, 2, EnrollmentStatus.Active),
                E("fatma.z@dev.com", Data, 6, 5, EnrollmentStatus.Pending),
                E("omar.k@security.net", AI, 6, 6, EnrollmentStatus.Active),
                E("hoda.f@ai.com", Data, 6, 7, EnrollmentStatus.Pending),
                E("nour.design@behance.net", React, 6, 8, EnrollmentStatus.Pending),
                E("essam.hussein@example.com", DevOps, 7, 1, EnrollmentStatus.Active),
                E("hady.tarek@example.com", Angular, 7, 2, EnrollmentStatus.Pending),
                E("ali.salah@example.com", Angular, 7, 3, EnrollmentStatus.Active),

                // Same date, different enrollments:
                // useful for testing latest enrollment tie-breaking.
                E("mostafa.r@test.com", DevOps, 6, 1, EnrollmentStatus.Pending),
                E("mostafa.r@test.com", UIUX, 6, 1, EnrollmentStatus.Active)
            };

            // Key: InternId + TrackId + EnrollmentDate
            var enrollmentLookup = context.Enrollments
                .ToList()
                .GroupBy(e => (e.InternId, e.TrackId, e.EnrollmentDate))
                .ToDictionary(g => g.Key, g => g.First());

            foreach (var seed in enrollmentSeeds)
            {
                int internId = internLookup[seed.Email].Id;
                int trackId = trackLookup[seed.Track].Id;

                var key = (internId, trackId, seed.Date);

                if (enrollmentLookup.ContainsKey(key))
                    continue;

                var enrollment = new Enrollment
                {
                    InternId = internId,
                    TrackId = trackId,
                    EnrollmentDate = seed.Date,
                    Status = seed.Status
                };

                context.Enrollments.Add(enrollment);
                enrollmentLookup.Add(key, enrollment);
            }

            context.SaveChanges();

            // ==========================================
            // 4. PAYMENTS
            // ==========================================

            var feesByTrack = trackSeeds.ToDictionary(
                t => t.Name,
                t => t.Fees,
                StringComparer.OrdinalIgnoreCase);

            var paymentMethods = new[]
            {
                PaymentMethod.CreditCard,
                PaymentMethod.Cash,
                PaymentMethod.BankTransfer,
                PaymentMethod.Online
            };

            var paidEnrollmentIds = context.Payments
                .Select(p => p.EnrollmentId)
                .ToHashSet();

            var newPayments = new List<Payment>();

            for (int i = 0; i < enrollmentSeeds.Length; i++)
            {
                // Leave some enrollments without payments
                // to support unpaid enrollment queries.
                if (i % 6 == 5)
                    continue;

                var seed = enrollmentSeeds[i];

                var key = (
                    internLookup[seed.Email].Id,
                    trackLookup[seed.Track].Id,
                    seed.Date
                );

                var enrollment = enrollmentLookup[key];

                if (paidEnrollmentIds.Contains(enrollment.Id))
                    continue;

                var payment = new Payment
                {
                    EnrollmentId = enrollment.Id,
                    Amount = feesByTrack[seed.Track],
                    PaymentDate = seed.Date.AddDays(i < 5 ? 0 : i % 3),
                    Method = paymentMethods[i % paymentMethods.Length],
                    Status = seed.Status == EnrollmentStatus.Active
                        ? PaymentStatus.Completed
                        : PaymentStatus.Pending
                };

                newPayments.Add(payment);
                paidEnrollmentIds.Add(enrollment.Id);
            }

            context.Payments.AddRange(newPayments);
            context.SaveChanges();
        }
    }
}
