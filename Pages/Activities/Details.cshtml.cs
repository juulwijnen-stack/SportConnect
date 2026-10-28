using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SportConnect.Data;
using SportConnect.Models;

namespace SportConnect.Pages.Activities;

public class DetailsModel : PageModel
{
    private readonly AppDbContext _db;

    public DetailsModel(AppDbContext db) => _db = db;

    public Activity Activity { get; set; } = null!;
    public bool IsJoined { get; set; }
    public bool IsFull => Activity.Participants.Count >= Activity.MaxParticipants;
    public int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    public async Task<IActionResult> OnGetAsync(int id)
    {
        if (!await LoadAsync(id)) return NotFound();
        return Page();
    }

    public async Task<IActionResult> OnPostJoinAsync(int id)
    {
        if (!await LoadAsync(id)) return NotFound();

        if (!IsJoined && !IsFull && Activity.StartsAt > DateTime.Now)
        {
            _db.Participations.Add(new Participation { ActivityId = id, UserId = CurrentUserId });
            await _db.SaveChangesAsync();
        }

        return RedirectToPage(new { id });
    }

    public async Task<IActionResult> OnPostLeaveAsync(int id)
    {
        if (!await LoadAsync(id)) return NotFound();

        var participation = Activity.Participants.FirstOrDefault(p => p.UserId == CurrentUserId);
        // De organisator kan niet weggaan uit de eigen activiteit
        if (participation is not null && Activity.OrganizerId != CurrentUserId)
        {
            _db.Participations.Remove(participation);
            await _db.SaveChangesAsync();
        }

        return RedirectToPage(new { id });
    }

    private async Task<bool> LoadAsync(int id)
    {
        var activity = await _db.Activities
            .Include(a => a.Organizer)
            .Include(a => a.Participants).ThenInclude(p => p.User)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (activity is null) return false;

        Activity = activity;
        IsJoined = activity.Participants.Any(p => p.UserId == CurrentUserId);
        return true;
    }
}
