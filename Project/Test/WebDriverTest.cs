﻿using Codeer.Friendly.Dynamic;
using Codeer.Friendly.Windows;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Selenium.CefSharp.Driver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Codeer.Friendly.Windows.Grasp;
using OpenQA.Selenium.Internal;
using NUnit.Framework;

namespace Test
{
    public class WebDriverTestWinForm : WebDriverTestBase
    {
        WindowsAppFriend _app;
        CefSharpDriver _driver;

        public override IWebDriver GetDriver() => _driver;

        [SetUp]
        public void SetUp()
        {
            _driver.Url = this.GetHtmlUrl();
        }

        [OneTimeSetUp]
        public void ClassInit()
        {
            ClassInitBase();
            
            var appWithDriver = AppRunner.RunWinFormApp();
            _app = appWithDriver.App;
            _driver = appWithDriver.Driver;
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Process.GetProcessById(_app.ProcessId).Kill();
            ClassCleanupBase();
        }
    }

    public class WebDriverTestWPF : WebDriverTestBase
    {
        WindowsAppFriend _app;
        CefSharpDriver _driver;

        public override IWebDriver GetDriver() => _driver;

        [SetUp]
        public void SetUp()
        {
            _driver.Url = this.GetHtmlUrl();
        }

        [OneTimeSetUp]
        public void ClassInit()
        {
            ClassInitBase();
            var appWithDriver = AppRunner.RunWpfApp();
            _app = appWithDriver.App;
            _driver = appWithDriver.Driver;
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Process.GetProcessById(_app.ProcessId).Kill();
            ClassCleanupBase();
        }
    }

    public class WebDriverTestSelenium : WebDriverTestBase
    {
        IWebDriver _driver;

        public override IWebDriver GetDriver() => _driver;

        [SetUp]
        public void initialize()
        {
            _driver.Url = this.GetHtmlUrl();
        }

        [TearDown]
        public void TearDown()
        {
        }

