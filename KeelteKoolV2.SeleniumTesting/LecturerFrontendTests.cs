using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using Xunit;

namespace KeelteKoolV2.SeleniumTesting
{
    //Õppejõudude (Lecturers) kasutajaliidese testid, mis kontrollivad CRUD tegevusi brauseri kaudu.
    //Iga test loob endale unikaalse eesnimega õppejõu, et testid ei sõltuks üksteisest.
    public class LecturerFrontendTests
    {
        /// <summary>
        /// Testib uue õppejõu loomist kasutajaliidese kaudu
        /// ja kontrollib, et loodud õppejõud kuvatakse nimekirjas.
        /// </summary>
        [Fact]
        public void Should_CreateLecturer_AndDisplayItInIndex()
        {
            /* Ülesseade */
            IWebDriver driver = new FirefoxDriver(); //brauserit juhtiv ja seal tegutsev draiver
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5); //ootame elementide ilmumist kuni 5 sekundit

            //tekitame unikaalse id, et just enda loodud õppejõud nimekirjast üles leida
            string uniqueId = Guid.NewGuid().ToString("N");
            string firstName = $"Test{uniqueId}";
            string lastName = "Lecturer";
            string qualifications = "Master of Education";

            try
            {
                driver.Navigate().GoToUrl("https://localhost:7191/"); //navigeeritav aadress

                /* Tegevus */
                IWebElement lecturersLink =
                    driver.FindElement(By.LinkText("Õppejõud")); //otsime testitava osa lingi
                lecturersLink.Click(); //simuleeritud tegevus elemendiga, antud juhul klikk

                IWebElement createLink =
                    driver.FindElement(By.LinkText("Add lecturer")); //otsime loomise nupu
                createLink.Click(); //vajutame loomise nupule

                //sisestame andmed loomise vormi
                driver.FindElement(By.Name("FirstName"))
                    .SendKeys(firstName);

                driver.FindElement(By.Name("LastName"))
                    .SendKeys(lastName);

                driver.FindElement(By.Name("Qualifications"))
                    .SendKeys(qualifications);

                //vajutame loomise vaates salvestamise nupule
                driver.FindElement(
                    By.CssSelector("form button[type='submit']"))
                    .Click();

                /* Kontroll */
                //kontrollime, et pärast loomist suunati tagasi õppejõudude nimekirja
                Assert.Contains("/Lecturers", driver.Url);

                //kogume andmed: võtame kõik nimekirja tabeli read
                var rows = driver.FindElements(
                    By.CssSelector("table tbody tr"));

                //otsime realt, kus on sama eesnimi, perekonnanimi ja kvalifikatsioon, mis sisestati
                bool lecturerExists = rows.Any(row =>
                    row.Text.Contains(firstName) &&
                    row.Text.Contains(lastName) &&
                    row.Text.Contains(qualifications));

                //kontrollime, et loodud õppejõud on nimekirjas olemas
                Assert.True(
                    lecturerExists,
                    "Loodud õppejõudu ei leitud nimekirjast.");
            }
            finally
            {
                driver.Quit(); //sulgeme akna ka siis, kui test kukub läbi
            }
        }


        /// <summary>
        /// Testib õppejõu vaatamist ja kontrollib, et detailvaates
        /// kuvatakse sama õppejõu eesnimi, perekonnanimi ja kvalifikatsioon.
        /// </summary>
        [Fact]
        public void Should_OpenLecturerDetails_AndDisplayCorrectData()
        {
            /* Ülesseade */
            IWebDriver driver = new FirefoxDriver(); //brauserit juhtiv ja seal tegutsev draiver
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5); //ootame elementide ilmumist kuni 5 sekundit

            //tekitame unikaalse id, et just enda loodud õppejõud nimekirjast üles leida
            string uniqueId = Guid.NewGuid().ToString("N");
            string firstName = $"Read{uniqueId}";
            string lastName = "TestLecturer";
            string qualifications = "Bachelor of Science";

