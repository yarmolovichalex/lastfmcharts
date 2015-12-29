using System;
using System.Net;
using System.Web.Mvc;

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
        public JsonResult Index(string artist)
        {
            try
            {
                return Json(LastFM.getTopTracks(artist));
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message);
            }
        }
    }
}