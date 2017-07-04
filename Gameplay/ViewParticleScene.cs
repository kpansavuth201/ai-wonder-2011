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
    public class BattleAnnouncer : SpriteObj
    {
        Timer timer;
        Vector2 startPos = AIW.ScreenPosByRatio(-0.5, 0.5);
        public BattleAnnouncer(double duration):base("battleLabel")
        {
            setPosition(startPos);
            timer = new Timer(duration);
        }
        public override void update(GameTime gt)
        {
            base.update(gt);
            updateState(gt);
        }
        public override void state0(GameTime gt)
        {
            MoveDirectionInDegree(gt, 16, 0);
            if (Physics2D.DistanceTo(AIW.ScreenCenter, position) < 32)
            {
                state_++;
            }
        }
        public override void state1(GameTime gt)
        {
            if (timer.Update(gt))
            {
                timer = new Timer(timer.Duration);
                state_++;
            }
        }
        public override void state2(GameTime gt)
        {
            setAlpha(timer.TimerPositionInvert);
            setScale(1+timer.TimerPosition * 3);
            if (timer.Update(gt))
            {
                removeSelf();
            }
        }
    }
    public class DarkBGParticle : RectangleObj
    {
        Timer fadeinTimer;
        Timer delayTimer;
        Timer fadeoutTimer; 
        int state = 0;
        public DarkBGParticle(double fadeInDuration, double displayDuration, double fadeOutDuration)
            : base(AIW.ScreenWidth, AIW.ScreenHeight)
        {
            fadeinTimer = new Timer(fadeInDuration);
            delayTimer = new Timer(displayDuration);
            fadeoutTimer = new Timer(fadeOutDuration);
            setAlpha(0);
            setPosition(AIW.ScreenCenter);
            color_ = Color.Black;
           

        }
        public override void update(GameTime gt)
        {
            base.update(gt);
            if (state == 0)
            {
                setAlpha(fadeinTimer.TimerPosition*0.75f);
                if (fadeinTimer.Update(gt))
                {
                    setAlpha(1 * 0.75f);
                    state++;
                }
            }
            else if (state == 1)
            {
                if (delayTimer.Update(gt))
                {
                    state++;
                }
            }
            else if (state == 2)
            {
                setAlpha(fadeoutTimer.TimerPositionInvert * 0.75f);
                if (fadeoutTimer.Update(gt))
                {
                    state++;
                }
            }
            else
            {
                removeSelf();
            }
        }
    }

    public class PlusWinkEmitter : Node
    {
        Timer timer = new Timer(0.1);
        int countParticle = 0;
        public PlusWinkEmitter(int numberOfParticle,Vector2 startPos)
        {
            setPosition(startPos);
            countParticle = numberOfParticle;
        }
        public override void update(GameTime gt)
        {
            base.update(gt);
            if (timer.Update(gt))
            {
                PlusWinkParticle particle = new PlusWinkParticle(position);
                countParticle--;
                ((Layer)parent).addChildToAdd(particle);

                if (countParticle <= 0)
                {
                    removeSelf();
                }
            }
        }
    }

    public class PlusWinkParticle : SpriteObj
    {
        Timer timer = new Timer(0.3);
        public PlusWinkParticle(Vector2 startPos)
            : base("starWink1")
        {
            int randRange = 100;
            int randPos = AIW.arc4random() % randRange;
            startPos.X -= 25;
            startPos.X += randPos;
            setPosition(startPos);
            float randTime = (AIW.arc4random() % 3) * 0.1f+0.3f;
            timer.ResetTimer(randTime);
            int randScale = (AIW.arc4random() % 5)+1;
            setScale(0.05f*randScale);
        }

        public override void update(GameTime gt)
        {
            base.update(gt);

            setAlpha(timer.TimerPositionInvert);
            MoveDirectionInDegree(gt, 4, -90);
            if (timer.Update(gt))
            {
                removeSelf();
            }
        }
    }

    public class DrawCardParticle : SpriteObj
    {
        //Timer decayTimer = new Timer(2.5);
        Vector2 _targetPos;
        bool isMoving = true;
        public DrawCardParticle(Vector2 startPos, Vector2 targetPos)
            : base("mockCard")
        {
            _targetPos = targetPos;
            setPosition(startPos);
           
        }
        public override void update(GameTime gt)
        {
            base.update(gt);
            //if (decayTimer.Update(gt))
            //{
            //    removeSelf();
            //}

            if (!isMoving) { return; }
            if (MoveToTarget(gt, 12, _targetPos))
            {
                isMoving = false;
            }
         
        }
    }
    public class DiscardParticle : SpriteObj
    {
        float initScale_ = 1;
        float finalScale_ = 1;
        Timer timer = new Timer(1);
        BattleScene.MainLayer mainLayerDelegate_;
        public DiscardParticle(BattleScene.MainLayer mainLayerDelegate, Vector2 startPos, double duration,float initScale,float finalScale)
            : base("mockCard")
        {
            mainLayerDelegate_ = mainLayerDelegate;
            initScale_ = initScale;
            finalScale_ = finalScale;
            timer.ResetTimer(duration);
            setPosition(startPos);
            setZOrder(3);
        }
        public override void update(GameTime gt)
        {
            base.update(gt);
             float scaleRange = finalScale_-initScale_;
             setScale(initScale_ + scaleRange * timer.TimerPosition);
            if (timer.Update(gt))
            {
               // mainLayerDelegate_.finishDiscardAnim();
                removeSelf();                
            }
        }
    }
    public class BuyCardParticle : SpriteObj
    {
        Vector2 _startPos;
        Vector2 _targetPos;
        float initScale_ = 1;
        float finalScale_ = 1;
        float distanceLength = 0;
        public BuyCardParticle(Vector2 startPos, Vector2 targetPos,float initScale,float finalScale)
            : base("mockCard")
        {
            initScale_ = initScale;
            finalScale_ = finalScale;
            _targetPos = targetPos;
            _startPos = startPos;
            setPosition(startPos);
            distanceLength = Physics2D.DistanceTo(startPos, targetPos);

        }
        public override void update(GameTime gt)
        {
            base.update(gt);
          
            float scaleRange = finalScale_-initScale_;
            float distance = Physics2D.DistanceTo(position, _targetPos);
            setScale(initScale_ + scaleRange * (distanceLength- distance)/distanceLength);

            if (MoveToTarget(gt, 18, _targetPos))
            {
                removeSelf();
            }

        }
    }
    public class ManaGainEmitter:Node
    {
         Timer timer = new Timer(0.1);
        int countParticle = 0;
        Vector2 startPos_;
        Vector2 targetPos_;
        public ManaGainEmitter(int numberOfParticle, Vector2 startPos,Vector2 target)
        {
            startPos_ = startPos;
            targetPos_ = target;
            countParticle = numberOfParticle;
            setZOrder(4);
        }
        public override void update(GameTime gt)
        {
            base.update(gt);
            if (timer.Update(gt))
            {
                ManaGainParticle particle = new ManaGainParticle(startPos_, targetPos_);            
                countParticle--;
                ((Layer)parent).addChildToAdd(particle);

                if (countParticle <= 0)
                {
                    removeSelf();
                }
            }
        }
    }

    public class DamageParticle : TextObj
    {
        Timer timer = new Timer(1.5);
        public DamageParticle(int damage, Vector2 pos)
            : base("" + damage)
        {
            setPosition(pos);
            if (damage >= 0)
            {
                color_ = Color.GreenYellow;
            }
            else
            {
                color_ = Color.Red;
            }
            setScale(6);
            setZOrder(3);
        }
        public override void update(GameTime gt)
        {
            base.update(gt);

            setAlpha(timer.TimerPositionInvert);
            MoveDirectionInDegree(gt, 2, -90);
            if (timer.Update(gt))
            {
                removeSelf();
            }
        }
    }

    public class ManaGainParticle : SpriteObj
    {
        Vector2 _targetPos;
        
        
        public ManaGainParticle(Vector2 startPos,Vector2 targetPos):base("circleTestGlowBlur3")
        {
            _targetPos = targetPos;
            setPosition(startPos);
            color_ = Color.Fuchsia;
            GenerateSeed();
        }

        public override void update(GameTime gt)
        {
            base.update(gt);
            if (MoveToTarget(gt, 8, _targetPos))
            {
                  EllipseParticle particle = new EllipseParticle(position);
                  ((Layer)parent).addChildToAdd(particle);
                removeSelf();
            }
        }

    }

    public class EllipseParticle : SpriteObj
    {
        Timer timer = new Timer(0.25);
        public EllipseParticle(Vector2 startPos)
            : base("particleWink2")
        {
            setPosition(startPos);
            color_ = Color.Fuchsia;
            setRotation(AIW.arc4random(0) % 90);
        }

        public override void update(GameTime gt)
        {
            base.update(gt);
            setAlpha(timer.TimerPositionInvert);
            setScale(timer.TimerPosition*3);

            if (timer.Update(gt))
            {
                removeSelf();
            }
        }
    }

    public class ViewParticleScene : Scene
    {
        public class MainLayer : Layer{
         

            void eventEmitEllipseParticle()
            {
                if (AIW.Input.isLeftMousePressed)
                {
                    //EllipseParticle particle = new EllipseParticle(AIW.Input.MousePosition);
                   // ManaGainParticle particle = new ManaGainParticle(
                   //AIW.Input.MousePosition, AIW.ScreenPosByRatio(0.5, 0.5));
                    //DrawCardParticle particle = new DrawCardParticle(AIW.Input.MousePosition,
                    //    AIW.ScreenCenter);
                       //PlusWinkParticle particle = new PlusWinkParticle(AIW.Input.MousePosition);

                        //PlusWinkEmitter emitter = new PlusWinkEmitter(5, AIW.Input.MousePosition);
                        //addChild(emitter);

                    //DarkBGParticle particle = new DarkBGParticle(0.5, 1, 0.5);
                    BattleAnnouncer particle = new BattleAnnouncer(0.25);
                    DarkBGParticle bg = new DarkBGParticle(1, 0.25, 0.25);
                    bg.setZOrder(particle.zOrder - 1);
                    addChild(bg);
                    addChild(particle);

                    
                }
            }

            public MainLayer()
            {
            }
            public override void update(GameTime gt)
            {
                base.update(gt);
                eventEmitEllipseParticle();
            }
        }

        #region Scene
        MainLayer mainLayer;
          public ViewParticleScene()
        {
            //AIW.OS.isShowWallpaper = false;
            MainLayer layer = new MainLayer();
            mainLayer = layer;
            addChild("mainLayer",layer);

        }
        #endregion
    }
}
