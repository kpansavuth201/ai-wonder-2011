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
    public class AiWonder_FieldMapScene : Old_Scene
    {
          public AiWonder_FieldMapScene()
            : base(new GameplayLayer())
        {
        }

          public class GameplayLayer : RootLayer
          {
              Engine3DController engine3DController;

              public override void LoadContent()
              {
                  base.LoadContent();
                  //AIW.OS.isShowWallpaper = false;

                  engine3DController = new Engine3DController(AIW.OS.graphic3DEngine);
              }


              public override void HandleInput(GameTime gt)
              {
                  base.HandleInput(gt);
                  if (AIW.Input.isKeyPressed(Keys.Space))
                  {
                      engine3DController.skipShot1();
                  }
                  if (AIW.Input.isKeyPressed(Keys.D2))
                  {
                      engine3DController.skipBattle();
                  }
                  if (AIW.Input.isKeyPressed(Keys.D1))
                  {
                      AIW.changeScene("BattleScene");
                  }
                 

                  engine3DController.handleInputToWalk();

              }
          }
    }
}
