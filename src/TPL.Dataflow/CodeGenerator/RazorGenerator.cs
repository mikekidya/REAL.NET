using RazorLight;
using Repo;

namespace CodeGenerator
{
    public class RazorGenerator
    {
        public static string GenerateFromRepoModel(IModel repoModel)
        {
            var model = ModelConverter.ConvertModelFromRepo(repoModel);
            return GenerateFromModel(model);
        }

        public static string GenerateFromModel(Model model)
        {
            var razorEngine = EngineFactory.CreatePhysical(System.IO.Path.GetFullPath(System.AppDomain.CurrentDomain.BaseDirectory));
            var result = razorEngine.Parse("template.cshtml", model);
            return result;
        }
    }
}
