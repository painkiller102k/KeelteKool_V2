using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace KeelteKoolV2.SeleniumTesting
{
    public class LanguageCourseFrontendTests
    {
        [Fact]
        public void Should_NavigateToIndex_AddSpaceshipWithCorrectData_ReturnToIndex()
        {
            /* Ülesseade */
            IWebDriver driver = new FirefoxDriver(); //brauserit juhtiv ja seal tegutsev draiver
            driver.Url = "https://localhost:7191/"; //navigeeritav aadress
            Thread.Sleep(3000);
            IWebElement navigateToLanguageCourse = driver.FindElement(By.LinkText("Keelekursused"));//otsime tesitava osa lingi
            navigateToLanguageCourse.Click(); //simuleeritud tegevus elemendiga, antud juhul klikk

            /* tegevus */
            IWebElement indexCreateButton = driver.FindElement(By.Id("IndexCreateNewLanguageCourse_BTN"));
            indexCreateButton.Click();
            IWebElement createEntryNimetus = driver.FindElement(By.Id("CU_LanguageCourse_Nimetus"));
            createEntryNimetus.SendKeys("Automaattestimise kursus");
            IWebElement createEntryKeel = driver.FindElement(By.Id("CU_LanguageCourse_Keel"));
            createEntryKeel.SendKeys("C# + Selenium & xUnit");
            IWebElement createEntryTase = driver.FindElement(By.Id("CU_LanguageCourse_Tase"));
            createEntryTase.SendKeys("3");
            IWebElement createEntryKirjeldus = driver.FindElement(By.Id("CU_LanguageCourse_Kirjeldus"));
            createEntryKirjeldus.SendKeys("Kursusel õpime mis on automaattestimine, kuidas erineb manuaaltestimisest ning kuidas kirjutada " +
                "backendi ja frontendi automaatteste.");
            IWebElement CreateCreateButton = driver.FindElement(By.Id("CU_Create_BTN"));
            CreateCreateButton.Click();

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
    }
}