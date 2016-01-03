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
    }
}