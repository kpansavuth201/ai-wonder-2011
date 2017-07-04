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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

using XNAnimation;
using XNAnimation.Controllers;
using XNAnimation.Effects;
using XNAnimation.Pipeline;

namespace AiWonderTest
{
   public enum ActionState
   {
       idle, battleidle, damaged, walk, run, back, changeweapon, dropweapon,castingDrop,castingStand,castingRelease, attack,dropdeath
           , standdeath, crouchidle, crouchmove
   }

   public class SkinnedModelObj
   {

      public bool spacularEnable = false;
      public bool boundingDrawEnable = true;
      public bool shadowEnable = false;

      public Vector3 diffuse = Vector3.One;

      //in feild
      Matrix[] absoluteBoneTransforms;
      public AnimationController animationController;
      public ActionState oldAnimation = ActionState.idle;
      public string modelName;

      public Vector3 position;


      public Vector3 rotation;
      public Vector3 velocity = Vector3.Zero;
      public Vector3 center;
      public Dictionary<string, BoundingBox> boundingBox;
      public BoundingBox talkingAreaCheck;
      public BoundingBox tempBoundingBox;
      public Model modelTest;

      BasicEffect effect;
      //testShadow




      Quad wall;
      Plane wallPlane;
      BasicEffect quadEffect;
      Vector3 shadowLightDir;
      VertexDeclaration wallVertexDecl;
      public GraphicsDeviceManager graphics;

      SpriteBatch spriteBatch;

