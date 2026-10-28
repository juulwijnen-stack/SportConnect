namespace SportConnect.Models;

public enum SkillLevel
{
    Beginner,
    Intermediate,
    Advanced
}

public class Activity
{
    public int Id { get; set; }

    // De sport staat gewoon in de titel, bijvoorbeeld "Football 5 vs 5"
    public string Title { get; set; } = "";

    public SkillLevel Level { get; set; }
    public DateTime StartsAt { get; set; }
    public string Location { get; set; } = "";
    public int MaxParticipants { get; set; }
    public string? Description { get; set; }

    public int OrganizerId { get; set; }
    public AppUser Organizer { get; set; } = null!;

    public List<Participation> Participants { get; set; } = new();
}
