using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Gtk;
using Cairo;

using Gdk;
using GLib;
using Application = Gtk.Application;
using Color = System.Drawing.Color;
using Window = Gtk.Window;
using UI = Gtk.Builder.ObjectAttribute;

namespace CG
{
    class MainWindow : Window
    {
        [UI] private DrawingArea _canvas = null;

        #region UI спинбаттонов и чекбоксов

        [UI] private CheckButton _allowZBuffer = null;
        [UI] private CheckButton _allowNormals = null;
        [UI] private CheckButton _allowWireframe = null;
        [UI] private CheckButton _allowInvisPoly = null;

        [UI] private Adjustment _xRotation = null;
        [UI] private Adjustment _yRotation = null;
        [UI] private Adjustment _zRotation = null;
        [UI] private Adjustment _xScale = null;
        [UI] private Adjustment _yScale = null;
        [UI] private Adjustment _zScale = null;
        [UI] private Adjustment _xShift = null;
        [UI] private Adjustment _yShift = null;
        [UI] private Adjustment _zShift = null;
        
        [UI] private Adjustment _a = null;
        [UI] private Adjustment _b = null;
        [UI] private Adjustment _c = null;
        
        [UI] private Adjustment _meridiansCount = null;
        [UI] private Adjustment _parallelsCount = null;

        [UI] private Adjustment _materialColorR = null;
        [UI] private Adjustment _materialColorG = null;
        [UI] private Adjustment _materialColorB = null;
        
        [UI] private Adjustment _k_aR = null;
        [UI] private Adjustment _k_aG = null;
        [UI] private Adjustment _k_aB = null;
        
        [UI] private Adjustment _k_dR = null;
        [UI] private Adjustment _k_dG = null;
        [UI] private Adjustment _k_dB = null;
        
        [UI] private Adjustment _k_sR = null;
        [UI] private Adjustment _k_sG = null;
        [UI] private Adjustment _k_sB = null;
        
        [UI] private Adjustment _p = null;

        [UI] private Adjustment _ambientLightColorR = null;
        [UI] private Adjustment _ambientLightColorG = null;
        [UI] private Adjustment _ambientLightColorB = null;
        
        [UI] private Adjustment _pointLightIntensityR = null;
        [UI] private Adjustment _pointLightIntensityG = null;
        [UI] private Adjustment _pointLightIntensityB = null;
        
        [UI] private Adjustment _pointLightPositionX = null;
        [UI] private Adjustment _pointLightPositionY = null;
        [UI] private Adjustment _pointLightPositionZ = null;

        #endregion

        #region UI матрицы

        [UI] private Adjustment _m11 = null;
        [UI] private Adjustment _m12 = null;
        [UI] private Adjustment _m13 = null;
        [UI] private Adjustment _m14 = null;
        [UI] private Adjustment _m21 = null;
        [UI] private Adjustment _m22 = null;
        [UI] private Adjustment _m23 = null;
        [UI] private Adjustment _m24 = null;
        [UI] private Adjustment _m31 = null;
        [UI] private Adjustment _m32 = null;
        [UI] private Adjustment _m33 = null;
        [UI] private Adjustment _m34 = null;
        [UI] private Adjustment _m41 = null;
        [UI] private Adjustment _m42 = null;
        [UI] private Adjustment _m43 = null;
        [UI] private Adjustment _m44 = null;

        #endregion
        
        [UI] private ComboBoxText _projectionMode = null;
        
        [UI] private ComboBoxText _lightingModel = null;
        
        private float _defaultScale = 200;
        private float _compressedScale = 1;
        private Matrix4x4 _defaultTransformationMatrix;
        private Matrix4x4 _transformationMatrix;

        private float _axisSize = 40;
        private Vector3 _axisPosition = Vector3.One;
        private Matrix4x4 _axisTransformMatrix = Matrix4x4.Identity;
        
        #region Мышь

        private Vector3 _mousePosition = new Vector3(0, 0, 0);
        private uint _mousePressedButton = 0;

