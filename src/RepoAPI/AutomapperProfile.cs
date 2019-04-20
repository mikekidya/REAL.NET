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

            CreateMap<IElement, Element>();

            CreateMap<IModel, Model>()
                .ForMember(x => x.MetamodelName, x => x.MapFrom(y => y.Metamodel.Name));
        }
    }
}
