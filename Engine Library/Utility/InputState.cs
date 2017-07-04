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
    public class InputState
    {
        public KeyboardState KS = new KeyboardState();
        KeyboardState oldKS = new KeyboardState();
        public MouseState MS = new MouseState();
        MouseState oldMS = new MouseState();

        Vector2 touchBeganPosition = Vector2.Zero;
        public Vector2 TouchBeganPosition { get { return touchBeganPosition; } }
        public Vector2 MousePosition { get { return new Vector2(MS.X, MS.Y); } }

        bool isClick = false;
        public bool IsClick { get { return isClick; } }

        public void Update()
        {
            oldKS = KS;
            KS = Keyboard.GetState();
            oldMS = MS;
            MS = Mouse.GetState();
            updateTouch();
            updateIsClick();
        }
        void updateIsClick()
        {
            if (isLeftMousePressed)
            {
                isClick = true;
            }
            else
            {
                if (isLeftMouseReleased)
                    isClick = false;
            }
        }

        void updateTouch()
        {
            if (isLeftMousePressed)
            {
                touchBeganPosition = new Vector2(MS.X, MS.Y);
            }
        }

        public bool isKeyDown(Keys what)
        {
            return KS.IsKeyDown(what);
        }
        public bool isKeyUp(Keys what)
        {
            return KS.IsKeyUp(what);
        }

        public bool isKeyPressed(Keys what)
        {
            return (KS.IsKeyDown(what) &&
               oldKS.IsKeyUp(what));
        }

        public bool isKeyReleased(Keys what)
        {
            return (oldKS.IsKeyDown(what) &&
               KS.IsKeyUp(what));
        }
        public bool isLeftMousePressed
        {
            get
            {
                return (MS.LeftButton == ButtonState.Pressed &&
                   oldMS.LeftButton == ButtonState.Released);
            }
        }

        public bool isLeftMouseReleased
        {
            get
            {
                return (MS.LeftButton == ButtonState.Released &&
                    oldMS.LeftButton == ButtonState.Pressed);
            }
        }

        public bool isRightMousePressed
        {
            get
            {
                return (MS.RightButton == ButtonState.Pressed &&
                   oldMS.RightButton == ButtonState.Released);
            }
        }

        public bool isRightMouseReleased
        {
            get
            {
                return (MS.RightButton == ButtonState.Released &&
                   oldMS.RightButton == ButtonState.Pressed);
            }
        }
    }
}
