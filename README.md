# KeelteKoolV2

Veebirakendus keelekooli õppejõudude ja õpilaste andmete haldamiseks. Projekt on arendatud kasutades C#, ASP.NET Core MVC-d ja .NET 10 platvormi.

## Kasutatud tehnoloogiad

| Tehnoloogia | Kasutus |
|---|---|
| C# | Programmeerimiskeel |
| ASP.NET Core MVC | Veebirakenduse arendamine |
| Entity Framework Core | Andmebaasiga töötamine |
| SQL Server | Andmete salvestamine |
| xUnit | Backend-testide kirjutamine |
| Selenium WebDriver | Veebiliidese automaattestimine |
| Git ja GitHub | Versioonihaldus ja meeskonnatöö |

## Tööjaotus

- **Lecturers** – õppejõudude haldamine.
- **Students** – õpilaste haldamine.

Projekt on arendatud ühises meeskonnarepositooriumis.

## Lecturers

Minu arendatud osa võimaldab hallata õppejõudude andmeid.

Rakendatud funktsioonid:

- Õppejõudude nimekirja vaatamine.
- Uue õppejõu lisamine.
- Õppejõu detailandmete vaatamine.
- Õppejõu andmete muutmine.
- Õppejõu kustutamine.

Arendamisel kasutati kontrollerit `LecturersController`, teenust `LecturersServices`, DTO-d `LecturerDTO`, domeenimudelit `Lecturer`, ViewModel-eid ja Razor-vaateid.

## Backend-testid

Backend-testid asuvad projektis `KeelteKoolV2.xUnitTesting` failis `LecturersServicesTests.cs` ja on kirjutatud xUnit-i abil testjuhitud arenduse põhimõttel (LanguageCourse testide eeskujul).

**Kokku: 14 õppejõu backend-testi** (5 põhitesti + 9 omal valikul testi). Igas testis on vähemalt 5 kontrolli (`Assert`).

Põhitestid (LanguageCourse eeskujul):

- Õppejõu loomine.
- Õppejõu loomise tagasilükkamine tühjade nimeväljade korral.
- Õppejõu leidmine ID alusel.
- Õppejõu andmete muutmine.
- Õppejõu kustutamine.

Omal valikul testid (enamasti negatiivsed):

- Olematu ID korral õppejõudu ei leita.
- Olematu ID korral õppejõudu ei kustutata.
- Olematu ID korral õppejõudu ei muudeta.
- `null` DTO korral õppejõudu ei looda.
- `null` DTO korral õppejõudu ei muudeta.
- Tühja GUID-i korral õppejõudu ei leita.
- `null` nimeväljade korral õppejõudu ei looda.
- Kasutaja ID ja kuupäevade salvestamine.
- Kahe õppejõu lisamisel saavad mõlemad erineva ID.

Kõikidele testidele on lisatud eestikeelsed XML-dokumentatsiooni `summary`-kommentaarid.

### Backend-testide käivitamine

```powershell
dotnet test .\KeelteKoolV2.xUnitTesting\KeelteKoolV2.xUnitTesting.csproj
```

<img width="640" height="180" alt="{9540A72D-B520-4DAE-8638-1A54AAE9AFF3}" src="https://github.com/user-attachments/assets/a8a5c38e-b608-4c4f-a2ea-5b208a7620d5" />


(14 lecturers testi ja 5 languagecourses testi, kokku 19)

## 🌐 Frontend-testid

Frontend-testid asuvad projektis `KeelteKoolV2.SeleniumTesting` failis `LecturerFrontendTests.cs` ja kasutavad Selenium WebDriverit ning xUnit-i.

**Kokku: 8 frontend-testi** (4 CRUD testi + 4 omal valikul testi).

| Test | Tüüp | Kontrollitav funktsioon |
|---|---|---|
| 1 | Create | Õppejõu loomine ja nimekirjas kuvamine |
| 2 | Read | Õppejõu detailvaate kontrollimine |
| 3 | Update | Õppejõu andmete muutmine |
| 4 | Delete | Õppejõu kustutamine |
| 5 | Oma test | Kustutamise tühistamine |
| 6 | Oma test | Olemasolevate andmete kontrollimine muutmislehel |
| 7 | Oma test | Tabeli veergude kontrollimine |
| 8 | Oma test | Loomisvormi väljade kontrollimine |

Kõikidele testidele on lisatud eestikeelsed XML-dokumentatsiooni `summary`-kommentaarid. Testide käigus kasutatakse Firefoxi brauserit.

### Frontend-testide ehitamine

```powershell
dotnet build .\KeelteKoolV2.SeleniumTesting\KeelteKoolV2.SeleniumTesting.csproj
```
<img width="995" height="101" alt="{300D9B72-E926-4EE6-ADFC-592FEFA70D41}" src="https://github.com/user-attachments/assets/5ec95ca0-5cc1-4ede-868a-336099ba744b" />

### Veebirakenduse käivitamine

Enne Selenium-testide käivitamist peab veebirakendus töötama aadressil `https://localhost:7191`:

```powershell
dotnet run --project .\KeelteKoolV2\KeelteKoolV2.csproj --launch-profile https
```

<img width="1578" height="272" alt="{41F776C7-0215-496B-9F54-7F8D135AD270}" src="https://github.com/user-attachments/assets/efb329e3-37e6-4f42-ba3b-a7e2812d318f" />


### Frontend-testide käivitamine

Teises terminalis (rakendus peab samal ajal töötama):

```powershell
dotnet test .\KeelteKoolV2.SeleniumTesting\KeelteKoolV2.SeleniumTesting.csproj --filter "FullyQualifiedName~LecturerFrontendTests"
```

Selenium-testide käivitamiseks peab veebirakendus olema kättesaadav ja Firefox WebDriver peab töötama.

## 📋 Kokkuvõte

Lecturers osa raames on loodud õppejõudude haldamise CRUD-funktsionaalsus ning koostatud backend- ja frontend-testid rakenduse töö kontrollimiseks.

Projekti arendamisel kasutati testjuhitud arenduse põhimõtteid, versioonihaldust ja automaattestimist.

---

**Autor:** Martin Rossakov TARpv24
