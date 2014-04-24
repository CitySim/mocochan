using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MocoChan.Converter.Data
{
    public class Model
    {
        public List<Vertex> Vertices = new List<Vertex>();
        public List<Polygon> Polygons = new List<Polygon>();
        public Dictionary<string, Material> Materials = new Dictionary<string, Material>();
        public List<ModelAnimation> ModelAnimations = new List<ModelAnimation>();

        public Model()
        {
        }

        public Model(Model Model)
        {
            Vertices = Model.Vertices;
            Polygons = Model.Polygons;
            Materials = Model.Materials;
        }

        public void Scale(double factor)
        {
            //scale model
            foreach (Vertex vertex in Vertices)
            {
                vertex.Coordinate = vertex.Coordinate * factor;
            }
            //scale animation
            foreach (ModelAnimation Animation in ModelAnimations)
            {
                foreach (ModelAnimationFrame frame in Animation.Frames)
                {
                    foreach (KeyValuePair<int, Vertex> vertex in frame.Vertices)
                    {
                        vertex.Value.Coordinate = vertex.Value.Coordinate * factor;
                    }
                }
            }
        }

		/// <summary>
		/// Recalcutes the Normals of all Polygons.
		/// </summary>
        public void RecalculateNormals()
        {
            foreach (Polygon polygon in Polygons)
            {
                Vector3 vector1 = Vertices[polygon.Point2Id].Coordinate - Vertices[polygon.Point1Id].Coordinate;
                Vector3 vector2 = Vertices[polygon.Point3Id].Coordinate - Vertices[polygon.Point1Id].Coordinate;

                Vector3 Normal = new Vector3();
                Normal.X = (vector1.Y * vector2.Z) - (vector1.Z * vector2.Y);
                Normal.Y = -((vector2.Z * vector1.X) - (vector2.X * vector1.Z));
                Normal.Z = (vector1.X * vector2.Y) - (vector1.Y * vector2.X);

                //normalize
				double CombinedSquares =
                    (Normal.X * Normal.X) +
                    (Normal.Y * Normal.Y) +
                    (Normal.Z * Normal.Z);

				double NormalisationFactor = Math.Sqrt(CombinedSquares);
                Normal.X = Normal.X / NormalisationFactor;
                Normal.Y = Normal.Y / NormalisationFactor;
                Normal.Z = Normal.Z / NormalisationFactor;

                Vertices[polygon.Point1Id].Normals = Normal;
                Vertices[polygon.Point2Id].Normals = Normal;
                Vertices[polygon.Point3Id].Normals = Normal;
            }
        }
    }
}
