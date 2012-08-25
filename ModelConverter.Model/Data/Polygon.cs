using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelConverter.Model
{
    public class Polygon
    {
		private BaseModel model;

        public int Point1Id;
		public Vertex Point1Vertex
		{
			get {
				return model.Vertices[Point1Id];
			}
		}

        public int Point2Id;
		public Vertex Point2Vertex
		{
			get
			{
				return model.Vertices[Point2Id];
			}
		}

        public int Point3Id;
		public Vertex Point3Vertex
		{
			get
			{
				return model.Vertices[Point3Id];
			}
		}

        public string materialId;

		public Polygon(BaseModel model)
		{
			this.model = model;
		}
    }
}
