using RazorLight;
using Repo;

namespace CodeGenerator
{
    class RazorGenerator
    {
        public static string GenerateFromRepoModel(IModel repoModel)
        {
            var model = ModelConverter.ConvertModelFromRepo(repoModel);
            var razorEngine = EngineFactory.CreatePhysical(System.IO.Path.GetFullPath(System.AppDomain.CurrentDomain.BaseDirectory));
            return razorEngine.Parse("template.cshtml", model);
        }
    }
}
