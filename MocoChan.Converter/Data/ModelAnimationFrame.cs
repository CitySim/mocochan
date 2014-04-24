using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MocoChan.Converter.Data
{
    public class ModelAnimationFrame
    {
        public int Duration;
        public Dictionary<int, Vertex> Vertices = new Dictionary<int, Vertex>();
    }
}
