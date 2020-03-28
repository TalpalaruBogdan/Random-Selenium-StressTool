using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebMonkey;

namespace Tests
{
    public class Tests
    {
        IWebDriver driver;
        Monkey m;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver(Config.DriverPath);
            m = new Monkey();
            m.LoggerFilePath = Config.LogsPath;
            m.PictureSavePath = Config.PicsPath;
            m.Driver = driver;
            m.StressPeak = true;
        }

        [Test]
        public void StressTestGoogle()
        {            
            m.BaseURL = "https://www.google.com/";
            m.shouldContain = "google.com";
            m.MinutesTestTime = 10;
            m.DoMonkeyWork();
        }
    }
}