      public SkinnedModelObj(string modelName, Dictionary<string, SkinnedModel> model, GraphicsDeviceManager _graphics)
      {
         graphics = _graphics;
         spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

         effect = new BasicEffect(graphics.GraphicsDevice, null);

         this.modelName = modelName;
         absoluteBoneTransforms = new Matrix[model[modelName].Model.Bones.Count];
         model[modelName].Model.CopyBoneTransformsTo(absoluteBoneTransforms);
         animationController = new AnimationController(model[modelName].SkeletonBones);
         animationController.StartClip(model[modelName].AnimationClips.Values[0]);

         boundingBox = new Dictionary<string, BoundingBox>();
         CreateBoundingBox(model[modelName].Model);

         // Create the View and Projection matrices

         // Create a new Textured Quad to represent a wall
         wall = new Quad(Vector3.Zero, Vector3.Down, Vector3.Backward, 7, 7);
         // Create a Plane using three points on the Quad
         wallPlane =
             new Plane(wall.UpperLeft, wall.UpperRight, wall.LowerLeft);

         quadEffect = new BasicEffect(graphics.GraphicsDevice, null);
         quadEffect.EnableDefaultLighting();

         quadEffect.TextureEnabled = true;
         //quadEffect.Texture = texture;

         wallVertexDecl = new VertexDeclaration(graphics.GraphicsDevice,
                     VertexPositionNormalTexture.VertexElements);
         shadowEnable = false;
      }
      public SkinnedModelObj(string modelName, Model modelBounding, Dictionary<string, SkinnedModel> model
            , GraphicsDeviceManager _graphics)
      {
         graphics = _graphics;
         spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

         effect = new BasicEffect(graphics.GraphicsDevice, null);

         this.modelName = modelName;
         absoluteBoneTransforms = new Matrix[model[modelName].Model.Bones.Count];
         model[modelName].Model.CopyBoneTransformsTo(absoluteBoneTransforms);
         animationController = new AnimationController(model[modelName].SkeletonBones);
         animationController.StartClip(model[modelName].AnimationClips.Values[0]);
         boundingBox = new Dictionary<string, BoundingBox>();
         CreateBoundingBox(modelBounding);

         // Create a new Textured Quad to represent a wall
         wall = new Quad(Vector3.Zero, Vector3.Down, Vector3.Backward, 7, 7);
         // Create a Plane using three points on the Quad
         wallPlane =
             new Plane(wall.UpperLeft, wall.UpperRight, wall.LowerLeft);
         shadowEnable = false;
      }
      public SkinnedModelObj(string modelName, Dictionary<string, SkinnedModel> model, GraphicsDeviceManager _graphics,
          Vector3 modelPosition)
      {
         graphics = _graphics;
         spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

         effect = new BasicEffect(graphics.GraphicsDevice, null);

         this.modelName = modelName;
         absoluteBoneTransforms = new Matrix[model[modelName].Model.Bones.Count];
         model[modelName].Model.CopyBoneTransformsTo(absoluteBoneTransforms);
         animationController = new AnimationController(model[modelName].SkeletonBones);
         animationController.StartClip(model[modelName].AnimationClips.Values[0]);

         boundingBox = new Dictionary<string, BoundingBox>();
         CreateBoundingBox(model[modelName].Model);

         // Create the View and Projection matrices

         // Create a new Textured Quad to represent a wall
         wall = new Quad(Vector3.Zero, Vector3.Down, Vector3.Backward, 7, 7);
         // Create a Plane using three points on the Quad
         wallPlane =
             new Plane(wall.UpperLeft, wall.UpperRight, wall.LowerLeft);

         quadEffect = new BasicEffect(graphics.GraphicsDevice, null);
         quadEffect.EnableDefaultLighting();

         quadEffect.TextureEnabled = true;
         //quadEffect.Texture = texture;

         wallVertexDecl = new VertexDeclaration(graphics.GraphicsDevice,
                     VertexPositionNormalTexture.VertexElements);

         position = modelPosition;
         TranslateBounding(position);
         shadowEnable = false;
      }
      public SkinnedModelObj(string modelName, Model modelBounding, Dictionary<string, SkinnedModel> model
           , GraphicsDeviceManager _graphics, Vector3 modelPosition)
      {
         graphics = _graphics;
         spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

         effect = new BasicEffect(graphics.GraphicsDevice, null);

         this.modelName = modelName;
         absoluteBoneTransforms = new Matrix[model[modelName].Model.Bones.Count];
         model[modelName].Model.CopyBoneTransformsTo(absoluteBoneTransforms);

         animationController = new AnimationController(model[modelName].SkeletonBones);
         animationController.StartClip(model[modelName].AnimationClips.Values[0]);

         boundingBox = new Dictionary<string, BoundingBox>();
         CreateBoundingBox(modelBounding);

         // Create a new Textured Quad to represent a wall
         wall = new Quad(Vector3.Zero, Vector3.Down, Vector3.Backward, 7, 7);
         // Create a Plane using three points on the Quad
         wallPlane =
             new Plane(wall.UpperLeft, wall.UpperRight, wall.LowerLeft);
         position = modelPosition;
         TranslateBounding(position);
         shadowEnable = false;
      }
      public void Update(GameTime gameTime)
      {
         animationController.TranslationInterpolation = InterpolationMode.Linear;
         animationController.OrientationInterpolation = InterpolationMode.Spherical;
         animationController.ScaleInterpolation = InterpolationMode.Linear;
         animationController.Update(gameTime.ElapsedGameTime, Matrix.Identity);
         TranslateBounding();

      }
      public Vector3 setToOne(Vector3 input)
      {
         return input /= input.Y;
      }
      public void Draw(SkinnedModel model, ActionState action, Camera camera,
          Vector3 position, Vector3 rotation)
      {
         SetupModel(animationController, model, absoluteBoneTransforms, camera);//ทำให้โมเดลขยับอนิเมชั่น
         SetAnimate(model, action);
         SetupDefaultLight(model, camera);
         //SetupLight(model, Vector3.One, new Vector3(0, 100, 0));

         SetupPositionRotation(model, position, rotation);
         //SetBoundingBox(position);
         this.position = position;
         if (shadowEnable)
            DrawShadow(model, position, rotation, camera, setToOne(new Vector3(0, 100, 0)));
         //List<string> boundingIndex = new List<string>(boundingBox.Keys);
         //if (boundingDrawEnable)
         //{
         //    foreach (string bound in boundingIndex)
         //    {
         //        DrawLine(boundingBox[bound].Max, boundingBox[bound].Min, Color.WhiteSmoke, camera);
         //    }
         //}
      }
      public void Draw(Dictionary<string, SkinnedModel> model, ActionState action, Camera camera,
         Vector3 position, Vector3 rotation)
      {

         SetupModel(animationController, model[modelName], absoluteBoneTransforms, camera);//ทำให้โมเดลขยับอนิเมชั่น
         SetAnimate(model[modelName], action);
         SetupDefaultLight(model[modelName], camera);
         //SetupLight(model[modelName], Vector3.One, new Vector3(0, 100, 0));
         SetupPositionRotation(model[modelName], position, rotation);
         //SetBoundingBox(position);
         this.position = position;
         if (shadowEnable)
            DrawShadow(model, position, rotation, camera, new Vector3(0, 1, 0));
         //List<string> boundingIndex = new List<string>(boundingBox.Keys);
         //if (boundingDrawEnable)
         //{
         //    foreach (string bound in boundingIndex)
         //    {
         //        DrawLine(boundingBox[bound].Max, boundingBox[bound].Min, Color.WhiteSmoke, camera);
         //    }
         //}

      }
      public void DrawNPC(Dictionary<string, SkinnedModel> model, ActionState action, Camera camera,
              Vector3 position, Vector3 rotation)
      {

         SetupModel(animationController, model[modelName], absoluteBoneTransforms, camera);//ทำให้โมเดลขยับอนิเมชั่น
         SetAnimate(model[modelName], action);
         SetupDefaultLight(model[modelName], camera);
         this.position = position;
         this.rotation = rotation;
         SetupPositionRotation(model[modelName], this.position, this.rotation);
         SetBoundingBox(position);

         if (shadowEnable)
            DrawShadow(model, position, rotation, camera, new Vector3(0, 1, 0));
         //List<string> boundingIndex = new List<string>(boundingBox.Keys);
         //if (boundingDrawEnable)
         //{
         //    foreach (string bound in boundingIndex)
         //    {
         //        DrawLine(boundingBox[bound].Max, boundingBox[bound].Min, Color.WhiteSmoke, camera);
         //    }
         //}

      }
      public void DrawShadow(SkinnedModel model, Vector3 modelPosition, Vector3 modelRotation, Camera camera, Vector3 lightPosition)
      {

         //graphics.GraphicsDevice.Clear(ClearOptions.Stencil, Color.Black, 0, 0);

         graphics.GraphicsDevice.RenderState.StencilEnable = true;
         // Draw on screen if 0 is the stencil buffer value           
         graphics.GraphicsDevice.RenderState.StencilFunction =
             CompareFunction.Equal;
         // Increment the stencil buffer if we draw
         graphics.GraphicsDevice.RenderState.StencilPass =
             StencilOperation.Increment;
         // Setup alpha blending to make the shadow semi-transparent
         graphics.GraphicsDevice.RenderState.AlphaBlendEnable = true;
         graphics.GraphicsDevice.RenderState.ReferenceAlpha = 120;
         //graphics.GraphicsDevice.RenderState.AlphaFunction = CompareFunction.Greater;
         graphics.GraphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
         //graphics.GraphicsDevice.RenderState.DestinationBlend =
         //    Blend.InverseSourceAlpha;
         // Draw the shadow without lighting
         foreach (ModelMesh mesh in model.Model.Meshes)
         {

            foreach (SkinnedModelBasicEffect effect in mesh.Effects)
            {
               effect.LightEnabled = false;
               //effect.Alpha = 0.5f;
               effect.AmbientLightColor = Vector3.Zero;
               effect.View = camera.cameraViewMatrix;
               effect.Projection = camera.cameraProjectionMatrix;
               effect.World = Matrix.CreateRotationY(MathHelper.Pi + modelRotation.Y)

                   * Matrix.CreateShadow(lightPosition, wallPlane)
                   * Matrix.CreateRotationY(MathHelper.ToRadians(180))
                   * Matrix.CreateTranslation(new Vector3(0, 0.1f, 0))
                   * Matrix.CreateTranslation(modelPosition);
            }
            mesh.Draw();
         }
         // Return render states to normal            

         // turn stencilling off
         graphics.GraphicsDevice.RenderState.StencilEnable = false;
         // turn alpha blending off
         graphics.GraphicsDevice.RenderState.AlphaBlendEnable = false;


      }
      public void DrawShadow(Dictionary<string, SkinnedModel> model, Vector3 modelPosition, Vector3 modelRotation, Camera camera, Vector3 lightPosition)
      {

         graphics.GraphicsDevice.Clear(ClearOptions.Stencil, Color.Black, 0, 0);

         graphics.GraphicsDevice.RenderState.StencilEnable = true;
         // Draw on screen if 0 is the stencil buffer value           
         graphics.GraphicsDevice.RenderState.StencilFunction =
             CompareFunction.Equal;
         // Increment the stencil buffer if we draw
         graphics.GraphicsDevice.RenderState.StencilPass =
             StencilOperation.Increment;
         // Setup alpha blending to make the shadow semi-transparent
         graphics.GraphicsDevice.RenderState.AlphaBlendEnable = false;
         graphics.GraphicsDevice.RenderState.ReferenceAlpha = 120;
         //graphics.GraphicsDevice.RenderState.AlphaFunction = CompareFunction.Greater;
         graphics.GraphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
         //graphics.GraphicsDevice.RenderState.DestinationBlend =
         //    Blend.InverseSourceAlpha;
         // Draw the shadow without lighting
         foreach (ModelMesh mesh in model[modelName].Model.Meshes)
         {

            foreach (SkinnedModelBasicEffect effect in mesh.Effects)
            {
               effect.LightEnabled = false;
               effect.Alpha = 0.5f;
               effect.AmbientLightColor = Vector3.Zero;
               effect.View = camera.cameraViewMatrix;
               effect.Projection = camera.cameraProjectionMatrix;
               effect.World = Matrix.CreateRotationY(MathHelper.Pi + modelRotation.Y)

                   * Matrix.CreateShadow(lightPosition, wallPlane)
                   * Matrix.CreateRotationY(MathHelper.ToRadians(180))
                  //* Matrix.CreateTranslation(new Vector3(0, 0.1f, 0))
                   * Matrix.CreateTranslation(modelPosition);
            }
            mesh.Draw();
         }
         // Return render states to normal            

         // turn stencilling off
         graphics.GraphicsDevice.RenderState.StencilEnable = false;
         // turn alpha blending off
         graphics.GraphicsDevice.RenderState.AlphaBlendEnable = false;

      }
      public void SetupModel(AnimationController thisAnimationController, SkinnedModel model
          , Matrix[] thisAbsoluteBoneTransforms, Camera camera)
      {

         foreach (ModelMesh modelMesh in model.Model.Meshes)
         {
            foreach (SkinnedModelBasicEffect effect in modelMesh.Effects)
            {
               effect.Alpha = 1;
               effect.World = thisAbsoluteBoneTransforms[modelMesh.ParentBone.Index];
               //effect.GraphicsDevice.RenderState.AlphaTestEnable = true;
               //effect.GraphicsDevice.RenderState.AlphaBlendEnable = true;
               // Setup camera
               effect.View = camera.cameraViewMatrix;
               effect.Projection = camera.cameraProjectionMatrix;

               //Matrix.CreateOrthographic(300, 200, camera.nearPlaneDistance,
               //    camera.farPlaneDistance)  ;
               // Set the animated bones to the model
               effect.Bones = thisAnimationController.SkinnedBoneTransforms;

            }

         }
      }
      public void SetupPositionRotation(SkinnedModel model, Vector3 position, Vector3 rotation)
      {
         foreach (ModelMesh modelMesh in model.Model.Meshes)
         {
            foreach (SkinnedModelBasicEffect effect in modelMesh.Effects)
            {
               effect.World = Matrix.Identity;
               effect.World *= Matrix.CreateRotationX(rotation.X)
                   * Matrix.CreateRotationY(rotation.Y)
                   * Matrix.CreateRotationZ(rotation.Z)
                   * Matrix.CreateTranslation(position);
            }
            modelMesh.Draw();

         }
      }
      public void SetAnimate(SkinnedModel baseModel,ActionState actionState)
      {
         if (oldAnimation != actionState)
         {
            switch (actionState)
            {
               case ActionState.idle:
                  animationController.CrossFade(baseModel.AnimationClips["idle"], TimeSpan.FromMilliseconds(100));
                  break;
               case ActionState.walk:
                  animationController.CrossFade(baseModel.AnimationClips["run"], TimeSpan.FromMilliseconds(100));
                  break;
               case ActionState.attack:
                  animationController.CrossFade(baseModel.AnimationClips["attack"], TimeSpan.FromMilliseconds(100));
                  break;
                case ActionState.castingDrop:
                  animationController.CrossFade(baseModel.AnimationClips["castingDrop"], TimeSpan.FromMilliseconds(1));
                  break;
                case ActionState.castingStand:
                  animationController.CrossFade(baseModel.AnimationClips["castingStand"], TimeSpan.FromMilliseconds(100));
                  break;
                case ActionState.castingRelease:
                  animationController.CrossFade(baseModel.AnimationClips["castingRelease"], TimeSpan.FromMilliseconds(1));
                  break;
               case ActionState.battleidle:
                  animationController.CrossFade(baseModel.AnimationClips["battleidle"], TimeSpan.FromMilliseconds(100));
                  break;
               case ActionState.damaged:
                  animationController.CrossFade(baseModel.AnimationClips["damaged"], TimeSpan.FromMilliseconds(100));
                  break;
               case ActionState.dropdeath:
                  animationController.CrossFade(baseModel.AnimationClips["dropdeath"], TimeSpan.FromMilliseconds(100));
                  break;
               case ActionState.standdeath:
                  animationController.CrossFade(baseModel.AnimationClips["standdeath"], TimeSpan.FromMilliseconds(100));
                  break;
               default:
                  animationController.CrossFade(baseModel.AnimationClips.Values[0], TimeSpan.FromMilliseconds(100));
                  break;
            }

         }
         oldAnimation = actionState;
      }

