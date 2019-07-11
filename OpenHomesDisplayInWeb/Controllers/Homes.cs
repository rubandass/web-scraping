using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace OpenHomesDisplayInWeb.Controllers
{
    public class Homes
    {

        public static Suburb HomesMain(string region, string district, string suburb)
        {
            //var value = JsonConvert.DeserializeObject(name);

            //JToken token = JObject.Parse(name);
            //string region = (string)token.SelectToken("Region");
            //string district = (string)token.SelectToken("District");
            var districtName = district.Split('(');
            //string suburb = (string)token.SelectToken("Suburb");
            var suburbName = suburb.Split('(');
            HomeDetails homeDetails = new HomeDetails();
            homeDetails.InvokeBrowser();
            var objsuburb = homeDetails.AllHomes(region, districtName[0], suburbName[0]);
            homeDetails.CloseBrowser();
            return objsuburb;
        }
    }

    class HomeDetails
    {
        public IWebDriver driver;
        
        List<string> homeUrlList = new List<string>();

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

        public Suburb AllHomes(string regionName,string districtName, string suburbName)
        {
            new SelectElement(driver.FindElement(By.XPath("//form[@id='sidebarSearch']/div/span//select[contains(@onchange,'region')]"))).SelectByText(regionName);
            new SelectElement(driver.FindElement(By.XPath("//form[@id='sidebarSearch']/div/span//select[contains(@onchange,'suburb')]"))).SelectByText(districtName, true);
            //Open the suburb drop down list
            driver.FindElement(By.XPath("//form[@id='sidebarSearch']/div//div[@class='multiselect-dropdown']")).Click();
            //Select the suburb by the name
            driver.FindElement(By.XPath("//div[contains(@class,'multiselect-option-list')]/ul/li/label[contains(text(),'" + suburbName + "')]")).Click();
            //Close the suburb list
            driver.FindElement(By.CssSelector("div.multiselect-button.drop-container.focus")).Click();
            driver.FindElement(By.XPath("//button[contains(text(),'Search')]")).Click();
            SearchResults();
            Suburb suburb = new Suburb();
            suburb.HomeList = new List<OpenHome>();
            OpenHomeDetails(suburb);

            return suburb;
        }

        public void SearchResults()
        {
            try
            {
                //Getting all the homes available in the selected suburb
                List<IWebElement> openhomelinkList = driver.FindElements(By.CssSelector("ul.openhomes li div.listingTitle a")).ToList();
                //Iterate through the list and get url of individual home
                foreach (var item in openhomelinkList)
                {
                    homeUrlList.Add(item.GetAttribute("href"));
                }
                //If the suburb has more than 30 homes then we need to click next page(s)
                bool nextButtonDisplay = driver.FindElement(By.XPath("//table[@id='PagingFooter']//font/b[contains(text(),'Next')]")).Displayed;
                if (nextButtonDisplay)
                {
                    driver.FindElement(By.XPath("//table[@id='PagingFooter']//font/b[contains(text(),'Next')]")).Click();
                    //As of now only the first page home(s) are added to "homeUrlList".So we need to add the next page(s) homes
                    SearchResults();
                }
            }
            catch (Exception)
            {

            }
        }


        public void OpenHomeDetails(Suburb suburb)
        {
            foreach (var url in homeUrlList)
            {
                driver.Navigate().GoToUrl(url);
                string location = driver.FindElement(By.XPath("//th[contains(text(),'Location:')]/following::td[1]")).Text;
                //There is soft hyphen presented in city name. To avoid that use the below line, first replace with '~' and then replace with ""
                location = location.Replace((char)173, '~').Replace("~", "");
                string rooms = driver.FindElement(By.XPath("//th[contains(text(),'Rooms:')]/following::td[1]")).Text;
                string propertyType = driver.FindElement(By.XPath("//th[contains(text(),'Property type:')]/following::td[1]")).Text;

                OpenHome home = new OpenHome();

                home.Location = location;
                home.Rooms = rooms;
                home.PropertyType = propertyType;
                home.Price = Price();
                home.Parking = Parking();
                home.OpenHomeTime = OpenHomeTimes();
                suburb.HomeList.Add(home);
            }
            homeUrlList.Clear();
        }

        public string Price()
        {
            try
            {
                string price = driver.FindElement(By.XPath("//th[contains(text(),'Price:')]/following::td[1]")).Text;
                //There is one instance 'price by negotiation' contains soft hyphen
                price = price.Replace((char)173, '~').Replace("~", "");
                return price;
            }
            catch (Exception)
            {
                return "Price not found";
            }
        }

        public string Parking()
        {
            try
            {
                string parking = driver.FindElement(By.XPath("//th[contains(text(),'Parking:')]/following::td[1]")).Text;
                return parking;
            }
            catch (Exception)
            {
                return "Parking not found";
            }
        }

        public string OpenHomeTimes()
        {
            try
            {
                string openHomeTiming = driver.FindElement(By.XPath("//th[contains(text(),'Open home times:')]/following::td[1]/div[1]")).Text;
                return openHomeTiming;
            }
            catch (Exception)
            {
                return "Open home timings not found";
            }
        }
    }
}