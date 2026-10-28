using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SportConnect.Data;
using SportConnect.Models;

namespace SportConnect.Pages;

public class AnnouncementsModel : PageModel
{
    private readonly AppDbContext _db;

    public AnnouncementsModel(AppDbContext db) => _db = db;

    public List<Announcement> Announcements { get; set; } = new();

    public async Task OnGetAsync()
    {
        Announcements = await _db.Announcements
            .OrderByDescending(a => a.PostedAt)
            .ToListAsync();
    }
}
