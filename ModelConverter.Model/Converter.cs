using System;
using System.Collections.Generic;
using System.Diagnostics;
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

		public ConverterSettings settings;

		public Converter(ConverterSettings settings, ILogProvider logProvider)
		{
			this.settings = settings;
			this.logProvider = logProvider;
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

		public string getTargetPath(string file)
		{
			if (string.IsNullOrWhiteSpace(settings.outputDir))
			{
				// simply replace extension
				return Path.ChangeExtension(file, settings.exportType);
			}
			else
			{
				// output dir is set
				return Path.Combine(settings.outputDir, Path.GetFileNameWithoutExtension(file) + "." + settings.exportType);
			}
		}

		public void Convert(String importFile)
		{
			Convert(importFile, getTargetPath(importFile));
		}

		public void Convert(String importFile, String exportFile)
		{
			Stopwatch watch = new Stopwatch();
			watch.Start();

			logProvider.Log(LogLevel.Info, "─────────────────────────────────────────────────────────────────────");
			logProvider.Log(LogLevel.Info, "converting " + importFile);
			logProvider.Log(LogLevel.Info, "will be saved at " + exportFile);

			// a short check for valid paths
			try
			{
				Path.GetFileName(importFile);
				Path.GetFileName(exportFile);

				if (importFile.Length == 0 || exportFile.Length == 0)
				{
					throw new Exception("Paths need to have a length");
				}
			}
			catch (Exception ex)
			{
				logProvider.Log(LogLevel.Error, "seems like a invalid file path\n" + ex.ToString());
				return;
			}

			// get plugin for import
			string importExt = Path.GetExtension(importFile).Substring(1);
			if (!extensions.ContainsKey(importExt))
			{
				logProvider.Log(LogLevel.Error, "no plugin found for " + importExt);
				return;
			}

			IPlugin importPlugin = extensions[importExt];

			if (!importPlugin.canRead)
			{
				logProvider.Log(LogLevel.Error, string.Format("Import-Plugin {0} cant read files", importPlugin.Name));
				return;
			}

			// get plugin for export
			string exportExt = Path.GetExtension(exportFile).Substring(1);
			if (!extensions.ContainsKey(exportExt))
			{
				logProvider.Log(LogLevel.Error, "no plugin found for " + exportExt);
				return;
			}

			IPlugin exportPlugin = extensions[exportExt];
			
			if (!importPlugin.canWrite)
			{
				logProvider.Log(LogLevel.Error, string.Format("Export-Plugin {0} cant write files", importPlugin.Name));
				return;
			}

			// convertings starts here
			logProvider.Log(LogLevel.Info, "reading " + importPlugin.fileExtensions[importExt] + " (" + importExt + ")");
			BaseModel imported;
			try
			{
				imported = importPlugin.Read(importFile);
			}
			catch (Exception ex)
			{
				logProvider.Log(LogLevel.Error, "exception while reading File\n" + ex.ToString());
				return;
			}

			// processing
			if (settings.scaleFactor != 1.0f)
			{
				imported.Scale(settings.scaleFactor);
			}

			logProvider.Log(LogLevel.Info, "writing " + exportPlugin.fileExtensions[exportExt] + " (" + exportExt + ")");
			try
			{
				exportPlugin.Write(exportFile, imported);
			}
			catch (Exception ex)
			{
				logProvider.Log(LogLevel.Error, "exception while writing File\n" + ex.ToString());
				return;
			}

			// log time
			watch.Stop();
			logProvider.Log(LogLevel.Info, "completed in " + watch.Elapsed.ToString());
		}
	}
}