      #region boundingBox
      public void DrawLine(Vector3 p1, Vector3 p2, Color C, Camera camera)
      {
         effect.View = camera.cameraViewMatrix;
         effect.Projection = camera.cameraProjectionMatrix;
         effect.World = Matrix.Identity;
         bool temp = effect.TextureEnabled;
         effect.TextureEnabled = false;
         VertexPositionColor[] verts;
         verts = new VertexPositionColor[16];
         verts[0] = new VertexPositionColor(p1, C);
         verts[1] = new VertexPositionColor(new Vector3(p1.X, p2.Y, p1.Z), C);
         verts[2] = new VertexPositionColor(new Vector3(p1.X, p2.Y, p2.Z), C);
         verts[3] = new VertexPositionColor(new Vector3(p1.X, p1.Y, p2.Z), C);
         verts[4] = new VertexPositionColor(p1, C);
         verts[5] = new VertexPositionColor(new Vector3(p2.X, p1.Y, p1.Z), C);
         verts[6] = new VertexPositionColor(new Vector3(p2.X, p2.Y, p1.Z), C);
         verts[7] = new VertexPositionColor(new Vector3(p1.X, p2.Y, p1.Z), C);
         verts[8] = new VertexPositionColor(new Vector3(p2.X, p2.Y, p1.Z), C);
         verts[9] = new VertexPositionColor(p2, C);
         verts[10] = new VertexPositionColor(new Vector3(p1.X, p2.Y, p2.Z), C);
         verts[11] = new VertexPositionColor(p2, C);
         verts[12] = new VertexPositionColor(new Vector3(p2.X, p1.Y, p2.Z), C);
         verts[13] = new VertexPositionColor(new Vector3(p1.X, p1.Y, p2.Z), C);
         verts[14] = new VertexPositionColor(new Vector3(p2.X, p1.Y, p2.Z), C);
         verts[15] = new VertexPositionColor(new Vector3(p2.X, p1.Y, p1.Z), C);
         graphics.GraphicsDevice.VertexDeclaration =
             new VertexDeclaration(graphics.GraphicsDevice, VertexPositionColor.VertexElements);
         effect.Begin();
         foreach (EffectPass pass in effect.CurrentTechnique.Passes)
         {
            pass.Begin();
            graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>
                (PrimitiveType.LineList, verts, 0, 1);
            graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>
                (PrimitiveType.LineList, verts, 1, 1);
            graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>
                (PrimitiveType.LineList, verts, 2, 1);
            graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>
                (PrimitiveType.LineList, verts, 3, 1);
            graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>
                (PrimitiveType.LineList, verts, 4, 1);
            graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>
                (PrimitiveType.LineList, verts, 5, 1);
            graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>
                (PrimitiveType.LineList, verts, 6, 1);
            graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>
                (PrimitiveType.LineList, verts, 7, 1);
            graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>
                (PrimitiveType.LineList, verts, 8, 1);
            graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>
                (PrimitiveType.LineList, verts, 9, 1);
            graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>
                (PrimitiveType.LineList, verts, 10, 1);
            graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>
                (PrimitiveType.LineList, verts, 11, 1);
            graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>
                (PrimitiveType.LineList, verts, 12, 1);
            graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>
                (PrimitiveType.LineList, verts, 13, 1);
            graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>
                (PrimitiveType.LineList, verts, 14, 1);
            pass.End();
         }
         effect.End();
         effect.TextureEnabled = temp;
      }
      public Vector3[] tempVecs3 = new Vector3[4096];
      public ushort[] tempUshorts = new ushort[4096 * 4];
      public void CalculateBoundingBox(ModelMesh mm, out BoundingBox bb)
      {
         bb = new BoundingBox();
         bool first = true;
         Matrix x = Matrix.Identity;
         ModelBone mb = mm.ParentBone;
         while (mb != null)
         {
            x = x * mb.Transform;
            mb = mb.Parent;
         }
         foreach (ModelMeshPart mp in mm.MeshParts)
         {

            int n = mp.NumVertices;
            if (n > tempVecs3.Length)
               tempVecs3 = new Vector3[n + 128];
            int l = mp.PrimitiveCount * 3;
            if (l > tempUshorts.Length)
               tempUshorts = new ushort[l + 128];
            if (n == 0 || l == 0)
               continue;
            mm.IndexBuffer.GetData<ushort>(tempUshorts, mp.StartIndex, l);
            mm.VertexBuffer.GetData<Vector3>(mp.StreamOffset, tempVecs3, mp.BaseVertex, n, mp.VertexStride);
            if (first)
            {
               bb.Min = Vector3.Transform(tempVecs3[tempUshorts[0]], x);
               bb.Max = bb.Min;
               first = false;
            }
            for (int i = 0; i != l; ++i)
            {
               ushort us = tempUshorts[i];
               Vector3 v = Vector3.Transform(tempVecs3[us], x);
               Vector3.Max(ref v, ref bb.Max, out bb.Max);
               Vector3.Min(ref v, ref bb.Min, out bb.Min);
            }
         }
      }
      public void CreateBoundingBox(Model model)
      {
         boundingBox.Clear();
         foreach (ModelMesh mesh in model.Meshes)
         {
            string meshName = mesh.Name.ToString();
            CalculateBoundingBox(mesh, out tempBoundingBox);
            boundingBox.Add(meshName, tempBoundingBox);
         }
      }
      public void CreateTalkingCheck(Vector3 plusFromBoundingbox)
      {
         talkingAreaCheck = tempBoundingBox;
         talkingAreaCheck.Max += plusFromBoundingbox;
         talkingAreaCheck.Min -= plusFromBoundingbox;
      }
      public void TranslateBounding(Vector3 position)
      {
         List<string> boundingIndex = new List<string>(boundingBox.Keys);
         foreach (string index in boundingIndex)
         {
            //center = tempBoundingBox.Max - tempBoundingBox.Min;
            Vector3 offsetMax = position + tempBoundingBox.Max;
            Vector3 offsetMin = position + tempBoundingBox.Min;
            tempBoundingBox = boundingBox[index];
            tempBoundingBox.Max = offsetMax;
            tempBoundingBox.Min = offsetMin;
            boundingBox.Remove(index);
            boundingBox.Add(index, tempBoundingBox);


         }
      }
      public void TranslateBounding()
      {

         List<string> boundingIndex = new List<string>(boundingBox.Keys);
         foreach (string index in boundingIndex)
         {

            tempBoundingBox = boundingBox[index];
            tempBoundingBox.Max += velocity;
            tempBoundingBox.Min += velocity;
            boundingBox.Remove(index);
            boundingBox.Add(index, tempBoundingBox);
         }

      }
      public void SetBoundingBox(Vector3 position)
      {
         List<string> boundingIndex = new List<string>(boundingBox.Keys);
         foreach (string index in boundingIndex)
         {

            tempBoundingBox = boundingBox[index];
            Vector3 diff = (tempBoundingBox.Max - tempBoundingBox.Min);
            center = diff / 2;

            Vector3 offsetMin = -center;
            offsetMin.Y = 0;
            Vector3 offsetMax = offsetMin + diff;

            tempBoundingBox.Max = offsetMax + position;
            tempBoundingBox.Min = offsetMin + position;
            boundingBox.Remove(index);
            boundingBox.Add(index, tempBoundingBox);


         }

      }
      public BoundingBox TranslateBounding(BoundingBox bounding, Vector3 position)
      {

         Vector3 offsetMax = position + bounding.Max;
         Vector3 offsetMin = position + bounding.Min;
         tempBoundingBox = bounding;
         tempBoundingBox.Max = offsetMax;
         tempBoundingBox.Min = offsetMin;

         return tempBoundingBox;
      }
      public BoundingBox TranslateBounding(BoundingBox bounding)
      {

         tempBoundingBox = bounding;
         tempBoundingBox.Max += velocity;
         tempBoundingBox.Min += velocity;
         return tempBoundingBox;

      }
      public bool CollisionObj(SkinnedModelObj skinnedModel)
      {
         List<string> boundingIndex = new List<string>(skinnedModel.boundingBox.Keys);
         List<string> thisboundingIndex = new List<string>(this.boundingBox.Keys);
         foreach (string index in boundingIndex)
         {
            foreach (string thisIndex in thisboundingIndex)
            {
               if (this.boundingBox[thisIndex].Intersects(skinnedModel.boundingBox[index]))
                  return true;
            }
         }
         return false;
      }