        #endregion

        private Vector3 _darkBlue = new Vector3(0, (float)0.2, (float)0.4);
        
        private enum Projection
        {
            None,
            Front,
            Right,
            Top,
            Isometric
        }

        #region выстраиваивание сцены

        private Mesh _figure = new Ellipsoid(1, 1, 1, 16, 8);
        private AmbientLight _ambientLight = new AmbientLight(new Vector3(0.5f, 0.5f, 0.5f));
        private PointLight _pointLight = new PointLight(2, 0, 0, 1, 1, 1);
        // private Material _material = new Material();
        
        #endregion
        
        public MainWindow() : this(new Builder("CGLab3.glade"))
        {
            _transformationMatrix = new Matrix4x4(
                1, 0, 0, 0,
                0, 1, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1);

            CalculateTranformationMatrix();
        }

        private MainWindow(Builder builder) : base(builder.GetRawOwnedObject("MainWindow"))
        {
            builder.Autoconnect(this);
            DeleteEvent += (o, args) => Application.Quit();

            _canvas.Drawn += (o, args) =>
            {
                var context = args.Cr;
                
                context.SetSourceRGB(0, 0, 0);
                context.Paint();

                context.Antialias = Antialias.Subpixel;
                context.LineWidth = 2d;
                
                _figure.ApplyTransformation(_transformationMatrix * _defaultTransformationMatrix);
                _pointLight.ApplyTransformation(_transformationMatrix * _defaultTransformationMatrix);
                
                DrawMesh(context, _figure);
                DrawPointLight(context, _pointLight);

                if (_allowNormals.Active)
                {
                    DrawNormals(context, _figure);
                }
                
                CalculateAxisTransformationMatrix();
                DrawAxis(context);
            };

            _canvas.SizeAllocated += (o, args) =>
            {
                if (_defaultScale > Math.Min(args.Allocation.Width, args.Allocation.Height)){
                    _compressedScale = Math.Min(args.Allocation.Width, args.Allocation.Height) / _defaultScale;
                }
                if (_defaultScale < Math.Min(args.Allocation.Width, args.Allocation.Height)){
                    _compressedScale = Math.Min(args.Allocation.Width, args.Allocation.Height) / _defaultScale;
                    if (_compressedScale > 1){
                        _compressedScale = 1;
                    }
                }

                float trueScale = _defaultScale * _compressedScale;
                
                _defaultTransformationMatrix = Matrix4x4.CreateScale(trueScale, trueScale, trueScale);
                _defaultTransformationMatrix *= Matrix4x4.CreateTranslation(args.Allocation.Width / 2, args.Allocation.Height / 2, 0);

                _axisPosition.X = args.Allocation.Width - 45;
                _axisPosition.Y = args.Allocation.Height - 45;
            };

            #region Обработка спинбатоннов, чекбоксов и комботекстбоксов

            _xShift.ValueChanged += (o, args) => { CalculateTranformationMatrix(); _canvas.QueueDraw();};
            _yShift.ValueChanged += (o, args) => { CalculateTranformationMatrix(); _canvas.QueueDraw();};
            _zShift.ValueChanged += (o, args) => { CalculateTranformationMatrix(); _canvas.QueueDraw();};

            _xScale.ValueChanged += (o, args) => { CalculateTranformationMatrix(); _canvas.QueueDraw();};
            _yScale.ValueChanged += (o, args) => { CalculateTranformationMatrix(); _canvas.QueueDraw();};
            _zScale.ValueChanged += (o, args) => { CalculateTranformationMatrix(); _canvas.QueueDraw();};
            
            _xRotation.ValueChanged += (o, args) => { CalculateTranformationMatrix(); _canvas.QueueDraw();};
            _yRotation.ValueChanged += (o, args) => { CalculateTranformationMatrix(); _canvas.QueueDraw();};
            _zRotation.ValueChanged += (o, args) => { CalculateTranformationMatrix(); _canvas.QueueDraw();};
            
            _allowNormals.Toggled += (o, args) => { CalculateTranformationMatrix(); _canvas.QueueDraw();};
            _allowWireframe.Toggled += (o, args) => { CalculateTranformationMatrix(); _canvas.QueueDraw();};
            _allowInvisPoly.Toggled += (o, args) => { CalculateTranformationMatrix(); _canvas.QueueDraw();};
            _allowZBuffer.Toggled += (o, args) => { CalculateTranformationMatrix(); _canvas.QueueDraw();};

            _projectionMode.Changed += (o, args) => { SetProjection(); CalculateTranformationMatrix(); _canvas.QueueDraw();};
            
            _a.ValueChanged += (o, args) => { _figure = new Ellipsoid((float)_a.Value, 
                    (float)_b.Value, (float)_c.Value, 
                    (int)_meridiansCount.Value, 
                    (int)_parallelsCount.Value); 
                    _canvas.QueueDraw();};
            _b.ValueChanged += (o, args) => { _figure = new Ellipsoid((float)_a.Value, 
                    (float)_b.Value, (float)_c.Value, 
                    (int)_meridiansCount.Value, 
                    (int)_parallelsCount.Value); 
                    _canvas.QueueDraw();};
            _c.ValueChanged += (o, args) => { _figure = new Ellipsoid((float)_a.Value, 
                    (float)_b.Value, (float)_c.Value, 
                    (int)_meridiansCount.Value, 
                    (int)_parallelsCount.Value); 
                    _canvas.QueueDraw();};
            _meridiansCount.ValueChanged += (o, args) => { _figure = new Ellipsoid((float)_a.Value, 
                    (float)_b.Value, (float)_c.Value, 
                    (int)_meridiansCount.Value, 
                    (int)_parallelsCount.Value); 
                    _canvas.QueueDraw();};
            _parallelsCount.ValueChanged += (o, args) => { _figure = new Ellipsoid((float)_a.Value, 
                    (float)_b.Value, (float)_c.Value, 
                    (int)_meridiansCount.Value, 
                    (int)_parallelsCount.Value); 
                    _canvas.QueueDraw();};

            _materialColorR.ValueChanged += (o, args) => {_figure.SetColor((float)_materialColorR.Value, 
                                                                                 (float)_materialColorG.Value, 
                                                                                 (float)_materialColorB.Value); 
                                                                _canvas.QueueDraw();};
            _materialColorG.ValueChanged += (o, args) => {_figure.SetColor((float)_materialColorR.Value, 
                                                                                 (float)_materialColorG.Value, 
                                                                                 (float)_materialColorB.Value); 
                                                                _canvas.QueueDraw();};
            _materialColorB.ValueChanged += (o, args) => {_figure.SetColor((float)_materialColorR.Value, 
                                                                                 (float)_materialColorG.Value, 
                                                                                 (float)_materialColorB.Value); 
                                                                _canvas.QueueDraw();};
            
            _k_aR.ValueChanged += (o, args) => {_canvas.QueueDraw();};
            _k_aG.ValueChanged += (o, args) => {_canvas.QueueDraw();};
            _k_aB.ValueChanged += (o, args) => {_canvas.QueueDraw();};
            
            _k_dR.ValueChanged += (o, args) => {_canvas.QueueDraw();};
            _k_dG.ValueChanged += (o, args) => {_canvas.QueueDraw();};
            _k_dB.ValueChanged += (o, args) => {_canvas.QueueDraw();};
            
            _k_sR.ValueChanged += (o, args) => {_canvas.QueueDraw();};
            _k_sG.ValueChanged += (o, args) => {_canvas.QueueDraw();};
            _k_sB.ValueChanged += (o, args) => {_canvas.QueueDraw();};
            
            _p.ValueChanged += (o, args) => {_canvas.QueueDraw();};
            
            _ambientLightColorR.ValueChanged += (o, args) => {_ambientLight.Color.X = (float)_ambientLightColorR.Value; 
                                                                    _canvas.QueueDraw();};
            _ambientLightColorG.ValueChanged += (o, args) => {_ambientLight.Color.Y = (float)_ambientLightColorG.Value; 
                                                                    _canvas.QueueDraw();};
            _ambientLightColorB.ValueChanged += (o, args) => {_ambientLight.Color.Z = (float)_ambientLightColorB.Value; 
                                                                    _canvas.QueueDraw();};
            
            _pointLightIntensityR.ValueChanged += (o, args) => {_pointLight.Intensity.X = (float)_pointLightIntensityR.Value; 
                                                                      _canvas.QueueDraw();};
            _pointLightIntensityG.ValueChanged += (o, args) => {_pointLight.Intensity.Y = (float)_pointLightIntensityG.Value; 
                                                                      _canvas.QueueDraw();};
            _pointLightIntensityB.ValueChanged += (o, args) => {_pointLight.Intensity.Z = (float)_pointLightIntensityB.Value; 
                                                                      _canvas.QueueDraw();};
            
            _pointLightPositionX.ValueChanged += (o, args) => {_pointLight.Position.X = (float)_pointLightPositionX.Value; 
                                                                     _canvas.QueueDraw();};
            _pointLightPositionY.ValueChanged += (o, args) => {_pointLight.Position.Y = (float)_pointLightPositionY.Value; 
                                                                     _canvas.QueueDraw();};
            _pointLightPositionZ.ValueChanged += (o, args) => {_pointLight.Position.Z = (float)_pointLightPositionZ.Value; 
                                                                     _canvas.QueueDraw();};
            
            #endregion

            #region Обработка матрицы

            _m11.ValueChanged += (o, args) => { _transformationMatrix.M11 = (float)_m11.Value; _canvas.QueueDraw();};
            _m12.ValueChanged += (o, args) => { _transformationMatrix.M12 = (float)_m12.Value; _canvas.QueueDraw();};
            _m13.ValueChanged += (o, args) => { _transformationMatrix.M13 = (float)_m13.Value; _canvas.QueueDraw();};
            _m14.ValueChanged += (o, args) => { _transformationMatrix.M14 = (float)_m14.Value; _canvas.QueueDraw();};
            _m21.ValueChanged += (o, args) => { _transformationMatrix.M21 = (float)_m21.Value; _canvas.QueueDraw();};
            _m22.ValueChanged += (o, args) => { _transformationMatrix.M22 = (float)_m22.Value; _canvas.QueueDraw();};
            _m23.ValueChanged += (o, args) => { _transformationMatrix.M23 = (float)_m23.Value; _canvas.QueueDraw();};
            _m24.ValueChanged += (o, args) => { _transformationMatrix.M24 = (float)_m24.Value; _canvas.QueueDraw();};
            _m31.ValueChanged += (o, args) => { _transformationMatrix.M31 = (float)_m31.Value; _canvas.QueueDraw();};
            _m32.ValueChanged += (o, args) => { _transformationMatrix.M32 = (float)_m32.Value; _canvas.QueueDraw();};
            _m33.ValueChanged += (o, args) => { _transformationMatrix.M33 = (float)_m33.Value; _canvas.QueueDraw();};
            _m34.ValueChanged += (o, args) => { _transformationMatrix.M34 = (float)_m34.Value; _canvas.QueueDraw();};
            _m41.ValueChanged += (o, args) => { _transformationMatrix.M41 = (float)_m41.Value; _canvas.QueueDraw();};
            _m42.ValueChanged += (o, args) => { _transformationMatrix.M42 = (float)_m42.Value; _canvas.QueueDraw();};
            _m43.ValueChanged += (o, args) => { _transformationMatrix.M43 = (float)_m43.Value; _canvas.QueueDraw();};
            _m44.ValueChanged += (o, args) => { _transformationMatrix.M44 = (float)_m44.Value; _canvas.QueueDraw();};

            #endregion

            #region Обработка мыши

            _canvas.Events |= EventMask.ScrollMask | EventMask.PointerMotionMask | EventMask.ButtonPressMask |
                              EventMask.ButtonReleaseMask;
            
            _canvas.ButtonPressEvent += (o, args) =>
            {
                _mousePressedButton = args.Event.Button;
                _mousePosition.X = (float)args.Event.X;
                _mousePosition.Y = (float)args.Event.Y;
            };
            
            _canvas.MotionNotifyEvent += (o, args) =>
            {
                Vector3 _currentMousePosition = new Vector3((float)args.Event.X, (float)args.Event.Y, 0);
                
                if (_mousePressedButton == 1)
                {
                    _xShift.Value += (double)(_currentMousePosition.X - _mousePosition.X) / (double)_defaultTransformationMatrix.M11;
                    _yShift.Value += (double)(_currentMousePosition.Y - _mousePosition.Y) / (double)_defaultTransformationMatrix.M22;
                }
                if (_mousePressedButton == 3)
                {
                    if (_xRotation.Value + _currentMousePosition.Y - _mousePosition.Y < 0)
                        _xRotation.Value += 360 + _currentMousePosition.Y - _mousePosition.Y;
                    else if (_xRotation.Value + _currentMousePosition.Y - _mousePosition.Y > 360)
                        _xRotation.Value += -360 + _currentMousePosition.Y - _mousePosition.Y;
                    else
                        _xRotation.Value += _currentMousePosition.Y - _mousePosition.Y;
                    
                    if (_yRotation.Value + _currentMousePosition.X - _mousePosition.X < 0)
                        _yRotation.Value += 360 + _currentMousePosition.X - _mousePosition.X;
                    else if (_yRotation.Value + _currentMousePosition.X - _mousePosition.X > 360)
                        _yRotation.Value += -360 + _currentMousePosition.X - _mousePosition.X;
                    else
                        _yRotation.Value += _currentMousePosition.X - _mousePosition.X;
                }

                _mousePosition = _currentMousePosition;
            };
            
            _canvas.ButtonReleaseEvent += (o, args) => _mousePressedButton = 0;
            
            _canvas.ScrollEvent += (o, args) =>
            {
                if (args.Event.Direction == ScrollDirection.Down)
                {
                    _xScale.Value -= _xScale.StepIncrement;
                    _yScale.Value -= _yScale.StepIncrement;
                    _zScale.Value -= _zScale.StepIncrement;
                }
                else if (args.Event.Direction == ScrollDirection.Up)
                {
                    _xScale.Value += _xScale.StepIncrement;
                    _yScale.Value += _yScale.StepIncrement;
                    _zScale.Value += _zScale.StepIncrement;
                }
                _canvas.QueueDraw();
            };

            #endregion
        }

