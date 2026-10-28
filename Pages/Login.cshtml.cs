using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using SportConnect.Data;
using SportConnect.Models;
using SportConnect.Services;

namespace SportConnect.Pages;

[EnableRateLimiting("login")]
public class LoginModel : PageModel
{
    private readonly AppDbContext _db;
    private readonly IPasswordHasher<AppUser> _hasher;

    public LoginModel(AppDbContext db, IPasswordHasher<AppUser> hasher)
    {
        _db = db;
        _hasher = hasher;
    }

    [BindProperty, Required]
    public string Username { get; set; } = "";

    [BindProperty, Required, DataType(DataType.Password)]
    public string Password { get; set; } = "";

    public string? Error { get; set; }

    public IActionResult OnGet()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToPage("/Index");
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == Username);
        var ok = user is not null &&
                 _hasher.VerifyHashedPassword(user, user.PasswordHash, Password)
                     != PasswordVerificationResult.Failed;

        if (!ok)
        {
            // Bewust dezelfde melding voor "onbekende gebruiker" en "fout wachtwoord"
            Error = "Wrong username or password.";
            return Page();
        }

        await AuthHelper.SignInAsync(HttpContext, user!);
        return RedirectToPage("/Index");
    }
}
