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
    public abstract class Node
    {
        Vector2 position_ = Vector2.Zero;
        public void setPosition(Vector2 pos) {
            position_ = pos;
            positionS_ = position_;
        }
        public void setPosition(float posX,float posY) {
            position_ = new Vector2(posX,posY);
            positionS_ = position_;
        }
        public Vector2 position { get { return position_; } }
        public Vector2 positionRelative
        {
            get
            {
                if (parent_ != null)
                {
                    Vector2 pos = position_; 
                    if (parent_.rotationRelative != 0)
                    {
                        //calculate translation relative
                        float angleFromParentOrigin = Physics2D.AngleToInRadian(
                            Vector2.Zero, position_);
                        float distanceFromParentOrigin = Physics2D.DistanceTo(
                          Vector2.Zero, position_);
                        double newAngle = angleFromParentOrigin + MathHelper.ToRadians(parent_.rotationRelative);
                        pos = new Vector2((float)Math.Cos(newAngle) * distanceFromParentOrigin,
                            (float)Math.Sin(newAngle) * distanceFromParentOrigin);
                    }
                    pos *= parent_.scaleRelative.X;
                    return pos + parent_.positionRelative;
                }
                return position_;
            }
        }

        public Vector2 anchorPoint_ = new Vector2(0.5f, 0.5f);
        
        float rotation_ = 0.0f;
        public float rotation { get { return rotation_; } }
        public float rotationRelative
        {
            get
            {
                if (parent_ != null)
                {
                    return eulerAngle(rotation_ + parent_.rotationRelative);
                }
                return rotation_;
            }
        }

        public float eulerAngle(float angle)
        {
            while (angle > 360) angle -= 360;
            while (angle < -360) angle += 360;
            return angle;
        }
        public void setRotation(float angleInDegree)
        {
            rotation_ = eulerAngle(angleInDegree);
        }

        Vector2 scale_ = Vector2.One;
        public void setScale(Vector2 inputScale)
        {
            scale_ = inputScale;
        }
        public void setScale(float inputScale)
        {
            scale_ = Vector2.One*inputScale;
        }
        public Vector2 scale { get { return scale_; } }
        public Vector2 scaleRelative
        {
            get
            {
                if (parent_ != null)
                {
                    return scale_ * parent_.scaleRelative;
                }
                return scale_;
            }
        }

        public void setAlpha(float ialpha) { alpha_ = ialpha; }
        float alpha_ = 1.0f;
        public float alpha { get { return alpha_; } }
        public float alphaRelative
        {
            get
            {
                if (parent_ != null)
                {
                    return alpha_ * parent_.alphaRelative;
                }
                return alpha_;
            }
        }

        public bool visible_ = true;
        public bool active_ = true;
        public void setZOrder(float z)
        {
            zOrder_ = z;
        }
        float zOrder_ = 0; // -1000 to 1000
        public float zOrder { get { return zOrder_; } }
        public float zOrderRelative
        {
            get
            {
                if (parent_ != null)
                {
                    return zOrder_ + parent_.zOrderRelative+1;
                }
                return zOrder_;
            }
        }
        public Color color_ = Color.White;

        public string tag_ = "";
        Node parent_ = null;
        public Node parent { get { return parent_; } }
        public void setParent(Node parent) { parent_ = parent; }

        Dictionary<string, Node> children_ = new Dictionary<string, Node>();
        public Dictionary<string, Node> children { get { return children_; } }
        bool checkIfChildrenKeyExist(string key)
        {

            ICollection<string> keys = children_.Keys;
            foreach (string id in keys)
            {
                if (key == id)
                {
                    //AIW.Log(this, "node key : " + key + " exist and cannot be added.");
                    return true;
                }
            }
            return false;
        }


        public virtual Node addChild(string tag, Node node)
        {
            if (!checkIfChildrenKeyExist(tag))
            {
                if (node.parent_ == null)
                {
                    node.setParent(this);
                    node.tag_ = tag;
                }
               
                children_.Add(tag, node);
                return node;
            }
            else return null;
        }

        public virtual Node addChild(Node node)
        {
            string tag = Director.autoGenerateTag;
            if (!checkIfChildrenKeyExist(tag))
            {
                if (node.parent_ == null)
                {
                    node.setParent(this);
                    node.tag_ = tag;
                }

                children_.Add(tag, node);
                return node;
            }
            else return null;
        }

        public Node getChildByTag(string tag)
        {
            if (checkIfChildrenKeyExist(tag))
            {
                return children_[tag];
            }
            return null;
        }
        public void removeFromParent()
        {
            List<Node> nodeToDelete = new  List<Node>();
             ICollection<string> keys = children.Keys;
             foreach (string id in keys)
             {
                 nodeToDelete.Add((Node)children[id]);
             }
            foreach(Node child in nodeToDelete)
            {
                child.removeFromParent();
            }

            parent_.children.Remove(tag_);
            if (parent_.parent != null)
            {
                if (parent_.parent is Layer)
                {
                    parent_.parent.children.Remove(parent_.tag_ + "." + tag_);
                }
            }

        }
        public void removeSelf()
        {
            if (!(parent is Layer)) return;
            ((Layer)parent).addChildToDelete(this);
        }
        public virtual void update(GameTime gt) { }
        #region Update State
        public int state_ = 0;
        public void updateState(GameTime gt)
        {
            switch (state_)
            {
                case 0: state0(gt);
                    break;
                case 1: state1(gt);
                    break;          
                case 2: state2(gt);
                    break;
                case 3: state3(gt);
                    break;
                case 4: state4(gt);
                    break;
                case 5: state5(gt);
                    break;
                case 6: state6(gt);
                    break;
                case 7: state7(gt);
                    break;
                case 8: state8(gt);
                    break;
                case 9: state9(gt);
                    break;
                default: break;
            };
        }
        public virtual void state0(GameTime gt)
        {
        }
        public virtual void state1(GameTime gt)
        {
        }
        public virtual void state2(GameTime gt)
        {
        }
        public virtual void state3(GameTime gt)
        {
        }
        public virtual void state4(GameTime gt)
        {
        }
        public virtual void state5(GameTime gt)
        {
        }
        public virtual void state6(GameTime gt)
        {
        }
        public virtual void state7(GameTime gt)
        {
        }
        public virtual void state8(GameTime gt)
        {
        }
        public virtual void state9(GameTime gt)
        {
        }
        #endregion

        #region Physics
        Vector2 positionS_ = Vector2.Zero;
        float direction_ = 0.0f;
        Vector2 velocity_ = Vector2.Zero;
        Vector2 acceleration_ = Vector2.Zero;

        float seed = 0.0f;
        public bool MoveToTarget(GameTime gameTime, float speed, Vector2 target)
        {
            direction_ = Physics2D.AngleToInRadian(position, target);
            MoveDirectionInRadian(gameTime, speed, direction_ +
                MathHelper.ToRadians(seed));

            float threshold = speed + 1;// pixel
            if (Physics2D.DistanceTo(position, target) < threshold)
            {
                setPosition(target);
                return true;
            }
            return false;
        }
        public void MoveDirectionInRadian(GameTime gameTime,
         float speed, float _directionInRadian)
        {
            float x, y;
            x = (float)Math.Cos(_directionInRadian);
            y = (float)Math.Sin(_directionInRadian);
            velocity_.X = (float)(x * speed);
            velocity_.Y = (float)(y * speed);

            UpdatePhysicsPosition(gameTime);
        }
        public void MoveDirectionInDegree(GameTime gameTime,
     float speed, float _directionInDegree)
        {
            float _directionInRadian = MathHelper.ToRadians(_directionInDegree);
            MoveDirectionInRadian(gameTime, speed, _directionInRadian);
        }
    
        public void GenerateSeed()
        {
            float magnitude = 10.0f;
            int range = 5;
            Random rand = new Random(
                (int)positionS_.X + (int)positionS_.Y + DateTime.Now.Millisecond);
            if (rand.Next(2) == 0)
                seed = rand.Next(range) * magnitude;
            else
                seed = rand.Next(range) * magnitude * (-1);
        }

        void UpdatePhysicsPosition(GameTime gameTime)
        {
            float dt = (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 16.0);
            velocity_.X += acceleration_.X * dt;
            velocity_.Y += acceleration_.Y * dt;
            positionS_.X += velocity_.X * dt;
            positionS_.Y += velocity_.Y * dt;
            setPosition(positionS_);
        }
        #endregion
    }


    public abstract class Layer : Node
    {
        List<Node> nodeToDelete = new List<Node>();
        public void updateNodeToDelete()
        {
            foreach (Node node in nodeToDelete)
            {
                node.removeFromParent();
            }
            nodeToDelete.Clear();
        }
        public void addChildToDelete(Node node)
        {
            nodeToDelete.Add(node);
        }

        List<Node> nodeToAdd = new List<Node>();
        public void updateNodeToAdd()
        {
            foreach (Node node in nodeToAdd)
            {
                addChild(node);
            }
            nodeToAdd.Clear();
        }
        public void addChildToAdd(Node node)
        {
            nodeToAdd.Add(node);
        }

        public override Node addChild(Node node)
        {
            base.addChild(node);
            ICollection<string> keys = node.children.Keys;
            foreach (string id in keys)
            {
                base.addChild(node.tag_ + "." + id, node.children[id]);
            }

            return node;
        }
        public override Node addChild(string tag, Node node)
        {
          base.addChild(tag, node); 
            ICollection<string> keys = node.children.Keys;
            foreach (string id in keys)
            {
                base.addChild(tag+"."+id, node.children[id]);
            }

            return node;
        }
        public void popFromParent()
        {
            active_ = false;
            visible_ = false;
            ((Scene)parent).popLayer(this);
        }
    }


    public abstract class Scene : Node
    {
        public Scene prevScene_;
        public virtual void viewDidLoad() { }
        public virtual void viewDidUnload() { }

        List<Layer> layerToAdd = new List<Layer>();
        List<Layer> layerToDelete = new List<Layer>();
         public void updatePushAndPopLayer()
        {
            foreach (Layer layer in layerToAdd)
            {
                addChild(layer.tag_, layer);
            }
            layerToAdd.Clear();

            foreach (Layer layer in layerToDelete)
            {
                layer.removeFromParent();
            }
            layerToDelete.Clear();
        }
         public void pushLayer(string tag, Layer layer)
         {
             layer.tag_ = tag;
             layerToAdd.Add(layer);
         }
         public void popLayer(Layer layer)
         {
             layerToDelete.Add(layer);
         }

    }

    public class Director
    {
        static Int64 generateTagIndexCount = 0;
        public static string autoGenerateTag
        {
            get
            {
                string tag = "" + generateTagIndexCount;
                generateTagIndexCount++;
                //if (generateTagIndexCount > 223372036854775808)
                //{
                //    generateTagIndexCount = 0;
                //}
                return tag;
            }
        }

        Scene runningScene_;
        //Singlaton
        static Director instanceOfDirector;
        public static Director sharedDirector
        {
            get
            {
                if (instanceOfDirector == null)
                    instanceOfDirector = new Director();

                return instanceOfDirector;
            }
        }

        public void update(GameTime gt) 
        {
            //if (AIW.Input.isKeyReleased(Keys.F1)) pauseOrResume();

            if (isPaused_) return;
            if (runningScene_ == null) return;
            
             runningScene_.update(gt);
             ICollection<string> keys = runningScene_.children.Keys;
             foreach (string id in keys)
             {
                 if (!runningScene_.children[id].active_) continue;
                 if (runningScene_.children[id] is Layer)
                 {
                     Layer layer = (Layer)runningScene_.children[id];

                     layer.update(gt);
                    
                     ICollection<string> keys2 = layer.children.Keys;
                     foreach (string id2 in keys2)
                     {
                         if (!layer.children[id2].active_) continue;
                         layer.children[id2].update(gt);                        
                     }
                     layer.updateNodeToDelete();
                     layer.updateNodeToAdd();
                    
                 }
             }
             runningScene_.updatePushAndPopLayer();
        }

        public void draw(GameTime gt) 
        {
            if (runningScene_ == null) return;
            Graphic2DEngine g = AIW.OS.Graphic2DEngine;
            ICollection<string> keys = runningScene_.children.Keys;
            float layerZ = 0;
            foreach (string id in keys)
            {
                layerZ+= 100;
                if (!runningScene_.children[id].visible_) continue;
                if (runningScene_.children[id] is Layer)
                {
                     Layer layer = (Layer)runningScene_.children[id];
                    ICollection<string> keys2 =layer.children.Keys;
                    foreach (string id2 in keys2)
                    {
                        if (layer.children[id2] is SpriteObj)
                        {
                            SpriteObj obj = (SpriteObj)layer.children[id2];

                            g.drawSprite2D(obj.textureName, obj.positionRelative, obj.anchorPoint_, obj.rotationRelative,
                               obj.scaleRelative, obj.color_, obj.alphaRelative, obj.zOrderRelative + layerZ);
                        }
                        else if (layer.children[id2] is TextObj)
                        {
                            TextObj obj = (TextObj)layer.children[id2];
                            g.drawLabel(obj.font, obj.text, obj.positionRelative, obj.rotationRelative, obj.scaleRelative.X,
                                obj.color_, obj.alphaRelative, obj.zOrderRelative + layerZ);
                        }
                        else if (layer.children[id2] is RectangleObj)
                        {
                            RectangleObj obj = (RectangleObj)layer.children[id2];
                            g.drawRectangle(obj.positionRelative,
                                obj.contentSize.X * obj.scaleRelative.X,
                                obj.contentSize.Y * obj.scaleRelative.Y,
                                obj.rotationRelative, obj.color_, obj.alphaRelative, obj.anchorPoint_, obj.zOrderRelative + layerZ);
                            
                        }
                    }
                }

            }    

        }

        bool isPaused_ = false;
        public void pause()
        {
            if (!isPaused_) isPaused_ = true;
        }
        public void resume()
        {
            if (isPaused_) isPaused_ = false;
        }
        public void pauseOrResume()
        {
            if (isPaused_) resume();
            else pause();
        }
       
     
        public void pushScene(Scene scene)
        {
            if (runningScene_ == null)
            {
                runningScene_ = scene;
            }
            else
            {
                Scene tmp = runningScene_;
                runningScene_ = scene;
                runningScene_.prevScene_ = tmp;
            }
        }
        public void popScene()
        {
            if (runningScene_.prevScene_ != null)
            {
                runningScene_ = runningScene_.prevScene_;
            }
        }
        public void replaceScene(Scene scene)
        {
            runningScene_ = scene;
        }

        public void runWithScene(Scene scene)
        {
            runningScene_.viewDidUnload();
            replaceScene(scene);
            scene.viewDidLoad();
        }

        public static float textureWidth(string name)
        {
            return AIW.OS.Graphic2DEngine.Texture2DStock.TextureWidth(name);
        }
        public static float textureHeight(string name)
        {
            return AIW.OS.Graphic2DEngine.Texture2DStock.TextureHeight(name);
        }
    }

    public class RectangleObj : Node
    {
        public RectangleObj()
        {
            
        }
        public RectangleObj(float width, float height)
        {
            setContentSize(width, height);
        }
        Vector2 contentSize_ = Vector2.Zero;
        public Vector2 contentSize { get { return contentSize_; } }
        public void setContentSize(float width, float height)
        {
            contentSize_ = new Vector2(width, height);
        }

        public Rectangle boundingBox
        {
            get
            {              
                return boundingBoxWithArea(100);
            }
        }
        public Rectangle boundingBoxWithArea(float percentArea)
        {
            percentArea *= 0.01f;
                Rectangle rect = new Rectangle(
                    (int)positionRelative.X, (int)positionRelative.Y,
                    (int)(contentSize_.X * percentArea * scaleRelative.X),
                    (int)(contentSize_.Y * percentArea * scaleRelative.Y));
                return rect;
            
        }
        public bool isContainPoint(Vector2 point)
        {
            if(boundingBox.Contains((int)point.X,(int)point.Y)) return true;
            return false;
        }
        public bool isContainPointWithArea(Vector2 point, float percentArea)
        {
            if (boundingBoxWithArea(percentArea).Contains(
                (int)point.X, (int)point.Y))
                return true;
            return false;
        }

        public bool isContainRect(Rectangle rect)
        {
            if (boundingBox.Contains(rect)) return true;
            return false;
        }
        public bool isContainRectWithArea(Rectangle rect, float percentArea)
        {
            if (boundingBoxWithArea(percentArea).Contains(rect)) return true;
            return false;
        }

    }

    public class SpriteObj : RectangleObj
    {
        string filename_ = "";
        public void setFilename(string filename)
        {
            filename_ = filename;
        }
        public string textureName { get { return filename_; } }

        public SpriteObj(string filename)
        {
            filename_ = filename;
            setContentSize(
                Director.textureWidth(filename_),
                Director.textureHeight(filename_));
        }
    }
    public class TextObj : Node
    {
        string text_ = "";       
        public string text { get { return text_; } }
        public void setText(string text)
        {
            text_ = text;
        }

        kFont font_ = kFont.Font;
        public kFont font { get { return font_; } }
        public void setFont(kFont font)
        {
            font_ = font;
        }

        public TextObj(string text)
        {
            setText(text);
        }
    }

    public class TestScene2 : Scene
    {
        public class TestLayer:Layer
        {
            public TestLayer()
            {
                SpriteObj obj = new SpriteObj("profileNui2");
                addChild("profileNui2", obj);
            }
            public override void update(GameTime gt)
            {
                base.update(gt);              
                if (AIW.Input.isLeftMousePressed)
                {
                    Director.sharedDirector.runWithScene(new TestScene());
                }
            }
        }

        public TestScene2()
        {
            TestLayer layer = new TestLayer();
            addChild("testLayer", layer);


        }
    }

    public class TestScene : Scene
    {
        public TestScene()
        {
            TestLayer layer = new TestLayer();
            addChild("testLayer", layer);
        }
    }

    public class TestLayer : Layer
    {
        SpriteObj obj2;
        RectangleObj obj4;
        public TestLayer()
        {
            //TestNode node = new TestNode();
            //addChild("testNode", node);
            //SpriteObj obj = new SpriteObj("profileNui2");
           
            //obj2 = new SpriteObj("profileAnn2");
            ////obj2.position_ = AIW.ScreenCenter;
            //obj2.anchorPoint_ = Vector2.One;
            //obj2.setRotation(180);

            //TextObj obj3 = new TextObj("ARIA");
            //obj3.position_ = AIW.ScreenCenter;

            //obj4 = new RectangleObj(100, 50);
            //obj4.color_ = Color.Blue;
            //obj4.position_ = AIW.ScreenCenter;
            //obj4.anchorPoint_ = Vector2.One;

            ////addChild("profileNui2", obj);
            //addChild("profileAnn2", obj2);
            //addChild("aria", obj3);
            //addChild("box", obj4);
           
         
        }
        
        public override void update(GameTime gt)
        {
            base.update(gt);
            obj2.setRotation(obj2.rotationRelative+1);
            obj4.setRotation(obj4.rotationRelative + 1);
            if (AIW.Input.isLeftMousePressed)
            {
                Director.sharedDirector.runWithScene(new TestScene2());
            }
           
        }

    }

    public class TestNode : Node
    {
       // public TestNode() { AIW.Log(this, "test node constructor"); }
    }

}
/*

//Layer


//CircleObj
public float radius_ = 0;

//LineObj
public Vector2 head_ = Vector2.Zero;
public Vector2 tail_ = Vector2.Zero;

*/
