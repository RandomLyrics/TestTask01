using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Test01.DB;
using Test01.DB.Models;

namespace Test01.Helpers
{
    public static class Helper
    {
        //could be served as a service, and then we could use async
        internal static User EnsureUser(TripContext db, ClaimsPrincipal user)
        {
            if (user == null)
                return null;

            var u = db.Users.FirstOrDefault(x => x.Name == user.Identity.Name);
            if (u == null)
            {
                //register new user
                u = new User{ Name = user.Identity.Name };
                db.Users.Add(u);
                db.SaveChanges();
            }
            return u;
        }
    }
}
