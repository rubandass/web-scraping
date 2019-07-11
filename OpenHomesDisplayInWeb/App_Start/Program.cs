using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;


namespace OpenHomesDisplayInWeb
{
    class Program
    {
        public static void Main()
        {
            TradeMe tradeMe = new TradeMe();
            tradeMe.InvokeBrowser();
            tradeMe.AllRegions();
            //SaveToDB saveToDB = new SaveToDB();
            //saveToDB.Insert(regions);
            tradeMe.CloseBrowser();
        }
    }

    class TradeMe
    {
        public IWebDriver driver;
        public static List<Region> regionsList = new List<Region>();


        public void InvokeBrowser()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("disable-infobars", "blink-settings=imagesEnabled=false","headless");
            driver = new ChromeDriver(options);
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(60);
            driver.Navigate().GoToUrl("https://www.trademe.co.nz/property/open-homes");
        }

        public void CloseBrowser()
        {
            driver.Close();
        }

        public void AllRegions()
        {
            List<IWebElement> regionsElementsList = new List<IWebElement>();

            regionsElementsList = driver.FindElements(By.XPath("//form[@id='sidebarSearch']/div/span//select[contains(@onchange,'region')]/option")).ToList();
            foreach (var item in regionsElementsList)
            {
                if (item.Text != "" && item.Text != "All regions")
                {
                        Region region = new Region();
                        region.DistrictList = new List<District>();
                        region.RegionName = item.Text;
                        regionsList.Add(region);
                        AllDistrict(region);
                }
            }
        }

        public void AllDistrict(Region region)
        {
            List<IWebElement> districtElementsList = new List<IWebElement>();
            List<District> districtsList = new List<District>();

            new SelectElement(driver.FindElement(By.XPath("//form[@id='sidebarSearch']/div/span//select[contains(@onchange,'region')]"))).SelectByText(region.RegionName);
            districtElementsList = driver.FindElements(By.XPath("//form[@id='sidebarSearch']/div/span//select[contains(@onchange,'suburb')]/option")).ToList();
            foreach (var item in districtElementsList)
            {
                if (item.Text != "" && item.Text != "All districts")
                {
                    District district = new District();
                    district.SuburbList = new List<Suburb>();
                    district.DistrictName = item.Text;
                    districtsList.Add(district);
                    AllSuburbMethod(district);
                    region.DistrictList.Add(district);
                }
            }
        }

        public void AllSuburbMethod(District district)
        {
            List<IWebElement> suburbElementsList = new List<IWebElement>();
            List<Suburb> suburbsList = new List<Suburb>();

            new SelectElement(driver.FindElement(By.XPath("//form[@id='sidebarSearch']/div/span//select[contains(@onchange,'suburb')]"))).SelectByText(district.DistrictName, true);

            suburbElementsList = driver.FindElements(By.XPath("//div[contains(@class,'multiselect-option-list')]/ul/li/span/input[contains(@id,'suburb')]")).ToList();

            foreach (var item in suburbElementsList)
            {
                if (item.GetAttribute("Text") != "" && item.GetAttribute("Text") != "All suburbs")
                {
                    Suburb suburb = new Suburb();
                    suburb.HomeList = new List<OpenHome>();
                    suburb.SuburbName = item.GetAttribute("text");
                    suburbsList.Add(suburb);
                    district.SuburbList.Add(suburb);
                }
            }
        }

    }
}
