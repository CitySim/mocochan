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
        public Dictionary<string, string> fileExtensions
        {
            get
            {
                Dictionary<string, string> extensionsDic = new Dictionary<string, string>();
                extensionsDic.Add("v3o", "Emergency 3 & 4");
                return extensionsDic;
            }
        }
        public DateTime PluginVersion
        {
            get
            {
                Version version = Assembly.GetExecutingAssembly().GetName().Version;
                return new DateTime(2000, 1, 1)
                    .AddDays(version.Build)
                    .AddSeconds(version.Revision * 2);
            }
        }
        public string Creator { get { return "Sven Tatter"; } }
        public string About { get { return "Support for v3o used by Emergency3 and Emergncy4"; } }

        public bool canRead { get { return true; } }
        public bool canWrite { get { return true; } }
        public bool supportReadTexture { get { return true; } }
        public bool supportWriteTexture { get { return true; } }
        public bool supportModelAnimation { get { return true; } }
        public bool supportModelAnimationNormal { get { return true; } }
        public bool supportModelAnimationTexture { get { return false; } }

        public BaseModel Read(string filePath, out List<LogMessage> Log)
        {
            string[] fileLines = File.ReadAllLines(filePath);
            Log = new List<LogMessage>();
            BaseModel model = new BaseModel();

            for (int i = 0; i < fileLines.Length; i++)
            {
                string line = fileLines[i].Trim();
                if (line == String.Empty)
                    continue; //yeah, emtpy line
                if (line.StartsWith("//"))
                    continue; //a comment
                if (line.StartsWith("["))
                    continue; //TODO: some Meta Info, we dont read

                string[] splittedLine;
                splittedLine = line.Split(',');

                ModelAnimationFrame activeFrame = null;

                switch (splittedLine[0])
                {
                    case "SRF":
                        if (splittedLine.Length != 19)
                        {
                            Log.Add(new LogMessage("Malformed Material in Line " + i, LogLevel.Warning));
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
                            Log.Add(new LogMessage("Malformed Vertex in Line " + i, LogLevel.Warning));
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
                            Log.Add(new LogMessage("Malformed \"BP\" in Line " + i, LogLevel.Warning));
                            continue;
                        }
                        Log.Add(new LogMessage("Node that BP-Lines are not recognized", LogLevel.Warning));
                        break;

                    case "P":
                        if (splittedLine.Length != 21)
                        {
                            Log.Add(new LogMessage("Malformed Polygon in Line " + i, LogLevel.Warning));
                            continue;
                        }
                        Polygon polygon = new Polygon();
                        polygon.Point1Id = Convert.ToInt32(splittedLine[4]) - 1;
                        polygon.Point2Id = Convert.ToInt32(splittedLine[3]) - 1;
                        polygon.Point3Id = Convert.ToInt32(splittedLine[2]) - 1;
                        polygon.materialId = model.Materials.ElementAt(Convert.ToInt32(splittedLine[9]) - 1).Value.Name;
                        model.Polygons.Add(polygon);
                        break;

                    case "K":
                        Log.Add(new LogMessage("Node that K-Lines are not recognized", LogLevel.Warning));
                        break;

                    case "BOX":
                        if (splittedLine.Length != 7)
                        {
                            Log.Add(new LogMessage("Malformed \"BOX\" in Line " + i, LogLevel.Warning));
                            continue;
                        }
                        Log.Add(new LogMessage("Node that BOX-Line are not recognized", LogLevel.Warning));
                        break;

                    case "N":
                        if (splittedLine.Length != 5)
                        {
                            Log.Add(new LogMessage("Malformed \"N\" in Line " + i, LogLevel.Warning));
                            continue;
                        }
                        Log.Add(new LogMessage("Node that N-Line is not recognized", LogLevel.Warning));
                        break;

                    case "CBOX":
                        if (splittedLine.Length != 7)
                        {
                            Log.Add(new LogMessage("Malformed \"CBOX\" in Line " + i, LogLevel.Warning));
                            continue;
                        }
                        Log.Add(new LogMessage("Node that CBOX-Line is not recognized", LogLevel.Warning));
                        break;

                    case "M":
                        if (splittedLine.Length != 3)
                        {
                            Log.Add(new LogMessage("Malformed Modelanimation in Line " + i, LogLevel.Warning));
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
                            Log.Add(new LogMessage("Malformed Modelanimation in Line " + i, LogLevel.Warning));
                            continue;
                        }
                        if (activeFrame == null)
                        {
                            Log.Add(new LogMessage("No Animation defined in Line " + i, LogLevel.Warning));
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

        public void Write(string filePath, BaseModel model, out List<LogMessage> Log)
        {
            Log = new List<LogMessage>();
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
                    ", " + Convert.ToInt32(vertex.Coordinate.Z).ToString() +
                    ", " + Convert.ToInt32(vertex.Coordinate.X).ToString() +
                    ", " + Convert.ToInt32(vertex.Coordinate.Y).ToString() +
                    ", " + Convert.ToInt32(vertex.Normals.Z * 256.0f).ToString() +
                    ", " + Convert.ToInt32(vertex.Normals.X * 256.0f).ToString() +
                    ", " + Convert.ToInt32(vertex.Normals.Y * 256.0f).ToString() +
                    ", " + Convert.ToInt32(vertex.TextureCoordinate.X * 1024.0f).ToString() +
                    ", " + Convert.ToInt32(vertex.TextureCoordinate.Y * 1024.0f).ToString() +
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
                ", " + Convert.ToInt32(smalltestPoint.Z / 100.0f).ToString() +
                ", " + Convert.ToInt32(smalltestPoint.X / 100.0f).ToString() +
                ", " + Convert.ToInt32(smalltestPoint.Y / 100.0f).ToString() +
                ", " + Convert.ToInt32(biggestPoint.Z / 100.0f).ToString() +
                ", " + Convert.ToInt32(biggestPoint.X / 100.0f).ToString() +
                ", " + Convert.ToInt32(biggestPoint.Y / 100.0f).ToString()
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
                            ", " + Convert.ToInt32(vertex.Value.Coordinate.Z - model.Vertices[vertex.Key].Coordinate.Z).ToString() +
                            ", " + Convert.ToInt32(vertex.Value.Coordinate.X - model.Vertices[vertex.Key].Coordinate.X).ToString() +
                            ", " + Convert.ToInt32(vertex.Value.Coordinate.Y - model.Vertices[vertex.Key].Coordinate.Y).ToString() +
                            ", " + Convert.ToInt32(vertex.Value.Normals.Z).ToString() +
                            ", " + Convert.ToInt32(vertex.Value.Normals.X).ToString() +
                            ", " + Convert.ToInt32(vertex.Value.Normals.Y).ToString()
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