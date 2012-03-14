using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media.Media3D;
using bm = ModelConverter.Model;

namespace ModelConverter
{
    /// <summary>
    /// Interaktionslogik für ViewWindow.xaml
    /// </summary>
    public partial class ViewWindow : Window
    {
        public ViewWindow(bm.BaseModel model)
        {
            InitializeComponent();

            MeshGeometry3D triangleMesh = new MeshGeometry3D();
            foreach(bm.Vertex vertex in model.Vertices)
            {
                triangleMesh.Positions.Add(
                    new Point3D(
                        vertex.Coordinate.X,
                        vertex.Coordinate.Y,
                        vertex.Coordinate.Z
                        ));
            }
            foreach (bm.Polygon polygon in model.Polygons)
            {
                triangleMesh.TriangleIndices.Add(polygon.Point3Id);
                triangleMesh.TriangleIndices.Add(polygon.Point2Id);
                triangleMesh.TriangleIndices.Add(polygon.Point1Id);
            }
            foreach (bm.Vertex vertex in model.Vertices)
            {
                triangleMesh.Normals.Add(
                    new Vector3D(
                        vertex.Normals.X,
                        vertex.Normals.Y,
                        vertex.Normals.Z
                        ));
            }
            //foreach (bm.Material material in model.Materials.Values)
            //{
            //    Material wpfMaterial = new EmissiveMaterial(new ImageBrush(
            //}
            Material wpfMaterial = new DiffuseMaterial(
                new SolidColorBrush(Colors.DarkKhaki));

            GeometryModel3D triangleModel = new GeometryModel3D(triangleMesh, wpfMaterial);

            ModelVisual3D wpfModel = new ModelVisual3D();
            wpfModel.Content = triangleModel;

            this.modelVieport.Children.Add(wpfModel);
        }
    }
}
