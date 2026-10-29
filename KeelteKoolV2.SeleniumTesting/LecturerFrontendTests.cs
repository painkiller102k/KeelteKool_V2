using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using Xunit;

namespace KeelteKoolV2.SeleniumTesting
{
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
            IWebDriver driver = new FirefoxDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            string uniqueId = Guid.NewGuid().ToString("N");
            string firstName = $"Test{uniqueId}";
            string lastName = "Lecturer";
            string qualifications = "Master of Education";

            try
            {
                driver.Navigate().GoToUrl("https://localhost:7191/");

                /* Tegevus */
                IWebElement lecturersLink =
                    driver.FindElement(By.LinkText("Õppejõud"));
                lecturersLink.Click();

                IWebElement createLink =
                    driver.FindElement(By.LinkText("Add lecturer"));
                createLink.Click();

                driver.FindElement(By.Name("FirstName"))
                    .SendKeys(firstName);

                driver.FindElement(By.Name("LastName"))
                    .SendKeys(lastName);

                driver.FindElement(By.Name("Qualifications"))
                    .SendKeys(qualifications);

                driver.FindElement(
                    By.CssSelector("form button[type='submit']"))
                    .Click();

                /* Kontroll */
                Assert.Contains("/Lecturers", driver.Url);

                var rows = driver.FindElements(
                    By.CssSelector("table tbody tr"));

                bool lecturerExists = rows.Any(row =>
                    row.Text.Contains(firstName) &&
                    row.Text.Contains(lastName) &&
                    row.Text.Contains(qualifications));

                Assert.True(
                    lecturerExists,
                    "Loodud õppejõudu ei leitud nimekirjast.");
            }
            finally
            {
                driver.Quit();
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
            IWebDriver driver = new FirefoxDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            string uniqueId = Guid.NewGuid().ToString("N");
            string firstName = $"Read{uniqueId}";
            string lastName = "TestLecturer";
            string qualifications = "Bachelor of Science";

            try
            {
                driver.Navigate().GoToUrl("https://localhost:7191/");

                /* Tegevus: avame õppejõudude nimekirja */
                driver.FindElement(By.LinkText("Õppejõud")).Click();
                driver.FindElement(By.LinkText("Add lecturer")).Click();

                driver.FindElement(By.Name("FirstName")).SendKeys(firstName);
                driver.FindElement(By.Name("LastName")).SendKeys(lastName);
                driver.FindElement(By.Name("Qualifications")).SendKeys(qualifications);

                driver.FindElement(
                    By.CssSelector("form button[type='submit']")).Click();

                /* Leiame nimekirjast just enda loodud õppejõu */
                var rows = driver.FindElements(By.CssSelector("table tbody tr"));

                var createdRow = rows.FirstOrDefault(row =>
                    row.Text.Contains(firstName) &&
                    row.Text.Contains(lastName) &&
                    row.Text.Contains(qualifications));

                Assert.NotNull(createdRow);

                /* Avame sama õppejõu detailvaate */
                createdRow!.FindElement(By.LinkText("Details")).Click();

                /* Kontroll: detailvaates on õiged andmed */
                string detailsText = driver.FindElement(By.TagName("body")).Text;

                Assert.Contains(firstName, detailsText);
                Assert.Contains(lastName, detailsText);
                Assert.Contains(qualifications, detailsText);
                Assert.Contains("Back to list", detailsText);
            }
            finally
            {
                driver.Quit();
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
            IWebDriver driver = new FirefoxDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            string uniqueId = Guid.NewGuid().ToString("N");
            string firstName = $"Update{uniqueId}";
            string lastName = "OldLecturer";
            string qualifications = "Bachelor";

            string updatedFirstName = $"Updated{uniqueId}";
            string updatedLastName = "NewLecturer";
            string updatedQualifications = "Master of Science";

            try
            {
                driver.Navigate().GoToUrl("https://localhost:7191/");

                /* Tegevus: loome õppejõu */
                driver.FindElement(By.LinkText("Õppejõud")).Click();
                driver.FindElement(By.LinkText("Add lecturer")).Click();

                driver.FindElement(By.Name("FirstName")).SendKeys(firstName);
                driver.FindElement(By.Name("LastName")).SendKeys(lastName);
                driver.FindElement(By.Name("Qualifications")).SendKeys(qualifications);

                driver.FindElement(
                    By.CssSelector("form button[type='submit']")).Click();

                /* Leiame loodud õppejõu */
                var rows = driver.FindElements(By.CssSelector("table tbody tr"));

                var createdRow = rows.FirstOrDefault(row =>
                    row.Text.Contains(firstName) &&
                    row.Text.Contains(lastName) &&
                    row.Text.Contains(qualifications));

                Assert.NotNull(createdRow);

                /* Avame muutmise lehe */
                createdRow!.FindElement(By.LinkText("Edit")).Click();

                /* Muudame andmeid */
                var firstNameInput = driver.FindElement(By.Name("FirstName"));
                firstNameInput.Clear();
                firstNameInput.SendKeys(updatedFirstName);

                var lastNameInput = driver.FindElement(By.Name("LastName"));
                lastNameInput.Clear();
                lastNameInput.SendKeys(updatedLastName);

                var qualificationsInput =
                    driver.FindElement(By.Name("Qualifications"));
                qualificationsInput.Clear();
                qualificationsInput.SendKeys(updatedQualifications);

                driver.FindElement(
                    By.CssSelector("form button[type='submit']")).Click();

                /* Kontroll: nimekirjas on uuendatud andmed */
                var updatedRows = driver.FindElements(
                    By.CssSelector("table tbody tr"));

                bool lecturerWasUpdated = updatedRows.Any(row =>
                    row.Text.Contains(updatedFirstName) &&
                    row.Text.Contains(updatedLastName) &&
                    row.Text.Contains(updatedQualifications));

                Assert.True(
                    lecturerWasUpdated,
                    "Õppejõu uuendatud andmeid ei leitud nimekirjast.");

                bool oldDataStillExists = updatedRows.Any(row =>
                    row.Text.Contains(firstName) &&
                    row.Text.Contains(lastName));

                Assert.False(
                    oldDataStillExists,
                    "Nimekirjas kuvatakse endiselt õppejõu vanu andmeid.");
            }
            finally
            {
                driver.Quit();
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
            IWebDriver driver = new FirefoxDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            string uniqueId = Guid.NewGuid().ToString("N");
            string firstName = $"Delete{uniqueId}";
            string lastName = "DeleteLecturer";
            string qualifications = "Test Qualification";

            try
            {
                driver.Navigate().GoToUrl("https://localhost:7191/");

                /* Tegevus: loome õppejõu */
                driver.FindElement(By.LinkText("Õppejõud")).Click();
                driver.FindElement(By.LinkText("Add lecturer")).Click();

                driver.FindElement(By.Name("FirstName")).SendKeys(firstName);
                driver.FindElement(By.Name("LastName")).SendKeys(lastName);
                driver.FindElement(By.Name("Qualifications")).SendKeys(qualifications);

                driver.FindElement(
                    By.CssSelector("form button[type='submit']")).Click();

                /* Leiame loodud õppejõu */
                var rows = driver.FindElements(By.CssSelector("table tbody tr"));

                var createdRow = rows.FirstOrDefault(row =>
                    row.Text.Contains(firstName) &&
                    row.Text.Contains(lastName) &&
                    row.Text.Contains(qualifications));

                Assert.NotNull(createdRow);

                /* Avame kustutamise lehe */
                createdRow!.FindElement(By.LinkText("Delete")).Click();

                /* Kontrollime, et kustutamise lehel on õige õppejõud */
                string deletePageText =
                    driver.FindElement(By.TagName("body")).Text;

                Assert.Contains(firstName, deletePageText);
                Assert.Contains(lastName, deletePageText);

                /* Kinnitame kustutamise */
                driver.FindElement(
                    By.CssSelector("form button[type='submit']")).Click();

                /* Kontroll: õppejõudu ei ole enam nimekirjas */
                var remainingRows = driver.FindElements(
                    By.CssSelector("table tbody tr"));

                bool lecturerStillExists = remainingRows.Any(row =>
                    row.Text.Contains(firstName) &&
                    row.Text.Contains(lastName) &&
                    row.Text.Contains(qualifications));

                Assert.False(
                    lecturerStillExists,
                    "Kustutatud õppejõud kuvatakse endiselt nimekirjas.");
            }
            finally
            {
                driver.Quit();
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
            IWebDriver driver = new FirefoxDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            string uniqueId = Guid.NewGuid().ToString("N");
            string firstName = $"Cancel{uniqueId}";
            string lastName = "CancelDelete";
            string qualifications = "Test Qualification";

            try
            {
                driver.Navigate().GoToUrl("https://localhost:7191/");

                /* Tegevus: loome õppejõu */
                driver.FindElement(By.LinkText("Õppejõud")).Click();
                driver.FindElement(By.LinkText("Add lecturer")).Click();

                driver.FindElement(By.Name("FirstName")).SendKeys(firstName);
                driver.FindElement(By.Name("LastName")).SendKeys(lastName);
                driver.FindElement(By.Name("Qualifications")).SendKeys(qualifications);

                driver.FindElement(
                    By.CssSelector("form button[type='submit']")).Click();

                /* Leiame loodud õppejõu */
                var rows = driver.FindElements(By.CssSelector("table tbody tr"));

                var createdRow = rows.FirstOrDefault(row =>
                    row.Text.Contains(firstName) &&
                    row.Text.Contains(lastName) &&
                    row.Text.Contains(qualifications));

                Assert.NotNull(createdRow);

                /* Avame kustutamise lehe ja tühistame tegevuse */
                createdRow!.FindElement(By.LinkText("Delete")).Click();
                driver.FindElement(By.LinkText("Cancel")).Click();

                /* Kontroll: õppejõud on endiselt nimekirjas */
                var remainingRows = driver.FindElements(
                    By.CssSelector("table tbody tr"));

                bool lecturerStillExists = remainingRows.Any(row =>
                    row.Text.Contains(firstName) &&
                    row.Text.Contains(lastName) &&
                    row.Text.Contains(qualifications));

                Assert.True(
                    lecturerStillExists,
                    "Õppejõud kadus nimekirjast pärast kustutamise tühistamist.");
            }
            finally
            {
                driver.Quit();
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
            IWebDriver driver = new FirefoxDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            string uniqueId = Guid.NewGuid().ToString("N");
            string firstName = $"Edit{uniqueId}";
            string lastName = "ExistingLecturer";
            string qualifications = "Bachelor of Arts";

            try
            {
                driver.Navigate().GoToUrl("https://localhost:7191/");

                /* Tegevus: loome õppejõu */
                driver.FindElement(By.LinkText("Õppejõud")).Click();
                driver.FindElement(By.LinkText("Add lecturer")).Click();

                driver.FindElement(By.Name("FirstName")).SendKeys(firstName);
                driver.FindElement(By.Name("LastName")).SendKeys(lastName);
                driver.FindElement(By.Name("Qualifications")).SendKeys(qualifications);

                driver.FindElement(
                    By.CssSelector("form button[type='submit']")).Click();

                /* Leiame loodud õppejõu */
                var rows = driver.FindElements(By.CssSelector("table tbody tr"));

                var createdRow = rows.FirstOrDefault(row =>
                    row.Text.Contains(firstName) &&
                    row.Text.Contains(lastName) &&
                    row.Text.Contains(qualifications));

                Assert.NotNull(createdRow);

                /* Avame muutmislehe */
                createdRow!.FindElement(By.LinkText("Edit")).Click();

                /* Kontroll: väljad sisaldavad olemasolevaid andmeid */
                Assert.Equal(
                    firstName,
                    driver.FindElement(By.Name("FirstName")).GetAttribute("value"));

                Assert.Equal(
                    lastName,
                    driver.FindElement(By.Name("LastName")).GetAttribute("value"));

                Assert.Equal(
                    qualifications,
                    driver.FindElement(By.Name("Qualifications")).GetAttribute("value"));
            }
            finally
            {
                driver.Quit();
            }
        }


        /// <summary>
        /// Kontrollib, et õppejõudude nimekirjas kuvatakse kõik vajalikud tabeli veerud.
        /// </summary>
        [Fact]
        public void Should_DisplayAllExpectedColumns_InLecturerList()
        {
            /* Ülesseade */
            IWebDriver driver = new FirefoxDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            try
            {
                /* Tegevus: avame õppejõudude nimekirja */
                driver.Navigate().GoToUrl("https://localhost:7191/");
                driver.FindElement(By.LinkText("Õppejõud")).Click();

                /* Leiame tabeli päised */
                var headers = driver
                    .FindElements(By.CssSelector("table thead th"))
                    .Select(header => header.Text.Trim())
                    .ToList();

                /* Kontroll: kõik vajalikud veerud on olemas */
                Assert.Contains("First name", headers);
                Assert.Contains("Last name", headers);
                Assert.Contains("Qualifications", headers);
                Assert.Contains("Created at", headers);
                Assert.Contains("Modified at", headers);
                Assert.Contains("Actions", headers);
            }
            finally
            {
                driver.Quit();
            }
        }


    }
}
