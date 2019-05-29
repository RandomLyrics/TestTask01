using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
using Flurl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Test01.Configs;
using Newtonsoft.Json;

namespace Test01.Controllers
{
    // MODELS

    //autogen
    public class Status
    {
        public string status { get; set; }
        public string description { get; set; }
        public object changeDate { get; set; }
    }

    public class Notyfication
    {
        public string category { get; set; }
        public string city { get; set; }
        public string subcategory { get; set; }
        public string district { get; set; }
        public object aparmentNumber { get; set; }
        public object street2 { get; set; }
        public string notificationType { get; set; }
        public long createDate { get; set; }
        public string siebelEventId { get; set; }
        public string source { get; set; }
        public double yCoordOracle { get; set; }
        public string street { get; set; }
        public string deviceType { get; set; }
        public List<Status> statuses { get; set; }
        public double xCoordOracle { get; set; }
        public string notificationNumber { get; set; }
        public double yCoordWGS84 { get; set; }
        public string @event { get; set; }
        public double xCoordWGS84 { get; set; }
    }

    public class IncidentModel
    {
        public string SubCategory { get; set; }
        public string District { get; set; }
        public string Event { get; set; }
        public string Status { get; set; }
    }

    // METHODS
    [Route("api/waw")]
    [ApiController]
    public class WawApiController : ControllerBase
    {
        private readonly IOptions<WawApiConfig> _cfg;

        public WawApiController(IOptions<WawApiConfig> config)
        {
            _cfg = config;
        }

        [HttpGet]
        [AllowAnonymous] //guest
        [Route("incidents")]
        public async Task<ActionResult<IEnumerable<IncidentModel>>> GetIncidents(
            [FromQuery]DateTime? DateFrom,
            [FromQuery]DateTime? DateTo
            )
        {
            if (!(DateFrom.HasValue && DateTo.HasValue))
                return BadRequest();

            var url = _cfg.Value.RequestDateURL
                .SetQueryParam("id", _cfg.Value.ResourceId)
                .SetQueryParam("apikey", _cfg.Value.ApiKey)
                .SetQueryParam("dateFrom", GetUnixMiliseconds(DateFrom.Value))
                .SetQueryParam("dateTo", GetUnixMiliseconds(DateTo.Value));

            var r = await url.GetJsonAsync();
            List<Notyfication> pr = JsonConvert.DeserializeObject<List<Notyfication>>
                (JsonConvert.SerializeObject(r.result.result.notifications));

            var objs = pr.Select(x => new IncidentModel
            {
                District = x.district,
                Event = x.@event,
                SubCategory = x.subcategory,
                Status = x.statuses.Last().status
            });

            return new JsonResult(objs);
        }

        // PRIVATE
        private long GetUnixMiliseconds(DateTime date)
        {
            return Convert.ToInt64((date - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds);
        }
    }
}