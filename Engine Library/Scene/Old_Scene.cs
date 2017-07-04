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
    public abstract class Old_Scene
    {
        #region Model
        RootLayer rootLayer;
        public RootLayer RootLayer { get { return rootLayer; } }
        public TransitionState state = TransitionState.Hidden;
        #endregion

        #region Initialize
        public Old_Scene(RootLayer _rootLayer)
        {
            rootLayer = _rootLayer;
        }

        public virtual void LoadContent()
        {
            rootLayer.LoadContent();
        }
        public virtual void ViewDidLoad()
        {
            AIW.SystemLog(this, "View Did load.");
            rootLayer.state = TransitionState.TransitionOn;
            rootLayer.ViewDidLoad();
        }
        public virtual void ViewDidUnload()
        {
            AIW.SystemLog(this, "View did unload.");
            rootLayer.state = TransitionState.Hidden;
            rootLayer.ViewDidUnload();
        }
        #endregion


        #region Controller
        public virtual void Update(GameTime gt)
        {
            if (rootLayer.isActive)
            {
                rootLayer.Update(gt);
            }
        }


        #endregion


        #region Transition
        float TransitionPosition = 0;
        TimeSpan transitionOffDuration = TimeSpan.FromSeconds(1.0);
        TimeSpan transitionOnDuration = TimeSpan.FromSeconds(1.5);
        public virtual bool TransitionOff(GameTime gameTime)
        {
            TransitionPosition += (float)(gameTime.ElapsedGameTime.TotalMilliseconds);
            float alpha = TransitionPosition /
                (float)transitionOffDuration.TotalMilliseconds * 1.0f;

            // Do  FadeBackBufferToColor
            AIW.OS.Graphic2DEngine.SolidColor(Color.Black, alpha, 0.5f);

            if (TransitionPosition >= transitionOffDuration.TotalMilliseconds)
            {
                TransitionPosition = 0.0f;
                return true;
            }
            return false;
        }
        public virtual bool TransitionOn(GameTime gameTime)
        {
            TransitionPosition += (float)(gameTime.ElapsedGameTime.TotalMilliseconds);
            float alpha = 1 - TransitionPosition /
                (float)transitionOnDuration.TotalMilliseconds * 1.0f;

            // Do  FadeBackBufferToColor
            AIW.OS.Graphic2DEngine.SolidColor(Color.Black, alpha, 0.5f);

            if (TransitionPosition >= transitionOnDuration.TotalMilliseconds)
            {
                TransitionPosition = 0.0f;
                return true;
            }
            return false;
        }
        #endregion

        #region View
        public virtual void Draw(GameTime gt)
        {
            if (rootLayer.isVisible)
            {
                rootLayer.Draw(gt);
            }

        }

        #endregion
    }

}
