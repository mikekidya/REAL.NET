using AutoMapper;
using Repo;
using RepoAPI.Models;

namespace RepoAPI
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<IModel, Model>()
                .ForMember(x => x.MetamodelName, x => x.MapFrom(y => y.Metamodel.Name));
        }
    }
}
