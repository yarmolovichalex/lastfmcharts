using System;
using System.Linq;
using System.Web.Http;

namespace LastFMCharts.Controllers
{
    public class ArtistSuggestionsController : ApiController
    {
        public IHttpActionResult Get(string userInput)
        {
            try
            {
                return Ok(LastFM.getArtistSuggestions(userInput).Take(5));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}