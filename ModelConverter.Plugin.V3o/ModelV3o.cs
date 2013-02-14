using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModelConverter.Model;
using System.IO;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Globalization;

namespace ModelConverter.Plugin.V3o
{
    public class ModelV3o : IPlugin
    {
		public string Name { get { return "v3o Emergency 3 & 4"; } }
		public string Creator { get { return "Sven Tatter"; } }
		public string About { get { return "Support for v3o used by Emergency3 and Emergncy4"; } }
		public Version PluginVersion { get { return Assembly.GetExecutingAssembly().GetName().Version; } }

		public IPluginHost host { get; set; }
		
        public Dictionary<string, string> fileExtensions
        {
            get
            {
                Dictionary<string, string> extensionsDic = new Dictionary<string, string>();
                extensionsDic.Add("v3o", "Emergency 3 & 4");
                return extensionsDic;
            }
        }

        public bool canRead { get { return true; } }
        public bool canWrite { get { return true; } }

        public BaseModel Read(string filePath)
        {
			// check for packed file
			FileStream fileStream = File.OpenRead(filePath);
			if (fileStream.ReadByte() == 0xd3 && fileStream.ReadByte() == 0xc0)
			{
				host.logProvider.Log(LogLevel.Fatal, "File looks packed. Please unpack first with Emergency 4 Editor.");
				fileStream.Close();
				return new BaseModel();
			}
			fileStream.Close();

            string[] fileLines = File.ReadAllLines(filePath);
            BaseModel model = new BaseModel();

            for (int i = 0; i < fileLines.Length; i++)
            {
                string line = fileLines[i].Trim();
                if (line == String.Empty)
                    continue; // yeah, emtpy line
                if (line.StartsWith("//"))
                    continue; // a comment
                if (line.StartsWith("["))
                    continue; // some Meta Info, we dont read

                string[] splittedLine = line.Split(',');

                ModelAnimationFrame activeFrame = null;

                switch (splittedLine[0])
                {
                    case "SRF":
                        if (splittedLine.Length != 19)
                        {
							host.logProvider.Log(LogLevel.Warning, "Malformed Material in Line " + i);
                            continue;
                        }
                        Material material = new Material();
                        material.Name = splittedLine[1].Trim();
                        material.TextureFile = splittedLine[5].Trim();
                        model.Materials.Add(material.Name, material);
                        break;

                    case "D":
                        if (splittedLine.Length != 13)
                        {
                            host.logProvider.Log(LogLevel.Warning, "Malformed Vertex in Line " + i);
                            continue;
                        }
                        Vertex vertex = new Vertex();
                        vertex.Coordinate = new Vector3(
                                Convert.ToInt32(splittedLine[2]),
                                Convert.ToInt32(splittedLine[3]),
                                Convert.ToInt32(splittedLine[1])
                                );
                        vertex.Normals = new Vector3(
                                float.Parse(splittedLine[5]) / 256.0f,
                                float.Parse(splittedLine[6]) / 256.0f,
                                float.Parse(splittedLine[4]) / 256.0f
                                );
                        vertex.TextureCoordinate = new Vector2(
                                float.Parse(splittedLine[7]) / 1024.0f,
                                float.Parse(splittedLine[8]) / 1024.0f
                                );
                        model.Vertices.Add(vertex);
                        break;

                    case "BP":
                        if (splittedLine.Length != 9)
                        {
                            host.logProvider.Log(LogLevel.Warning, "Malformed \"BP\" in Line " + i);
                            continue;
                        }
                        host.logProvider.Log(LogLevel.Warning, "Node that BP-Lines are not recognized");
                        break;

                    case "P":
                        if (splittedLine.Length != 21)
                        {
                            host.logProvider.Log(LogLevel.Warning, "Malformed Polygon in Line " + i);
                            continue;
                        }
                        Polygon polygon = new Polygon(model);
                        polygon.Point1Id = Convert.ToInt32(splittedLine[4]) - 1;
                        polygon.Point2Id = Convert.ToInt32(splittedLine[3]) - 1;
                        polygon.Point3Id = Convert.ToInt32(splittedLine[2]) - 1;
                        polygon.materialId = model.Materials.ElementAt(Convert.ToInt32(splittedLine[9]) - 1).Value.Name;
                        model.Polygons.Add(polygon);
                        break;

                    case "K":
                        host.logProvider.Log(LogLevel.Warning, "Node that K-Lines are not recognized");
                        break;

                    case "BOX":
                        if (splittedLine.Length != 7)
                        {
							host.logProvider.Log(LogLevel.Warning, "Malformed \"BOX\" in Line " + i);
                            continue;
                        }
						host.logProvider.Log(LogLevel.Warning, "Node that BOX-Line are not recognized");
                        break;

                    case "N":
                        if (splittedLine.Length != 5)
                        {
							host.logProvider.Log(LogLevel.Warning, "Malformed \"N\" in Line " + i);
                            continue;
                        }
						host.logProvider.Log(LogLevel.Warning, "Node that N-Line is not recognized");
                        break;

                    case "CBOX":
                        if (splittedLine.Length != 7)
                        {
							host.logProvider.Log(LogLevel.Warning, "Malformed \"CBOX\" in Line " + i);
                            continue;
                        }
                        host.logProvider.Log(LogLevel.Warning, "Node that CBOX-Line is not recognized");
                        break;

                    case "M":
                        if (splittedLine.Length != 3)
                        {
                            host.logProvider.Log(LogLevel.Warning, "Malformed Modelanimation in Line " + i);
                            continue;
                        }
                        string AnimationName = splittedLine[1].Trim();
                        string numberString = Regex.Match(AnimationName, @"\d+$").Value;
                        int FrameNumber = Convert.ToInt32(numberString);
                        AnimationName = AnimationName.Substring(0, AnimationName.Length - numberString.Length);

                        int found = -1;
                        for (int j = 0; j < model.ModelAnimations.Count; j++)
                        {
                            if (model.ModelAnimations[j].Name == AnimationName)
                            {
                                found = j;
                                break;
                            }
                        }
                        if (found == -1)
                        {
                            ModelAnimation newAnimation = new ModelAnimation();
                            newAnimation.Name = AnimationName;
                            model.ModelAnimations.Add(newAnimation);

                            found = model.ModelAnimations.Count - 1;
                        }

                        ModelAnimationFrame frame = new ModelAnimationFrame();
                        frame.Duration = Convert.ToInt32(splittedLine[2]);

                        model.ModelAnimations[found].Frames.Add(new ModelAnimationFrame());

                        break;

                    case "A":
                        if (splittedLine.Length != 8)
                        {
                            host.logProvider.Log(LogLevel.Warning, "Malformed Modelanimation in Line " + i);
                            continue;
                        }
                        if (activeFrame == null)
                        {
                            host.logProvider.Log(LogLevel.Warning, "No Animation defined in Line " + i);
                            continue;
                        }
                        int vertexId = Convert.ToInt32(splittedLine[1]);

                        vertex = new Vertex();
                        vertex.Coordinate = new Vector3(
                                Convert.ToInt32(splittedLine[2]),
                                Convert.ToInt32(splittedLine[3]),
                                Convert.ToInt32(splittedLine[1])
                                );
                        vertex.Coordinate += model.Vertices[vertexId].Coordinate;
                        vertex.Normals = new Vector3(
                                Convert.ToInt32(splittedLine[5]),
                                Convert.ToInt32(splittedLine[6]),
                                Convert.ToInt32(splittedLine[4])
                                );
                        vertex.TextureCoordinate = model.Vertices[vertexId].TextureCoordinate;

                        activeFrame.Vertices.Add(vertexId, vertex);
                        break;
                }
            }
            return model;
        }

