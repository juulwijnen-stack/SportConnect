namespace SportConnect.Models;

// Koppeltabel: welke gebruiker doet mee aan welke activiteit
public class Participation
{
    public int ActivityId { get; set; }
    public Activity Activity { get; set; } = null!;

    public int UserId { get; set; }
    public AppUser User { get; set; } = null!;
}
