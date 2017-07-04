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
  
        public static class Physics2D
        {
            public static float AngleToInRadian(Vector2 source, Vector2 des)
            {
                return ((float)Math.Atan2((des.Y - source.Y),
                      (des.X - source.X)));
            }
            public static float AngleToInDegree(Vector2 source, Vector2 des)
            {
                float angle = ((float)Math.Atan2((des.Y - source.Y),
                      (des.X - source.X)));
                return MathHelper.ToDegrees(angle);
            }
            public static float DistanceTo(Vector2 source, Vector2 des)
            {
                return ((float)Math.Sqrt(Math.Pow((des.X - source.X), 2)
                    + Math.Pow((des.Y - source.Y), 2)));
            }
        }
    
}