      #endregion
      #region Light
      public void SetupDefaultLight(SkinnedModel model, Camera camera)
      {

         foreach (ModelMesh modelMesh in model.Model.Meshes)
         {
            foreach (SkinnedModelBasicEffect effect in modelMesh.Effects)
            {

               //effect.Material.DiffuseColor = new Vector3(1);
               //effect.Material.SpecularColor = new Vector3(0.3f);
               //effect.Material.SpecularPower = 8;
               //effect.AmbientLightColor = new Vector3(0.1f);

               effect.Material.DiffuseColor = diffuse;
               effect.Material.SpecularColor = Vector3.One;
               effect.Material.SpecularPower = 8;

               //OPTIONAL - Configure lights
               switch (modelName)
               {

                   case "Kris":
                   case "Kirk":
                   case "Rainel":
                   case "Angel":
                   case "DarkAngel":
                   case "MonsterBig":
                   case "MonsterMedium":
                   case "MonsterSmall":
                   case "NPCLeader":
                   case "NPCSave":
                   case "NPCFAQ":
                   case "NPC01":
                   case "NPC02":
                   case "NPC03":
                   case "NPC04":
                       effect.AmbientLightColor = new Vector3(0.6f);
                       break;
                   default:
                       effect.AmbientLightColor = Vector3.One;
                       break;
               }
               //effect.NormalMapEnabled = true;

               effect.LightEnabled = false;

            }

         }


      }
      public void SetupLight(SkinnedModel model, Vector3 lightColor, Vector3 lightPosition)
      {

         foreach (ModelMesh modelMesh in model.Model.Meshes)
         {
            foreach (SkinnedModelBasicEffect effect in modelMesh.Effects)
            {
               effect.Material.DiffuseColor = diffuse;
               //effect.Material.SpecularColor = new Vector3(0.8f);
               effect.Material.SpecularColor = Vector3.One;
               effect.Material.SpecularPower = 16;

               // OPTIONAL - Configure lights
               //effect.AmbientLightColor = new Vector3(0.2f);
               effect.AmbientLightColor = new Vector3(0.5f);

               //effect.NormalMapEnabled = true;

               effect.LightEnabled = true;

               effect.EnabledLights = EnabledLights.One;
               effect.PointLights[0].Color = lightColor;
               effect.PointLights[0].Position = lightPosition;

            }
         }


      }
      public void SetupLight(SkinnedModel model, Camera camera, PointLight light1, PointLight light2)
      {

         foreach (ModelMesh modelMesh in model.Model.Meshes)
         {
            foreach (SkinnedModelBasicEffect effect in modelMesh.Effects)
            {
               effect.Material.DiffuseColor = new Vector3(0.8f);
               effect.Material.SpecularColor = new Vector3(0.3f);
               effect.Material.SpecularPower = 8;

               // OPTIONAL - Configure lights
               effect.AmbientLightColor = new Vector3(0.1f);
               //effect.NormalMapEnabled = true;

               effect.LightEnabled = true;

               effect.EnabledLights = EnabledLights.Two;
               effect.PointLights[0].Color = light1.Color;
               effect.PointLights[0].Position = light1.Position;
               effect.PointLights[1].Color = light2.Color;
               effect.PointLights[1].Position = light2.Position;
            }
         }


      }

