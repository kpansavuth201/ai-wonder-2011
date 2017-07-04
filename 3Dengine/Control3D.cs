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

namespace AiWonderTest
{
    public class Control3D
    {
        public float runBoost = 1.0f;

        public bool isKeyDownKeyD = false;
        public bool isKeyDownKeyA = false;
        public bool isKeyDownKeyS = false;
        public bool isKeyDownKeyW = false;
        public bool isKeyDownKeyLeftCtrl = false;

    }
}

    /*
   public class Remote3D : Obj
   {
      Timer walkSound = new Timer(TimeSpan.FromSeconds(0.35));
      public override void LoadContent()
      {
         isActive = true;
         isVisible = true;
      }
      float scrollArea = 2.5f;
      float moveArea = 50;

      public bool isCanWalk = true;

      public void Reset3D()
      {
         S.draw3DManager.SetMapTo(S.draw3DManager.bedroom, new Vector3(0, 0, 0));
         S.draw3DManager.sceneState.scene = 1;
         S.draw3DManager.sceneState.eventState = 1;
         S.draw3DManager.sceneState.dialogue = 1;
         S.draw3DManager.whatRoom = Draw3DManager.WhatRoom.WorkingRoom;
         S.GameFiniteState.aiWonderState = GameFiniteState.AiWonderState.PlayCutScene;
         S.draw3DManager.isPlayCutScene = false;
      }
      public override void Update(GameTime gameTime)
      {
         if(S.GameFiniteState.aiWonderState == GameFiniteState.AiWonderState.FieldMap)
            EventRun(gameTime);

         if (S.IsDebugEnable && S.input.isKeyPressed(Keys.F6))
         {
            S.draw3DManager.SetMapTo(S.draw3DManager.market, new Vector3(126, 0, 0));

            S.draw3DManager.sceneState.scene = 6;
            S.draw3DManager.sceneState.eventState = 1;
            S.draw3DManager.sceneState.dialogue = 1;
            S.draw3DManager.whatRoom = Draw3DManager.WhatRoom.Library;
            S.GameFiniteState.aiWonderState = GameFiniteState.AiWonderState.FieldMap;
            S.draw3DManager.isPlayCutScene = false;
         }
         if (S.IsDebugEnable && S.input.isKeyPressed(Keys.F12))
         {
            Reset3D();
         }
         if (S.IsDebugEnable && S.input.isKeyPressed(Keys.F7))
         {
            if (S.draw3DManager.whatRoom == Draw3DManager.WhatRoom.None)
            {
               S.draw3DManager.whatRoom = Draw3DManager.WhatRoom.TownHall;
            }else 
               if (S.draw3DManager.whatRoom == Draw3DManager.WhatRoom.TownHall)
            {
               S.draw3DManager.whatRoom = Draw3DManager.WhatRoom.Library;
            }else 
                  if (S.draw3DManager.whatRoom == Draw3DManager.WhatRoom.Library)
            {
               S.draw3DManager.whatRoom = Draw3DManager.WhatRoom.WorkingRoom;
            }else 
                     if (S.draw3DManager.whatRoom == Draw3DManager.WhatRoom.WorkingRoom)
            {
               S.draw3DManager.whatRoom = Draw3DManager.WhatRoom.None;
            } 
         }
      }   
   

      public void EventRunOld()
      {
         if (S.input.CurrentMouseState.RightButton == ButtonState.Pressed)
         {
            S.control3D.runBoost = 10.0f;
         }
         else
         {
            S.control3D.runBoost = 1.0f;
         }



         if (S.input.CurrentMouseState.LeftButton == ButtonState.Pressed)
         {


            if (S.input.CurrentMousePosition.X < S.screenWidth / scrollArea)
            {
               S.control3D.isKeyDownKeyA = true;

            }
            else
            {
               S.control3D.isKeyDownKeyA = false;
            }
            if (S.input.CurrentMousePosition.X > S.screenWidth - S.screenWidth / scrollArea)
            {
               S.control3D.isKeyDownKeyD = true;

            }
            else
            {
               S.control3D.isKeyDownKeyD = false;
            }

            if (S.input.CurrentMousePosition.Y < S.screenHeight / scrollArea)
            {
               S.control3D.isKeyDownKeyW = true;

            }
            else
            {
               S.control3D.isKeyDownKeyW = false;
            }
            if (S.input.CurrentMousePosition.Y > S.screenHeight - S.screenHeight / scrollArea)
            {
               S.control3D.isKeyDownKeyS = true;

            }
            else
            {
               S.control3D.isKeyDownKeyS = false;
            }

         }
         else
         {
            S.control3D.isKeyDownKeyA = false;
            S.control3D.isKeyDownKeyS = false;
            S.control3D.isKeyDownKeyW = false;
            S.control3D.isKeyDownKeyD = false;
         }
      }

      public void  RunByKeyboard(GameTime gameTime)
      {
         if (S.input.CurrentKeyboardStates.IsKeyDown(Keys.A))
         {
            S.control3D.isKeyDownKeyA = true;


            if (!walkSound.Update(gameTime) && !S.draw3DManager.isMapChanged)
            {
               S.sound.Play("walk");
            }
         }
         else
         {
            S.control3D.isKeyDownKeyA = false;
         }
         if (S.input.CurrentKeyboardStates.IsKeyDown(Keys.W))
         {
            S.control3D.isKeyDownKeyW = true;

            if (!S.control3D.isKeyDownKeyA)
            {
               if (!walkSound.Update(gameTime) && !S.draw3DManager.isMapChanged)
               {
                  S.sound.Play("walk");
               }
            }
         }
         else
         {
            S.control3D.isKeyDownKeyW = false;
         }
         if (S.input.CurrentKeyboardStates.IsKeyDown(Keys.S))
         {
            S.control3D.isKeyDownKeyS = true;

            if (!S.control3D.isKeyDownKeyW &&
              !S.control3D.isKeyDownKeyA)
            {
               if (!walkSound.Update(gameTime) && !S.draw3DManager.isMapChanged)
               {
                  S.sound.Play("walk");
               }
            }
         }
         else
         {
            S.control3D.isKeyDownKeyS = false;
         }
         if (S.input.CurrentKeyboardStates.IsKeyDown(Keys.D))
         {
            S.control3D.isKeyDownKeyD = true;

            if (!S.control3D.isKeyDownKeyW &&
              !S.control3D.isKeyDownKeyS &&
              !S.control3D.isKeyDownKeyA)
            {
               if (!walkSound.Update(gameTime) && !S.draw3DManager.isMapChanged)
               {
                  S.sound.Play("walk");
               }
            }
         }
         else
         {
            S.control3D.isKeyDownKeyD = false;
         }
      }
      public void RunByMouse(Vector2 characterPos)
      {

         if (S.input.CurrentMouseState.LeftButton == ButtonState.Pressed)
         {


            if (characterPos.X - S.input.CurrentMousePosition.X > moveArea)
            {
               S.control3D.isKeyDownKeyA = true;

            }
            else
            {
               S.control3D.isKeyDownKeyA = false;
            }
            if (S.input.CurrentMousePosition.X - characterPos.X > moveArea)
            {
               S.control3D.isKeyDownKeyD = true;

            }
            else
            {
               S.control3D.isKeyDownKeyD = false;
            }

            if (characterPos.Y - S.input.CurrentMousePosition.Y > moveArea)
            {
               S.control3D.isKeyDownKeyW = true;

            }
            else
            {
               S.control3D.isKeyDownKeyW = false;
            }
            if (S.input.CurrentMousePosition.Y - characterPos.Y > moveArea)
            {
               S.control3D.isKeyDownKeyS = true;

            }
            else
            {
               S.control3D.isKeyDownKeyS = false;
            }

         }
         else
         {
            S.control3D.isKeyDownKeyA = false;
            S.control3D.isKeyDownKeyS = false;
            S.control3D.isKeyDownKeyW = false;
            S.control3D.isKeyDownKeyD = false;
         }
      }
      public void EventRun(GameTime gameTime)
      {
         Vector2 characterPos = new Vector2(
            S.draw3DManager.camera.point2screen(
            S.draw3DManager.pareak.position).X,
           S.draw3DManager.camera.point2screen(
            S.draw3DManager.pareak.position).Y );

         if (S.IsDebugEnable)
         {
            if (S.input.CurrentMouseState.RightButton == ButtonState.Pressed)
            {
               S.control3D.runBoost = 10.0f;
            }
            else
            {
               S.control3D.runBoost = 1.0f;
            }
         }

         RunByKeyboard(gameTime);

      }


      public override void DrawAlphaBlend(GameTime gameTime)
      {
         if (S.isShowDebugInfo)
         {
            float y = 10;
            if (S.draw3DManager.nearestNpc != "")
            {
               DrawString(S.Font, "NPC : " + S.draw3DManager.nearestNpc, new Vector2(
                  20, y), 0.0f, 1.0f, Color.White, 100);
            }
            else
            {
               DrawString(S.Font, "NPC : " + S.draw3DManager.nearestNpc, new Vector2(
                    20, y), 0.0f, 1.0f, Color.Gray, 100);
            }

            y += 10;
            DrawString(S.Font, "Map : " + S.draw3DManager.map.modelName, new Vector2(
                   20, y), 0.0f, 1.0f, Color.White, 100);
            
            y += 10;
            DrawString(S.Font, "Character Pos : ("+S.draw3DManager.pareak.position.X+","
               + S.draw3DManager.pareak.position.Y + "," + S.draw3DManager.pareak.position.Z + ")"
             ,new Vector2(20, y) , 0.0f, 1.0f, Color.White, 100);

            y += 10;
            DrawString(S.Font, "Character Rotate : (" + S.draw3DManager.testRotation.X + ","
               + S.draw3DManager.testRotation.Y + "," + S.draw3DManager.testRotation.Z + ")"
             , new Vector2(20, y), 0.0f, 1.0f, Color.White, 100);

            y += 10;
            DrawString(S.Font, "whatRoom : "+S.draw3DManager.whatRoom.ToString()
             , new Vector2(20, y), 0.0f, 1.0f, Color.White, 100);

            y += 10;
            DrawString(S.Font, "scene : " + S.draw3DManager.sceneState.scene
             , new Vector2(20, y), 0.0f, 1.0f, Color.White, 100);
            y += 10;
            DrawString(S.Font, "eventState : " + S.draw3DManager.sceneState.eventState
             , new Vector2(20, y), 0.0f, 1.0f, Color.White, 100);
            y += 10;
            DrawString(S.Font, "dialogue : " + S.draw3DManager.sceneState.dialogue
             , new Vector2(20, y), 0.0f, 1.0f, Color.White, 100);
            y += 10;
            DrawString(S.Font, "isPlayCutScene : " + S.draw3DManager.isPlayCutScene
             , new Vector2(20, y), 0.0f, 1.0f, Color.White, 100);
            y += 10;
            DrawString(S.Font, "" + S.GameFiniteState.aiWonderState.ToString()
             , new Vector2(20, y), 0.0f, 1.0f, Color.White, 100);
            y += 10;
            DrawString(S.Font, "camera position :(" + S.draw3DManager.camera.cameraPosition+
               ")"
             , new Vector2(20, y), 0.0f, 1.0f, Color.White, 100);
            y += 10;
            DrawString(S.Font, "Velocity position :(" + S.draw3DManager.pareak.tempBoundingBox +
               ")"
             , new Vector2(20, y), 0.0f, 1.0f, Color.White, 100);
            //y += 10;
            //DrawString(S.Font, "point2screen X : " + S.draw3DManager.camera.point2screen(
            //   S.draw3DManager.pareak.position).X, new Vector2(
            //       20, y), 0.0f, 1.0f, Color.White, 100);
            //y += 10;
            //DrawString(S.Font, "point2screen Y : " + S.draw3DManager.camera.point2screen(
            //   S.draw3DManager.pareak.position).Y, new Vector2(
            //       20, y), 0.0f, 1.0f, Color.White, 100);
         }
      }
   }
}
    */