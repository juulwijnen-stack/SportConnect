using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportConnect.Data;
using SportConnect.Models;

namespace SportConnect.Pages.Activities;

public class CreateModel : PageModel
{
    private readonly AppDbContext _db;

    public CreateModel(AppDbContext db) => _db = db;

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public class InputModel
    {
        [Required, StringLength(80)]
        public string Title { get; set; } = "";

        public SkillLevel Level { get; set; }

        [Required(ErrorMessage = "Please choose a date and time.")]
        [Display(Name = "Date and time")]
        public DateTime? StartsAt { get; set; }

        [Required, StringLength(120)]
        public string Location { get; set; } = "";

        [Range(2, 100)]
        [Display(Name = "Max participants")]
        public int MaxParticipants { get; set; } = 6;

        [StringLength(500)]
        public string? Description { get; set; }
    }

    public void OnGet()
    {
        Input.StartsAt = DateTime.Today.AddDays(1).AddHours(18);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (Input.StartsAt.HasValue && Input.StartsAt.Value <= DateTime.Now)
            ModelState.AddModelError("Input.StartsAt", "The activity must be in the future.");

        if (!ModelState.IsValid) return Page();

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var activity = new Activity
        {
            Title = Input.Title.Trim(),
            Level = Input.Level,
            StartsAt = Input.StartsAt!.Value,
            Location = Input.Location.Trim(),
            MaxParticipants = Input.MaxParticipants,
            Description = string.IsNullOrWhiteSpace(Input.Description) ? null : Input.Description.Trim(),
            OrganizerId = userId,
            // De organisator doet automatisch mee
            Participants = { new Participation { UserId = userId } }
        };

        _db.Activities.Add(activity);
        await _db.SaveChangesAsync();

        return RedirectToPage("Details", new { id = activity.Id });
    }
}
