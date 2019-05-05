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
        private readonly object lockObject = new Object();

        public RepoFacadeController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet("models")]
        public ActionResult<IEnumerable<string>> GetModels() =>
            RepoContainer.CurrentRepo()
            .Models
            .ToList()
            .ConvertAll<string>(model => model.Name);

        [HttpGet("model/{modelName}")]
        public ActionResult<Model> Model(string modelName) =>
            _mapper.Map<Model>(RepoContainer.CurrentRepo().Model(modelName));

        [HttpGet("model/{modelName}/node/{id}")]
        public ActionResult<Node> GetNode(string modelName, int id) =>
            _mapper.Map<Node>((INode) GetElementFromRepo(modelName, id));

        [HttpGet("model/{modelName}/edge/{id}")]
        public ActionResult<Edge> GetEdge(string modelName, int id) => 
            _mapper.Map<Edge>((IEdge) GetElementFromRepo(modelName, id));

        [HttpGet("model/{modelName}/element/{id}")]
        public ActionResult<Element> GetElement(string modelName, int id) => 
            _mapper.Map<Element>(GetElementFromRepo(modelName, id));

        [HttpPost("model/create/{metamodel}/{name}")]
        public void CreateModel(string metamodel, string name)
        {
            lock (lockObject)
            {
                RepoContainer.CurrentRepo().CreateModel(name, metamodel);
            }
        }

        [HttpDelete("model/{modelName}")]
        public void DeleteModel(string modelName)
        {
            lock (lockObject)
            {
                IModel model = GetModelFromRepo(modelName);
                RepoContainer.CurrentRepo().DeleteModel(model);
            }
        }

        [HttpPost("save/{path}")]
        public void SaveRepo(string path)
        {
            lock (lockObject)
            {
                RepoContainer.CurrentRepo().Save(path);
            }
        }

        [HttpPost("model/{modelName}/element/create/{parentId}")]
        public ActionResult<int> CreateElement(string modelName, int parentId)
        {
            lock (lockObject)
            {
                //throw new NullReferenceException();
                IModel meta = GetModelFromRepo(modelName).Metamodel;
                IElement parentElement = GetElementFromRepo(meta.Name, parentId);
                IElement result = GetModelFromRepo(modelName).CreateElement(parentElement);
                return result.Id;
            }
        }

        private IElement GetElementFromRepo(string modelName, int id) =>
             GetModelFromRepo(modelName)
             .Elements
             .Where(elem => (elem.Id == id))
             .First();

        private IModel GetModelFromRepo(string name) =>
            RepoContainer.CurrentRepo().Model(name);


    }
}
