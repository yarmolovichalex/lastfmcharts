using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LastFMCharts.Models;

namespace LastFMCharts.Controllers
{
    public class ChartController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string artist)
        {
            var artistInfo = LastFM.getArtist(artist);

            var model = new ArtistViewModel
            {
                Name = artistInfo.Name,
                Listeners = artistInfo.Listeners,
                Plays = artistInfo.Plays,
                Similar = artistInfo.Similar
            };

            return View(model);
        }

        [HttpGet]
        public ActionResult Compare()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Compare(IEnumerable<string> artists)
        {
            try
            {
                var model = artists
                .Select(LastFM.getArtist)
                .Select(result => new ArtistViewModel
                {
                    Name = result.Name,
                    Listeners = result.Listeners,
                    Plays = result.Plays,
                    Similar = result.Similar
                }).ToList();

                return Json(model);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [HttpPost]
        public JsonResult GetArtistsSuggestions(string userInput)
        {
            try
            {
                return Json(LastFM.getArtistSuggestions(userInput).Where(x => x.StartsWith(userInput, StringComparison.InvariantCultureIgnoreCase)).Take(5));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
    }
}