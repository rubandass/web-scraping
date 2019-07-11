using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace OpenHomesDisplayInWeb.Controllers
{
    public class ResultController : Controller
    {
        OpenHomeDBContext db = new OpenHomeDBContext();

        public ActionResult Home()
        {
            return View();
        }


        public ActionResult Index(string name)
        {
            //List<Suburb> suburb = db.SuburbModels.Where(s => s.SuburbName == name).ToList();
            //ViewBag.Suburb = suburb;
            //Homes homes = new Homes();
            return View();

        }

        public JsonResult GetHomeDetails(string region, string district, string suburb)
        {
            var result = Homes.HomesMain(region, district, suburb);
            return Json(result.HomeList, JsonRequestBehavior.AllowGet);
            /*List<Suburb> suburbs = db.SuburbModels.Where(s => s.SuburbName == name).ToList();
            var homeList = db.OpenHomeModels.Where(s => s.Suburb.SuburbName == name).Select(x => new
            {
                Location = x.Location,
                Rooms = x.Rooms,
                PropertyType = x.PropertyType,
                Price = x.Price,
                Parking = x.Parking,
                OpenHomeTime = x.OpenHomeTime
            });
            return Json(homeList, JsonRequestBehavior.AllowGet);*/
        }

        public JsonResult NewRegions()
        {
            if (!RouteConfig.browserOpen)
            {
                Program.Main();
                RouteConfig.browserOpen = true;
            }
            var regions = TradeMe.regionsList;
            string value = string.Empty;
            value = JsonConvert.SerializeObject(regions, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return Json(value, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Regions()
        {
            if (!RouteConfig.browserOpen)
            {
                Program.Main();
                RouteConfig.browserOpen = true;
            }
            var regions = db.RegionModels.ToList();
            foreach (var region in regions)
            {
                List<District> distList = db.DistrictModels.Where(r => r.Region.RegionName == region.RegionName).ToList();

                //distList = db.DistrictModels.Where(r => r.Region.RegionName == region.RegionName).ToList();
                foreach (var district in distList)
                {
                    List<Suburb> suburbList = db.SuburbModels.Where(d => d.District.DistrictName == district.DistrictName).ToList();
                    //suburbList = db.SuburbModels.Where(d => d.District.DistrictName == district.DistrictName).ToList();
                }
            }
            string value = string.Empty;
            value = JsonConvert.SerializeObject(regions, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return Json(value, JsonRequestBehavior.AllowGet);
        }

    }
}