﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace PluginLibrary
{
    /// <summary>
    /// Class that is responsible for launching and keeping plugins
    /// </summary>
    public class PluginLauncher
    {
        /// <summary>
        /// Gets list of available plugins 
        /// </summary>
        public IList<IPlugin> Plugins => pluginsList;

        /// <summary>
        /// Gets list of plugins
        /// </summary>
        private List<IPlugin> pluginsList = new List<IPlugin>();
       
        /// <summary>
        /// Launch plugins from this directory
        /// </summary>
        /// <param name="path">Directory with plugins</param>
        /// <exception cref="DirectoryNotFoundException">This directory does not exist</exception>
        /// <exception cref="ArgumentException">Path is just spaces or null string</exception>
        /// <exception cref="PathTooLongException">Path is more than 260 digits</exception>
        public void LaunchPlugins(string path)
        {
            var files = Directory.GetFiles(path, "*Plugin*.dll");
            var assemblies = new List<Assembly>();
            foreach (var file in files)
            {
                try
                {
                    var assembly = Assembly.LoadFrom(file);
                    assemblies.Add(assembly);
                }
                catch (FileLoadException){}
            }
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    if (type.IsAbstract)
                    {
                        continue;
                    }
                    var interfaces = type.GetInterfaces();
                    foreach (var interFace in interfaces)
                    {
                        if (interFace.Name == "IPlugin")
                        {
                            var constructor = type.GetConstructor(Type.EmptyTypes);
                            if (constructor == null)
                            {
                                continue;
                            }
                            object almostPlugin = Activator.CreateInstance(type);
                            //TODO:create instance with invoked constructor object almostPlugin = constructor.Invoke(new object[] { });
                            var plugin = almostPlugin as IPlugin;
                            pluginsList.Add(plugin);
                        }
                    }
                }
            }
        }
    }
}