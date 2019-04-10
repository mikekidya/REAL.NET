using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

using Repo;
using RepoAPI.Models;

namespace RepoAPI.Controllers
{ 
    [Route("api/[controller]")]
    [ApiController]
    public class RepoFacadeController : ControllerBase
    { 
        [HttpGet("models")]
        public ActionResult<IEnumerable<string>> Get()
        {
            var models = RepoContainer.CurrentRepo().Models;
            return models.ToList().ConvertAll<string>((model) => model.Name);
        }
    }
}
