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
    /// Element controller is used to create, get, change and remove elements.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ElementController : ControllerBase
    {
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:RepoAPI.Controllers.ElementController"/> class.
        /// </summary>
        /// <param name="mapper">Mapper is used to map object from Repo to Model classes.</param>
        public ElementController(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Returns the element in model specified by its unique number key.
        /// </summary>
        /// <returns>The element.</returns>
        /// <param name="modelName">Model name.</param>
        /// <param name="id">Number key.</param>
        [HttpGet("{modelName}/{id}")]
        public ActionResult<Element> GetElement(string modelName, int id) =>
            _mapper.Map<Element>(GetElementFromRepo(modelName, id));

        /// <summary>
        /// Returns the node in model specified by its unique number key.
        /// </summary>
        /// <returns>The node.</returns>
        /// <param name="modelName">Model name.</param>
        /// <param name="id">Number key</param>
        [HttpGet("{modelName}/{id}/asNode")]
        public ActionResult<Node> GetNode(string modelName, int id) =>
            _mapper.Map<Node>((INode)GetElementFromRepo(modelName, id));


        /// <summary>
        /// Returns the edge in model specified by its unique number key.
        /// </summary>
        /// <returns>The edge.</returns>
        /// <param name="modelName">Model name.</param>
        /// <param name="id">Number key.</param>
        [HttpGet("{modelName}/{id}/asEdge")]
        public ActionResult<Edge> GetEdge(string modelName, int id) =>
            _mapper.Map<Edge>((IEdge)GetElementFromRepo(modelName, id));


        /// <summary>
        /// Creates new element in model by its parent.
        /// </summary>
        /// <returns>The element.</returns>
        /// <param name="modelName">Model name.</param>
        /// <param name="parentId">Parent identifier.</param>
        [HttpPost("{modelName}/{parentId}")]
        public ActionResult<int> CreateElement(string modelName, int parentId)
        {
            lock (Locker.obj)
            {
                IModel meta = GetModelFromRepo(modelName).Metamodel;
                IElement parentElement = GetElementFromRepo(meta.Name, parentId);
                IElement result = GetModelFromRepo(modelName).CreateElement(parentElement);
                return result.Id;
            }
        }


        /// <summary>
        /// Changes the name of the element.
        /// </summary>
        /// <param name="modelName">Model name.</param>
        /// <param name="elementId">Element identifier.</param>
        /// <param name="newName">New name.</param>
        [HttpPut("{modelName}/{elementId}/name/{newName}")]
        public void ChangeElementName(string modelName, int elementId, string newName)
        {
            lock (Locker.obj)
            {
                GetElementFromRepo(modelName, elementId).Name = newName;
            }
        }

        /// <summary>
        /// Changes the edge "from" element.
        /// </summary>
        /// <param name="modelName">Model name.</param>
        /// <param name="edgeId">Edge identifier.</param>
        /// <param name="elementId">"From" element identifier.</param>
        [HttpPut("{modelName}/{edgeId}/from/{elementId}")]
        public void ChangeEdgeFrom(string modelName, int edgeId, int elementId)
        {
            lock (Locker.obj)
            {
                ((IEdge)GetElementFromRepo(modelName, edgeId)).From =
                    GetElementFromRepo(modelName, elementId);
            }
        }

        /// <summary>
        /// Changes the edge "to" element.
        /// </summary>
        /// <param name="modelName">Model name.</param>
        /// <param name="edgeId">Edge identifier.</param>
        /// <param name="elementId">"To" element identifier.</param>
        [HttpPut("{modelName}/{edgeId}/to/{elementId}")]
        public void ChangeEdgeTo(string modelName, int edgeId, int elementId)
        {
            lock (Locker.obj)
            {
                ((IEdge)GetElementFromRepo(modelName, edgeId)).To =
                    GetElementFromRepo(modelName, elementId);
            }
        }


        /// <summary>
        /// Removes the element from model.
        /// </summary>
        /// <param name="modelName">Model name.</param>
        /// <param name="elementId">Element identifier.</param>
        [HttpDelete("{modelName}/{elementId}")]
        public void DeleteElement(string modelName, int elementId)
        {
            lock (Locker.obj)
            {
                GetModelFromRepo(modelName).DeleteElement(
                    GetElementFromRepo(modelName, elementId));
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
