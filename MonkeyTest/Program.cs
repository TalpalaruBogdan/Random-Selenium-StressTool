using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebMonkey;

namespace MonkeyTest
{
    class Program
    {
        static void Main(string[] args)
        {
            IWebDriver driver = new ChromeDriver(@"C:\");
            Monkey m = new Monkey();
            m.LoggerFilePath = @"C:\Users\bogda\Desktop\MonkeyLogs\Logs.TXT";
            m.PictureSavePath = @"C:\Users\bogda\Desktop\MonkeyPics\";
            m.Driver = driver;
            m.StressPeak = false;
            m.BaseURL = "http://www.lego.com/innovation-intake/#/";
            m.shouldContain = "lego.com";
            m.MinutesTestTime = 10;
            m.DoMonkeyWork();
        }
    }
}