        private void Window_DeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
        }
        
        #region Отрисовка фигруы
        
        private void DrawPolygon(Context context, Polygon polygon, Vector3 color)
        {
            if (polygon.Vertexes.Count == 0)
                return;
            if (_allowInvisPoly.Active && polygon.CalculateNormal().Z < 0)
                return;

            context.MoveTo(polygon.Vertexes[0].Point.X, polygon.Vertexes[0].Point.Y);
            
            for (int i = 1; i < polygon.Vertexes.Count; ++i)
            {
                context.LineTo(polygon.Vertexes[i].Point.X, polygon.Vertexes[i].Point.Y);
            }
            context.ClosePath();
            
            if (_allowWireframe.Active == false)
            {
                //фоновая составляющая
                Vector3 I_a = _ambientLight.Color;
                
                //рассеяная составляющая
                Vector4 L = _pointLight.TransformedPosition - polygon.CalculatePosition();
                L /= L.Length();
                Vector4 N = polygon.CalculateNormal();
                float cosLN = Math.Max(0, (float)(Vector4.Dot(L, N) / (L.Length() * N.Length())));
                Vector3 I_d = new Vector3((float)(_k_dR.Value * _pointLightIntensityR.Value * cosLN), 
                                          (float)(_k_dG.Value * _pointLightIntensityG.Value * cosLN), 
                                          (float)(_k_dB.Value * _pointLightIntensityB.Value * cosLN));
                
                //отражающая составляющая
                Vector4 R = L + 2 * ((cosLN * N) - L); //отраженный от порехности вектор
                Matrix4x4 _invertedTransformationMatrix;
                Matrix4x4.Invert(_transformationMatrix, out _invertedTransformationMatrix);
                
                Vector4 S = Vector4.Transform(new Vector4(0, 0, -1, 1), _invertedTransformationMatrix);
                
                float cosRSp = (float)Math.Pow(Math.Max(0, (float)(Vector4.Dot(R, S) / (R.Length() * S.Length()))), _p.Value);
                Vector3 I_s = new Vector3((float)(_k_sR.Value * _pointLightIntensityR.Value * cosRSp), 
                                          (float)(_k_sG.Value * _pointLightIntensityG.Value * cosRSp), 
                                          (float)(_k_sB.Value * _pointLightIntensityB.Value * cosRSp));

                Vector3 polygonTrueColor = (I_a + I_d + I_s) * polygon.Color;
                
                context.SetSourceRGB(polygonTrueColor.X,
                                     polygonTrueColor.Y,
                                     polygonTrueColor.Z);
                
                context.Fill();
            }
            
            // context.SetSourceRGB(.5, 1, .5);
            // context.Stroke();
        }

