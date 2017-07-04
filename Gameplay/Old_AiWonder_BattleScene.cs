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
    public class Old_AiWonder_BattleScene : Old_Scene
    {
        //Constant
       const int handSize = 3;
       const int pileSize = 5;
       const int space = 70;
       const int cardWidth = 60;
       const int cardHeight = 90;

        public Old_AiWonder_BattleScene()
            : base(new GameplayLayer())
        {
        }

        public class Card
        {
            public static int[] atkArray = { 1, 2, 3, 0, 0, 0 };
            public static int[] defArray = { 0, 0, 0, 1, 2, 3 };
            public static int[] costArray = { 1, 2, 3, 1, 2, 3 };
                            
            public static int ATK(int id)
            {
                if (id < atkArray.Length) return atkArray[id];                
                    return -1;
            }
            public static int DEF(int id)
            {
                if (id < defArray.Length) return defArray[id];
                return -1;
            }
            public static int COST(int id)
            {
                if (id < costArray.Length) return costArray[id];
                return -1;
            }
        }

        public class GameplayLayer : RootLayer
        {
            public abstract class CardViewNode : Old_Node
            {
                public CardViewNode(RootLayer parentLayer) : base(parentLayer) { }
                public void displayCard(int cardID,Vector2 pos)
                {
                    if (cardID < 0) return;
                    Graphic2DEngine drawEngine = AIW.OS.Graphic2DEngine;
                    drawEngine.Old_Rectangle(pos, cardWidth, cardHeight, 0, Color.Gray, 0.5f, true, AIW.z);
                    drawEngine.Old_Label(kFont.Font, "Card ID " + cardID, pos + new Vector2(-30, -40),
                       0, 1, Color.White, 1, AIW.z);
                    drawEngine.Old_Label(kFont.Font, "ATK " + Card.ATK(cardID), pos + new Vector2(-30, -30),
                        0, 1, Color.White, 1, AIW.z);
                    drawEngine.Old_Label(kFont.Font, "DEF " + Card.DEF(cardID), pos + new Vector2(-30, -20),
                        0, 1, Color.White, 1, AIW.z);
                    drawEngine.Old_Label(kFont.Font, "COST " + Card.COST(cardID), pos + new Vector2(-30, -10),
                        0, 1, Color.White, 1, AIW.z);
                }
            }

            public class CombinationPileVN : CardViewNode
            {
                const int maxCardInPile = 5;
                int[] cardList = new int[maxCardInPile];
                public CombinationPileVN(RootLayer parentLayer):base(parentLayer)
                {
                    for (int i = 0; i < cardList.Length; ++i)
                    {
                        cardList[i] = AIW.arc4random(0)%6;
                    }
                }

                public void setCard(int index, int cardID)
                {
                    if (index < 0 || index >= cardList.Length) return;
                    cardList[index] = cardID;
                }

                public override void Draw(GameTime gt)
                {                 
                    for(int i=0;i<cardList.Length;++i)
                    {                       
                        displayCard(cardList[i],
                            new Vector2(AIW.ScreenWidthOver2+(i-cardList.Length/2)*space,AIW.ScreenHeightOver2));
                    }
                    base.Draw(gt);
                }
            }

            public class PlayerHandVN : CardViewNode
            {
                const int maxCardInHand = 3;
                int[] cardList = new int[maxCardInHand];
                public PlayerHandVN(RootLayer parent):base(parent)
                {
                    for (int i = 0; i < cardList.Length; ++i)
                    {
                        cardList[i] = 0;
                    }
                }
                public void setCard(int index, int cardID)
                {
                    if (index < 0 || index >= cardList.Length) return;
                    cardList[index] = cardID;
                }
                public override void Draw(GameTime gt)
                {
                    for (int i = 0; i < cardList.Length; ++i)
                    {
                        displayCard(cardList[i],
                            new Vector2(AIW.ScreenWidthOver2 + (i - cardList.Length / 2) * space, AIW.ScreenHeight*0.9f));
                    }
                    base.Draw(gt);
                }
            }
            public class AIHandVN : CardViewNode
            {
                const int maxCardInHand = 3;
                int[] cardList = new int[maxCardInHand];
                public AIHandVN(RootLayer parent)
                    : base(parent)
                {
                    for (int i = 0; i < cardList.Length; ++i)
                    {
                        cardList[i] = 0;
                    }
                }
                public void setCard(int index, int cardID)
                {
                    if (index < 0 || index >= cardList.Length) return;
                    cardList[index] = cardID;
                }
                public override void Draw(GameTime gt)
                {
                    for (int i = 0; i < cardList.Length; ++i)
                    {
                        displayCard(cardList[i],
                            new Vector2(AIW.ScreenWidthOver2 + (i - cardList.Length / 2) * space, AIW.ScreenHeight * 0.1f));
                    }
                    base.Draw(gt);
                }
            }
            public class DropPileArea : Old_Node
            {
                const int size = 100;
                string tag_ = "";
                public DropPileArea(RootLayer parent,Vector2 pos,string tag)
                    : base(parent)
                {
                    position = pos;
                    tag_ = tag;
                }
                public bool checkIfContainPoint(Vector2 point)
                {
                    Rectangle rect = new Rectangle(
                        (int)(position.X - size / 2),
                        (int)(position.Y - size / 2),
                        size, size);
                    return rect.Contains((int)point.X, (int)point.Y);

                }
                public override void Draw(GameTime gt)
                {
                    base.Draw(gt);
                    Graphic2DEngine G = AIW.OS.Graphic2DEngine;
                    G.Old_Rectangle(position, size, size, 0, Color.Blue, 1, true, AIW.z);

                }
            }

            /// <summary>
            /// Field
            /// </summary>
            //Engine3DController engine3DController;
            CombinationPileVN combinationPile;
            PlayerHandVN playerHand;
            AIHandVN aiHand;

            UIButton[] PlayerHandBTN = new UIButton[handSize];
            UIButton[] AIHandBTN = new UIButton[handSize];
            UIButton[] pileBTN = new UIButton[pileSize];

            DropPileArea playerManapool;
            DropPileArea AIManapool;
            DropPileArea playerBattlepool;
            DropPileArea AIBattlepool;


            int cardInPlayerHandSelectedIndex = -1;
            int cardInAIHandSelectedIndex = -1;
            int cardInPileSelectedByAIIndex = -1;
            int cardInPileSelectedByPlayerIndex = -1;



            /// <summary>
            /// 
            /// </summary>

            public override void LoadContent()
            {
                base.LoadContent();
                

                //engine3DController = new Engine3DController(AIW.OS.graphic3DEngine);
            }
            public override void  initNodes()
            {
 	             base.initNodes();
                 combinationPile = new CombinationPileVN(this);
                 playerHand = new PlayerHandVN(this);
                 aiHand = new AIHandVN(this);

                 playerManapool = new DropPileArea(this, AIW.ScreenPosByRatio(0.7f, 0.7f), "playerMana");
                 AIManapool = new DropPileArea(this, AIW.ScreenPosByRatio(0.3f, 0.3f), "AIMana");
                 playerBattlepool = new DropPileArea(this, AIW.ScreenPosByRatio(0.8f, 0.7f), "playerBattle");
                 AIBattlepool = new DropPileArea(this, AIW.ScreenPosByRatio(0.2f, 0.3f), "AIBattle");

                 AddNode("CombinationPileVN", combinationPile);
                 AddNode("PlayerHandVN", playerHand);
                 AddNode("AIHand", aiHand);

                 
                 for (int i = 0; i < PlayerHandBTN.Length; ++i)
                 {
                     PlayerHandBTN[i] = new UIButton(this,
                         new Rectangle(AIW.ScreenWidthOver2 + (i - PlayerHandBTN.Length / 2) * space ,
                            (int)(AIW.ScreenHeight * 0.9f) ,
                             cardWidth,
                             cardHeight));
                     PlayerHandBTN[i].isVisible = false;
                     AddNode("PlayerHandSelectBTN" + i, PlayerHandBTN[i]);
                 }
                 for (int i = 0; i < AIHandBTN.Length; ++i)
                 {
                     AIHandBTN[i] = new UIButton(this,
                         new Rectangle(AIW.ScreenWidthOver2 + (i - AIHandBTN.Length / 2) * space,
                            (int)(AIW.ScreenHeight * 0.1f),
                             cardWidth,
                             cardHeight));
                     AIHandBTN[i].isVisible = false;
                     AddNode("AIHandSelectBTN" + i, AIHandBTN[i]);
                 }
                 for (int i = 0; i < pileBTN.Length; ++i)
                 {
                     pileBTN[i] = new UIButton(this,
                         new Rectangle( AIW.ScreenWidthOver2+(i-pileBTN.Length/2)*space,
                             AIW.ScreenHeightOver2,
                              cardWidth,
                             cardHeight));
                     pileBTN[i].isVisible = false;
                     AddNode("pileBTN" + i, pileBTN[i]);
                    
                 }
            }

            bool isDragCard = false;
            public override void HandleInput(GameTime gt)
            {
                base.HandleInput(gt);
             
                //engine3DController.handleInputToWalk();
                if (AIW.Input.isKeyPressed(Keys.D1))
                {
                    AIW.changeScene("FieldMapScene");
                }
                for (int i = 0; i < PlayerHandBTN.Length; ++i)
                {
                    if (PlayerHandBTN[i].ReceiveValue())
                    {
                        cardInPlayerHandSelectedIndex = i;
                    }
                }
                for (int i = 0; i < AIHandBTN.Length; ++i)
                {
                    if (AIHandBTN[i].ReceiveValue())
                    {
                        cardInAIHandSelectedIndex = i;
                    }
                }
                if (AIW.Input.isRightMousePressed)
                {
                     cardInPlayerHandSelectedIndex = -1;
                     cardInAIHandSelectedIndex = -1;
                     cardInPileSelectedByAIIndex = -1;
                     cardInPileSelectedByPlayerIndex = -1;
                }

                for (int i = 0; i < pileBTN.Length; ++i)
                {
                    if (!pileBTN[i].ReceiveValue()) continue;
                    if (AIW.Input.isKeyDown(Keys.D2))
                    {
                        cardInPileSelectedByAIIndex = i;
                    }
                    else
                    {
                        cardInPileSelectedByPlayerIndex = i;
                    }
                }

                if (AIW.Input.isLeftMouseReleased)
                {
                    if (isDragCard)
                    {
                        //check all drop card pile
                        //if there is card dragged and drop on pile,
                        //do action
                    }
                }
            }

            void displayBG()
            {
                AIW.OS.Graphic2DEngine.Old_Rectangle(
                    Vector2.Zero, AIW.ScreenWidth, AIW.ScreenHeight, 0, Color.Black, 0.75f, false, AIW.z);
            }
            void displaySelectCardInfo()
            {
                AIW.OS.Graphic2DEngine.Old_Label(kFont.Font, "Player Select Hand : " + cardInPlayerHandSelectedIndex,
                    new Vector2(10, 10), 0, 1, Color.White, 1, AIW.z);
                AIW.OS.Graphic2DEngine.Old_Label(kFont.Font, "AI Select Hand : " + cardInAIHandSelectedIndex,
                  new Vector2(10, 20), 0, 1, Color.White, 1, AIW.z);
                AIW.OS.Graphic2DEngine.Old_Label(kFont.Font, "Player select pile : " + cardInPileSelectedByPlayerIndex,
                  new Vector2(10, 30), 0, 1, Color.White, 1, AIW.z);
                AIW.OS.Graphic2DEngine.Old_Label(kFont.Font, "AI select pile : " + cardInPileSelectedByAIIndex,
                  new Vector2(10, 40), 0, 1, Color.White, 1, AIW.z);
            }
            public override void Draw(GameTime gt)
            {
                base.Draw(gt);
                displaySelectCardInfo();
                displayBG();               
            }



           
        }
    }
}
