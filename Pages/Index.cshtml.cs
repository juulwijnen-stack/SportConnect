using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SportConnect.Data;
using SportConnect.Models;

namespace SportConnect.Pages;

public class IndexModel : PageModel
{
    private readonly AppDbContext _db;

    public IndexModel(AppDbContext db) => _db = db;

    public string Username => User.Identity?.Name ?? "";
    public List<Announcement> Announcements { get; set; } = new();
    public List<Activity> Activities { get; set; } = new();

    public async Task OnGetAsync()
    {
        Announcements = await _db.Announcements
            .OrderByDescending(a => a.PostedAt)
            .Take(3)
            .ToListAsync();

        var now = DateTime.Now;
        Activities = await _db.Activities
            .Include(a => a.Participants)
            .Where(a => a.StartsAt >= now)
            .OrderBy(a => a.StartsAt)
            .ToListAsync();
    }
}