        private void DrawMesh(Context context, Mesh mesh)
        {
            if (_allowZBuffer.Active)
            {
                mesh.TransformedPolygons = mesh.TransformedPolygons.OrderBy(polygon => (polygon.Vertexes.Select(vertex => vertex.Point.Z)).Max()).ToList();
            }
            
            for (int i = 0; i < mesh.TransformedPolygons.Count; ++i)
            {
                DrawPolygon(context, mesh.TransformedPolygons[i], _darkBlue);
            }
        }

        private void DrawNormal(Context context, Polygon polygon)
        {
            if (_allowInvisPoly.Active && polygon.CalculateNormal().Z < 0)
                return;
            
            Vector4 normal = polygon.CalculateNormal();
            normal *= 100;
            Vector4 polygonCenter = new Vector4(0, 0, 0, 0);
            for (int i = 0; i < polygon.Vertexes.Count; ++i)
            {
                polygonCenter += polygon.Vertexes[i].Point;
            }
            polygonCenter /= polygon.Vertexes.Count;

            context.MoveTo(polygonCenter.X, polygonCenter.Y);
            context.LineTo(polygonCenter.X + normal.X, polygonCenter.Y + normal.Y);
            
            context.SetSourceRGB(.0, 1, 1);
            context.Stroke();
        }
        
