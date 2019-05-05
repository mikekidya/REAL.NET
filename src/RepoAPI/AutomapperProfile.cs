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
            CreateMap<Repo.Metatype, Models.Metatype>().ReverseMap();
            CreateMap<Repo.AttributeKind, Models.AttributeKind>().ReverseMap();
            CreateMap<IAttribute, Attribute>().ReverseMap();
            CreateMap<IElement, ElementInfo>();
            CreateMap<IElement, Element>();
            CreateMap<INode, Node>();
            CreateMap<IEdge, Edge>();
            CreateMap<IModel, Model>()
                .ForMember(x => x.MetamodelName, x => x.MapFrom(y => y.Metamodel.Name));
        }
    }
}
