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

    }
}
