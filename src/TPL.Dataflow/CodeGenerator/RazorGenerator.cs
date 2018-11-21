using RazorLight;
using Repo;

namespace CodeGenerator
{
    class RazorGenerator
    {
        public static string GenerateFromRepoModel(IModel repoModel)
        {
            var model = ModelConverter.ConvertModelFromRepo(repoModel);
            /*var razorEngine = new RazorLightEngineBuilder()
                .UseFilesystemProject(System.IO.Path.GetFullPath(System.AppDomain.CurrentDomain.BaseDirectory))
                .UseMemoryCachingProvider()
                .Build();

            string result = razorEngine.CompileRenderAsync("template.cshtml", model).Result;*/
            var razorEngine = EngineFactory.CreatePhysical(System.IO.Path.GetFullPath(System.AppDomain.CurrentDomain.BaseDirectory));
            var result = razorEngine.Parse("template.cshtml", model);
            return result;
        }
    }
}
