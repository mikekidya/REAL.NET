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
        /// <param name="path">Path of the file.</param>
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

        /// <summary>
        /// Changes the name of the model.
        /// </summary>
        /// <param name="modelName">Model name.</param>
        /// <param name="newName">New name.</param>
        [HttpPut("model/{modelName}/name/{newName}")]
        public void ChangeModelName(string modelName, string newName)
        {
            lock (lockObject)
            {
                GetModelFromRepo(modelName).Name = newName;
            }
        }

        /// <summary>
        /// Changes the model visability.
        /// </summary>
        /// <param name="modelName">Model name.</param>
        /// <param name="isVisible">New visability value (true/fasle).</param>
        [HttpPut("model/{modelName}/visability/{isVisible}")]
        public void ChangeModelVisability(string modelName, bool isVisible)
        {
            lock (lockObject)
            {
                GetModelFromRepo(modelName).IsVisible = isVisible;
            }
        }

        /// <summary>
        /// Removes the element from model.
        /// </summary>
        /// <param name="modelName">Model name.</param>
        /// <param name="elementId">Element identifier.</param>
        [HttpDelete("model/{modelName}/element/{elementId}")]
        public void DeleteElement(string modelName, int elementId)
        {
            lock (lockObject)
            {
                GetModelFromRepo(modelName).DeleteElement(
                    GetElementFromRepo(modelName, elementId));
            }
        }

        /// <summary>
        /// Changes the edge "from" element.
        /// </summary>
        /// <param name="modelName">Model name.</param>
        /// <param name="edgeId">Edge identifier.</param>
        /// <param name="elementId">"From" element identifier.</param>
        [HttpPut("model/{modelName}/edge/{edgeId}/from/{elementId}")]
        public void ChangeEdgeFrom(string modelName, int edgeId, int elementId)
        {
            lock (lockObject)
            {
                ((IEdge) GetElementFromRepo(modelName, edgeId)).From =
                    GetElementFromRepo(modelName, elementId);
            }
        }

        /// <summary>
        /// Changes the edge "to" element.
        /// </summary>
        /// <param name="modelName">Model name.</param>
        /// <param name="edgeId">Edge identifier.</param>
        /// <param name="elementId">"To" element identifier.</param>
        [HttpPut("model/{modelName}/edge/{edgeId}/to/{elementId}")]
        public void ChangeEdgeTo(string modelName, int edgeId, int elementId)
        {
            lock (lockObject)
            {
                ((IEdge)GetElementFromRepo(modelName, edgeId)).To =
                    GetElementFromRepo(modelName, elementId);
            }
        }

        /// <summary>
        /// Changes the name of the element.
        /// </summary>
        /// <param name="modelName">Model name.</param>
        /// <param name="elementId">Element identifier.</param>
        /// <param name="newName">New name.</param>
        [HttpPut("model/{modelName}/element/{elementId}/name/{newName}")]
        public void ChangeElementName(string modelName, int elementId, string newName)
        {
            lock (lockObject)
            {
                GetElementFromRepo(modelName, elementId).Name = newName;
            }
        }

        /// <summary>
        /// Adds the attribute into element.
        /// </summary>
        /// <param name="modelName">Model name.</param>
        /// <param name="elementId">Element identifier.</param>
        /// <param name="attributeName">Attribute name.</param>
        /// <param name="attributeKind">Attribute kind.</param>
        /// <param name="defaultValue">Default value.</param>
        [HttpPost("model/{modelName}/element/{elementId}/attribute/{attributeName}/{attributeKind}/{defaultValue}")]
        public void AddAttribute(string modelName, int elementId, 
            string attributeName, Models.AttributeKind attributeKind, string defaultValue)
        {
            lock (lockObject)
            {
                GetElementFromRepo(modelName, elementId).AddAttribute(
                    attributeName, 
                    _mapper.Map<Repo.AttributeKind>(attributeKind), 
                    defaultValue);
            }
        }

        /// <summary>
        /// Changes the attribute value.
        /// </summary>
        /// <param name="modelName">Model name.</param>
        /// <param name="elementId">Element identifier.</param>
        /// <param name="attributeName">Attribute name.</param>
        /// <param name="newValue">New value.</param>
        [HttpPut("model/{modelName}/element/{elementId}/attribute/{attributeName}/value/{newValue}")]
        public void ChangeAttributeValue(string modelName, int elementId,
            string attributeName, string newValue)
        {
            lock (lockObject)
            {
                //var infrastructure = new InfrastructureSemantic(RepoContainer.CurrentRepo());
                //var elem = GetElementFromRepo(modelName, elementId);
                //infrastructure.Element.SetAttributeValue(elem, attributeName, newValue);
                GetElementFromRepo(modelName, elementId)
                    .Attributes
                    .Where(attribute => (attribute.Name == attributeName))
                    .First()
                    .StringValue = newValue;
            }
        }

        /// <summary>
        /// Changes the attribute reference. (Not supported in current version)
        /// </summary>
        /// <param name="modelName">Model name.</param>
        /// <param name="elementId">Element identifier.</param>
        /// <param name="attributeName">Attribute name.</param>
        /// <param name="newReference">New reference element identifier.</param>
        [HttpPut("model/{modelName}/element/{elementId}/attribute/{attributeName}/reference/{newReference}")]
        public void ChangeAttributeReference(string modelName, int elementId,
            string attributeName, int newReference)
        {
            lock (lockObject)
            {
                GetElementFromRepo(modelName, elementId)
                    .Attributes
                    .Where(attribute => (attribute.Name == attributeName))
                    .First()
                    .ReferenceValue = GetElementFromRepo(modelName, newReference);
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
