using System;
using System.Collections.Generic;
using System.Numerics;
using Gtk;
using Gdk;
using System.IO;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using CGLab7;
using SharpGL;
using Application = Gtk.Application;
using Window = Gtk.Window;
using UI = Gtk.Builder.ObjectAttribute;

namespace CG
{
    class MainWindow : Window
    {
        [UI] private GLArea _glArea = null;

        #region Мышь

        private Vector3 _mousePosition;
        private uint _mousePressedButton;

        #endregion
        
        private Matrix4x4 _transformationMatrix = Matrix4x4.Identity;
        float minDist = 1000000000;
        int minInd = -1;
        private bool _splineChanged = true;

        #region выстраиваивание сцены

        private CardinalSpline cardinalSpline = new CardinalSpline(new Vector4(0.2f, -0.5f, 0f, 1.0f), 
                                                                    new Vector4(0.4f, 0.5f, 0f, 1.0f), 
                                                                    new Vector4(0.6f, 0.5f, 0f, 1.0f),
                                                                    new Vector4(0.8f, -0.5f, 0f, 1.0f));

        #endregion

        public MainWindow() : this(new Builder("CGLab7.glade")) {}

        private MainWindow(Builder builder) : base(builder.GetRawOwnedObject("MainWindow"))
        {
            builder.Autoconnect(this);
            DeleteEvent += (o, args) => Application.Quit();
            
            _glArea.Realized += GLInit;

            _glArea.SizeAllocated += (o, args) =>
            {
                // _transformationMatrix.M11
                float aspectRatio = (float)args.Allocation.Width / (float)args.Allocation.Height;
                _transformationMatrix *= Matrix4x4.CreateScale(new Vector3(aspectRatio, 1 / aspectRatio, 0f));
            };

            #region Обработка мыши

            _glArea.Events |= EventMask.ScrollMask | EventMask.PointerMotionMask | EventMask.ButtonPressMask |
                              EventMask.ButtonReleaseMask;

            _glArea.ButtonPressEvent += (o, args) =>
            {
                _mousePressedButton = args.Event.Button;
                _mousePosition.X = (float) args.Event.X;
                _mousePosition.Y = (float) args.Event.Y;
                
                Vector4 position = new Vector4((float) (args.Event.X - (float) Window.Width / 2) / ((float) Window.Width / 2), 
                    (float) -(args.Event.Y - (float) Window.Height / 2) / ((float) Window.Height / 2), 0f, 1.0f);

                minDist = 1000000000;
                minInd = -1;
                for (int i = 0; i < cardinalSpline.Points.Count; ++i)
                {
                    if (Math.Abs((position - cardinalSpline.Points[i]).Length()) < minDist)
                    {
                        minDist = Math.Abs((position - cardinalSpline.Points[i]).Length());
                        minInd = i;
                    }
                }
                if (minDist > 0.1)
                {
                    minInd = -1;
                }
            };

            _glArea.MotionNotifyEvent += (o, args) =>
            {
                Vector3 _currentMousePosition = new Vector3((float) args.Event.X, (float) args.Event.Y, 0);
                Vector4 position = new Vector4((float) args.Event.X / Window.Width, (float) args.Event.Y / Window.Height, 0f, 0f);
                Vector4 shift = new Vector4((float) (_currentMousePosition.X - _mousePosition.X) / ((float) Window.Width / 2),
                                            (float) -(_currentMousePosition.Y - _mousePosition.Y) / ((float) Window.Height / 2), 0f, 0f);
                
                if (_mousePressedButton == 1)
                {
                    if (minInd != -1)
                    {
                        cardinalSpline.Points[minInd] += shift;
                        _splineChanged = true;
                    }
                }
                if (_mousePressedButton == 3)
                {
                    _transformationMatrix *= Matrix4x4.CreateTranslation(shift.X, shift.Y, 0f);
                }

                _mousePosition = _currentMousePosition;
            };

            _glArea.ButtonReleaseEvent += (o, args) => _mousePressedButton = 0;

            _glArea.ScrollEvent += (o, args) =>
            {
                if (args.Event.Direction == ScrollDirection.Up)
                {
                    _transformationMatrix *= Matrix4x4.CreateScale(1.25f, 1.25f, 0f);
                }
                else if (args.Event.Direction == ScrollDirection.Down)
                {
                    _transformationMatrix *= Matrix4x4.CreateScale(0.8f, 0.8f, 0f);
                }
            };

            #endregion
        }

