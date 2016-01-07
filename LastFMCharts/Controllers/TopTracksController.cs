using System;
using System.Linq;
using System.Web.Configuration;
using System.Web.Http;

namespace LastFMCharts.Controllers
{
    public class TopTracksController : ApiController
    {
        public IHttpActionResult Get(string input, int page = 1)
        {
            try
            {
                var topTracks = LastFM.getTopTracksNames(input);
                var tracksOnPage = int.Parse(WebConfigurationManager.AppSettings["tracksOnPage"]);

                return Ok(topTracks.Skip((page - 1) * tracksOnPage).Take(tracksOnPage));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}