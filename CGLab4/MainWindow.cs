using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Gtk;
using Gdk;
using Cairo;
using System.IO;
using System.Text;
using System.Reflection;
using System.Diagnostics;


using SharpGL;
using SharpGL.Shaders;
using Application = Gtk.Application;
using Window = Gtk.Window;
using UI = Gtk.Builder.ObjectAttribute;

namespace CG
{
    class MainWindow : Window
    {
        [UI] private GLArea _glArea = null;

        #region UI спинбаттонов и чекбоксов
        //камера
        [UI] private Adjustment _xPosition = null;
        [UI] private Adjustment _yPosition = null;
        [UI] private Adjustment _zPosition = null;
        [UI] private Adjustment _xRotation = null;
        [UI] private Adjustment _yRotation = null;
        [UI] private Adjustment _zRotation = null;

        [UI] private Adjustment _aspectRatio = null;
        [UI] private Adjustment _FOV = null;
        [UI] private Adjustment _clipStart = null;
        [UI] private Adjustment _clipEnd = null;

        //параметры элипсойда
        [UI] private Adjustment _a = null;
        [UI] private Adjustment _b = null;
        [UI] private Adjustment _c = null;

        [UI] private Adjustment _meridiansCount = null;
        [UI] private Adjustment _parallelsCount = null;

        // опции
        [UI] private CheckButton _allowZBuffer = null;
        [UI] private CheckButton _allowNormals = null;
        [UI] private CheckButton _allowWireframe = null;
        [UI] private CheckButton _allowInvisPoly = null;

        //материал
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
        
        //точечный источник света
        [UI] private CheckButton _allowPointLightVisible = null;

        [UI] private Adjustment _pointLightIntensityR = null;
        [UI] private Adjustment _pointLightIntensityG = null;
        [UI] private Adjustment _pointLightIntensityB = null;

        [UI] private Adjustment _pointLightPositionX = null;
        [UI] private Adjustment _pointLightPositionY = null;
        [UI] private Adjustment _pointLightPositionZ = null;

        [UI] private Adjustment _attenuationСoefficient = null;

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
        private float _mouseRotationSensitivity = 1f / 1000f;
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

        private CairoSurface _surface;

        private enum Projection
        {
            None,
            Front,
            Right,
            Top,
            Isometric
        }

        private enum Shading
        {
            Flat,
            Gouraud
        }

        #region выстраиваивание сцены

        private Camera _camera = new Camera(new Vector3(0, 3, 0), new Vector3(0, 0, 0), 
            1, 60, (float)0.01, (float)1000);
        private Mesh _figure = new Ellipsoid(1, 1, 1, 16, 8);
        private PointLight _pointLight = new PointLight(3, 0, 0, 1, 1, 1);

        #endregion

        public MainWindow() : this(new Builder("CGLab4.glade"))
        {
            _transformationMatrix = Matrix4x4.Identity;

            _figure.TriangulateSquares();
        }