        [OneTimeSetUp]
        public void ClassInit()
        {
            ClassInitBase();
            _driver = new ChromeDriver();
        }
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _driver.Dispose();
            ClassCleanupBase();
        }
    }

    public abstract class WebDriverTestBase: CompareTestBase
    {
        //TODO LinkText, PartialLinkText

        //FindElement(s)ById

        [Test]
        public void ShouldGetFirstElementWhenUsedFindElementById()
        {
            foreach (var element in new[] { GetDriver().FindElement(By.Id("idtest")), GetDriver<IFindsById>().FindElementById("idtest") })
            {
                var dataKey = element.GetAttribute("data-key");
                Assert.AreEqual("1", dataKey);
            } 
        }

        [Test]
        public void ShouldThrowExceptionWhenMissingElementUsedFindElementById()
        {
            AssertCompatible.ThrowsException<NoSuchElementException>(() => GetDriver().FindElement(By.Id("idtest_no")));
            AssertCompatible.ThrowsException<NoSuchElementException>(() => GetDriver<IFindsById>().FindElementById("idtest_no"));
        }

        [Test]
        public void ShouldGetAllElementWhenUsedFindElementsById()
        {
            foreach (var elements in new[] { GetDriver().FindElements(By.Id("idtest")), GetDriver<IFindsById>().FindElementsById("idtest") })
            {
                Assert.AreEqual(3, elements.Count);
                Assert.AreEqual("1", elements[0].GetAttribute("data-key"));
                Assert.AreEqual("2", elements[1].GetAttribute("data-key"));
                Assert.AreEqual("3", elements[2].GetAttribute("data-key"));
            }
        }

        [Test]
        public void ShouldIgnoreQuerySelectorNameWhenUsedFindElementsById()
        {
            foreach (var elements in new[] { GetDriver().FindElements(By.Id("form .term")), GetDriver<IFindsById>().FindElementsById("form .term") })
            {
                Assert.AreEqual(0, elements.Count);
            }
        }

        [Test]
        public void ShouldReturnEmptyWhenMissingElementsUsedByFindElementsById()
        {
            foreach (var elements in new[] { GetDriver().FindElements(By.Id("idtest_no")), GetDriver<IFindsById>().FindElementsById("idtest_no") })
            {
                Assert.AreEqual(0, elements.Count);
            }
        }

        //FindElement(s)ByName

        [Test]
        public void ShouldGetFirstElementWhenUsedFindElementByName()
        {
            foreach (var element in new[] { GetDriver().FindElement(By.Name("nametest")), GetDriver<IFindsByName>().FindElementByName("nametest") })
            {
                var dataKey = element.GetAttribute("data-key");
                Assert.AreEqual("1", dataKey);
            }
        }

        [Test]
        public void ShouldThrowExceptionWhenMissingElementUsedFindElementByName()
        {
            AssertCompatible.ThrowsException<NoSuchElementException>(() => GetDriver().FindElement(By.Name("nametest_no")));
            AssertCompatible.ThrowsException<NoSuchElementException>(() => GetDriver<IFindsByName>().FindElementByName("nametest_no"));
        }

        [Test]
        public void ShouldGetAllElementWhenUsedFindElementsByName()
        {
            foreach (var elements in new[] { GetDriver().FindElements(By.Name("nametest")), GetDriver<IFindsByName>().FindElementsByName("nametest") })
            {
                Assert.AreEqual(3, elements.Count);
                Assert.AreEqual("1", elements[0].GetAttribute("data-key"));
                Assert.AreEqual("2", elements[1].GetAttribute("data-key"));
                Assert.AreEqual("3", elements[2].GetAttribute("data-key"));
            }
        }

        [Test]
        public void ShouldReturnEmptyWhenMissingElementsUsedByFindElementsByName()
        {
            foreach (var elements in new[] { GetDriver().FindElements(By.Name("nametest_no")), GetDriver<IFindsByName>().FindElementsByName("nametest_no") })
                Assert.AreEqual(0, elements.Count);
        }

        //FindElement(s)ByClassName

        [Test]
        public void ShouldGetFirstElementWhenUsedFindElementByClassName()
        {
            foreach (var element in new[] { GetDriver().FindElement(By.ClassName("classtest")), GetDriver<IFindsByClassName>().FindElementByClassName("classtest") })
            {
                var dataKey = element.GetAttribute("data-key");
                Assert.AreEqual("1", dataKey);
            }
        }

        [Test]
        public void ShouldThrowExceptionWhenMissingElementUsedFindElementByClassName()
        {
            AssertCompatible.ThrowsException<NoSuchElementException>(() => GetDriver().FindElement(By.ClassName("classtest_no")));
            AssertCompatible.ThrowsException<NoSuchElementException>(() => GetDriver<IFindsByClassName>().FindElementByClassName("classtest_no"));
        }

        [Test]
        public void ShouldGetAllElementWhenUsedFindElementsByClassName()
        {
            foreach (var elements in new[] { GetDriver().FindElements(By.ClassName("classtest")), GetDriver<IFindsByClassName>().FindElementsByClassName("classtest") })
            {
                Assert.AreEqual(3, elements.Count);
                Assert.AreEqual("1", elements[0].GetAttribute("data-key"));
                Assert.AreEqual("2", elements[1].GetAttribute("data-key"));
                Assert.AreEqual("3", elements[2].GetAttribute("data-key"));
            }
        }

        [Test]
        public void ShouldReturnEmptyWhenMissingElementsUsedByFindElementsByClassName()
        {
            foreach (var elements in new[] { GetDriver().FindElements(By.ClassName("classtest_no")), GetDriver<IFindsByClassName>().FindElementsByClassName("classtest_no") })
            {
                Assert.AreEqual(0, elements.Count);
            }
        }

        //FindElement(s)ByCssSelector

        [Test]
        public void ShouldGetFirstElementWhenUsedFindElementByCssSelector()
        {
            foreach (var element in new[] { GetDriver().FindElement(By.CssSelector(".bytest > #idtest[name='nametest']")), GetDriver<IFindsByCssSelector>().FindElementByCssSelector(".bytest > #idtest[name='nametest']") })
            {
                var dataKey = element.GetAttribute("data-key");
                Assert.AreEqual("1", dataKey);
            }
        }

        [Test]
        public void ShouldThrowExceptionWhenMissingElementUsedFindElementByCssSelector()
        {
            AssertCompatible.ThrowsException<NoSuchElementException>(() => GetDriver().FindElement(By.CssSelector(".bytest > #idtest_no[name='nametest']")));
            AssertCompatible.ThrowsException<NoSuchElementException>(() => GetDriver<IFindsByCssSelector>().FindElementByCssSelector(".bytest > #idtest_no[name='nametest']"));
        }

        [Test]
        public void ShouldGetAllElementWhenUsedFindElementsByCssSelector()
        {
            foreach (var elements in new[] { GetDriver().FindElements(By.CssSelector(".bytest > #idtest[name='nametest']")), GetDriver<IFindsByCssSelector>().FindElementsByCssSelector(".bytest > #idtest[name='nametest']") })
            {
                Assert.AreEqual(2, elements.Count);
                Assert.AreEqual("1", elements[0].GetAttribute("data-key"));
                Assert.AreEqual("2", elements[1].GetAttribute("data-key"));
            }
        }

        [Test]
        public void ShouldReturnEmptyWhenMissingElementsUsedByFindElementsByCssSelector()
        {
            foreach (var elements in new[] { GetDriver().FindElements(By.CssSelector(".bytest > #idtest[name='nametest_no']")), GetDriver<IFindsByCssSelector>().FindElementsByCssSelector(".bytest > #idtest[name='nametest_no']") })
            {
                Assert.AreEqual(0, elements.Count);
            }
        }

        //FindElement(s)ByTagName

        [Test]
        public void ShouldGetFirstElementWhenUsedFindElementByTagName()
        {
            foreach (var element in new[] { GetDriver().FindElement(By.TagName("tagtest")), GetDriver<IFindsByTagName>().FindElementByTagName("tagtest") })
            {
                var dataKey = element.GetAttribute("data-key");
                Assert.AreEqual("1", dataKey);
            }
        }

        [Test]
        public void ShouldThrowExceptionWhenMissingElementUsedFindElementByTagName()
        {
            AssertCompatible.ThrowsException<NoSuchElementException>(() => GetDriver().FindElement(By.TagName("tagtest_no")));
            AssertCompatible.ThrowsException<NoSuchElementException>(() => GetDriver<IFindsByTagName>().FindElementByTagName("tagtest_no"));
        }

        [Test]
        public void ShouldGetAllElementWhenUsedFindElementsByTagName()
        {
            foreach (var elements in new[] { GetDriver().FindElements(By.TagName("tagtest")), GetDriver<IFindsByTagName>().FindElementsByTagName("tagtest") })
            {
                Assert.AreEqual(2, elements.Count);
                Assert.AreEqual("1", elements[0].GetAttribute("data-key"));
                Assert.AreEqual("2", elements[1].GetAttribute("data-key"));
            }
        }

        [Test]
        public void ShouldReturnEmptyWhenMissingElementsUsedByFindElementsByTagName()
        {
            foreach (var elements in new[] { GetDriver().FindElements(By.TagName("tagtest_no")), GetDriver<IFindsByTagName>().FindElementsByTagName("tagtest_no") })
            {
                Assert.AreEqual(0, elements.Count);
            }
        }

        //FindElement(s)ByXPath
        [Test]
        public void ShouldGetFirstElementWhenUsedFindElementByXPath()
        {
            foreach (var element in new[] { GetDriver().FindElement(By.XPath("/html/body/div[1]/tagtest")), GetDriver<IFindsByXPath>().FindElementByXPath("/html/body/div[1]/tagtest") })
            {
                var dataKey = element.GetAttribute("data-key");
                Assert.AreEqual("1", dataKey);
            }
        }

        [Test]
        public void ShouldThrowExceptionWhenMissingElementUsedFindElementByXPath()
        {
            AssertCompatible.ThrowsException<NoSuchElementException>(() => GetDriver().FindElement(By.XPath("/html/body/div[1]/tagtest_no")));
            AssertCompatible.ThrowsException<NoSuchElementException>(() => GetDriver<IFindsByXPath>().FindElementByXPath("/html/body/div[1]/tagtest_no"));
        }

        [Test]
        public void ShouldGetAllElementWhenUsedFindElementsByXPath()
        {
            foreach (var elements in new[] { GetDriver().FindElements(By.XPath("/html/body/div[1]/tagtest")), GetDriver<IFindsByXPath>().FindElementsByXPath("/html/body/div[1]/tagtest") })
            {
                Assert.AreEqual(2, elements.Count);
                Assert.AreEqual("1", elements[0].GetAttribute("data-key"));
                Assert.AreEqual("2", elements[1].GetAttribute("data-key"));
            }
        }

        [Test]
        public void ShouldReturnEmptyWhenMissingElementsUsedByFindElementsByXPath()
        {
            foreach (var elements in new[] { GetDriver().FindElements(By.XPath("/html/body/div[1]/tagtest_no")), GetDriver<IFindsByXPath>().FindElementsByXPath("/html/body/div[1]/tagtest_no") })
            {
                Assert.AreEqual(0, elements.Count);
            }
        }

        // Other

        [Test]
        public void ShouldThrowExceptionWhenReferenceTheRemovedElement()
        {
            var element = GetDriver().FindElement(By.Id("textBoxName"));
            AssertCompatible.IsInstanceOfType(element, typeof(IWebElement));
            element.SendKeys("ABC");
            GetExecutor().ExecuteScript("const elem = document.querySelector('#textBoxName'); elem.parentNode.removeChild(elem);");
            AssertCompatible.ThrowsException<StaleElementReferenceException>(() => element.SendKeys("DEF"));

            GetExecutor().ExecuteScript(@"
const elem = document.createElement('input');
elem.setAttribute('id', 'textBoxName');
document.body.appendChild(elem);");

            AssertCompatible.ThrowsException<StaleElementReferenceException>(() => element.SendKeys("DEF"));

            element = GetDriver().FindElement(By.Id("textBoxName"));
            AssertCompatible.IsInstanceOfType(element, typeof(IWebElement));
            element.SendKeys("ABC");
        }
    }
}