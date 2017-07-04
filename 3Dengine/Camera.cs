using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;


namespace AiWonderTest
{


    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    /// 
    public enum CameraState
    {
        Manual, ThirdView, LookAt , PlanCamera ,Rotate , ThirdPersonCamera,CameraMode,
    }
    public class Camera
    {
        #region Fields
        public Matrix cameraViewMatrix;
        public Matrix cameraProjectionMatrix;
        public Matrix cameraWorldMatrix;
        // camera
        public Vector3 mouse_rotation;
        public Vector3 mouse_position;
        public float mouse_zoom;
        public Vector3 cameraPosition = new Vector3(0, 0, 5.0f);
        public Vector3 cameraRotation = Vector3.Zero;
        public Vector3 cameraTarget = Vector3.Zero;
        public Vector3 cameraUpVector = Vector3.Up;
        //public float fieldOfView = MathHelper.ToRadians(45.0f);
        //public float aspectRatio = 1.0f;
        public float fieldOfView = MathHelper.ToRadians(55.0f);
        public float aspectRatio = 1.5f;
        public float nearPlaneDistance = 0.1f;
        public float farPlaneDistance = 10000.0f;

        public int screenWidth, screenHeight;
        public BasicEffect effect;

        GraphicsDevice Ldevice;
        public MouseState ms, oms;
        public KeyboardState ks;
        const float position_factor = 0.01f;
        const float rotation_factor = 1.00f;
        const float zoom_factor = 0.00005f;
        public const float DEGREE2RADIAN = (3.1514926f / 180.0f);
        public const float RADIAN2DEGREE = (1.0f / DEGREE2RADIAN);

        public SpriteFont font1;
        VertexPositionTexture[] vbox;
        Int16[] vboxidx;

        IndexBuffer ib;

