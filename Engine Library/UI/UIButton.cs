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
    public class UIButton : Old_Node
    {
        #region Model
        bool isRollOver = false;
        public bool IsRollOver { get { return isRollOver; } }
        bool isClick = false;
        public bool IsClick { get { return isClick; } }
        bool isOn = false;
        bool isEnable = true;
        Rectangle hitArea;
        string textLabel = "";
        public void SetTextLabel(string text)
        {
            textLabel = text;
        }
        public void SetHitArea(Rectangle _hitArea)
        {
            hitArea = new Rectangle(
                (int)(_hitArea.X - _hitArea.Width * 0.5f),
                (int)(_hitArea.Y - _hitArea.Height * 0.5f),
                _hitArea.Width, _hitArea.Height);
            position = new Vector2(_hitArea.X, _hitArea.Y);
        }

        #endregion

        #region Initialize
        public UIButton(RootLayer _parentLayer, Rectangle _hitArea)
            : base(_parentLayer)
        {
            SetHitArea(_hitArea);
        }

        #endregion

        #region Controller
        public override void HandleInput(GameTime gt)
        {
            base.HandleInput(gt);
            if (isEnable)
            {
                InputState input = AIW.OS.InputState;
                if (hitArea.Contains((int)input.MS.X, (int)input.MS.Y))
                {
                    isRollOver = true;
                    if (input.isLeftMousePressed) isClick = true;
                }
                else isRollOver = false;

                if (input.isLeftMouseReleased)
                {
                    if (isClick && hitArea.Contains((int)input.MS.X, (int)input.MS.Y))
                        isOn = true;

                    isClick = false;
                }
            }
        }

        public bool ReceiveValue()
        {
            if (isOn)
            {
                isOn = false;
                return true;
            }
            return false;
        }

        #endregion

        #region View
        void ButtonBG()
        {
            Graphic2DEngine g = AIW.OS.Graphic2DEngine;
            int borderWidth = 2;

            if (isClick)
            {
                g.Old_Rectangle(position, hitArea.Width, hitArea.Height,
               0, ColorPalette.BlueWP7, 1, true, AIW.z);
            }

            g.Old_Label(kFont.Font, textLabel, position, 0, 1, ColorPalette.BlueWP7, 1, AIW.z);
            g.Old_Rectangle(position, hitArea.Width, hitArea.Height,
                0, Color.White, 1, true, AIW.z);
            //border
            g.Old_Rectangle(position,
                hitArea.Width + 2 * borderWidth, hitArea.Height + 2 * borderWidth,
               0, ColorPalette.BlueWP7, 1, true, AIW.z);
        }

        public override void Draw(GameTime gt)
        {
            base.Draw(gt);
            ButtonBG();
        }
        #endregion
    }
}
