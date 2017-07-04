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
    public abstract class RootLayer
    {
        #region Model
        public Vector2 position = Vector2.Zero;
        public Vector2 origin = Vector2.Zero;
        public float rotation = 0.0f;
        public Vector2 scale = Vector2.One;
        public float scaleAll = 1.0f;
        public float z = 0.5f;
        public float alpha = 1.0f;
        public bool isVisible = true;
        public bool isActive = true;
        public TransitionState state = TransitionState.Hidden;
        ChildLayer nextLayer;
        public bool isFocus
        {
            get
            {
                if (!isActive) return false;
                if (nextLayer == null) return true;
                else return false;
            }
        }
        public RootLayer TopChildLayer
        {
            get
            {
                if (nextLayer != null)
                    return nextLayer.TopChildLayer;
                else return this;
            }
        }

        #endregion

        #region Dictionary Nodes
        Dictionary<string, Old_Node> nodes;
        public Dictionary<string, Old_Node> Nodes
        {
            get { return nodes; }
        }
        bool checkIfSameNodeKeyExist(string name)
        {
            ICollection<string> index = nodes.Keys;
            foreach (string id in index)
            {
                if (name == id)
                {
                    AIW.Log(this, "Same node key : " + name + " exist and cannot be added.");
                    return true;
                }
            }
            return false;
        }
        public Old_Node AddNode(string name, Old_Node tmp)
        {
            if (!checkIfSameNodeKeyExist(name))
            {
                nodes.Add(name, tmp);
            }
            return tmp;
        }
        public virtual void initNodes()
        {
            nodes = new Dictionary<string, Old_Node>();
        }
        #endregion


        #region Initialize
        public RootLayer()
        { initNodes(); }
        public virtual void LoadContent()
        {
            ICollection<string> index = nodes.Keys;
            foreach (string id in index)
            {
                nodes[id].LoadContent();
            }
        }
        public virtual void ViewDidLoad() { }
        public virtual void ViewDidUnload()
        {
            nodes.Clear();
            if (nextLayer != null)
            {
                nextLayer.ViewDidUnload();
                nextLayer = null;
            }
        }
        #endregion


        #region Controller
        public virtual void Update(GameTime gt)
        {
            if (nextLayer != null)
            {
                if (nextLayer.isActive && nextLayer.state == TransitionState.Active) nextLayer.Update(gt);
            }
            if (state == TransitionState.Active)
            {
                ICollection<string> index = nodes.Keys;
                foreach (string id in index)
                {
                    if (nodes[id].isActive)
                    {
                        nodes[id].Update(gt);
                    }
                }
            }
        }
        public virtual void HandleInput(GameTime gt) { }

        public void PushNextLayer(ChildLayer _nextLayer)
        {
            nextLayer = _nextLayer;
            nextLayer.ViewDidLoad();
            nextLayer.LoadContent();
            nextLayer.state = TransitionState.TransitionOn;
        }



        #endregion

        #region Transition
        float TransitionPosition = 0;
        TimeSpan transitionOffDuration = TimeSpan.FromSeconds(0.1);
        TimeSpan transitionOnDuration = TimeSpan.FromSeconds(0.15);
        public virtual bool TransitionOff(GameTime gameTime)
        {
            TransitionPosition += (float)(gameTime.ElapsedGameTime.TotalMilliseconds);
            float alpha = TransitionPosition /
                (float)transitionOffDuration.TotalMilliseconds * 1.0f;

            // Do  FadeBackBufferToColor
            //AIW.OS.Graphic2DEngine.SolidColor(Color.Black, alpha);

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
            //AIW.OS.Graphic2DEngine.SolidColor(Color.Black, alpha);

            if (TransitionPosition >= transitionOnDuration.TotalMilliseconds)
            {
                TransitionPosition = 0.0f;
                return true;
            }
            return false;
        }
        #endregion


        #region View
        #region Draw Param
        public string n = ""; //texture name
        public Vector2 p; //position
        public Vector2 o; //origin
        public float r; // rotation in Radion
        public Vector2 s; //scale
        public Color c; //color
        public float a; //alpha
        // public float d; //depth
        void ResetDrawParam()
        {
            n = "blank";
            p = Vector2.Zero;
            o = Vector2.Zero;
            r = 0;
            s = Vector2.One;
            c = Color.White;
            a = 1.0f;
            //d = z;
        }
        public void D() //draw sprite
        {
            o = AIW.TextureOrigin(n);
            AIW.OS.Graphic2DEngine.Old_Sprite2D(n, p, o, r, s, c, a, AIW.z);
            ResetDrawParam();
        }
        public void S(kFont font)//draw string
        {
            AIW.OS.Graphic2DEngine.Old_Label(font, n, p, r, s.X, c, a, AIW.z);
            ResetDrawParam();
        }
        public void D(bool isCenterOrigin)
        {
            if (isCenterOrigin)
                o = AIW.TextureOrigin(n);
            AIW.OS.Graphic2DEngine.Old_Sprite2D(n, p, o, r, s, c, a, AIW.z);
            ResetDrawParam();
        }
        #endregion
        public virtual void Draw(GameTime gt)
        {
            ResetDrawParam();
            ICollection<string> index = nodes.Keys;
            foreach (string id in index)
            {
                if (nodes[id].isVisible)
                {
                    nodes[id].Draw(gt);
                }
            }
            if (nextLayer != null)
            {
                if (nextLayer.state != TransitionState.Hidden)
                {
                    nextLayer.Draw(gt);
                    if (nextLayer.state == TransitionState.TransitionOff)
                    {
                        if (nextLayer.TransitionOff(gt))
                        {
                            nextLayer.ViewDidUnload();
                            nextLayer.state = TransitionState.Hidden;
                            nextLayer = null;
                        }
                    }
                    else if (nextLayer.state == TransitionState.TransitionOn)
                    {
                        if (nextLayer.TransitionOn(gt))
                        {
                            nextLayer.state = TransitionState.Active;
                            //nextLayer.ViewDidLoad();
                        }
                    }
                }
            }


        }

        #endregion
    }

    public class ChildLayer : RootLayer
    {
        RootLayer parentLayer;
        public RootLayer getParentLayer { get { return parentLayer; } }
        public ChildLayer(RootLayer _parentLayer)
            : base()
        {
            parentLayer = _parentLayer;
        }
        public void PopFromParentLayer()
        {
            this.state = TransitionState.TransitionOff;
        }
    }
}