        public CameraState cameraState = CameraState.ThirdView;
        //
        #endregion
        public Camera(GraphicsDevice graphic, GameWindow window, ContentManager content)
        {

            SetupContent(graphic, content);
            SetupWorld(graphic, window);
            mouse_zoom = 0.04f;
            mouse_rotation.X = 34;
            mouse_rotation.Y = 15;
            defineCamera(window);

        }
        public void defineCamera(GameWindow Window)
        {
            screenWidth = Window.ClientBounds.Width;
            screenHeight = Window.ClientBounds.Height;
            aspectRatio = (float)Window.ClientBounds.Width / Window.ClientBounds.Height;
            cameraViewMatrix = Matrix.CreateLookAt(cameraPosition, cameraTarget, cameraUpVector);
            cameraProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio,
                            nearPlaneDistance, farPlaneDistance);
            cameraWorldMatrix = Matrix.Identity;
        }
        public void Window_ClientSizeChanged(GameWindow Window, object sender, EventArgs e)
        {
            // Make changes to handle the new window size.
            defineCamera(Window);

            if (effect != null)
            {
                effect.World = cameraWorldMatrix;
                effect.View = cameraViewMatrix;
                effect.Projection = cameraProjectionMatrix;
            }
        }
        public void SetRenderState(GraphicsDevice device)
        {
            // set renderstate
            /*
            device.RenderState.CullMode = CullMode.None;            
            device.RenderState.DepthBufferEnable = true; //*
            device.RenderState.DepthBufferFunction = CompareFunction.LessEqual; //*
            */


            //device.RenderState.CullMode = CullMode.None;
            device.RenderState.CullMode = CullMode.CullCounterClockwiseFace;
            device.RenderState.DepthBufferWriteEnable = true;
            device.RenderState.DepthBufferEnable = true; //*
            ///device.RenderState.DepthBufferFunction = CompareFunction.GreaterEqual; //*
            device.RenderState.DepthBufferFunction = CompareFunction.LessEqual;
            //device.RenderState.ReferenceAlpha =180;


            device.RenderState.AlphaTestEnable = false;
            device.RenderState.AlphaBlendEnable = false;
            device.RenderState.FillMode = FillMode.Solid;
            device.RenderState.StencilEnable = false;

            device.RenderState.TwoSidedStencilMode = false;


            //device.RenderState.FogEnable = true;
            //device.RenderState.FogColor = Color.Wheat;
            //device.RenderState.FogDensity = 0.01f;
            //device.RenderState.FogStart = 0.0001f;
            //device.RenderState.FogEnd = 0.001f;


            device.RenderState.AlphaTestEnable = true;
            device.RenderState.AlphaBlendEnable = false;
            device.RenderState.SourceBlend = Blend.SourceAlpha;
            device.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
            device.RenderState.DepthBufferEnable = true;
            device.RenderState.DepthBufferWriteEnable = true;
            device.RenderState.ReferenceAlpha = 180;

            Ldevice = device; // **
        }
        public void SetupWorld(GraphicsDevice device, GameWindow Window)
        {
            //--------------
            //device.RenderState.CullMode = CullMode.None;
            //device.RenderState.DepthBufferEnable = true; //*
            //device.RenderState.DepthBufferFunction = CompareFunction.LessEqual; //*

            //--------------
            // 3D - draw - setup
            //--------------
            defineCamera(Window);
            effect = new BasicEffect(device, null);
            effect.World = cameraWorldMatrix;
            effect.View = cameraViewMatrix;
            effect.Projection = cameraProjectionMatrix;
            effect.VertexColorEnabled = true;



            //texture

            //effect.Alpha = 0.5f;

            effect.TextureEnabled = true;
            //--------------
            Ldevice = device; // **
        }
        public void SetupContent(GraphicsDevice device, ContentManager content)
        {
          
        }
        #region update
        public void UpdateMouse()
        {
            ms = Mouse.GetState();

            // drag right mouse for rotation
            if (ms.RightButton == ButtonState.Pressed)
            {
                mouse_rotation.Y += (ms.X - oms.X) * rotation_factor;
                mouse_rotation.X += (ms.Y - oms.Y) * rotation_factor;
            }
            mouse_zoom += (ms.ScrollWheelValue - oms.ScrollWheelValue) * zoom_factor;
            if (mouse_zoom < 0.01f) mouse_zoom = 0.01f;
            if (mouse_zoom > 10.0f) mouse_zoom = 10.0f;
            oms = ms;
            if (ms.LeftButton == ButtonState.Pressed)
            {
                Ray mouseRay = getMouseRay();

            }
        }
        public void UpdateKeyboard()
        {
            if (ks.IsKeyDown(Keys.Left))
            {
                mouse_rotation.Y--;
            }
            if (ks.IsKeyDown(Keys.Right))
            {
                mouse_rotation.Y++;
            }
            if (ks.IsKeyDown(Keys.Up))
            {
                mouse_rotation.X++;
            }
            if (ks.IsKeyDown(Keys.Down))
            {
                mouse_rotation.X--;
            }

        }
        #endregion
        #region applyworld
        float time =0;
        public void ResetTime()
        {
            time = 0;
        }
        public void applyWorldManual(GameTime gameTime, Vector3 cameraPosition, Vector3 rotation)
        {

            UpdateMouse();
            UpdateKeyboard();
            mouse_rotation = Vector3.Zero;
            float time = (float)gameTime.TotalRealTime.Ticks * 0.0000001f;
            float sinetime = (float)Math.Sin(time);

            //Matrix T = Matrix.CreateTranslation(-0.5f * Math.Abs(sinetime), 0, 0);
            Matrix R = Matrix.CreateRotationY(2.0f * (float)Math.PI * (time * 0.1f));
            //Matrix S = Matrix.CreateScale(1.0f * sinetime) ;


            // world matrix setup
            cameraViewMatrix = Matrix.Identity;

            cameraViewMatrix *= Matrix.CreateTranslation(
                    -cameraPosition);

            // mouse control rotation
            cameraViewMatrix *= Matrix.CreateRotationZ(rotation.Z * DEGREE2RADIAN)
                * Matrix.CreateRotationY(rotation.Y * DEGREE2RADIAN)
                * Matrix.CreateRotationX(rotation.X * DEGREE2RADIAN);
            // 3rd Position Cam


            effect.View = cameraViewMatrix;
        }
        public void applyWorldThirdView(GameTime gameTime, Vector3 cameraLookAt, Vector3 rotation)
        {

            UpdateMouse();
            UpdateKeyboard();
            float time = (float)gameTime.TotalRealTime.Ticks * 0.0000001f;
            float sinetime = (float)Math.Sin(time);

            //Matrix T = Matrix.CreateTranslation(-0.5f * Math.Abs(sinetime), 0, 0);
            Matrix R = Matrix.CreateRotationY(2.0f * (float)Math.PI * (time * 0.1f));
            //Matrix S = Matrix.CreateScale(1.0f * sinetime) ;


            // world matrix setup
            cameraViewMatrix = Matrix.Identity;

            cameraViewMatrix *= Matrix.CreateTranslation(
                    -cameraLookAt);

            // mouse control rotation
            cameraViewMatrix *= Matrix.CreateRotationZ(rotation.Z * DEGREE2RADIAN)
                * Matrix.CreateRotationY(rotation.Y * DEGREE2RADIAN)
                * Matrix.CreateRotationX(rotation.X * DEGREE2RADIAN);
            // 3rd Position Cam
            cameraViewMatrix *= Matrix.CreateTranslation(
                            0,
                            -5,
                            -15f);

            effect.View = cameraViewMatrix;
        }
        public void applyWorldThirdPersonView(GameTime gameTime, Vector3 cameraLookAt, Vector3 rotation)
        {

            UpdateMouse();
            UpdateKeyboard();
            float time = (float)gameTime.TotalRealTime.Ticks * 0.0000001f;
            float sinetime = (float)Math.Sin(time);

            //Matrix T = Matrix.CreateTranslation(-0.5f * Math.Abs(sinetime), 0, 0);
            Matrix R = Matrix.CreateRotationY(2.0f * (float)Math.PI * (time * 0.1f));
            //Matrix S = Matrix.CreateScale(1.0f * sinetime) ;


            // world matrix setup
            cameraViewMatrix = Matrix.Identity;

            cameraViewMatrix *= Matrix.CreateTranslation(
                    -cameraLookAt);

            // mouse control rotation
            cameraViewMatrix *= Matrix.CreateRotationZ(mouse_rotation.Z*DEGREE2RADIAN)
                * Matrix.CreateRotationY(mouse_rotation.Y * DEGREE2RADIAN)
                * Matrix.CreateRotationX(mouse_rotation.X * DEGREE2RADIAN);
            // 3rd Position Cam
            cameraViewMatrix *= Matrix.CreateTranslation(
                            0,
                            -5,
                            -15f);

            effect.View = cameraViewMatrix;
        }
        public void applyWorldLookAt(GameTime gameTime, Vector3 lookAtPosition, Vector3 cameraPosition)
        {
            mouse_rotation = Vector3.Zero;
            float time = (float)gameTime.TotalRealTime.Ticks * 0.0000001f;
            float sinetime = (float)Math.Sin(time);

            //Matrix T = Matrix.CreateTranslation(-0.5f * Math.Abs(sinetime), 0, 0);
            //Matrix R = Matrix.CreateRotationY(2.0f * (float)Math.PI * (time * 0.1f));
            //Matrix S = Matrix.CreateScale(1.0f * sinetime) ;


            // world matrix setup
            cameraViewMatrix = Matrix.Identity;

            cameraViewMatrix = Matrix.CreateLookAt(cameraPosition, lookAtPosition, cameraUpVector);

            effect.View = cameraViewMatrix;

        }
        public void applyWorldPlanView(GameTime gameTime, Vector3 cameraPosition, Vector3 cameraLookAt, Vector3 rotation)
        {

            UpdateMouse();
            UpdateKeyboard();
            cameraRotation = rotation;
            float time = (float)gameTime.TotalRealTime.Ticks * 0.0000001f;
            float sinetime = (float)Math.Sin(time);

            //Matrix T = Matrix.CreateTranslation(-0.5f * Math.Abs(sinetime), 0, 0);
            Matrix R = Matrix.CreateRotationY(2.0f * (float)Math.PI * (time * 0.1f));
            //Matrix S = Matrix.CreateScale(1.0f * sinetime) ;


            // world matrix setup
            cameraViewMatrix = Matrix.Identity;

            // mouse control rotation
            cameraViewMatrix = Matrix.CreateLookAt(cameraPosition, cameraLookAt, cameraUpVector);
            // 3rd Position Cam

            effect.View = cameraViewMatrix;
        }
        public void applyWorldRotateView(GameTime gameTime, Vector3 cameraLookAt, Vector3 rotation, Vector3 offsetPosition)
        {

            UpdateMouse();
            UpdateKeyboard();
            time += (float)gameTime.ElapsedGameTime.Ticks * 0.0000001f;
            float sinetime = (float)Math.Sin(time);
            Matrix R = Matrix.Identity;
            //Matrix T = Matrix.CreateTranslation(-0.5f * Math.Abs(sinetime), 0, 0);
            R = Matrix.CreateRotationY(2.0f * (float)Math.PI * (time * 0.1f));
            //Matrix S = Matrix.CreateScale(1.0f * sinetime) ;


            // world matrix setup
            cameraViewMatrix = Matrix.Identity;

            cameraViewMatrix *= Matrix.CreateTranslation(-cameraLookAt);
            //cameraViewMatrix = Matrix.CreateLookAt(cameraPosition, cameraLookAt, cameraUpVector);
            // mouse control rotation
            cameraViewMatrix *= Matrix.CreateRotationZ(rotation.Z * DEGREE2RADIAN)
                * Matrix.CreateRotationY(rotation.Y * DEGREE2RADIAN)
                * Matrix.CreateRotationX(rotation.X * DEGREE2RADIAN)
                *R;
            // 3rd Position Cam
            cameraViewMatrix *= Matrix.CreateTranslation(-offsetPosition);

            effect.View = cameraViewMatrix;
        }
        public void applyWorldCameraMode(GameTime gameTime, Vector3 position, Vector3 rotation)
        {

            UpdateMouse();
            UpdateKeyboard();
            float time = (float)gameTime.TotalRealTime.Ticks * 0.0000001f;
            float sinetime = (float)Math.Sin(time);

            //Matrix T = Matrix.CreateTranslation(-0.5f * Math.Abs(sinetime), 0, 0);
            Matrix R = Matrix.CreateRotationY(2.0f * (float)Math.PI * (time * 0.1f));
            //Matrix S = Matrix.CreateScale(1.0f * sinetime) ;


            // world matrix setup
            cameraViewMatrix = Matrix.Identity;


            cameraViewMatrix *= Matrix.CreateTranslation(
                    -position + Vector3.One);
            // mouse control rotation
            cameraViewMatrix *= Matrix.CreateRotationZ(mouse_rotation.Z * DEGREE2RADIAN)
                 * Matrix.CreateRotationY(mouse_rotation.Y * DEGREE2RADIAN)
                 * Matrix.CreateRotationX(mouse_rotation.X * DEGREE2RADIAN);

            
            // 3rd Position Cam


            effect.View = cameraViewMatrix;
        }
        #endregion


