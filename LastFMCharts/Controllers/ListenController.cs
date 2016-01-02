using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web.Mvc;
using LastFMCharts.Models;

namespace LastFMCharts.Controllers
{
    public class ListenController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Index(string artist, string token)
        {
            try
            {
                // TODO find out, why more than 5 simultaneous requests cause error. this is temporary solution
                var topTracksNames = LastFM.getTopTracksNames(artist).Take(5);
                var tracks = topTracksNames.Select(trackName =>
                {
                    var fullTrackName = $"{artist} - {trackName}";

                    var url = VK.getTrackUrl(fullTrackName, token);

                    return new TrackViewModel
                    {
                        Name = trackName,
                        Url = url
                    };
                }).ToList();

                return Json(tracks);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message);
            }
        }
    }
}