        private void DrawNormals(Context context, Mesh mesh)
        {
            foreach (var polygon in mesh.TransformedPolygons)
            {
                DrawNormal(context, polygon);
            }
        }

        private void DrawAxis(Context context)
        {
            Vector4 o = Vector4.Transform(new Vector4(1, 1, 1, 1), _axisTransformMatrix);
            Vector4 x = Vector4.Transform( new Vector4(_axisSize, 1, 1, 1), _axisTransformMatrix);
            Vector4 y = Vector4.Transform( new Vector4(1, _axisSize, 1, 1), _axisTransformMatrix);
            Vector4 z = Vector4.Transform( new Vector4(1, 1, _axisSize, 1), _axisTransformMatrix);

            context.MoveTo(o.X, o.Y);
            context.LineTo(x.X, x.Y);
            context.SetSourceRGB(1, .0, .0);
            context.Stroke();
            
            context.MoveTo(o.X, o.Y);
            context.LineTo(y.X, y.Y);
            context.SetSourceRGB(0, 1, .0);
            context.Stroke();
            
            context.MoveTo(o.X, o.Y);
            context.LineTo(z.X, z.Y);
            context.SetSourceRGB(.0, .0, 1);
            context.Stroke();
        }

        private void DrawPointLight(Context context, PointLight pointLight)
        {
            Cairo.Gradient radpat = new RadialGradient(pointLight.TransformedPosition.X, 
                                                       pointLight.TransformedPosition.Y, 
                                                       0, 
                                                       pointLight.TransformedPosition.X, 
                                                       pointLight.TransformedPosition.Y, 50);
            radpat.AddColorStop(0, new Cairo.Color(1, 1, 1, 1));
            radpat.AddColorStop(1, new Cairo.Color(0, 0, 0, 0));


            context.Rectangle(0, 0, Window.Width, Window.Height);
            context.Source = radpat;
            context.Fill();
        }
        
