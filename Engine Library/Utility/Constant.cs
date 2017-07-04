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
    /*
         Convert number to string
         use double y = Convert.ToDouble("543.56"); 
     

      ** Singlaton Pattern **
          public class AppModel
          {

              static AppModel instanceOfAppModel;
              public static AppModel sharedATMModel()
              {
                  if (instanceOfAppModel == null)
                  {
                      instanceOfAppModel = new AppModel();
                  }
                  return instanceOfAppModel;
              }
        }
      */

    public static class ASCIIManager
    {
        // A is 65
        // a is 97
        public static int ConvertToInt(char input)
        {
            return (int)input;
        }

        public static char ConvertToChar(int input)
        {
            return (char)input;
        }
    }
    public static class ColorPalette
    {
        public static Color BlueWP7
        {
            get { return new Color(35, 160, 220); }
        }
        public static Color OrangeWP7
        {
            get { return new Color(240, 150, 30); }
        }
        public static Color RedWP7
        {
            get { return new Color(230, 30, 40); }
        }
        public static Color GreenWP7
        {
            get { return new Color(50, 153, 72); }
        }

    }
    public enum TransitionState
    {
        TransitionOn,
        Active,
        TransitionOff,
        Hidden,
    }
    public enum kFont
    {
        Font,
        HeadFont,
        TitleFont,
    }
}
