# SportConnect (prototype)

Sports community platform in ASP.NET Core Razor Pages (C#, .NET 10), gebouwd op basis van de
wireframes: Login -> Dashboard -> Announcements.

## Starten

Vereist: .NET 10 SDK.

```bash
dotnet run
```

Open daarna https://localhost:7080 (of http://localhost:5080).
Bij de eerste keer starten wordt `sportconnect2.db` (SQLite) automatisch aangemaakt met testdata.

**Testaccount:** gebruikersnaam `Test`, wachtwoord `Test1234` (staat in `Data/SeedData.cs`).

Eerste keer HTTPS-fout in de browser? Voer eenmalig `dotnet dev-certs https --trust` uit.

## Wat zit erin (MoSCoW)

| Prioriteit | Onderdeel | Waar |
|---|---|---|
| Must | Startup/login pagina | `Pages/Login` |
| Must | Homepagina (dashboard) | `Pages/Index` |
| Must | Announcements pagina | `Pages/Announcements` |
| Must | Activiteiten aanmaken | `Pages/Activities/Create` |
| Should | Account aanmaken | alleen de knop staat op de loginpagina, werkt niet |
| Should | Activiteitenpagina | `Pages/Activities/Details` |
| Should | Terug naar homepagina | home-icoon (⌂) |
| Could | Filters | alleen het vak staat er, werkt niet |
| Could | Lijst van deelnemers + meedoen/afmelden | `Pages/Activities/Details` |
| Could | Meldingen | nog niet gebouwd |
| Won't | Zoekfunctie, beoordelingen, kaart, matching | niet gebouwd |

De sport geef je zelf aan in de titel van de activiteit (bijvoorbeeld "Football 5 vs 5").

## Beveiliging

- Wachtwoorden gehasht (ASP.NET `PasswordHasher`)
- Cookie-authenticatie (HttpOnly), alle pagina's behalve Login vereisen inloggen
- Anti-forgery (CSRF) staat standaard aan in Razor Pages
- EF Core gebruikt parameters (geen SQL-injectie), Razor encodeert uitvoer (geen XSS)
- Rate limiting op login (10 pogingen per minuut)
- Input wordt op de server gevalideerd

## Structuur

```
Data/        DbContext + testdata
Models/      AppUser, Activity, Participation, Announcement
Pages/       Razor Pages (.cshtml + .cshtml.cs)
Services/    AuthHelper (inloggen met cookie)
wwwroot/css  site.css
```
