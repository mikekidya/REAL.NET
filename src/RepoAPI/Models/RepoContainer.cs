using Repo;

namespace RepoAPI.Models
{
    public static class RepoContainer
    {
        public static IRepo repo { get; private set; }

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
