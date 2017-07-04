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
    public class FontStock
    {
        ContentManager content;
        public SpriteFont Font;
        public SpriteFont Gamefont;
        public SpriteFont Menufont;
        public SpriteFont MyFont1;
        public SpriteFont HeadFont;
        public SpriteFont TitleFont;
        public SpriteFont ContextFont;

        public FontStock()
        {
            if (content == null)
                content = new ContentManager(AIW.OS.Game.Services, "Content");
            initFonts(content);
        }
        void initFonts(ContentManager content)
        {
            Font = content.Load<SpriteFont>("Font\\FreesiaUPC");
            Gamefont = content.Load<SpriteFont>("Font\\gamefont");
            Menufont = content.Load<SpriteFont>("Font\\menufont");
            MyFont1 = content.Load<SpriteFont>("Font\\myFont1");
            HeadFont = content.Load<SpriteFont>("Font\\headFont");
            TitleFont = content.Load<SpriteFont>("Font\\titleFont");
            ContextFont = content.Load<SpriteFont>("Font\\ContextFont");
        }
    }

    public class Texture2DModel
    {
        Texture2DStock texture2DStock;
        public Texture2DModel(Texture2DStock _texture2DStock)
        {
            texture2DStock = _texture2DStock;
        }
        public void LoadTexture2D()
        {
            //Default texture
            texture2DStock.AddTexture2D("defaultTexture", "blank");
            texture2DStock.AddTexture2D("defaultTexture", "blankBlack");
            texture2DStock.AddTexture2D("defaultTexture", "blackAlpha50");
            texture2DStock.AddTexture2D("defaultTexture", "circleTexture");

            texture2DStock.AddTexture2D("particleTexture", "circleTestGlowBlur3");
            texture2DStock.AddTexture2D("particleTexture", "lightningEffect0");
            texture2DStock.AddTexture2D("particleTexture", "lightningEffect1");
            texture2DStock.AddTexture2D("particleTexture", "lightningEffect3");
            texture2DStock.AddTexture2D("particleTexture", "particleWink2");
            texture2DStock.AddTexture2D("particleTexture", "starParticleGlow");
            texture2DStock.AddTexture2D("particleTexture", "starWink1");

            texture2DStock.AddTexture2D("uiTexture", "mouseCursor");
            texture2DStock.AddTexture2D("uiTexture", "mouseCursorWink");
            texture2DStock.AddTexture2D("uiTexture", "squareBtn1");
            texture2DStock.AddTexture2D("uiTexture", "squareBtnGlow1");
            texture2DStock.AddTexture2D("moonBG");


            //Add your own Texture2D Here
            texture2DStock.AddTexture2D("profileNui2");
            texture2DStock.AddTexture2D("profileAnn2");
            texture2DStock.AddTexture2D("mockCard");
            texture2DStock.AddTexture2D("mockBG");
            texture2DStock.AddTexture2D("radar");
            texture2DStock.AddTexture2D("handCursor");
            texture2DStock.AddTexture2D("hpGear1");
            texture2DStock.AddTexture2D("hpGear2");
            texture2DStock.AddTexture2D("hpGear3");
            texture2DStock.AddTexture2D("hpTube");
            texture2DStock.AddTexture2D("commandLayerBG");
            texture2DStock.AddTexture2D("cardBtn");
            texture2DStock.AddTexture2D("battleLabel");

            texture2DStock.AddTexture2D("fire");
            texture2DStock.AddTexture2D("ice");
            texture2DStock.AddTexture2D("lightning");
            texture2DStock.AddTexture2D("atkIcon");
            texture2DStock.AddTexture2D("cardBorder");
            texture2DStock.AddTexture2D("defIcon");

            texture2DStock.AddTexture2D("s_fire");
            texture2DStock.AddTexture2D("s_ice");
            texture2DStock.AddTexture2D("s_lightning");
            texture2DStock.AddTexture2D("s_atkIcon");
            texture2DStock.AddTexture2D("s_cardBorder");
            texture2DStock.AddTexture2D("s_defIcon");

            texture2DStock.AddTexture2D("battle01");
            texture2DStock.AddTexture2D("battle02");
            texture2DStock.AddTexture2D("battle03");

            texture2DStock.AddTexture2D("bin01");
            texture2DStock.AddTexture2D("bin02");
            texture2DStock.AddTexture2D("bin03");

            texture2DStock.AddTexture2D("cancel_button");
            texture2DStock.AddTexture2D("end_button");

            texture2DStock.AddTexture2D("mana01");
            texture2DStock.AddTexture2D("mana02");
            texture2DStock.AddTexture2D("mana03");

            texture2DStock.AddTexture2D("s_heal");
            texture2DStock.AddTexture2D("s_poison");
            texture2DStock.AddTexture2D("s_def");
            texture2DStock.AddTexture2D("s_back");

            texture2DStock.AddTexture2D("disclaimer");

            texture2DStock.AddTexture2D("Market");
            texture2DStock.AddTexture2D("Library");
            texture2DStock.AddTexture2D("Town");
            texture2DStock.AddTexture2D("Townhall");
            texture2DStock.AddTexture2D("Residence");
            texture2DStock.AddTexture2D("Bedroom");
        }
    }
}
