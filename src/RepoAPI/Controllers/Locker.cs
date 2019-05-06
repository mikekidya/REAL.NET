using System;
namespace RepoAPI.Controllers
{
    /// <summary>
    /// Really poor implementation for concurrency in controllers.
    /// </summary>
    public static class Locker
    {
        public static object obj = new object();
    }
}
