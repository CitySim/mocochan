using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ModelConverter.Model;

namespace ModelConverter
{
	class Program
	{
		private static Config config;

		static void Main(string[] args)
		{
			Stopwatch watch = new Stopwatch();
			watch.Start();

			Program.config = new Config();
			bool argsOK = true;

			try
			{
				// TODO: load some optional global config file
				parseCliArgumenst(args);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				argsOK = false;
			}
	
			// alls things loaded
			if (Program.config.writeInfo)
			{
				string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
				Console.WriteLine("ModelConverter CLI v. {0}", version);
				Console.WriteLine();
			}

			// check if import and export is given
			if (Program.config.InputFiles.Count == 0)
			{
				Console.WriteLine("At least one input File is needed");
				Console.WriteLine();
				Program.config.writeHelp = true;
				argsOK = false;
			}

			if (string.IsNullOrWhiteSpace(Program.config.exportType))
			{
				Console.WriteLine("export Type (-e) must be set");
				Console.WriteLine();
				Program.config.writeHelp = true;
				argsOK = false;
			}

			if (Program.config.writeHelp)
			{
				Console.WriteLine("Basic Usage:");
				Console.WriteLine(" ModelConverter -e [options] inputFile.ext");
				Console.WriteLine();
				Console.WriteLine("List of avaible options:");
				Console.WriteLine(" -e     --exportType : Extension of File Type to the exported to.");
				Console.WriteLine("        --pluginDir  : Allows you to load Plugin from a Directory different than");
				Console.WriteLine("                       the current Folder.");
				Console.WriteLine(" -s     --scale      : Scale the model by Factor. 2 = double size, 0.5 half size");
				Console.WriteLine(" -o     --output     : Set output Folder. Default same as file origin.");

				Console.WriteLine();
			}

			if (!argsOK)
			{
				// a message shouls be already be printed
				return;
			}

			ConverterSettings settings = new ConverterSettings();
			settings.outputDir = Program.config.outputDir;
			settings.exportType = Program.config.exportType;

			Converter modelConverter = new Converter(settings, new ConsoleLogProvider());
			modelConverter.loadPlugins(Program.config.PluginDirectory);

			foreach (string file in Program.config.InputFiles)
			{
				modelConverter.Convert(file);
			}

			Console.WriteLine();
			Console.WriteLine("Completed in {0}", watch.Elapsed.ToString());

#if DEBUG
			Console.ReadKey();
#endif
		}

		private static Dictionary<string, string> parseCliArgumenst(string[] args)
		{
			Dictionary<string, string> parsedArgs = new Dictionary<string, string>();

			for (int i = 0; i < args.Length; i++)
			{
				if (args[i].StartsWith("-") && config.InputFiles.Count > 0)
				{
					throw new Exception("Invaled options, use --help or -h to see options");
				}

				switch (args[i])
				{
					case "--pluginDir":
						config.PluginDirectory = args[++i];
						break;

					case "-s":
					case "--scale":
						config.scaleFactor = double.Parse(args[++i]);
						break;

					case "-o":
					case "--output":
						string path = args[++i];

						while (path.StartsWith("\"") && !path.EndsWith("\""))
						{
							path += args[++i];
						}

						if (path.StartsWith("\"") && path.EndsWith("\""))
						{
							path = path.Substring(1, path.Length - 1);
						}

						config.outputDir = path;
						break;

					case "-e":
					case "--exportType":
						config.exportType = args[++i];
						break;

					default: // input
						if (args[i].StartsWith("-"))
						{
							throw new Exception("Invaled option " + args[i] + ", use --help or -h to see options");
						}

						string file = args[i];

						while (file.StartsWith("\"") && !file.EndsWith("\""))
						{
							file += args[++i];
						}

						if (file.StartsWith("\"") && file.EndsWith("\""))
						{
							file = file.Substring(1, file.Length - 1);
						}

						config.InputFiles.Add(file);
						break;
				}
			}

			return parsedArgs;
		}
	}
}
