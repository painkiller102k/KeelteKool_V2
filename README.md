# KeelteKoolV2

Veebirakendus keelekooli õppejõudude ja õpilaste andmete haldamiseks. Projekt on arendatud kasutades C#, ASP.NET Core MVC-d ja .NET 10 platvormi.

## 🛠️ Kasutatud tehnoloogiad

| Tehnoloogia | Kasutus |
|---|---|
| C# | Programmeerimiskeel |
| ASP.NET Core MVC | Veebirakenduse arendamine |
| Entity Framework Core | Andmebaasiga töötamine |
| SQL Server | Andmete salvestamine |
| xUnit | Backend-testide kirjutamine |
| Selenium WebDriver | Veebiliidese automaattestimine |
| Git ja GitHub | Versioonihaldus ja meeskonnatöö |

## 👥 Tööjaotus

- **Lecturers** – õppejõudude haldamine.
- **Students** – õpilaste haldamine.

Projekt on arendatud ühises meeskonnarepositooriumis.

## 👨‍🏫 Lecturers

Minu arendatud osa võimaldab hallata õppejõudude andmeid.

Rakendatud funktsioonid:

- Õppejõudude nimekirja vaatamine.
- Uue õppejõu lisamine.
- Õppejõu detailandmete vaatamine.
- Õppejõu andmete muutmine.
- Õppejõu kustutamine.

Arendamisel kasutati kontrollerit `LecturersController`, teenuseid, DTO-d, ViewModel-eid ja Razor-vaateid.

## 🧪 Backend-testid

Backend-testid asuvad projektis `KeelteKoolV2.xUnitTesting` ja on kirjutatud xUnit-i abil.

Testidega kontrollitakse:

- Õppejõu loomist.
- Õppejõu leidmist ID alusel.
- Õppejõu andmete muutmist.
- Õppejõu kustutamist.
- Olematu ID käsitlemist.
- `null`- ja tühjade väärtuste käsitlemist.
- Andmete salvestamist ja andmebaasist lugemist.
- Kasutaja ID ning kuupäevade salvestamist.

**Kokku: 15 backend-testi.**

Kõikidele testidele on lisatud eestikeelsed XML-dokumentatsiooni `summary`-kommentaarid.

### Backend-testide käivitamine

```powershell
dotnet test .\KeelteKoolV2.xUnitTesting\KeelteKoolV2.xUnitTesting.csproj
```
<img width="572" height="65" alt="{82A566CB-EE2A-4FC1-A581-25CE46A9514B}" src="https://github.com/user-attachments/assets/0d5c72ff-a95f-491a-889f-73fc2adfe066" />
( 15 lecturers testid ja 5 languagecourses testid )

## 🌐 Frontend-testid

Frontend-testid asuvad projektis `KeelteKoolV2.SeleniumTesting` ja kasutavad Selenium WebDriverit ning xUnit-i.

**Kokku: 8 frontend-testi.**

| Test | Kontrollitav funktsioon |
|---|---|
| 1 | Õppejõu loomine ja nimekirjas kuvamine |
| 2 | Õppejõu detailvaate kontrollimine |
| 3 | Õppejõu andmete muutmine |
| 4 | Õppejõu kustutamine |
| 5 | Kustutamise tühistamine |
| 6 | Olemasolevate andmete kontrollimine muutmislehel |
| 7 | Tabeli veergude kontrollimine |
| 8 | Loomisvormi väljade kontrollimine |

Kõikidele testidele on lisatud eestikeelsed XML-dokumentatsiooni `summary`-kommentaarid. Testide käigus kasutatakse Firefoxi brauserit.

### Frontend-testide ehitamine

```powershell
dotnet build .\KeelteKoolV2.SeleniumTesting\KeelteKoolV2.SeleniumTesting.csproj
```
<img width="995" height="101" alt="{300D9B72-E926-4EE6-ADFC-592FEFA70D41}" src="https://github.com/user-attachments/assets/5ec95ca0-5cc1-4ede-868a-336099ba744b" />


### Frontend-testide käivitamine

```powershell
dotnet test .\KeelteKoolV2.SeleniumTesting\KeelteKoolV2.SeleniumTesting.csproj
```

Selenium-testide käivitamiseks peab veebirakendus olema kättesaadav ja Firefox WebDriver peab töötama.

## 📋 Kokkuvõte

Lecturers osa raames on loodud õppejõudude haldamise CRUD-funktsionaalsus ning koostatud backend- ja frontend-testid rakenduse töö kontrollimiseks.

Projekti arendamisel kasutati testjuhitud arenduse põhimõtteid, versioonihaldust ja automaattestimist.

---

**Projekt:** KeelteKoolV2  
**Arendusplatvorm:** .NET 10  
**Testimine:** xUnit ja Selenium WebDriver
**Autor:** Martin Rossakov TARpv24

