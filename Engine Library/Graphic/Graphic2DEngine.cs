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
    public class Graphic2DEngine
    {
        #region Model

        SpriteBatch spriteBatch;
        Texture2DStock texture2DStock;
        public Texture2DStock Texture2DStock { get { return texture2DStock; } }
        public FontStock fontStock;
        DepthManager depthManager;
        public DepthManager DepthManager
        { get { return depthManager; } }
        #endregion

        #region Initialize
        public Graphic2DEngine()
        {
            spriteBatch = AIW.OS.SpriteBatch;
            initTexture2DStock();
            initFontStock();
            initDepthManager();
        }

        void initDepthManager()
        {
            depthManager = new DepthManager();
        }
        void initTexture2DStock()
        {
            texture2DStock = new Texture2DStock();
            texture2DStock.LoadContent();
        }
        void initFontStock()
        {
            fontStock = new FontStock();
        }

        #endregion

        #region Draw Sprite
        public void drawSprite2D(string textureName,
            Vector2 position, Vector2 anchorPoint,
            float rotation, Vector2 scale,
             Color color, float alpha,
            float zOrder)
        {
            Texture2D texture = texture2DStock.Textures(textureName);
            Vector2 origin = texture2DStock.TextureSize(textureName) * anchorPoint;

            spriteBatch.Draw(texture,
                position,
                null,
                ColorWithAlpha(color, alpha),
             MathHelper.ToRadians(rotation),
             origin,
             scale,
             SpriteEffects.None,
                getDepth(zOrder));
        }
        public float getDepth(float zOrder)
        {
            return (zOrder + 1000) * 0.0001f;
        }

        public void Old_Sprite2D(string textureName,
            Vector2 position, Vector2 origin,
            float rotate, Vector2 scale,
             Color color, float alpha,
            float depth)
        {
            Texture2D texture = texture2DStock.Textures(textureName);
            spriteBatch.Draw(texture, position,
                null, ColorWithAlpha(color, alpha),
             rotate, origin, scale, SpriteEffects.None, depth);
        }


        #endregion

        #region Primitive Draw
        public void SolidColor(Color solidColor, float alpha, float depth)
        {

            spriteBatch.Draw(texture2DStock.Textures("blankTexture"),
                             new Rectangle(0, 0,
                                 (int)AIW.ScreenSize.X, (int)AIW.ScreenSize.Y),
                            ColorWithAlpha(solidColor, alpha));
            Old_Rectangle(Vector2.Zero, AIW.ScreenWidth, AIW.ScreenHeight, 0.0f,
               solidColor, alpha, false, depth);
        }
        public void Line(Vector2 a, Vector2 b,
          Color col, float alpha, float depth)
        {

            Vector2 Origin = new Vector2(0.5f, 0.0f);
            Vector2 diff = b - a;
            Vector2 Scale = new Vector2(2.0f, diff.Length()
                / texture2DStock.TextureHeight("blankTexture"));
            float angle = (float)(Math.Atan2(diff.Y, diff.X) - MathHelper.PiOver2);


            spriteBatch.Draw(texture2DStock.Textures("blankTexture"),
                a, null, ColorWithAlpha(col, alpha), angle,
               Origin, Scale, SpriteEffects.None, depth);
        }
        public void Old_Rectangle(Vector2 position, float width,
         float height, float rotate, Color col, float alpha,
          bool centerAtOrigin, float depth)
        {
            Vector2 center = Vector2.Zero;
            if (centerAtOrigin)
                center = new Vector2(0.5f, 0.5f);

            spriteBatch.Draw(texture2DStock.Textures("blankTexture"), position,
                null, ColorWithAlpha(col, alpha), rotate,
             center, new Vector2(width, height), SpriteEffects.None, depth);
        }
        public void drawRectangle(Vector2 position, float width,
        float height, float rotation, Color col, float alpha,
        Vector2 anchorPoint, float zOrder)
        {
            Vector2 origin = anchorPoint;

            spriteBatch.Draw(texture2DStock.Textures("blankTexture"), position,
                null, ColorWithAlpha(col, alpha), MathHelper.ToRadians(rotation),
             origin, new Vector2(width, height), SpriteEffects.None, getDepth(zOrder));
        }
        public void Circle(Vector2 position, float radius,
            Color col, float alpha, float depth)
        {
            spriteBatch.Draw(texture2DStock.Textures("circleTexture"),
                position, null, ColorWithAlpha(col, alpha), 0.0f,
            texture2DStock.TextureOrigin("circleTexture")
               , radius * 0.01f, SpriteEffects.None, depth);
        }
        public void drawLabel(kFont fontType, string text,
             Vector2 position, float rotate, float scale, Color color, float alpha, float zOrder)
        {
            SpriteFont font;
            switch (fontType)
            {
                case kFont.Font: font = fontStock.Font;
                    break;
                case kFont.HeadFont: font = fontStock.HeadFont;
                    break;
                case kFont.TitleFont: font = fontStock.TitleFont;
                    break;

                default: font = fontStock.Font;
                    break;
            }
            spriteBatch.DrawString(font, text, position,
                 ColorWithAlpha(color, alpha), MathHelper.ToRadians(rotate), Vector2.Zero,
                               scale, SpriteEffects.None, getDepth(zOrder));
        }
        public void Old_Label(kFont fontType, string text,
            Vector2 position, float rotate, float scale, Color color, float alpha, float depth)
        {
            SpriteFont font;
            switch (fontType)
            {
                case kFont.Font: font = fontStock.Font;
                    break;
                case kFont.HeadFont: font = fontStock.HeadFont;
                    break;
                case kFont.TitleFont: font = fontStock.TitleFont;
                    break;

                default: font = fontStock.Font;
                    break;
            }
            spriteBatch.DrawString(font, text, position,
                 ColorWithAlpha(color, alpha), rotate, Vector2.Zero,
                               scale, SpriteEffects.None, depth);
        }
        #endregion

        #region Utility Function

        public Color ColorWithAlpha(Color sourceColor, float alpha)
        {
            alpha = MathHelper.Clamp(alpha, 0, 1);
            byte alphaByte = (byte)(alpha * 255);
            return new Color(sourceColor.R,
                        sourceColor.G,
                       sourceColor.B,
                       alphaByte);


        }
        #endregion
    }

    public class DepthManager
    {
        const float initialDepth = 0.5f;
        const float initialDeveloperDepth = 0.2f;
        float nodeLayerDepth = initialDepth;
        public void ResetDepth()
        {
            nodeLayerDepth = initialDepth;
        }
        public float RequestDepth()
        {
            nodeLayerDepth -= 0.0001f;
            return nodeLayerDepth;
        }
        public float RequestDepthDeveloper()
        {
            nodeLayerDepth -= 0.0001f;
            return nodeLayerDepth - initialDepth + initialDeveloperDepth;
        }

    }

    public class Texture2DStock
    {
        #region Model
        ContentManager content;
        Texture2D blankTexture;
        Dictionary<string, Texture2D> textures =
        new Dictionary<string, Texture2D>();
        public Texture2D Textures(string name)
        {
            if (checkIfTexture2DKeyExist(name)) return textures[name];
            return blankTexture;
        }
        bool checkIfTexture2DKeyExist(string name)
        {
            ICollection<string> index = textures.Keys;
            foreach (string id in index)
            {
                if (name == id)
                {
                    return true;
                }
            }
            AIW.SystemLog(this, "Error, Texture2D key : " + name + " not found.");
            return false;
        }
        bool checkIfSameTexture2DKeyExist(string name)
        {
            ICollection<string> index = textures.Keys;
            foreach (string id in index)
            {
                if (name == id)
                {
                    AIW.SystemLog(this, "Same Texture2D key : " + name + " exist and cannot be added.");
                    return true;
                }
            }
            return false;
        }
        public void AddTexture2D(string name)
        {
            if (!checkIfSameTexture2DKeyExist(name))
            {
                Texture2D texture = content.Load<Texture2D>("Sprite2D\\" + name);
                textures.Add(name, texture);
                AIW.SystemLog(this, "texture " + name + " added.");
            }
        }
        public void AddTexture2D(string folderName, string name)
        {
            if (checkIfSameTexture2DKeyExist(name)) return;

            Texture2D texture = content.Load<Texture2D>("Sprite2D\\" + folderName + "\\" + name);
            textures.Add(name, texture);
            AIW.SystemLog(this, "texture " + name + " added.");
        }
        public void AddTexture2D(string folderName, string subFolderName, string name)
        {
            if (checkIfSameTexture2DKeyExist(name)) return;

            Texture2D texture = content.Load<Texture2D>(
                "Sprite2D\\" + folderName + "\\" + subFolderName + "\\" + name);
            textures.Add(name, texture);
            AIW.SystemLog(this, "texture " + name + " added.");
        }
        public float TextureWidth(string name)
        {
            return Textures(name).Width;
        }
        public float TextureHeight(string name)
        {
            return Textures(name).Height;
        }
        public Vector2 TextureOrigin(string name)
        {
            return new Vector2(TextureWidth(name) * 0.5f, TextureHeight(name) * 0.5f);
        }
        public Vector2 TextureSize(string name)
        {
            return new Vector2(TextureWidth(name), TextureHeight(name));
        }
        void initDefaultTexture()
        {
            initBlankTexture();


            Texture2DModel texture2DModel = new Texture2DModel(this);
            texture2DModel.LoadTexture2D();
        }
        #endregion


        #region Initialize
        public void LoadContent()
        {
            if (content == null)
                content = new ContentManager(AIW.OS.Game.Services, "Content");
            initDefaultTexture();
            AIW.SystemLog(this, "load content");
        }
        void initBlankTexture()
        {
            blankTexture = new Texture2D(AIW.OS.GraphicsDevice, 1, 1);
            blankTexture.SetData(new Color[] { Color.White });
            textures.Add("blankTexture", blankTexture);
        }
        #endregion

    }

    public class GraphicHelper
    {
        public void MousePositionInfo(Color color)
        {
            int shiftX = 50;
            int shiftY = 0;
            if (!(AIW.OS.InputState.MS.X > AIW.ScreenWidth - 50))
            {
                shiftX = 0;
            }
            else
            {
                shiftX = 50;
            }
            if (!(AIW.OS.InputState.MS.Y < 40))
            {
                shiftY = 0;
            }
            else
            {
                shiftY = -30;
                shiftX = 50;
            }

            AIW.OS.Graphic2DEngine.Old_Label(kFont.Font,
                "x " + AIW.OS.InputState.MS.X,
                new Vector2(AIW.OS.InputState.MS.X - shiftX,
                    AIW.OS.InputState.MS.Y - shiftY),
                    0.0f, 1.0f, color, 1.0f,

                     AIW.OS.Graphic2DEngine.DepthManager.RequestDepthDeveloper());
            shiftY += 10;
            AIW.OS.Graphic2DEngine.Old_Label(kFont.Font,
               "y " + AIW.OS.InputState.MS.Y,
               new Vector2(AIW.OS.InputState.MS.X - shiftX,
                   AIW.OS.InputState.MS.Y - shiftY),
                   0.0f, 1.0f, color, 1.0f,
                    AIW.OS.Graphic2DEngine.DepthManager.RequestDepthDeveloper());

            float ratioX = 0;
            float ratioY = 0;         
            ratioX = (float)AIW.OS.InputState.MS.X / (float)AIW.ScreenWidth;
            ratioY = (float)AIW.OS.InputState.MS.Y / (float)AIW.ScreenHeight;

            AIW.OS.Graphic2DEngine.Old_Label(kFont.Font,
              "x ratio: " + String.Format("{0:0.00}", ratioX)   ,
              new Vector2(10,
                  AIW.ScreenHeight-40),
                  0.0f, 1.0f, color, 1.0f,
                   AIW.OS.Graphic2DEngine.DepthManager.RequestDepthDeveloper());
            AIW.OS.Graphic2DEngine.Old_Label(kFont.Font,
             "y ratio: " + String.Format("{0:0.00}", ratioY),
             new Vector2(10,
                 AIW.ScreenHeight - 30),
                 0.0f, 1.0f, color, 1.0f,
                  AIW.OS.Graphic2DEngine.DepthManager.RequestDepthDeveloper());
        }
        public void LogMousePosition()
        {
            AIW.Log(this, "X : " + AIW.OS.InputState.MS.X);
            AIW.Log(this, "Y : " + AIW.OS.InputState.MS.Y);
            Vector2 ratioOnScreen =
                AIW.RatioByScreenPos(AIW.OS.InputState.MS.X,
                                        AIW.OS.InputState.MS.Y);
            AIW.Log(this, "X ratio : " + ratioOnScreen.X);
            AIW.Log(this, "Y ratio : " + ratioOnScreen.Y);
        }
        public void GridCursor(Color color)
        {
            //Horizontal line
            AIW.OS.Graphic2DEngine.Old_Rectangle(
                new Vector2(AIW.ScreenWidth * 0.5f, AIW.OS.InputState.MS.Y),
                AIW.ScreenWidth, 1, 0.0f, color, 1.0f, true,
                AIW.OS.Graphic2DEngine.DepthManager.RequestDepthDeveloper());

            //Vertical line
            AIW.OS.Graphic2DEngine.Old_Rectangle(
               new Vector2(AIW.OS.InputState.MS.X, AIW.ScreenHeight * 0.5f),
              1, AIW.ScreenHeight, 0.0f, color, 1.0f, true,
               AIW.OS.Graphic2DEngine.DepthManager.RequestDepthDeveloper());
        }
        public void DragRectangle(Color color)
        {

            InputState input = AIW.OS.InputState;
            if (!input.IsClick) return;

          
            Graphic2DEngine g = AIW.OS.Graphic2DEngine;

            //Circle point
            AIW.OS.Graphic2DEngine.Circle(
                AIW.OS.InputState.MousePosition,
                10, color, 1.0f,
                 AIW.OS.Graphic2DEngine.DepthManager.RequestDepthDeveloper());
            AIW.OS.Graphic2DEngine.Circle(
                  AIW.OS.InputState.TouchBeganPosition,
                  10, color, 1.0f,
                   AIW.OS.Graphic2DEngine.DepthManager.RequestDepthDeveloper());

            //Label
            float widthRatioX = 0;
            float heightRatioY = 0;
            AIW.OS.Graphic2DEngine.Old_Label(kFont.Font,
                "width " + Math.Abs(AIW.OS.InputState.MS.X - AIW.OS.InputState.TouchBeganPosition.X),
                AIW.OS.InputState.TouchBeganPosition,
                0.0f, 1.0f, color, 1.0f,
                  AIW.OS.Graphic2DEngine.DepthManager.RequestDepthDeveloper());
            AIW.OS.Graphic2DEngine.Old_Label(kFont.Font,
              "height " + Math.Abs(AIW.OS.InputState.MS.Y - AIW.OS.InputState.TouchBeganPosition.Y),
              AIW.OS.InputState.TouchBeganPosition + new Vector2(0, 10),
              0.0f, 1.0f, color, 1.0f,
                AIW.OS.Graphic2DEngine.DepthManager.RequestDepthDeveloper());

            widthRatioX = Math.Abs(AIW.OS.InputState.MS.X - AIW.OS.InputState.TouchBeganPosition.X) /(float) AIW.ScreenWidth;
            heightRatioY = Math.Abs(AIW.OS.InputState.MS.Y - AIW.OS.InputState.TouchBeganPosition.Y) / (float)AIW.ScreenHeight;
            AIW.OS.Graphic2DEngine.Old_Label(kFont.Font,
        "width ratio: "  +String.Format("{0:0.00}", widthRatioX),
        new Vector2(10, -60+AIW.ScreenHeight),
        0.0f, 1.0f, Color.White, 1.0f,
          AIW.OS.Graphic2DEngine.DepthManager.RequestDepthDeveloper());
            AIW.OS.Graphic2DEngine.Old_Label(kFont.Font,
"height ratio: " + String.Format("{0:0.00}", heightRatioY),
new Vector2(10, -50 + AIW.ScreenHeight),
0.0f, 1.0f, Color.White, 1.0f,
AIW.OS.Graphic2DEngine.DepthManager.RequestDepthDeveloper());
            //Line           
            if (input.MS.X < input.TouchBeganPosition.X)
            {
                g.Old_Rectangle(input.MousePosition,
                    input.TouchBeganPosition.X - input.MS.X,
                    1, 0.0f, color, 1.0f, false,
                     AIW.OS.Graphic2DEngine.DepthManager.RequestDepthDeveloper());
                g.Old_Rectangle(new Vector2(input.MS.X, input.TouchBeganPosition.Y),
                   input.TouchBeganPosition.X - input.MS.X,
                   1, 0, color, 1, false,
                      AIW.OS.Graphic2DEngine.DepthManager.RequestDepthDeveloper());
            }
            else
            {
                g.Old_Rectangle(new Vector2(input.TouchBeganPosition.X, input.MS.Y),
                   input.MS.X - input.TouchBeganPosition.Y,
                   1, 0.0f, color, 1.0f, false,
                    AIW.OS.Graphic2DEngine.DepthManager.RequestDepthDeveloper());
                g.Old_Rectangle(input.TouchBeganPosition,
                   input.MS.X - input.TouchBeganPosition.X,
                   1, 0, color, 1, false,
                      AIW.OS.Graphic2DEngine.DepthManager.RequestDepthDeveloper());
            }

            if (input.MS.Y < input.TouchBeganPosition.Y)
            {
                g.Old_Rectangle(input.MousePosition,
                    1, input.TouchBeganPosition.Y - input.MS.Y,
                    0, color, 1, false,
                     AIW.OS.Graphic2DEngine.DepthManager.RequestDepthDeveloper());
                g.Old_Rectangle(new Vector2(input.TouchBeganPosition.X, input.MS.Y),
                  1, input.TouchBeganPosition.Y - input.MS.Y,
                  0, color, 1, false,
                   AIW.OS.Graphic2DEngine.DepthManager.RequestDepthDeveloper());
            }
            else
            {
                g.Old_Rectangle(new Vector2(input.MS.X, input.TouchBeganPosition.Y),
                 1, input.MS.Y - input.TouchBeganPosition.Y,
                 0, color, 1, false,
                  AIW.OS.Graphic2DEngine.DepthManager.RequestDepthDeveloper());
                g.Old_Rectangle(input.TouchBeganPosition,
                  1, input.MS.Y - input.TouchBeganPosition.Y,
                  0, color, 1, false,
                   AIW.OS.Graphic2DEngine.DepthManager.RequestDepthDeveloper());
            }

        }

        public void DrawAllGridInfo()
        {
            MousePositionInfo(Color.White);
            GridCursor(Color.GreenYellow);
            DragRectangle(Color.Fuchsia);
        }

        public void PlaceSprite2D(string textureName)
        {
            Graphic2DEngine g = AIW.OS.Graphic2DEngine;
            g.Old_Sprite2D(textureName, AIW.OS.InputState.MousePosition,
                g.Texture2DStock.TextureOrigin(textureName), 0, Vector2.One, Color.White, 1,
                 AIW.OS.Graphic2DEngine.DepthManager.RequestDepthDeveloper());
        }
    }

}
