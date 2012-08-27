using System;
using System.Collections.Generic;
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
			Program.config = new Config();

			// TODO: load some optional global config file
            parseArgumenst(parseCliArgumenst(args));
            loadPlugins();

			// alls things loaded
			if (Program.config.writeInfo)
			{
				string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
				Console.WriteLine("ModelConverter CLI v. {0}", version);
				Console.WriteLine();
			}

			if (Program.config.writePluginInfo)
			{
				Console.WriteLine("List of loaded Plugin:");
				foreach (IPlugin plugin in Program.config.Plugins)
				{
					Console.WriteLine("- {0}", plugin.Name);
				}
				Console.WriteLine();
			}

			// check if import and export is given
			if (Program.config.InputFiles.Count == 0) 
			{
				Console.WriteLine("At least one input File is needed");
				Console.WriteLine();
				Program.config.writeHelp = true;
			}

			if (Program.config.writeHelp)
			{
				Console.WriteLine("Basic Usage:");
				Console.WriteLine(" ModelConverter [options] inputFile.ext");
				Console.WriteLine();
				Console.WriteLine("List of avaible options:");
				Console.WriteLine("        --pluginDir : Allows you to load Plugin from a Directory different than");
				Console.WriteLine("                      the current Folder.");
				Console.WriteLine(" -s     --scale     : Scale the model by Factor. 2 = double size, 0.5 half size");
				Console.WriteLine(" -o     --output    : Set output Folder. Default same as file origin.");
			}


        }

		private static Dictionary<string, string> parseCliArgumenst(string[] args)
        {
			Dictionary<string, string> parsedArgs = new Dictionary<string, string>();

            for (int i = 0; i < args.Length; i++)
            {
				if (args[i].Contains('='))
				{
					string[] splittedArg = args[i].Split('=');
					parsedArgs.Add(splittedArg[0], splittedArg[0]);
				}
				else
				{
					parsedArgs.Add("input", args[i]);
				}
            }

            return parsedArgs;
        }

        private static void parseArgumenst(Dictionary<string, string> parsedArgs)
        {
			foreach (KeyValuePair<string, string> arg in parsedArgs)
			{
				switch (arg.Key)
				{
					case "--pluginDir":
						config.PluginDirectory = arg.Value;
						break;

					case "-s":
					case "--scale":
						config.scaleFactor = double.Parse(arg.Value);
						break;

					case "-o":
					case "--output":
						config.Output = arg.Value;
						break;

					default: // input
						config.InputFiles.Add(arg.Value);
						break;
				}
			}
        }

        private static void loadPlugins()
        {
            foreach (string file in Directory.GetFiles(Program.config.PluginDirectory))
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
					Console.WriteLine("error loading plugin: {0}", ex.Message);
                }

                foreach (System.Type type in assembly.GetTypes())
                {
                    if (type.IsPublic && !type.IsAbstract && type.GetInterface("ModelConverter.Model.IPlugin") != null)
                    {
                        IPlugin newPlugin = (IPlugin)Activator.CreateInstance(type);
						Program.config.Plugins.Add(newPlugin);
                    }
                }
            }
        }
    }
}
