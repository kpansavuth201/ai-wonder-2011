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
    public abstract class Old_Node
    {
        #region Model
        public Vector2 position = Vector2.Zero;
        public Vector2 origin = Vector2.Zero;
        public float rotation = 0.0f;
        public float scaleAll = 1.0f;
        public Vector2 scale = Vector2.One;
        public float alpha = 1.0f;
        public bool isActive = true;
        public bool isVisible = true;
        public float z = 0.5f;


        RootLayer parentLayer;
        public RootLayer ParentLayer { get { return parentLayer; } }

        #endregion


        #region Initialize
        public Old_Node(RootLayer _parentLayer)
        {
            initWithParentLayer(_parentLayer);
        }
        void initWithParentLayer(RootLayer tmp)
        {
            parentLayer = tmp;
        }

        public virtual void LoadContent() { }
        public virtual void ViewDidLoad() { }
        public virtual void ViewDidUnload() { }
        #endregion


        #region Controller
        public virtual void Update(GameTime gt) { }
        public virtual void HandleInput(GameTime gt) { }


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
        void ResetDrawParam()
        {
            n = "blank";
            p = Vector2.Zero;
            o = Vector2.Zero;
            r = 0;
            s = Vector2.One;
            c = Color.White;
            a = 1.0f;
        }
        //Get relative variable
        public Vector2 pr(Vector2 _pos)
        {
            if (parentLayer.rotation != 0)
            {
                //calculate translation relative
                float angleFromLayerOrigin = Physics2D.AngleToInRadian(
                    Vector2.Zero, _pos);
                float distanceFromLayerOrigin = Physics2D.DistanceTo(
                  Vector2.Zero, _pos);
                double newAngle = angleFromLayerOrigin + parentLayer.rotation;
                _pos = new Vector2((float)Math.Cos(newAngle) * distanceFromLayerOrigin,
                    (float)Math.Sin(newAngle) * distanceFromLayerOrigin);
            }
            return _pos * parentLayer.scaleAll + parentLayer.position;
        }
        public float rr(float _rotate)
        {
            return _rotate + parentLayer.rotation;
        }

        public Vector2 sr(Vector2 _scale)
        {
            return _scale * parentLayer.scaleAll * parentLayer.scale;
        }
        public float ar(float _alpha)
        {
            return _alpha * parentLayer.alpha;
        }

        public void S(kFont font)//draw string
        {
            AIW.OS.Graphic2DEngine.Old_Label(font, n, p, r, s.X, c, a, AIW.z);
            ResetDrawParam();
        }

        public void D() //draw sprite
        {
            o = AIW.TextureOrigin(n);
            AIW.OS.Graphic2DEngine.Old_Sprite2D(n, p, o, r, s, c, a, AIW.z);
            ResetDrawParam();
        }
        public void DR() //draw sprite relative with parent node
        {
            o = AIW.TextureOrigin(n);
            
            AIW.OS.Graphic2DEngine.Old_Sprite2D(n, pr(p), o,
             rr(r),
            sr(s), c, ar(a), AIW.z);
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
        public virtual void Draw(GameTime gt) { ResetDrawParam(); }

        #endregion
    }
}
