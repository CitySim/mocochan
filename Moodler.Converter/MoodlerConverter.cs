using Moodler.Converter.Data;
using Moodler.Converter.Logging;
using Moodler.Converter.PluginInterface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Moodler.Converter
{
	public class MoodlerConverter : IPluginHost
	{
		/// <summary>
		/// List of al loaded Plugins
		/// </summary>
		public List<IPlugin> plugins = new List<IPlugin>();
		public Dictionary<string, IPlugin> extensions = new Dictionary<string, IPlugin>();
		public ILogProvider logProvider { get; set; }

		public ConverterSettings settings;

		public MoodlerConverter() : this(new ConverterSettings())
		{
		}

		public MoodlerConverter(ConverterSettings settings)
		{
			this.settings = settings;
			this.logProvider = settings.LogProvider;
		}

		public void loadPlugins(string loadDir)
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			foreach (string file in Directory.GetFiles(loadDir))
			{
				// skip non dll files
				if (!System.IO.Path.GetFileName(file).EndsWith(".dll"))
					continue;

				try
				{
					assembly = Assembly.LoadFrom(file);

					foreach (System.Type type in assembly.GetTypes())
					{
						if (type.IsPublic && !type.IsAbstract && type.GetInterface("Moodler.Converter.IPlugin") != null)
						{
							// create instance of plugin
							IPlugin newPlugin = (IPlugin)Activator.CreateInstance(type);
							newPlugin.Host = this;
							plugins.Add(newPlugin);

							/*
							foreach (string extension in newPlugin.FileExtensions.Keys)
							{
								extensions.Add(extension, newPlugin);
							}
							*/

							logProvider.Log(LogLevel.Info, "loaded Plugin-DLL: " + newPlugin.Name);
						}
					}
				}
				catch (FileLoadException ex)
				{
					// check if exception is caused by blocked dll
					// Could not load file or assembly '<file>' or one of its dependencies.
					// Operation is not supported. (Exception from HRESULT: 0x80131515)
					if (ex.Message.Contains("0x80131515"))
					{
						logProvider.Log(LogLevel.Error, "Couldn't load Plugin-DLL: " + file);
						logProvider.Log(LogLevel.Error, "The DLL-File locks blockes (HRESULT 0x80131515)");
					}
					else
					{
						throw ex;
					}
				}
				catch (Exception ex)
				{
					logProvider.Log(LogLevel.Error, "Couldn't load Plugin-DLL: " + ex.Message);
				}
			}
		}
		public Model Convert(String importFile, String exportFile)
		{
			return Convert(importFile, exportFile, new ConvertSettings());
		}

		public Model Convert(String importFile, String exportFile, ConvertSettings settings)
		{
			logProvider.Log(LogLevel.Info, "converting " + importFile);
			logProvider.Log(LogLevel.Info, "will be saved at " + exportFile);
			
			IImporter importer = null;
			IExporter exporter = null;

			Stream fileIn = File.OpenRead(importFile);
			Stream fileOut = File.Open(exportFile, FileMode.Create, FileAccess.Write);
			return Convert(fileIn, importer, fileOut, exporter, settings);
		}

		public Model Convert(Stream input, IImporter importer, Stream output, IExporter exporter, ConvertSettings settings)
		{
			Stopwatch watch = new Stopwatch();
			watch.Start();

			Model model = importer.Read(input);

			// processing
			if (settings.ScaleFactor != 1.0f)
				model.Scale(settings.ScaleFactor);
			if (settings.RecalculateNormals)
				model.RecalculateNormals();

			exporter.Write(model, output);

			// log time
			watch.Stop();
			logProvider.Log(LogLevel.Info, "completed in " + watch.Elapsed.ToString());

			return model;
		}
	}
}
