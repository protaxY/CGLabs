using System;
using System.Numerics;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GLib;

using Gtk;

namespace CG
{
    public class Vertex
    {
        public Vector4 Point;

        public Vertex(float x, float y, float z)
        {
            Point = new Vector4(x, y, z, 1);
        }

        public Vertex()
        {
            Point = new Vector4(0, 0, 0, 1);
        }
        
        public Vertex(Vector4 vector4)
        {
            Point = vector4;
        }
    }
    
    public class Polygon
    {
        public List<Vertex> Vertexes;
        public Vector3 Color = new Vector3(0.5f, 0.5f, 0.5f);

        public Polygon(Polygon polygon)
        {
            Vertexes = polygon.Vertexes;
        }
        
        public Polygon(List<Vertex> vertexes)
        {
            Vertexes = vertexes;
        }
        
        private Vector3 toVector3(Vertex vertex)
        { 
            return new Vector3(vertex.Point.X, vertex.Point.Y, vertex.Point.Z);
        }   

        public Vector4 CalculateNormal()
        {
            if (Vertexes.Count() >= 3)
            {
                Vector3 a = toVector3(Vertexes[1]) - toVector3(Vertexes[0]);
                Vector3 b = toVector3(Vertexes[2]) - toVector3(Vertexes[0]);
                Vector3 normal = Vector3.Cross(b, a);
                normal = normal / normal.Length();
                return new Vector4(normal.X, normal.Y, normal.Z, 0);
            }
        
            return new Vector4(0, 0, 0, 0);
        }

        public Vector4 CalculatePosition()
        {
            Vector4 result = new Vector4(0, 0, 0, 0);
            for (int i = 0; i < Vertexes.Count; ++i)
            {
                result += (1 / (float)Vertexes.Count) * Vertexes[i].Point;
            }

            return result;
        } 
    }

    public class Mesh
    {
        public List<Vertex> Vertices;
        public List<Vertex> TransformedVertices;
        // public List<Polygon> Polygons;
        public List<Polygon> TransformedPolygons; //связывают преобразованные вершины

        public Mesh()
        {
            Vertices = new List<Vertex>();
            TransformedVertices = new List<Vertex>();
            // Polygons = new List<Polygon>();
            TransformedPolygons = new List<Polygon>();
        }

        public Mesh(List<Vertex> vertices, List<Polygon> polygons)
        {
            Vertices = vertices;
            TransformedVertices = new List<Vertex>(Vertices.Count);
            // Polygons = polygons;
            // TransformedPolygons = new List<Polygon>(Polygons.Count);
        }
        
        public Mesh(List<Vertex> vertices, List<List<int>> polygons)
        {
            Vertices = new List<Vertex>(vertices);
            TransformedVertices = new List<Vertex>(vertices.Count);
            for (int i = 0; i < TransformedVertices.Capacity; ++i)
            {
                TransformedVertices.Add(new Vertex(0, 0, 0));
            }
            
            // Polygons = new List<Polygon>();
            TransformedPolygons = new List<Polygon>();
            
            foreach (var polygon in polygons)
            {
                List<Vertex> polygonVertices = new List<Vertex>();
                List<Vertex> transformedPolygonVertices = new List<Vertex>();
                foreach (var vertexIndex in polygon)
                {
                    polygonVertices.Add(vertices[vertexIndex]);
                    transformedPolygonVertices.Add(TransformedVertices[vertexIndex]);
                }
                // Polygons.Add(new Polygon(polygonVertices));
                TransformedPolygons.Add(new Polygon(transformedPolygonVertices));
            }
        }
        
        public void ApplyTransformation(Matrix4x4 transformationMatrix)
        {
            for (int i = 0; i < Vertices.Count(); ++i)
            {
                TransformedVertices[i].Point = Vector4.Transform(Vertices[i].Point, transformationMatrix);
            }
        }

        public void SetColor(Vector3 color)
        {
            for (int i = 0; i < TransformedPolygons.Count; ++i)
            {
                TransformedPolygons[i].Color = color;
            }
        }
        
        public void SetColor(float r, float g, float b)
        {
            for (int i = 0; i < TransformedPolygons.Count; ++i)
            {
                TransformedPolygons[i].Color = new Vector3(r, g, b);
            }
        }
    }
}