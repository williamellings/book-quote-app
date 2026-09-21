# Bok- & Citatportal (CRUD med Tokenhantering)

## 📌 Funktionalitet & Uppfyllda krav

### 1. Bokhantering (CRUD)
- **Startsida (`/books`):** Visar alla tillgängliga böcker i ett responsivt rutnät med titel, författare, utgivningsår, genre och beskrivning.
- **Sökning:** Möjlighet att filtrera böcker i realtid via sökrutan.
- **Skapa ny bok (`/books/new`):** Klicka på knappen "Lägg till ny bok" för att komma till ett validerat formulär. Efter att formuläret skickats in omdirigeras man automatiskt tillbaka till startsidan och boken visas i listan.
- **Redigera bok (`/books/edit/:id`):** Varje bok har en "Redigera"-knapp som öppnar formuläret förifyllt med bokens uppgifter. Vid sparande uppdateras informationen och man skickas tillbaka till startsidan.
- **Radera bok:** Varje bok har en "Radera"-knapp som öppnar en bekräftelsedialog. När man bekräftar tas boken bort ur databasen och listan uppdateras omedelbart.

### 2. Tokenhantering & Säkerhet (JWT)
- **Registrering (`/register`):** Nya användare kan registrera konto med validering av lösenord och e-post.
- **Inloggning (`/login`):** Användaren loggar in med sina uppgifter, varpå API:et genererar en signerad JWT-token som skickas tillbaka och sparas i `localStorage`.
- **Skyddade anrop:** En HTTP Interceptor (`auth.interceptor.ts`) skickar med `Authorization: Bearer <token>` på alla förfrågningar.
- **Backend-validering:** Samtliga CRUD-endpoints är skyddade med `[Authorize]` på API-nivå. Obehöriga förfrågningar nekas med `401 Unauthorized`.
- **Route Guards:** Oinloggade besökare omdirigeras automatiskt till `/login`.

### 3. "Mina citat" (`/quotes`)
- Egen separat vy för att hantera citat.
- Kommer förladdad med 5 favoritcitat (Cicero, Walt Disney, George R.R. Martin, Theodore Roosevelt och Kofi Annan).
- Full CRUD: Man kan lägga till nya citat via modal, redigera befintliga och radera citat.
- Filtrering per kategori samt snabbknapp för att kopiera citat till urklipp.
- Menylänkar i navbaren gör att man enkelt kan hoppa fram och tillbaka mellan böcker och citat.

### 4. Responsiv Design & Styling
- Byggt med **Bootstrap 5** och **Font Awesome**-ikoner.
- Layouten anpassar sig för stationära skärmar, surfplattor och mobiler.
- Navigationsmenyn fälls automatiskt ihop till en responsiv mobilmeny (hamburgermeny) på mindre skärmar.

### 5. Extra utmaning: Ljust / Mörkt tema
- En knapp i navbaren (sol/måne) gör det möjligt att växla mellan ljust och mörkt läge för hela applikationen.
- Temapreferensen sparas i `localStorage` så att den finns kvar när sidan laddas om.

---

## 🛠 Teknikstack

- **Frontend:** Angular 20, TypeScript, Bootstrap 5, Font Awesome 6, Reactive Forms, RxJS / Signals
- **Backend:** .NET 9 (C#), ASP.NET Core Web API, Entity Framework Core
- **Databas:** SQLite (`bookquote.db` – skapas och seedas automatiskt vid start)
- **Autentisering:** JWT (JSON Web Tokens) med HMAC SHA-256 och BCrypt för lösenordshashning

---

## 💻 Köra projektet lokalt

### 1. Starta backend (API)
Gå till mappen `backend` och starta API:et:
```bash
cd backend
dotnet run
```
API:et körs på `http://localhost:5000`.  
Swagger-dokumentation finns på `http://localhost:5000/swagger`.

> Databasen skapas automatiskt vid första körningen och lägger in standarddata (böcker, citat och ett demokonto).

### 2. Starta frontend (Angular)
Öppna en ny terminal, gå till mappen `frontend` och starta:
```bash
cd frontend
npm install
npm start
```
Webbapplikationen nås på: `http://localhost:4200`

### Demoinloggning
Det finns ett förskapat testkonto för snabb inloggning:
- **Användarnamn:** `demo`
- **Lösenord:** `Demo123!`  
*(Finns även en snabbknapp på inloggningssidan som fyller i detta automatiskt)*
Man kan även klicka på **"Registrera dig här"** för att skapa ett helt nytt eget konto.
