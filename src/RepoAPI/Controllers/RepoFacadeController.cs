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

        /// <summary>
        /// Initializes a new instance of the <see cref="T:RepoAPI.Controllers.RepoFacadeController"/> class.
        /// </summary>
        /// <param name="mapper">Mapper is used to map object from Repo to Model classes.</param>
        public RepoFacadeController(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Return all visible model names in repository.
        /// </summary>
        /// <returns>The sequence of models names.</returns>
        [HttpGet("models")]
        public ActionResult<IEnumerable<string>> GetModels() =>
            RepoContainer.CurrentRepo()
            .Models
            .ToList()
            .ConvertAll<string>(model => model.Name);

        /// <summary>
        /// Returns model by its name.
        /// </summary>
        /// <returns>Model by its name.</returns>
        /// <param name="modelName">Model name.</param>
        [HttpGet("model/{modelName}")]
        public ActionResult<Model> Model(string modelName) =>
            _mapper.Map<Model>(RepoContainer.CurrentRepo().Model(modelName));

        /// <summary>
        /// Returns the node in model specified by its unique number key.
        /// </summary>
        /// <returns>The node.</returns>
        /// <param name="modelName">Model name.</param>
        /// <param name="id">Number key</param>
        [HttpGet("model/{modelName}/node/{id}")]
        public ActionResult<Node> GetNode(string modelName, int id) =>
            _mapper.Map<Node>((INode) GetElementFromRepo(modelName, id));

        /// <summary>
        /// Returns the edge in model specified by its unique number key.
        /// </summary>
        /// <returns>The edge.</returns>
        /// <param name="modelName">Model name.</param>
        /// <param name="id">Number key.</param>
        [HttpGet("model/{modelName}/edge/{id}")]
        public ActionResult<Edge> GetEdge(string modelName, int id) => 
            _mapper.Map<Edge>((IEdge) GetElementFromRepo(modelName, id));

        /// <summary>
        /// Returns the element in model specified by its unique number key.
        /// </summary>
        /// <returns>The element.</returns>
        /// <param name="modelName">Model name.</param>
        /// <param name="id">Number key.</param>
        [HttpGet("model/{modelName}/element/{id}")]
        public ActionResult<Element> GetElement(string modelName, int id) => 
            _mapper.Map<Element>(GetElementFromRepo(modelName, id));

        /// <summary>
        /// Creates new model from metamodel.
        /// </summary>
        /// <param name="metamodel">Metamodel name.</param>
        /// <param name="name">Name from new model. Should be unique.</param>
        [HttpPost("model/create/{metamodel}/{name}")]
        public void CreateModel(string metamodel, string name)
        {
            lock (lockObject)
            {
                RepoContainer.CurrentRepo().CreateModel(name, metamodel);
            }
        }

        /// <summary>
        /// Removes model from repository.
        /// </summary>
        /// <param name="modelName">Model name.</param>
        [HttpDelete("model/{modelName}")]
        public void DeleteModel(string modelName)
        {
            lock (lockObject)
            {
                IModel model = GetModelFromRepo(modelName);
                RepoContainer.CurrentRepo().DeleteModel(model);
            }
        }

        /// <summary>
        /// Saves repository into file.
        /// </summary>
        /// <param name="path">Path of the file./param>
        [HttpPost("save/{path}")]
        public void SaveRepo(string path)
        {
            lock (lockObject)
            {
                RepoContainer.CurrentRepo().Save(path);
            }
        }

        /// <summary>
        /// Creates new element in model by its parent.
        /// </summary>
        /// <returns>The element.</returns>
        /// <param name="modelName">Model name.</param>
        /// <param name="parentId">Parent identifier.</param>
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
