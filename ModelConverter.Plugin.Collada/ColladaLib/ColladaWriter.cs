using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using ModelConverter.Model;

namespace ModelConverter.Plugin.Collada.ColladaLib
{
	public class ColladaWriter
	{
		CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");

		public ColladaWriter()
		{
		}

		public void Write(BaseModel model, string path)
		{
			XmlTextWriter doc = new XmlTextWriter(path, Encoding.UTF8);

			doc.WriteStartDocument();

			doc.WriteStartElement("COLLADA");
			doc.WriteAttributeString("version", "1.4.0");
			doc.WriteAttributeString("xmlns", "http://www.collada.org/2005/11/COLLADASchema");

			WriteAsset(doc);
			WriteGeometrieLibrary(doc, model);
			WriteVisualScenesLibrary(doc, model);
			WriteScene(doc, model);			

			doc.WriteEndElement();
			doc.WriteEndDocument();

			doc.Close();
		}

		private void WriteAsset(XmlTextWriter doc)
		{
			doc.WriteStartElement("asset");

			doc.WriteStartElement("contributor");

			// TODO: write the correct version here
			doc.WriteElementString("authoring_tool", "ModelConverter x.x.x.x; COLLADA Plugin x.x.x.x");

			doc.WriteEndElement();

			// TODO: write actual time here
			doc.WriteElementString("created", "1970-01-01T00:00:00");
			doc.WriteElementString("modified", "1970-01-01T00:00:00");

			doc.WriteStartElement("contributor");
			doc.WriteAttributeString("meter", "0.01");
			doc.WriteAttributeString("name", "centimeter");
			doc.WriteEndElement();

			doc.WriteElementString("up_axis", "Y_UP");

			doc.WriteEndElement();
			doc.Flush();
		}

		private void WriteGeometrieLibrary(XmlTextWriter doc, BaseModel model)
		{
			doc.WriteStartElement("library_geometries");
			doc.WriteStartElement("geometry");
			doc.WriteAttributeString("id", "model");
			doc.WriteAttributeString("name", "model");
			doc.WriteStartElement("mesh");

			#region Write Positions
			doc.WriteStartElement("source");
			doc.WriteAttributeString("id", "model-positions");

			doc.WriteStartElement("float_array");
			doc.WriteAttributeString("id", "model-positions-array");
			doc.WriteAttributeString("count", (model.Vertices.Count * 3).ToString());

			foreach (Vertex vertex in model.Vertices)
			{
				doc.WriteString(vertex.Coordinate.X.ToString("0.000000", culture) + " ");
				doc.WriteString(vertex.Coordinate.Y.ToString("0.000000", culture) + " ");
				doc.WriteString(vertex.Coordinate.Z.ToString("0.000000", culture) + " ");
			}

			doc.WriteEndElement();  // float_array

			doc.WriteStartElement("technique_common");
			doc.WriteStartElement("accessor");
			doc.WriteAttributeString("count", model.Vertices.Count.ToString());
			doc.WriteAttributeString("source", "#model-positions-array");
			doc.WriteAttributeString("stride", "3");

			doc.WriteStartElement("param");
			doc.WriteAttributeString("name", "X");
			doc.WriteAttributeString("type", "float");
			doc.WriteEndElement();

			doc.WriteStartElement("param");
			doc.WriteAttributeString("name", "Y");
			doc.WriteAttributeString("type", "float");
			doc.WriteEndElement();

			doc.WriteStartElement("param");
			doc.WriteAttributeString("name", "Z");
			doc.WriteAttributeString("type", "float");
			doc.WriteEndElement();

			doc.WriteEndElement();  // accessor
			doc.WriteEndElement();  // technique_common

			doc.WriteEndElement(); // source
			#endregion

			#region Write Normals
			doc.WriteStartElement("source");
			doc.WriteAttributeString("id", "model-normals");

			doc.WriteStartElement("float_array");
			doc.WriteAttributeString("id", "model-normals-array");
			doc.WriteAttributeString("count", (model.Vertices.Count * 3).ToString());

			foreach (Vertex vertex in model.Vertices)
			{
				doc.WriteString(vertex.Normals.X.ToString("0.000000", culture) + " ");
				doc.WriteString(vertex.Normals.Y.ToString("0.000000", culture) + " ");
				doc.WriteString(vertex.Normals.Z.ToString("0.000000", culture) + " ");
			}

			doc.WriteEndElement();  // float_array

			doc.WriteStartElement("technique_common");
			doc.WriteStartElement("accessor");
			doc.WriteAttributeString("count", model.Vertices.Count.ToString());
			doc.WriteAttributeString("source", "#model-normals-array");
			doc.WriteAttributeString("stride", "3");

			doc.WriteStartElement("param");
			doc.WriteAttributeString("name", "X");
			doc.WriteAttributeString("type", "float");
			doc.WriteEndElement();

			doc.WriteStartElement("param");
			doc.WriteAttributeString("name", "Y");
			doc.WriteAttributeString("type", "float");
			doc.WriteEndElement();

			doc.WriteStartElement("param");
			doc.WriteAttributeString("name", "Z");
			doc.WriteAttributeString("type", "float");
			doc.WriteEndElement();

			doc.WriteEndElement();  // accessor
			doc.WriteEndElement();  // technique_common

			doc.WriteEndElement(); // source
			#endregion

			#region Write UV
			doc.WriteStartElement("source");
			doc.WriteAttributeString("id", "model-uv");

			doc.WriteStartElement("float_array");
			doc.WriteAttributeString("id", "model-uv-array");
			doc.WriteAttributeString("count", (model.Vertices.Count * 3).ToString());

			foreach (Vertex vertex in model.Vertices)
			{
				doc.WriteString(vertex.TextureCoordinate.X.ToString("0.000000", culture) + " ");
				doc.WriteString(vertex.TextureCoordinate.Y.ToString("0.000000", culture) + " ");
			}

			doc.WriteEndElement();  // float_array

			doc.WriteStartElement("technique_common");
			doc.WriteStartElement("accessor");
			doc.WriteAttributeString("count", model.Vertices.Count.ToString());
			doc.WriteAttributeString("source", "#model-uv-array");
			doc.WriteAttributeString("stride", "3");

			doc.WriteStartElement("param");
			doc.WriteAttributeString("name", "S");
			doc.WriteAttributeString("type", "float");
			doc.WriteEndElement();

			doc.WriteStartElement("param");
			doc.WriteAttributeString("name", "T");
			doc.WriteAttributeString("type", "float");
			doc.WriteEndElement();

			doc.WriteEndElement(); // accessor
			doc.WriteEndElement(); // technique_common
			doc.WriteEndElement(); // source
			#endregion

			#region write vertices node
			doc.WriteStartElement("vertices");
			doc.WriteAttributeString("id", "model-vertices");

			doc.WriteStartElement("input");
			doc.WriteAttributeString("semantic", "POSITION");
			doc.WriteAttributeString("source", "#model-positions");
			doc.WriteEndElement();

			doc.WriteEndElement();
			#endregion

			#region write polylist node
			doc.WriteStartElement("polylist");
			doc.WriteAttributeString("count", model.Polygons.Count.ToString());
			doc.WriteAttributeString("material", "");

			doc.WriteStartElement("input");
			doc.WriteAttributeString("offset", "0");
			doc.WriteAttributeString("semantic", "VERTEX");
			doc.WriteAttributeString("source", "#model-vertices");
			doc.WriteEndElement();

			doc.WriteStartElement("input");
			doc.WriteAttributeString("offset", "1");
			doc.WriteAttributeString("semantic", "NORMAL");
			doc.WriteAttributeString("source", "#model-normal");
			doc.WriteEndElement();

			doc.WriteStartElement("input");
			doc.WriteAttributeString("offset", "2");
			doc.WriteAttributeString("semantic", "TEXTCOORD");
			doc.WriteAttributeString("source", "#model-uv");
			doc.WriteEndElement();

			doc.WriteStartElement("vcount");
			for (int i = 0; i < model.Polygons.Count; i++)
			{
				doc.WriteString("3 ");
			}
			doc.WriteEndElement();

			doc.WriteStartElement("p");
			for (int i = 0; i < model.Polygons.Count; i++)
			{
				Polygon poly = model.Polygons[i];

				string point3 = poly.Point3Id.ToString();
				doc.WriteString(point3 + " " + point3 + " " + point3 + " ");

				string point2 = poly.Point2Id.ToString();
				doc.WriteString(point2 + " " + point2 + " " + point2 + " ");

				string point1 = poly.Point1Id.ToString();
				doc.WriteString(point1 + " " + point1 + " " + point1 + " ");
			}
			doc.WriteEndElement();



			doc.WriteEndElement();
			#endregion

			doc.WriteEndElement(); // mesh
			doc.WriteEndElement(); // geometry
			doc.WriteEndElement(); // library_geometries
			doc.Flush();
		}

