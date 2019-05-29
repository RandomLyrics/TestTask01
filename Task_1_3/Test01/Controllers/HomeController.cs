using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Test01.DB;
using Test01.DB.Models;
using Test01.Helpers;
using Test01.Models;

namespace Test01.Controllers
{
    [Authorize]
    [Route("home")]
    public class HomeController : Controller
    {
        private readonly User _User;
        private readonly TripContext _DB;

        public HomeController(TripContext db, IHttpContextAccessor cta)
        {
            _DB = db;
            _User = Helper.EnsureUser(db, cta.HttpContext.User);
        }

        [Route("/")]
        [Route("")]
        public IActionResult Index()
        {
            ViewData["AppUserId"] = _User.UserId;
            //var us = _DB.Users.ToList();
            //var t = _DB.Trips.First();
            //t.Country = "Polska";
            //_DB.SaveChanges();
            //_DB.Users.Add(new User
            //{
            //    Name = "Bodyzar" + dd++,
            //    Trips = new List<Trip>
            //    {
            //       new Trip
            //       {
            //           Title = "Majoreczka" + dd++,
            //           DateUTC = DateTime.UtcNow,
            //           Destination = "Majorka" + dd++
            //       }
            //    }
            //});
            //_DB.SaveChanges();
            return View();
        }

        [Route("privacy")]
        [Authorize(Roles = "Admin,Writer")]
        public IActionResult Privacy()
        {
            return View();
        }

        [Route("waw")]
        [AllowAnonymous]
        public IActionResult Waw()
        {
            return View();
        }

        [Route("error")]
        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