        private MainWindow(Builder builder) : base(builder.GetRawOwnedObject("MainWindow"))
        {
            builder.Autoconnect(this);
            DeleteEvent += (o, args) => Application.Quit();
            
            _glArea.Realized += GLInit;

            _glArea.SizeAllocated += (o, args) =>
            {
                _camera.AspectRatio = (float)args.Allocation.Width / (float)args.Allocation.Height;
            };

            #region Обработка спинбатоннов, чекбоксов и комботекстбоксов

            _xPosition.ValueChanged += (o, args) =>
            {
                // _camera.Position.X = (float)_xPosition.Value;
            };
            _xPosition.ValueChanged += (o, args) =>
            {
                // _camera.Position.Y = (float)_yPosition.Value;
                
            };
            _xPosition.ValueChanged += (o, args) =>
            {
                // _camera.Position.Z = (float)_zPosition.Value;
                
            };

            _xRotation.ValueChanged += (o, args) =>
            {
                // _camera.Rotation.X = (float)_xRotation.Value;
                
            };
            _yRotation.ValueChanged += (o, args) =>
            {
                // _camera.Rotation.Y = (float)_yRotation.Value;
                
            };
            _zRotation.ValueChanged += (o, args) =>
            {
                // _camera.Rotation.Z = (float)_zRotation.Value;
                
            };

            _allowNormals.Toggled += (o, args) =>
            {
                
                
            };
            _allowWireframe.Toggled += (o, args) =>
            {
                
                
            };
            _allowInvisPoly.Toggled += (o, args) =>
            {
                
                
            };
            _allowZBuffer.Toggled += (o, args) =>
            {
                
                
            };
            
            _a.ValueChanged += (o, args) =>
            {
                _figure = new Ellipsoid((float) _a.Value,
                    (float) _b.Value, (float) _c.Value,
                    (int) _meridiansCount.Value,
                    (int) _parallelsCount.Value);
                _figure.TriangulateSquares();
                _figure.SetColor((float) _materialColorR.Value,
                    (float) _materialColorG.Value,
                    (float) _materialColorB.Value);
                
            };
            _b.ValueChanged += (o, args) =>
            {
                _figure = new Ellipsoid((float) _a.Value,
                    (float) _b.Value, (float) _c.Value,
                    (int) _meridiansCount.Value,
                    (int) _parallelsCount.Value);
                _figure.TriangulateSquares();
                _figure.SetColor((float) _materialColorR.Value,
                    (float) _materialColorG.Value,
                    (float) _materialColorB.Value);
                
            };
            _c.ValueChanged += (o, args) =>
            {
                _figure = new Ellipsoid((float) _a.Value,
                    (float) _b.Value, (float) _c.Value,
                    (int) _meridiansCount.Value,
                    (int) _parallelsCount.Value);
                _figure.TriangulateSquares();
                _figure.SetColor((float) _materialColorR.Value,
                    (float) _materialColorG.Value,
                    (float) _materialColorB.Value);
                
            };
            _meridiansCount.ValueChanged += (o, args) =>
            {
                _figure = new Ellipsoid((float) _a.Value,
                    (float) _b.Value, (float) _c.Value,
                    (int) _meridiansCount.Value,
                    (int) _parallelsCount.Value);
                _figure.TriangulateSquares();
                _figure.SetColor((float) _materialColorR.Value,
                    (float) _materialColorG.Value,
                    (float) _materialColorB.Value);
                
            };
            _parallelsCount.ValueChanged += (o, args) =>
            {
                _figure = new Ellipsoid((float) _a.Value,
                    (float) _b.Value, (float) _c.Value,
                    (int) _meridiansCount.Value,
                    (int) _parallelsCount.Value);
                _figure.TriangulateSquares();
                _figure.SetColor((float) _materialColorR.Value,
                    (float) _materialColorG.Value,
                    (float) _materialColorB.Value);
                
            };

            #endregion

            #region Обработка матрицы

            _m11.ValueChanged += (o, args) =>
            {
                _transformationMatrix.M11 = (float) _m11.Value;
                
            };
            _m12.ValueChanged += (o, args) =>
            {
                _transformationMatrix.M12 = (float) _m12.Value;
                
            };
            _m13.ValueChanged += (o, args) =>
            {
                _transformationMatrix.M13 = (float) _m13.Value;
                
            };
            _m14.ValueChanged += (o, args) =>
            {
                _transformationMatrix.M14 = (float) _m14.Value;
                
            };
            _m21.ValueChanged += (o, args) =>
            {
                _transformationMatrix.M21 = (float) _m21.Value;
                
            };
            _m22.ValueChanged += (o, args) =>
            {
                _transformationMatrix.M22 = (float) _m22.Value;
                
            };
            _m23.ValueChanged += (o, args) =>
            {
                _transformationMatrix.M23 = (float) _m23.Value;
                
            };
            _m24.ValueChanged += (o, args) =>
            {
                _transformationMatrix.M24 = (float) _m24.Value;
                
            };
            _m31.ValueChanged += (o, args) =>
            {
                _transformationMatrix.M31 = (float) _m31.Value;
                
            };
            _m32.ValueChanged += (o, args) =>
            {
                _transformationMatrix.M32 = (float) _m32.Value;
                
            };
            _m33.ValueChanged += (o, args) =>
            {
                _transformationMatrix.M33 = (float) _m33.Value;
                
            };
            _m34.ValueChanged += (o, args) =>
            {
                _transformationMatrix.M34 = (float) _m34.Value;
                
            };
            _m41.ValueChanged += (o, args) =>
            {
                _transformationMatrix.M41 = (float) _m41.Value;
                
            };
            _m42.ValueChanged += (o, args) =>
            {
                _transformationMatrix.M42 = (float) _m42.Value;
                
            };
            _m43.ValueChanged += (o, args) =>
            {
                _transformationMatrix.M43 = (float) _m43.Value;
                
            };
            _m44.ValueChanged += (o, args) =>
            {
                _transformationMatrix.M44 = (float) _m44.Value;
                
            };

            #endregion

            #region Обработка мыши

            _glArea.Events |= EventMask.ScrollMask | EventMask.PointerMotionMask | EventMask.ButtonPressMask |
                              EventMask.ButtonReleaseMask;

            _glArea.ButtonPressEvent += (o, args) =>
            {
                _mousePressedButton = args.Event.Button;
                _mousePosition.X = (float) args.Event.X;
                _mousePosition.Y = (float) args.Event.Y;
            };

            _glArea.MotionNotifyEvent += (o, args) =>
            {
                Vector3 _currentMousePosition = new Vector3((float) args.Event.X, (float) args.Event.Y, 0);
                Matrix4x4 rotationtransformation = _camera.CalculateRotationMatrix();
                
                if (_mousePressedButton == 1)
                {
                    // Vector4 shift = Vector4.Transform(new Vector4((_currentMousePosition.X - _mousePosition.X) / 200, 
                    //     (_currentMousePosition.Y - _mousePosition.Y) / 200, 0, 0), rotationtransformation);
                    Vector4 shift = Vector4.Transform(new Vector4((_currentMousePosition.X - _mousePosition.X) / 200, 
                        (_currentMousePosition.Y - _mousePosition.Y) / 200, 0, 0), rotationtransformation);
                    
                    
                    _xPosition.Value = shift.X;
                    _yPosition.Value = shift.Y;
                    _zPosition.Value = shift.Z;
                }

                if (_mousePressedButton == 3)
                {
                    Matrix4x4 mouseRotation = Matrix4x4.CreateRotationX((_currentMousePosition.Y - _mousePosition.Y) * _mouseRotationSensitivity);
                    mouseRotation *= Matrix4x4.CreateRotationY((_currentMousePosition.X - _mousePosition.X) * _mouseRotationSensitivity);

                    Matrix4x4 currnetRotation = mouseRotation * 
                                                Matrix4x4.CreateRotationX((float) (_xRotation.Value * Math.PI / 180)) *
                                                Matrix4x4.CreateRotationY((float) (_yRotation.Value * Math.PI / 180)) *
                                                Matrix4x4.CreateRotationZ((float) (_zRotation.Value * Math.PI / 180));

                    //для углов Эйлера
                    MatrixToAngles(currnetRotation, out var x, out var y, out var z);
                    _xRotation.Value = x;
                    _yRotation.Value = y;
                    _zRotation.Value = z;
                }

                _mousePosition = _currentMousePosition;
            };

            _glArea.ButtonReleaseEvent += (o, args) => _mousePressedButton = 0;

            _glArea.ScrollEvent += (o, args) =>
            {
                Vector4 shift = new Vector4(0, 0, (float)0.2, 0);
                shift = Vector4.Transform(shift, _camera.CalculateRotationMatrix());
                
                if (args.Event.Direction == ScrollDirection.Up)
                {
                    _xPosition.Value -= shift.X;
                    _yPosition.Value -= shift.Y;
                    _zPosition.Value -= shift.Z;
                }
                else if (args.Event.Direction == ScrollDirection.Down)
                {
                    _xPosition.Value += shift.X;
                    _yPosition.Value += shift.Y;
                    _zPosition.Value += shift.Z;
                }
            };

            #endregion
        }