		private void WriteVisualScenesLibrary(XmlTextWriter doc, BaseModel model)
		{
			doc.WriteStartElement("library_visual_scenes");

			doc.WriteStartElement("visual_scene");
			doc.WriteAttributeString("id", "model-converter-scene");
			doc.WriteAttributeString("name", "ModelConverterScene");

			doc.WriteStartElement("node");
			doc.WriteAttributeString("layer", "L1");
			doc.WriteAttributeString("id", "model-node");
			doc.WriteAttributeString("name", "ModelNode");
			
			/*
			<translate sid="translate">0.00000 0.00000 0.00000</translate>
			<rotate sid="rotateZ">0 0 1 0.00000</rotate>
			<rotate sid="rotateY">0 1 0 0.00000</rotate>
			<rotate sid="rotateX">1 0 0 0.00000</rotate>
			<scale sid="scale">1.0000 1.0000 1.0000</scale>
			*/
			
			doc.WriteStartElement("instance_geometry");
			doc.WriteAttributeString("url", "#model");

			/*
			<bind_material>
				<technique_common>
					<instance_material symbol="default" target="#default">
						<bind_vertex_input input_semantic="TEXCOORD" input_set="1" semantic="CHANNEL1"/>
					</instance_material>
				</technique_common>
			</bind_material>
			*/

			doc.WriteEndElement();
			doc.WriteEndElement();
			doc.WriteEndElement();
			doc.WriteEndElement();
			doc.Flush();
		}

		private void WriteScene(XmlTextWriter doc, BaseModel model)
		{
			doc.WriteStartElement("scene");

			doc.WriteStartElement("instance_visual_scene");
			doc.WriteAttributeString("url", "#model-converter-scene");
			doc.WriteEndElement();

			doc.WriteEndElement();
			doc.Flush();
		}
	}
}
