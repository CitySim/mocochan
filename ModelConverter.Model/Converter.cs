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
		public Dictionary<string, IPlugin> extensions = new Dictionary<string, IPlugin>();
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

					foreach (System.Type type in assembly.GetTypes())
					{
						if (type.IsPublic && !type.IsAbstract && type.GetInterface("ModelConverter.Model.IPlugin") != null)
						{
							IPlugin newPlugin = (IPlugin)Activator.CreateInstance(type);
							newPlugin.host = this;
							plugins.Add(newPlugin);

							foreach (string extension in newPlugin.fileExtensions.Keys)
							{
								extensions.Add(extension, newPlugin);
							}

							logProvider.Log(LogLevel.Info, "loaded Plugin-DLL: " + newPlugin.Name);
						}
					}
				}
				catch (Exception ex)
				{
					logProvider.Log(LogLevel.Error, "Couldn't load Plugin-DLL: " + ex.Message);
				}
			}
		}

		public void Convert(String file)
		{
			string targetPath = "";

			Convert(file, targetPath);
		}

		public void Convert(String file, String target)
		{
			logProvider.Log(LogLevel.Info, "─────────────────────────────────────────────────────────────────────");
			logProvider.Log(LogLevel.Info, "converting " + file);
			logProvider.Log(LogLevel.Info, "will be saved at " + target);

			// get plugin for import
			string fileExt = Path.GetExtension(file).Substring(1);
			if (!extensions.ContainsKey(fileExt))
			{
				logProvider.Log(LogLevel.Error, "no plugin found for " + fileExt);
				return;
			}

			IPlugin importPlugin = extensions[fileExt];

			BaseModel imported = importPlugin.Read(file);
		}
	}
}
