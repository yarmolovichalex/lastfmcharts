﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using DB;
using LastFMCharts.Models;

namespace LastFMCharts.Controllers
{
    public class ChartController : Controller
    {
        private readonly IDBService dbService;

        public ChartController()
        {
            this.dbService = new DBService();
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
                .Select(LastFM.getArtistInfo)
                .Select(result => new ArtistViewModel
                {
                    Name = result.Name,
                    Listeners = result.Listeners,
                    Plays = result.Plays,
                    Views = dbService.GetArtistViews(result.Name)
                }).ToList();

                return Json(model);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message);
            }
        }

        [HttpPost]
        public JsonResult GetArtistsSuggestions(string userInput)
        {
            try
            {
                return Json(LastFM.getArtistSuggestions(userInput).Take(5));
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message);
            }
        }
    }
}