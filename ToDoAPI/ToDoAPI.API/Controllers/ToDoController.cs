using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ToDoAPI.API.Models; //added for access to DTO's
using ToDoAPI.DATA.EF; //added for access to EF
using System.Web.Http.Cors; //added for access to modify the CORS for this controller specifically

namespace ToDoAPI.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ToDoController : ApiController
    {
    }
}
