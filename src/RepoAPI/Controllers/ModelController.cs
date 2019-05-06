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
    /// Model controller is used to create, get, change and remove models.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ModelController : ControllerBase
    {
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:RepoAPI.Controllers.ModelController"/> class.
        /// </summary>
        /// <param name="mapper">Mapper is used to map object from Repo to Model classes.</param>
        public ModelController(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Return all visible model names in repository.
        /// </summary>
        /// <returns>The sequence of models names.</returns>
        [HttpGet("all")]
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
        [HttpGet("{modelName}")]
        public ActionResult<Model> Model(string modelName) =>
            _mapper.Map<Model>(RepoContainer.CurrentRepo().Model(modelName));

        /// <summary>
        /// Creates new model from metamodel.
        /// </summary>
        /// <param name="metamodel">Metamodel name.</param>
        /// <param name="name">Name from new model. Should be unique.</param>
        [HttpPost("{metamodel}/{name}")]
        public void CreateModel(string metamodel, string name)
        {
            lock (Locker.obj)
            {
                RepoContainer.CurrentRepo().CreateModel(name, metamodel);
            }
        }

        /// <summary>
        /// Changes the name of the model.
        /// </summary>
        /// <param name="modelName">Model name.</param>
        /// <param name="newName">New name.</param>
        [HttpPut("{modelName}/name/{newName}")]
        public void ChangeModelName(string modelName, string newName)
        {
            lock (Locker.obj)
            {
                GetModelFromRepo(modelName).Name = newName;
            }
        }

        /// <summary>
        /// Changes the model visability.
        /// </summary>
        /// <param name="modelName">Model name.</param>
        /// <param name="isVisible">New visability value (true/fasle).</param>
        [HttpPut("{modelName}/visability/{isVisible}")]
        public void ChangeModelVisability(string modelName, bool isVisible)
        {
            lock (Locker.obj)
            {
                GetModelFromRepo(modelName).IsVisible = isVisible;
            }
        }

        /// <summary>
        /// Removes model from repository.
        /// </summary>
        /// <param name="modelName">Model name.</param>
        [HttpDelete("{modelName}")]
        public void DeleteModel(string modelName)
        {
            lock (Locker.obj)
            {
                IModel model = GetModelFromRepo(modelName);
                RepoContainer.CurrentRepo().DeleteModel(model);
            }
        }

        private IModel GetModelFromRepo(string name) =>
            RepoContainer.CurrentRepo().Model(name);

    }
}
