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
        public class Timer
        {
            #region Model
            TimeSpan duration;
            bool direction = true;
            public bool Direction { get { return direction; } }
            public TimeSpan Duration
            {
                get
                {
                    return duration;
                }
                set
                {
                    duration = value;
                }
            }

            float totalTime = 0.0f;

            public void ResetTimer(TimeSpan _duration)
            {
                duration = _duration;
                totalTime = 0;
                direction = true;
            }
            public void ResetTimer(double _duration)
            {

                duration = TimeSpan.FromSeconds(_duration);
                totalTime = 0;
                direction = true;
            }
            public float TimerPosition
            {
                get
                {
                    float timerPosition = MathHelper.Clamp(
                     totalTime / (float)duration.TotalMilliseconds, 0.0f, 1.0f);
                    return timerPosition;
                }
            }
            public float TimerPositionInvert
            {
                get { return 1 - TimerPosition; }
            }
            public float TimerPositionLoop
            {
                get
                {
                    if (direction)
                    {
                        if (totalTime != 0)
                            return TimerPosition;
                        else return 0;
                    }
                    else
                    {
                        if (totalTime != 0)
                            return TimerPositionInvert;
                        else return 1;
                    }

                }
            }
            #endregion

            #region Initialize
            public Timer(TimeSpan _duration)
            {
                duration = _duration;
            }
            public Timer(double _duration)
            {
                duration = TimeSpan.FromSeconds(_duration);
            }
            #endregion

            #region Controller
            public bool Update(GameTime gt)
            {
                totalTime += (float)(gt.ElapsedGameTime.TotalMilliseconds);
                if (totalTime >= duration.TotalMilliseconds)
                {
                    totalTime = 0;
                    direction = !direction;
                    return true;
                }
                return false;
            }
            #endregion
        }
    

}
