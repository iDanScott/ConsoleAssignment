﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAssignment.Core
{
    public class PluginRepository
    {
        public static List<ICommandPlugin> Commands = new List<ICommandPlugin>();

        public static int LoadPlugins()
        {
            DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory);
            foreach (FileInfo fi in di.GetFiles("ConsoleAssignment.Plugins.*.dll"))
            {
                Assembly assembly = Assembly.LoadFrom(fi.FullName);
                foreach (Type t in assembly.GetTypes())
                {
                    if (typeof(ICommandPlugin).IsAssignableFrom(t))
                    {
                        ConstructorInfo ci = t.GetConstructor(new Type[] { });
                        ICommandPlugin plugin = (ICommandPlugin)ci.Invoke(new Object[] { });
                        Commands.Add(plugin);
                    }
                }
            }
            return Commands.Count;
        }

        public static void DisplayLoadedPlugins()
        {
            UserInteraction.ShowMessage(String.Format("Loaded {0} command plugins\n\r", Commands.Count));
            foreach (ICommandPlugin plugin in Commands)
            {
                IDescribable description = (IDescribable)plugin;
                UserInteraction.ShowMessage(String.Format($"{description.GetName()}\n\r{description.GetDescription()}\n\r-----------\n\r"));
            }
        }
    }
}
