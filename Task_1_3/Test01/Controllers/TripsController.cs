using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Test01.DB;
using Test01.DB.Models;
using Test01.Helpers;

namespace Test01.Controllers
{
    // MODELS
    public class TripModel
    {
        public int Id { get; set; }
        public DateTime DateUTC { get; set; }
        public string Destination { get; set; }
        public string Country { get; set; }
        public string Title { get; set; }
        public int UserId { get; set; }
    }

    // METHODS
    /// <summary>
    /// should create TripRepository, but for simplicity for this specific exmaple its fairly enough
    /// </summary>

    [Route("api/trips")]
    [ApiController]
    [Authorize]
    public class TripsController : ControllerBase
    {
        private readonly User _User;
        private readonly TripContext _DB;

        public TripsController(TripContext db, IHttpContextAccessor cta)
        {
            _DB = db;
            _User = Helper.EnsureUser(db, cta.HttpContext.User);
        }

        // GET: api/Trips
        [HttpGet]
        [AllowAnonymous] //guest
        public async Task<ActionResult<IEnumerable<TripModel>>> GetTrips()
        {
            return await _DB.Trips.Select(x => new TripModel
            {
                Id = x.TripId,
                DateUTC = x.DateUTC,
                Destination = x.Destination,
                Country = x.Country,
                Title = x.Title,
                UserId = x.UserId
            }).ToListAsync();
        }

        // GET: api/Trips/5
        [HttpGet("{id}")]
        [AllowAnonymous] //guest
        public async Task<ActionResult<TripModel>> GetTrip(int id)
        {
            var t = await _DB.Trips.FindAsync(id);
            if (t == null)
                return NotFound();
            
            return new TripModel
            {
                Id = t.TripId,
                DateUTC = t.DateUTC,
                Destination = t.Destination,
                Country = t.Country,
                Title = t.Title,
                UserId = t.UserId
            };
        }

        // PUT: api/Trips/5
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> PutTrip(TripModel m)
        {
            var t = await _DB.Trips.FindAsync(m.Id);
            if (t == null)
                return NotFound();
            if (!CanEditOrDelete(_User.UserId, m.UserId))
                return NotFound(); //for internal should be NotAuthorized, but for public security reasons we should return zero info about request.


            //update entry
            t.DateUTC = m.DateUTC;
            t.Destination = m.Destination;
            t.Country = m.Country;
            t.Title = m.Title;
            t.UserId = m.UserId;

            _DB.Entry(t).State = EntityState.Modified;

            try
            {
                await _DB.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TripExists(m.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Trips
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Trip>> PostTrip(TripModel m)
        {
            var uid = //get id from User AD
            _DB.Trips.Add(new Trip
            {
                DateUTC = m.DateUTC,
                Destination = m.Destination,
                Country = m.Country,
                Title = m.Title,
                UserId = _User.UserId
            });
            await _DB.SaveChangesAsync();

            return CreatedAtAction("GetTrip", new { id = m.Id }, m);
        }

        // DELETE: api/Trips/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<Trip>> DeleteTrip(int id)
        {
            var t = await _DB.Trips.FindAsync(id);
            if (t == null)
                return NotFound();
            if (!CanEditOrDelete(_User.UserId, t.UserId))
                return NotFound();

            _DB.Trips.Remove(t);
            await _DB.SaveChangesAsync();

            return t;
        }

        // MISC
        [HttpGet]
        [AllowAnonymous] //guest
        [Route("countries")]
        public async Task<ActionResult<object>> GetCountries()
        {
            const string countriesEndpoint = @"https://restcountries.eu/rest/v2/all?fields=name";
            var objs = await countriesEndpoint.GetJsonListAsync();
            return new JsonResult(objs.Select(x => x.name));
        }



        // PRIVATE
        private bool CanEditOrDelete(int rId, int oId)
        {
            return (rId == oId || User.IsInRole("Admin"));
        }
        private bool TripExists(int id)
        {
            return _DB.Trips.Any(e => e.TripId == id);
        }
    }

    
}
