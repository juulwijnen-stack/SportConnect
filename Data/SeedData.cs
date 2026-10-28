using Microsoft.AspNetCore.Identity;
using SportConnect.Models;

namespace SportConnect.Data;

public static class SeedData
{
    public static void Initialize(AppDbContext db)
    {
        db.Database.EnsureCreated();
        if (db.Users.Any()) return;

        // Standaard testaccount (verwijder dit voor echt gebruik)
        var test = new AppUser { Username = "Test" };
        test.PasswordHash = new PasswordHasher<AppUser>().HashPassword(test, "Test1234");
        db.Users.Add(test);

        db.Announcements.AddRange(
            new Announcement
            {
                Title = "Welcome to SportConnect!",
                Text = "Find people to sport with, or create your own activity with the + button.",
                PostedAt = DateTime.Now.AddDays(-3)
            },
            new Announcement
            {
                Title = "Be kind and respectful",
                Text = "Everyone is welcome, from beginner to advanced. Keep it friendly.",
                PostedAt = DateTime.Now.AddDays(-2)
            },
            new Announcement
            {
                Title = "Meet in public places",
                Text = "For your safety, meet at public sports locations and do not share your home address.",
                PostedAt = DateTime.Now.AddDays(-1)
            });

        db.Activities.AddRange(
            new Activity
            {
                Title = "Running: evening run in the park",
                Level = SkillLevel.Beginner,
                StartsAt = DateTime.Today.AddDays(2).AddHours(18),
                Location = "City Park, main entrance",
                MaxParticipants = 8,
                Description = "Relaxed 5 km at a conversation pace.",
                Organizer = test,
                Participants = { new Participation { User = test } }
            },
            new Activity
            {
                Title = "Football 5 vs 5",
                Level = SkillLevel.Intermediate,
                StartsAt = DateTime.Today.AddDays(4).AddHours(20),
                Location = "Sportshall North",
                MaxParticipants = 10,
                Description = "Bring indoor shoes.",
                Organizer = test,
                Participants = { new Participation { User = test } }
            });

        db.SaveChanges();
    }
}
