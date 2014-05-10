using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace MocoChan.Converter.Data.BaseTypes
{
    public class VertexComparer : IEqualityComparer<Vertex>, IComparer<Vertex>
    {
        public bool Equals(Vertex x, Vertex y)
        {
            return x == y;
        }

        public int Compare(Vertex x, Vertex y)
        {
            if (x == y)
                return 0;

            if (x.Coordinate >= y.Coordinate ||
                x.Normals >= y.Normals ||
                x.TextureCoordinate >= y.TextureCoordinate)
                return +1;

            // x smaller than y
            return -1;
        }

        public int GetHashCode(Vertex obj)
        {
			return obj.GetHashCode();
        }
    }
}