        private void GLInit(object sender, EventArgs args)
        {
            var glArea = sender as GLArea;
            glArea.MakeCurrent();
            var gl = new OpenGL();
            
            var frame_clock = glArea.Context.Window.FrameClock;
            frame_clock.Update += (_, _) => glArea.QueueRender();
            frame_clock.BeginUpdating();

            #region сборка шейдоров и шейдерной программы

            uint vertexShader;
            vertexShader = gl.CreateShader(OpenGL.GL_VERTEX_SHADER);
            string vertexShaderSource = ReadFromRes(@"CGLab7.shader.vert");
            gl.ShaderSource(vertexShader, vertexShaderSource);
            gl.CompileShader(vertexShader);
            
            var txt = new StringBuilder(512);
            int[] tmp = new int[1];
            gl.GetShaderInfoLog(vertexShader, 512, IntPtr.Zero, txt);
            gl.GetShader(vertexShader, OpenGL.GL_COMPILE_STATUS, tmp);
            if (tmp[0] != OpenGL.GL_TRUE) Debug.WriteLine(txt);
            Debug.Assert(tmp[0] == OpenGL.GL_TRUE, "Vertexes Shader compilation failed");

            uint fragmentShader;
            fragmentShader = gl.CreateShader(OpenGL.GL_FRAGMENT_SHADER);
            string fragmentShaderSource = ReadFromRes(@"CGLab7.shader.frag");
            gl.ShaderSource(fragmentShader, fragmentShaderSource);
            gl.CompileShader(fragmentShader);
            
            gl.GetShaderInfoLog(fragmentShader, 512, IntPtr.Zero, txt);
            gl.GetShader(fragmentShader, OpenGL.GL_COMPILE_STATUS, tmp);
            if (tmp[0] != OpenGL.GL_TRUE) Debug.WriteLine(txt);
            Debug.Assert(tmp[0] == OpenGL.GL_TRUE, "Fragmet Shader compilation failed");
            
            uint shaderProgram, polygonNormalsShaderProgram;
            shaderProgram = gl.CreateProgram();
            polygonNormalsShaderProgram = gl.CreateProgram();

            gl.AttachShader(shaderProgram, vertexShader);
            gl.AttachShader(shaderProgram, fragmentShader);
            gl.LinkProgram(shaderProgram);

            gl.GetProgram(shaderProgram, OpenGL.GL_LINK_STATUS, tmp);
            Debug.Assert(tmp[0] == OpenGL.GL_TRUE, "Shader program link failed");

            gl.AttachShader(polygonNormalsShaderProgram, vertexShader);
            gl.AttachShader(polygonNormalsShaderProgram, fragmentShader);
            gl.LinkProgram(polygonNormalsShaderProgram);
            
            gl.GetProgram(polygonNormalsShaderProgram, OpenGL.GL_LINK_STATUS, tmp);
            Debug.Assert(tmp[0] == OpenGL.GL_TRUE, "Normals program link failed");
            
            gl.DeleteShader(vertexShader);
            gl.DeleteShader(fragmentShader);
            
            #endregion
            
            // создать объект вершинного массива
            uint[] arrays = new uint[2];
            gl.GenVertexArrays(2, arrays);
            uint mainVAO = arrays[0];
            uint pointVAO = arrays[1];

            // создать буффер вершин
            uint[] buffers = new uint[3];
            gl.GenBuffers(3, buffers);
            uint VBO = buffers[0];
            uint VIO = buffers[1];
            uint pointVBO = buffers[2];

            List<uint> indexes = new List<uint>();
            
            gl.BindVertexArray(mainVAO);
                gl.UseProgram(shaderProgram);
                gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, VBO);
                gl.BindBuffer(OpenGL.GL_ELEMENT_ARRAY_BUFFER, VIO);
                gl.VertexAttribPointer((uint)gl.GetAttribLocation(shaderProgram, "position"), 2, OpenGL.GL_FLOAT, false, 2 * sizeof(float), IntPtr.Zero);
                gl.EnableVertexAttribArray((uint)gl.GetAttribLocation(shaderProgram, "position"));
            gl.BindVertexArray(0);
            gl.BindVertexArray(pointVAO);
                gl.UseProgram(shaderProgram);
                gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, pointVBO);
                gl.VertexAttribPointer((uint)gl.GetAttribLocation(shaderProgram, "position"), 2, OpenGL.GL_FLOAT, false, 2 * sizeof(float), IntPtr.Zero);
                gl.EnableVertexAttribArray((uint)gl.GetAttribLocation(shaderProgram, "position"));
            gl.BindVertexArray(0);