            try
            {
                driver.Navigate().GoToUrl("https://localhost:7191/"); //navigeeritav aadress

                /* Tegevus: avame õppejõudude nimekirja */
                driver.FindElement(By.LinkText("Õppejõud")).Click(); //avame õppejõudude lehe
                driver.FindElement(By.LinkText("Add lecturer")).Click(); //vajutame loomise nupule

                //sisestame andmed loomise vormi
                driver.FindElement(By.Name("FirstName")).SendKeys(firstName);
                driver.FindElement(By.Name("LastName")).SendKeys(lastName);
                driver.FindElement(By.Name("Qualifications")).SendKeys(qualifications);

                //salvestame õppejõu
                driver.FindElement(
                    By.CssSelector("form button[type='submit']")).Click();

                /* Leiame nimekirjast just enda loodud õppejõu */
                var rows = driver.FindElements(By.CssSelector("table tbody tr")); //kõik tabeli read

                //otsime rea, kus on meie sisestatud andmed
                var createdRow = rows.FirstOrDefault(row =>
                    row.Text.Contains(firstName) &&
                    row.Text.Contains(lastName) &&
                    row.Text.Contains(qualifications));

                //kontrollime, et loodud õppejõud leiti nimekirjast
                Assert.NotNull(createdRow);

                /* Avame sama õppejõu detailvaate */
                createdRow!.FindElement(By.LinkText("Details")).Click(); //vajutame selle rea "Details" lingile

                /* Kontroll: detailvaates on õiged andmed */
                //kogume andmed: võtame kogu detailvaate teksti
                string detailsText = driver.FindElement(By.TagName("body")).Text;

                Assert.Contains(firstName, detailsText); //detailvaates kuvatakse sama eesnimi
                Assert.Contains(lastName, detailsText); //detailvaates kuvatakse sama perekonnanimi
                Assert.Contains(qualifications, detailsText); //detailvaates kuvatakse sama kvalifikatsioon
                Assert.Contains("Back to list", detailsText); //detailvaates on nimekirja tagasimineku nupp
            }
            finally
            {
                driver.Quit(); //sulgeme akna ka siis, kui test kukub läbi
            }
        }


        /// <summary>
        /// Testib olemasoleva õppejõu andmete muutmist ja kontrollib,
        /// et nimekirjas kuvatakse uuendatud andmed.
        /// </summary>
        [Fact]
        public void Should_UpdateLecturer_AndDisplayUpdatedData()
        {
            /* Ülesseade */
            IWebDriver driver = new FirefoxDriver(); //brauserit juhtiv ja seal tegutsev draiver
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5); //ootame elementide ilmumist kuni 5 sekundit

            //esialgsed andmed, millega õppejõud luuakse
            string uniqueId = Guid.NewGuid().ToString("N");
            string firstName = $"Update{uniqueId}";
            string lastName = "OldLecturer";
            string qualifications = "Bachelor";

            //uued andmed, millega õppejõudu muudetakse
            string updatedFirstName = $"Updated{uniqueId}";
            string updatedLastName = "NewLecturer";
            string updatedQualifications = "Master of Science";

            try
            {
                driver.Navigate().GoToUrl("https://localhost:7191/"); //navigeeritav aadress

                /* Tegevus: loome õppejõu */
                driver.FindElement(By.LinkText("Õppejõud")).Click(); //avame õppejõudude lehe
                driver.FindElement(By.LinkText("Add lecturer")).Click(); //vajutame loomise nupule

                //sisestame esialgsed andmed
                driver.FindElement(By.Name("FirstName")).SendKeys(firstName);
                driver.FindElement(By.Name("LastName")).SendKeys(lastName);
                driver.FindElement(By.Name("Qualifications")).SendKeys(qualifications);

                //salvestame õppejõu
                driver.FindElement(
                    By.CssSelector("form button[type='submit']")).Click();

                /* Leiame loodud õppejõu */
                var rows = driver.FindElements(By.CssSelector("table tbody tr")); //kõik tabeli read

                //otsime rea, kus on meie sisestatud andmed
                var createdRow = rows.FirstOrDefault(row =>
                    row.Text.Contains(firstName) &&
                    row.Text.Contains(lastName) &&
                    row.Text.Contains(qualifications));

                //kontrollime, et loodud õppejõud leiti nimekirjast
                Assert.NotNull(createdRow);

                /* Avame muutmise lehe */
                createdRow!.FindElement(By.LinkText("Edit")).Click(); //vajutame selle rea "Edit" lingile

                /* Muudame andmeid */
                //puhastame eesnime välja ja sisestame uue eesnime
                var firstNameInput = driver.FindElement(By.Name("FirstName"));
                firstNameInput.Clear();
                firstNameInput.SendKeys(updatedFirstName);

                //puhastame perekonnanime välja ja sisestame uue perekonnanime
                var lastNameInput = driver.FindElement(By.Name("LastName"));
                lastNameInput.Clear();
                lastNameInput.SendKeys(updatedLastName);

                //puhastame kvalifikatsiooni välja ja sisestame uue kvalifikatsiooni
                var qualificationsInput =
                    driver.FindElement(By.Name("Qualifications"));
                qualificationsInput.Clear();
                qualificationsInput.SendKeys(updatedQualifications);

                //salvestame muudatused
                driver.FindElement(
                    By.CssSelector("form button[type='submit']")).Click();

                /* Kontroll: nimekirjas on uuendatud andmed */
                //kogume andmed: võtame pärast muutmist kõik tabeli read
                var updatedRows = driver.FindElements(
                    By.CssSelector("table tbody tr"));

                //otsime rea, kus on uued andmed
                bool lecturerWasUpdated = updatedRows.Any(row =>
                    row.Text.Contains(updatedFirstName) &&
                    row.Text.Contains(updatedLastName) &&
                    row.Text.Contains(updatedQualifications));

                //kontrollime, et nimekirjas kuvatakse uuendatud andmed
                Assert.True(
                    lecturerWasUpdated,
                    "Õppejõu uuendatud andmeid ei leitud nimekirjast.");

                //otsime, kas nimekirjas on veel vanade andmetega rida
                bool oldDataStillExists = updatedRows.Any(row =>
                    row.Text.Contains(firstName) &&
                    row.Text.Contains(lastName));

                //kontrollime, et vanu andmeid enam ei kuvata (andmed muudeti, mitte ei lisatud uut rida)
                Assert.False(
                    oldDataStillExists,
                    "Nimekirjas kuvatakse endiselt õppejõu vanu andmeid.");
            }
            finally
            {
                driver.Quit(); //sulgeme akna ka siis, kui test kukub läbi
            }
        }

        /// <summary>
        /// Testib õppejõu kustutamist ja kontrollib,
        /// et kustutatud õppejõudu ei kuvata enam nimekirjas.
        /// </summary>
        [Fact]
        public void Should_DeleteLecturer_AndRemoveItFromIndex()
        {
            /* Ülesseade */
            IWebDriver driver = new FirefoxDriver(); //brauserit juhtiv ja seal tegutsev draiver
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5); //ootame elementide ilmumist kuni 5 sekundit

            //tekitame unikaalse id, et just enda loodud õppejõud nimekirjast üles leida
            string uniqueId = Guid.NewGuid().ToString("N");
            string firstName = $"Delete{uniqueId}";
            string lastName = "DeleteLecturer";
            string qualifications = "Test Qualification";

            try
            {
                driver.Navigate().GoToUrl("https://localhost:7191/"); //navigeeritav aadress

                /* Tegevus: loome õppejõu */
                driver.FindElement(By.LinkText("Õppejõud")).Click(); //avame õppejõudude lehe
                driver.FindElement(By.LinkText("Add lecturer")).Click(); //vajutame loomise nupule

                //sisestame andmed loomise vormi
                driver.FindElement(By.Name("FirstName")).SendKeys(firstName);
                driver.FindElement(By.Name("LastName")).SendKeys(lastName);
                driver.FindElement(By.Name("Qualifications")).SendKeys(qualifications);

                //salvestame õppejõu
                driver.FindElement(
                    By.CssSelector("form button[type='submit']")).Click();

                /* Leiame loodud õppejõu */
                var rows = driver.FindElements(By.CssSelector("table tbody tr")); //kõik tabeli read

                //otsime rea, kus on meie sisestatud andmed
                var createdRow = rows.FirstOrDefault(row =>
                    row.Text.Contains(firstName) &&
                    row.Text.Contains(lastName) &&
                    row.Text.Contains(qualifications));

                //kontrollime, et loodud õppejõud leiti nimekirjast
                Assert.NotNull(createdRow);

                /* Avame kustutamise lehe */
                createdRow!.FindElement(By.LinkText("Delete")).Click(); //vajutame selle rea "Delete" lingile

                /* Kontrollime, et kustutamise lehel on õige õppejõud */
                //kogume andmed: võtame kogu kustutamise lehe teksti
                string deletePageText =
                    driver.FindElement(By.TagName("body")).Text;

                Assert.Contains(firstName, deletePageText); //kustutamise lehel on sama eesnimi
                Assert.Contains(lastName, deletePageText); //kustutamise lehel on sama perekonnanimi

                /* Kinnitame kustutamise */
                driver.FindElement(
                    By.CssSelector("form button[type='submit']")).Click(); //vajutame "Delete" nupule

                /* Kontroll: õppejõudu ei ole enam nimekirjas */
                //kogume andmed: võtame pärast kustutamist kõik tabeli read
                var remainingRows = driver.FindElements(
                    By.CssSelector("table tbody tr"));

                //otsime, kas kustutatud õppejõu rida on veel alles
                bool lecturerStillExists = remainingRows.Any(row =>
                    row.Text.Contains(firstName) &&
                    row.Text.Contains(lastName) &&
                    row.Text.Contains(qualifications));

                //kontrollime, et kustutatud õppejõudu nimekirjas enam ei ole
                Assert.False(
                    lecturerStillExists,
                    "Kustutatud õppejõud kuvatakse endiselt nimekirjas.");
            }
            finally
            {
                driver.Quit(); //sulgeme akna ka siis, kui test kukub läbi
            }
        }


        /// <summary>
        /// Testib õppejõu kustutamise tühistamist ja kontrollib,
        /// et õppejõud jääb pärast tühistamist nimekirja alles.
        /// </summary>
        [Fact]
        public void Should_CancelLecturerDeletion_AndKeepLecturerInIndex()
        {
            /* Ülesseade */
            IWebDriver driver = new FirefoxDriver(); //brauserit juhtiv ja seal tegutsev draiver
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5); //ootame elementide ilmumist kuni 5 sekundit

            //tekitame unikaalse id, et just enda loodud õppejõud nimekirjast üles leida
            string uniqueId = Guid.NewGuid().ToString("N");
            string firstName = $"Cancel{uniqueId}";
            string lastName = "CancelDelete";
            string qualifications = "Test Qualification";

            try
            {
                driver.Navigate().GoToUrl("https://localhost:7191/"); //navigeeritav aadress

                /* Tegevus: loome õppejõu */
                driver.FindElement(By.LinkText("Õppejõud")).Click(); //avame õppejõudude lehe
                driver.FindElement(By.LinkText("Add lecturer")).Click(); //vajutame loomise nupule

                //sisestame andmed loomise vormi
                driver.FindElement(By.Name("FirstName")).SendKeys(firstName);
                driver.FindElement(By.Name("LastName")).SendKeys(lastName);
                driver.FindElement(By.Name("Qualifications")).SendKeys(qualifications);

                //salvestame õppejõu
                driver.FindElement(
                    By.CssSelector("form button[type='submit']")).Click();

                /* Leiame loodud õppejõu */
                var rows = driver.FindElements(By.CssSelector("table tbody tr")); //kõik tabeli read

                //otsime rea, kus on meie sisestatud andmed
                var createdRow = rows.FirstOrDefault(row =>
                    row.Text.Contains(firstName) &&
                    row.Text.Contains(lastName) &&
                    row.Text.Contains(qualifications));

                //kontrollime, et loodud õppejõud leiti nimekirjast
                Assert.NotNull(createdRow);

                /* Avame kustutamise lehe ja tühistame tegevuse */
                createdRow!.FindElement(By.LinkText("Delete")).Click(); //vajutame selle rea "Delete" lingile
                driver.FindElement(By.LinkText("Cancel")).Click(); //kustutamise lehel vajutame "Cancel"

                /* Kontroll: õppejõud on endiselt nimekirjas */
                //kogume andmed: võtame pärast tühistamist kõik tabeli read
                var remainingRows = driver.FindElements(
                    By.CssSelector("table tbody tr"));

                //otsime, kas õppejõu rida on endiselt alles
                bool lecturerStillExists = remainingRows.Any(row =>
                    row.Text.Contains(firstName) &&
                    row.Text.Contains(lastName) &&
                    row.Text.Contains(qualifications));

                //kontrollime, et tühistamise järel ei kustutatud õppejõudu
                Assert.True(
                    lecturerStillExists,
                    "Õppejõud kadus nimekirjast pärast kustutamise tühistamist.");
            }
            finally
            {
                driver.Quit(); //sulgeme akna ka siis, kui test kukub läbi
            }
        }


        /// <summary>
        /// Testib õppejõu muutmislehe avamist ja kontrollib,
        /// et vormi väljad sisaldavad õppejõu praeguseid andmeid.
        /// </summary>
        [Fact]
        public void Should_OpenLecturerEditPage_WithCorrectExistingData()
        {
            /* Ülesseade */
            IWebDriver driver = new FirefoxDriver(); //brauserit juhtiv ja seal tegutsev draiver
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5); //ootame elementide ilmumist kuni 5 sekundit

            //tekitame unikaalse id, et just enda loodud õppejõud nimekirjast üles leida
            string uniqueId = Guid.NewGuid().ToString("N");
            string firstName = $"Edit{uniqueId}";
            string lastName = "ExistingLecturer";
            string qualifications = "Bachelor of Arts";

            try
            {
                driver.Navigate().GoToUrl("https://localhost:7191/"); //navigeeritav aadress

                /* Tegevus: loome õppejõu */
                driver.FindElement(By.LinkText("Õppejõud")).Click(); //avame õppejõudude lehe
                driver.FindElement(By.LinkText("Add lecturer")).Click(); //vajutame loomise nupule

                //sisestame andmed loomise vormi
                driver.FindElement(By.Name("FirstName")).SendKeys(firstName);
                driver.FindElement(By.Name("LastName")).SendKeys(lastName);
                driver.FindElement(By.Name("Qualifications")).SendKeys(qualifications);

                //salvestame õppejõu
                driver.FindElement(
                    By.CssSelector("form button[type='submit']")).Click();

                /* Leiame loodud õppejõu */
                var rows = driver.FindElements(By.CssSelector("table tbody tr")); //kõik tabeli read

                //otsime rea, kus on meie sisestatud andmed
                var createdRow = rows.FirstOrDefault(row =>
                    row.Text.Contains(firstName) &&
                    row.Text.Contains(lastName) &&
                    row.Text.Contains(qualifications));

                //kontrollime, et loodud õppejõud leiti nimekirjast
                Assert.NotNull(createdRow);

                /* Avame muutmislehe */
                createdRow!.FindElement(By.LinkText("Edit")).Click(); //vajutame selle rea "Edit" lingile

                /* Kontroll: väljad sisaldavad olemasolevaid andmeid */
                //eesnime väli on eeltäidetud õppejõu praeguse eesnimega
                Assert.Equal(
                    firstName,
                    driver.FindElement(By.Name("FirstName")).GetAttribute("value"));

                //perekonnanime väli on eeltäidetud õppejõu praeguse perekonnanimega
                Assert.Equal(
                    lastName,
                    driver.FindElement(By.Name("LastName")).GetAttribute("value"));

                //kvalifikatsiooni väli on eeltäidetud õppejõu praeguse kvalifikatsiooniga
                Assert.Equal(
                    qualifications,
                    driver.FindElement(By.Name("Qualifications")).GetAttribute("value"));
            }
            finally
            {
                driver.Quit(); //sulgeme akna ka siis, kui test kukub läbi
            }
        }


        /// <summary>
        /// Kontrollib, et õppejõudude nimekirjas kuvatakse kõik vajalikud tabeli veerud.
        /// </summary>
        [Fact]
        public void Should_DisplayAllExpectedColumns_InLecturerList()
        {
            /* Ülesseade */
            IWebDriver driver = new FirefoxDriver(); //brauserit juhtiv ja seal tegutsev draiver
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5); //ootame elementide ilmumist kuni 5 sekundit

            try
            {
                /* Tegevus: avame õppejõudude nimekirja */
                driver.Navigate().GoToUrl("https://localhost:7191/"); //navigeeritav aadress
                driver.FindElement(By.LinkText("Õppejõud")).Click(); //avame õppejõudude lehe

                /* Leiame tabeli päised */
                //kogume andmed: võtame kõikide päise lahtrite tekstid nimekirja
                var headers = driver
                    .FindElements(By.CssSelector("table thead th"))
                    .Select(header => header.Text.Trim())
                    .ToList();

                /* Kontroll: kõik vajalikud veerud on olemas */
                Assert.Contains("First name", headers); //eesnime veerg on olemas
                Assert.Contains("Last name", headers); //perekonnanime veerg on olemas
                Assert.Contains("Qualifications", headers); //kvalifikatsiooni veerg on olemas
                Assert.Contains("Created at", headers); //loomise aja veerg on olemas
                Assert.Contains("Modified at", headers); //muutmise aja veerg on olemas
                Assert.Contains("Actions", headers); //tegevuste (Details/Edit/Delete) veerg on olemas
            }
            finally
            {
                driver.Quit(); //sulgeme akna ka siis, kui test kukub läbi
            }
        }


        /// <summary>
        /// Kontrollib, et õppejõu loomise vormis kuvatakse kõik vajalikud väljad.
        /// </summary>
        [Fact]
        public void Should_DisplayAllFields_OnCreateLecturerPage()
        {
            /* Ülesseade */
            IWebDriver driver = new FirefoxDriver(); //brauserit juhtiv ja seal tegutsev draiver
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5); //ootame elementide ilmumist kuni 5 sekundit

            try
            {
                /* Avame õppejõu loomise lehe */
                driver.Navigate().GoToUrl("https://localhost:7191/"); //navigeeritav aadress
                driver.FindElement(By.LinkText("Õppejõud")).Click(); //avame õppejõudude lehe
                driver.FindElement(By.LinkText("Add lecturer")).Click(); //vajutame loomise nupule

                /* Kontrollime vormi välju */
                Assert.True(driver.FindElement(By.Name("FirstName")).Displayed); //eesnime väli on nähtav
                Assert.True(driver.FindElement(By.Name("LastName")).Displayed); //perekonnanime väli on nähtav
                Assert.True(driver.FindElement(By.Name("Qualifications")).Displayed); //kvalifikatsiooni väli on nähtav
            }
            finally
            {
                driver.Quit(); //sulgeme akna ka siis, kui test kukub läbi
            }
        }
    }
}