        #endregion

        private void CalculateTranformationMatrix()
        {
            _transformationMatrix = Matrix4x4.CreateScale((float) _xScale.Value, (float) _yScale.Value, (float) _zScale.Value);
            _transformationMatrix *= Matrix4x4.CreateRotationX((float)(_xRotation.Value * Math.PI / 180)) *
                                     Matrix4x4.CreateRotationY((float)(_yRotation.Value * Math.PI / 180)) *
                                     Matrix4x4.CreateRotationZ((float)(_zRotation.Value * Math.PI / 180));
        
            _transformationMatrix *= Matrix4x4.CreateTranslation((float)_xShift.Value, (float)_yShift.Value, (float)_zShift.Value);

            #region Обновление спинбатоннов для матрицы

            _m11.Value = _transformationMatrix.M11;
            _m12.Value = _transformationMatrix.M12;
            _m13.Value = _transformationMatrix.M13;
            _m14.Value = _transformationMatrix.M14;
            _m21.Value = _transformationMatrix.M21;
            _m22.Value = _transformationMatrix.M22;
            _m23.Value = _transformationMatrix.M23;
            _m24.Value = _transformationMatrix.M24;
            _m31.Value = _transformationMatrix.M31;
            _m32.Value = _transformationMatrix.M32;
            _m33.Value = _transformationMatrix.M33;
            _m34.Value = _transformationMatrix.M34;
            _m41.Value = _transformationMatrix.M41;
            _m42.Value = _transformationMatrix.M42;
            _m43.Value = _transformationMatrix.M43;
            _m44.Value = _transformationMatrix.M44;
            
            #endregion
        }