            #region настройка параметров

            gl.FrontFace(OpenGL.GL_CW);
            
            gl.Enable(OpenGL.GL_DEPTH_TEST);
            gl.DepthFunc(OpenGL.GL_LESS);
            
            gl.Enable(OpenGL.GL_BLEND);
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            
            gl.Enable(OpenGL.GL_CULL_FACE);
            gl.CullFace(OpenGL.GL_BACK);

            gl.ClearColor(0.2f, 0.2f, 0.2f, 1);

            #endregion

            glArea.Render += (o, args) =>
            {
                gl.UseProgram(shaderProgram);
                //отчистить буферы
                gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
                gl.ClearDepth(1.0f); // 0 - ближе, 1 - далеко 
                gl.ClearStencil(0);

                #region отрисовка
                
                if (_splineChanged)
                {
                    _splineChanged = false;

                    //интерполяция
                    int interpolationQuality = 10;
                    List<float> points = new List<float>();
                    for (int i = 0; i < cardinalSpline.Points.Count; ++i)
                    {
                        points.Add(cardinalSpline.Points[i].X);
                        points.Add(cardinalSpline.Points[i].Y);
                    }
                    List<float> interpolatedPoints = new List<float>();
                    indexes.Clear();
                    for (int i = 1; i < cardinalSpline.Points.Count - 2; ++i)
                    {
                        for (int j = 0; j <= interpolationQuality; ++j)
                        {
                            interpolatedPoints.Add(cardinalSpline.Points[i].X + ((float)j / interpolationQuality) * (cardinalSpline.Points[i + 1].X - cardinalSpline.Points[i].X));
                            interpolatedPoints.Add(cardinalSpline.Interpolate(i, (float) j / interpolationQuality));
                            indexes.Add((uint)indexes.Count);
                        }
                    }
                    
                    
                    #region обновить буферы

                    gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, VBO);
                    gl.BufferData(OpenGL.GL_ARRAY_BUFFER, interpolatedPoints.ToArray(), OpenGL.GL_DYNAMIC_DRAW);
                    gl.BindBuffer(OpenGL.GL_ELEMENT_ARRAY_BUFFER, VIO);
                    gl.BufferData(OpenGL.GL_ELEMENT_ARRAY_BUFFER, indexes.ToArray(), OpenGL.GL_DYNAMIC_DRAW);
                    gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, pointVBO);
                    gl.BufferData(OpenGL.GL_ARRAY_BUFFER, points.ToArray(), OpenGL.GL_DYNAMIC_DRAW);
                    
                    #endregion
                }
                
                // int transformationMatrixLocation = gl.GetUniformLocation(shaderProgram, "transformation");
                // gl.UniformMatrix4(transformationMatrixLocation, 1, false,  ToArray(_transformationMatrix));

                gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_LINE);
                int colorModeLocation = gl.GetUniformLocation(shaderProgram, "ColorMode");
                gl.BindVertexArray(mainVAO);
                    gl.Uniform1(colorModeLocation, 1);
                    gl.DrawElements(OpenGL.GL_LINE_STRIP, indexes.Count, OpenGL.GL_UNSIGNED_INT, IntPtr.Zero);
                gl.BindVertexArray(0);
                gl.BindVertexArray(pointVAO);
                    gl.Uniform1(colorModeLocation, 2);
                    gl.PointSize(5);
                    gl.DrawArrays(OpenGL.GL_POINTS, 0, cardinalSpline.Points.Count);
                    gl.Uniform1(colorModeLocation, 0);
                    gl.DrawArrays(OpenGL.GL_LINES, 0, 2);
                    gl.DrawArrays(OpenGL.GL_LINES, cardinalSpline.Points.Count - 2, 2);
                gl.BindVertexArray(0);
                
                #endregion
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