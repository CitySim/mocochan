using MocoChan.Converter.Data;
using MocoChan.Converter.Logging;
using MocoChan.Converter.PluginInterface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace MocoChan.Converter
{
	public class MocoConverter : IPluginHost
	{
		/// <summary>
		/// List of al loaded Plugins
		/// </summary>
		public List<IPlugin> plugins = new List<IPlugin>();
		public Dictionary<string, IPlugin> extensions = new Dictionary<string, IPlugin>();

		public ILogProvider LogProvider
		{
			get { return this.Settings.LogProvider; }
			set { this.Settings.LogProvider = value; }
		}

		public MocoSettings Settings;

		public MocoConverter() : this(new MocoSettings())
		{
		}

		public MocoConverter(MocoSettings settings)
		{
			this.Settings = settings;
		}

		public void LoadPlugins(string loadDir)
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
						if (type.IsPublic && !type.IsAbstract && type.GetInterface("MocoChan.Converter.PluginInterface.IPlugin") != null)
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

							LogProvider.Log(LogLevel.Info, "loaded Plugin-DLL: " + newPlugin.Name);
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
						LogProvider.Log(LogLevel.Error, "Couldn't load Plugin-DLL: " + file);
						LogProvider.Log(LogLevel.Error, "The DLL-File locks blockes (HRESULT 0x80131515)");
					}
					else
					{
						throw ex;
					}
				}
				catch (Exception ex)
				{
					LogProvider.Log(LogLevel.Error, "Couldn't load Plugin-DLL: " + ex.Message);
				}
			}
		}

		public Model Convert(String input, String output)
		{
			return Convert(input, output, new ConvertSettings());
		}

		public Model Convert(String input, String output, ConvertSettings settings)
		{
			LogProvider.Log(LogLevel.Info, "converting " + input);
			LogProvider.Log(LogLevel.Info, "will be saved at " + output);

			// TODO: guess importer/exporter to use by file extension if not set
			if (settings.Importer == null)
			{
			}
			if (settings.Exporter == null)
			{
			}

			Stream streamInput = File.Open(input, FileMode.Open, FileAccess.Read);
			Stream streamOutput = File.Open(output, FileMode.Create, FileAccess.Write);
			return Convert(streamInput, streamOutput, settings);
		}

		public Model Convert(Stream input, Stream output, ConvertSettings settings)
		{
			Stopwatch watch = new Stopwatch();
			watch.Start();

			Model model = settings.Importer.Read(input);

			// processing
			if (settings.ScaleFactor != 1.0f)
				model.Scale(settings.ScaleFactor);
			if (settings.RecalculateNormals)
				model.RecalculateNormals();

			settings.Exporter.Write(model, output);

			// log time
			watch.Stop();
			LogProvider.Log(LogLevel.Info, "completed in " + watch.Elapsed.ToString());

			return model;
		}
	}
}
