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

			Console.ReadKey();
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
						config.PluginDirectory = double.Parse(arg.Value);
						break;

					default: // input-output
						if (config.Output != string.Empty)
						{
							config.InputFiles.Add(config.Output);
						}
						config.Output = arg.Value;
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
					Console.WriteLine("error loading plugin");
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
