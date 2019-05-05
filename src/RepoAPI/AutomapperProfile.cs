using AutoMapper;
using Repo;
using RepoAPI.Models;
using System.Linq;

namespace RepoAPI
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<IAttribute, Attribute>();
            CreateMap<IElement, ElementInfo>();
            CreateMap<IElement, Element>();
            CreateMap<INode, Node>();
            CreateMap<IEdge, Edge>();
            CreateMap<IModel, Model>()
                .ForMember(x => x.MetamodelName, x => x.MapFrom(y => y.Metamodel.Name));
        }
    }
}
