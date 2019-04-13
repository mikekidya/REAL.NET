using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using AutoMapper;

using Repo;
using RepoAPI.Models;

namespace RepoAPI.Controllers
{ 
    [Route("api/[controller]")]
    [ApiController]
    public class RepoFacadeController : ControllerBase
    {
        private readonly IMapper _mapper;

        public RepoFacadeController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet("modelsNames")]
        public ActionResult<IEnumerable<string>> Get()
        {
            var models = RepoContainer.CurrentRepo().Models;
            return models.ToList().ConvertAll<string>((model) => model.Name);
        }

        [HttpGet("models/{modelName}/isVisible")]
        public ActionResult<bool> IsModelVisible(string modelName)
        {
            return RepoContainer.CurrentRepo().Model(modelName).IsVisible;
        }

        [HttpGet("models/{modelName}")]
        public ActionResult<Model> Model(string modelName)
        {
            
            return _mapper.Map<Model>(RepoContainer.CurrentRepo().Model(modelName));
        }

        [HttpGet("models/{modelName}/metamodelName")]
        public ActionResult<string> MetamodelName(string modelName)
        {
            return RepoContainer.CurrentRepo().Model(modelName).Metamodel.Name;
        }



    }
}