        private void CalculateAxisTransformationMatrix()
        {
            _axisTransformMatrix = Matrix4x4.CreateRotationX((float) (_xRotation.Value * Math.PI / 180)) *
                                   Matrix4x4.CreateRotationY((float) (_yRotation.Value * Math.PI / 180)) *
                                   Matrix4x4.CreateRotationZ((float) (_zRotation.Value * Math.PI / 180));
            _axisTransformMatrix *= Matrix4x4.CreateTranslation(_axisPosition.X, _axisPosition.Y, 0);
        }

        private void SetProjection()
        {
            if (_projectionMode.Active == (int)Projection.Isometric)
            {
                _xScale.Value = 1;
                _yScale.Value = 1;
                _zScale.Value = 1;
                _xRotation.Value = 35;
                _yRotation.Value = 45;
            }
            else if (_projectionMode.Active == (int) Projection.Front)
            {
                _xScale.Value = 1;
                _yScale.Value = 1;
                _zScale.Value = 0;
            }
            else if (_projectionMode.Active == (int) Projection.Top)
            {
                _xScale.Value = 1;
                _yScale.Value = 0;
                _zScale.Value = 1;
            }
            else if (_projectionMode.Active == (int) Projection.Right)
            {
                _xScale.Value = 0;
                _yScale.Value = 1;
                _zScale.Value = 1;
            }
            else if (_projectionMode.Active == (int) Projection.None)
            {
                _xScale.Value = 1;
                _yScale.Value = 1;
                _zScale.Value = 1;
                _xRotation.Value = 0;
                _yRotation.Value = 0;
            }
        }
    }
}