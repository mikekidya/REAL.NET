using Repo;

namespace RepoAPI.Models
{
    public static class RepoContainer
    {
        private static IRepo repo;

        public static void Create()
        {
            repo = RepoFactory.Create();
        }

        public static void Load(string path)
        {
            repo = RepoFactory.Load(path);
        }

        public static IRepo CurrentRepo()
        {
            if (repo is null)
            {
                Create();
            }
            return repo;
        }

    }
}