      public void setDiffuse(float inputDiffuse)
      {
         diffuse.X = inputDiffuse;
         diffuse.Y = inputDiffuse;
         diffuse.Z = inputDiffuse;
      }
      #endregion
   }


   public struct Quad
   {
      public Vector3 Origin;
      public Vector3 UpperLeft;
      public Vector3 LowerLeft;
      public Vector3 UpperRight;
      public Vector3 LowerRight;
      public Vector3 Normal;
      public Vector3 Up;
      public Vector3 Left;

      public VertexPositionNormalTexture[] Vertices;
      public int[] Indexes;
      public VertexBuffer VertexBuf;

      public Quad(Vector3 origin, Vector3 normal, Vector3 up,
          float width, float height)
      {
         Vertices = new VertexPositionNormalTexture[4];
         Indexes = new int[6];
         Origin = origin;
         Normal = normal;
         Up = up;

         // Calculate the quad corners
         Left = Vector3.Cross(normal, Up);
         Vector3 uppercenter = (Up * height / 2) + origin;
         UpperLeft = uppercenter + (Left * width / 2);
         UpperRight = uppercenter - (Left * width / 2);
         LowerLeft = UpperLeft - (Up * height);
         LowerRight = UpperRight - (Up * height);

         VertexBuf = null;
         FillVertices();
      }
      private void FillVertices()
      {
         // Fill in texture coordinates to display full texture
         // on quad
         Vector2 textureUpperLeft = new Vector2(0.0f, 0.0f);
         Vector2 textureUpperRight = new Vector2(1.0f, 0.0f);
         Vector2 textureLowerLeft = new Vector2(0.0f, 1.0f);
         Vector2 textureLowerRight = new Vector2(1.0f, 1.0f);

         // Provide a normal for each vertex
         for (int i = 0; i < Vertices.Length; i++)
         {
            Vertices[i].Normal = Normal;
         }

         // Set the position and texture coordinate for each
         // vertex
         Vertices[0].Position = LowerLeft;
         Vertices[0].TextureCoordinate = textureLowerLeft;
         Vertices[1].Position = UpperLeft;
         Vertices[1].TextureCoordinate = textureUpperLeft;
         Vertices[2].Position = LowerRight;
         Vertices[2].TextureCoordinate = textureLowerRight;
         Vertices[3].Position = UpperRight;
         Vertices[3].TextureCoordinate = textureUpperRight;

         // Set the index buffer for each vertex, using
         // clockwise winding
         Indexes[0] = 0;
         Indexes[1] = 1;
         Indexes[2] = 2;
         Indexes[3] = 2;
         Indexes[4] = 1;
         Indexes[5] = 3;
      }
      public void Load(GraphicsDevice device)
      {
         VertexBuf = new VertexBuffer(device,
             VertexPositionNormalTexture.SizeInBytes * Vertices.Length,
             BufferUsage.WriteOnly);
         VertexBuf.SetData<VertexPositionNormalTexture>(Vertices);
      }

   }
}
