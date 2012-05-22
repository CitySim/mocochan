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
				Console.WriteLine("--------------------------------------------------------------------------------");
			}

			if (Program.config.writePluginInfo)
			{
				Console.Write("╔══════════════════════╤═══════════╤═══════════╤═══════════╤═══════════════════╗");
				Console.Write("║ Name                 │ Geometry  │ Texure    │ Animation │                   ║");
				Console.Write("║                      │  R  │  W  │  R  │  W  │  R  │  W  │                   ║");
				Console.Write("╟──────────────────────┼─────┴─────┼─────┴─────┼─────┴─────┼───────────────────╢");
				bool first = true;
				foreach (IPlugin plugin in Program.config.Plugins)
				{
					if (!first)
					{
						Console.Write("╟──────────────────────┼─────┴─────┼─────┴─────┼─────┴─────┼───────────────────╢");
					}
				
					Console.Write("║ ");
					Console.Write(FillSpace(plugin.Name, 20));
					Console.Write(" │           │           │           │                  ");
					Console.Write(" ║");

					first = false;
				}
				Console.Write("╚══════════════════════╧═══════════╧═══════════╧═══════════╧═══════════════════╝");
			}

			Console.WriteLine("...");
			Console.ReadKey();
        }

		private static string FillSpace(string text, int length)
		{
			for (; text.Length < length; )
			{
				text = text + " ";
			}

			return text;
		}

		private static Dictionary<string, object> parseCliArgumenst(string[] args)
        {
            Dictionary<string, object> parsedArgs = new Dictionary<string, object>();

            for (int i = 0; i < args.Length; i++)
            {

            }

            return parsedArgs;
        }

        private static void parseArgumenst(Dictionary<string, object> parsedArgs)
        {
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
					Console.WriteLine("error loadin plugin");
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
