﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Organization.API.Handlers;

namespace Organization.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ApiController
    {
        public ValuesController(
            INotificationHandler notificationHandler) : base(notificationHandler)
        {
        }
        
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            var a = Request.Headers;
            var b = a["unique_name"];
            return new string[] { "value1", "value2" };
        }
    }
}
