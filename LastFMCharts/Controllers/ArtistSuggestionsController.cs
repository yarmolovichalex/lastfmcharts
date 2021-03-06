﻿using System;
using System.Linq;
using System.Web.Http;

namespace LastFMCharts.Controllers
{
    public class ArtistSuggestionsController : ApiController
    {
        public IHttpActionResult Get(string input)
        {
            try
            {
                // todo move 5 to config
                return Ok(LastFM.getArtistSuggestions(input).Take(5));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}