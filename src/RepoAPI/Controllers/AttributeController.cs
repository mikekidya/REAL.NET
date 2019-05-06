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
    /// Attribute controller is used to create, get and change attributes in elements. 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AttributeController : ControllerBase
    {
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:RepoAPI.Controllers.AttributeController"/> class.
        /// </summary>
        /// <param name="mapper">Mapper is used to map object from Repo to Model classes.</param>
        public AttributeController(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the attribute value.
        /// </summary>
        /// <returns>The attribute value.</returns>
        /// <param name="modelName">Model name.</param>
        /// <param name="elementId">Element identifier.</param>
        /// <param name="attributeName">Attribute name.</param>
        [HttpGet("{modelName}/{elementId}/{attributeName}")]
        public ActionResult<string> GetAttribute(string modelName, 
            int elementId, string attributeName)
        {
            return GetElementFromRepo(modelName, elementId)
                .Attributes
                .Where(attr => attr.Name == attributeName)
                .First()
                .StringValue;
        }


        /// <summary>
        /// Adds the attribute into element.
        /// </summary>
        /// <param name="modelName">Model name.</param>
        /// <param name="elementId">Element identifier.</param>
        /// <param name="attributeName">Attribute name.</param>
        /// <param name="attributeKind">Attribute kind.</param>
        /// <param name="defaultValue">Default value.</param>
        [HttpPost("{modelName}/{elementId}/{attributeName}/{attributeKind}/{defaultValue}")]
        public void AddAttribute(string modelName, int elementId,
            string attributeName, Models.AttributeKind attributeKind, string defaultValue)
        {
            lock (Locker.obj)
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
        [HttpPut("{modelName}/{elementId}/{attributeName}/value/{newValue}")]
        public void ChangeAttributeValue(string modelName, int elementId,
            string attributeName, string newValue)
        {
            lock (Locker.obj)
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
        [HttpPut("{modelName}/{elementId}/{attributeName}/reference/{newReference}")]
        public void ChangeAttributeReference(string modelName, int elementId,
            string attributeName, int newReference)
        {
            lock (Locker.obj)
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
