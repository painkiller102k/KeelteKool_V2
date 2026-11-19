using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace KeelteKoolV2.SeleniumTesting
{
    [TestCaseOrderer("KeelteKoolV2.SeleniumTesting.PriorityOrderer", "KeelteKoolV2.SeleniumTesting")]
    public class LecturerFrontendTests
    {
        public static string thisRunGuid { get; set; }

        /// <summary>
        /// Testib uue õppejõu loomist kasutajaliidese kaudu
        /// ja kontrollib, et loodud õppejõud kuvatakse nimekirjas.
        /// </summary>
        [Fact]
        [Order(1)]
        public void Should_CreateLecturer_AndDisplayItInIndex()
        {
            /* Ülesseade */
            IWebDriver driver = TestSetup_NavigateToIndex();

            /* tegevus */
            IWebElement indexCreateButton = driver.FindElement(By.LinkText("Add lecturer"));
            indexCreateButton.Click(); //click on create button

            string uniqueId = DataEntry_Lecturer(driver); //enter data in view
            thisRunGuid = uniqueId;

            IWebElement CreateCreateButton = driver.FindElement(By.CssSelector("form button[type='submit']"));
            CreateCreateButton.Click(); //click create button in create view
            Thread.Sleep(500);

            /* kontroll */

            //kogume andmed
            string currentUrl = driver.Url;
            var tableOfIndex = driver.FindElement(By.TagName("table"));
            List<IWebElement> rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);
            bool isObjectFound = false;
            string LecturerFirstNameData = "";
            string LecturerLastNameData = "";
            string LecturerQualificationsData = "";
            foreach (var row in rowsOfTable)
            {
                var cells = row.FindElements(By.TagName("td"));
                if (cells[0].Text == thisRunGuid)
                {
                    isObjectFound = true;
                    LecturerFirstNameData = cells[0].Text;
                    LecturerLastNameData = cells[1].Text;
                    LecturerQualificationsData = cells[2].Text;
                }
            }
            //sulgeme akna
            Thread.Sleep(500);
            driver.Quit();
            //assertime
            Assert.True(isObjectFound);
            Assert.Contains("/Lecturers", currentUrl);
            Assert.Equal(uniqueId, LecturerFirstNameData);
            Assert.Equal("Automaattestija", LecturerLastNameData);
            Assert.Equal("Tarkvaraarendaja", LecturerQualificationsData);
            Assert.True((uniqueId == LecturerFirstNameData) && ("Automaattestija" == LecturerLastNameData));
        }

        /// <summary>
        /// Testib õppejõu vaatamist ja kontrollib, et detailvaates
        /// kuvatakse sama õppejõu eesnimi, perekonnanimi ja kvalifikatsioon.
        /// </summary>
        [Fact]
        [Order(2)]
        public void Should_OpenLecturerDetails_AndDisplayCorrectData()
        {
            /* ülesseade */
            IWebDriver driver = TestSetup_NavigateToIndex();
            var tableOfIndex = driver.FindElement(By.TagName("table"));
            List<IWebElement> rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);
            bool isObjectFound = false;

            /* tegevus */
            foreach (var row in rowsOfTable)
            {
                var firstNameElement = row.FindElements(By.TagName("td"))[0].Text;
                if ((firstNameElement == thisRunGuid))
                {
                    isObjectFound = true;
                    var clickAction = row.FindElement(By.LinkText("Details"));
                    clickAction.Click();
                    break;
                }
            }
            Thread.Sleep(1000);
            //find data
            var detailsData = driver.FindElements(By.TagName("dd"));
            string details_firstName = detailsData[0].Text; //omab testimise jaoks vajalikku id-d
            string details_lastName = detailsData[1].Text;
            string details_qualifications = detailsData[2].Text;
            string details_createdAt = detailsData[3].Text;
            IWebElement goBack = driver.FindElement(By.LinkText("Back to list"));
            goBack.Click();
            Thread.Sleep(1000);
            driver.Quit();

            /* Kontroll */
            Assert.True(isObjectFound);
            Assert.NotEmpty(details_firstName);
            Assert.NotEmpty(details_lastName);
            Assert.NotEmpty(details_qualifications);
            Assert.NotEmpty(details_createdAt);
            Assert.Equal(thisRunGuid, details_firstName);
            Assert.Equal("Automaattestija", details_lastName);
            Assert.Equal("Tarkvaraarendaja", details_qualifications);
        }

        /// <summary>
        /// Testib olemasoleva õppejõu andmete muutmist ja kontrollib,
        /// et detailvaates kuvatakse uuendatud andmed.
        /// </summary>
        [Fact]
        [Order(3)]
        public void Should_UpdateLecturer_AndDisplayUpdatedData()
        {
            /* Ülesseade */
            IWebDriver driver = TestSetup_NavigateToIndex();
            var tableOfIndex = driver.FindElement(By.TagName("table"));
            List<IWebElement> rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);
            bool isObjectFound = false;

            /* tegevus */
            foreach (var row in rowsOfTable)
            {
                var firstNameElement = row.FindElements(By.TagName("td"))[0].Text;
                if ((firstNameElement == thisRunGuid))
                {
                    isObjectFound = true;
                    var clickAction = row.FindElement(By.LinkText("Edit"));
                    clickAction.Click();
                    break;
                }
            }
            Thread.Sleep(500);
            DataClear_CreateUpdate_SaveName(driver);
            DataEntry_Lecturer(driver, true, false);
            IWebElement updateclick = driver.FindElement(By.CssSelector("form button[type='submit']"));
            updateclick.Click();
            Thread.Sleep(1000);

            /* kontrolle */
            //otsi objekt üles:
            tableOfIndex = driver.FindElement(By.TagName("table"));
            rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);
            isObjectFound = false;
            foreach (var row in rowsOfTable)
            {
                var firstNameElement = row.FindElements(By.TagName("td"))[0].Text;
                if ((firstNameElement == thisRunGuid))
                {
                    isObjectFound = true;
                    var clickAction = row.FindElement(By.LinkText("Details"));
                    clickAction.Click();
                    break;
                }
            }
            Thread.Sleep(1000);
            //find data
            var detailsData = driver.FindElements(By.TagName("dd"));
            string details_firstName = detailsData[0].Text; //unikaalne id
            string details_lastName = detailsData[1].Text;
            string details_qualifications = detailsData[2].Text;
            Thread.Sleep(1000);
            driver.Quit();

            Assert.True(isObjectFound);
            Assert.NotEmpty(details_firstName);
            Assert.Equal(thisRunGuid, details_firstName);
            Assert.Equal("Analüütik", details_lastName);
            Assert.Equal("Testiinsener", details_qualifications);
            Assert.NotEqual("Automaattestija", details_lastName);
            Assert.NotEqual("Tarkvaraarendaja", details_qualifications);
        }

        /// <summary>
        /// Testib õppejõu kustutamist ja kontrollib,
        /// et kustutatud õppejõudu ei kuvata enam nimekirjas.
        /// </summary>
        [Fact]
        [Order(4)]
        public void Should_DeleteLecturer_AndRemoveItFromIndex()
        {
            /* ülesseade */
            IWebDriver driver = TestSetup_NavigateToIndex();
            var tableOfIndex = driver.FindElement(By.TagName("table"));
            List<IWebElement> rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);
            bool isObjectFound = false;

            /* tegevus */
            foreach (var row in rowsOfTable)
            {
                var firstNameElement = row.FindElements(By.TagName("td"))[0].Text;
                if ((firstNameElement == thisRunGuid))
                {
                    isObjectFound = true;
                    var clickAction = row.FindElement(By.LinkText("Delete"));
                    clickAction.Click();
                    break;
                }
            }
            Thread.Sleep(1000);
            string delete_firstName = driver.FindElements(By.TagName("dd"))[0].Text;
            IWebElement clickDelete = driver.FindElement(By.CssSelector("form button[type='submit']"));
            clickDelete.Click();
            Thread.Sleep(1000);

            //kontroll
            bool isObjectFoundAfterDelete = false;
            tableOfIndex = driver.FindElement(By.TagName("table"));
            rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);
            foreach (var row in rowsOfTable)
            {
                var firstNameElement = row.FindElements(By.TagName("td"))[0].Text;
                if ((firstNameElement == thisRunGuid))
                {
                    isObjectFoundAfterDelete = true;
                    break;
                }
            }
            driver.Quit();

            Assert.True(isObjectFound);
            Assert.Equal(thisRunGuid, delete_firstName);
            Assert.False(isObjectFoundAfterDelete);
            Assert.True(!isObjectFoundAfterDelete);
        }

        /// <summary>
        /// Testib õppejõu kustutamise tühistamist ja kontrollib,
        /// et õppejõud jääb pärast tühistamist nimekirja alles.
        /// </summary>
        [Fact]
        [Order(5)]
        public void Should_CancelLecturerDeletion_AndKeepLecturerInIndex()
        {
            /* Ülesseade */
            IWebDriver driver = TestSetup_NavigateToIndex();
            IWebElement indexCreateButton = driver.FindElement(By.LinkText("Add lecturer"));
            indexCreateButton.Click(); //click on create button
            string uniqueId = DataEntry_Lecturer(driver); //enter data in view
            IWebElement CreateCreateButton = driver.FindElement(By.CssSelector("form button[type='submit']"));
            CreateCreateButton.Click(); //click create button in create view
            Thread.Sleep(500);

            /* tegevus */
            var tableOfIndex = driver.FindElement(By.TagName("table"));
            List<IWebElement> rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);
            bool isObjectFound = false;
            foreach (var row in rowsOfTable)
            {
                var firstNameElement = row.FindElements(By.TagName("td"))[0].Text;
                if ((firstNameElement == uniqueId))
                {
                    isObjectFound = true;
                    var clickAction = row.FindElement(By.LinkText("Delete"));
                    clickAction.Click();
                    break;
                }
            }
            Thread.Sleep(1000);
            IWebElement clickCancel = driver.FindElement(By.LinkText("Cancel"));
            clickCancel.Click();
            Thread.Sleep(1000);

            //kontroll
            bool isObjectFoundAfterCancel = false;
            string LecturerLastNameData = "";
            tableOfIndex = driver.FindElement(By.TagName("table"));
            rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);
            foreach (var row in rowsOfTable)
            {
                var cells = row.FindElements(By.TagName("td"));
                if ((cells[0].Text == uniqueId))
                {
                    isObjectFoundAfterCancel = true;
                    LecturerLastNameData = cells[1].Text;
                    break;
                }
            }
            driver.Quit();

            Assert.True(isObjectFound);
            Assert.True(isObjectFoundAfterCancel);
            Assert.False(!isObjectFoundAfterCancel);
            Assert.Equal("Automaattestija", LecturerLastNameData);
        }

        /// <summary>
        /// Testib õppejõu muutmislehe avamist ja kontrollib,
        /// et vormi väljad sisaldavad õppejõu praeguseid andmeid.
        /// </summary>
        [Fact]
        [Order(6)]
        public void Should_OpenLecturerEditPage_WithCorrectExistingData()
        {
            /* Ülesseade */
            IWebDriver driver = TestSetup_NavigateToIndex();
            IWebElement indexCreateButton = driver.FindElement(By.LinkText("Add lecturer"));
            indexCreateButton.Click(); //click on create button
            string uniqueId = DataEntry_Lecturer(driver); //enter data in view
            IWebElement CreateCreateButton = driver.FindElement(By.CssSelector("form button[type='submit']"));
            CreateCreateButton.Click(); //click create button in create view
            Thread.Sleep(500);

            /* tegevus */
            var tableOfIndex = driver.FindElement(By.TagName("table"));
            List<IWebElement> rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);
            bool isObjectFound = false;
            foreach (var row in rowsOfTable)
            {
                var firstNameElement = row.FindElements(By.TagName("td"))[0].Text;
                if ((firstNameElement == uniqueId))
                {
                    isObjectFound = true;
                    var clickAction = row.FindElement(By.LinkText("Edit"));
                    clickAction.Click();
                    break;
                }
            }
            Thread.Sleep(1000);
            //find data
            string edit_firstName = driver.FindElement(By.Name("FirstName")).GetAttribute("value");
            string edit_lastName = driver.FindElement(By.Name("LastName")).GetAttribute("value");
            string edit_qualifications = driver.FindElement(By.Name("Qualifications")).GetAttribute("value");
            Thread.Sleep(500);
            driver.Quit();

            /* Kontroll */
            Assert.True(isObjectFound);
            Assert.NotEmpty(edit_firstName);
            Assert.Equal(uniqueId, edit_firstName);
            Assert.Equal("Automaattestija", edit_lastName);
            Assert.Equal("Tarkvaraarendaja", edit_qualifications);
        }

        /// <summary>
        /// Kontrollib, et õppejõudude nimekirjas kuvatakse kõik vajalikud tabeli veerud.
        /// </summary>
        [Fact]
        [Order(7)]
        public void Should_DisplayAllExpectedColumns_InLecturerList()
        {
            /* Ülesseade */
            IWebDriver driver = TestSetup_NavigateToIndex();

            /* tegevus */
            var tableOfIndex = driver.FindElement(By.TagName("table"));
            List<IWebElement> headersOfTable = tableOfIndex.FindElements(By.TagName("th")).ToList();
            List<string> headers = new List<string>();
            foreach (var header in headersOfTable)
            {
                headers.Add(header.Text.Trim());
            }
            //sulgeme akna
            Thread.Sleep(500);
            driver.Quit();

            /* kontroll */
            Assert.Contains("First name", headers);
            Assert.Contains("Last name", headers);
            Assert.Contains("Qualifications", headers);
            Assert.Contains("Created at", headers);
            Assert.Contains("Modified at", headers);
            Assert.Contains("Actions", headers);
        }

        /// <summary>
        /// Kontrollib, et õppejõu loomise vormis kuvatakse kõik vajalikud väljad.
        /// </summary>
        [Fact]
        [Order(8)]
        public void Should_DisplayAllFields_OnCreateLecturerPage()
        {
            /* Ülesseade */
            IWebDriver driver = TestSetup_NavigateToIndex();

            /* tegevus */
            IWebElement indexCreateButton = driver.FindElement(By.LinkText("Add lecturer"));
            indexCreateButton.Click(); //click on create button
            Thread.Sleep(500);

            //kogume andmed
            bool isFirstNameDisplayed = driver.FindElement(By.Name("FirstName")).Displayed;
            bool isLastNameDisplayed = driver.FindElement(By.Name("LastName")).Displayed;
            bool isQualificationsDisplayed = driver.FindElement(By.Name("Qualifications")).Displayed;
            bool isCreateButtonDisplayed = driver.FindElement(By.CssSelector("form button[type='submit']")).Displayed;
            //sulgeme akna
            Thread.Sleep(500);
            driver.Quit();

            /* kontroll */
            Assert.True(isFirstNameDisplayed);
            Assert.True(isLastNameDisplayed);
            Assert.True(isQualificationsDisplayed);
            Assert.True(isCreateButtonDisplayed);
            Assert.True(isFirstNameDisplayed && isLastNameDisplayed && isQualificationsDisplayed);
        }

        //eof

        /// <summary>
        /// Puhastab andmeväljad meetodile kaasaantud draiveri sessioonis
        /// </summary>
        /// <param name="driver">Hetkel käsitletav draiver</param>
        private void DataClear_CreateUpdate(IWebDriver driver)
        {
            IWebElement createEntryFirstName = driver.FindElement(By.Name("FirstName"));
            createEntryFirstName.Clear();
            IWebElement createEntryLastName = driver.FindElement(By.Name("LastName"));
            createEntryLastName.Clear();
            IWebElement createEntryQualifications = driver.FindElement(By.Name("Qualifications"));
            createEntryQualifications.Clear();
        }
        /// <summary>
        /// Puhastab andmeväljad meetodile kaasaantud draiveri sessioonis
        /// välja arvatud eesnime väli
        /// </summary>
        /// <param name="driver">Hetkel käsitletav draiver</param>
        private void DataClear_CreateUpdate_SaveName(IWebDriver driver)
        {
            IWebElement createEntryLastName = driver.FindElement(By.Name("LastName"));
            createEntryLastName.Clear();
            IWebElement createEntryQualifications = driver.FindElement(By.Name("Qualifications"));
            createEntryQualifications.Clear();
        }

        /// <summary>
        /// Meetod mis testimise eesmärgil sisestab lehel andmeid, on võimalik konfigureerida.
        /// </summary>
        /// <param name="driver">WebDriver mida hetkel test rakendab</param>
        /// <param name="isOneOrTwo">Muutuja mis valib kahe erineva andmekomplekti vahel</param>
        /// <param name="updateName">Muutuja mis ütleb kas eesnime on vaja muuta (eesnimes hoitakse testimiseks vajalikku unikaalset Guidi)</param>
        /// <returns>Tagastab lisatud Guidi stringi kujul mis pandi eesnime välja (Kui on konfigureeritud nii)</returns>
        private static string DataEntry_Lecturer(IWebDriver driver, bool isOneOrTwo = false, bool updateName = true)
        {
            Guid generatedId = Guid.NewGuid();
            if (updateName)
            {
                IWebElement createEntryFirstName = driver.FindElement(By.Name("FirstName"));
                createEntryFirstName.SendKeys(generatedId.ToString());
            }

            if (!isOneOrTwo)
            {
                IWebElement createEntryLastName = driver.FindElement(By.Name("LastName"));
                createEntryLastName.SendKeys("Automaattestija");
                IWebElement createEntryQualifications = driver.FindElement(By.Name("Qualifications"));
                createEntryQualifications.SendKeys("Tarkvaraarendaja");
            }
            else
            {
                IWebElement createEntryLastName = driver.FindElement(By.Name("LastName"));
                createEntryLastName.SendKeys("Analüütik");
                IWebElement createEntryQualifications = driver.FindElement(By.Name("Qualifications"));
                createEntryQualifications.SendKeys("Testiinsener");
            }
            return generatedId.ToString();
        }
        /// <summary>
        /// Ülesseade meetod mis tekitab draiveri, navigeerib aadressile ja vajutab Õppejõudude lingile.
        /// </summary>
        /// <returns>Tagastab testisiseselt kasutatava draiveri</returns>
        private static IWebDriver TestSetup_NavigateToIndex()
        {
            IWebDriver driver = new FirefoxDriver(); //brauserit juhtiv ja seal tegutsev draiver
            driver.Url = "https://localhost:7191/"; //navigeeritav aadress
            Thread.Sleep(3000);
            IWebElement navigateToLecturers = driver.FindElement(By.LinkText("Õppejõud"));//otsime tesitava osa lingi
            navigateToLecturers.Click(); //simuleeritud tegevus elemendiga, antud juhul klikk
            return driver;
        }
    }
}
