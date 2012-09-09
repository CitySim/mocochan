using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModelConverter.Model;
using System.Reflection;
using System.IO;
using System.Globalization;

namespace ModelConverter.Plugin.X
{
    public class ModelX : IPlugin
    {
		public string Name { get { return "x DirectX"; } }
		public string Creator { get { return "Sven Tatter"; } }
		public string About { get { return "Support for x Files"; } }
		public Version PluginVersion { get { return Assembly.GetExecutingAssembly().GetName().Version; } }

		public IPluginHost host { get; set; }

        public Dictionary<string, string> fileExtensions
        {
            get
            {
                Dictionary<string, string> extensionsDic = new Dictionary<string, string>();
                extensionsDic.Add("x", "DirectX");
                return extensionsDic;
            }
        }

        public bool canRead { get { return false; } }
        public bool canWrite { get { return false; } }

        public BaseModel Read(string filePath)
        {
			throw new NotImplementedException();
        }

        public void Write(string filePath, BaseModel model)
        {
			throw new NotImplementedException();

            List<string> fileLines = new List<string>();

            fileLines.Add("xof 0302txt 0032");
            fileLines.Add("");
            fileLines.Add("//exported by ModelConverter");
            fileLines.Add("");

            fileLines.Add("Mesh {");

            // export vertices --------------------------------------------------------------------------------------------
            fileLines.Add(model.Vertices.Count.ToString() + ";");
            foreach (Vertex vertex in model.Vertices)
            {
                fileLines.Add(
                    vertex.Coordinate.X.ToString("0.000000", CultureInfo.GetCultureInfo("en-US")) +
                    ", " + vertex.Coordinate.Y.ToString("0.000000", CultureInfo.GetCultureInfo("en-US")) +
                    ", " + vertex.Coordinate.Z.ToString("0.000000", CultureInfo.GetCultureInfo("en-US")) +
                    ";,"
                    );
            }
            fileLines[fileLines.Count - 1] =
                fileLines[fileLines.Count - 1].Substring(0, fileLines[fileLines.Count - 1].Length - 1);
            fileLines[fileLines.Count - 1] += ";";
            fileLines.Add("");
            // export polygons --------------------------------------------------------------------------------------------
            fileLines.Add(model.Polygons.Count.ToString() + ";");
            foreach (Polygon polygon in model.Polygons)
            {
                fileLines.Add(
                    polygon.Point1Id.ToString("0.000000", CultureInfo.GetCultureInfo("en-US")) +
                    ", " + polygon.Point2Id.ToString("0.000000", CultureInfo.GetCultureInfo("en-US")) +
                    ", " + polygon.Point3Id.ToString("0.000000", CultureInfo.GetCultureInfo("en-US")) +
                    ";,"
                    );
            }
            fileLines[fileLines.Count - 1] =
                fileLines[fileLines.Count - 1].Substring(0, fileLines[fileLines.Count - 1].Length - 1);
            fileLines[fileLines.Count - 1] += ";";
            fileLines.Add("");
            // completed export --------------------------------------------------------------------------------------------
            fileLines.Add("}");

            File.WriteAllLines(filePath, fileLines);
        }
    }
}
 