        private void Window_DeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
        }

        #region Отрисовка фигруы

        #endregion
        
        private static void MatrixToAngles(Matrix4x4 matrix, out double x, out double y, out double z)
        {
            //область определения аркстангенса от pi/2 до -pi/2
            x = Math.Atan2(matrix.M23, matrix.M33) / Math.PI * 180;
            y = Math.Atan2(-matrix.M13, Math.Sqrt(1 - matrix.M13 * matrix.M13)) / Math.PI * 180;
            z = Math.Atan2(matrix.M12, matrix.M11) / Math.PI * 180;
        }

        private void CalculateAxisTransformationMatrix()
        {
            _axisTransformMatrix = Matrix4x4.CreateRotationX((float) (_xRotation.Value * Math.PI / 180)) *
                                   Matrix4x4.CreateRotationY((float) (_yRotation.Value * Math.PI / 180)) *
                                   Matrix4x4.CreateRotationZ((float) (_zRotation.Value * Math.PI / 180));
            _axisTransformMatrix *= Matrix4x4.CreateTranslation(_axisPosition.X, _axisPosition.Y, 0);
        }

        private void GLInit(object sender, EventArgs args)
        {
            var glArea = sender as GLArea;
            var gl = new OpenGL();
            glArea.MakeCurrent();
            
            var frame_clock = glArea.Context.Window.FrameClock;
            frame_clock.Update += (_, _) => glArea.QueueRender();
            frame_clock.BeginUpdating();

            #region сборка шейдоров и шейдерной программы

            uint vertexShader;
            vertexShader = gl.CreateShader(OpenGL.GL_VERTEX_SHADER);
            string vertexShaderSource = ReadFromRes(@"CGLab4.shader.vert");
            gl.ShaderSource(vertexShader, vertexShaderSource);
            gl.CompileShader(vertexShader);
            
            //TODO добавить вывод ошибок компиляции
            var txt = new StringBuilder(512);
            int[] tmp = new int[1];
            gl.GetShaderInfoLog(vertexShader, 512, IntPtr.Zero, txt);
            gl.GetShader(vertexShader, OpenGL.GL_COMPILE_STATUS, tmp);
            if (tmp[0] != OpenGL.GL_TRUE) Debug.WriteLine(txt);
            Debug.Assert(tmp[0] == OpenGL.GL_TRUE, "Vertexes Shader compilation failed");
            
            uint fragmentShader;
            fragmentShader = gl.CreateShader(OpenGL.GL_FRAGMENT_SHADER);
            string fragmentShaderSource = ReadFromRes(@"CGLab4.shader.frag");
            gl.ShaderSource(fragmentShader, fragmentShaderSource);
            gl.CompileShader(fragmentShader);
            
            //TODO добавить вывод ошибок компиляции
            gl.GetShaderInfoLog(fragmentShader, 512, IntPtr.Zero, txt);
            gl.GetShader(fragmentShader, OpenGL.GL_COMPILE_STATUS, tmp);
            if (tmp[0] != OpenGL.GL_TRUE) Debug.WriteLine(txt);
            Debug.Assert(tmp[0] == OpenGL.GL_TRUE, "Fragmet Shader compilation failed");
            
            uint shaderProgram;
            shaderProgram = gl.CreateProgram();
            
            gl.AttachShader(shaderProgram, vertexShader);
            gl.AttachShader(shaderProgram, fragmentShader);
            gl.LinkProgram(shaderProgram);
            
            //TODO добавить вывод ошибок компиляции
            gl.GetProgram(shaderProgram, OpenGL.GL_LINK_STATUS, tmp);
            Debug.Assert(tmp[0] == OpenGL.GL_TRUE, "Shader program link failed");

            gl.DeleteShader(vertexShader);
            gl.DeleteShader(fragmentShader);
            
            #endregion

            //перевожу координаты верши в массив float
            List<float> vertices = new List<float>();
            for (int i = 0; i < _figure.Vertices.Count; ++i)
            {
                vertices.Add(_figure.Vertices[i].Position.X);
                vertices.Add(_figure.Vertices[i].Position.Y);
                vertices.Add(_figure.Vertices[i].Position.Z);
            }
            
            // создать объект вершинного массива
            uint[] arrays = new uint[1];
            gl.GenVertexArrays(1, arrays);
            uint VAO = arrays[0];
            // создать буффер вершин
            uint[] buffers = new uint[2];
            gl.GenBuffers(2, buffers);
            uint VBO = buffers[0], VIO = buffers[1];
            
            gl.BindVertexArray(VAO);
            //загрузить данные в буфер
            gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, VBO);
            gl.BindBuffer(OpenGL.GL_ELEMENT_ARRAY_BUFFER, VIO);
            
