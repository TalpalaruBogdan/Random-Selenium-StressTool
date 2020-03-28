using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace WebMonkey
{
    public class Monkey
    {

        public IWebDriver Driver { private get; set; }
        public int SecondsWaitAfterAction { private get; set; }
        public int SecondsImplicitWait { private get; set; }
        public int MinutesTestTime { private get; set; }
        public string BaseURL { private get; set; }
        public string PictureSavePath { private get; set; }
        public string LoggerFilePath { private get; set; }
        public bool StressPeak { private get; set; }
        public bool TakeScreenshotMode { private get; set; }


        private Stopwatch stopwatch;
        private TimeSpan t;
        private Random rand;
        private List<IWebElement> clk, tx;
        public string shouldContain;

        public string GetRandomString(int length = 30)
        {
            string baseString = "abcdefghijklmnopqrstuvxywzABCDEFGHIJKLMNOPQRSTUVXYWZ12343567890!@#$%^&*()\'\\/?<>,.     ";
            StringBuilder sb = new StringBuilder("");
            for (int i = 0; i < length; i++)
                sb.Append(baseString[rand.Next(0, baseString.Length)]);
            return sb.ToString();
        }

        public Monkey()
        {
            TakeScreenshotMode = true;
            StressPeak = false;
            SecondsWaitAfterAction = 5;
            SecondsImplicitWait = 5;
            rand = new Random();
            MinutesTestTime = 1;
            clk = new List<IWebElement>();
            tx = new List<IWebElement>();
        }

        private void Log(string message)
        {
            if (LoggerFilePath != null)
            {
                File.AppendAllText(LoggerFilePath, message + Environment.NewLine);
            }
        }

        private void TakeScreenshot()
        {
            
            if (PictureSavePath != null)
            {
                var pic = ((ITakesScreenshot)Driver).GetScreenshot();
                string name = DateTime.Now.ToString("yyyyMMddhhmmss") + ".png";
                pic.SaveAsFile(PictureSavePath + name);
                Log("Saving picture " + PictureSavePath + name);

            }
            else
            {
                Log("Picture save path not defined. Please set it first");
            }


        }


        private void clickElement(List<IWebElement> col)
        {

            int index = rand.Next(0, col.Count);
            col[index].Click();
            Log("Clicking element with index# " + index);
        }

        private void SendText(List<IWebElement> col)
        {
            string input = GetRandomString() + Keys.Enter;
            int index = rand.Next(0, col.Count);
            col[index].Clear();
            col[index].SendKeys(input);
            Log("Sending text \"" + input + "\" to textable element with index# " + index);
        }



        public void DoMonkeyWork()
        {

            if (StressPeak)
            {
                SecondsImplicitWait = 0;
                SecondsWaitAfterAction = 0;
                TakeScreenshotMode = false;
            }

            Log("********* SESSION STARTED ***********");
            Driver.Manage().Window.Maximize();
            Driver.Navigate().GoToUrl(BaseURL);
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(SecondsImplicitWait);
            stopwatch = new Stopwatch();

            stopwatch.Start();
            do
            {
                #region handle iframes
                /*
                var iframes = Driver.FindElements(By.TagName("iframe"));

                if (iframes.Count > 0)
                {
                    switch (rand.Next(0, 2))
                    {
                        case (0):
                            {
                                break;
                            }
                        case (1):
                            {
                                int nextFrame = rand.Next(0, iframes.Count);
                                Log("Switching ot iframe#: " + nextFrame);
                                Driver.SwitchTo().Frame(nextFrame);
                                break;
                            }
                    }
                }
                */
                #endregion

                Driver.SwitchTo().Window(Driver.WindowHandles.First());

                if (!Driver.Url.Contains(shouldContain))
                {
                    Driver.Navigate().GoToUrl(BaseURL);
                }


                try
                {
                    Driver.SwitchTo().Alert().Accept();
                }
                catch (Exception ex)
                {
                    try
                    {
                        Driver.SwitchTo().Alert().Dismiss();
                    }
                    catch (Exception ex1)
                    {
                        //log ?
                    }
                }




                var buttons = Driver.FindElements(By.TagName("button"));
                var links = Driver.FindElements(By.TagName("a"));
                var clickables = Driver.FindElements(By.XPath("//input[@type=\"submit\"]"));
                var textInputs = Driver.FindElements(By.XPath("//input[@type=\"text\" or @type=\"password\" or @type=\"search\"]"));

                //TODO add radio, drop down etc...
                clk.Clear();
                tx.Clear();

                for (int i = 0; i < buttons.Count; i++)
                    clk.Add(buttons[i]);
                for (int i = 0; i < clickables.Count; i++)
                    clk.Add(clickables[i]);
                for (int i = 0; i < links.Count; i++)
                    clk.Add(links[i]);
                for (int i = 0; i < textInputs.Count; i++)
                    tx.Add(textInputs[i]);

                try
                {
                    

                    int choice = rand.Next(0, 3);
                    switch (choice)
                    {
                        case (0):
                            {
                                if (clk.Count > 0)
                                {
                                    clickElement(clk);
                                }
                                else if (clk.Count > 0)
                                {
                                    SendText(clk);
                                }
                                else
                                {
                                    Driver.Navigate().GoToUrl(BaseURL);
                                }
                                break;
                            }
                        case (1):
                            {
                                if (tx.Count > 0)
                                {
                                    SendText(tx);
                                }
                                else if (clk.Count > 0)
                                {
                                    clickElement(clk);
                                }
                                else
                                {
                                    Driver.Navigate().GoToUrl(BaseURL);
                                }
                                break;
                            }
                        default:
                            {
                                continue;
                            }
                    }
                }
                catch (Exception ex)
                {
                    Log(ex.Message);
                    continue;
                }
                Thread.Sleep(SecondsWaitAfterAction * 1000);
                if (TakeScreenshotMode)
                    TakeScreenshot();

                t = stopwatch.Elapsed;
            }
            while (t.TotalSeconds < (MinutesTestTime * 60));

            Log("*********** DONE ***********\n>\n");
            Driver.Quit();
        }
    }
}
