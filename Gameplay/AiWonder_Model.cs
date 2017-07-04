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
    public class BattleFieldModel
    {
        #region singlaton
        static BattleFieldModel instanceOfBattleFieldModel;
        public static BattleFieldModel sharedBattleFieldModel
        {
            get
            {
                if (instanceOfBattleFieldModel == null)
                {
                    instanceOfBattleFieldModel = new BattleFieldModel();
                }
                return instanceOfBattleFieldModel;
            }
        }
        #endregion

        const int poisonDamage = 1;
        const int poisonDuration = 3;


        public Player player1 = new Player(50);
        public Player player2 = new Player(50);
        public BattleFieldModel()
        {

        }

        public void calculateHealInBattle(Player p1, Player p2,
            CardBattleModel.BattlePile p1BattlePile,
            CardBattleModel.BattlePile p2BattlePile)
        {
            int p1Heal = 0;
            int p2Heal = 0;
            foreach (CardBattleModel.Card card in p1BattlePile.cards)
            {
                if (card.cardType == CardBattleModel.CardType.Heal)
                {
                    p1Heal += card.power;
                }
            }
            foreach (CardBattleModel.Card card in p2BattlePile.cards)
            {
                if (card.cardType == CardBattleModel.CardType.Heal)
                {
                    p2Heal += card.power;
                }
            }

            p1.increaseHealth(p1Heal);
            p2.increaseHealth(p2Heal);

        }

        public void calculatePoisonInBattle(Player p1, Player p2,
        CardBattleModel.BattlePile p1BattlePile,
        CardBattleModel.BattlePile p2BattlePile)
        {
            bool isP1UsePoison = false;
            bool isP2UsePoison = false;
            foreach (CardBattleModel.Card card in p1BattlePile.cards)
            {
                if (card.abilityType == CardBattleModel.AbilityType.Poison)
                    isP1UsePoison = true;
            }
            foreach (CardBattleModel.Card card in p2BattlePile.cards)
            {

                if (card.abilityType == CardBattleModel.AbilityType.Poison)
                    isP2UsePoison = true;
            }


            //apply poison
            if (isP1UsePoison)
            {
                p2.applyPoison(poisonDuration);
            }
            if (isP2UsePoison)
            {
                p1.applyPoison(poisonDuration);
            }

            if (p1.status == Status.Poison)
            {
                if (p1.calculateDamageFromPoison())
                {
                    p1.removePoison();
                }

            }
            if (p2.status == Status.Poison)
            {
                if (p2.calculateDamageFromPoison())
                {
                    p2.removePoison();
                }

            }
        }

        public void calculateDamageInBattle(Player p1, Player p2,
            CardBattleModel.BattlePile p1BattlePile,
            CardBattleModel.BattlePile p2BattlePile)
        {
            int p1ATK = 0;
            int p1DEF = 0;
            bool isP1UseBerserk = false;

            int p2ATK = 0;
            int p2DEF = 0;
            bool isP2UseBerserk = false;



            foreach (CardBattleModel.Card card in p1BattlePile.cards)
            {
                if (card.cardType == CardBattleModel.CardType.Attack)
                {
                    p1ATK += card.power;
                }
                else if (card.cardType == CardBattleModel.CardType.Defence)
                {
                    p1DEF += card.power;
                }

                if (card.abilityType == CardBattleModel.AbilityType.Berserk)
                    isP1UseBerserk = true;

            }

            foreach (CardBattleModel.Card card in p2BattlePile.cards)
            {
                if (card.cardType == CardBattleModel.CardType.Attack)
                {
                    p2ATK += card.power;
                }
                else if (card.cardType == CardBattleModel.CardType.Defence)
                {
                    p2DEF += card.power;
                }


                if (card.abilityType == CardBattleModel.AbilityType.Berserk)
                    isP2UseBerserk = true;
            }

            //apply damage
            p1.decreaseHealth((int)MathHelper.Clamp(p2ATK - p1DEF, 0, 9999));
            p2.decreaseHealth((int)MathHelper.Clamp(p1ATK - p2DEF, 0, 9999));

            //apply berserk
            if (isP1UseBerserk)
            {
                if (AIW.arc4random(p1.hp) % 2 == 0)
                    p1.decreaseHealth((p1ATK - p1DEF) / 2);
            }
            if (isP2UseBerserk)
            {
                if (AIW.arc4random(p2.hp) % 2 == 0)
                    p2.decreaseHealth((p2ATK - p2DEF) / 2);
            }


        }

        public class Player
        {
            int hp_ = 0;
            int maxHp_ = 0;
            public int poisonTurn_ = 0;
            Status status_ = Status.None;
            public int hp { get { return hp_; } }
            public int maxHp { get { return maxHp_; } }
            public Status status { get { return status_; } }

            public Player(int maxHp)
            {
                maxHp_ = maxHp;
                hp_ = maxHp_;
            }

            public void applyPoison(int numberOfTurn)
            {
                poisonTurn_ += numberOfTurn;
                status_ = Status.Poison;
            }
            public bool calculateDamageFromPoison()
            {
                if (poisonTurn_ > 0)
                {
                    poisonTurn_--;
                    decreaseHealth(poisonDamage);
                    return false;
                } return true;

            }
            public void removePoison()
            {
                status_ = Status.None;
                poisonTurn_ = 0;
            }

            public void calculatePoisonDamage()
            {
                if (status_ != Status.Poison) return;
                poisonTurn_--;
                decreaseHealth(poisonDamage);

                if (poisonTurn_ <= 0) status_ = Status.None;
            }

            //return true if player dead
            public bool decreaseHealth(int value)
            {
                hp_ -= value;
                if (hp_ < 0) hp_ = 0;

                if (hp_ <= 0) return true;
                return false;
            }
            public void increaseHealth(int value)
            {
                hp_ += value;
                if (hp_ > maxHp_) hp_ = maxHp_;
            }
        }

        public enum Status
        {
            None,
            Poison,
            Stun,
        }
    }

    public class CardBattleModel
    {
        #region singlaton
        static CardBattleModel instanceOfCardBattleModel;
        public static CardBattleModel sharedCardBattleModel
        {
            get
            {
                if (instanceOfCardBattleModel == null)
                {
                    instanceOfCardBattleModel = new CardBattleModel();
                }
                return instanceOfCardBattleModel;
            }
        }
        #endregion

        public CardBattleModel()
        {
            initMarket();
        }
        const int numberOfPileInMarket = 5;
        public int marketSize { get { return numberOfPileInMarket; } }
        public const int minCardInDeck = 6;
        const int handSize_ = 3;
        public int handSize { get { return handSize_; } }
        const int battlePileSize_ = 3;
        public int battlePileSize { get { return battlePileSize_; } }


        public Player player1 = new Player();
        public Player player2 = new Player();
        public MarketPile[] market = new MarketPile[numberOfPileInMarket];
        void initMarket()
        {
            for (int i = 0; i < market.Length; ++i)
            {
                market[i] = new MarketPile();
                /*
                //test market pile
                if (i == 2) market[i].removeAllCards();
                if (i == 3) market[i].removeAllCards();
                */
            }
        }
        public bool discardFromHand(Player player, int index)
        {
            int numOfCardInDeck = player.deck.countCard + player.hand.countCard;
            if (numOfCardInDeck <= minCardInDeck) return false;

            Card cardToDiscard = useCardFromHandAndRemove(player.hand, index);
            if (!(cardToDiscard.cardType == CardType.NullCard)) return true;
            return false;
        }

        public Card useCardFromHandAndRemove(Hand hand, int index)
        {
            return hand.pickAtIndex(index);
        }
        public bool useCardFromHandAndReturnToDeck(Hand hand, Deck deck, int index)
        {
            Card card = hand.pickAtIndex(index);
            if (card.cardType != CardType.NullCard)
            { deck.addCard(card); return true; }
            return false;
        }
        public bool assignCardToBattle(Player player, int index)
        {
            if (index < player.hand.countCard)
            {
                Card card = player.hand.cards[index];
                player.battlePile.addCard(card);
                useCardFromHandAndReturnToDeck(player.hand, player.deck, index);
                return true;
            }
            return false;
        }

        public bool assignCardToManapool(Player player, int index)
        {
            int manaGain = 0;
            if (index < player.hand.countCard && index >= 0)
            {
                Card card = player.hand.cards[index];
                manaGain = card.manaCost;
                useCardFromHandAndReturnToDeck(player.hand, player.deck, index);
                player.manapool += manaGain;
                return true;
            }
            return false;
        }


        public bool drawCardFromDeck(Player player)
        {
            if (player.hand.countCard < handSize_ && player.deck.countCard > 0)
            {
                player.hand.addCard(player.deck.pickFromTop());
                return true;
            }
            return false;
        }
        public bool buyFromMarket(Player player, int marketIndex)
        {
            if (market[marketIndex].countCard <= 0) return false;
            Card cardToBuy = market[marketIndex].lookFromTop();

            //check if can buy
            if (player.manapool >= cardToBuy.buyCost)
            {
                player.manapool -= cardToBuy.buyCost;
                player.deck.addCard(market[marketIndex].pickFromTop());
                return true;
            }
            return false;
        }

        public void cleanupForNewTurn(Player player)
        {
            for (int i = 0; i < handSize_; ++i)
            {
                useCardFromHandAndReturnToDeck(player.hand, player.deck, 0);
            }

            player.battlePile.removeAllCards();
            //player.manapool = 0;

        }
        public void drawAtBeginOfTurn(Player player)
        {
            player.deck.shuffle();
            for (int i = 0; i < handSize_; ++i)
            {
                drawCardFromDeck(player);
            }
        }

        public class Player
        {
            public int manapool = 0;
            public BattlePile battlePile = new BattlePile();
            public Deck deck = new Deck();
            public Hand hand = new Hand();
        }

        public class Hand : CardPile
        {
            const int size_ = handSize_;
            public int size { get { return size_; } }
        }
        public class BattlePile : CardPile
        {
            const int size_ = 3;
            public int size { get { return size_; } }
        }

        public class Deck : CardPile
        {
            const int numberOfInitialCard = 6;
            public Deck()
            {
                setInitialCards();
            }

            void setInitialCards()
            {
                removeAllCards();

                for (int i = 0; i < numberOfInitialCard; ++i)
                { addCard(getInitialCardIndex(i)); }
            }
        }
        public class MarketPile : CardPile
        {
            const int numberOfInitialCard = 10;
            public MarketPile()
            { setInitialCards(); }
            void setInitialCards()
            {
                removeAllCards();

                for (int i = 0; i < 100; ++i)
                { addCard(CardLibrary.getCardStep3(AIW.arc4random(i) % 6)); }
                for (int i = 0; i < 3; ++i)
                { addCard(CardLibrary.getCardStep2(AIW.arc4random(i) % 8)); }
                for (int i = 0; i < 3; ++i)
                { addCard(CardLibrary.getCardStep1(AIW.arc4random(i) % 6)); }
            }
        }

        public enum CardType
        {
            Attack,
            Defence,
            Heal,
            Support,
            NullCard,

        }
        public enum AbilityType
        {
            None,
            Poison,
            Berserk,
            Stun,
        }

        public enum Element
        {
            None,
            fire,
            ice,
            lightning,
        }

        public class Card
        {
            public string name = "";
            public int buyCost = 0;
            public int manaCost = 0;

            public int power = 0;
            public CardType cardType = CardType.NullCard;
            public AbilityType abilityType = AbilityType.None;
            public Element element = Element.fire;
        }

        public static class CardLibrary
        {
            //Singlaton
            //static CardLibrary instanceOfCardLibrary;
            //public static CardLibrary sharedCardLibrary
            //{
            //    get
            //    {
            //        if (instanceOfCardLibrary == null)
            //            instanceOfCardLibrary = new CardLibrary();

            //        return instanceOfCardLibrary;
            //    }
            //}

            static Card getCard(int power, int mana, CardType type)
            {
                Card cardToReturn = new Card();
                cardToReturn.power = power;
                cardToReturn.cardType = type;
                cardToReturn.buyCost = mana;
                cardToReturn.manaCost = mana;
                cardToReturn.abilityType = AbilityType.None;
                int rand = AIW.arc4random() % 3;
                if (rand == 0)
                {
                    cardToReturn.element = Element.fire;
                }
                else if (rand == 1)
                {
                    cardToReturn.element = Element.ice;
                }
                else
                {
                    cardToReturn.element = Element.lightning;
                }
                return cardToReturn;
            }

            public static int maxLibraryIndex = 34;
            public static Card getCardStep1(int index)
            {
                Card card = new Card();
                switch (index)
                {
                    case 0: card = getCard(2, 2, CardType.Attack);
                        break;
                    case 1: card = getCard(3, 4, CardType.Attack);
                        break;
                    case 2: card = getCard(2, 2, CardType.Defence);
                        break;
                    case 3: card = getCard(3, 4, CardType.Defence);
                        break;
                    case 4: card = getCard(1, 3, CardType.Heal);
                        break;
                    case 5: card = getCard(2, 7, CardType.Heal);
                        break;
                    case 6: card = getCard(2, 2, CardType.Attack);
                        break;
                    case 7: card = getCard(3, 4, CardType.Attack);
                        break;
                    case 8: card = getCard(2, 2, CardType.Defence);
                        break;
                    case 9: card = getCard(3, 4, CardType.Defence);
                        break;
                    case 10: card = getCard(2, 2, CardType.Attack);
                        break;
                    case 11: card = getCard(3, 4, CardType.Attack);
                        break;
                    case 12: card = getCard(2, 2, CardType.Defence);
                        break;
                    case 13: card = getCard(3, 4, CardType.Defence);
                        break;

                    default: break;
                };
                return card;
            }
            public static Card getCardStep2(int index)
            {
                Card card = new Card();
                switch (index)
                {


                    case 0: card = getCard(4, 6, CardType.Attack);
                        break;
                    case 1: card = getCard(5, 9, CardType.Attack);
                        break;
                    case 2: card = getCard(6, 11, CardType.Attack);
                        break;
                    case 3: card = getCard(4, 6, CardType.Defence);
                        break;
                    case 4: card = getCard(5, 9, CardType.Defence);
                        break;
                    case 5: card = getCard(6, 11, CardType.Defence);
                        break;
                    case 6: card = getCard(1, 3, CardType.Heal);
                        break;
                    case 7: card = getCard(2, 7, CardType.Heal);
                        break;
                    case 8: card = getCard(3, 11, CardType.Heal);
                        break;
                    case 9: card = getCard(4, 6, CardType.Attack);
                        break;
                    case 10: card = getCard(5, 9, CardType.Attack);
                        break;
                    case 11: card = getCard(6, 11, CardType.Attack);
                        break;
                    case 12: card = getCard(4, 6, CardType.Defence);
                        break;
                    case 13: card = getCard(5, 9, CardType.Defence);
                        break;
                    case 14: card = getCard(6, 11, CardType.Defence);
                        break;

                    default: break;
                };
                return card;
            }
            public static Card getCardStep3(int index)
            {
                Card card = new Card();
                switch (index)
                {

                    case 0: card = getCard(5, 9, CardType.Attack);
                        break;
                    case 1: card = getCard(6, 11, CardType.Attack);
                        break;
                    case 2: card = getCard(5, 9, CardType.Defence);
                        break;
                    case 3: card = getCard(6, 11, CardType.Defence);
                        break;
                    case 4: card = getCard(4, 15, CardType.Heal);
                        break;
                    case 5: card = getCard(5, 20, CardType.Heal);
                        break;


                    default: break;
                };
                return card;
            }


            public static Card getInitialCardInDeckAtIndex(int index)
            {
                Card card = new Card();
                switch (index)
                {
                    case 0: card = getCard(1, 1, CardType.Attack);
                        break;
                    case 1: card = getCard(1, 1, CardType.Attack);
                        break;
                    case 2: card = getCard(2, 2, CardType.Attack);
                        break;
                    case 3: card = getCard(1, 1, CardType.Defence);
                        break;
                    case 4: card = getCard(1, 1, CardType.Defence);
                        break;
                    case 5: card = getCard(2, 2, CardType.Defence);
                        break;










                    default: break;
                };


                return card;
            }
        }


        public abstract class CardPile
        {
            protected Card getInitialCardIndex(int index)
            {
                return CardLibrary.getInitialCardInDeckAtIndex(index);
            }
            protected Card randomGenCard()
            {
                return CardLibrary.getInitialCardInDeckAtIndex(AIW.arc4random(0) % CardLibrary.maxLibraryIndex);
            }

            protected Card old_randomGenCard()
            {
                Card card = new Card();
                card.cardType = CardType.NullCard;
                switch (AIW.arc4random(0) % 3)
                {
                    case 0: card.cardType = CardType.Attack; break;
                    case 1: card.cardType = CardType.Defence; break;
                    case 2: card.cardType = CardType.Heal; break;
                    //case 3: card.cardType = CardType.Support; break;

                    default: break;
                }


                card.power = AIW.arc4random(0) % 4;
                card.buyCost = AIW.arc4random(0) % 4;
                card.manaCost = card.buyCost;

                return card;
            }
            List<Card> cards_ = new List<Card>();
            public List<Card> cards { get { return cards_; } }
            public int countCard
            {
                get { return cards_.Count; }
            }
            public void addCard(Card card)
            {
                cards_.Add(card);
            }
            public Card pickFromTop()
            {
                return pickAtIndex(cards.Count - 1);
            }
            public Card lookFromTop()
            {
                Card cardToLook = new Card();
                int index = cards.Count - 1;
                if (index < cards_.Count && index >= 0)
                {
                    cardToLook = cards_[index];
                }
                return cardToLook;
            }
            public Card pickAtIndex(int index)
            {
                Card cardToRemove = new Card();
                if (index < cards_.Count && index >= 0)
                {
                    cardToRemove = cards_[index];
                    cards_.RemoveAt(index);
                }
                return cardToRemove;
            }
            public void removeAllCards()
            {
                cards_.Clear();
            }
            public void shuffle()
            {
                List<Card> tempPile = new List<Card>();
                foreach (Card card in cards_)
                {
                    tempPile.Add(card);
                }
                cards_.Clear();
                while (tempPile.Count > 0)
                {
                    int randIndex = AIW.arc4random(tempPile.Count) % tempPile.Count;
                    cards_.Add(tempPile[randIndex]);
                    tempPile.RemoveAt(randIndex);
                }
            }

        }
    }

    public class FieldMapModel
    {
        public enum Place
        {
            Bedroom,
            Library,
            Residence,
            Town,
            TownHall,
            Market,
        }
        #region singlaton
        static FieldMapModel instanceOfAiWonderModel;
        public static FieldMapModel summon
        {
            get
            {
                if (instanceOfAiWonderModel == null)
                {
                    instanceOfAiWonderModel = new FieldMapModel();
                }
                return instanceOfAiWonderModel;
            }
        }
        #endregion

        #region Field
        public Place place = Place.Bedroom;
        #endregion

        public FieldMapModel()
        {

        }
    }
}
