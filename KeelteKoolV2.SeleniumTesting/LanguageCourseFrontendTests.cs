using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace KeelteKoolV2.SeleniumTesting
{
    //järjesti klass, mis omab omadusi mille järgi teste saab järjestada, väärtusi saab omistada ainult läbi konstruktori argumentide.
    public class OrderAttribute : Attribute
    {
        public int Value { get; } //väärtus
        public OrderAttribute(int value) //omistuskonstruktor
        {
            Value = value;
        }
    }
    //xUnit järjesti mis implementeerib ITestCaseOrdereri liidesest "ORderTestCases" meetodi, ning see meetod on see, mida me kasutame
    //testide käivitamisel nende testide õigesti järjestamiseks. Järjestatakse vastavalt OrderAttribute omadusele
    public class PriorityOrderer : ITestCaseOrderer
    {
        //meetod võtab sisse kõik test cased
        public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases) where TTestCase : ITestCase
        {
            //toimub sorteerimistegevus kus järjestatakse arendaja kirjutatud järjesti järgi.
            var result = testCases.OrderBy(test =>
            {
                //otsitakse järjesti omadusena
                var method = test.TestMethod.Method.GetCustomAttributes(typeof(OrderAttribute)).FirstOrDefault();
                //kui ei leita
                if (method == null)
                {
                    //antakse tagasi maksimaalne täisarv, ehk test teostatakse kohe, ega ei panda teiste potensiaalselt ÕIGES
                    //järjekorras olevate testide vahele
                    return int.MaxValue;
                }
                //kui leitakse
                else
                {
                    //siis tagastatakse selle järjesti omaduse väärtus
                    return method.GetNamedArgument<int>(nameof(OrderAttribute.Value));
                }
            }).ToList(); //testid asetatakse nimekirja

            return result; //sorteeritud tulemus tagastatakse
        }
    }

    [TestCaseOrderer("KeelteKoolV2.SeleniumTesting.PriorityOrderer", "KeelteKoolV2.SeleniumTesting")]
    public class LanguageCourseFrontendTests
    {
        [Fact]
        [Order(1)]
        public void Should_NavigateToIndex_AddLanguageCourseWithCorrectData_ReturnToIndex()
        {
            /* Ülesseade */
            IWebDriver driver = TestSetup_NavigateToIndex();

            /* tegevus */
            IWebElement indexCreateButton = driver.FindElement(By.Id("IndexCreateNewLanguageCourse_BTN"));
            indexCreateButton.Click(); //click on create button

            DataEntry_LanguageCourse(driver); //enter data in view

            IWebElement CreateCreateButton = driver.FindElement(By.Id("CU_Create_BTN"));
            CreateCreateButton.Click(); //click create button in create view

            /* kontroll */

            //kogume andmed
            string LanguageCourseNameData = driver.FindElement(By.Id("IndexTableCourseNimetus")).Text;
            string LanguageCourseKeelData = driver.FindElement(By.Id("IndexTableCourseKeel")).Text;
            //sulgeme akna
            Thread.Sleep(500);
            driver.Quit();
            //assertime
            Assert.Equal("Automaattestimise kursus", LanguageCourseNameData);
            Assert.Equal("C# + Selenium & xUnit", LanguageCourseKeelData);
            Assert.True(("Automaattestimise kursus" == LanguageCourseNameData) && ("C# + Selenium & xUnit" == LanguageCourseKeelData));
        }
        // siia vaja details (READ) test
        [Fact]
        [Order(2)]
        public void Should_NavigateToDetailsOfALanguageCourse_WithPreviouslyCorrectData()
        {
            /* ülesseade */
            IWebDriver driver = TestSetup_NavigateToIndex();
            var tableOfIndex = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);
            bool isObjectFound = false;

            /* tegevus */
            foreach (var row in rowsOfTable)
            {
                var nameElement = row.FindElement(By.Id("IndexTableCourseNimetus")).Text;
                var keelElement = row.FindElement(By.Id("IndexTableCourseKeel")).Text;
                if ((nameElement == "Automaattestimise kursus") && (keelElement == "C# + Selenium & xUnit"))
                {
                    isObjectFound = true;
                    var clickAction = row
                        .FindElement(By.Id("IndexTableCourseActions"))
                        .FindElement(By.Id("IndexTableCourseDetails_BTN"));
                    clickAction.Click();
                    break;
                }
            }
            Thread.Sleep(1000);
            //find data
            string details_id = driver.FindElement(By.Id("DD_LanguageCourseGuid")).Text;
            string details_name = driver.FindElement(By.Id("DD_LanguageCourseNimetus")).Text;
            string details_keel = driver.FindElement(By.Id("DD_LanguageCourseKeel")).Text;
            string details_kirjeldus = driver.FindElement(By.Id("DD_LanguageCourseKirjeldus")).Text;
            string details_tase = driver.FindElement(By.Id("DD_LanguageCourseTase")).Text;
            IWebElement goBack = driver.FindElement(By.Id("DetailsBack_BTN"));
            goBack.Click();
            Thread.Sleep(1000);
            driver.Quit();

            /* Kontroll */
            Assert.True(isObjectFound);
            Assert.NotEmpty(details_id);
            Assert.NotEmpty(details_name);
            Assert.NotEmpty(details_keel);
            Assert.NotEmpty(details_kirjeldus);
            Assert.NotEmpty(details_tase);
            Assert.Equal("Automaattestimise kursus", details_name);
            Assert.Equal("C# + Selenium & xUnit", details_keel);
        }
        //vaja teha uuenduse test (UPDATE)
        [Fact]
        [Order(3)]
        public void Should_UpdateDetailsOfLanguageCourse_WithNewData()
        {
            /* Ülesseade */
            IWebDriver driver = TestSetup_NavigateToIndex();
            var tableOfIndex = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);
            bool isObjectFound = false;

            /* tegevus */
            foreach (var row in rowsOfTable)
            {
                var nameElement = row.FindElement(By.Id("IndexTableCourseNimetus")).Text;
                var keelElement = row.FindElement(By.Id("IndexTableCourseKeel")).Text;
                if ((nameElement == "Automaattestimise kursus") && (keelElement == "C# + Selenium & xUnit"))
                {
                    isObjectFound = true;
                    var clickAction = row
                        .FindElement(By.Id("IndexTableCourseActions"))
                        .FindElement(By.Id("IndexTableCourseUpdate_BTN"));
                    clickAction.Click();
                    break;
                }
            }
            Thread.Sleep(500);
            DataClear_CreateUpdate(driver);
            DataEntry_LanguageCourse(driver, true);
            IWebElement updateclick = driver.FindElement(By.Id("CU_Update_BTN"));
            updateclick.Click();
            IWebElement goBack = driver.FindElement(By.Id("CU_Cancel_BTN"));
            goBack.Click();

            /* kontrolle */
            //otsi objekt üles:
            tableOfIndex = driver.FindElement(By.Id("IndexTable"));
            rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);
            isObjectFound = false;
            Thread.Sleep(1000);
            foreach (var row in rowsOfTable)
            {
                var nameElement = row.FindElement(By.Id("IndexTableCourseNimetus")).Text;
                var keelElement = row.FindElement(By.Id("IndexTableCourseKeel")).Text;
                if ((nameElement == "Manuaaltestimise kursus") && (keelElement == "Mantis, Excel"))
                {
                    isObjectFound = true;
                    var clickAction = row
                        .FindElement(By.Id("IndexTableCourseActions"))
                        .FindElement(By.Id("IndexTableCourseDetails_BTN"));
                    clickAction.Click();
                    break;
                }
            }
            //find data
            string details_id = driver.FindElement(By.Id("DD_LanguageCourseGuid")).Text;
            string details_name = driver.FindElement(By.Id("DD_LanguageCourseNimetus")).Text;
            string details_keel = driver.FindElement(By.Id("DD_LanguageCourseKeel")).Text;
            string details_kirjeldus = driver.FindElement(By.Id("DD_LanguageCourseKirjeldus")).Text;
            string details_tase = driver.FindElement(By.Id("DD_LanguageCourseTase")).Text;
            Thread.Sleep(1000);
            driver.Quit();

            Assert.True(isObjectFound);
            Assert.NotNull(details_id);
            Assert.NotEmpty(details_id);
            Assert.NotEqual("Automaattestimise kursus", details_name);
            Assert.NotEqual("C# + Selenium & xUnit", details_keel);
            Assert.NotEqual("3", details_kirjeldus);
            Assert.NotEqual("Kursusel õpime mis on automaattestimine, kuidas erineb manuaaltestimisest ning kuidas kirjutada " +
                    "backendi ja frontendi automaatteste.", details_tase);
        }
        //Delete test
        [Fact]
        [Order(4)]
        public void Should_DeleteLanguageCourse_FromDeleteView()
        {
            /* ülesseade */
            IWebDriver driver = TestSetup_NavigateToIndex();
            var tableOfIndex = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);
            bool isObjectFound = false;

            /* tegevus */
            foreach (var row in rowsOfTable)
            {
                var nameElement = row.FindElement(By.Id("IndexTableCourseNimetus")).Text;
                var keelElement = row.FindElement(By.Id("IndexTableCourseKeel")).Text;
                if ((nameElement == "Manuaaltestimise kursus") && (keelElement == "Mantis, Excel"))
                {
                    isObjectFound = true;
                    var clickAction = row
                        .FindElement(By.Id("IndexTableCourseActions"))
                        .FindElement(By.Id("IndexTableCourseDelete_BTN"));
                    clickAction.Click();
                    break;
                }
            }
            Thread.Sleep(1000);
            IWebElement clickDelete = driver.FindElement(By.Id("DeleteDelete_BTN"));
            clickDelete.Click();

            //kontroll
            isObjectFound = false;
            tableOfIndex = driver.FindElement(By.Id("IndexTable"));
            rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);
            foreach (var row in rowsOfTable)
            {
                var nameElement = row.FindElement(By.Id("IndexTableCourseNimetus")).Text;
                var keelElement = row.FindElement(By.Id("IndexTableCourseKeel")).Text;
                if ((nameElement == "Manuaaltestimise kursus") && (keelElement == "Mantis, Excel"))
                {
                    isObjectFound = true;
                    break;
                }
            }
            Assert.False(isObjectFound);
            Assert.True(!isObjectFound);
        }//eof

        private void DataClear_CreateUpdate(IWebDriver driver)
        {
            IWebElement createEntryNimetus = driver.FindElement(By.Id("CU_LanguageCourse_Nimetus"));
            createEntryNimetus.Clear();
            IWebElement createEntryKeel = driver.FindElement(By.Id("CU_LanguageCourse_Keel"));
            createEntryKeel.Clear();
            IWebElement createEntryTase = driver.FindElement(By.Id("CU_LanguageCourse_Tase"));
            createEntryTase.Clear();
            IWebElement createEntryKirjeldus = driver.FindElement(By.Id("CU_LanguageCourse_Kirjeldus"));
            createEntryKirjeldus.Clear();
        }


        // Delete
        private static void DataEntry_LanguageCourse(IWebDriver driver, bool isOneOrTwo = false)
        {
            if (!isOneOrTwo)
            {
                IWebElement createEntryNimetus = driver.FindElement(By.Id("CU_LanguageCourse_Nimetus"));
                createEntryNimetus.SendKeys("Automaattestimise kursus");
                IWebElement createEntryKeel = driver.FindElement(By.Id("CU_LanguageCourse_Keel"));
                createEntryKeel.SendKeys("C# + Selenium & xUnit");
                IWebElement createEntryTase = driver.FindElement(By.Id("CU_LanguageCourse_Tase"));
                createEntryTase.SendKeys("3");
                IWebElement createEntryKirjeldus = driver.FindElement(By.Id("CU_LanguageCourse_Kirjeldus"));
                createEntryKirjeldus.SendKeys("Kursusel õpime mis on automaattestimine, kuidas erineb manuaaltestimisest ning kuidas kirjutada " +
                    "backendi ja frontendi automaatteste.");
            }
            else
            {
                IWebElement createEntryNimetus = driver.FindElement(By.Id("CU_LanguageCourse_Nimetus"));
                createEntryNimetus.SendKeys("Manuaaltestimise kursus");
                IWebElement createEntryKeel = driver.FindElement(By.Id("CU_LanguageCourse_Keel"));
                createEntryKeel.SendKeys("Mantis, Excel");
                IWebElement createEntryTase = driver.FindElement(By.Id("CU_LanguageCourse_Tase"));
                createEntryTase.SendKeys("4");
                IWebElement createEntryKirjeldus = driver.FindElement(By.Id("CU_LanguageCourse_Kirjeldus"));
                createEntryKirjeldus.SendKeys("Kursusel õpime mis on manuaaltestimine, kuidas erineb automaattestimisest ning " +
                    "mis dokumentatsioon manuaaltestimise juures tähtis on ning kuidas neid kirjutada.");
            }
        }

        private static IWebDriver TestSetup_NavigateToIndex()
        {
            IWebDriver driver = new FirefoxDriver(); //brauserit juhtiv ja seal tegutsev draiver
            driver.Url = "https://localhost:7191/"; //navigeeritav aadress
            Thread.Sleep(3000);
            IWebElement navigateToLanguageCourse = driver.FindElement(By.LinkText("Keelekursused"));//otsime tesitava osa lingi
            navigateToLanguageCourse.Click(); //simuleeritud tegevus elemendiga, antud juhul klikk
            return driver;
        }
    }
}