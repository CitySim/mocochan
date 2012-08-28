using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ModelConverter.Model
{
	public class Converter : IPluginHost
	{
		public List<IPlugin> plugins = new List<IPlugin>();
		public ILogProvider logProvider { get; set; }

		public Converter()
		{
			
		}

		public void loadPlugins(string loadDir)
		{
			foreach (string file in Directory.GetFiles(loadDir))
			{
				if (!System.IO.Path.GetFileName(file).EndsWith(".dll"))
				{
					continue;
				}

				Assembly assembly = Assembly.GetExecutingAssembly();
				try
				{
					assembly = Assembly.LoadFrom(file);
				}
				catch (Exception ex)
				{
					logProvider.Log(LogLevel.Error, "Couldn't load Plugin-DLL: " + ex.Message);
				}

				foreach (System.Type type in assembly.GetTypes())
				{
					if (type.IsPublic && !type.IsAbstract && type.GetInterface("ModelConverter.Model.IPlugin") != null)
					{
						IPlugin newPlugin = (IPlugin)Activator.CreateInstance(type);
						newPlugin.host = this;
						plugins.Add(newPlugin);

						logProvider.Log(LogLevel.Info, "loaded Plugin-DLL: " + newPlugin.Name);
					}
				}
			}
		}
	}
}
