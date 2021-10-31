using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CGLab1
{
    public partial class Form1 : Form
    {
        private float interpreter = 0.1f;
        private float scale_in_X = 1;
        private float scale_in_Y = 1;
        private float compressed_scale = 1;
        private float X_offset = 0;
        private float Y_offset = 0;
        private float angle_of_rotation = 0;
        private float center_of_rotation_X = 0;
        private float center_of_rotation_Y = 0;
        private PointF[] points;
        private float sizeX = 1;
        private float sizeY = 1;
        private float curve_parametr = 1;
        private float mark_scale = 0.05f;
        
        public Form1()
        {
            InitializeComponent();
        }

        private PointF[] GetCurvePoints(float quality)
        {
            List<PointF> list = new List<PointF>();
            for (float t = 0; t < 2*Math.PI; t+=quality)
            {
                list.Add(new PointF(curve_parametr*(float)Math.Pow(Math.Cos(t),3), curve_parametr*(float)Math.Pow(Math.Sin(t),3)));
            }
            PointF[] res = list.ToArray();
            return res;
        }
        
        private float GetXSize(PointF[] points){
            float maxX = float.MinValue;
            float minX = float.MaxValue;
            foreach (PointF point in points){
                minX = Math.Min(minX, point.X);
                maxX = Math.Max(maxX, point.X);
            }
            return maxX-minX;
        }
        private float GetYSize(PointF[] points){
            float maxY = float.MinValue;
            float minY = float.MaxValue;
            foreach (PointF point in points){
                minY = Math.Min(minY, point.X);
                maxY = Math.Max(maxY, point.X);
            }
            return maxY-minY;
        }
        
        private void DrawAxis(object sender, PaintEventArgs e)
        {
            Matrix transformMatrix = new Matrix();
            transformMatrix.Scale(300, 300);
            transformMatrix.Scale(scale_in_X*compressed_scale, scale_in_Y*compressed_scale, MatrixOrder.Append);
            transformMatrix.Rotate(angle_of_rotation, MatrixOrder.Append);
            transformMatrix.Translate((float)this.Width/2, (float)this.Height/2, MatrixOrder.Append);
            transformMatrix.Translate(X_offset, Y_offset, MatrixOrder.Append);
            
            PointF[] XAxis = {new PointF(100, 0), new PointF(-100, 0)};
            PointF[] YAxis = {new PointF(0, 100), new PointF(0, -100)};
            List<PointF> XMarkPoints = new List<PointF>();
            List<PointF> YMarkPoints = new List<PointF>();

            XMarkPoints.Add(new PointF(1, mark_scale));
            XMarkPoints.Add(new PointF(1, -mark_scale)); 
            XMarkPoints.Add(new PointF(-1, mark_scale)); 
            XMarkPoints.Add(new PointF(-1, -mark_scale));
            
            XMarkPoints.Add(new PointF(1-0.5f, mark_scale*0.5f));
            XMarkPoints.Add(new PointF(1-0.5f, -mark_scale*0.5f));
            XMarkPoints.Add(new PointF(-1+0.5f, mark_scale*0.5f));
            XMarkPoints.Add(new PointF(-1+0.5f, -mark_scale*0.5f));
            
            YMarkPoints.Add(new PointF(mark_scale, 1));
            YMarkPoints.Add(new PointF(-mark_scale, 1));
            YMarkPoints.Add(new PointF(mark_scale, -1));
            YMarkPoints.Add(new PointF(-mark_scale, -1));
                
            YMarkPoints.Add(new PointF(mark_scale*0.5f, 1-0.5f));
            YMarkPoints.Add(new PointF(-mark_scale*0.5f, 1-0.5f));
            YMarkPoints.Add(new PointF(mark_scale*0.5f, -1+0.5f)); 
            YMarkPoints.Add(new PointF(-mark_scale*0.5f, -1+0.5f));
  

            PointF[] XMarkPointsArr = XMarkPoints.ToArray();
            PointF[] YMarkPointsArr = YMarkPoints.ToArray();
            
            transformMatrix.TransformPoints(XAxis);
            transformMatrix.TransformPoints(YAxis);
            transformMatrix.TransformPoints(XMarkPointsArr);
            transformMatrix.TransformPoints(YMarkPointsArr);

            Pen pen = new Pen(Color.Black, 2);
            e.Graphics.DrawLine(pen, XAxis[0], XAxis[1]);
            e.Graphics.DrawLine(pen, YAxis[0], YAxis[1]);

            e.Graphics.DrawLine(pen, XMarkPointsArr[0], XMarkPointsArr[1]); 
            e.Graphics.DrawLine(pen, XMarkPointsArr[2], XMarkPointsArr[3]);
            e.Graphics.DrawLine(pen, XMarkPointsArr[4], XMarkPointsArr[5]);
            e.Graphics.DrawLine(pen, XMarkPointsArr[6], XMarkPointsArr[7]);
            e.Graphics.DrawLine(pen, YMarkPointsArr[0], YMarkPointsArr[1]);
            e.Graphics.DrawLine(pen, YMarkPointsArr[2], YMarkPointsArr[3]);
            e.Graphics.DrawLine(pen, YMarkPointsArr[4], YMarkPointsArr[5]);
            e.Graphics.DrawLine(pen, YMarkPointsArr[6], YMarkPointsArr[7]);
        }
        
        private void DrawCurve(object sender, PaintEventArgs e)
        {
            points = GetCurvePoints(interpreter);
            Matrix transformMatrix = new Matrix();
            transformMatrix.Scale(300, 300);
            transformMatrix.Scale(scale_in_X*compressed_scale, scale_in_Y*compressed_scale, MatrixOrder.Append);
            transformMatrix.Rotate(angle_of_rotation, MatrixOrder.Append);
            transformMatrix.Translate((float)this.Width/2, (float)this.Height/2, MatrixOrder.Append);
            transformMatrix.Translate(X_offset, Y_offset, MatrixOrder.Append);
            
            transformMatrix.TransformPoints(points);

            Pen pen = new Pen(Color.Red, 3);
            PointF prewPoint = points[points.Length - 1];
            foreach (PointF curPoint in points) 
            {
                e.Graphics.DrawLine(pen, prewPoint, curPoint);
                prewPoint = curPoint;
            }
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawAxis(sender, e);
            DrawCurve(sender, e);
        }
        
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            sizeX = GetXSize(points);
            sizeY = GetYSize(points);
            if (sizeX > this.Width){
                compressed_scale *= this.Width/sizeX;
            }
            if (sizeY > this.Height){
                compressed_scale *= this.Height/sizeY;
            }
            if ((sizeX < this.Width) || (sizeY < this.Height)){
                compressed_scale *= Math.Min(this.Width/sizeX,this.Height/sizeY);
                if (compressed_scale > 1){
                    compressed_scale = 1;
                }
            }
            Invalidate();
        }

        private void scale_in_X_var_ValueChanged(object sender, EventArgs e)
        {
            scale_in_X = float.Parse(scale_in_X_var.Text);
            Invalidate();
        }

        private void scale_in_Y_var_ValueChanged(object sender, EventArgs e)
        {
            scale_in_Y = float.Parse(scale_in_Y_var.Text);
            Invalidate();
        }

        private void X_offset_var_ValueChanged(object sender, EventArgs e)
        {
            X_offset = float.Parse(X_offset_var.Text);
            Invalidate();
        }

        private void Y_offset_var_ValueChanged(object sender, EventArgs e)
        {
            Y_offset = float.Parse(Y_offset_var.Text);
            Invalidate();
        }

        private void angle_of_rotation_var_ValueChanged(object sender, EventArgs e)
        {
            angle_of_rotation = float.Parse(angle_of_rotation_var.Text);
            Invalidate();
        }

        private void interpreter_var_ValueChanged(object sender, EventArgs e)
        {
            interpreter = float.Parse(interpreter_var.Text);
            Invalidate();
        }

        private void curve_parametr_var_ValueChanged(object sender, EventArgs e)
        {
            curve_parametr = float.Parse(curve_parametr_var.Text);
            Invalidate();
        }
    }
}