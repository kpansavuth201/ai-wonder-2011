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
    public class DisclaimerPage : Scene
    {
        public class DisclaimerLayer:Layer
        {
            Timer timer = new Timer(0.5);
            SpriteObj label;
            public DisclaimerLayer()
                : base()
            {
                RectangleObj bg = new RectangleObj(AIW.ScreenWidth, AIW.ScreenHeight);
                bg.setPosition(AIW.ScreenCenter);
                addChild(bg);
                bg.setZOrder(-1);

                SpriteObj obj = new SpriteObj("disclaimer");
                obj.setPosition(AIW.ScreenCenter);
                addChild(obj);
                label = obj;
            }

            public override void update(GameTime gt)
            {
                base.update(gt);
                updateState(gt);
              
                if (state_ == 2) return;
                if (timer.Update(gt))
                {
                    state_++;
                   
                }
               
            }
            public override void state0(GameTime gt)
            {
                label.setAlpha(timer.TimerPosition);
            }
            public override void state1(GameTime gt)
            {
                label.setAlpha(1);
            }
            public override void state2(GameTime gt)
            {
                if (AIW.Input.isKeyPressed(Keys.Z))
                  // || AIW.Input.isLeftMousePressed)
                {
                    state_++;
                }
            }
            public override void state3(GameTime gt)
            {
                label.setAlpha(timer.TimerPositionInvert);
            }
            public override void state4(GameTime gt)
            {
                BattleScene scene = new BattleScene();
                Director.sharedDirector.pushScene(scene);
                SoundEngine.sharedSoundEngine.BGMManager.Play("Shinobi");
            }
     

        }

        public DisclaimerPage()
            : base()
        {
            
            DisclaimerLayer layer = new DisclaimerLayer();
            addChild(layer);
            
        }
    }
}