        public Vector3 point2screen(Vector3 vp)
        {
            /*
            Vector3 GraphicsDevice.Viewport.Project(Vector3 source, Matrix projection, Matrix view, Matrix world)
            * http://thunderfist-podium.blogspot.com/2009/08/xna-screen-projection.html
            * http://msdn.microsoft.com/en-us/library/bb447672(XNAGameStudio.10).aspx
            */

            Vector3 vs;
            vs = Ldevice.Viewport.Project(vp, cameraProjectionMatrix,
                                                cameraViewMatrix,
                                                cameraWorldMatrix);
            return vs;

        }
        public Vector3 screen2point(Vector3 vs)
        {
            /*
             * http://msdn.microsoft.com/en-us/library/microsoft.xna.framework.graphics.viewport.unproject(XNAGameStudio.10).aspx
             */

            Vector3 vp;
            vp = Ldevice.Viewport.Unproject(vs, cameraProjectionMatrix,
                                                cameraViewMatrix,
                                                cameraWorldMatrix);

            return vp;

        }
        public Ray getMouseRay()
        {
            // (How To: Detect Whether a User Clicked a 3D Object)
            // http://msdn.microsoft.com/en-us/library/bb203905.aspx

            Vector3 nearPoint = screen2point(new Vector3(ms.X, ms.Y, 0f));//near
            Vector3 farPoint = screen2point(new Vector3(ms.X, ms.Y, 1f));//far

            // Create a ray from the near clip plane to the far clip plane.
            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();
            Ray pickRay = new Ray(nearPoint, direction);

            return pickRay;
        }
        public Vector3 mousetofloor(float floor_y_level)
        {
            // (How To: Detect Whether a User Clicked a 3D Object)
            // http://msdn.microsoft.com/en-us/library/bb203905.aspx

            Vector3 nearPoint = screen2point(new Vector3(ms.X, ms.Y, 0f));//near
            Vector3 farPoint = screen2point(new Vector3(ms.X, ms.Y, 1f));//far

            // Create a ray from the near clip plane to the far clip plane.
            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();
            Ray pickRay = new Ray(nearPoint, direction);

            // (Plane Structure)
            // http://msdn.microsoft.com/en-us/library/microsoft.xna.framework.plane.aspx

            Plane floorPlane = new Plane(new Vector3(0, 1, 0), -floor_y_level);

            // (Ray.Intersects Method (Plane))
            // http://msdn.microsoft.com/en-us/library/bb464124.aspx

            Nullable<float> cdistance = pickRay.Intersects(floorPlane);

            if (cdistance != null)
            {
                Vector3 floor_at_y_level = pickRay.Position
                    + pickRay.Direction * (float)cdistance;

                return floor_at_y_level;
            }
            else
            {
                return new Vector3(-12345f, -12345f, -12345f);
            }

        }
        public void DrawLine(Vector3 p1, Vector3 p2, Color C)
        {
            bool temp = effect.TextureEnabled;
            effect.TextureEnabled = false;
            VertexPositionColor[] verts;
            verts = new VertexPositionColor[3];
            verts[0] = new VertexPositionColor(p1, C);
            verts[1] = new VertexPositionColor(p2, C);

            Ldevice.VertexDeclaration =
                new VertexDeclaration(Ldevice, VertexPositionColor.VertexElements);
            effect.Begin();
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Begin();
                Ldevice.DrawUserPrimitives<VertexPositionColor>
                    (PrimitiveType.LineList, verts, 0, 1);
                pass.End();
            }
            effect.End();
            effect.TextureEnabled = temp;
        }
        public void fix3DBug()
        {
            //http://blogs.msdn.com/shawnhar/archive/2006/11/13/spritebatch-and-renderstates.aspx
            Ldevice.RenderState.CullMode = CullMode.CullCounterClockwiseFace;
            Ldevice.RenderState.DepthBufferEnable = false;

            Ldevice.RenderState.AlphaBlendEnable = true;
            Ldevice.RenderState.AlphaBlendOperation = BlendFunction.Add;
            Ldevice.RenderState.SourceBlend = Blend.SourceAlpha;
            Ldevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
            Ldevice.RenderState.SeparateAlphaBlendEnabled = false;

            Ldevice.RenderState.AlphaTestEnable = true;
            Ldevice.RenderState.AlphaFunction = CompareFunction.Greater;
            Ldevice.RenderState.ReferenceAlpha = 0;

            Ldevice.SamplerStates[0].AddressU = TextureAddressMode.Clamp;
            Ldevice.SamplerStates[0].AddressV = TextureAddressMode.Clamp;

            Ldevice.SamplerStates[0].MagFilter = TextureFilter.Linear;
            Ldevice.SamplerStates[0].MinFilter = TextureFilter.Linear;
            Ldevice.SamplerStates[0].MipFilter = TextureFilter.Linear;

            Ldevice.SamplerStates[0].MipMapLevelOfDetailBias = 0.0f;
            Ldevice.SamplerStates[0].MaxMipLevel = 0;

            Ldevice.RenderState.DepthBufferEnable = true;
            Ldevice.RenderState.AlphaBlendEnable = false;
            Ldevice.RenderState.AlphaTestEnable = false;

            Ldevice.SamplerStates[0].AddressU = TextureAddressMode.Wrap;
            Ldevice.SamplerStates[0].AddressV = TextureAddressMode.Wrap;
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
    }
}