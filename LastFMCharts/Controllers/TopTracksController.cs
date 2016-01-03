using System;
using System.Web.Http;

namespace LastFMCharts.Controllers
{
    public class TopTracksController : ApiController
    {
        public IHttpActionResult Get(string input)
        {
            try
            {
                return Ok(LastFM.getTopTracksNames(input));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}