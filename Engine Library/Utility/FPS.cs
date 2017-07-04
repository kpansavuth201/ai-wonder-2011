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
    public class FPS
    {
        #region Model
        float totalTime = 0;
        float displayFPS = 0;
        float countFrame = 0;

        public int GetFPS { get { return (int)displayFPS; } }

        void calculateFPS(GameTime gt)
        {
            float ElapsedTime = (float)gt.ElapsedGameTime.TotalSeconds;
            totalTime += ElapsedTime;

            if (totalTime >= 1)
            {
                displayFPS = countFrame;
                countFrame = 0;
                totalTime = 0;
            }
            countFrame += 1;
        }
        public void DisplayFPS(GameTime gt)
        {
            calculateFPS(gt);
            DrawFPS();
        }

        void DrawFPS()
        {

            string text = "FPS " + displayFPS;
            Color color = Color.Gray;

            if (displayFPS < 40) color = Color.Red;
            else if (displayFPS < 50) color = Color.Gold;
            else if (displayFPS < 60) color = Color.White;

            AIW.OS.Graphic2DEngine.Old_Label(kFont.Font, text,
                new Vector2(10, AIW.ScreenHeight - 20), 0,
                1, color, 1,
                  AIW.OS.Graphic2DEngine.DepthManager.RequestDepthDeveloper());
        }

        #endregion
    }
}