            // данные о вершинах 
            gl.BufferData(OpenGL.GL_ARRAY_BUFFER, vertices.ToArray(), OpenGL.GL_DYNAMIC_DRAW);
            // массив индексов
            uint[] indexes = _figure.GetEnumerationOfVertexes().ToArray();
            gl.BufferData(OpenGL.GL_ELEMENT_ARRAY_BUFFER, indexes, OpenGL.GL_DYNAMIC_DRAW);
            
            gl.VertexAttribPointer(0, 3, OpenGL.GL_FLOAT, false, 3 * sizeof(float), IntPtr.Zero);
            gl.EnableVertexAttribArray(0);

            gl.BindVertexArray(0);
            
            //настройка параметров 
            gl.FrontFace(OpenGL.GL_CCW);
            
            gl.Enable(OpenGL.GL_DEPTH_TEST);
            gl.DepthFunc(OpenGL.GL_LESS);
            
            // gl.Enable(OpenGL.GL_BLEND);
            // gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            
            gl.Enable(OpenGL.GL_CULL_FACE);
            gl.CullFace(OpenGL.GL_BACK);
            
            // gl.Enable(OpenGL.GL_LINE_SMOOTH);
            // gl.Hint(OpenGL.GL_LINE_SMOOTH_HINT, OpenGL.GL_NICEST);
            
