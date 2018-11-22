using RazorLight;
using Repo;

namespace CodeGenerator
{
    /// <summary>
    /// Class represents methods generating C# sourse code from models
    /// </summary>
    public class RazorGenerator
    {
        /// <summary>
        /// Generates C# source code from Repo model
        /// </summary>
        public static string GenerateFromRepoModel(IModel repoModel)
        {
            var model = ModelConverter.ConvertModelFromRepo(repoModel);
            return GenerateFromModel(model);
        }

        /// <summary>
        /// Generates C# source code from generator model
        /// </summary>
        public static string GenerateFromModel(Model model)
        {
            var razorEngine = EngineFactory.CreatePhysical(System.IO.Path.GetFullPath(System.AppDomain.CurrentDomain.BaseDirectory));
            var result = razorEngine.Parse("template.cshtml", model);
            return result;
        }
    }
}
