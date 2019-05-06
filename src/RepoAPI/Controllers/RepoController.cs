using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using AutoMapper;

using Repo;
using RepoAPI.Models;

namespace RepoAPI.Controllers
{ 
    /// <summary>
    /// Repo controller is used to controll the whole repository.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RepoController : ControllerBase
    {

        /// <summary>
        /// Saves repository into file.
        /// </summary>
        /// <param name="path">Path of the file.</param>
        [HttpPost("save/{path}")]
        public void SaveRepo(string path)
        {
            lock (Locker.obj)
            {
                RepoContainer.CurrentRepo().Save(path);
            }
        }
    }
}
