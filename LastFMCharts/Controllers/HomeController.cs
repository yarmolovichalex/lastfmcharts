using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LastFMCharts.Models;

namespace LastFMCharts.Controllers
{
    public class HomeController : Controller
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
        public ActionResult Compare(IEnumerable<string> artists)
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

                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpGet]
        public ActionResult Test()
        {
            return null;
        }
    }
}