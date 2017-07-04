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
    public class FieldMap : Scene
    {
        public class SignMap : Layer 
        {
            SpriteObj label;
            public SignMap()
                : base()
            {
                label = new SpriteObj("bedroom");
                label.setPosition(AIW.ScreenPosByRatio(0.1, 0.1));
                addChild(label);
                label.setScale(0.5f);

            }
            public override void update(GameTime gt)
            {
                base.update(gt);
                label.setFilename("" + AIW.OS.graphic3DEngine.map.modelName);
            }
        }

        Engine3DController engine3DController;
        public FieldMap()
            : base()
        {
            if (AIW.OS.isUse3DEngine)
            {
                init3DEngineController();
            }
            SignMap signmapLayer = new SignMap();
            addChild(signmapLayer);
        }
        public override void update(GameTime gt)
        {
            base.update(gt);
            event3DEngineControl();
        }
        void event3DEngineControl()
        {
            if (AIW.Input.isKeyPressed(Keys.D1))
            {
                engine3DController.skipShot1();
            }
            if (AIW.Input.isKeyPressed(Keys.D2))
            {
                //engine3DController.skipBattle();
                engine3DController.reset3DWorld();
            }
            if (AIW.Input.isKeyPressed(Keys.D3))
            {
                engine3DController.jumpToBattle();
            }

            engine3DController.handleInputToWalk();
        }

        void init3DEngineController()
        {
            engine3DController = new Engine3DController(AIW.OS.graphic3DEngine);
        }
    }
}