            gl.ClearColor(0, 0, 0, 1);
            
            glArea.Render += (o, args) =>
            {
                //обновитья данные камеры
                _camera.Position.X = (float)_xPosition.Value;
                _camera.Position.Y = (float)_yPosition.Value;
                _camera.Position.Z = (float)_zPosition.Value;
                
                _camera.Rotation.X = (float)_xRotation.Value;
                _camera.Rotation.Y = (float)_yRotation.Value;
                _camera.Rotation.Z = (float)_zRotation.Value;
                
                gl.UseProgram(shaderProgram);
                //отчистить буферы
                gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
                gl.ClearDepth(1.0f);    // 0 - ближе, 1 - далеко 
                gl.ClearStencil(0);

                int transformationMatrixLocation = gl.GetUniformLocation(shaderProgram, "tramsformation");
                Matrix4x4 cameraTransformationMatrix = _camera.GetProjectionMatrix() * _camera.GetViewMatrix();
                gl.UniformMatrix4(transformationMatrixLocation, 1, false,  ToArray(cameraTransformationMatrix));

                gl.BindVertexArray(VAO);
                gl.DrawElements(OpenGL.GL_TRIANGLES, indexes.Length, OpenGL.GL_UNSIGNED_INT, IntPtr.Zero);
                gl.BindVertexArray(0);
            };
            
            glArea.Unrealized += (_, _) => {
                // gl.DeleteBuffers(buffers.Length, buffers);
                // gl.DeleteVertexArrays(arrays.Length, arrays);
                // gl.DeleteProgram(shaderProgram);
            };
        }
        
        public static float[] ToArray(Matrix4x4 m)
        {
            return new float[]
            {
                m.M11, m.M21, m.M31, m.M41,
                m.M12, m.M22, m.M32, m.M42,
                m.M13, m.M23, m.M33, m.M43,
                m.M14, m.M24, m.M34, m.M44
            };
        }
        
        // считать исходин шейдера по названию файла
        public static string ReadFromRes(string name) {
            // Format: "{Namespace}.{Folder}.{filename}.{Extension}"
            var assembly = Assembly.GetExecutingAssembly();
            var names = assembly.GetManifestResourceNames();
            using (Stream stream = assembly.GetManifestResourceStream(name))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}