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
    public class BattleScene : Scene
    {
        public delegate void Startdelegate();
        public enum Phase
        {
            Begin,
            Draw,
            MainPlayer1,
            MainPlayer2,
            BattleDamageP1,
            BattleDamageP2,
            BattleHeal,
            BattleBerserk,
            BattlePoison,
            Cleanup,
            Idle,
        }
        public enum CursorAt
        {
            None,
            Hand,
            Market,
        }
      
        public class CardSprite : SpriteObj
        {
            public CardSprite retain()
            {
                CardSprite obj = new CardSprite(type_, value_, cost_,element_);
                return obj;
            }

            int value_ = 0;
            int cost_ = 0;
            CardBattleModel.CardType type_ = CardBattleModel.CardType.NullCard;
            CardBattleModel.Element element_ = CardBattleModel.Element.fire;

            TextObj valueLabel;
            TextObj costLabel;
            TextObj typeLabel;

            public CardSprite(CardBattleModel.CardType type, int value, int cost,CardBattleModel.Element element)
                : base("s_fire")
            {

                element_ = element;
                value_ = value;
                type_ = type;
                cost_ = cost;

                SpriteObj border = new SpriteObj("s_cardBorder");
                addChild(border);
                border.setZOrder(0);

             
                if (type == CardBattleModel.CardType.Attack)
                {
                    SpriteObj icon = new SpriteObj("s_atkIcon");
                    addChild(icon);
                    
                    setFilename("s_"+element_.ToString());
          
                   
                }
                if (type == CardBattleModel.CardType.Defence)
                {
                    SpriteObj icon = new SpriteObj("s_defIcon");
                    addChild(icon);
                    setFilename("s_def");
                }
                if (type == CardBattleModel.CardType.Heal)
                {
                    setFilename("s_heal");
                }



                valueLabel = new TextObj("" + value_);
                valueLabel.color_ = Color.White;
                costLabel = new TextObj("" + cost_);
                typeLabel = new TextObj("" + type_.ToString());

                valueLabel.setScale(2);
                valueLabel.color_ = Color.Black;
                costLabel.color_ = Color.Black;
                costLabel.setScale(2);
                typeLabel.setScale(2);
                    valueLabel.setPosition(-40,-78);
                    typeLabel.setPosition(-28, -52);
                    costLabel.setPosition(25, 45);
                addChild("valueLabel", valueLabel);
                addChild("costLabel", costLabel);
                setZOrder(-2);
                border.setZOrder(0);
                valueLabel.setZOrder(1);
                costLabel.setZOrder(1);
                //addChild("typeLabel", typeLabel);
            }


        }

        public class CommandButton : SpriteObj
        {
            Timer timer = new Timer(0.5);
            int index = 1;
            string name_;
            public CommandButton(string name)
                : base(name + "0" + 1)
            {
                name_ = name;
            }
            public override void update(GameTime gt)
            {
                base.update(gt);
                if(timer.Update(gt))
                {
                    if (index < 3)
                    {
                        index++;
                    }
                    else
                    {
                        index = 1;
                    }
                    setFilename(name_ + "0" + index);
                }
            }
        }

        public class HPLabelLayer : Layer
        {
            BattleFieldModel battleFieldModel;

            RectangleObj hp1Rect;
            RectangleObj hp2Rect;

            TextObj p1HpLabel;
            TextObj p2HpLabel;
            public HPLabelLayer()
                : base()
            {
                battleFieldModel = BattleFieldModel.sharedBattleFieldModel;
                initHpLabel();
                initHpBar1();
                initHpBar2();
                visible_ = false;
            }
            public override void update(GameTime gt)
            {
                base.update(gt);
                if (AIW.Input.isKeyPressed(Keys.D5))
                {
                    visible_ = true;
                }
                if (AIW.Input.isKeyPressed(Keys.D6))
                {
                    visible_ = false;
                }
            }

            public void updateHpLabel()
            {
                string text1 = "hp " + battleFieldModel.player1.hp + "/" + battleFieldModel.player1.maxHp;
                string text2 = "hp " + battleFieldModel.player2.hp + "/" + battleFieldModel.player2.maxHp;
                p1HpLabel.setText(text1);
                p2HpLabel.setText(text2);
            }

            void initHpLabel()
            {
                TextObj hpP1 = new TextObj("N/A");
                hpP1.setPosition(AIW.ScreenPosByRatio(0.83, 0.94));
                addChild(hpP1);
                hpP1.setZOrder(3);
                hpP1.setScale(3);
                p1HpLabel = hpP1;

                TextObj hpP2 = new TextObj("N/A");
                hpP2.setPosition(AIW.ScreenPosByRatio(0.83, 0.22));
                addChild(hpP2);
                hpP2.setZOrder(3);
                hpP2.setScale(3);
                p2HpLabel = hpP2;
            }

            public void updateHpBar()
            {
                float scale1 = battleFieldModel.player1.hp / (float)battleFieldModel.player1.maxHp;
                float scale2 = battleFieldModel.player2.hp / (float)battleFieldModel.player2.maxHp;
                hp1Rect.setScale(new Vector2(scale1, 1));
                hp2Rect.setScale(new Vector2(scale2, 1));
            }
            void initHpBar1()
            {
                SpriteObj obj = new SpriteObj("hpGear1");
                obj.setPosition(AIW.ScreenPosByRatio(0.85, 0.93));
                obj.setZOrder(1);
                addChild(obj);



                SpriteObj obj2 = new SpriteObj("hpTube");
                obj2.setPosition(obj.position + AIW.ScreenPosByRatio(0.01, 0));
                obj2.setZOrder(3);
                addChild(obj2);

                RectangleObj rect = new RectangleObj(
                   120,
                   10);
                rect.color_ = Color.GreenYellow;
                rect.anchorPoint_ = new Vector2(0, 0.5f);
                rect.setPosition(
                    new Vector2(858, 716));
                rect.setZOrder(2);
                addChild(rect);
                hp1Rect = rect;

            }
            void initHpBar2()
            {
                SpriteObj obj = new SpriteObj("hpGear3");
                obj.setPosition(AIW.ScreenPosByRatio(0.85, 0.21));
                obj.setZOrder(1);
                addChild(obj);

                SpriteObj obj2 = new SpriteObj("hpTube");
                obj2.setPosition(obj.position + AIW.ScreenPosByRatio(0.01, 0));
                obj2.setZOrder(3);
                addChild(obj2);

                RectangleObj rect = new RectangleObj(
                    120,
                    10);
                rect.color_ = Color.GreenYellow;
                rect.anchorPoint_ = new Vector2(0, 0.5f);
                rect.setPosition(
                    new Vector2(858, 164));
                rect.setZOrder(2);
                addChild(rect);
                hp2Rect = rect;
            }
        }

        public class MainLayer : Layer
        {
            Engine3DController engine3DController;
            BattleScene battleSceneDelegate;
            HPLabelLayer hpLabelLayerDelegate;
            public void setHPLabelLayerDelegate(HPLabelLayer delegate_)
            {
                hpLabelLayerDelegate = delegate_;
            }
            int whatPlayerTurn = 1;
            Phase phase = Phase.Begin;
            Phase phaseStacked = Phase.Idle;

            CardBattleModel model;
            BattleFieldModel battleFieldModel;

            Vector2[] marketCardPos;
            Vector2[] p1HandCardPos;
            Vector2[] p2HandCardPos;
            Vector2[] p1BattlePilePos;
            Vector2[] p2BattlePilePos;
            List<CardSprite> marketCards = new List<CardSprite>();
            List<CardSprite> p1HandCards = new List<CardSprite>();
            List<CardSprite> p2HandCards = new List<CardSprite>();
            List<SpriteObj> p1BattlePile = new List<SpriteObj>();
            List<SpriteObj> p2BattlePile = new List<SpriteObj>();

            SpriteObj handCursor;
            TextObj p1DeckLabel;
            TextObj p2DeckLabel;
            TextObj p1ManaLabel;
            TextObj p2ManaLabel;
            //bool receiveInput = false;
         

            public MainLayer(BattleScene delegate_)
            {
                battleSceneDelegate = delegate_;
                model = CardBattleModel.sharedCardBattleModel;
                battleFieldModel = BattleFieldModel.sharedBattleFieldModel;
                //initMockBG();
                
                initP1Deck();
                initP2Deck();
                initP1Radar();
                initP2Radar();
              
                initHandCursor();
                initCardPosition();
             
                updateAll();

                visible_ = false;
                

                if (AIW.OS.isUse3DEngine) {                   
                    init3DEngineController();
                    //engine3DController.jumpToBattle();
                }
              
            }
            void init3DEngineController()
            {
                engine3DController = new Engine3DController(AIW.OS.graphic3DEngine);
            }
            void event3DEngineControl()
            {
                if (AIW.Input.isKeyPressed(Keys.D7))
                {
                    engine3DController.skipShot1();
                }
                if (AIW.Input.isKeyPressed(Keys.D6))
                {
                    //engine3DController.skipBattle();
                    engine3DController.reset3DWorld();
                    visible_ = false;
                }
                if (AIW.Input.isKeyPressed(Keys.D5))
                {
                    engine3DController.jumpToBattle();
                    visible_ = true;
                }
                if (AIW.Input.isKeyPressed(Keys.D1))
                {
                   
                }
                
                engine3DController.handleInputToWalk();
            }

            void updateAll()
            {
                clearDrawCardParticleArray();
                updateMarketCard();
                updateP1HandCard();
                updateP2HandCard();
                updateP1BattlePileCard();
                updateP2BattlePileCard();
                refreshHandCursor();
            }

          

            void initMockBG()
            {
                SpriteObj obj = new SpriteObj("mockBG");
                obj.setPosition(AIW.ScreenCenter);
                obj.setAlpha(0.5f);
                addChild(obj);
            }

            CardSprite getCardPointedTo()
            {
                if (cursorAt == CursorAt.Hand)
                {
                    return p1HandCards[cursorIndex];
                }
                else if (cursorAt == CursorAt.Market)
                {
                    return marketCards[cursorIndex];
                }
                return new CardSprite(CardBattleModel.CardType.NullCard, 0, 0,CardBattleModel.Element.fire);
            }

            #region Cursor
            int cursorIndex = -1;
            int searchForNextMarketIndex(int index)
            {
                for (int i = 0; i < model.market.Length; ++i)
                {
                    if (model.market[i].countCard > 0)
                    {
                        if (i > index) return i;
                    }
                }
                return -1;
            }
            int searchForPreviousMarketIndex(int index)
            {
                for (int i = model.market.Length - 1; i >= 0; --i)
                {
                    if (model.market[i].countCard > 0)
                    {
                        if (i < index) return i;
                    }
                }
                return -1;
            }
            int getMarketIndex(int index)
            {
                List<int> indexFromMarket = new List<int>();
                for (int i = 0; i < model.market.Length; ++i)
                {
                    if (model.market[i].countCard > 0)
                    {
                        indexFromMarket.Add(i);
                    }
                }
                if (index < indexFromMarket.Count && index >= 0)
                {
                    return indexFromMarket[index];
                }
                return -1;
            }
            int getNumberOfMarketIndex()
            {
                List<int> indexFromMarket = new List<int>();
                for (int i = 0; i < model.market.Length; ++i)
                {
                    if (model.market[i].countCard > 0)
                    {
                        indexFromMarket.Add(i);
                    }
                }
                return indexFromMarket.Count - 1;
            }
            int getMinMarketIndex()
            {
                for (int i = 0; i < model.market.Length; ++i)
                {
                    if (model.market[i].countCard > 0)
                    {
                        return i;
                    }
                }
                return -1;
            }
            int getMinHandIndex()
            {
                for (int i = 0; i < model.player1.hand.countCard; ++i)
                {
                    // model.player1.hand[i];
                    return i;

                }
                return -1;
            }
            CursorAt cursorAt = CursorAt.None;

            void selectPreviousCardInHand()
            {
                int maxIndex = model.player1.hand.countCard-1;
                if (maxIndex == -1) return;

                if (cursorIndex - 1 < 0)
                {
                    cursorIndex = maxIndex;
                }
                else
                {
                    cursorIndex--;
                }
                    updateHandCursorPos();
            }
            void selectNextCardInHand()
            {
                int maxIndex = model.player1.hand.countCard - 1;
                if (maxIndex == -1) return;

                if (cursorIndex + 1 > maxIndex)
                {
                    cursorIndex = 0;
                }
                else
                {
                    cursorIndex++;
                }
                updateHandCursorPos();
            }
            void eventSelectCardInHand()
            {
                if (AIW.Input.isKeyPressed(Keys.Left))
                {
                    selectPreviousCardInHand();
                }
                if (AIW.Input.isKeyPressed(Keys.Right))
                {
                    selectNextCardInHand();
                }
                if (AIW.Input.isKeyPressed(Keys.Z))
                {
                    battleSceneDelegate.displayCommandLayer(getCardPointedTo().retain());                   
                }
            }
            void selectPreviousCardInMarket()
            {
                int maxIndex = getNumberOfMarketIndex();
                if (maxIndex == -1) return;

                if (searchForPreviousMarketIndex(cursorIndex) == -1)
                {
                    cursorIndex = getMarketIndex(getNumberOfMarketIndex());
                }
                else
                {
                    cursorIndex = searchForPreviousMarketIndex(cursorIndex);
                }
                updateHandCursorPos();
            }
            void selectNextCardInMarket()
            {
                int maxIndex = getNumberOfMarketIndex();
                if (maxIndex == -1) return;

                if (searchForNextMarketIndex(cursorIndex) == -1)
                {
                    cursorIndex = getMinMarketIndex();
                }
                else
                {
                    cursorIndex = searchForNextMarketIndex(cursorIndex);
                }
                updateHandCursorPos();
            }
            void eventSelectCardInMarket()
            {
                if (AIW.Input.isKeyPressed(Keys.Left))
                {
                    selectPreviousCardInMarket();
                }
                if (AIW.Input.isKeyPressed(Keys.Right))
                {
                    selectNextCardInMarket();
                }
                if (AIW.Input.isKeyPressed(Keys.Z))
                {
                    battleSceneDelegate.displayMarketLayer(getCardPointedTo().retain());                  
                }
            }
             

            bool isHasAnyCardInHand()
            {
                if (model.player1.hand.countCard > 0) return true;
                return false;
            }
            bool isHasAnyCardInMarket()
            {
                for (int i = 0; i < model.market.Length; ++i)
                {
                    if(model.market[i].countCard >0) return true;
                }
                return false;
            }
            void changeCursorAt()
            {
                if (cursorAt == CursorAt.Hand)
                {
                    if (isHasAnyCardInMarket())
                    {
                        cursorAt = CursorAt.Market;
                        cursorIndex = getMinMarketIndex();
                    }
                    
                }
                else if (cursorAt == CursorAt.Market)
                {
                    if (isHasAnyCardInHand())
                    {
                        cursorAt = CursorAt.Hand;
                        cursorIndex = getMinHandIndex();
                    }
                }
                updateHandCursorPos();
            }
            void eventChangeCursorAt()
            {
                if (!isHasAnyCardInHand() && !isHasAnyCardInMarket()) cursorAt = CursorAt.None;

                if (AIW.Input.isKeyPressed(Keys.Up) || AIW.Input.isKeyPressed(Keys.Down))
                {
                    changeCursorAt();
                }
            }
            void refreshHandCursor()
            {

                if (isHasAnyCardInHand())
                {
                    cursorAt = CursorAt.Hand;
                    int maxIndex = model.player1.hand.countCard - 1;
                    if (maxIndex < 0)
                    {
                        cursorIndex = -1;
                    }
                    else
                    {
                        cursorIndex = 0;
                    }
                }
                else if (isHasAnyCardInMarket())
                {
                    cursorAt = CursorAt.Market;
                    cursorIndex = 0;
                }
                else
                {
                    cursorAt = CursorAt.None;
                }
                updateHandCursorPos();
            }

            void updateHandCursorPos()
            {
                handCursor.visible_ = true ;
                if (cursorIndex == -1)
                {
                    handCursor.visible_ = false; return;
                }
                if (cursorAt == CursorAt.Hand)
                {
                   
                    if (cursorIndex >= 0 && cursorIndex < p1HandCardPos.Length)
                    {
                        handCursor.setPosition(p1HandCardPos[cursorIndex] + AIW.ScreenPosByRatio(-0.08f, 0));
                    }
                }
                else if (cursorAt == CursorAt.Market)
                {
                    if (cursorIndex >= 0 && cursorIndex < marketCardPos.Length)
                    {
                        handCursor.setPosition(marketCardPos[cursorIndex] + AIW.ScreenPosByRatio(-0.08f, 0));
                    }
                }
                else if (cursorAt == CursorAt.None)
                {
                    cursorIndex = -1;
                    handCursor.visible_ = false;
                }

            }

            //class HandCursor : SpriteObj
            //{
            //    public HandCursor()
            //        : base("mouseCursor")
            //    {

            //    }


            //}

            void initHandCursor()
            {
                SpriteObj obj = new SpriteObj("mouseCursor");
                obj.setPosition(AIW.ScreenPosByRatio(0.17,0.84));
                addChild(obj);
                obj.setZOrder(2);
                //obj.setScale(3);
                handCursor = obj;
                obj.visible_ = false;
                obj.setRotation(45+180-10);
            }

            #endregion
            void updateDeckLabel()
            {
                int deck1 = model.player1.deck.countCard-model.player1.battlePile.countCard;
                int deck2 = model.player2.deck.countCard - model.player2.battlePile.countCard;
                p1DeckLabel.setText("" + deck1);
                p2DeckLabel.setText("" + deck2);
            }
            void initP1Deck()
            {
                SpriteObj obj = new SpriteObj("mockCard");
                obj.setPosition(AIW.ScreenPosByRatio(0.07f,0.93f));
                obj.setRotation(90);
                obj.setScale(0.6f);
                addChild(obj);

                TextObj text = new TextObj("p1 Deck : 0");
                text.setPosition(obj.position-AIW.ScreenPosByRatio(0.01f,0.03f));
                addChild(text);
                text.setZOrder(1);
                text.setScale(3);
                p1DeckLabel = text;

            }
            void initP2Deck()
            {
                SpriteObj obj = new SpriteObj("mockCard");
                obj.setPosition(AIW.ScreenPosByRatio(0.19f, 0.09f));
                obj.setRotation(90);
                obj.setScale(0.6f);
                addChild(obj);

                TextObj text = new TextObj("p2 Deck : 0");
                text.setPosition(obj.position - AIW.ScreenPosByRatio(0.01f, 0.03f));
                addChild(text);
                text.setZOrder(1);
                text.setScale(3);
                p2DeckLabel = text;
            }
            void updateManaLabel()
            {
                int mana1 = model.player1.manapool;
                int mana2 = model.player2.manapool;
                p1ManaLabel.setText("" + mana1);
                p2ManaLabel.setText("" + mana2);
            }

            void initP1Radar()
            {
                SpriteObj obj = new SpriteObj("radar");
                obj.setPosition(AIW.ScreenPosByRatio(0.07f, 0.77f));
                obj.setRotation(90);
                obj.setScale(0.7f);
                addChild(obj);

                TextObj text = new TextObj("P1 mana");
                text.setPosition(obj.position - AIW.ScreenPosByRatio(0, 0.04f));
                addChild(text);
                text.setZOrder(1);
                text.setScale(3);
                p1ManaLabel = text;
            }
            void initP2Radar()
            {
                SpriteObj obj = new SpriteObj("radar");
                obj.setPosition(AIW.ScreenPosByRatio(0.07f, 0.09f));
                obj.setRotation(90);
                obj.setScale(0.7f);
                addChild(obj);

                TextObj text = new TextObj("P2 mana");
                text.setPosition(obj.position - AIW.ScreenPosByRatio(0, 0.04f));
                addChild(text);
                text.setZOrder(1);
                text.setScale(3);
                p2ManaLabel = text;
            }
          

            void updateP1HandCard()
            {
                foreach (CardSprite sprite in p1HandCards)
                {
                    sprite.removeFromParent();
                }
                p1HandCards.Clear();
                for (int i = 0; i < model.player1.hand.countCard; ++i)
                {
                    CardBattleModel.Card card = model.player1.hand.cards[i];
                    if (card.cardType == CardBattleModel.CardType.NullCard)
                    {
                        continue;
                    }
                    CardSprite sprite = new CardSprite(card.cardType, card.power, card.buyCost,card.element);
                    sprite.setPosition(p1HandCardPos[i]);
                    sprite.setScale(1.2f);
                    p1HandCards.Add(sprite);
                    addChild(sprite);
                }
            }
            void updateP2HandCard()
            {
                foreach (CardSprite sprite in p2HandCards)
                {
                    sprite.removeFromParent();
                }
                p2HandCards.Clear();
                for (int i = 0; i < model.player2.hand.countCard; ++i)
                {
                    CardBattleModel.Card card = model.player2.hand.cards[i];
                    if (card.cardType == CardBattleModel.CardType.NullCard)
                    {
                        continue;
                    }
                    CardSprite sprite = new CardSprite(card.cardType, card.power, card.buyCost,card.element);
                    sprite.setPosition(p2HandCardPos[i]);
                    sprite.setScale(0.75f);
                    p2HandCards.Add(sprite);
                    addChild(sprite);
                }
            }
            void updateP1BattlePileCard()
            {
                foreach (SpriteObj sprite in p1BattlePile)
                {
                    sprite.removeFromParent();
                }
                p1BattlePile.Clear();
                for (int i = 0; i < model.player1.battlePile.countCard; ++i)
                {
                    CardBattleModel.Card card = model.player1.battlePile.cards[i];
                    if (card.cardType == CardBattleModel.CardType.NullCard)
                    {
                        continue;
                    }
                    CardSprite sprite = new CardSprite(card.cardType, card.power, card.buyCost, card.element);
                    sprite.setPosition(p1BattlePilePos[i]);
                    sprite.setScale(0.75f);
                    p1BattlePile.Add(sprite);
                    addChild(sprite);
                }
            }
            void updateP2BattlePileCard()
            {
                foreach (SpriteObj sprite in p2BattlePile)
                {
                    sprite.removeFromParent();
                }
                p2BattlePile.Clear();
                for (int i = 0; i < model.player2.battlePile.countCard; ++i)
                {
                    CardBattleModel.Card card = model.player2.battlePile.cards[i];
                    if (card.cardType == CardBattleModel.CardType.NullCard)
                    {
                        continue;
                    }
                    SpriteObj sprite = new SpriteObj("mockCard");
                    sprite.setPosition(p2BattlePilePos[i]);
                    sprite.setScale(0.75f);
                    p2BattlePile.Add(sprite);
                    addChild(sprite);
                }
            }

            void updateMarketCard()
            {
                foreach (CardSprite sprite in marketCards)
                {
                    sprite.removeFromParent();
                }
                marketCards.Clear();

                 for (int i = 0; i < marketCardPos.Length; ++i)
                {
                    CardBattleModel.Card card = model.market[i].lookFromTop();
                    if (card.cardType == CardBattleModel.CardType.NullCard)
                    {
                        continue;
                    }

                    CardSprite sprite = new CardSprite(card.cardType, card.power, card.buyCost, card.element);
                    sprite.setPosition(marketCardPos[i]);
                     sprite.setScale(1.6f);
                    marketCards.Add(sprite);
                    addChild(sprite);
                }
            }

            void initCardPosition()
            {
                marketCardPos = new Vector2[model.marketSize];
                for (int i = 0; i < marketCardPos.Length; ++i)
                {
                    marketCardPos[i] = AIW.ScreenPosByRatio(0.5f + ((i - marketCardPos.Length/2) * 0.19f), 
                        0.45f+ ((i - marketCardPos.Length/2) * 0.03f));  
                }

                p1HandCardPos = new Vector2[model.handSize];
                for (int i = 0; i < p1HandCardPos.Length; ++i)
                {
                    p1HandCardPos[i] = AIW.ScreenPosByRatio(0.39f + ((i - p1HandCardPos.Length / 2) * 0.15f), 0.84f);
                }
                p2HandCardPos = new Vector2[model.handSize];
                for (int i = 0; i < p2HandCardPos.Length; ++i)
                {
                    p2HandCardPos[i] = AIW.ScreenPosByRatio(0.47f + ((i - p2HandCardPos.Length / 2) * 0.09f), 0.11f);
                }

                p1BattlePilePos = new Vector2[model.battlePileSize];
                for (int i = 0; i < p1BattlePilePos.Length; ++i)
                {
                    p1BattlePilePos[i] = AIW.ScreenPosByRatio(0.81f + ((i - p1BattlePilePos.Length / 2) * 0.09f), 0.86f);
                }
                p2BattlePilePos = new Vector2[model.battlePileSize];
                for (int i = 0; i < p2BattlePilePos.Length; ++i)
                {
                    p2BattlePilePos[i] = AIW.ScreenPosByRatio(0.81f + ((i - p2BattlePilePos.Length / 2) * 0.09f), 0.15f);
                }
            }


            void testModel()
            {
                

                if (AIW.Input.isKeyPressed(Keys.D8))
                {
                    model.drawAtBeginOfTurn(model.player1);
                    model.drawAtBeginOfTurn(model.player2);
                    logGameplay();
                }
                if (AIW.Input.isKeyPressed(Keys.D9))
                {
                    battleFieldModel.calculateDamageInBattle(
                        battleFieldModel.player1,
                        battleFieldModel.player2,
                        model.player1.battlePile,
                        model.player2.battlePile);
                    logGameplay();
                }
                if (AIW.Input.isKeyPressed(Keys.D0))
                {
                    model.cleanupForNewTurn(model.player1);
                    logGameplay();
                }
                if (AIW.Input.isKeyPressed(Keys.D1))
                {
                    model.buyFromMarket(model.player1, 0); logGameplay();
                }
                if (AIW.Input.isKeyPressed(Keys.D2))
                {
                    model.buyFromMarket(model.player1, 1); logGameplay();
                }
                if (AIW.Input.isKeyPressed(Keys.D3))
                {
                    model.buyFromMarket(model.player1, 2); logGameplay();
                }
                if (AIW.Input.isKeyPressed(Keys.D4))
                {
                    model.buyFromMarket(model.player1, 3); logGameplay();
                }
                if (AIW.Input.isKeyPressed(Keys.D5))
                {
                    model.buyFromMarket(model.player1, 4); logGameplay();
                }
                if (AIW.Input.isKeyPressed(Keys.Z))
                {
                    model.discardFromHand(model.player1, 0);
                    logGameplay();
                }
                if (AIW.Input.isKeyPressed(Keys.X))
                {
                    model.discardFromHand(model.player1, 1); logGameplay();
                }
                if (AIW.Input.isKeyPressed(Keys.C))
                {
                    model.discardFromHand(model.player1, 2); logGameplay();
                }
                if (AIW.Input.isKeyPressed(Keys.A))
                {
                    model.assignCardToBattle(model.player1, 0);
                    model.assignCardToBattle(model.player2, 0); logGameplay();
                }
                if (AIW.Input.isKeyPressed(Keys.S))
                {
                    model.assignCardToBattle(model.player1, 1); logGameplay();
                }
                if (AIW.Input.isKeyPressed(Keys.D))
                {
                    model.assignCardToBattle(model.player1, 2); logGameplay();
                }
                if (AIW.Input.isKeyPressed(Keys.Q))
                {
                    model.assignCardToManapool(model.player1, 0); logGameplay();
                }
                if (AIW.Input.isKeyPressed(Keys.W))
                {
                    model.assignCardToManapool(model.player1, 1); logGameplay();
                }
                if (AIW.Input.isKeyPressed(Keys.E))
                {
                    model.assignCardToManapool(model.player1, 2); logGameplay();
                }

            }
           
            public bool commandCardInHand(int commandIndex)
            {
                bool canUseCard = false;
                if (cursorAt != CursorAt.Hand) { return canUseCard; }
                if (commandIndex == 0)
                {
                    int mana = model.player1.hand.cards[cursorIndex].manaCost;
                    if (model.assignCardToManapool(model.player1, cursorIndex))
                    {
                        for (int i = 0; i < mana; ++i)
                        {
                            ManaGainEmitter emitter = new ManaGainEmitter(mana,
                                 p1HandCardPos[cursorIndex],
                                AIW.ScreenPosByRatio(0.07f, 0.77f));
                            addChild(emitter);
                        }
                        updateAll(); canUseCard = true;
                    }
                }
                else if (commandIndex == 1)
                {
                    if (model.assignCardToBattle(model.player1, cursorIndex))
                    {
                        updateAll(); canUseCard = true;
                    }
                }
                else if (commandIndex == 2)
                {
                    if (model.discardFromHand(model.player1, cursorIndex))
                    {
                        DiscardParticle particle = new DiscardParticle(this,
                          p1HandCardPos[cursorIndex], 0.5, 1.2f, 0);
                        addChild(particle); delayPhase(0.5); canUseCard = true;
                        p1HandCards[cursorIndex].removeFromParent();
                      
                    }
                }
               
                return canUseCard;
            }
            public bool commandCardInMarket(int commandIndex)
            {
                bool canBuyCard = false;
                if (cursorAt != CursorAt.Market) { return canBuyCard; }
                if (commandIndex == 0)
                {
                    if (model.buyFromMarket(model.player1, cursorIndex))
                    {
                        BuyCardParticle particle = new BuyCardParticle(marketCardPos[cursorIndex],
                            AIW.ScreenPosByRatio(0.07f, 0.93f), 1.6f, 0.75f);
                        particle.setZOrder(3);
                        addChild(particle);
                        updateAll();
                        canBuyCard = true;
                    }
                }     
                return canBuyCard;
            }
            public void commandEndPhase()
            {
                changePhase();
            }


          
            void simpleAIAction()
            {
                int cardInHand = model.player2.hand.countCard;
                for (int i = 0; i < cardInHand; ++i)
                {
                    model.assignCardToBattle(model.player2, 0);
                }
                updateAll();
              
               
            }
            void eventPlayerMainAction()
            {
                eventCursorPassPhase();
                eventChangeCursorAt();
                if (cursorAt == CursorAt.Hand)
                {
                    eventSelectCardInHand();
                }
                else if (cursorAt == CursorAt.Market)
                {
                    eventSelectCardInMarket();
                }
                else if (cursorAt == CursorAt.None)
                {

                }
            }

            void logPhase()
            {
                AIW.Log(this, "Player : "+whatPlayerTurn+ ",Phase : " + phase.ToString());
            }

            int countDrawCardAnim = 0;
            Timer drawCardAnimTimer = new Timer(0.5);
            List<Node> drawCardParticleArray = new List<Node>();
            void clearDrawCardParticleArray()
            {
                foreach (Node node in drawCardParticleArray)
                {
                    node.removeSelf();
                }
                drawCardParticleArray.Clear();
            }

            void delayPhase(double duration)
            {
                if (phase == Phase.Idle) { return; }
                phaseStacked = phase;
                phase = Phase.Idle;
                idlePhaseTimer.ResetTimer(duration);
            }
            Timer idlePhaseTimer = new Timer(1);
            void turnPhase(GameTime gt)
            {
                
                if (phase == Phase.Idle)
                {
                    if (idlePhaseTimer.Update(gt))
                    {
                        phase = phaseStacked;
                        phaseStacked = Phase.Idle;
                        updateAll();
                    }
                    return;
                }

                if (phase == Phase.Begin)
                {
                    changePhase();
                    countDrawCardAnim = 3;
                    drawCardAnimTimer.ResetTimer(0.5);
                }
                else if (phase == Phase.Draw)
                {
                    if (!drawCardAnimTimer.Update(gt)) { return; }
                    countDrawCardAnim--;
                    if (countDrawCardAnim >= 0)
                    {
                        DrawCardParticle particle = new DrawCardParticle(
                            AIW.ScreenPosByRatio(0.07f, 0.93f), 
                            p1HandCardPos[countDrawCardAnim]);
                        particle.setZOrder(-1);
                        particle.setScale(1.2f);
                        addChild(particle);
                        

                        DrawCardParticle particle2 = new DrawCardParticle(
                        AIW.ScreenPosByRatio(0.19f, 0.09f),
                          p2HandCardPos[countDrawCardAnim]);
                        particle2.setZOrder(-1);
                        particle2.setScale(0.75f);
                        addChild(particle2);

                        drawCardParticleArray.Add(particle);
                        drawCardParticleArray.Add(particle2);
                    }
                    else
                    {

                        model.drawAtBeginOfTurn(model.player1);
                        model.drawAtBeginOfTurn(model.player2);
                        updateAll();
                        refreshHandCursor();
                        changePhase();
                    }
                }
                else if (phase == Phase.MainPlayer1)
                {
                    eventPlayerMainAction();
                }
                else if (phase == Phase.MainPlayer2)
                {
                    simpleAIAction();
                    changePhase();
                }
                else if (phase == Phase.BattleDamageP1)
                {
                    /*
                    int oldp1Hp = battleFieldModel.player1.hp;
                    int oldp2Hp = battleFieldModel.player2.hp;
                    battleFieldModel.calculateDamageInBattle(
                       battleFieldModel.player1,
                       battleFieldModel.player2,
                       model.player1.battlePile,
                       model.player2.battlePile);
                    int diffP1Hp = battleFieldModel.player1.hp - oldp1Hp;
                    int diffP2Hp = battleFieldModel.player2.hp - oldp2Hp;

                    DamageParticle particle1 = new DamageParticle(diffP1Hp, AIW.ScreenPosByRatio(0.85, 0.93));
                    DamageParticle particle2 = new DamageParticle(diffP2Hp, AIW.ScreenPosByRatio(0.85, 0.21));
                    addChild(particle1);
                    addChild(particle2);

                    AIW.Log(this, "P1 HP: " + battleFieldModel.player1.hp + "/" + battleFieldModel.player1.maxHp);
                    AIW.Log(this, "P2 HP: " + battleFieldModel.player2.hp + "/" + battleFieldModel.player2.maxHp);
                     * */
                    changePhase();
                    delayPhase(8);
                    visible_ = false;
                    engine3DController.assignedBattleAnimation(Caster.Player01, Caster.Enemy01, Spell.Fire01_Effect);
                
                }
                else if (phase == Phase.BattleDamageP2)
                {
                 
                    changePhase();
                    delayPhase(8);
                    visible_ = false;
                    engine3DController.assignedBattleAnimation(Caster.Enemy01, Caster.Player01, Spell.Ice01_Effect);

                }
                else if (phase == Phase.BattleHeal)
                {
                 

                    changePhase();
                    delayPhase(2);

                }
               
                else if (phase == Phase.BattleBerserk)
                {
                    //int oldp1Hp = battleFieldModel.player1.hp;
                    //int oldp2Hp = battleFieldModel.player2.hp;
                    //battleFieldModel.calculateDamageInBattle(
                    //   battleFieldModel.player1,
                    //   battleFieldModel.player2,
                    //   model.player1.battlePile,
                    //   model.player2.battlePile);
                    //int diffP1Hp = battleFieldModel.player1.hp - oldp1Hp;
                    //int diffP2Hp = battleFieldModel.player2.hp - oldp2Hp;

                    //DamageParticle particle1 = new DamageParticle(diffP1Hp, AIW.ScreenPosByRatio(0.85, 0.93));
                    //DamageParticle particle2 = new DamageParticle(diffP2Hp, AIW.ScreenPosByRatio(0.85, 0.21));
                    //addChild(particle1);
                    //addChild(particle2);

                    //AIW.Log(this, "P1 HP: " + battleFieldModel.player1.hp + "/" + battleFieldModel.player1.maxHp);
                    //AIW.Log(this, "P2 HP: " + battleFieldModel.player2.hp + "/" + battleFieldModel.player2.maxHp);
                    changePhase();

                }
                else if (phase == Phase.BattlePoison)
                {
                    battleFieldModel.calculateHealInBattle(
                      battleFieldModel.player1,
                      battleFieldModel.player2,
                      model.player1.battlePile,
                      model.player2.battlePile);
                    int oldp1Hp = battleFieldModel.player1.hp;
                    int oldp2Hp = battleFieldModel.player2.hp;
                    battleFieldModel.calculateDamageInBattle(
                       battleFieldModel.player1,
                       battleFieldModel.player2,
                       model.player1.battlePile,
                       model.player2.battlePile);
                    int diffP1Hp = battleFieldModel.player1.hp - oldp1Hp;
                    int diffP2Hp = battleFieldModel.player2.hp - oldp2Hp;

                    DamageParticle particle1 = new DamageParticle(diffP1Hp, AIW.ScreenPosByRatio(0.85, 0.93));
                    DamageParticle particle2 = new DamageParticle(diffP2Hp, AIW.ScreenPosByRatio(0.85, 0.21));
                    addChild(particle1);
                    addChild(particle2);

                    AIW.Log(this, "P1 HP: " + battleFieldModel.player1.hp + "/" + battleFieldModel.player1.maxHp);
                    AIW.Log(this, "P2 HP: " + battleFieldModel.player2.hp + "/" + battleFieldModel.player2.maxHp);
                    changePhase();
                }
                else if (phase == Phase.Cleanup)
                {
                        model.cleanupForNewTurn(model.player1);
                        model.cleanupForNewTurn(model.player2);
                        changePhase();
                        updateAll();
                        visible_ = true;
                }

            }

            void eventCursorPassPhase()
            {
                if (AIW.Input.isKeyPressed(Keys.X))
                {
                    battleSceneDelegate.displayEndPhaseLayer();
                }
            }
            void changePhase()
            {
                if (whatPlayerTurn == 1)
                {
                    if (phase == Phase.Idle) { return; }

                    if (phase == Phase.Begin)
                    {
                        phase = Phase.Draw;
                    }
                    else
                        if (phase == Phase.Draw)
                        {
                                phase = Phase.MainPlayer1;

                        }
                        else
                            if (phase == Phase.MainPlayer1)
                            {
                                phase = Phase.MainPlayer2;

                            }
                        else
                            if (phase == Phase.MainPlayer2)
                            {
                                    phase = Phase.BattleDamageP1;
                                
                            }
                            else
                                if (phase == Phase.BattleDamageP1)
                                {
                                        phase = Phase.BattleDamageP2;
                                 
                                }
                                else
                                    if (phase == Phase.BattleDamageP2)
                                    {
                                        phase = Phase.BattleHeal;
                                    }
                                    else
                                            if (phase == Phase.BattleHeal)
                                            {
                                                phase = Phase.BattleBerserk;
                                            }
                                            else
                                                if (phase == Phase.BattleBerserk)
                                                {
                                                    phase = Phase.BattlePoison;
                                                }
                                                else
                                                    if (phase == Phase.BattlePoison)
                                                    {
                                                        phase = Phase.Cleanup;
                                                    }
                                                    else
                                                        if (phase == Phase.Cleanup)
                                                        {
                                                            phase = Phase.Begin;
                                                          
                                                                whatPlayerTurn = 2;
                                                        
                                                        }
                }
                else
                {
                    if (phase == Phase.Idle) { return; }

                    if (phase == Phase.Begin)
                    {
                        phase = Phase.Draw;
                    }
                    else
                        if (phase == Phase.Draw)
                        {
                            phase = Phase.MainPlayer2;

                        }
                        else
                            if (phase == Phase.MainPlayer2)
                            {
                                phase = Phase.MainPlayer1;

                            }
                            else
                                if (phase == Phase.MainPlayer1)
                                {
                                    phase = Phase.BattleDamageP2;

                                }
                                else
                                    if (phase == Phase.BattleDamageP2)
                                    {
                                        phase = Phase.BattleDamageP1;

                                    }
                                    else
                                        if (phase == Phase.BattleDamageP1)
                                        {
                                            phase = Phase.BattleHeal;
                                        }
                                        else
                                            if (phase == Phase.BattleHeal)
                                            {
                                                phase = Phase.BattleBerserk;
                                            }
                                            else
                                                if (phase == Phase.BattleBerserk)
                                                {
                                                    phase = Phase.BattlePoison;
                                                }
                                                else
                                                    if (phase == Phase.BattlePoison)
                                                    {
                                                        phase = Phase.Cleanup;
                                                    }
                                                    else
                                                        if (phase == Phase.Cleanup)
                                                        {
                                                            phase = Phase.Begin;

                                                            whatPlayerTurn = 1;

                                                        }
                }
               
             
                logPhase();
            }

            public override void update(GameTime gt)
            {
                base.update(gt);

                if (hpLabelLayerDelegate != null)
                {
                    hpLabelLayerDelegate.updateHpLabel();
                    hpLabelLayerDelegate.updateHpBar();
                }
                updateManaLabel();
                updateDeckLabel();
                turnPhase(gt);
                //testModel();
                //if (AIW.Input.isKeyPressed(Keys.Space))
                //{
                //    updateAll();
                //}
                
                if (AIW.OS.isUse3DEngine)
                {
                    event3DEngineControl();
                }
            }

            void logGameplay()
            {
                Console.Clear();
                Console.WriteLine("Player deck : " + model.player1.deck.countCard 
                    + ", " + "mana : " + model.player1.manapool);
                Console.WriteLine("Player BattlePile : " + model.player1.battlePile.countCard);
                Console.WriteLine("P1 Hp : " +battleFieldModel.player1.hp
                    +", P2 Hp : "+battleFieldModel.player2.hp);
                string marketInfo = "Market : [";
                for(int i=0;i<model.market.Length;++i)
                {
                    marketInfo += ""+model.market[i].countCard;
                    if(i == model.market.Length-1) marketInfo += "]";
                    else marketInfo += ",";
                }
                Console.WriteLine(marketInfo);
                Console.WriteLine("Player hand : " + model.player1.hand.countCard);
                int j = 0;
                foreach (CardBattleModel.Card card in model.player1.hand.cards)
                {
                    Console.WriteLine("" + (j + 1) + "#");
                    logCard(card);
                    ++j;
                }
            }

            void logCard(CardBattleModel.Card card)
            {
                Console.WriteLine("name " + card.name);
                Console.WriteLine("power : " + card.power);
                Console.WriteLine("Cost : " + card.manaCost);
                Console.WriteLine("_________________");
            }
        }

        #region CommandLayer
        public class CardCommandLayer : Layer
        {
            BattleScene battleSceneDelegate;
            MainLayer mainLayerDelegate;
            List<CommandButton> cardButtons = new List<CommandButton>();
            SpriteObj cursor;
            int currentSelectIndex = 1;
            const int maxIndex = 3;

            void updateCursorPosition()
            {
                cursor.setPosition(cardButtons[currentSelectIndex].position+new Vector2(-115,-18));
            }
            void selectNextIndex()
            {

                if (currentSelectIndex - 1 < 0)
                {
                    currentSelectIndex = maxIndex - 1;
                }
                else
                {
                    currentSelectIndex--;
                }
                updateCursorPosition();
            }
            void selectPreviousIndex()
            {

                if (currentSelectIndex + 1 >= maxIndex)
                {
                    currentSelectIndex = 0;
                }
                else
                {
                    currentSelectIndex++;
                }
                updateCursorPosition();
            }

            void eventSelectIndex()
            {
                if (AIW.Input.isKeyPressed(Keys.Up))
                {
                    selectPreviousIndex();
                }
                if (AIW.Input.isKeyPressed(Keys.Down))
                {
                    selectNextIndex();
                }
            }
            public override void update(GameTime gt)
            {
                base.update(gt);
                eventSelectIndex();

                if (AIW.Input.isKeyPressed(Keys.X))
                {
                    popFromParent();
                    battleSceneDelegate.changeFocusToMainLayer();
                }
                if (AIW.Input.isKeyPressed(Keys.Z))
                {
                 
                    if (mainLayerDelegate.commandCardInHand(currentSelectIndex))
                    {
                        battleSceneDelegate.changeFocusToMainLayer();
                        popFromParent();
                    }
                }
            }


            void initBG()
            {
                RectangleObj rect = new RectangleObj(AIW.ScreenWidth, AIW.ScreenHeight);
                rect.setZOrder(-5);
                rect.setAlpha(0.85f);
                rect.color_ = Color.Black;
                rect.setPosition(AIW.ScreenCenter);
                addChild(rect);

                SpriteObj bg = new SpriteObj("commandLayerBG");
                bg.setPosition(AIW.ScreenCenter);
                bg.setAlpha(0.5f);
                bg.setZOrder(-1);
                //addChild(bg);
            }
            void addTextToButton(string text,SpriteObj button)
            {
                TextObj label = new TextObj(text);
                button.addChild(label);
                label.setPosition(new Vector2(50, 0));
                addChild(label);
            }
            void initBtnSprite()
            {
                CommandButton obj = new CommandButton("mana");
                obj.setPosition(AIW.ScreenPosByRatio(0.78,0.5));                
                addChild(obj);
                addTextToButton("Mana", obj);

                CommandButton obj2 = new CommandButton("battle");
                obj2.setPosition(AIW.ScreenPosByRatio(0.73, 0.35));
                addChild(obj2);
                addTextToButton("Battle", obj2);

                CommandButton obj3 = new CommandButton("bin");
                obj3.setPosition(AIW.ScreenPosByRatio(0.73,0.65));
                addChild(obj3);
                addTextToButton("Discard", obj3);

                obj.setScale(1.2f);
                obj2.setScale(1.2f);
                obj3.setScale(1.2f);
                cardButtons.Add(obj);
                cardButtons.Add(obj2);
                cardButtons.Add(obj3);

            }
            void initCardSprite(CardSprite cardSprite)
            {
                cardSprite.setPosition(AIW.ScreenPosByRatio(0.44, 0.5));
                cardSprite.setScale(3.5f);
                addChild(cardSprite);
            }

            void initCursor()
            {
                SpriteObj obj = new SpriteObj("mouseCursor");
                obj.setZOrder(1);
                addChild(obj);
                cursor = obj;
                obj.anchorPoint_ = new Vector2(1, 0.5f);
                //obj.setScale(3);
                obj.setPosition(AIW.ScreenCenter);
                updateCursorPosition();
                obj.setRotation(45 + 180 - 10);
            }

            public CardCommandLayer(BattleScene delegate_,MainLayer mainLayerDelegate_, CardSprite cardSprite)
            {
                battleSceneDelegate = delegate_;
                mainLayerDelegate = mainLayerDelegate_;
                initBG();
                initBtnSprite();
                initCardSprite(cardSprite);
                initCursor();
            }
        }

        public class EndPhaseCommandLayer : Layer
        {
            BattleScene battleSceneDelegate;
            MainLayer mainLayerDelegate;
            SpriteObj cursor;
            int currentSelectIndex = 0;
            List<SpriteObj> labels = new List<SpriteObj>();
            int maxIndex{get{return labels.Count;}}

            void updateCursorPosition()
            {
                cursor.setPosition(labels[currentSelectIndex].position+new Vector2(-150,-18));
            }
            void selectNextIndex()
            {
                if (currentSelectIndex + 1 >= maxIndex)
                {
                    currentSelectIndex = 0;
                }
                else
                {
                    currentSelectIndex++;
                }
                updateCursorPosition();
            }
            void selectPreviousIndex()
            {
                if (currentSelectIndex - 1 < 0)
                {
                    currentSelectIndex = maxIndex - 1;
                }
                else
                {
                    currentSelectIndex--;
                }
                updateCursorPosition();
            }

            void eventSelectIndex()
            {
                if (AIW.Input.isKeyPressed(Keys.Up))
                {
                    selectPreviousIndex();
                }
                if (AIW.Input.isKeyPressed(Keys.Down))
                {
                    selectNextIndex();
                }
            }

            void eventCancelCommandButtonB()
            {

                if (AIW.Input.isKeyPressed(Keys.X))
                {
                    popFromParent();
                    battleSceneDelegate.changeFocusToMainLayer();
                }
            }
            void eventSelectCommand()
            {
                if (AIW.Input.isKeyPressed(Keys.Z))
                {

                    if (currentSelectIndex == 1)
                    {
                            mainLayerDelegate.commandEndPhase();
                            popFromParent();
                            battleSceneDelegate.changeFocusToMainLayer();
                        
                    }
                    else if (currentSelectIndex == 0)
                    {
                        popFromParent();
                        battleSceneDelegate.changeFocusToMainLayer();
                    }
                }
            }

            public override void update(GameTime gt)
            {
                base.update(gt);
                eventSelectIndex();
                eventCancelCommandButtonB();
                eventSelectCommand();
            }

            void initCursor()
            {
                SpriteObj obj = new SpriteObj("mouseCursor");
                obj.setZOrder(1);
                addChild(obj);
                cursor = obj;
                obj.anchorPoint_ = new Vector2(1, 0.5f);
                //obj.setScale(3);
                obj.setPosition(AIW.ScreenCenter);
                obj.setRotation(45 + 180 - 10);
                updateCursorPosition();
            }
      


            void initBG()
            {
                RectangleObj rect = new RectangleObj(AIW.ScreenWidth, AIW.ScreenHeight);
                rect.setZOrder(-5);
                rect.setAlpha(0.85f);
                rect.color_ = Color.Black;
                rect.setPosition(AIW.ScreenCenter);
                addChild(rect);
               }
            void initLabels()
            {
                SpriteObj text1 = new SpriteObj("cancel_button");
                text1.setPosition(AIW.ScreenPosByRatio(0.74,0.35));
                //text1.setScale(3);
                addChild(text1);
                labels.Add(text1);
                text1.setZOrder(-1);

                SpriteObj text2 = new SpriteObj("end_button");
                text2.setPosition(AIW.ScreenPosByRatio(0.78,0.5));
                //text2.setScale(3);
                addChild(text2);
                labels.Add(text2);
                text2.setZOrder(-1);
            }

            public EndPhaseCommandLayer(BattleScene battleSceneDelegate_, MainLayer mainLayerDelegate_)
            {
                battleSceneDelegate = battleSceneDelegate_;
                mainLayerDelegate = mainLayerDelegate_;
                initBG();               
                initLabels();
                initCursor();
            }
        }

        public class MarketCommandLayer : Layer
        {
            BattleScene battleSceneDelegate;
            MainLayer mainLayerDelegate;
             SpriteObj cursor;
            int currentSelectIndex = 0;
          
           
            List<TextObj> labels = new List<TextObj>();
            int maxIndex{get{return labels.Count;}}

            void updateCursorPosition()
            {
                cursor.setPosition(labels[currentSelectIndex].position+new Vector2(-66,+10));
            }
            void selectNextIndex()
            {
                if (currentSelectIndex + 1 >= maxIndex)
                {
                    currentSelectIndex = 0;
                }
                else
                {
                    currentSelectIndex++;
                }
                updateCursorPosition();
            }
            void selectPreviousIndex()
            {
                if (currentSelectIndex - 1 < 0)
                {
                    currentSelectIndex = maxIndex - 1;
                }
                else
                {
                    currentSelectIndex--;
                }
                updateCursorPosition();
            }

            void eventSelectIndex()
            {
                if (AIW.Input.isKeyPressed(Keys.Up))
                {
                    selectPreviousIndex();
                }
                if (AIW.Input.isKeyPressed(Keys.Down))
                {
                    selectNextIndex();
                }
            }
            public override void update(GameTime gt)
            {
                base.update(gt);
                eventSelectIndex();

                if (AIW.Input.isKeyPressed(Keys.X))
                {
                    popFromParent();
                    battleSceneDelegate.changeFocusToMainLayer();
                }
                if (AIW.Input.isKeyPressed(Keys.Z))
                {
             
                    if (currentSelectIndex == 0)
                    {

                        if (mainLayerDelegate.commandCardInMarket(currentSelectIndex))
                        {
                            popFromParent();
                            battleSceneDelegate.changeFocusToMainLayer();
                        }
                    }
                    else if (currentSelectIndex == 1)
                    {
                        popFromParent();
                        battleSceneDelegate.changeFocusToMainLayer();
                    }
                }
            }

             void initCardSprite(CardSprite cardSprite)
            {
                cardSprite.setPosition(AIW.ScreenPosByRatio(0.44, 0.5));
                cardSprite.setScale(3.5f);
                addChild(cardSprite);
            }

            void initCursor()
            {
                SpriteObj obj = new SpriteObj("mouseCursor");
                obj.setZOrder(1);
                addChild(obj);
                cursor = obj;
                obj.anchorPoint_ = new Vector2(1, 0.5f);
                //obj.setScale(3);
                obj.setPosition(AIW.ScreenCenter);
                obj.setRotation(45 + 180 - 10);
                updateCursorPosition();
            }
               void initBG()
            {
                RectangleObj rect = new RectangleObj(AIW.ScreenWidth, AIW.ScreenHeight);
                rect.setZOrder(-5);
                rect.setAlpha(0.85f);
                rect.color_ = Color.Black;
                rect.setPosition(AIW.ScreenCenter);
                addChild(rect);
               }
            void initLabels()
            {
                TextObj text1 = new TextObj("Buy");
                text1.setPosition(AIW.ScreenPosByRatio(0.74,0.35));
                text1.setScale(3);
                addChild(text1);
                labels.Add(text1);

                TextObj text2 = new TextObj("Cancel");
                text2.setPosition(AIW.ScreenPosByRatio(0.78,0.5));
                text2.setScale(3);
                addChild(text2);
                labels.Add(text2);
            }

            public MarketCommandLayer(BattleScene battleSceneDelegate_,MainLayer mainLayerDelegate_,CardSprite card)
            {
                battleSceneDelegate = battleSceneDelegate_;
                mainLayerDelegate = mainLayerDelegate_;
                initBG();
                initCardSprite(card);
                initLabels();
                initCursor();
            }
        }
        #endregion

        #region Scene
        MainLayer mainLayer;
        public BattleScene()
        {
            AIW.OS.isShowWallpaper = false;
            MainLayer layer = new MainLayer(this);
            mainLayer = layer;
            addChild("mainLayer",layer);

            HPLabelLayer hpLabelLayer = new HPLabelLayer();
            mainLayer.setHPLabelLayerDelegate(hpLabelLayer);
            addChild("hpLabelLayer", hpLabelLayer);
            
        }

        //Delegate Method
        public void displayCommandLayer(CardSprite cardSprite)
        {
            CardCommandLayer commandLayer = new CardCommandLayer(this,mainLayer,cardSprite);
            pushLayer("commandLayer", commandLayer);
            mainLayer.active_ = false;
        }
        public void displayMarketLayer(CardSprite cardSprite)
        {
            MarketCommandLayer marketLayer = new MarketCommandLayer(this, mainLayer, cardSprite);
            pushLayer("marketCommandLayer", marketLayer);
            mainLayer.active_ = false;
        }
      
        public void displayEndPhaseLayer()
        {
            EndPhaseCommandLayer endPhaseLayer = new EndPhaseCommandLayer(this, mainLayer);
            pushLayer("endPhaseLayer",endPhaseLayer);
             mainLayer.active_ = false;
        }
        public void changeFocusToMainLayer()
        {
            mainLayer.active_ = true;
        }
        #endregion
    }
}
