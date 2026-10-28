namespace SportConnect.Models;

public class AppUser
{
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public List<Participation> Participations { get; set; } = new();
}
