DentalFlow  
DentalFlow är ett modernt webbaserat bokningssystem för tandläkarkliniker, utvecklat som examensarbete inom Webbutveckling (WEBD24). Systemet är byggt för att ge en smidig upplevelse för både kunder och administratörer och visar fullstack-kompetens inom ASP.NET Core, databaser, autentisering och UI-design.

Funktioner:

Kundfunktioner
- Boka behandlingar via ett tydligt flerstegsflöde (behandling → datum → tid → bekräftelse)
- Se sina bokningar via “Mina sidor”
- Automatisk e-postbekräftelse efter genomförd bokning
- Mobilvänlig och responsiv design

Administrationsfunktioner
- Fullständig adminpanel
- CRUD-hantering av behandlingar
- Se och filtrera bokningar per dag
- Roller och rättigheter via ASP.NET Identity

 Säkerhet & autentisering
- Inloggning och registrering via Identity
- Roller: Admin och Customer
- Validering i hela bokningsflödet
- Separata adminfunktioner skyddade bakom rollbaserad autentisering

Admin-inloggning (för testning)
När systemet startas första gången seedas följande administratörskonto:

**E-post:** admin@dentalflow.se  
**Lösenord:** Admin123!

Detta konto används endast för demo och test.

Enhetstester
Projektet innehåller xUnit-tester för att säkerställa att bokningslogiken fungerar korrekt:

- Test av överlappande bokningar
- Test av tider som nuddar varandra
- Test av tom bokningslista
- Razor Page Model-test för Create (giltig/ogiltig inmatning)

Testerna använder mocking och InMemory-databas.

Tekniker & verktyg
- **Backend:** ASP.NET Core MVC & Razor Pages, C#
- **Databas:** SQL Server + Entity Framework Core
- **Frontend:** HTML, CSS, Bootstrap, JavaScript
- **Autentisering:** ASP.NET Identity
- **Testning:** xUnit
- **Planering:** Jira
- **Design:** Figma
- **Övrigt:** SMTP-bekräftelsemail

Installation & körning

1. Klona projektet:
```bash
git clone https://github.com/<ditt-användarnamn>/DentalFlow.git
cd DentalFlow
```

2. Kör migrationer:
```bash
dotnet ef database update
```

3. Starta applikationen:
```bash
dotnet run
```

Öppna sedan applikationen i webbläsaren:  
**https://localhost:7270**

 Projektets syfte
DentalFlow utvecklades för att visa kunskap i fullstack-utveckling genom att skapa ett verkligt fungerande bokningssystem med:

- Databasmodellering
- Bokningsflöde och affärslogik
- Autentisering och roller
- Enhetstestning
- Responsiv UI-design
- Planering och spårning via Jira



## ✨ Tack för att du besöker projektet!
DentalFlow är utvecklat med fokus på kvalitet, struktur och tydlighet – perfekt för portfölj och framtida arbetsgivare.