        public void Write(string filePath, BaseModel model)
        {
            List<string> fileLines = new List<string>();

            fileLines.Add("[Exporter=ModelConverter]");
            fileLines.Add("");
            fileLines.Add("[VNUM=1.43]");
            fileLines.Add("");
            fileLines.Add("[NUMSURFACES=" + model.Materials.Count.ToString() + "]");
            fileLines.Add("[NUMVERTICES=" + model.Vertices.Count.ToString() + "]");
            //TODO: What are BLENDVERTS???
            fileLines.Add("[NUMBLENDVERTS=0]");
            fileLines.Add("[NUMPOLYGONS=" + model.Polygons.Count.ToString() + "]");
            //TODO: support Bones
            fileLines.Add("[NUMBONES=0]");
            //TODO: support Bones Animation (is this Bone Animation?)
            fileLines.Add("[NUMMUSCLEANIMS=0]");
            fileLines.Add("[NUMMUSCLEFRAMES=0]");
            fileLines.Add("");

            // Export Materials --------------------------------------------------------------------------------------------
            foreach (Material material in model.Materials.Values)
            {
                fileLines.Add(
                    "SRF" +
                    ", " + material.Name +
                    ", 0" +
                    ", 0" +
                    ", 0" +
                    ", " + material.TextureFile +
                    ", 517" +
                    ", 2" +
                    ", 0" +
                    ", 0" +
                    ", 0" +
                    ", 0" +
                    ", 1" +
                    ", 0" +
                    ", 0" +
                    ", 0" +
                    ", 0" +
                    ", 0" +
                    ", 0"
                    );
            }
            // Export Vertives --------------------------------------------------------------------------------------------
            foreach (Vertex vertex in model.Vertices)
            {
                fileLines.Add(
                    "D" +
					", " + (vertex.Coordinate.Z).ToString("0", CultureInfo.GetCultureInfo("en-US")) +
					", " + (vertex.Coordinate.X).ToString("0", CultureInfo.GetCultureInfo("en-US")) +
					", " + (vertex.Coordinate.Y).ToString("0", CultureInfo.GetCultureInfo("en-US")) +
					", " + (vertex.Normals.Z * 256.0f).ToString("0", CultureInfo.GetCultureInfo("en-US")) +
					", " + (vertex.Normals.X * 256.0f).ToString("0", CultureInfo.GetCultureInfo("en-US")) +
					", " + (vertex.Normals.Y * 256.0f).ToString("0", CultureInfo.GetCultureInfo("en-US")) +
					", " + (vertex.TextureCoordinate.X * 1024.0f).ToString("0", CultureInfo.GetCultureInfo("en-US")) +
					", " + (vertex.TextureCoordinate.Y * 1024.0f).ToString("0", CultureInfo.GetCultureInfo("en-US")) +
                    ", 0" +
                    ", 0" +
                    ", 0" +
                    ", 0"
                    );
            }
            // Export ??? --------------------------------------------------------------------------------------------
            //TODO: export BP Lines
            // Export Polygons --------------------------------------------------------------------------------------------
            foreach (Polygon polygon in model.Polygons)
            {
                fileLines.Add(
                    "P" +
                    ", 3" +
                    ", " + (polygon.Point3Id + 1).ToString() +
                    ", " + (polygon.Point2Id + 1).ToString() +
                    ", " + (polygon.Point1Id + 1).ToString() +
                    ", 0" +
                    ", 0" +
                    ", 0" +
                    ", 0" +
                    ", " + GetMaterialId(polygon.materialId, model).ToString() +
                    ", 0" +
                    ", 0" +
                    ", 0" +
                    ", 0" +
                    ", 0" +
                    ", 0" +
                    ", 0" +
                    ", 0" +
                    ", 0" +
                    ", 0" +
                    ", 0"
                    );
            }
            // Export ??? --------------------------------------------------------------------------------------------
            //TODO: export K Lines
            // Export Box --------------------------------------------------------------------------------------------
            //find points
            Vector3 smalltestPoint = new Vector3();
            smalltestPoint.X = model.Vertices[0].Coordinate.X;
            smalltestPoint.Y = model.Vertices[0].Coordinate.Y;
            smalltestPoint.Z = model.Vertices[0].Coordinate.Z;
            Vector3 biggestPoint = new Vector3();
            biggestPoint.X = model.Vertices[0].Coordinate.X;
            biggestPoint.Y = model.Vertices[0].Coordinate.Y;
            biggestPoint.Z = model.Vertices[0].Coordinate.Z;

            foreach (Vertex vertex in model.Vertices)
            {
                if (smalltestPoint.X > vertex.Coordinate.X)
                    smalltestPoint.X = vertex.Coordinate.X;
                if (smalltestPoint.Y > vertex.Coordinate.Y)
                    smalltestPoint.Y = vertex.Coordinate.Y;
                if (smalltestPoint.Z > vertex.Coordinate.Z)
                    smalltestPoint.Z = vertex.Coordinate.Z;

                if (biggestPoint.X < vertex.Coordinate.X)
                    biggestPoint.X = vertex.Coordinate.X;
                if (biggestPoint.Y < vertex.Coordinate.Y)
                    biggestPoint.Y = vertex.Coordinate.Y;
                if (biggestPoint.Z < vertex.Coordinate.Z)
                    biggestPoint.Z = vertex.Coordinate.Z;
            }

            fileLines.Add(
                "BOX" +
				", " + (smalltestPoint.Z / 100.0f).ToString("0", CultureInfo.GetCultureInfo("en-US")) +
				", " + (smalltestPoint.X / 100.0f).ToString("0", CultureInfo.GetCultureInfo("en-US")) +
				", " + (smalltestPoint.Y / 100.0f).ToString("0", CultureInfo.GetCultureInfo("en-US")) +
				", " + (biggestPoint.Z / 100.0f).ToString("0", CultureInfo.GetCultureInfo("en-US")) +
				", " + (biggestPoint.X / 100.0f).ToString("0", CultureInfo.GetCultureInfo("en-US")) +
				", " + (biggestPoint.Y / 100.0f).ToString("0", CultureInfo.GetCultureInfo("en-US"))
                );

            // Export ??? --------------------------------------------------------------------------------------------
            fileLines.Add(
                "N" +
                ", 0" +
                ", 0" +
                ", 0" +
                ", 100" //TODO: What is this? (v3o export, N)?
                );

            // Export CBox --------------------------------------------------------------------------------------------
            fileLines.Add(
                "CBOX" +
                ", " + (smalltestPoint.Z / 100.0f).ToString("0.000", CultureInfo.GetCultureInfo("en-US")) +
                ", " + (smalltestPoint.X / 100.0f).ToString("0.000", CultureInfo.GetCultureInfo("en-US")) +
                ", " + (smalltestPoint.Y / 100.0f).ToString("0.000", CultureInfo.GetCultureInfo("en-US")) +
                ", " + (biggestPoint.Y / 100.0f).ToString("0.000", CultureInfo.GetCultureInfo("en-US")) +
                ", " + (biggestPoint.Y / 100.0f).ToString("0.000", CultureInfo.GetCultureInfo("en-US")) +
                ", " + (biggestPoint.Y / 100.0f).ToString("0.000", CultureInfo.GetCultureInfo("en-US"))
                );
            // Export Model Frames --------------------------------------------------------------------------------------------
            foreach (ModelAnimation Animation in model.ModelAnimations)
            {
                for (int i = 0; i < Animation.Frames.Count; i++)
                {
                    fileLines.Add(
                        "M" +
                        ", " + Animation.Name + i.ToString() +
                        ", " + Animation.Frames[i].Duration.ToString()
                        );
                    foreach (KeyValuePair<int, Vertex> vertex in Animation.Frames[i].Vertices)
                    {
                        fileLines.Add(
                            "A" +
                            ", " + vertex.Key.ToString() +
							", " + (vertex.Value.Coordinate.Z - model.Vertices[vertex.Key].Coordinate.Z).ToString("0", CultureInfo.GetCultureInfo("en-US")) +
							", " + (vertex.Value.Coordinate.X - model.Vertices[vertex.Key].Coordinate.X).ToString("0", CultureInfo.GetCultureInfo("en-US")) +
							", " + (vertex.Value.Coordinate.Y - model.Vertices[vertex.Key].Coordinate.Y).ToString("0", CultureInfo.GetCultureInfo("en-US")) +
							", " + (vertex.Value.Normals.Z).ToString("0", CultureInfo.GetCultureInfo("en-US")) +
							", " + (vertex.Value.Normals.X).ToString("0", CultureInfo.GetCultureInfo("en-US")) +
							", " + (vertex.Value.Normals.Y).ToString("0", CultureInfo.GetCultureInfo("en-US"))
                            );
                    }
                }
            }

            File.WriteAllLines(filePath, fileLines);
        }

        private int GetMaterialId(string MaterialName, BaseModel model)
        {
            for (int i = 0; i < model.Materials.Values.Count; i++)
            {
                if (model.Materials.Values.ElementAt(i).Name == MaterialName)
                    return i + 1;
            }
            return 1;
        }
	}
}