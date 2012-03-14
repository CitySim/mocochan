using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModelConverter.Model;
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;

namespace ModelConverter.Plugin.Obj
{
    public class ModelObj : IPlugin
    {
        public string Name { get { return "Wavefront OBJ"; } }
        public Dictionary<string, string> fileExtensions
        {
            get
            {
                Dictionary<string, string> extensionsDic = new Dictionary<string, string>();
                extensionsDic.Add("obj", "Wavefront OBJ");
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
        public string About { get { return "Support for Wavefront OBJ Files"; } }

        public bool canRead { get { return true; } }
        public bool canWrite { get { return true; } }
        public bool supportReadTexture { get { return true; } }
        public bool supportWriteTexture { get { return true; } }
        public bool supportModelAnimation { get { return false; } }
        public bool supportModelAnimationNormal { get { return false; } }
        public bool supportModelAnimationTexture { get { return false; } }

        public BaseModel Read(string filePath, out List<LogMessage> Log)
        {
            Log = new List<LogMessage>();
            BaseModel model = new BaseModel();
            string[] fileLines = File.ReadAllLines(filePath);
            List<Vector3> VertexList = new List<Vector3>();
            List<Vector2> TextureCoordinates = new List<Vector2>();
            List<Vector3> Normals = new List<Vector3>();

            bool openGroup = false;
            //create an emtpy material for polygons without one
            string usedMaterial = "material-" + Guid.NewGuid().ToString();
            model.Materials.Add(usedMaterial, new Material() { Name = usedMaterial });

            for (int i = 0; i < fileLines.Length; i++)
            {
                if (fileLines[i].StartsWith("#"))
                    continue; //comment

                string[] tokens = Regex.Split(fileLines[i].Trim(), @"\s+", RegexOptions.ExplicitCapture);

                switch (tokens[0])
                {
                    case "mtllib":
                        if (tokens.Length != 2)
                        {
                            Log.Add(new LogMessage("Illigal Material Link, Line " + i, LogLevel.Warning));
                            continue;
                        }
                        string mtlPath = Path.Combine(
                            Path.GetDirectoryName(filePath),
                            fileLines[i].Substring(7)
                            );
                        //TODO: DO a bit more
						string[] mtllibLines = new string[0];
						try
						{
							mtllibLines = File.ReadAllLines(mtlPath);
						}
						catch { }

                        Material material = null;

                        foreach (string line in mtllibLines)
                        {
                            string[] mtlTokens = Regex.Split(line.Trim(), @"\s+", RegexOptions.ExplicitCapture);

                            switch (mtlTokens[0])
                            {
                                case "newmtl":
                                    if (material != null)
                                        model.Materials.Add(material.Name, material);
                                    material = new Material();
                                    material.Name = mtlTokens[1];
                                    break;

                                case "map_Kd":
                                    if (material == null)
                                    {
                                        Log.Add(new LogMessage("Texture without Material, File " + fileLines[i].Substring(7) + ", Line " + i, LogLevel.Warning));
                                        continue;
                                    }
									if (mtlTokens.Count() >= 2)
									{
										material.TextureFile = mtlTokens[1];
									}
									else
									{
										material.TextureFile = "";
									}
									break;
                            }
                        }
                        if (material != null)
                            model.Materials.Add(material.Name, material);
                        break;

                    case "o":
                        if (tokens.Length != 2)
                        {
                            Log.Add(new LogMessage("Illigal Object, Line " + i, LogLevel.Warning));
                            continue;
                        }
                        openGroup = false;
                        break;

                    case "v":
                        if (tokens.Length != 4 && tokens.Length != 5)
                        {
                            Log.Add(new LogMessage("Illigal Vertex, Line " + i, LogLevel.Warning));
                            continue;
                        }
                        Vector3 point3D = new Vector3();
                        point3D.X = float.Parse(tokens[1], CultureInfo.GetCultureInfo("en-US"));
                        point3D.Y = float.Parse(tokens[2], CultureInfo.GetCultureInfo("en-US"));
                        point3D.Z = float.Parse(tokens[3], CultureInfo.GetCultureInfo("en-US"));
                        VertexList.Add(point3D);
                        break;

                    case "vt":
                        if (tokens.Length != 3)
                        {
                            Log.Add(new LogMessage("Illigal Vertex Texture, Line " + i, LogLevel.Warning));
                            continue;
                        }
                        Vector2 textureCoordinate = new Vector2();
                        textureCoordinate.X = float.Parse(tokens[1], CultureInfo.GetCultureInfo("en-US"));
                        textureCoordinate.Y = 1.0f - float.Parse(tokens[2], CultureInfo.GetCultureInfo("en-US"));
                        TextureCoordinates.Add(textureCoordinate);
                        break;

                    case "vn":
                        if (tokens.Length != 4)
                        {
                            Log.Add(new LogMessage("Illigal Vertex Normals, Line " + i, LogLevel.Warning));
                            continue;
                        }
                        Vector3 normal = new Vector3();
                        normal.X = float.Parse(tokens[1]);
                        normal.Y = float.Parse(tokens[2]);
                        normal.Z = float.Parse(tokens[3]);
                        Normals.Add(normal);
                        break;

                    case "g":
                        if (tokens.Length != 1 && tokens.Length != 2)
                        {
                            Log.Add(new LogMessage("Illigal Group, Line " + i, LogLevel.Warning));
                            continue;
                        }
                        openGroup = true;
                        break;

                    case "usemtl":
                        if (tokens.Length != 2)
                        {
                            Log.Add(new LogMessage("Illigal Material use, Line " + i, LogLevel.Warning));
                            continue;
                        }
                        usedMaterial = tokens[1];
                        break;

                    case "s":
                        if (tokens.Length != 2)
                        {
                            Log.Add(new LogMessage("Illigal Smooth, Line " + i, LogLevel.Warning));
                            continue;
                        }
                        if (!openGroup)
                        {
                            Log.Add(new LogMessage("No Group, Line " + i, LogLevel.Warning));
                            continue;
                        }
                        Log.Add(new LogMessage("smooth ignored, Line " + i, LogLevel.Info));
                        break;

                    case "f":
                        for (int j = tokens.Length - 2; j > 1; j--)
                        {
                            Polygon polygon = new Polygon();

                            string[][] points = new string[3][];
                            points[0] = tokens[tokens.Length - 1].Split(new char[] { '/' });
                            points[1] = tokens[j].Split(new char[] { '/' });
                            points[2] = tokens[j - 1].Split(new char[] { '/' });

                            polygon.materialId = usedMaterial;

                            for (int k = 0; k < 3; k++)
                            {
                                Vertex vertex = new Vertex();
                                int vertexId = Convert.ToInt32(points[k][0]) - 1;

                                vertex.Coordinate =
                                    VertexList[vertexId].Copy();

                                if (points[k].Length >= 2)
                                {
                                    if (points[k][1].Trim() != String.Empty)
                                    {
                                        vertex.TextureCoordinate =
                                            TextureCoordinates[Convert.ToInt32(points[k][1]) - 1];
                                    }
                                }

								if (points[k].Length >= 3)
                                {
                                    if (points[k][2].Trim() != String.Empty)
                                    {
                                        vertex.Normals =
                                            Normals[Convert.ToInt32(points[k][2]) - 1];
                                    }
                                }

                                if (model.Vertices.Contains(vertex, new VertexComparer()))
                                {
                                    for (int l = 0; l < model.Vertices.Count; l++)
                                    {
                                        if (model.Vertices[l] == vertex)
                                            points[k][0] = l.ToString();
                                    }
                                }
                                else
                                {
                                    points[k][0] = model.Vertices.Count.ToString();
                                    model.Vertices.Add(vertex);
                                }
                            }
                            polygon.Point1Id = Convert.ToInt32(points[0][0]);
                            polygon.Point2Id = Convert.ToInt32(points[1][0]);
                            polygon.Point3Id = Convert.ToInt32(points[2][0]);

                            model.Polygons.Add(polygon);
                        }
                        break;
                }
            }

            bool materialUsed = false;
            foreach (Polygon polygon in model.Polygons)
            {
                if (polygon.materialId == model.Materials.Values.ElementAt(0).Name)
                {
                    materialUsed = true;
                }
            }
            if (!materialUsed)
            {
                model.Materials.Remove(model.Materials.Keys.ElementAt(0));
            }

            return model;
        }

        public void Write(string filePath, BaseModel model, out List<LogMessage> Log)
        {
            Log = new List<LogMessage>(); Log = new List<LogMessage>();

            List<string> fileLines = new List<string>();

            fileLines.Add("#Exporter ModelConverter");

            // write mtllib --------------------------------------------------------------------------------------------
            string mtlPath = Path.Combine(
                Path.GetDirectoryName(filePath),
                Path.GetFileNameWithoutExtension(filePath) + ".mtl"
                );

            List<string> mtllibLines = new List<string>();
            mtllibLines.Add("#Exporter ModelConverter");

            foreach (Material material in model.Materials.Values)
            {
                mtllibLines.Add("newmtl " + material.Name.Replace(" ", ""));
                mtllibLines.Add("map_Kd " + material.TextureFile);
                mtllibLines.Add("");
            }
            File.WriteAllLines(mtlPath, mtllibLines);

            fileLines.Add("mtllib " + Path.GetFileName(mtlPath));

            // write vertices --------------------------------------------------------------------------------------------
            foreach (Vertex vertex in model.Vertices)
            {
                fileLines.Add(
                    "v" +
                    " " + vertex.Coordinate.X.ToString("0.00000000", CultureInfo.GetCultureInfo("en-US")) +
                    " " + vertex.Coordinate.Y.ToString("0.00000000", CultureInfo.GetCultureInfo("en-US")) +
                    " " + vertex.Coordinate.Z.ToString("0.00000000", CultureInfo.GetCultureInfo("en-US"))
                    );
            }

            // write texture points --------------------------------------------------------------------------------------------
            foreach (Vertex vertex in model.Vertices)
            {
                fileLines.Add(
                    "vt" +
                    " " + (vertex.TextureCoordinate.X).ToString("0.00000000", CultureInfo.GetCultureInfo("en-US")) +
                    " " + (1.0f - vertex.TextureCoordinate.Y).ToString("0.00000000", CultureInfo.GetCultureInfo("en-US"))
                    );
            }
            // write normals --------------------------------------------------------------------------------------------
            foreach (Vertex vertex in model.Vertices)
            {
                fileLines.Add(
                    "vn" +
                    " " + vertex.Normals.X.ToString("0.00000000", CultureInfo.GetCultureInfo("en-US")) +
                    " " + vertex.Normals.Y.ToString("0.00000000", CultureInfo.GetCultureInfo("en-US")) +
                    " " + vertex.Normals.Z.ToString("0.00000000", CultureInfo.GetCultureInfo("en-US"))
                    );
            }
			// write faces  which has no material --------------------------------------------------------------------------------------------
			foreach (Polygon polygon in model.Polygons)
			{
				bool found = false;
				foreach (Material material in model.Materials.Values)
				{
					if (material.Name == polygon.materialId)
					{
						found = true;
					}
				}
				if (!found)
				{
					fileLines.Add(
						"f" +
						" " + (polygon.Point3Id + 1) + "/" + (polygon.Point3Id + 1) + "/" + (polygon.Point3Id + 1) +
						" " + (polygon.Point2Id + 1) + "/" + (polygon.Point2Id + 1) + "/" + (polygon.Point2Id + 1) +
						" " + (polygon.Point1Id + 1) + "/" + (polygon.Point1Id + 1) + "/" + (polygon.Point1Id + 1)
						);
				}

			}
			// write faces --------------------------------------------------------------------------------------------
			foreach (Material material in model.Materials.Values)
			{
				fileLines.Add("usemtl " + material.Name);
				foreach (Polygon polygon in model.Polygons)
				{
					if (polygon.materialId == material.Name)
					{
						fileLines.Add(
							"f" +
							" " + (polygon.Point3Id + 1) + "/" + (polygon.Point3Id + 1) + "/" + (polygon.Point3Id + 1) +
							" " + (polygon.Point2Id + 1) + "/" + (polygon.Point2Id + 1) + "/" + (polygon.Point2Id + 1) +
							" " + (polygon.Point1Id + 1) + "/" + (polygon.Point1Id + 1) + "/" + (polygon.Point1Id + 1)
							);
					}
				}
			}

            File.WriteAllLines(filePath, fileLines);
        }
    }
}