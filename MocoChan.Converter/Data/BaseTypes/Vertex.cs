using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace MocoChan.Converter.Data.BaseTypes
{
    public class Vertex
    {
        public Vector3 Coordinate = new Vector3();
        public Vector3 Normals = new Vector3();
        public Vector2 TextureCoordinate = new Vector2();

        public override bool Equals(object obj)
        {
            if (obj is Vertex)
                return (Vertex)obj == this;
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Vertex a, Vertex b)
        {
            if (a.Coordinate != b.Coordinate ||
                a.Normals != b.Normals ||
                a.TextureCoordinate != b.TextureCoordinate)
                return false;
            return true;
        }

        public static bool operator !=(Vertex a, Vertex b)
        {
            if (a.Coordinate == b.Coordinate &&
                a.Normals == b.Normals &&
                a.TextureCoordinate == b.TextureCoordinate)
                return false;
            return true;
        }
    }
}
