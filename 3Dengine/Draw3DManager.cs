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

using XNAnimation;
using XNAnimation.Controllers;
using XNAnimation.Effects;
using XNAnimation.Pipeline;


namespace AiWonderTest
{
    public class Engine3DController
    {
        Draw3DManager graphic3DEngine;
        public Engine3DController(Draw3DManager _graphic3DEngine)
        {
            graphic3DEngine = _graphic3DEngine;
        }
        public void reset3D()
        {
            graphic3DEngine.SetMapTo(graphic3DEngine.bedroom, new Vector3(0, 0, 0));
            graphic3DEngine.sceneState.scene = 1;
            graphic3DEngine.sceneState.eventState = 1;
            graphic3DEngine.sceneState.dialogue = 1;
            graphic3DEngine.whatRoom = Draw3DManager.WhatRoom.WorkingRoom;
            graphic3DEngine.finiteState3DEngine.aiWonderState = FiniteState3DEngine.AiWonderState.PlayCutScene;
            graphic3DEngine.isPlayCutScene = false;
        }

        public void skipShot1()
        {           
            graphic3DEngine.finiteState3DEngine.aiWonderState = FiniteState3DEngine.AiWonderState.FieldMap;
            graphic3DEngine.sceneState.dialogue = 1;
        }
        public void skipShot2()
        {
            graphic3DEngine.finiteState3DEngine.aiWonderState = FiniteState3DEngine.AiWonderState.FieldMap;
            graphic3DEngine.sceneState.scene = 2;
            graphic3DEngine.sceneState.eventState = 1;
            graphic3DEngine.sceneState.dialogue = 1;
            graphic3DEngine.isPlayCutScene = false;
        }
        public void skipToBattle()
        {
            graphic3DEngine.finiteState3DEngine.aiWonderState = FiniteState3DEngine.AiWonderState.Battle;
            //graphic3DEngine.sceneState.scene = 2;
            //graphic3DEngine.sceneState.eventState = 4;
            //graphic3DEngine.sceneState.dialogue = 1;
            //graphic3DEngine.isPlayCutScene = false;
            //graphic3DEngine.pareak.position = new Vector3(100, 0, -2.3f);
            
        }

        public void jumpToBattle()
        {
            graphic3DEngine.finiteState3DEngine.aiWonderState = FiniteState3DEngine.AiWonderState.Battle;
            graphic3DEngine.sceneState.scene = 2;
            graphic3DEngine.sceneState.eventState = 5;
        }

        public void reset3DWorld()
        {
            graphic3DEngine.finiteState3DEngine.aiWonderState = FiniteState3DEngine.AiWonderState.FieldMap;
            
            graphic3DEngine.sceneState.scene = 1;
            graphic3DEngine.sceneState.eventState = 1;
            graphic3DEngine.sceneState.dialogue = 1;
            graphic3DEngine.isPlayCutScene = false;
            
            graphic3DEngine.gameState = GameState.City;
            graphic3DEngine.map = graphic3DEngine.bedroom;
            graphic3DEngine.testPosition = Vector3.Zero;
            graphic3DEngine.pareak.SetBoundingBox(Vector3.Zero);
        }

        public void skipBattle()
        {
            graphic3DEngine.finiteState3DEngine.aiWonderState = FiniteState3DEngine.AiWonderState.FieldMap;
           /* graphic3DEngine.sceneState.scene = 1;
            graphic3DEngine.sceneState.eventState = 1;
            graphic3DEngine.sceneState.dialogue = 1;
            graphic3DEngine.isPlayCutScene = false;
            graphic3DEngine.pareak.position = new Vector3(0, 0, 0);
            graphic3DEngine.gameState = GameState.City;*/
        }
        public void testSomeThing()
        {
            graphic3DEngine.testSomeThing();
        }
        public void handleInputToWalk()
        {
            if (AIW.Input.isKeyDown(Keys.Left))
            {
                graphic3DEngine.control3D.isKeyDownKeyA = true;
            }
            else
            {
                graphic3DEngine.control3D.isKeyDownKeyA = false;
            }
            if (AIW.Input.isKeyDown(Keys.Right))
            {
                graphic3DEngine.control3D.isKeyDownKeyD = true;
            }
            else
            {
                graphic3DEngine.control3D.isKeyDownKeyD = false;
            }
            if (AIW.Input.isKeyDown(Keys.Up))
            {
                graphic3DEngine.control3D.isKeyDownKeyW = true;
            }
            else
            {
                graphic3DEngine.control3D.isKeyDownKeyW = false;
            }
            if (AIW.Input.isKeyDown(Keys.Down))
            {
                graphic3DEngine.control3D.isKeyDownKeyS = true;
            }
            else
            {
                graphic3DEngine.control3D.isKeyDownKeyS = false;
            }

        }
      


        public void assignedBattleAnimation(Caster caster, Caster target, Spell magic)
        {
            graphic3DEngine.SetEventDamaged(target);
            graphic3DEngine.SetEventAttacker(caster);
            graphic3DEngine.SetEventEffect(magic);
            AIW.Log(this, "assignedBattleAnimation");
            //graphic3DEngine.runAnimation = true;
            //graphic3DEngine.actionStep = 0;
            //graphic3DEngine.tempAnimationStep = 0;
        }
        public bool checkIfAnimationFinish()
        {
            return graphic3DEngine.runAnimation;
        }
    }

    public class FiniteState3DEngine
    {
        public enum AiWonderState
        {
            FieldMap,
            PauseMenu,
            Battle,
            NPC,
            PlayCutScene,
            Shop,
        }

        public AiWonderState aiWonderState = AiWonderState.FieldMap;

    }

   public enum GameState
   {
      City, Battle, CameraMode,
   }
   public enum Caster
   {
       None,
       Player01,
       Enemy01,

       MAX
   }
   public enum Cast
   {
       None,
       Cast_Fire,
       Cast_Dark,
       Cast_Ice,
       Cast_Life,
       Cast_Lighting,

       Max
   }
   public enum Spell
   {
       None,
       Fire01_Effect,
       Fire02_Effect,
       Fire03_Effect,
       Dark01_Effect,
       Dark02_Effect,
       Dark03_Effect,
       Ice01_Effect,
       Ice02_Effect,
       Ice03_Effect,
       Life01_Effect,
       Life02_Effect,
       Life03_Effect,
       Lighting01_Effect,
       Lighting02_Effect,
       Lighting03_Effect,
       Ultimate01_Effect,
       Ultimate02_Effect,

       MAX
   }
   public struct SceneState
   {
      public int scene;
      public int eventState;
      public int dialogue;
   }
   public class Draw3DManager
   {
       #region RFC Session
       bool isTestSomeThing = false;
       public void testSomeThing()
       {
           isTestSomeThing = true;
       }
       void testSomeThing(GameTime gameTime)
       {
           gameState = GameState.Battle;
           isRelease = false;
           C.isKeyDownKeyLeftCtrl = false;
           effectObj["Fire03_Effect"].animationController.StartClip(models["Fire03_Effect"].AnimationClips.Values[0]);
           effectObj["Fire03_Effect"].Update(gameTime);
       }
       #endregion


       ContentManager content;
      public Draw3DManager( )
      {
      }
      #region Field
      public SceneState sceneState;
      public FiniteState3DEngine finiteState3DEngine = new FiniteState3DEngine();

      public BoundingBox testBounding;
      public bool talkingIsCollision = false;
      public string a;



      SkinnedModelObj skyDome;

      public Camera camera;

      public SkinnedModelObj map;
      public SkinnedModelObj market;
      public SkinnedModelObj workingRoom;
      public SkinnedModelObj bedroom;
      public SkinnedModelObj townHall;
      public SkinnedModelObj center;
      public SkinnedModelObj residence;
      public SkinnedModelObj library;
      public SkinnedModelObj marketWater;
      public SkinnedModelObj residenceWater;

      SkinnedModelObj battle;

      public SkinnedModelObj pareak;
      public SkinnedModelObj Kirk;
      public SkinnedModelObj rainel;
      public SkinnedModelObj angel;
      public SkinnedModelObj darkAngel;

      SkinnedModelObj monsterBig;
      SkinnedModelObj monsterMedium;
      SkinnedModelObj monsterSmall;

      SkinnedModelObj warpMarket_Center;
      SkinnedModelObj warpMarket_Residence;
      SkinnedModelObj warpTownHall_Center;
      SkinnedModelObj warpWorkroom_TownHall;
      SkinnedModelObj warpTownHall_Workroom;
      SkinnedModelObj warpCenter_TownHall;
      SkinnedModelObj warpCenter_Market;
      SkinnedModelObj warpResidence_Bedroom;
      SkinnedModelObj warpResidence_Market;
      SkinnedModelObj warpBedroom_Residence;
      SkinnedModelObj warpLibrary_TownHall;

      public Dictionary<string, SkinnedModelObj> npcList;

      public Dictionary<string, SkinnedModelObj> effectObj;
      Dictionary<string, SkinnedModelObj> player;
      Dictionary<string, SkinnedModelObj> enemy;
      Dictionary<string, SkinnedModelObj> effectCast;

      public enum WhatRoom
      {
         WorkingRoom,
         Library,
         TownHall,
         None,
      }
      
      public WhatRoom whatRoom = WhatRoom.WorkingRoom;


      SkinnedModelObj selectEffect;
      SkinnedModelObj AreaBattleEffect;
      SkinnedModelObj effectWarp;

      public GameState gameState = GameState.City;

      public int selectStep = 0;
      public int actionStep = 0;
      public bool isAnimationFinished()
      {
          if (actionStep == 4)
          {
              return true;
          }
          return false;
      }

      public ActionState[] actionStateList;

      public Caster attacker = Caster.None;
      public Caster damaged = Caster.None;
      public Spell effectCurrent = Spell.None;


      public bool runAnimation = false;

      public Dictionary<string, SkinnedModel> models = new Dictionary<string, SkinnedModel>();
      void addModel(string keyName, string AddressModel)
      {
         models.Add(keyName, content.Load<SkinnedModel>(AddressModel));
      }
      #endregion
      public void LoadContent()
      {
          if (content == null)
              content = new ContentManager(AIW.OS.Game.Services, "Content");

          
         camera = new Camera(AIW.OS.graphicsDevice, AIW.OS.Game.Window, content);

         sceneState.scene = 1;
         sceneState.eventState = 1;
         sceneState.dialogue = 1;

         
         finiteState3DEngine.aiWonderState = FiniteState3DEngine.AiWonderState.PlayCutScene;

         addModel("Kris", "Model3D\\Kris_Model");
         addModel("Kirk", "Model3D\\Kirk_Model");
         addModel("Rainel", "Model3D\\Rainel_Model");
         addModel("Angel", "Model3D\\Angel_Model");
         addModel("DarkAngel", "Model3D\\DarkAngel_Model");

         addModel("MonsterBig", "Model3D\\MonsterBig_Model");
         addModel("MonsterMedium", "Model3D\\MonsterMid_Model");
         addModel("MonsterSmall", "Model3D\\MonsterSmall_Model");

         addModel("NPCLeader", "Model3D\\NPCLeader_Model");
         addModel("NPCSave", "Model3D\\NPCSave_Model");
         addModel("NPCFAQ", "Model3D\\NPCFAQ_Model");
         addModel("NPC01", "Model3D\\NPC01_Model");
         addModel("NPC02", "Model3D\\NPC02_Model");
         addModel("NPC03", "Model3D\\NPC03_Model");
         addModel("NPC04", "Model3D\\NPC04_Model");


         addModel("WorkingRoom", "Scene3D\\WorkingRoom_Model");
         addModel("Market", "Scene3D\\Market_Model");
         addModel("Market_Water", "Scene3D\\MarketWater_Model");
         addModel("Bedroom", "Scene3D\\Bedroom_Model");
         addModel("TownHall", "Scene3D\\TownHall_Model");
         addModel("Center", "Scene3D\\Center_Model");
         addModel("Residence", "Scene3D\\Residence_Model");
         addModel("Residence_Water", "Scene3D\\ResidenceWater_Model");
         addModel("Library", "Scene3D\\Library_Model");

         addModel("SkyDome", "Effect3D\\SkyDome");

         addModel("Warp", "Model3D\\Warp");


         addModel("Cast_Dark", "Effect3D\\Casting_Dark");
         addModel("Cast_Fire", "Effect3D\\Casting_Fire");
         addModel("Cast_Ice", "Effect3D\\Casting_Ice");
         addModel("Cast_Life", "Effect3D\\Casting_Life");
         addModel("Cast_Lighting", "Effect3D\\Casting_Lighting");

         addModel("Dark01_Effect", "Effect3D\\Dark02_Effect");
         addModel("Dark02_Effect", "Effect3D\\Dark02_Effect");
         addModel("Dark03_Effect", "Effect3D\\Dark03_Effect");
         addModel("Fire01_Effect", "Effect3D\\Fire01_Effect");
         addModel("Fire02_Effect", "Effect3D\\Fire02_Effect");
         addModel("Fire03_Effect", "Effect3D\\Fire03_Effect");
         addModel("Ice01_Effect", "Effect3D\\Ice01_Effect");
         addModel("Ice02_Effect", "Effect3D\\Ice02_Effect");
         addModel("Ice03_Effect", "Effect3D\\Ice03_Effect");
         addModel("Life01_Effect", "Effect3D\\Life01_Effect");
         addModel("Life02_Effect", "Effect3D\\Life02_Effect");
         addModel("Life03_Effect", "Effect3D\\Life03_Effect");
         addModel("Lighting01_Effect", "Effect3D\\Lighting01_Effect");
         addModel("Lighting02_Effect", "Effect3D\\Lighting02_Effect");
         addModel("Lighting03_Effect", "Effect3D\\Lighting03_Effect");
         addModel("Ultimate01_Effect", "Effect3D\\Ultimate01_Effect");
         addModel("Ultimate02_Effect", "Effect3D\\Ultimate02_Effect");

         addModel("Select_Effect", "Model3D\\SelectCharacterEffect");
         addModel("AreaBattle", "Model3D\\AreaBattle");
         addModel("EffectWarp", "Model3D\\EffectWarp");


         selectEffect = new SkinnedModelObj("Select_Effect", models, AIW.OS.graphicsDeviceManager);

         skyDome = new SkinnedModelObj("SkyDome", models, AIW.OS.graphicsDeviceManager);

         effectWarp = new SkinnedModelObj("EffectWarp", models, AIW.OS.graphicsDeviceManager);

         AreaBattleEffect = new SkinnedModelObj("AreaBattle", models, AIW.OS.graphicsDeviceManager);

         effectCast = new Dictionary<string, SkinnedModelObj>();

         effectCast.Add("Cast_Dark", new SkinnedModelObj("Cast_Dark", models, AIW.OS.graphicsDeviceManager));
         effectCast.Add("Cast_Fire", new SkinnedModelObj("Cast_Fire", models, AIW.OS.graphicsDeviceManager));
         effectCast.Add("Cast_Ice", new SkinnedModelObj("Cast_Ice", models, AIW.OS.graphicsDeviceManager));
         effectCast.Add("Cast_Life", new SkinnedModelObj("Cast_Life", models, AIW.OS.graphicsDeviceManager));
         effectCast.Add("Cast_Lighting", new SkinnedModelObj("Cast_Lighting", models, AIW.OS.graphicsDeviceManager));

         effectObj = new Dictionary<string, SkinnedModelObj>();

         effectObj.Add("Dark01_Effect", new SkinnedModelObj("Dark01_Effect", models, AIW.OS.graphicsDeviceManager));
         effectObj.Add("Dark02_Effect", new SkinnedModelObj("Dark02_Effect", models, AIW.OS.graphicsDeviceManager));
         effectObj.Add("Dark03_Effect", new SkinnedModelObj("Dark03_Effect", models, AIW.OS.graphicsDeviceManager));
         effectObj.Add("Fire01_Effect", new SkinnedModelObj("Fire01_Effect", models, AIW.OS.graphicsDeviceManager));
         effectObj.Add("Fire02_Effect", new SkinnedModelObj("Fire02_Effect", models, AIW.OS.graphicsDeviceManager));
         effectObj.Add("Fire03_Effect", new SkinnedModelObj("Fire03_Effect", models, AIW.OS.graphicsDeviceManager));
         effectObj.Add("Ice01_Effect", new SkinnedModelObj("Ice01_Effect", models, AIW.OS.graphicsDeviceManager));
         effectObj.Add("Ice02_Effect", new SkinnedModelObj("Ice02_Effect", models, AIW.OS.graphicsDeviceManager));
         effectObj.Add("Ice03_Effect", new SkinnedModelObj("Ice03_Effect", models, AIW.OS.graphicsDeviceManager));
         effectObj.Add("Life01_Effect", new SkinnedModelObj("Life01_Effect", models, AIW.OS.graphicsDeviceManager));
         effectObj.Add("Life02_Effect", new SkinnedModelObj("Life02_Effect", models, AIW.OS.graphicsDeviceManager));
         effectObj.Add("Life03_Effect", new SkinnedModelObj("Life03_Effect", models, AIW.OS.graphicsDeviceManager));
         effectObj.Add("Lighting01_Effect", new SkinnedModelObj("Lighting01_Effect", models, AIW.OS.graphicsDeviceManager));
         effectObj.Add("Lighting02_Effect", new SkinnedModelObj("Lighting02_Effect", models, AIW.OS.graphicsDeviceManager));
         effectObj.Add("Lighting03_Effect", new SkinnedModelObj("Lighting03_Effect", models, AIW.OS.graphicsDeviceManager));
         effectObj.Add("Ultimate01_Effect", new SkinnedModelObj("Ultimate01_Effect", models, AIW.OS.graphicsDeviceManager));
         effectObj.Add("Ultimate02_Effect", new SkinnedModelObj("Ultimate02_Effect", models, AIW.OS.graphicsDeviceManager));

         warpMarket_Center = new SkinnedModelObj("Warp", models, AIW.OS.graphicsDeviceManager, new Vector3(126, 0, -150));
         warpMarket_Residence = new SkinnedModelObj("Warp", models, AIW.OS.graphicsDeviceManager, new Vector3(141, 0, -73));
         warpTownHall_Center = new SkinnedModelObj("Warp", models, AIW.OS.graphicsDeviceManager, new Vector3(0, 0, 53));
         warpTownHall_Workroom = new SkinnedModelObj("Warp", models, AIW.OS.graphicsDeviceManager, new Vector3(-0.5f, 0, -50));
         warpWorkroom_TownHall = new SkinnedModelObj("Warp", models, AIW.OS.graphicsDeviceManager, new Vector3(24.5f, 0, 13));
         warpCenter_TownHall = new SkinnedModelObj("Warp", models, AIW.OS.graphicsDeviceManager, new Vector3(0, 0, -14.75f));
         warpCenter_Market = new SkinnedModelObj("Warp", models, AIW.OS.graphicsDeviceManager, new Vector3(45.5f, 0, 13.5f));
         warpResidence_Bedroom = new SkinnedModelObj("Warp", models, AIW.OS.graphicsDeviceManager, new Vector3(19, 0, -14));
         warpResidence_Market = new SkinnedModelObj("Warp", models, AIW.OS.graphicsDeviceManager, new Vector3(5, 0, 5));
         warpBedroom_Residence = new SkinnedModelObj("Warp", models, AIW.OS.graphicsDeviceManager, new Vector3(-14, 0, 9));
         warpLibrary_TownHall = new SkinnedModelObj("Warp", models, AIW.OS.graphicsDeviceManager, new Vector3(-26, 0, 4.5f));

         residence = new SkinnedModelObj("Residence", content.Load<Model>("Scene3D\\Residence_Bounding"), models, AIW.OS.graphicsDeviceManager);
         residenceWater = new SkinnedModelObj("Residence_Water", models, AIW.OS.graphicsDeviceManager);
         center = new SkinnedModelObj("Center", content.Load<Model>("Scene3D\\Center_Bounding"), models, AIW.OS.graphicsDeviceManager);
         workingRoom = new SkinnedModelObj("WorkingRoom", content.Load<Model>("Scene3D\\WorkingRoom_Bounding"), models, AIW.OS.graphicsDeviceManager);
         market = new SkinnedModelObj("Market", content.Load<Model>("Scene3D\\Market_Bounding"), models, AIW.OS.graphicsDeviceManager);
         marketWater = new SkinnedModelObj("Market_Water", models, AIW.OS.graphicsDeviceManager);
         bedroom = new SkinnedModelObj("Bedroom", content.Load<Model>("Scene3D\\Bedroom_Bounding"), models, AIW.OS.graphicsDeviceManager);
         townHall = new SkinnedModelObj("TownHall", content.Load<Model>("Scene3D\\TownHall_Bounding"), models, AIW.OS.graphicsDeviceManager);
         library = new SkinnedModelObj("Library", content.Load<Model>("Scene3D\\Library_Bounding"), models, AIW.OS.graphicsDeviceManager);

         battle = market;
         map = bedroom;

         pareak = new SkinnedModelObj("Kris", content.Load<Model>("Model3D\\boundingCharacter"), models, AIW.OS.graphicsDeviceManager);

         pareak.CreateTalkingCheck(new Vector3(3, 3, 3));
         pareak.animationController.StartClip(models[pareak.modelName].AnimationClips["idle"]);
         pareak.shadowEnable = true;

         Kirk = new SkinnedModelObj("Kirk", content.Load<Model>("Model3D\\boundingCharacter"), models
             , AIW.OS.graphicsDeviceManager, warpResidence_Bedroom.position + Vector3.Left);
         Kirk.animationController.StartClip(models[Kirk.modelName].AnimationClips["idle"]);

         rainel = new SkinnedModelObj("Rainel", content.Load<Model>("Model3D\\boundingCharacter"), models
             , AIW.OS.graphicsDeviceManager);
         angel = new SkinnedModelObj("Angel", content.Load<Model>("Model3D\\boundingCharacter"), models
             , AIW.OS.graphicsDeviceManager);
         angel.shadowEnable = true;
         angel.animationController.StartClip(models[angel.modelName].AnimationClips["idle"]);

         darkAngel = new SkinnedModelObj("DarkAngel", content.Load<Model>("Model3D\\boundingCharacter"), models
             , AIW.OS.graphicsDeviceManager);
         darkAngel.oldAnimation = ActionState.idle;
         darkAngel.animationController.StartClip(models[darkAngel.modelName].AnimationClips["idle"]);
         darkAngel.shadowEnable = true;
         //testBounding = pareak.talkingAreaCheck;
         nearestNpc = string.Empty;
         npcList = new Dictionary<string, SkinnedModelObj>();


         npcList.Add("NPCLeader", new SkinnedModelObj("NPCLeader", content.Load<Model>("Model3D\\boundingCharacter"), models,
            AIW.OS.graphicsDeviceManager));
         npcList.Add("NPCSave01", new SkinnedModelObj("NPCSave", content.Load<Model>("Model3D\\boundingCharacter"), models,
             AIW.OS.graphicsDeviceManager));
         npcList.Add("NPCSave02", new SkinnedModelObj("NPCSave", content.Load<Model>("Model3D\\boundingCharacter"), models,
             AIW.OS.graphicsDeviceManager));
         npcList.Add("NPCFAQ01", new SkinnedModelObj("NPCFAQ", content.Load<Model>("Model3D\\boundingCharacter"), models,
             AIW.OS.graphicsDeviceManager));
         npcList.Add("NPCFAQ02", new SkinnedModelObj("NPCFAQ", content.Load<Model>("Model3D\\boundingCharacter"), models,
             AIW.OS.graphicsDeviceManager));
         npcList.Add("NPCPeople01", new SkinnedModelObj("NPC01", content.Load<Model>("Model3D\\boundingCharacter"), models,
             AIW.OS.graphicsDeviceManager));
         npcList.Add("NPCPeople02", new SkinnedModelObj("NPC02", content.Load<Model>("Model3D\\boundingCharacter"), models,
             AIW.OS.graphicsDeviceManager));
         npcList.Add("NPCPeople03", new SkinnedModelObj("NPC03", content.Load<Model>("Model3D\\boundingCharacter"), models,
             AIW.OS.graphicsDeviceManager));
         npcList.Add("NPCPeople04", new SkinnedModelObj("NPC04", content.Load<Model>("Model3D\\boundingCharacter"), models,
             AIW.OS.graphicsDeviceManager));
         npcList.Add("NPCPeople05", new SkinnedModelObj("NPC01", content.Load<Model>("Model3D\\boundingCharacter"), models,
             AIW.OS.graphicsDeviceManager));
         npcList.Add("NPCPeople06", new SkinnedModelObj("NPC01", content.Load<Model>("Model3D\\boundingCharacter"), models,
             AIW.OS.graphicsDeviceManager));
         npcList.Add("NPCPeople07", new SkinnedModelObj("NPC02", content.Load<Model>("Model3D\\boundingCharacter"), models,
             AIW.OS.graphicsDeviceManager));
         npcList.Add("NPCPeople08", new SkinnedModelObj("NPC03", content.Load<Model>("Model3D\\boundingCharacter"), models,
             AIW.OS.graphicsDeviceManager));
         npcList.Add("NPCPeople09", new SkinnedModelObj("NPC04", content.Load<Model>("Model3D\\boundingCharacter"), models,
             AIW.OS.graphicsDeviceManager));
         npcList.Add("NPCPeople10", new SkinnedModelObj("NPC02", content.Load<Model>("Model3D\\boundingCharacter"), models,
             AIW.OS.graphicsDeviceManager));

         foreach (string npcIndex in npcList.Keys)
         {
            npcList[npcIndex].shadowEnable = true;
            npcList[npcIndex].SetBoundingBox(new Vector3(-100));
            npcList[npcIndex].position.Y = -100;
         }



         monsterBig = new SkinnedModelObj("MonsterBig", content.Load<Model>("Model3D\\boundingCharacter"), models,
             AIW.OS.graphicsDeviceManager, new Vector3(-15, 0, 0));
         monsterBig.shadowEnable = true;
         monsterMedium = new SkinnedModelObj("MonsterMedium", content.Load<Model>("Model3D\\boundingCharacter"), models,
            AIW.OS.graphicsDeviceManager, new Vector3(-15, 0, 0));
         monsterMedium.shadowEnable = true;
         monsterSmall = new SkinnedModelObj("MonsterSmall", content.Load<Model>("Model3D\\boundingCharacter"), models,
             AIW.OS.graphicsDeviceManager, new Vector3(-15, 0, 0));
         monsterSmall.shadowEnable = true;
         player = new Dictionary<string, SkinnedModelObj>();
         enemy = new Dictionary<string, SkinnedModelObj>();
         player.Add("Player01", new SkinnedModelObj("Kris", content.Load<Model>("Model3D\\boundingCharacter"), models,
             AIW.OS.graphicsDeviceManager, new Vector3(15, 0, 0)));

         enemy.Add("Enemy01", new SkinnedModelObj("MonsterBig", content.Load<Model>("Model3D\\boundingCharacter")
             , models, AIW.OS.graphicsDeviceManager, new Vector3(-15, 0, 0)));



         foreach (string index in player.Keys)
         {
            player[index].shadowEnable = true;
            player[index].rotation = new Vector3(0, -90 * DEGREE2RADIAN, 0);
         }
         foreach (string index in enemy.Keys)
         {
            enemy[index].shadowEnable = true;
            enemy[index].rotation = new Vector3(0, 90 * DEGREE2RADIAN, 0);
         }

      }

      public void Update(GameTime gameTime)
      {
         skyDome.Update(gameTime);


        
         switch (gameState)
         {
            case GameState.CameraMode:
               pareak.Update(gameTime);
               map.Update(gameTime);
               marketWater.Update(gameTime);
               residenceWater.Update(gameTime);
               foreach (string index in npcList.Keys)
               {
                  if (npcList[index].position.Y != -100)
                  {
                     npcList[index].Update(gameTime);
                  }
               }
               //ks = Keyboard.GetState();
               UpdateCameraMode(gameTime);
               if (!isPlayCutScene)
               {

                  if (nearestNpc != string.Empty)
                  {
                     selectEffect.Update(gameTime);
                  }
               }
               SetCamera(CameraState.CameraMode, gameTime, camera.cameraPosition, camera.mouse_rotation, Vector3.Zero, camera);
               //UpdateNPCAndWarp(gameTime);
               break;
            case GameState.City:
               pareak.Update(gameTime);
               map.Update(gameTime);
               marketWater.Update(gameTime);
               residenceWater.Update(gameTime);
               UpdateCutScene(sceneState, map.modelName, gameTime);
               foreach (string index in npcList.Keys)
               {
                  if (npcList[index].position.Y != -100)
                  {
                     npcList[index].Update(gameTime);
                  }
               }
               //Console.WriteLine("cameraState " + isPlayCutScene.ToString());
               //Console.WriteLine("Scene " + sceneState.scene.ToString());
               //Console.WriteLine("EventState " + sceneState.eventState.ToString());
               //Console.WriteLine("aiWonderState " + finiteState3DEngine.aiWonderState.ToString());
               //ks = Keyboard.GetState();

               if (!isPlayCutScene)
               {
                  HandleCameraInput(gameTime);
                  if (nearestNpc != string.Empty)
                  {
                     selectEffect.Update(gameTime);
                  }
               }
               UpdateNPCAndWarp(gameTime);
               break;
            case GameState.Battle:
               battle.Update(gameTime);
               AreaBattleEffect.Update(gameTime);

               foreach (string index in player.Keys)
               {
                  player[index].Update(gameTime);
               }
               foreach (string index in enemy.Keys)
               {
                  enemy[index].Update(gameTime);
               }
               //HandleBattleInput(gameTime);

               UpdateCutScene(sceneState, map.modelName, gameTime);
               HandleBattleTest(gameTime);
               break;
         }
      }
      public void UpdateCameraMode(GameTime gameTime)
      {
         float u = 5.0f * 100000 * 0.00005f * C.runBoost;
         int numOfPressed = 0;
         Vector3 speed = Vector3.Zero;
        // ks = Keyboard.GetState();

         if (isRelease)
         {
            if (!isMapChanged && !isPlayCutScene)
            {
               if (C.isKeyDownKeyD)
               {
                  speed.X--;
                  numOfPressed++;
                  cameraStateCanChanged = false;
               }
               if (C.isKeyDownKeyA)
               {
                  speed.X++;
                  numOfPressed++;
                  cameraStateCanChanged = false;
               }
               if (C.isKeyDownKeyW)
               {
                  speed.Z++;
                  numOfPressed++;
                  cameraStateCanChanged = false;
               }

               if (C.isKeyDownKeyS)
               {
                  speed.Z--;
                  numOfPressed++;
                  cameraStateCanChanged = false;
               }
            }
            //if (ks.IsKeyDown(Keys.F11))
            //{
            //   gameState = GameState.City;
            //   map.modelName = mapTemp;
            //   isRelease = false;
            //}
         }

         //if (!ks.IsKeyDown(Keys.F11))
         //{
         //   isRelease = true;
         //}
     

         camera.cameraPosition += Move(speed.X * u, speed.Z * u, camera.mouse_rotation.Y);
         //if (ks.IsKeyDown(Keys.Up))
         //   camera.cameraPosition.Y += 0.1f;
         //if (ks.IsKeyDown(Keys.Down))
         //   camera.cameraPosition.Y -= 0.1f;
      }

      #region Fields

      public Vector3 testBoundingPosition;
      public string isCollision;
      public bool isPlayCutScene = false;


      public const float DEGREE2RADIAN = (3.1514926f / 180.0f);
      public const float RADIAN2DEGREE = (1.0f / DEGREE2RADIAN);
      const float position_factor = 0.01f;


      Vector3 temp = Vector3.Zero;
      public bool isRelease = true;
      ActionState currentAnimationTest = ActionState.walk;


      //KeyboardState ks;

      public Vector3 testPosition;
      public Vector3 testVelocity = Vector3.Zero;

      public Vector3 testRotation = Vector3.Zero;


      RenderTarget2D colorRT;
      RenderTarget2D depthRT;

      float focalDistance = 7.0f;
      float focalWidth = 20.0f;
      //
      #endregion

      #region Camera and NPC
      public string[] npcInarea = new string[50];
      public string nearestNpc;
      public bool isMapChanged = false;
      float tempNpcNearestDistance = 5;
      public float distanceNearest = 0;
      public Vector3 cameraPosition = Vector3.Zero;
      bool cameraStateCanChanged = true;
      bool isThirdPersonCamera = false;
      public Control3D control3D = new Control3D();
      public Control3D C { get { return control3D; } }

      public string mapTemp = string.Empty;

      private void HandleCameraInput(GameTime gameTime)
      {
         float u = 5.0f * 100000 * 0.00005f * C.runBoost;
         int numOfPressed = 0;
         Vector3 speed = Vector3.Zero;
        

         if (isRelease)
         {
            if (!isMapChanged && !isPlayCutScene)
               #region Control Player
               switch (camera.cameraState)
               {
                  case CameraState.ThirdPersonCamera:
                     {
                        if (C.isKeyDownKeyD)
                        {
                           speed.X--;
                           numOfPressed++;
                           testRotation.Y = (-camera.mouse_rotation.Y + 90) * DEGREE2RADIAN;
                           camera.cameraRotation = camera.mouse_rotation;
                           currentAnimationTest = ActionState.walk;
                           cameraStateCanChanged = false;
                        }
                        if (C.isKeyDownKeyA)
                        {
                           speed.X++;
                           numOfPressed++;
                           testRotation.Y = (-camera.mouse_rotation.Y - 90) * DEGREE2RADIAN;
                           camera.cameraRotation = camera.mouse_rotation;
                           currentAnimationTest = ActionState.walk;
                           cameraStateCanChanged = false;
                        }
                        if (C.isKeyDownKeyW)
                        {
                           speed.Z++;
                           numOfPressed++;
                           testRotation.Y = (-camera.mouse_rotation.Y + 180) * DEGREE2RADIAN;
                           camera.cameraRotation = camera.mouse_rotation;
                           currentAnimationTest = ActionState.walk;
                           cameraStateCanChanged = false;
                        }

                        if (C.isKeyDownKeyS)
                        {
                           speed.Z--;
                           numOfPressed++;
                           testRotation.Y = (-camera.mouse_rotation.Y) * DEGREE2RADIAN;
                           camera.cameraRotation = camera.mouse_rotation;
                           currentAnimationTest = ActionState.walk;
                           cameraStateCanChanged = false;
                        }

                     }
                     break;
                  case CameraState.ThirdView:
                     {
                        if (C.isKeyDownKeyD)
                        {
                           speed.X--;
                           numOfPressed++;
                           testRotation.Y = (-camera.mouse_rotation.Y + 90) * DEGREE2RADIAN;
                           camera.cameraRotation = camera.mouse_rotation;
                           currentAnimationTest = ActionState.walk;
                           cameraStateCanChanged = false;
                        }
                        if (C.isKeyDownKeyA)
                        {
                           speed.X++;
                           numOfPressed++;
                           testRotation.Y = (-camera.mouse_rotation.Y - 90) * DEGREE2RADIAN;
                           camera.cameraRotation = camera.mouse_rotation;
                           currentAnimationTest = ActionState.walk;
                           cameraStateCanChanged = false;
                        }
                        if ( C.isKeyDownKeyW)
                        {
                           speed.Z++;
                           numOfPressed++;
                           testRotation.Y = (-camera.mouse_rotation.Y + 180) * DEGREE2RADIAN;
                           camera.cameraRotation = camera.mouse_rotation;
                           currentAnimationTest = ActionState.walk;
                           cameraStateCanChanged = false;
                        }

                        if (C.isKeyDownKeyS)
                        {
                           speed.Z--;
                           numOfPressed++;
                           testRotation.Y = (-camera.mouse_rotation.Y) * DEGREE2RADIAN;
                           camera.cameraRotation = camera.mouse_rotation;
                           currentAnimationTest = ActionState.walk;
                           cameraStateCanChanged = false;
                        }
                     }
                     break;

                  case CameraState.PlanCamera:
                     {
                        if (C.isKeyDownKeyD)
                        {
                           speed.X--;
                           numOfPressed++;
                           testRotation.Y = (-camera.cameraRotation.Y + 90) * DEGREE2RADIAN;
                           currentAnimationTest = ActionState.walk;
                           cameraStateCanChanged = false;
                        }
                        if (C.isKeyDownKeyA)
                        {
                           speed.X++;
                           numOfPressed++;
                           testRotation.Y = (-camera.cameraRotation.Y - 90) * DEGREE2RADIAN;
                           currentAnimationTest = ActionState.walk;
                           cameraStateCanChanged = false;
                        }
                        if (C.isKeyDownKeyW)
                        {
                           speed.Z++;
                           numOfPressed++;
                           testRotation.Y = (-camera.cameraRotation.Y + 180) * DEGREE2RADIAN;
                           currentAnimationTest = ActionState.walk;
                           cameraStateCanChanged = false;
                        }

                        if (C.isKeyDownKeyS)
                        {
                           speed.Z--;
                           numOfPressed++;
                           testRotation.Y = (-camera.cameraRotation.Y) * DEGREE2RADIAN;
                           currentAnimationTest = ActionState.walk;
                           cameraStateCanChanged = false;
                        }
                     }

                     break;
                  case CameraState.LookAt:
                     {
                        if (C.isKeyDownKeyD)
                        {
                           speed.X--;
                           numOfPressed++;
                           testRotation.Y = (-camera.cameraRotation.Y + 90) * DEGREE2RADIAN;
                           currentAnimationTest = ActionState.walk;
                           cameraStateCanChanged = false;
                        }
                        if (C.isKeyDownKeyA)
                        {
                           speed.X++;
                           numOfPressed++;
                           testRotation.Y = (-camera.cameraRotation.Y - 90) * DEGREE2RADIAN;
                           currentAnimationTest = ActionState.walk;
                           cameraStateCanChanged = false;
                        }
                        if (C.isKeyDownKeyW)
                        {
                           speed.Z++;
                           numOfPressed++;
                           testRotation.Y = (-camera.cameraRotation.Y + 180) * DEGREE2RADIAN;
                           currentAnimationTest = ActionState.walk;
                           cameraStateCanChanged = false;
                        }

                        if (C.isKeyDownKeyS)
                        {
                           speed.Z--;
                           numOfPressed++;
                           testRotation.Y = (-camera.cameraRotation.Y) * DEGREE2RADIAN;
                           currentAnimationTest = ActionState.walk;
                           cameraStateCanChanged = false;
                        }
                     }
                     break;
                  default:
                     {
                        if (C.isKeyDownKeyD)
                        {
                           speed.X--;
                           numOfPressed++;
                           testRotation.Y = (-camera.cameraRotation.Y + 90) * DEGREE2RADIAN;
                           currentAnimationTest = ActionState.walk;
                           cameraStateCanChanged = false;
                        }
                        if (C.isKeyDownKeyA)
                        {
                           speed.X++;
                           numOfPressed++;
                           testRotation.Y = (-camera.cameraRotation.Y - 90) * DEGREE2RADIAN;
                           currentAnimationTest = ActionState.walk;
                           cameraStateCanChanged = false;
                        }
                        if (C.isKeyDownKeyW)
                        {
                           speed.Z++;
                           numOfPressed++;
                           testRotation.Y = (-camera.cameraRotation.Y + 180) * DEGREE2RADIAN;
                           currentAnimationTest = ActionState.walk;
                           cameraStateCanChanged = false;
                        }

                        if (C.isKeyDownKeyS)
                        {
                           speed.Z--;
                           numOfPressed++;
                           testRotation.Y = (-camera.cameraRotation.Y) * DEGREE2RADIAN;
                           currentAnimationTest = ActionState.walk;
                           cameraStateCanChanged = false;
                        }
                     }
                     break;

               }

            if (numOfPressed % 2 == 0)
            {
               speed /= 2;
            }

            testVelocity = Move(speed.X * u, speed.Z * u, camera.cameraRotation.Y);
               #endregion

            //if (ks.IsKeyDown(Keys.F12))
            //{
            //   if (!isThirdPersonCamera)
            //   {
            //      isThirdPersonCamera = true;
            //      isRelease = false;
            //   }
            //   else
            //   {
            //      isThirdPersonCamera = false;
            //      isRelease = false;
            //   }
            //}

            //if (ks.IsKeyDown(Keys.LeftControl) || C.isKeyDownKeyLeftCtrl)
            //{
            //   gameState = GameState.Battle;
            //   isRelease = false;
            //   C.isKeyDownKeyLeftCtrl = false;
            //   effectObj["Fire03_Effect"].animationController.StartClip(models["Fire03_Effect"].AnimationClips.Values[0]);
            //   effectObj["Fire03_Effect"].Update(gameTime);
            //}

            ///game state control
            ///
            //if (isRelease)
            //   if (ks.IsKeyDown(Keys.Right))
            //   {
            //      sceneState.eventState++;
            //      isRelease = false;
            //   }
            //if (ks.IsKeyDown(Keys.Left))
            //{
            //   sceneState.eventState--;
            //   isRelease = false;
            //}
            //if (ks.IsKeyDown(Keys.Up))
            //{
            //   sceneState.scene++;
            //   sceneState.eventState = 1;
            //   isRelease = false;
            //}
            //if (ks.IsKeyDown(Keys.Down))
            //{
            //   sceneState.scene--;
            //   sceneState.eventState = 1;
            //   isRelease = false;
            //}

            //if (ks.IsKeyDown(Keys.D0))
            //   sceneState.scene = 0;
            //if (ks.IsKeyDown(Keys.D1))
            //   sceneState.scene = 1;
            //if (ks.IsKeyDown(Keys.D2))
            //   sceneState.scene = 2;
            //if (ks.IsKeyDown(Keys.D3))
            //   sceneState.scene = 3;
            //if (ks.IsKeyDown(Keys.D4))
            //   sceneState.scene = 4;
            //if (ks.IsKeyDown(Keys.D5))
            //   sceneState.scene = 5;
            //if (ks.IsKeyDown(Keys.D6))
            //   sceneState.scene = 6;

            //if (ks.IsKeyDown(Keys.F11))
            //{
            //    gameState = GameState.CameraMode;
            //    mapTemp = map.modelName;
            //    isRelease = false;
            //}

            if (isTestSomeThing) { testSomeThing(gameTime); isTestSomeThing = false; }

         }


         if (!C.isKeyDownKeyA && !C.isKeyDownKeyD && !C.isKeyDownKeyS && !C.isKeyDownKeyW && !C.isKeyDownKeyLeftCtrl)
            
         {
            currentAnimationTest = ActionState.idle;
            testVelocity = Vector3.Zero;
            isMapChanged = false;
            isRelease = true;
            cameraStateCanChanged = true;
         }
      }
      public void UpdateNPCAndWarp(GameTime gameTime)
      {

         int npcCountInArea = 0;

         string tempNpcNearestName = string.Empty;
         //npcInarea = string.Empty;

         #region collision
         List<string> mainCharacterBounding = new List<string>(pareak.boundingBox.Keys);
         List<string> mapBounding = new List<string>(map.boundingBox.Keys);
         foreach (string indexMainCharacterBounding in mainCharacterBounding)
         {
            foreach (string indexMapBounding in mapBounding)
            {

               if (TranslateBounding(pareak.boundingBox[indexMainCharacterBounding]
                   , new Vector3(testVelocity.X, 0, 0)).Intersects
                   (map.boundingBox[indexMapBounding]))
               {
                  testVelocity.X = 0;
               }
               if (TranslateBounding(pareak.boundingBox[indexMainCharacterBounding]
                   , new Vector3(0, 0, testVelocity.Z)).Intersects
                   (map.boundingBox[indexMapBounding]))
               {
                  testVelocity.Z = 0;
               }
            }
            foreach (string npcName in npcList.Keys)
            {
               foreach (string indexNpcBounding in npcList[npcName].boundingBox.Keys)
               {
                  if (TranslateBounding(pareak.boundingBox[indexMainCharacterBounding]
                      , new Vector3(testVelocity.X, 0, 0)).Intersects
                      (npcList[npcName].boundingBox[indexNpcBounding]))
                  {
                     testVelocity.X = 0;
                  }
                  if (TranslateBounding(pareak.boundingBox[indexMainCharacterBounding]
                  , new Vector3(0, 0, testVelocity.Z)).Intersects
                  (npcList[npcName].boundingBox[indexNpcBounding]))
                  {
                     testVelocity.Z = 0;
                  }

                  if (pareak.talkingAreaCheck.Intersects
                      (npcList[npcName].boundingBox[indexNpcBounding]))
                  {
                     npcInarea[npcCountInArea] = npcName;
                     npcCountInArea++;
                     talkingIsCollision = true;
                  }

               }
            }
         }
         #endregion

         for (int i = 0; i < npcCountInArea; i++)
         {
            if (Distance(testPosition, npcList[npcInarea[i]].position) < tempNpcNearestDistance)
            {
               tempNpcNearestDistance = Distance(testPosition, npcList[npcInarea[i]].position);
               tempNpcNearestName = npcInarea[i];
            }
         }

         talkingIsCollision = false;
         distanceNearest = tempNpcNearestDistance;

         nearestNpc = tempNpcNearestName;
         tempNpcNearestDistance = 5;


         #region warpUpdate
         List<string> warpBounding;
         List<string> warpBounding2;

         switch (map.modelName)
         {
            case "Library":
               warpLibrary_TownHall.Update(gameTime);
               if (isThirdPersonCamera)
               {
                  SetCamera(CameraState.ThirdPersonCamera, gameTime, new Vector3(-17, 12, 35)
                     , new Vector3(17, -2.5f, 0), testPosition, camera);
               }
               else if (!isPlayCutScene)
               {
                  SetCamera(CameraState.Manual, gameTime, new Vector3(-17, 12, 35)
                      , new Vector3(17, -2.5f, 0), testPosition, camera);
               }
               warpBounding = new List<string>(warpLibrary_TownHall.boundingBox.Keys);
               foreach (string indexMainCharacterBounding in mainCharacterBounding)
               {
                  foreach (string indexWarpBounding in warpBounding)
                  {
                     if (pareak.boundingBox[indexMainCharacterBounding].Intersects
                         (warpLibrary_TownHall.boundingBox[indexWarpBounding]))
                     {
                        if (whatRoom == WhatRoom.None)
                           whatRoom = WhatRoom.TownHall;
                        isMapChanged = true;
                        currentAnimationTest = ActionState.idle;

                        if (whatRoom == WhatRoom.WorkingRoom)
                        {
                           if (effectWarp.animationController.IsPlaying)
                           {
                              testPosition = pareak.position;
                              effectWarp.position = pareak.position;
                              effectWarp.animationController.LoopEnabled = false;
                              effectWarp.Update(gameTime);

                           }
                           else
                           {
                              effectWarp.animationController.StartClip(models[effectWarp.modelName].AnimationClips.Values[0]);
                              SetMapTo(workingRoom, new Vector3(20, 0, 14));
                              effectWarp.animationController.LoopEnabled = true;
                              whatRoom = WhatRoom.None;
                           }

                        }
                        else if (whatRoom == WhatRoom.Library)
                        {
                           if (effectWarp.animationController.IsPlaying)
                           {
                              testPosition = pareak.position;
                              effectWarp.position = pareak.position;
                              effectWarp.animationController.LoopEnabled = false;
                              effectWarp.Update(gameTime);


                           }
                           else
                           {
                              effectWarp.animationController.StartClip(models[effectWarp.modelName].AnimationClips.Values[0]);
                              SetMapTo(library, new Vector3(-21, 0, 4));
                              effectWarp.animationController.LoopEnabled = true;
                              whatRoom = WhatRoom.None;
                           }
                        }
                        else if (whatRoom == WhatRoom.TownHall)
                        {
                           if (effectWarp.animationController.IsPlaying)
                           {
                              testPosition = pareak.position;
                              effectWarp.position = pareak.position;
                              effectWarp.Update(gameTime);
                              effectWarp.animationController.LoopEnabled = false;
                           }
                           else
                           {
                              effectWarp.animationController.StartClip(models[effectWarp.modelName].AnimationClips.Values[0]);
                              SetMapTo(townHall, new Vector3(0, 0, -44));
                              effectWarp.animationController.LoopEnabled = true;
                              whatRoom = WhatRoom.None;
                           }
                        }
                     }
                     else
                     {
                        effectWarp.position = Vector3.Zero;
                     }
                  }
               }
               break;

            case "WorkingRoom":
               warpWorkroom_TownHall.Update(gameTime);
               if (isThirdPersonCamera)
               {
                  SetCamera(CameraState.ThirdPersonCamera, gameTime, new Vector3(-17, 12, 35)
                     , new Vector3(17, -2.5f, 0), testPosition, camera);
               }
               else if (!isPlayCutScene)
               {
                  SetCamera(CameraState.Manual, gameTime, new Vector3(23, 8, 40)
                          , new Vector3(-4.5f, -20, 0), testPosition, camera);
               }
               warpBounding = new List<string>(warpWorkroom_TownHall.boundingBox.Keys);
               foreach (string indexMainCharacterBounding in mainCharacterBounding)
               {
                  foreach (string indexWarpBounding in warpBounding)
                  {
                     if (pareak.boundingBox[indexMainCharacterBounding].Intersects
                        (warpWorkroom_TownHall.boundingBox[indexWarpBounding]))
                     {
                        if (whatRoom == WhatRoom.None)
                           whatRoom = WhatRoom.TownHall;

                        isMapChanged = true;
                        currentAnimationTest = ActionState.idle;
                        if (whatRoom == WhatRoom.WorkingRoom)
                        {
                           if (effectWarp.animationController.IsPlaying)
                           {
                              testPosition = pareak.position;
                              effectWarp.position = pareak.position;
                              effectWarp.Update(gameTime);
                              effectWarp.animationController.LoopEnabled = false;
                           }
                           else
                           {
                              effectWarp.animationController.StartClip(models[effectWarp.modelName].AnimationClips.Values[0]);
                              SetMapTo(workingRoom, new Vector3(20, 0, 14));
                              effectWarp.animationController.LoopEnabled = true;
                              whatRoom = WhatRoom.None;
                           }

                        }
                        else if (whatRoom == WhatRoom.Library)
                        {
                           if (effectWarp.animationController.IsPlaying)
                           {
                              testPosition = pareak.position;
                              effectWarp.position = pareak.position;
                              effectWarp.Update(gameTime);
                              effectWarp.animationController.LoopEnabled = false;
                           }
                           else
                           {
                              effectWarp.animationController.StartClip(models[effectWarp.modelName].AnimationClips.Values[0]);
                              SetMapTo(library, new Vector3(-21, 0, 4));
                              effectWarp.animationController.LoopEnabled = true;
                              whatRoom = WhatRoom.None;
                           }
                        }
                        else if (whatRoom == WhatRoom.TownHall)
                        {
                           if (effectWarp.animationController.IsPlaying)
                           {
                              testPosition = pareak.position;
                              effectWarp.position = pareak.position;
                              effectWarp.Update(gameTime);
                              effectWarp.animationController.LoopEnabled = false;
                           }
                           else
                           {
                              effectWarp.animationController.StartClip(models[effectWarp.modelName].AnimationClips.Values[0]);
                              SetMapTo(townHall, new Vector3(0, 0, -44));
                              effectWarp.animationController.LoopEnabled = true;
                              whatRoom = WhatRoom.None;
                           }
                        }
                     }
                     else
                     {
                        effectWarp.position = Vector3.Zero;
                     }
                  }
               }
               break;
            case "TownHall":
               warpTownHall_Center.Update(gameTime);
               warpTownHall_Workroom.Update(gameTime);
               if (isThirdPersonCamera)
               {
                  SetCamera(CameraState.ThirdPersonCamera, gameTime, new Vector3(-17, 12, 35)
                     , new Vector3(17, -2.5f, 0), testPosition, camera);
               }
               else if (!isPlayCutScene)
               {
                  SetCamera(CameraState.Manual, gameTime, new Vector3(testPosition.X, 4, testPosition.Z + 25)
                                    , new Vector3(-7, -4, 0), testPosition, camera);
               }
               warpBounding = new List<string>(warpTownHall_Center.boundingBox.Keys);
               warpBounding2 = new List<string>(warpTownHall_Workroom.boundingBox.Keys);
               foreach (string indexMainCharacterBounding in mainCharacterBounding)
               {
                  foreach (string indexWarpBounding in warpBounding)// warp อันที่1
                  {
                     if (pareak.boundingBox[indexMainCharacterBounding].Intersects
                        (warpTownHall_Center.boundingBox[indexWarpBounding]))
                     {
                        isMapChanged = true;
                        currentAnimationTest = ActionState.idle;

                        //SetMapTo(center, new Vector3(0, 0, -10));
                        if (effectWarp.animationController.IsPlaying)
                        {
                           testPosition = pareak.position;
                           effectWarp.Update(gameTime);
                           effectWarp.position = pareak.position;
                           effectWarp.animationController.LoopEnabled = false;
                        }
                        else
                        {
                           effectWarp.animationController.StartClip(models[effectWarp.modelName].AnimationClips.Values[0]);
                           SetMapTo(market, new Vector3(126, 0, -140));
                           camera.cameraPosition = new Vector3(160, 13, -140);
                           effectWarp.animationController.LoopEnabled = true;
                        }
                     }
                     else
                     {
                        foreach (string indexWarpBounding2 in warpBounding2)// warp อันที่2
                        {
                           if (pareak.boundingBox[indexMainCharacterBounding].Intersects
                                  (warpTownHall_Workroom.boundingBox[indexWarpBounding]))
                           {
                              if (whatRoom == WhatRoom.None)
                                 whatRoom = WhatRoom.WorkingRoom;

                              isMapChanged = true;
                              currentAnimationTest = ActionState.idle;

                              if (whatRoom == WhatRoom.WorkingRoom)
                              {
                                 if (effectWarp.animationController.IsPlaying)
                                 {
                                    testPosition = pareak.position;
                                    effectWarp.Update(gameTime);
                                    effectWarp.position = pareak.position;
                                    effectWarp.animationController.LoopEnabled = false;
                                 }
                                 else
                                 {
                                    SetMapTo(workingRoom, new Vector3(20, 0, 14));
                                    effectWarp.animationController.StartClip(models[effectWarp.modelName].AnimationClips.Values[0]);
                                    effectWarp.animationController.LoopEnabled = true;
                                    whatRoom = WhatRoom.None;
                                 }

                              }
                              else if (whatRoom == WhatRoom.Library)
                              {
                                 if (effectWarp.animationController.IsPlaying)
                                 {
                                    testPosition = pareak.position;
                                    effectWarp.position = pareak.position;
                                    effectWarp.Update(gameTime);
                                    effectWarp.animationController.LoopEnabled = false;
                                 }
                                 else
                                 {
                                    SetMapTo(library, new Vector3(-21, 0, 4));
                                    effectWarp.animationController.StartClip(models[effectWarp.modelName].AnimationClips.Values[0]);
                                    effectWarp.animationController.LoopEnabled = true;
                                    whatRoom = WhatRoom.None;
                                 }
                              }
                              else if (whatRoom == WhatRoom.TownHall)
                              {
                                 if (effectWarp.animationController.IsPlaying)
                                 {
                                    testPosition = pareak.position;
                                    effectWarp.position = pareak.position;
                                    effectWarp.Update(gameTime);
                                    effectWarp.animationController.LoopEnabled = false;
                                 }

                                 else
                                 {
                                    SetMapTo(townHall, new Vector3(0, 0, -44));
                                    effectWarp.animationController.StartClip(models[effectWarp.modelName].AnimationClips.Values[0]);
                                    effectWarp.animationController.LoopEnabled = true;
                                    whatRoom = WhatRoom.None;
                                 }
                              }
                           }
                           else
                           {
                              effectWarp.position = Vector3.Zero;
                           }
                        }
                     }
                  }

               }
               break;
            case "Center":
               warpCenter_TownHall.Update(gameTime);
               warpCenter_Market.Update(gameTime);
               if (isThirdPersonCamera)
               {
                  SetCamera(CameraState.ThirdPersonCamera, gameTime, new Vector3(-17, 12, 35)
                     , new Vector3(17, -2.5f, 0), testPosition, camera);
               }
               else if (!isPlayCutScene)
               {
                  SetCamera(CameraState.Manual, gameTime, new Vector3(-62, 23, 11.5f)
                                     , new Vector3(12, 80, 0), testPosition, camera);
               }
               warpBounding = new List<string>(warpCenter_Market.boundingBox.Keys);
               warpBounding2 = new List<string>(warpCenter_TownHall.boundingBox.Keys);
               foreach (string indexMainCharacterBounding in mainCharacterBounding)
               {
                  foreach (string indexWarpBounding in warpBounding)// warp อันที่1
                  {
                     if (pareak.boundingBox[indexMainCharacterBounding].Intersects
                       (warpCenter_Market.boundingBox[indexWarpBounding]))
                     {
                        currentAnimationTest = ActionState.idle;
                        isMapChanged = true;
                        if (effectWarp.animationController.IsPlaying)
                        {
                           testPosition = pareak.position;
                           effectWarp.position = pareak.position;
                           effectWarp.Update(gameTime);
                           effectWarp.animationController.LoopEnabled = false;
                        }
                        else
                        {
                           effectWarp.animationController.StartClip(models[effectWarp.modelName].AnimationClips.Values[0]);
                           SetMapTo(market, new Vector3(126, 0, -140));
                           camera.cameraPosition = new Vector3(160, 13, -110);
                        }
                     }
                     else
                     {
                        foreach (string indexWarpBounding2 in warpBounding2)// warp อันที่2
                        {
                           if (pareak.boundingBox[indexMainCharacterBounding].Intersects
                             (warpCenter_TownHall.boundingBox[indexWarpBounding]))
                           {
                              currentAnimationTest = ActionState.idle;
                              isMapChanged = true;
                              if (effectWarp.animationController.IsPlaying)
                              {
                                 testPosition = pareak.position;
                                 effectWarp.position = pareak.position;
                                 effectWarp.Update(gameTime);
                                 effectWarp.animationController.LoopEnabled = false;
                              }
                              else
                              {
                                 effectWarp.animationController.StartClip(models[effectWarp.modelName].AnimationClips.Values[0]);
                                 SetMapTo(townHall, new Vector3(0, 0, 47));
                              }
                           }
                           else
                           {
                              effectWarp.position = Vector3.Zero;
                           }
                        }
                     }
                  }

               }
               break;

            case "Residence":
               warpResidence_Bedroom.Update(gameTime);
               warpResidence_Market.Update(gameTime);
               if (isThirdPersonCamera)
               {
                  SetCamera(CameraState.ThirdPersonCamera, gameTime, new Vector3(-17, 12, 35)
                     , new Vector3(17, -2.5f, 0), testPosition, camera);
               }
               else if (!isPlayCutScene)
               {
                  if (Distance(testPosition.X, camera.cameraPosition.X) > 5)
                  {
                     if (camera.cameraPosition.X < 40)
                        PlanCamera(camera, gameTime, new Vector3(testPosition.X - 5, 18, 20), Vector3.Zero,
                             new Vector3(testPosition.X, 0, -4), 1);
                     else
                        PlanCamera(camera, gameTime, camera.cameraPosition, Vector3.Zero,
                            new Vector3(45, 0, -4), 1);
                  }
                  else if (Distance(testPosition.X, camera.cameraPosition.X) < -5)
                  {
                     if (camera.cameraPosition.X > -50)
                        PlanCamera(camera, gameTime, new Vector3(testPosition.X + 5, 18, 20), Vector3.Zero,
                             new Vector3(testPosition.X, 0, -4), 1);
                     else
                        PlanCamera(camera, gameTime, camera.cameraPosition, Vector3.Zero,
                          new Vector3(-55, 0, -4), 1);
                  }
                  else
                  {
                     PlanCamera(camera, gameTime, camera.cameraPosition, Vector3.Zero,
                             new Vector3(testPosition.X, 0, -4), 1);
                  }
               }
               warpBounding = new List<string>(warpResidence_Market.boundingBox.Keys);
               warpBounding2 = new List<string>(warpResidence_Bedroom.boundingBox.Keys);
               foreach (string indexMainCharacterBounding in mainCharacterBounding)
               {
                  foreach (string indexWarpBounding in warpBounding)// warp อันที่1
                  {
                     if (pareak.boundingBox[indexMainCharacterBounding].Intersects
                         (warpResidence_Market.boundingBox[indexWarpBounding]))
                     {
                        currentAnimationTest = ActionState.idle;
                        isMapChanged = true;
                        if (effectWarp.animationController.IsPlaying)
                        {
                           testPosition = pareak.position;
                           effectWarp.position = pareak.position;
                           effectWarp.Update(gameTime);
                           effectWarp.animationController.LoopEnabled = false;
                        }
                        else
                        {
                           effectWarp.animationController.StartClip(models[effectWarp.modelName].AnimationClips.Values[0]);

                           SetMapTo(market, new Vector3(135, 0, -73));
                           camera.cameraPosition = new Vector3(testPosition.X + 25, 13, testPosition.Z);
                        }
                     }
                     else
                     {
                        foreach (string indexWarpBounding2 in warpBounding2)// warp อันที่2
                        {
                           if (pareak.boundingBox[indexMainCharacterBounding].Intersects
                               (warpResidence_Bedroom.boundingBox[indexWarpBounding]))
                           {
                              currentAnimationTest = ActionState.idle;
                              isMapChanged = true;
                              if (effectWarp.animationController.IsPlaying)
                              {
                                 testPosition = pareak.position;
                                 effectWarp.position = pareak.position;
                                 effectWarp.Update(gameTime);
                                 effectWarp.animationController.LoopEnabled = false;
                              }
                              else
                              {
                                 effectWarp.animationController.StartClip(models[effectWarp.modelName].AnimationClips.Values[0]);
                                 SetMapTo(bedroom, Vector3.Zero);
                              }
                           }
                           else
                           {
                              effectWarp.position = Vector3.Zero;
                           }
                        }
                     }
                  }
               }
               break;
            case "Bedroom":
               warpBedroom_Residence.Update(gameTime);
               if (isThirdPersonCamera)
               {
                  SetCamera(CameraState.ThirdPersonCamera, gameTime, new Vector3(-17, 12, 35)
                     , new Vector3(17, -2.5f, 0), testPosition, camera);
               }
               else if (!isPlayCutScene)
               {
                  SetCamera(CameraState.LookAt, gameTime, new Vector3(12, 7, -5)
                      , new Vector3(-170, -71 - 45, -180), Vector3.Up * 5, camera);
               }
               warpBounding = new List<string>(warpBedroom_Residence.boundingBox.Keys);
               foreach (string indexMainCharacterBounding in mainCharacterBounding)
               {
                  foreach (string indexWarpBounding in warpBounding)
                  {
                     if (pareak.boundingBox[indexMainCharacterBounding].Intersects
                       (warpBedroom_Residence.boundingBox[indexWarpBounding]))
                     {
                        currentAnimationTest = ActionState.idle;
                        isMapChanged = true;
                        if (effectWarp.animationController.IsPlaying)
                        {
                           testPosition = pareak.position;
                           effectWarp.position = pareak.position;
                           effectWarp.Update(gameTime);
                           effectWarp.animationController.LoopEnabled = false;
                        }
                        else
                        {
                           effectWarp.animationController.StartClip(models[effectWarp.modelName].AnimationClips.Values[0]);
                           SetMapTo(residence, new Vector3(20, 0, -8.5f));
                           camera.cameraPosition = new Vector3(testPosition.X, 18, 20);
                        }
                     }
                     else
                     {
                        effectWarp.position = Vector3.Zero;
                     }
                  }
               }
               break;
            case "Market":
               warpMarket_Center.Update(gameTime);
               warpMarket_Residence.Update(gameTime);
               if (isThirdPersonCamera)
               {
                  SetCamera(CameraState.ThirdPersonCamera, gameTime, new Vector3(-17, 12, 35)
                     , new Vector3(17, -2.5f, 0), testPosition, camera);
               }
               else if (!isPlayCutScene)
               {
                  if (testPosition.X > 105)
                  {
                     PlanCamera(camera, gameTime, new Vector3(160, 13, testPosition.Z),
                        new Vector3(0, -90, 0), new Vector3(107, 0, testPosition.Z), 0.5f);

                     //SetCamera(CameraState.PlanCamera, gameTime, new Vector3(160, 13, testPosition.Z),
                     //       new Vector3(0, -90, 0), new Vector3(107, 0, testPosition.Z), camera);
                  }
                  else
                  {
                     if (testPosition.X < -9)
                     {
                        SetCamera(CameraState.PlanCamera, gameTime,
                                    new Vector3(-40, testPosition.Y + 20, testPosition.Z + 30), Vector3.Zero,
                                    new Vector3(testPosition.X, testPosition.Y + 5, testPosition.Z), camera);
                     }
                     else if (testPosition.X < 90 && testPosition.X > 51 && testPosition.Z <= -42)
                     {
                        SetCamera(CameraState.PlanCamera, gameTime, new Vector3(69, 35, -90),
                           new Vector3(0, 180, 0), new Vector3(78, 0, -59), camera);
                     }
                     else if (testPosition.X < 71 && testPosition.X > 65 && testPosition.Z < -18 && testPosition.Z > -42)//ตรอกซ้าย
                     {
                        SetCamera(CameraState.PlanCamera, gameTime, new Vector3(69, 13, 10),
                           Vector3.Zero, testPosition, camera);
                     }
                     else if (testPosition.Z > -61 && testPosition.Z < -54)//ตรอกขวา
                     {

                        SetCamera(CameraState.PlanCamera, gameTime, new Vector3(testPosition.X + 30, 13, testPosition.Z),
                            new Vector3(0, -90, 0), testPosition, camera);
                     }
                     else
                     {
                        if (testPosition.X < 20)
                           SetCamera(CameraState.PlanCamera, gameTime, new Vector3(testPosition.X + 30, 12, testPosition.Z + 13)
                         , new Vector3(0, -90, 0), testPosition, camera);
                        else
                           PlanCamera(camera, gameTime, new Vector3(testPosition.X + 30, 12, testPosition.Z + 13)
                               , new Vector3(0, -90, 0), testPosition, 0.5f);
                     }
                  }
               }

               warpBounding = new List<string>(warpMarket_Center.boundingBox.Keys);
               warpBounding2 = new List<string>(warpMarket_Residence.boundingBox.Keys);
               foreach (string indexMainCharacterBounding in mainCharacterBounding)
               {
                  foreach (string indexWarpBounding in warpBounding)// warp อันที่1
                  {
                     if (pareak.boundingBox[indexMainCharacterBounding].Intersects
                      (warpMarket_Center.boundingBox[indexWarpBounding]))
                     {
                        currentAnimationTest = ActionState.idle;
                        isMapChanged = true;
                        if (effectWarp.animationController.IsPlaying)
                        {
                           testPosition = pareak.position;
                           effectWarp.position = pareak.position;
                           effectWarp.Update(gameTime);
                           effectWarp.animationController.LoopEnabled = false;
                        }
                        else
                        {
                           effectWarp.animationController.StartClip(models[effectWarp.modelName].AnimationClips.Values[0]);
                           //SetMapTo(center, new Vector3(40, 0, 13));
                           SetMapTo(townHall, new Vector3(0, 0, 47));

                        }
                     }
                     else
                     {
                        foreach (string indexWarpBounding2 in warpBounding2)// warp อันที่2
                        {
                           if (pareak.boundingBox[indexMainCharacterBounding].Intersects
                            (warpMarket_Residence.boundingBox[indexWarpBounding]))
                           {
                              currentAnimationTest = ActionState.idle;
                              isMapChanged = true;
                              if (effectWarp.animationController.IsPlaying)
                              {
                                 testPosition = pareak.position;
                                 testPosition = pareak.position;
                                 effectWarp.position = pareak.position;
                                 effectWarp.Update(gameTime);
                                 effectWarp.animationController.LoopEnabled = false;
                              }
                              else
                              {
                                 effectWarp.animationController.StartClip(models[effectWarp.modelName].AnimationClips.Values[0]);
                                 SetMapTo(residence, new Vector3(5, 0, 0));
                                 camera.cameraPosition = new Vector3(testPosition.X, 18, 20);
                              }
                           }
                           else
                           {
                              effectWarp.position = Vector3.Zero;
                           }
                        }
                     }
                  }
               }
               break;
            default:
               break;
         }
         #endregion
         //if (!isPlayCutScene)
         //{
         pareak.velocity = testVelocity;
         testPosition += testVelocity;
         pareak.talkingAreaCheck = pareak.TranslateBounding(pareak.talkingAreaCheck, testVelocity);
         //}
      }
      public void TestDraw(GameTime gameTime)
      {

         camera.SetRenderState(AIW.OS.graphicsDevice);

         AIW.OS.graphicsDevice.RenderState.AlphaBlendEnable = true;
         AIW.OS.graphicsDevice.RenderState.CullMode = CullMode.None;
         AIW.OS.graphicsDevice.RenderState.DepthBufferWriteEnable = false;
         AIW.OS.graphicsDevice.RenderState.ReferenceAlpha = 0;
         AIW.OS.graphicsDevice.RenderState.RangeFogEnable = false;
         AIW.OS.graphicsDevice.RenderState.FogEnable = false;
         skyDome.Draw(models, ActionState.back, camera, Vector3.Zero, Vector3.Zero);

         AIW.OS.graphicsDevice.RenderState.RangeFogEnable = true;
         AIW.OS.graphicsDevice.RenderState.FogEnable = true;
         AIW.OS.graphicsDevice.RenderState.FogTableMode = FogMode.Linear;
         AIW.OS.graphicsDevice.RenderState.FogVertexMode = FogMode.Linear;

         AIW.OS.graphicsDevice.RenderState.FogStart = 40;
         AIW.OS.graphicsDevice.RenderState.FogEnd = 500;
         AIW.OS.graphicsDevice.RenderState.FogColor = new Color(106, 136, 157);//(200, 200, 255);


         AIW.OS.graphicsDevice.RenderState.AlphaBlendEnable = false;
         AIW.OS.graphicsDevice.RenderState.DepthBufferWriteEnable = true;
         AIW.OS.graphicsDevice.RenderState.CullMode = CullMode.CullCounterClockwiseFace;



         switch (gameState)
         {
            case GameState.CameraMode:
               AIW.OS.graphicsDevice.RenderState.ReferenceAlpha = 10;
               map.Draw(models, ActionState.back, camera, Vector3.Zero, Vector3.Zero);
               AIW.OS.graphicsDevice.RenderState.AlphaBlendEnable = true;

               switch (map.modelName)
               {
                  case "TownHall":
                     warpTownHall_Workroom.Draw(models, ActionState.back, camera, warpTownHall_Workroom.position, Vector3.Zero);
                     warpTownHall_Center.Draw(models, ActionState.back, camera, warpTownHall_Center.position, Vector3.Zero);

                     npcList["NPCSave01"].DrawNPC(models, ActionState.back, camera, new Vector3(24, 0, 30), new Vector3(0, -90 * DEGREE2RADIAN, 0));
                     npcList["NPCFAQ01"].DrawNPC(models, ActionState.back, camera, new Vector3(-24, 0, -36), new Vector3(0, 90 * DEGREE2RADIAN, 0));

                     break;
                  case "WorkingRoom":
                     warpWorkroom_TownHall.Draw(models, ActionState.back, camera, warpWorkroom_TownHall.position, Vector3.Zero);

                     if (sceneState.scene < 6)
                        npcList["NPCLeader"].DrawNPC(models, ActionState.back, camera, new Vector3(-7, 0, 13), new Vector3(0, 90 * DEGREE2RADIAN, 0));
                     break;
                  case "Center":
                     warpCenter_Market.Draw(models, ActionState.back, camera, warpCenter_Market.position, Vector3.Zero);
                     warpCenter_TownHall.Draw(models, ActionState.back, camera, warpCenter_TownHall.position, Vector3.Zero);

                     npcList["NPCPeople01"].DrawNPC(models, ActionState.back, camera, new Vector3(66, 0, -12), Vector3.Zero);
                     npcList["NPCPeople03"].DrawNPC(models, ActionState.back, camera, new Vector3(34, 0, 12), Vector3.Zero);
                     npcList["NPCPeople01"].animationController.Time += TimeSpan.FromMilliseconds(0.1f);
                     npcList["NPCPeople03"].animationController.Time += TimeSpan.FromMilliseconds(0.5f);
                     break;
                  case "Market":
                     warpMarket_Center.Draw(models, ActionState.back, camera, warpMarket_Center.position, Vector3.Zero);
                     warpMarket_Residence.Draw(models, ActionState.back, camera, warpMarket_Residence.position, Vector3.Zero);

                     //AIW.OS.graphicsDevice.RenderState.ReferenceAlpha = 0;
                     //AIW.OS.graphicsDevice.RenderState.AlphaBlendEnable = false;

                     npcList["NPCSave01"].DrawNPC(models, ActionState.back, camera, npcList["NPCSave01"].position, npcList["NPCSave01"].rotation);
                     npcList["NPCSave02"].DrawNPC(models, ActionState.back, camera, npcList["NPCSave02"].position, npcList["NPCSave02"].rotation);
                     npcList["NPCFAQ01"].DrawNPC(models, ActionState.back, camera, npcList["NPCFAQ01"].position, npcList["NPCFAQ01"].rotation);
                     npcList["NPCFAQ02"].DrawNPC(models, ActionState.back, camera, npcList["NPCFAQ02"].position, npcList["NPCFAQ02"].rotation);
                     npcList["NPCPeople01"].DrawNPC(models, ActionState.back, camera, npcList["NPCPeople01"].position, npcList["NPCPeople01"].rotation);
                     npcList["NPCPeople02"].DrawNPC(models, ActionState.back, camera, npcList["NPCPeople02"].position, npcList["NPCPeople02"].rotation);
                     npcList["NPCPeople03"].DrawNPC(models, ActionState.back, camera, npcList["NPCPeople03"].position, npcList["NPCPeople03"].rotation);
                     npcList["NPCPeople04"].DrawNPC(models, ActionState.back, camera, npcList["NPCPeople04"].position, npcList["NPCPeople04"].rotation);
                     npcList["NPCPeople05"].DrawNPC(models, ActionState.back, camera, npcList["NPCPeople05"].position, npcList["NPCPeople05"].rotation);
                     npcList["NPCPeople06"].DrawNPC(models, ActionState.back, camera, npcList["NPCPeople06"].position, npcList["NPCPeople06"].rotation);
                     npcList["NPCPeople07"].DrawNPC(models, ActionState.back, camera, npcList["NPCPeople07"].position, npcList["NPCPeople07"].rotation);
                     npcList["NPCPeople08"].DrawNPC(models, ActionState.back, camera, npcList["NPCPeople08"].position, npcList["NPCPeople08"].rotation);
                     npcList["NPCPeople09"].DrawNPC(models, ActionState.back, camera, npcList["NPCPeople09"].position, npcList["NPCPeople09"].rotation);
                     npcList["NPCPeople10"].DrawNPC(models, ActionState.back, camera, npcList["NPCPeople10"].position, npcList["NPCPeople10"].rotation);

                     //set animation offset
                     npcList["NPCSave01"].animationController.Time += TimeSpan.FromMilliseconds(0.1f);
                     npcList["NPCSave02"].animationController.Time += TimeSpan.FromMilliseconds(0.5f);
                     npcList["NPCFAQ01"].animationController.Time += TimeSpan.FromMilliseconds(0.6f);
                     npcList["NPCFAQ02"].animationController.Time += TimeSpan.FromMilliseconds(0.8f);
                     npcList["NPCPeople01"].animationController.Time += TimeSpan.FromMilliseconds(-0.5f);
                     npcList["NPCPeople02"].animationController.Time += TimeSpan.FromMilliseconds(-0.1f);
                     npcList["NPCPeople03"].animationController.Time += TimeSpan.FromMilliseconds(-0.4f);
                     npcList["NPCPeople04"].animationController.Time += TimeSpan.FromMilliseconds(-0.3f);
                     npcList["NPCPeople05"].animationController.Time += TimeSpan.FromMilliseconds(-0.2f);
                     npcList["NPCPeople06"].animationController.Time += TimeSpan.FromMilliseconds(0.3f);
                     npcList["NPCPeople07"].animationController.Time += TimeSpan.FromMilliseconds(0.4f);
                     npcList["NPCPeople08"].animationController.Time += TimeSpan.FromMilliseconds(-0.8f);
                     npcList["NPCPeople09"].animationController.Time += TimeSpan.FromMilliseconds(-0.1f);
                     npcList["NPCPeople10"].animationController.Time += TimeSpan.FromMilliseconds(-0.6f);
                     AIW.OS.graphicsDevice.RenderState.ReferenceAlpha = 0;
                     AIW.OS.graphicsDevice.RenderState.AlphaBlendEnable = true;
                     marketWater.Draw(models, ActionState.back, camera, Vector3.Zero, Vector3.Zero);
                     break;
                  case "Residence":
                     warpResidence_Bedroom.Draw(models, ActionState.back, camera, warpResidence_Bedroom.position, Vector3.Zero);
                     warpResidence_Market.Draw(models, ActionState.back, camera, warpResidence_Market.position, Vector3.Zero);
                     AIW.OS.graphicsDevice.RenderState.ReferenceAlpha = 0;
                     AIW.OS.graphicsDevice.RenderState.AlphaBlendEnable = true;
                     residenceWater.Draw(models, ActionState.back, camera, Vector3.Zero, Vector3.Zero);
                     AIW.OS.graphicsDevice.RenderState.ReferenceAlpha = 0;
                     AIW.OS.graphicsDevice.RenderState.AlphaBlendEnable = false;

                     npcList["NPCSave01"].DrawNPC(models, ActionState.back, camera, new Vector3(-23, 0, -13), Vector3.Zero);
                     npcList["NPCFAQ01"].DrawNPC(models, ActionState.back, camera, new Vector3(52, 0, -7), Vector3.Zero);
                     break;
                  case "Bedroom":

                     warpBedroom_Residence.Draw(models, ActionState.back, camera, warpBedroom_Residence.position, Vector3.Zero);

                     break;
                  case "Library":
                     warpLibrary_TownHall.Draw(models, ActionState.back, camera, warpLibrary_TownHall.position, Vector3.Zero);

                     npcList["NPCPeople05"].DrawNPC(models, ActionState.back, camera, new Vector3(-19, 0, -7), Vector3.Zero);
                     npcList["NPCPeople07"].DrawNPC(models, ActionState.back, camera, new Vector3(5, 0, -20), Vector3.Zero);

                     npcList["NPCPeople05"].animationController.Time += TimeSpan.FromMilliseconds(-0.5f);
                     npcList["NPCPeople07"].animationController.Time += TimeSpan.FromMilliseconds(0.1f);
                     break;
               }


               if (nearestNpc != string.Empty)
               {
                  AIW.OS.graphicsDevice.RenderState.AlphaTestEnable = true;
                  AIW.OS.graphicsDevice.RenderState.AlphaBlendEnable = true;
                  AIW.OS.graphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
                  AIW.OS.graphicsDevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
                  AIW.OS.graphicsDevice.RenderState.DepthBufferEnable = true;
                  AIW.OS.graphicsDevice.RenderState.ReferenceAlpha = 0;
                  AIW.OS.graphicsDevice.RenderState.DepthBufferWriteEnable = false;
                  selectEffect.Draw(models, ActionState.back, camera, npcList[nearestNpc].position, Vector3.Zero);
                  AIW.OS.graphicsDevice.RenderState.DepthBufferWriteEnable = true;
               }

               AIW.OS.graphicsDeviceManager.GraphicsDevice.RenderState.ReferenceAlpha = 180;
               pareak.Draw(models, currentAnimationTest, camera, testPosition, testRotation);
               if (effectWarp.position != Vector3.Zero)
               {
                  AIW.OS.graphicsDevice.RenderState.CullMode = CullMode.None;
                  AIW.OS.graphicsDevice.RenderState.AlphaTestEnable = true;
                  AIW.OS.graphicsDevice.RenderState.AlphaBlendEnable = true;
                  AIW.OS.graphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
                  AIW.OS.graphicsDevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
                  AIW.OS.graphicsDevice.RenderState.DepthBufferEnable = true;
                  AIW.OS.graphicsDevice.RenderState.ReferenceAlpha = 0;
                  AIW.OS.graphicsDevice.RenderState.DepthBufferWriteEnable = false;
                  effectWarp.Draw(models, ActionState.back, camera, effectWarp.position, new Vector3(0
                                      , (float)Math.Atan2(camera.cameraPosition.X - effectWarp.position.X
                                      , camera.cameraPosition.Z - effectWarp.position.Z), 0));
                  AIW.OS.graphicsDevice.RenderState.DepthBufferWriteEnable = true;
                  AIW.OS.graphicsDevice.RenderState.CullMode = CullMode.CullCounterClockwiseFace;
               }
               DrawCutScene(sceneState, map.modelName, gameTime);
               break;
            case GameState.City:

               AIW.OS.graphicsDevice.RenderState.ReferenceAlpha = 10;
               map.Draw(models, ActionState.back, camera, Vector3.Zero, Vector3.Zero);
               AIW.OS.graphicsDevice.RenderState.AlphaBlendEnable = true;

               switch (map.modelName)
               {
                  case "TownHall":
                     warpTownHall_Workroom.Draw(models, ActionState.back, camera, warpTownHall_Workroom.position, Vector3.Zero);
                     warpTownHall_Center.Draw(models, ActionState.back, camera, warpTownHall_Center.position, Vector3.Zero);

                     npcList["NPCSave01"].DrawNPC(models, ActionState.back, camera, new Vector3(24, 0, 30), new Vector3(0, -90 * DEGREE2RADIAN, 0));
                     npcList["NPCFAQ01"].DrawNPC(models, ActionState.back, camera, new Vector3(-24, 0, -36), new Vector3(0, 90 * DEGREE2RADIAN, 0));

                     break;
                  case "WorkingRoom":
                     warpWorkroom_TownHall.Draw(models, ActionState.back, camera, warpWorkroom_TownHall.position, Vector3.Zero);

                     if (sceneState.scene < 6)
                        npcList["NPCLeader"].DrawNPC(models, ActionState.back, camera, new Vector3(-7, 0, 13), new Vector3(0, 90 * DEGREE2RADIAN, 0));
                     break;
                  case "Center":
                     warpCenter_Market.Draw(models, ActionState.back, camera, warpCenter_Market.position, Vector3.Zero);
                     warpCenter_TownHall.Draw(models, ActionState.back, camera, warpCenter_TownHall.position, Vector3.Zero);

                     npcList["NPCPeople01"].DrawNPC(models, ActionState.back, camera, new Vector3(66, 0, -12), Vector3.Zero);
                     npcList["NPCPeople03"].DrawNPC(models, ActionState.back, camera, new Vector3(34, 0, 12), Vector3.Zero);
                     npcList["NPCPeople01"].animationController.Time += TimeSpan.FromMilliseconds(0.1f);
                     npcList["NPCPeople03"].animationController.Time += TimeSpan.FromMilliseconds(0.5f);
                     break;
                  case "Market":
                     warpMarket_Center.Draw(models, ActionState.back, camera, warpMarket_Center.position, Vector3.Zero);
                     warpMarket_Residence.Draw(models, ActionState.back, camera, warpMarket_Residence.position, Vector3.Zero);

                     //AIW.OS.graphicsDevice.RenderState.ReferenceAlpha = 0;
                     //AIW.OS.graphicsDevice.RenderState.AlphaBlendEnable = false;

                     npcList["NPCSave01"].DrawNPC(models, ActionState.back, camera, npcList["NPCSave01"].position, npcList["NPCSave01"].rotation);
                     npcList["NPCSave02"].DrawNPC(models, ActionState.back, camera, npcList["NPCSave02"].position, npcList["NPCSave02"].rotation);
                     npcList["NPCFAQ01"].DrawNPC(models, ActionState.back, camera, npcList["NPCFAQ01"].position, npcList["NPCFAQ01"].rotation);
                     npcList["NPCFAQ02"].DrawNPC(models, ActionState.back, camera, npcList["NPCFAQ02"].position, npcList["NPCFAQ02"].rotation);
                     npcList["NPCPeople01"].DrawNPC(models, ActionState.back, camera, npcList["NPCPeople01"].position, npcList["NPCPeople01"].rotation);
                     npcList["NPCPeople02"].DrawNPC(models, ActionState.back, camera, npcList["NPCPeople02"].position, npcList["NPCPeople02"].rotation);
                     npcList["NPCPeople03"].DrawNPC(models, ActionState.back, camera, npcList["NPCPeople03"].position, npcList["NPCPeople03"].rotation);
                     npcList["NPCPeople04"].DrawNPC(models, ActionState.back, camera, npcList["NPCPeople04"].position, npcList["NPCPeople04"].rotation);
                     npcList["NPCPeople05"].DrawNPC(models, ActionState.back, camera, npcList["NPCPeople05"].position, npcList["NPCPeople05"].rotation);
                     npcList["NPCPeople06"].DrawNPC(models, ActionState.back, camera, npcList["NPCPeople06"].position, npcList["NPCPeople06"].rotation);
                     npcList["NPCPeople07"].DrawNPC(models, ActionState.back, camera, npcList["NPCPeople07"].position, npcList["NPCPeople07"].rotation);
                     npcList["NPCPeople08"].DrawNPC(models, ActionState.back, camera, npcList["NPCPeople08"].position, npcList["NPCPeople08"].rotation);
                     npcList["NPCPeople09"].DrawNPC(models, ActionState.back, camera, npcList["NPCPeople09"].position, npcList["NPCPeople09"].rotation);
                     npcList["NPCPeople10"].DrawNPC(models, ActionState.back, camera, npcList["NPCPeople10"].position, npcList["NPCPeople10"].rotation);

                     //set animation offset
                     npcList["NPCSave01"].animationController.Time += TimeSpan.FromMilliseconds(0.1f);
                     npcList["NPCSave02"].animationController.Time += TimeSpan.FromMilliseconds(0.5f);
                     npcList["NPCFAQ01"].animationController.Time += TimeSpan.FromMilliseconds(0.6f);
                     npcList["NPCFAQ02"].animationController.Time += TimeSpan.FromMilliseconds(0.8f);
                     npcList["NPCPeople01"].animationController.Time += TimeSpan.FromMilliseconds(-0.5f);
                     npcList["NPCPeople02"].animationController.Time += TimeSpan.FromMilliseconds(-0.1f);
                     npcList["NPCPeople03"].animationController.Time += TimeSpan.FromMilliseconds(-0.4f);
                     npcList["NPCPeople04"].animationController.Time += TimeSpan.FromMilliseconds(-0.3f);
                     npcList["NPCPeople05"].animationController.Time += TimeSpan.FromMilliseconds(-0.2f);
                     npcList["NPCPeople06"].animationController.Time += TimeSpan.FromMilliseconds(0.3f);
                     npcList["NPCPeople07"].animationController.Time += TimeSpan.FromMilliseconds(0.4f);
                     npcList["NPCPeople08"].animationController.Time += TimeSpan.FromMilliseconds(-0.8f);
                     npcList["NPCPeople09"].animationController.Time += TimeSpan.FromMilliseconds(-0.1f);
                     npcList["NPCPeople10"].animationController.Time += TimeSpan.FromMilliseconds(-0.6f);
                     AIW.OS.graphicsDevice.RenderState.ReferenceAlpha = 0;
                     AIW.OS.graphicsDevice.RenderState.AlphaBlendEnable = true;
                     marketWater.Draw(models, ActionState.back, camera, Vector3.Zero, Vector3.Zero);
                     break;
                  case "Residence":
                     warpResidence_Bedroom.Draw(models, ActionState.back, camera, warpResidence_Bedroom.position, Vector3.Zero);
                     warpResidence_Market.Draw(models, ActionState.back, camera, warpResidence_Market.position, Vector3.Zero);
                     AIW.OS.graphicsDevice.RenderState.ReferenceAlpha = 0;
                     AIW.OS.graphicsDevice.RenderState.AlphaBlendEnable = true;
                     residenceWater.Draw(models, ActionState.back, camera, Vector3.Zero, Vector3.Zero);
                     AIW.OS.graphicsDevice.RenderState.ReferenceAlpha = 0;
                     AIW.OS.graphicsDevice.RenderState.AlphaBlendEnable = false;

                     npcList["NPCSave01"].DrawNPC(models, ActionState.back, camera, new Vector3(-23, 0, -13), Vector3.Zero);
                     npcList["NPCFAQ01"].DrawNPC(models, ActionState.back, camera, new Vector3(52, 0, -7), Vector3.Zero);
                     break;
                  case "Bedroom":

                     warpBedroom_Residence.Draw(models, ActionState.back, camera, warpBedroom_Residence.position, Vector3.Zero);

                     break;
                  case "Library":
                     warpLibrary_TownHall.Draw(models, ActionState.back, camera, warpLibrary_TownHall.position, Vector3.Zero);

                     npcList["NPCPeople05"].DrawNPC(models, ActionState.back, camera, new Vector3(-19, 0, -7), Vector3.Zero);
                     npcList["NPCPeople07"].DrawNPC(models, ActionState.back, camera, new Vector3(5, 0, -20), Vector3.Zero);

                     npcList["NPCPeople05"].animationController.Time += TimeSpan.FromMilliseconds(-0.5f);
                     npcList["NPCPeople07"].animationController.Time += TimeSpan.FromMilliseconds(0.1f);
                     break;
               }


               if (nearestNpc != string.Empty)
               {
                  AIW.OS.graphicsDevice.RenderState.AlphaTestEnable = true;
                  AIW.OS.graphicsDevice.RenderState.AlphaBlendEnable = true;
                  AIW.OS.graphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
                  AIW.OS.graphicsDevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
                  AIW.OS.graphicsDevice.RenderState.DepthBufferEnable = true;
                  AIW.OS.graphicsDevice.RenderState.ReferenceAlpha = 0;
                  AIW.OS.graphicsDevice.RenderState.DepthBufferWriteEnable = false;
                  selectEffect.Draw(models, ActionState.back, camera, npcList[nearestNpc].position, Vector3.Zero);
                  AIW.OS.graphicsDevice.RenderState.DepthBufferWriteEnable = true;
               }

               AIW.OS.graphicsDeviceManager.GraphicsDevice.RenderState.ReferenceAlpha = 180;
               pareak.Draw(models, currentAnimationTest, camera, testPosition, testRotation);
               if (effectWarp.position != Vector3.Zero)
               {
                  AIW.OS.graphicsDevice.RenderState.CullMode = CullMode.None;
                  AIW.OS.graphicsDevice.RenderState.AlphaTestEnable = true;
                  AIW.OS.graphicsDevice.RenderState.AlphaBlendEnable = true;
                  AIW.OS.graphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
                  AIW.OS.graphicsDevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
                  AIW.OS.graphicsDevice.RenderState.DepthBufferEnable = true;
                  AIW.OS.graphicsDevice.RenderState.ReferenceAlpha = 0;
                  AIW.OS.graphicsDevice.RenderState.DepthBufferWriteEnable = false;
                  effectWarp.Draw(models, ActionState.back, camera, effectWarp.position, new Vector3(0
                                      , (float)Math.Atan2(camera.cameraPosition.X - effectWarp.position.X
                                      , camera.cameraPosition.Z - effectWarp.position.Z), 0));
                  AIW.OS.graphicsDevice.RenderState.DepthBufferWriteEnable = true;
                  AIW.OS.graphicsDevice.RenderState.CullMode = CullMode.CullCounterClockwiseFace;
               }
               DrawCutScene(sceneState, map.modelName, gameTime);
               break;
            case GameState.Battle:
               battle.Draw(models, ActionState.back, camera, Vector3.Zero, Vector3.Zero);

               AIW.OS.graphicsDeviceManager.GraphicsDevice.RenderState.ReferenceAlpha = 0;
               AIW.OS.graphicsDeviceManager.GraphicsDevice.RenderState.AlphaBlendEnable = true;
               AIW.OS.graphicsDevice.RenderState.DepthBufferWriteEnable = false;
               AIW.OS.graphicsDeviceManager.GraphicsDevice.RenderState.CullMode = CullMode.None;

               AreaBattleEffect.Draw(models, ActionState.back, camera, Vector3.Zero, Vector3.Zero);

               AIW.OS.graphicsDeviceManager.GraphicsDevice.RenderState.ReferenceAlpha = 10;
               AIW.OS.graphicsDeviceManager.GraphicsDevice.RenderState.AlphaBlendEnable = false;
               AIW.OS.graphicsDevice.RenderState.DepthBufferWriteEnable = true;
               AIW.OS.graphicsDeviceManager.GraphicsDevice.RenderState.CullMode = CullMode.CullCounterClockwiseFace;
               //RunAnimationTest(gameTime);

               if (sceneState.scene == 6 && sceneState.eventState == 6)
                  PlayAnimationUltimateTest(gameTime);
               else
                  PlayAnimationTest(gameTime, attacker, effectCurrent, damaged);
               break;
         }

         camera.fix3DBug();
      }

      #region Utils
      public void SetNPC(string mapName)
      {
         foreach (string index in npcList.Keys)
         {
            npcList[index].SetBoundingBox(new Vector3(-100));
            npcList[index].position.Y = -100;
         }
         switch (mapName)
         {
            case "Market":
               npcList["NPCSave01"].position = new Vector3(-40, 0, -2);
               npcList["NPCSave02"].position = new Vector3(140, 0, -104);
               npcList["NPCFAQ01"].position = new Vector3(-37, 0, 7);
               npcList["NPCFAQ02"].position = new Vector3(140, 0, -111);
               npcList["NPCPeople01"].position = new Vector3(110, 0, -105);
               npcList["NPCPeople02"].position = new Vector3(96, 0, 15);
               npcList["NPCPeople03"].position = new Vector3(89, 0, -18);
               npcList["NPCPeople04"].position = new Vector3(74, 0, 11);
               npcList["NPCPeople05"].position = new Vector3(65, 0, 13);
               npcList["NPCPeople06"].position = new Vector3(36, 0, -19);
               npcList["NPCPeople07"].position = new Vector3(32, 0, -16);
               npcList["NPCPeople08"].position = new Vector3(-8, 0, -14);
               npcList["NPCPeople09"].position = new Vector3(-15, 0, 11);
               npcList["NPCPeople10"].position = new Vector3(-24, 0, -31);

               npcList["NPCSave01"].rotation = new Vector3(0, 90 * DEGREE2RADIAN, 0);
               npcList["NPCSave02"].rotation = new Vector3(0, -90 * DEGREE2RADIAN, 0);
               npcList["NPCFAQ01"].rotation = new Vector3(0, 90 * DEGREE2RADIAN, 0);
               npcList["NPCFAQ02"].rotation = new Vector3(0, -90 * DEGREE2RADIAN, 0);
               npcList["NPCPeople01"].rotation = new Vector3(0, 90 * DEGREE2RADIAN, 0);
               npcList["NPCPeople02"].rotation = new Vector3(0, 180 * DEGREE2RADIAN, 0);
               npcList["NPCPeople03"].rotation = Vector3.Zero;
               npcList["NPCPeople04"].rotation = new Vector3(0, 180 * DEGREE2RADIAN, 0);
               npcList["NPCPeople05"].rotation = new Vector3(0, 180 * DEGREE2RADIAN, 0);
               npcList["NPCPeople06"].rotation = Vector3.Zero;
               npcList["NPCPeople07"].rotation = Vector3.Zero;
               npcList["NPCPeople08"].rotation = Vector3.Zero;
               npcList["NPCPeople09"].rotation = new Vector3(0, 180 * DEGREE2RADIAN, 0);
               npcList["NPCPeople10"].rotation = new Vector3(0, 90 * DEGREE2RADIAN, 0);
               break;
            case "TownHall":
               npcList["NPCSave01"].position = new Vector3(24, 0, 30);
               npcList["NPCFAQ01"].position = new Vector3(-24, 0, -36);
               npcList["NPCSave01"].rotation = new Vector3(0, -90 * DEGREE2RADIAN, 0);
               npcList["NPCFAQ01"].rotation = new Vector3(0, 90 * DEGREE2RADIAN, 0);
               break;
            case "WorkingRoom":
               if (sceneState.scene < 6)
               {
                  npcList["NPCLeader"].position = new Vector3(-7, 0, 13);
                  npcList["NPCLeader"].rotation = new Vector3(0, 90 * DEGREE2RADIAN, 0);
               }
               break;
            case "Center":
               npcList["NPCPeople01"].position = new Vector3(66, 0, -12);
               npcList["NPCPeople03"].position = new Vector3(34, 0, 12);
               npcList["NPCPeople01"].rotation = Vector3.Zero;
               npcList["NPCPeople03"].rotation = Vector3.Zero;
               break;
            case "Residence":
               npcList["NPCSave01"].position = new Vector3(-23, 0, -13);
               npcList["NPCFAQ01"].position = new Vector3(52, 0, -7);
               npcList["NPCSave01"].rotation = Vector3.Zero;
               npcList["NPCFAQ01"].rotation = Vector3.Zero;
               break;
            case "Library":
               npcList["NPCPeople05"].position = new Vector3(-19, 0, -7);
               npcList["NPCPeople07"].position = new Vector3(5, 0, -20);
               npcList["NPCPeople05"].rotation = Vector3.Zero;
               npcList["NPCPeople07"].rotation = Vector3.Zero;
               break;
         }
      }
      public void SetCamera(CameraState cameraState, GameTime gameTime, Vector3 cameraPosition
            , Vector3 cameraRotation, Vector3 cameraLookAt, Camera camera)
      {
         if (cameraStateCanChanged)
         {
            camera.cameraRotation = cameraRotation;
            camera.cameraState = cameraState;
         }
         if (isMapChanged)
         {
            camera.cameraRotation = cameraRotation;
            camera.cameraState = cameraState;
         }

         camera.cameraPosition = cameraPosition;
         camera.cameraTarget = cameraLookAt;
         if (gameState == GameState.CameraMode)
         {
            camera.applyWorldCameraMode(gameTime, camera.cameraPosition, camera.mouse_rotation);
         }
         else
         {
            switch (cameraState)
            {
               case CameraState.Manual:
                  camera.applyWorldManual(gameTime, camera.cameraPosition, camera.cameraRotation);
                  break;
               case CameraState.LookAt:
                  camera.applyWorldLookAt(gameTime, camera.cameraTarget, camera.cameraPosition);
                  break;
               case CameraState.PlanCamera:
                  camera.applyWorldPlanView(gameTime, camera.cameraPosition, camera.cameraTarget, camera.cameraRotation);
                  break;
               case CameraState.Rotate:
                  camera.applyWorldRotateView(gameTime, cameraLookAt, camera.cameraRotation, cameraPosition);
                  break;
               case CameraState.ThirdPersonCamera:
                  camera.applyWorldThirdPersonView(gameTime, cameraLookAt, camera.mouse_rotation);
                  break;
               default:
                  camera.applyWorldThirdView(gameTime, camera.cameraTarget, camera.mouse_rotation);
                  break;
            }
         }
      }
      public float Distance(Vector3 position1, Vector3 position2)
      {

         return (float)Math.Sqrt(Math.Pow(position1.X - position2.X, 2) + Math.Pow(position1.Y - position2.Y, 2)
             + Math.Pow(position1.Z - position2.Z, 2));
      }
      public float Distance(float position1, float position2)
      {
         return position1 - position2;
      }

      public void SetMapTo(SkinnedModelObj changeMapTo, Vector3 moveToPosition)
      {
         isThirdPersonCamera = false;
         testPosition = moveToPosition;
         pareak.CreateBoundingBox(content.Load<Model>("Model3D\\boundingCharacter"));
         pareak.TranslateBounding(new Vector3(moveToPosition.X, 0, moveToPosition.Z));
         pareak.CreateTalkingCheck(new Vector3(3, 3, 3));
         map = changeMapTo;
         isMapChanged = true;
         currentAnimationTest = ActionState.idle;
         testRotation = Vector3.Zero;
         SetNPC(map.modelName);
      }
      public BoundingBox TranslateBounding(BoundingBox bounding, Vector3 velocity)
      {
         bounding.Max += velocity;
         bounding.Min += velocity;
         return bounding;
      }
      public Vector3 Move(float XX, float YY, float cameraRotation)
      {
         Vector3 position = Vector3.Zero;
         float a = cameraRotation * DEGREE2RADIAN;
         float diffx = (XX) * position_factor;
         float diffy = (YY) * position_factor;
         float cosa = (float)Math.Cos(-a);
         float sina = (float)Math.Sin(-a);
         position.X -= diffx * cosa + diffy * sina;
         position.Z -= -diffx * sina + diffy * cosa;
         return position;
      }
      public Vector3 Move(Vector3 positionFrom, Vector3 positionTo, float speed)
      {
         Vector3 positionMove = Vector3.Zero;
         float distance = Distance(positionFrom, positionTo);
         if (distance != 0)
         {

            positionMove.X = ((positionTo.X - positionFrom.X) / distance);
            positionMove.Y = ((positionTo.Y - positionFrom.Y) / distance);
            positionMove.Z = ((positionTo.Z - positionFrom.Z) / distance);
            positionMove *= speed;

         }
         return positionMove;
      }
      public void PlanCamera(Camera camera, GameTime gameTime, Vector3 planCameraToPosition
          , Vector3 cameraRotation, Vector3 cameraLookAtTo, float speed)
      {
         if (Math.Abs(Distance(camera.cameraPosition, planCameraToPosition)) > 1)
         {
            camera.cameraPosition += Move(camera.cameraPosition, planCameraToPosition, speed);

            SetCamera(CameraState.PlanCamera, gameTime, camera.cameraPosition, cameraRotation,
                cameraLookAtTo, camera);
         }
         else
            SetCamera(CameraState.PlanCamera, gameTime, planCameraToPosition, cameraRotation,
                cameraLookAtTo, camera);
      }
      public void RotateCamera(Camera camera, GameTime gameTime, Vector3 offsetPosition
         , Vector3 rotateCameraTo, Vector3 cameraLookAtTo, float speed)
      {
         if (Math.Abs(Distance(camera.cameraRotation.Y, rotateCameraTo.Y)) > 1)
         {

            if (Math.Abs(Distance(camera.cameraPosition, cameraLookAtTo + offsetPosition)) > 1)
               camera.cameraPosition += Move(camera.cameraPosition, camera.cameraPosition + offsetPosition, speed);
            //if (camera.cameraRotation.Y < rotateCameraTo.Y)
            //    camera.cameraRotation.Y++;
            //else
            //    camera.cameraRotation.Y--;
            //camera.cameraPosition += Move(camera.cameraPosition, offsetPosition, speed);
            camera.cameraRotation += Move(camera.cameraRotation, rotateCameraTo, speed);
            //camera.cameraPosition += Move(camera.cameraPosition, offsetPosition, speed);
            SetCamera(CameraState.Rotate, gameTime, camera.cameraPosition, camera.cameraRotation, cameraLookAtTo, camera);
         }
         else
         {
            SetCamera(CameraState.Rotate, gameTime, camera.cameraPosition, camera.cameraRotation, cameraLookAtTo, camera);
         }
      }
      public void ShakeCamera(Camera camera, GameTime gameTime, Vector3 cameraPosition
         , Vector3 cameraRotation, Vector3 cameraLookAtTo, int power, TimeSpan timeStart, TimeSpan timeEnd)
      {
         if (!isGetGameTime)
         {
            tempStartTime = gameTime.TotalGameTime + timeStart;
            tempEndTime = tempStartTime + timeEnd;
            isTimeEnd = false;
            isGetGameTime = true;
         }

         camera.cameraRotation = cameraRotation;
         camera.cameraState = CameraState.PlanCamera;
         camera.cameraPosition = cameraPosition;
         camera.cameraTarget = cameraLookAtTo;

         Vector3 a = Vector3.Zero;
         random = new Random();
         a.X = random.Next(power);
         a.Y = random.Next(power);
         a.Z = random.Next(power);
         //a.X = (float)random.NextDouble();
         //a.Y = (float)random.NextDouble();
         //a.Z = (float)random.NextDouble();
         if (gameTime.TotalGameTime > tempStartTime && gameTime.TotalGameTime < tempEndTime)
         {
            if (random.NextDouble() > 0.5f)
            {
               camera.applyWorldPlanView(gameTime, camera.cameraPosition + a, camera.cameraTarget + a, camera.cameraRotation);
            }
            else
            {
               camera.applyWorldPlanView(gameTime, camera.cameraPosition - a, camera.cameraTarget - a, camera.cameraRotation);
            }
         }
         else
            SetCamera(CameraState.PlanCamera, gameTime, camera.cameraPosition, camera.cameraRotation, camera.cameraTarget,
                camera);
         //if (seed > 2)
         //    seed --;
      }
      public void ShakeCamera(Camera camera, GameTime gameTime, Vector3 cameraPosition
        , Vector3 cameraRotation, Vector3 cameraLookAtTo, int power, int frameStart, int frameEnd)
      {
         if (!isGetGameTime)
         {
            tempStartTime = gameTime.TotalGameTime + new TimeSpan(0, 0, 0, (frameStart / 30), (frameStart % 30) * (100 / 30));
            tempEndTime = tempStartTime + +new TimeSpan(0, 0, 0, (frameEnd / 30), (frameEnd % 30) * (100 / 30));
            isTimeEnd = false;
            isGetGameTime = true;
         }

         camera.cameraRotation = cameraRotation;
         camera.cameraState = CameraState.PlanCamera;
         camera.cameraPosition = cameraPosition;
         camera.cameraTarget = cameraLookAtTo;

         Vector3 a = Vector3.Zero;
         random = new Random();
         a.X = (random.Next(power));
         a.Y = (random.Next(power));
         a.Z = (random.Next(power));
         //a.X = (float)random.NextDouble();
         //a.Y = (float)random.NextDouble();
         //a.Z = (float)random.NextDouble();
         if (gameTime.TotalGameTime > tempStartTime && gameTime.TotalGameTime < tempEndTime)
         {
            if (random.NextDouble() > 0.5f)
            {
               camera.applyWorldPlanView(gameTime, camera.cameraPosition + a, camera.cameraTarget + a, camera.cameraRotation);
            }
            else
            {
               camera.applyWorldPlanView(gameTime, camera.cameraPosition - a, camera.cameraTarget - a, camera.cameraRotation);
            }
            isCameraShakeEnd = false;
         }
         else
            isCameraShakeEnd = true;

         //if (seed > 2)
         //    seed --;
      }

      #endregion
      public bool isCameraShakeEnd = false;
      public TimeSpan tempStartTime;
      public TimeSpan tempEndTime;
      public int tempAnimationStep = 0;
      public int currentSelect = 0;
      int isPlayerSide = 0;
      int maxSelectAction = 3;
      int selectActionCount = 0;
      float diffuseSelect = 1.2f;
      float diffuseNotselect = 0.8f;
      string tempSideAttack = string.Empty;
      int seed = 2;
      public Random random = new Random();
      public bool isTimeEnd = true;
      public bool isGetGameTime = false;
      #endregion

      private void HandleBattleTest(GameTime gameTime)
      {
         //SetCamera(CameraState.LookAt, gameTime, new Vector3(40, 20, 0), Vector3.Zero, Vector3.Zero, camera);
         //ks = Keyboard.GetState();
         if (!runAnimation)
         {
            if (isRelease)
            {
               //set current attack
               //if (ks.IsKeyDown(Keys.Q))
               //{
               //   SetEventAttacker("Player01");
               //   //EventKeyAttack("Player01");
               //}
               //else if (ks.IsKeyDown(Keys.W))
               //{
               //    EventKeyAttack("Player02");
               //}
               //else if (ks.IsKeyDown(Keys.E))
               //{
               //    EventKeyAttack("Player03");
               //}
               //if (ks.IsKeyDown(Keys.R))
               //{
               //   SetEventAttacker("Enemy01");
               //   //EventKeyAttack("Enemy01");
               //}
               //else if (ks.IsKeyDown(Keys.T))
               //{
               //    EventKeyAttack("Enemy02");
               //}
               //else if (ks.IsKeyDown(Keys.Y))
               //{
               //    EventKeyAttack("Enemy03");
               //}

               //set current attacked
               //if (ks.IsKeyDown(Keys.Z))
               //{
               //   SetEventDamaged("Player01");
               //   //EventKeyDamaged("Player01");
               //}
               //else if (ks.IsKeyDown(Keys.X))
               //{
               //    EventKeyDamaged("Player02");
               //}
               //else if (ks.IsKeyDown(Keys.C))
               //{
               //    EventKeyDamaged("Player03");
               //}
               //if (ks.IsKeyDown(Keys.V))
               //{
               //   SetEventDamaged("Enemy01");
               //   //EventKeyDamaged("Enemy01");
               //}
               //else if (ks.IsKeyDown(Keys.B))
               //{
               //    EventKeyDamaged("Enemy02");
               //}
               //else if (ks.IsKeyDown(Keys.N))
               //{
               //    EventKeyDamaged("Enemy03");
               //}

               //set current effect
               //if (ks.IsKeyDown(Keys.A))
               //{
               //   SetEventEffect("Life01_Effect");
               //   //EventKeyEffect("Life01_Effect");
               //}
               //else if (ks.IsKeyDown(Keys.S))
               //{
               //   SetEventEffect("Life02_Effect");
               //   //EventKeyEffect("Dark02_Effect");
               //}
               //else if (ks.IsKeyDown(Keys.D))
               //{
               //   SetEventEffect("Life03_Effect");
               //   //EventKeyEffect("Ice01_Effect");
               //}
               //else if (ks.IsKeyDown(Keys.F))
               //{
               //   SetEventEffect("Ice01_Effect");
               //   //EventKeyEffect("Ice02_Effect");
               //}
               //else if (ks.IsKeyDown(Keys.G))
               //{
               //   SetEventEffect("Ice02_Effect");
               //   //EventKeyEffect("Lighting01_Effect");
               //}
               //else if (ks.IsKeyDown(Keys.H))
               //{
               //   SetEventEffect("Ice03_Effect");
               //   //EventKeyEffect("Lighting02_Effect");
               //}
               //else if (ks.IsKeyDown(Keys.J))
               //{
               //   SetEventEffect("Fire01_Effect");
               //   //EventKeyEffect("Fire01_Effect");
               //}
               //else if (ks.IsKeyDown(Keys.K))
               //{
               //   SetEventEffect("Fire02_Effect");
               //   //EventKeyEffect("Fire01_Effect");
               //}

               //else if (ks.IsKeyDown(Keys.L))
               //{
               //   SetEventEffect("Fire03_Effect");
               //   //EventKeyEffect("Fire01_Effect");
               //}

               //else if (ks.IsKeyDown(Keys.I))
               //{
               //   SetEventEffect("Dark01_Effect");
               //   //EventKeyEffect("Fire01_Effect");
               //}

               //else if (ks.IsKeyDown(Keys.O))
               //{
               //   SetEventEffect("Dark02_Effect");
               //   //EventKeyEffect("Fire01_Effect");
               //}

               //else if (ks.IsKeyDown(Keys.P))
               //{
               //   SetEventEffect("Dark03_Effect");
               //   //EventKeyEffect("Fire01_Effect");
               //}

               //else if (ks.IsKeyDown(Keys.T))
               //{
               //   SetEventEffect("Lighting01_Effect");
               //   //EventKeyEffect("Fire01_Effect");
               //}
               //else if (ks.IsKeyDown(Keys.Y))
               //{
               //   SetEventEffect("Lighting02_Effect");
               //   //EventKeyEffect("Fire01_Effect");
               //}
               //else if (ks.IsKeyDown(Keys.U))
               //{
               //   SetEventEffect("Lighting03_Effect");
               //   //EventKeyEffect("Fire01_Effect");
               //}


               //if (ks.IsKeyDown(Keys.LeftAlt))
               //{
               //   isRelease = false;
               //   if (currentSelect < 9)
               //      currentSelect++;
               //}
               //if (ks.IsKeyDown(Keys.Space))
               //{
               //    runAnimation = true;
               //    actionStep = 0;
               //    tempAnimationStep = 0;
               //}
               if (control3D.isKeyDownKeyLeftCtrl)
               {
                  gameState = GameState.City;
                  isRelease = false;
                  control3D.isKeyDownKeyLeftCtrl = false;
               }
            }
         }
         if ( !control3D.isKeyDownKeyLeftCtrl)
         {
            isRelease = true;
         }
      }

      #region Play Animation One Round

      public void SetEventAttacker(Caster setCurrentSelect)
      {
         attacker = setCurrentSelect;
      }
      public void SetEventEffect(Spell setCurrentSelect)
      {
         effectCurrent = setCurrentSelect;
      }
      public void SetEventDamaged(Caster setCurrentSelect)
      {
         damaged = setCurrentSelect;
      }
      public void PlayAnimationTest(GameTime gameTime, Caster _attacker, Spell _effectCurrent, Caster _damaged)
      {
          if (_attacker != Caster.None && _effectCurrent != Spell.None && _damaged != Caster.None)
            runAnimation = true;
         else
            runAnimation = false;


         if (runAnimation)
         {
             AIW.Log(this, "action step : " + actionStep);
            switch (actionStep)
            {
               case 0:
                  AnimationCasting(player, _attacker, _effectCurrent, gameTime);
                  AnimationCasting(enemy, _attacker, _effectCurrent, gameTime);
                  break;
               case 1:
                  AnimationRelease(player, _attacker, _effectCurrent, gameTime);
                  AnimationRelease(enemy, _attacker, _effectCurrent, gameTime);
                  break;
               case 2:
               case 3:
                  switch (_effectCurrent)
                  {
                     case Spell.Life01_Effect:
                     case Spell.Life02_Effect:
                     case Spell.Life03_Effect:
                        AnimationBattleIdle(player, gameTime);
                        AnimationBattleIdle(enemy, gameTime);
                        break;
                     default:
                        AnimationDamaged(player, _damaged, _effectCurrent, gameTime);
                        AnimationDamaged(enemy, _damaged, _effectCurrent, gameTime);
                        break;
                  }
                  switch (_damaged)
                  {
                     case Caster.Player01:
                     /*case "Player":
                     case "Kris":
                     case "Krik":*/
                        tempSideAttack = "Player";
                        break;
                     case Caster.Enemy01:
                     /*case "Enemy":
                     case "MonsterSmall":
                     case "MonsterBig":
                     case "MonsterMedium":*/
                        tempSideAttack = "Enemy";
                        break;
                     default:
                        tempSideAttack = string.Empty;
                        break;
                  }

                  if (!effectObj[_effectCurrent.ToString()].animationController.HasFinished)
                  {
                     AIW.OS.graphicsDevice.RenderState.CullMode = CullMode.None;
                     AIW.OS.graphicsDevice.RenderState.AlphaBlendEnable = true;
                     AIW.OS.graphicsDevice.RenderState.ReferenceAlpha = 10;

                     switch (_effectCurrent)
                     {
                        case Spell.Fire01_Effect:
                           AIW.OS.graphicsDevice.RenderState.DepthBufferWriteEnable = true;
                           break;
                        default:
                           AIW.OS.graphicsDevice.RenderState.DepthBufferWriteEnable = false;
                           break;
                     }


                     effectObj[_effectCurrent.ToString()].Update(gameTime);
                     effectObj[_effectCurrent.ToString()].animationController.LoopEnabled = false;

                     if (tempSideAttack == "Player")
                     {
                        ShakeCamera(camera, gameTime, enemy["Enemy01"].position + (Vector3.Right + Vector3.Up) * 4
                                   , Vector3.Zero, player[_damaged.ToString()].position, 2, 1, 45);
                        effectObj[_effectCurrent.ToString()].Draw(models, ActionState.back, camera
                                , player[_damaged.ToString()].position, new Vector3(0
                                    , (float)Math.Atan2(camera.cameraPosition.X - player[_damaged.ToString()].position.X
                                    , camera.cameraPosition.Z - player[_damaged.ToString()].position.Z)
                                    , 0));

                     }
                     else if (tempSideAttack == "Enemy")
                     {
                        ShakeCamera(camera, gameTime, player["Player01"].position + (Vector3.Left + Vector3.Up) * 4
                                , Vector3.Zero, enemy[_damaged.ToString()].position, 2, 1, 45);
                        effectObj[_effectCurrent.ToString()].Draw(models, ActionState.back, camera
                                , enemy[_damaged.ToString()].position, new Vector3(0
                                    , (float)Math.Atan2(camera.cameraPosition.X - enemy[_damaged.ToString()].position.X
                                    , camera.cameraPosition.Z - enemy[_damaged.ToString()].position.Z)
                                   , 0));
                     }
                     AIW.OS.graphicsDevice.RenderState.CullMode = CullMode.CullCounterClockwiseFace;
                     AIW.OS.graphicsDevice.RenderState.ReferenceAlpha = 180;
                     AIW.OS.graphicsDevice.RenderState.DepthBufferWriteEnable = true;
                  }
                  else
                  {
                     actionStep = 4;
                  }
                  break;
               default:
                  effectObj[_effectCurrent.ToString()].animationController.StartClip(
                      models[effectObj[_effectCurrent.ToString()].modelName].AnimationClips.Values[0]);

                  effectObj[_effectCurrent.ToString()].animationController.LoopEnabled = true;
                  runAnimation = false;
                  effectCurrent = Spell.None;
                  attacker = Caster.None;
                  damaged = Caster.None;
                  tempSideAttack = string.Empty;

                  isGetGameTime = false;

                  break;
            }
         }
         else
         {
            SetCamera(CameraState.Manual, gameTime, new Vector3(33, 10, -10), new Vector3(3, -103, 0), Vector3.Zero, camera);
            AnimationBattleIdle(player, gameTime);
            AnimationBattleIdle(enemy, gameTime);

            tempSideAttack = string.Empty;
            tempAnimationStep = 0;
            currentSelect = 0;

            actionStep = 0;


         }

      }
      public TimeSpan tempTime;
      public void PlayAnimationUltimateTest(GameTime gameTime)
      {
         if (true)
         {
            switch (actionStep)
            {
               case 0:
                  if (Game1.bloom.Settings.BloomThreshold < 0.25f)
                  {
                     SetCamera(CameraState.Manual, gameTime, new Vector3(33, 10, -10), new Vector3(3, -103, 0), Vector3.Zero, camera);
                     Game1.bloom.Settings.BloomThreshold += 0.05f;
                     player["Player01"].Update(gameTime);
                     enemy["Enemy01"].Update(gameTime);
                     player["Player01"].Draw(models, ActionState.battleidle, camera, player["Player01"].position
                         , player["Player01"].rotation);
                     enemy["Enemy01"].Draw(models, ActionState.battleidle, camera, enemy["Enemy01"].position
                         , enemy["Enemy01"].rotation);
                  }
                  else
                  {
                     AnimationCasting(player, Caster.Enemy01, Spell.Dark02_Effect, gameTime);
                     AnimationCasting(enemy, Caster.Enemy01, Spell.Dark02_Effect, gameTime);
                  }
                  break;
               case 1:
                  tempTime = gameTime.TotalGameTime + new TimeSpan(0, 0, 2);
                  AnimationRelease(player, Caster.Enemy01, Spell.Ultimate01_Effect, gameTime);
                  AnimationRelease(enemy, Caster.Enemy01, Spell.Ultimate01_Effect, gameTime);
                  break;
               case 2:
                  AnimationBattleIdle(enemy, gameTime);
                  AnimationBattleIdle(player, gameTime);
                  tempSideAttack = "Player";
                  if (!effectObj["Ultimate01_Effect"].animationController.HasFinished)
                  {
                     AIW.OS.graphicsDevice.RenderState.CullMode = CullMode.None;
                     AIW.OS.graphicsDevice.RenderState.AlphaBlendEnable = true;
                     AIW.OS.graphicsDevice.RenderState.ReferenceAlpha = 10;
                     AIW.OS.graphicsDevice.RenderState.DepthBufferWriteEnable = true;
                     effectObj["Ultimate01_Effect"].Update(gameTime);
                     effectObj["Ultimate01_Effect"].animationController.LoopEnabled = false;

                     ShakeCamera(camera, gameTime, enemy["Enemy01"].position + (Vector3.Right + Vector3.Up) * 4
                                , Vector3.Zero, player["Player01"].position, 2, 1, 45);
                     effectObj["Ultimate01_Effect"].Draw(models, ActionState.back, camera, player["Player01"].position
                         , new Vector3(0, (float)Math.Atan2(camera.cameraPosition.X - player["Player01"].position.X
                             , camera.cameraPosition.Z - player["Player01"].position.Z), 0));
                  }
                  if (gameTime.TotalGameTime > tempTime)
                  {
                     AIW.OS.graphicsDevice.RenderState.CullMode = CullMode.None;
                     AIW.OS.graphicsDevice.RenderState.AlphaBlendEnable = true;
                     AIW.OS.graphicsDevice.RenderState.ReferenceAlpha = 10;
                     AIW.OS.graphicsDevice.RenderState.DepthBufferWriteEnable = true;
                     effectObj["Ultimate02_Effect"].Update(gameTime);
                     effectObj["Ultimate02_Effect"].animationController.LoopEnabled = false;

                     effectObj["Ultimate02_Effect"].Draw(models, ActionState.back, camera, player["Player01"].position
                         , new Vector3(0, (float)Math.Atan2(camera.cameraPosition.X - player["Player01"].position.X
                             , camera.cameraPosition.Z - player["Player01"].position.Z), 0));
                  }
                  if (effectObj["Ultimate02_Effect"].animationController.HasFinished)
                  {
                     actionStep = 3;
                  }
                  break;
               case 3:
                  if (!player["Player01"].animationController.HasFinished)
                  {
                     AnimationDeath(player, gameTime);
                     AnimationBattleIdle(enemy, gameTime);
                  }
                  else
                  {
                     actionStep = 4;
                     
                      ////////////////////////
                  }
                  break;
               default:
                  effectObj["Ultimate01_Effect"].animationController.StartClip(
                      models[effectObj["Ultimate01_Effect"].modelName].AnimationClips.Values[0]);
                  effectObj["Ultimate01_Effect"].animationController.LoopEnabled = true;

                  effectObj["Ultimate02_Effect"].animationController.StartClip(
                     models[effectObj["Ultimate02_Effect"].modelName].AnimationClips.Values[0]);
                  effectObj["Ultimate02_Effect"].animationController.LoopEnabled = true;

                  runAnimation = false;
                  effectCurrent = Spell.None;
                  attacker = Caster.None;
                  damaged = Caster.None;
                  tempSideAttack = string.Empty;
                  isGetGameTime = false;
                  actionStep = 0;
                  AnimationBattleIdle(player, gameTime);
                  gameState = GameState.City;
                  break;
            }
         }
      }
      #endregion

      #region Animation

      public void AnimationCasting(Dictionary<string, SkinnedModelObj> skinnedModel, Caster currentAttack
          , Spell currentEffect, GameTime gameTime)
      {

         Cast currentCast = Cast.None;
         switch (currentEffect)
         {
             case Spell.Dark01_Effect:
             case Spell.Dark02_Effect:
             case Spell.Dark03_Effect:
                 currentCast = Cast.Cast_Dark;
               break;
             case Spell.Fire01_Effect:
             case Spell.Fire02_Effect:
             case Spell.Fire03_Effect:
               currentCast = Cast.Cast_Fire;
               break;
             case Spell.Ice01_Effect:
             case Spell.Ice02_Effect:
             case Spell.Ice03_Effect:
               currentCast = Cast.Cast_Ice;
               break;
            case Spell.Life01_Effect:
            case Spell.Life02_Effect:
            case Spell.Life03_Effect:
               currentCast = Cast.Cast_Life;
               break;
            case Spell.Lighting01_Effect:
            case Spell.Lighting02_Effect:
            case Spell.Lighting03_Effect:
               currentCast = Cast.Cast_Lighting;
               break;
            default:
               currentCast = Cast.None;
               break;
         }
        
         foreach (string indexskinnedModel in skinnedModel.Keys)
         {
            if (currentAttack != Caster.None)
            {
                AIW.Log(this, "aaaaaa : ");
                AIW.Log(this, "indexskinnedModel : " + indexskinnedModel);
                AIW.Log(this, "currentAttack : " + currentAttack);
               if (indexskinnedModel == currentAttack.ToString())
               {
                  
                  skinnedModel[indexskinnedModel].Draw(models, ActionState.castingDrop, camera
                          , skinnedModel[indexskinnedModel].position, skinnedModel[indexskinnedModel].rotation);

                  AIW.OS.graphicsDevice.RenderState.DepthBufferWriteEnable = false;
                  AIW.OS.graphicsDevice.RenderState.AlphaBlendEnable = true;
                  AIW.OS.graphicsDevice.RenderState.ReferenceAlpha = 0;
                  AIW.OS.graphicsDevice.RenderState.CullMode = CullMode.None;

                  effectCast[currentCast.ToString()].Update(gameTime);

                  effectCast[currentCast.ToString()].Draw(models, ActionState.back, camera
                  , skinnedModel[indexskinnedModel].position, Vector3.Zero);

                  skinnedModel[indexskinnedModel].animationController.LoopEnabled = false;
                  effectCast[currentCast.ToString()].animationController.LoopEnabled = false;


                  if (indexskinnedModel == "Player01")
                  {
                     SetCamera(CameraState.Rotate, gameTime, new Vector3(0, 3, 15), new Vector3(0, 30, 0)
                                , skinnedModel[indexskinnedModel].position, camera);
                  }
                  else
                  {
                     SetCamera(CameraState.Rotate, gameTime, new Vector3(0, 3, 15), new Vector3(0, 210, 0)
                         , skinnedModel[indexskinnedModel].position, camera);

                  }
                  if (skinnedModel[indexskinnedModel].animationController.HasFinished
                      && effectCast[currentCast.ToString()].animationController.HasFinished)
                  {
                     skinnedModel[indexskinnedModel].Draw(models, ActionState.castingRelease, camera
                             , skinnedModel[indexskinnedModel].position, skinnedModel[indexskinnedModel].rotation);
                     actionStep = 1;
                     effectCast[currentCast.ToString()].animationController.LoopEnabled = true;
                     skinnedModel[indexskinnedModel].animationController.LoopEnabled = true;
                  }
               }
               else
               {
                  skinnedModel[indexskinnedModel].Draw(models, ActionState.battleidle, camera,
                      skinnedModel[indexskinnedModel].position, skinnedModel[indexskinnedModel].rotation);
               }
            }

         }
      }
      public void AnimationRelease(Dictionary<string, SkinnedModelObj> skinnedModel, Caster currentAttack
          , Spell currentEffect, GameTime gameTime)
      {
         foreach (string indexskinnedModel in skinnedModel.Keys)
         {
            if (indexskinnedModel == currentAttack.ToString())
            {
               skinnedModel[indexskinnedModel].Draw(models, ActionState.castingRelease, camera
                   , skinnedModel[indexskinnedModel].position, skinnedModel[indexskinnedModel].rotation);
               skinnedModel[indexskinnedModel].animationController.LoopEnabled = false;

               if (skinnedModel[indexskinnedModel].animationController.HasFinished)
               {
                  skinnedModel[indexskinnedModel].animationController.LoopEnabled = true;
                  camera.ResetTime();
                  actionStep = 2;
               }

            }
            else
            {
               skinnedModel[indexskinnedModel].Draw(models, ActionState.battleidle, camera,
                   skinnedModel[indexskinnedModel].position, skinnedModel[indexskinnedModel].rotation);
            }
         }
      }
      public void AnimationDamaged(Dictionary<string, SkinnedModelObj> skinnedModel, Caster currentDamaged
           , Spell currentEffect, GameTime gameTime)
      {
         foreach (string indexPlayer in skinnedModel.Keys)
         {
            if (!isCameraShakeEnd)
            {
               AnimationBattleIdle(skinnedModel, gameTime);
            }
            else
            {
               if (currentDamaged.ToString() == indexPlayer)
               {
                  if (actionStep != 3)
                  {
                     skinnedModel[indexPlayer].animationController.LoopEnabled = false;
                     if (currentEffect.ToString() == "Life01_Effect" || currentEffect.ToString() == "Life02_Effect"
                         || currentEffect.ToString() == "Life03_Effect")
                        AnimationBattleIdle(skinnedModel, gameTime);
                     else
                        skinnedModel[indexPlayer].Draw(models, ActionState.damaged, camera
                            , skinnedModel[indexPlayer].position, skinnedModel[indexPlayer].rotation);
                  }
                  else
                     AnimationBattleIdle(skinnedModel, gameTime);
                  if (skinnedModel[indexPlayer].animationController.HasFinished)
                  {
                     actionStep = 3;
                  }
               }
               else
               {
                  AnimationBattleIdle(skinnedModel, gameTime);
               }

            }
         }
      }
      public void AnimationDamagedDummy(Dictionary<string, SkinnedModelObj> skinnedModel, string currentDamaged
          , string currentEffect, GameTime gameTime)
      {
         foreach (string indexskinnedModel in skinnedModel.Keys)
         {
            if (indexskinnedModel == currentDamaged)
            {
               skinnedModel[indexskinnedModel].animationController.LoopEnabled = false;
               skinnedModel[indexskinnedModel].Draw(models, ActionState.damaged, camera,
                   skinnedModel[indexskinnedModel].position, skinnedModel[indexskinnedModel].rotation);
               if (skinnedModel[indexskinnedModel].animationController.HasFinished)
               {

                  skinnedModel[indexskinnedModel].Draw(models, ActionState.battleidle, camera,
                  skinnedModel[indexskinnedModel].position, skinnedModel[indexskinnedModel].rotation);
                  actionStep = 4;
               }
            }
            else
            {
               skinnedModel[indexskinnedModel].Draw(models, ActionState.battleidle, camera,
                   skinnedModel[indexskinnedModel].position, skinnedModel[indexskinnedModel].rotation);
            }
         }
      }
      public void AnimationBattleIdle(Dictionary<string, SkinnedModelObj> skinnedModel, GameTime gameTime)
      {
         foreach (string indexskinnedModel in skinnedModel.Keys)
         {

            skinnedModel[indexskinnedModel].animationController.LoopEnabled = true;
            skinnedModel[indexskinnedModel].Draw(models, ActionState.battleidle, camera,
                skinnedModel[indexskinnedModel].position, skinnedModel[indexskinnedModel].rotation);

         }
      }
      public void AnimationDeath(Dictionary<string, SkinnedModelObj> skinnedModel, GameTime gameTime)
      {
         foreach (string indexskinnedModel in skinnedModel.Keys)
         {

            if (skinnedModel[indexskinnedModel].oldAnimation != ActionState.standdeath)
            {
               skinnedModel[indexskinnedModel].animationController.LoopEnabled = false;
               skinnedModel[indexskinnedModel].Draw(models, ActionState.dropdeath, camera,
                   skinnedModel[indexskinnedModel].position, skinnedModel[indexskinnedModel].rotation);

            }
            else
            {
               skinnedModel[indexskinnedModel].animationController.LoopEnabled = true;
               skinnedModel[indexskinnedModel].Draw(models, ActionState.standdeath, camera,
               skinnedModel[indexskinnedModel].position, skinnedModel[indexskinnedModel].rotation);
            }

         }

      }
      #endregion

      public void UpdateCutScene(SceneState _sceneState, string mapName, GameTime gameTime)
      {
         switch (_sceneState.scene)
         {
            #region scene01
            case 1:
               switch (_sceneState.eventState)
               {
                  case 1:
                     SetCamera(CameraState.LookAt, gameTime, new Vector3(12, 7, -5)
                   , new Vector3(-170, -71 - 45, -180), Vector3.Up * 5, camera);
                     currentAnimationTest = ActionState.idle;
                     isPlayCutScene = true;
                     if (finiteState3DEngine.aiWonderState != FiniteState3DEngine.AiWonderState.PlayCutScene)
                     {

                        Kirk.Update(gameTime);
                        sceneState.eventState++;
                     }
                     break;
                  case 2:
                     isPlayCutScene = false;
                     sceneState.eventState++;
                     break;
                  case 3:
                     if (mapName == "Residence")
                     {
                        cameraStateCanChanged = true;
                        finiteState3DEngine.aiWonderState = FiniteState3DEngine.AiWonderState.PlayCutScene;
                        sceneState.eventState++;
                        pareak.position = new Vector3(20, 0, -8.5f);
                        pareak.Update(gameTime);
                        Kirk.position = pareak.position + Vector3.Left * 2;
                        Kirk.Update(gameTime);
                        testRotation.Y = (float)Math.Atan2(Kirk.position.X - pareak.position.X
                           , Kirk.position.Z - pareak.position.Z);
                        rainel.position = Kirk.position + Vector3.Left * 5;
                        rainel.Update(gameTime);
                     }
                     break;
                  case 4:
                     if (mapName == "Residence")
                     {
                        isPlayCutScene = true;
                        SetCamera(CameraState.LookAt, gameTime
                            , (Kirk.position + pareak.position) / 2 + (Vector3.Up * 6) + Vector3.Backward * 6
                            , camera.cameraRotation, ((Kirk.position + pareak.position) / 2) + Vector3.Up * 6, camera);
                        Kirk.Update(gameTime);
                        rainel.Update(gameTime);

                        if (finiteState3DEngine.aiWonderState != FiniteState3DEngine.AiWonderState.PlayCutScene)
                        {
                           camera.cameraPosition = new Vector3(testPosition.X, 18, 20);
                           sceneState.eventState++;
                        }
                     }
                     break;
                  case 5:
                     SetCamera(CameraState.LookAt, gameTime, camera.cameraPosition, camera.cameraRotation
                         , (rainel.position + pareak.position) / 2, camera);
                     Kirk.Update(gameTime);
                     rainel.Update(gameTime);
                     if (Distance(Kirk.position, warpResidence_Market.position) < 1)
                     {

                        effectWarp.Update(gameTime);
                        effectWarp.animationController.LoopEnabled = false;
                        if (effectWarp.animationController.HasFinished)
                        {
                           effectWarp.animationController.StartClip(
                               models[effectWarp.modelName].AnimationClips.Values[0]);
                           effectWarp.animationController.LoopEnabled = true;
                           sceneState.eventState++;

                           finiteState3DEngine.aiWonderState = FiniteState3DEngine.AiWonderState.PlayCutScene;
                        }
                     }
                     else
                     {
                        isPlayCutScene = true;
                        Kirk.position += Move(Kirk.position, warpResidence_Market.position, .5f);
                     }
                     break;
                  case 6:
                     SetCamera(CameraState.LookAt, gameTime, camera.cameraPosition, camera.cameraRotation
                         , (rainel.position + pareak.position) / 2, camera);
                     rainel.Update(gameTime);
                     if (finiteState3DEngine.aiWonderState != FiniteState3DEngine.AiWonderState.PlayCutScene)
                     {
                        //sceneState.eventState++;
                        sceneState.scene = 2;
                        sceneState.eventState = 1;
                        isPlayCutScene = false;
                     }

                     break;
                  default:
                     sceneState.scene++;
                     sceneState.eventState = 1;
                     isPlayCutScene = false;
                     break;

               }
               break;
            #endregion
            #region scene02
            case 2:
               switch (sceneState.eventState)
               {
                  case 1:
                     if (mapName == "Market")
                     {
                        if (Distance(pareak.position, warpMarket_Residence.position) > 7)
                        {
                           SetCamera(CameraState.Manual, gameTime, new Vector3(125, 10, -56), new Vector3(0, -90, 0)
                      , Vector3.Zero, camera);
                           sceneState.eventState++;
                           isPlayCutScene = true;
                           finiteState3DEngine.aiWonderState = FiniteState3DEngine.AiWonderState.PlayCutScene;
                           testVelocity = Vector3.Zero;
                           currentAnimationTest = ActionState.idle;
                        }

                     }
                     break;
                  case 2:///บทพูด ....
                     effectWarp.Update(gameTime);
                     effectWarp.animationController.LoopEnabled = false;
                     testVelocity = Vector3.Zero;
                     if (finiteState3DEngine.aiWonderState != FiniteState3DEngine.AiWonderState.PlayCutScene)
                     {
                        if (effectWarp.animationController.HasFinished)
                        {
                           effectWarp.animationController.StartClip(
                               models[effectWarp.modelName].AnimationClips.Values[0]);
                           effectWarp.animationController.LoopEnabled = true;
                           sceneState.eventState++;
                           isPlayCutScene = false;
                        }

                     }
                     break;
                  case 3:
                     if (mapName == "Market")
                     {
                        if (pareak.position.X < 107)
                        {
                           ///บทพูด ตูม
                           ///บทพูด !!
                           ///
                           effectObj["Fire03_Effect"].animationController.StartClip(
                               models["Fire03_Effect"].AnimationClips.Values[0]);
                           effectObj["Fire03_Effect"].Update(gameTime);
                           effectObj["Fire03_Effect"].animationController.LoopEnabled = false;
                           monsterBig.position = new Vector3(50, -7, 0);
                           monsterBig.rotation = new Vector3(0, 90 * DEGREE2RADIAN, 0);
                           sceneState.eventState++;
                        }
                     }
                     break;
                  case 4:///big mon โผล่+ วาป
                     if (mapName == "Market")
                     {
                        effectObj["Fire03_Effect"].Update(gameTime);
                        effectObj["Fire03_Effect"].animationController.LoopEnabled = false;
                        testVelocity = Vector3.Zero;
                        currentAnimationTest = ActionState.idle;
                        if (pareak.position.X < 90)
                        {
                           monsterBig.Update(gameTime);
                           effectWarp.Update(gameTime);
                           effectWarp.animationController.LoopEnabled = false;
                           if (Distance(monsterBig.position, new Vector3(50, 0, 0)) > 0.2f)
                           {
                              monsterBig.position += Move(monsterBig.position, new Vector3(50, 0, 0), .05f);
                           }

                           isPlayCutScene = true;
                           testVelocity = Vector3.Zero;
                           currentAnimationTest = ActionState.idle;
                           if (effectWarp.animationController.HasFinished)
                           {
                              effectWarp.animationController.StartClip(
                                  models[effectWarp.modelName].AnimationClips.Values[0]);
                              effectWarp.animationController.LoopEnabled = true;
                              effectWarp.Update(gameTime);
                              finiteState3DEngine.aiWonderState = FiniteState3DEngine.AiWonderState.PlayCutScene;
                              sceneState.eventState++;
                           }
                        }
                     }
                     break;
                  case 5:///บทพูด big mon
                     monsterBig.Update(gameTime);
                     testVelocity = Vector3.Zero;
                     currentAnimationTest = ActionState.idle;

                     effectObj["Fire03_Effect"].Update(gameTime);
                     if (finiteState3DEngine.aiWonderState != FiniteState3DEngine.AiWonderState.PlayCutScene)
                     {

                        monsterBig.position = enemy["Enemy01"].position;

                        effectObj["Fire03_Effect"].animationController.StartClip(
                                  models["Fire03_Effect"].AnimationClips.Values[0]);
                        effectObj["Fire03_Effect"].animationController.LoopEnabled = true;
                        effectObj["Fire03_Effect"].Update(gameTime);

                        enemy["Enemy01"] = monsterBig;
                        sceneState.eventState++;
                        //finiteState3DEngine.aiWonderState = GameFiniteState.AiWonderState.FieldMap;
                        isPlayCutScene = false;
                        gameState = GameState.Battle;
                        SetCamera(CameraState.Manual, gameTime, new Vector3(33, 10, -10), new Vector3(3, -103, 0), Vector3.Zero, camera);


                     }
                     break;
                  case 6:///ต่อสู้
                         ///
                  
                     cameraStateCanChanged = true;
                     if (gameState == GameState.City)
                     {
                        if (Game1.bloom.Settings.BloomThreshold < -3)
                        {
                           testVelocity = Vector3.Zero;
                           currentAnimationTest = ActionState.idle;
                           enemy["Enemy01"] = new SkinnedModelObj("MonsterBig"
                               , content.Load<Model>("Model3D\\boundingCharacter"), models
                               , AIW.OS.graphicsDeviceManager, new Vector3(-15, 0, 0));
                           enemy["Enemy01"].rotation = new Vector3(0, 90 * DEGREE2RADIAN, 0);
                           finiteState3DEngine.aiWonderState = FiniteState3DEngine.AiWonderState.PlayCutScene;
                           monsterBig.position = new Vector3(50, 0, 0);
                           sceneState.eventState++;
                           SetCamera(CameraState.PlanCamera, gameTime, new Vector3(testPosition.X + 30, 12, testPosition.Z + 13)
                         , new Vector3(0, -90, 0), testPosition, camera);
                           isPlayCutScene = true;
                        }
                        else
                        {
                            
                           Game1.bloom.Settings.BloomThreshold -= 0.05f;
                        }
                     }
                     break;
                  case 7:///บทพูดBig mon ฝากไว้ก่อน....
                     ///big mon หายไป
                     if (Game1.bloom.Settings.BloomThreshold < 0.25f)
                     {
                        Game1.bloom.Settings.BloomThreshold += 0.05f;
                     }
                     else
                     {
                        testVelocity = Vector3.Zero;
                        currentAnimationTest = ActionState.idle;
                        monsterBig.Update(gameTime);
                        effectWarp.Update(gameTime);
                        effectWarp.animationController.LoopEnabled = false;
                        if (Distance(monsterBig.position, new Vector3(50, -7, 0)) > 0.2f)
                        {
                           monsterBig.position += Move(monsterBig.position, new Vector3(50, -7, 0), .05f);
                        }
                        if (effectWarp.animationController.HasFinished)
                        {
                           if (finiteState3DEngine.aiWonderState != FiniteState3DEngine.AiWonderState.PlayCutScene)
                           {
                              sceneState.eventState++;
                              effectWarp.animationController.StartClip(
                              models[effectWarp.modelName].AnimationClips.Values[0]);
                              effectWarp.animationController.LoopEnabled = true;
                              isPlayCutScene = false;

                           }
                        }
                     }
                     break;
                  default:
                     sceneState.scene++;
                     sceneState.eventState = 1;

                     break;
               }
               break;
            #endregion
            #region scene03
            case 3:
               switch (sceneState.eventState)
               {
                  case 1:
                     finiteState3DEngine.aiWonderState = FiniteState3DEngine.AiWonderState.PlayCutScene;
                     sceneState.eventState++;
                     isPlayCutScene = true;
                     break;
                  case 2:
                     if (finiteState3DEngine.aiWonderState != FiniteState3DEngine.AiWonderState.PlayCutScene)
                     {
                        sceneState.eventState++;
                        isPlayCutScene = false;
                     }
                     break;
                  default:
                     sceneState.eventState = 1;
                     sceneState.scene++;
                     break;
               }
               break;
            #endregion
            #region scene04
            case 4:
               switch (_sceneState.eventState)
               {
                  case 1:
                     if (mapName == "TownHall")
                     {
                        if (testPosition.Z < -2)
                        {
                           monsterMedium.position = warpTownHall_Workroom.position + Vector3.Down * 5;
                           monsterMedium.rotation = Vector3.Zero;
                           isPlayCutScene = true;
                           testVelocity = Vector3.Zero;
                           currentAnimationTest = ActionState.idle;
                           finiteState3DEngine.aiWonderState = FiniteState3DEngine.AiWonderState.PlayCutScene;
                           sceneState.eventState++;
                        }
                     }
                     break;
                  case 2:
                     if (mapName == "TownHall")
                     {
                        monsterMedium.Update(gameTime);
                        effectWarp.Update(gameTime);
                        effectWarp.animationController.LoopEnabled = false;
                        if (Distance(monsterMedium.position, new Vector3(monsterMedium.position.X, 0
                            , monsterMedium.position.Z)) > 0.2f)
                        {
                           monsterMedium.position += Move(monsterMedium.position, new Vector3(monsterMedium.position.X, 0
                           , monsterMedium.position.Z), .05f);
                        }
                        if (effectWarp.animationController.HasFinished)
                        {
                           if (finiteState3DEngine.aiWonderState != FiniteState3DEngine.AiWonderState.PlayCutScene)
                           {
                              effectWarp.animationController.StartClip(
                                  models[effectWarp.modelName].AnimationClips.Values[0]);
                              effectWarp.animationController.LoopEnabled = true;
                              monsterMedium.position = new Vector3(-15, 0, 0);
                              monsterMedium.rotation = new Vector3(0, 90 * DEGREE2RADIAN, 0);
                              map = market;
                              map.Update(gameTime);
                              battle = market;
                              battle.Update(gameTime);
                              cameraStateCanChanged = true;
                              SetCamera(CameraState.Manual, gameTime, new Vector3(33, 10, -10), new Vector3(3, -103, 0), Vector3.Zero, camera);
                              isPlayCutScene = false;
                              enemy["Enemy01"] = monsterMedium;
                              gameState = GameState.Battle;
                              sceneState.eventState++;
                           }
                        }
                     }
                     break;
                  case 3:///ต่อสู้
                     cameraStateCanChanged = true;
                     if (gameState == GameState.City)
                     {
                        if (Game1.bloom.Settings.BloomThreshold < -3)
                        {
                           enemy["Enemy01"] = new SkinnedModelObj("MonsterBig"
                               , content.Load<Model>("Model3D\\boundingCharacter"), models
                               , AIW.OS.graphicsDeviceManager, new Vector3(-15, 0, 0));
                           enemy["Enemy01"].rotation = new Vector3(0, 90 * DEGREE2RADIAN, 0);
                           finiteState3DEngine.aiWonderState = FiniteState3DEngine.AiWonderState.PlayCutScene;
                           monsterMedium.position = warpTownHall_Workroom.position;
                           monsterMedium.rotation = Vector3.Zero;
                           testVelocity = Vector3.Zero;
                           currentAnimationTest = ActionState.idle;
                           sceneState.eventState++;
                           SetCamera(CameraState.Manual, gameTime, new Vector3(testPosition.X, 4, testPosition.Z + 25)
                                 , new Vector3(-7, -4, 0), testPosition, camera);
                           isPlayCutScene = true;
                        }
                        else
                        {
                           map = townHall;
                           map.Update(gameTime);
                           Game1.bloom.Settings.BloomThreshold -= 0.05f;
                        }
                     }
                     break;
                  case 4:
                     if (Game1.bloom.Settings.BloomThreshold < 0.25f)
                     {
                        Game1.bloom.Settings.BloomThreshold += 0.05f;
                     }
                     else
                     {
                        monsterMedium.Update(gameTime);
                        effectWarp.Update(gameTime);
                        effectWarp.animationController.LoopEnabled = false;
                        if (Distance(monsterMedium.position, new Vector3(monsterMedium.position.X, -5
                            , monsterMedium.position.Z)) > 0.2f)
                        {
                           monsterMedium.position += Move(monsterMedium.position, new Vector3(monsterMedium.position.X, -5
                           , monsterMedium.position.Z), .05f);
                        }
                        if (effectWarp.animationController.HasFinished)
                        {
                           if (finiteState3DEngine.aiWonderState != FiniteState3DEngine.AiWonderState.PlayCutScene)
                           {
                              sceneState.eventState++;
                              effectWarp.animationController.StartClip(
                              models[effectWarp.modelName].AnimationClips.Values[0]);
                              effectWarp.animationController.LoopEnabled = true;
                              isPlayCutScene = false;

                           }
                        }
                     }
                     break;
                  default:

                     sceneState.eventState = 1;
                     sceneState.scene++;
                     break;
               }
               break;
            #endregion
            #region scene05
            case 5:
               switch (_sceneState.eventState)
               {
                  case 1:
                     if (mapName == "Library")
                     {
                        monsterSmall.position = new Vector3(-8, 0, 5);
                        Kirk.position = monsterSmall.position + Vector3.Left * 4;
                        testRotation = new Vector3(0, 90 * DEGREE2RADIAN, 0);
                        sceneState.eventState++;
                        isPlayCutScene = true;
                        SetCamera(CameraState.Manual, gameTime, new Vector3(-17, 12, 35)
                            , new Vector3(17, -2.5f, 0), testPosition, camera);
                        finiteState3DEngine.aiWonderState = FiniteState3DEngine.AiWonderState.PlayCutScene;
                     }
                     break;
                  case 2:
                     if (mapName == "Library")
                     {
                        monsterSmall.Update(gameTime);
                        Kirk.Update(gameTime);
                        if (finiteState3DEngine.aiWonderState != FiniteState3DEngine.AiWonderState.PlayCutScene)
                        {
                           monsterSmall.position = new Vector3(-15, 0, 0);
                           monsterSmall.rotation = new Vector3(0, 90 * DEGREE2RADIAN, 0);
                           enemy["Enemy01"] = monsterSmall;
                           isPlayCutScene = false;
                           battle = market;
                           battle.Update(gameTime);
                           gameState = GameState.Battle;
                           SetCamera(CameraState.Manual, gameTime, new Vector3(33, 10, -10), new Vector3(3, -103, 0), Vector3.Zero, camera);
                           sceneState.eventState++;

                        }
                     }
                     break;
                  case 3:///ต่อสู้
                     cameraStateCanChanged = true;
                     if (gameState == GameState.City)
                     {
                        if (Game1.bloom.Settings.BloomThreshold < -3)
                        {
                           enemy["Enemy01"] = new SkinnedModelObj("MonsterBig"
                               , content.Load<Model>("Model3D\\boundingCharacter"), models
                               , AIW.OS.graphicsDeviceManager, new Vector3(-15, 0, 0));
                           enemy["Enemy01"].rotation = new Vector3(0, 90 * DEGREE2RADIAN, 0);
                           finiteState3DEngine.aiWonderState = FiniteState3DEngine.AiWonderState.PlayCutScene;
                           monsterSmall.position = new Vector3(-8, 0, 5);
                           monsterSmall.rotation = Vector3.Zero;

                           sceneState.eventState++;
                           SetCamera(CameraState.Manual, gameTime, new Vector3(-17, 12, 35)
                               , new Vector3(17, -2.5f, 0), testPosition, camera);
                           isPlayCutScene = true;
                        }
                        else
                        {
                           map = library;
                           map.Update(gameTime);
                           Game1.bloom.Settings.BloomThreshold -= 0.05f;
                        }
                     }
                     break;
                  case 4:
                     if (Game1.bloom.Settings.BloomThreshold < 0.25f)
                     {
                        Game1.bloom.Settings.BloomThreshold += 0.05f;
                     }
                     else
                     {
                        monsterSmall.Update(gameTime);
                        effectWarp.Update(gameTime);
                        effectWarp.animationController.LoopEnabled = false;
                        if (Distance(monsterSmall.position, new Vector3(monsterSmall.position.X, -5
                            , monsterSmall.position.Z)) > 0.2f)
                        {
                           monsterSmall.position += Move(monsterSmall.position, new Vector3(monsterSmall.position.X, -5
                           , monsterSmall.position.Z), .05f);
                        }
                        if (effectWarp.animationController.HasFinished)
                        {
                           if (finiteState3DEngine.aiWonderState != FiniteState3DEngine.AiWonderState.PlayCutScene)
                           {
                              sceneState.eventState++;
                              effectWarp.animationController.StartClip(
                              models[effectWarp.modelName].AnimationClips.Values[0]);
                              effectWarp.animationController.LoopEnabled = true;
                              isPlayCutScene = false;

                           }
                        }
                     }
                     break;
                  default:
                     sceneState.eventState = 1;
                     sceneState.scene++;

                     break;
               }
               break;
            #endregion
            #region scene06
            case 6:
               switch (_sceneState.eventState)
               {
                  case 1:
                     if (mapName == "WorkingRoom")
                     {
                        darkAngel.position = new Vector3(4, 0, 17);
                        darkAngel.rotation = new Vector3(0, -90 * DEGREE2RADIAN, 0);
                        testRotation = new Vector3(0, (float)Math.Atan2(darkAngel.position.X - testPosition.X,
                            darkAngel.position.Z - testPosition.Z), 0);
                        sceneState.eventState++;
                        isPlayCutScene = true;
                        SetCamera(CameraState.Manual, gameTime, new Vector3(23, 8, 40)
                            , new Vector3(-4.5f, -20, 0), testPosition, camera);
                        finiteState3DEngine.aiWonderState = FiniteState3DEngine.AiWonderState.PlayCutScene;
                     }
                     break;
                  case 2:
                     if (mapName == "WorkingRoom")
                     {
                        angel.Update(gameTime);
                        darkAngel.Update(gameTime);
                        if (Distance(testPosition, new Vector3(8, 0, 17)) > 0.5f)
                        {
                           testPosition += Move(testPosition, new Vector3(8, 0, 17), 0.5f);
                           testRotation = new Vector3(0, (float)Math.Atan2(darkAngel.position.X - testPosition.X,
                               darkAngel.position.Z - testPosition.Z), 0);
                           currentAnimationTest = ActionState.run;
                        }
                        else
                        {
                           currentAnimationTest = ActionState.idle;
                        }
                        if (finiteState3DEngine.aiWonderState != FiniteState3DEngine.AiWonderState.PlayCutScene)
                        {
                           isPlayCutScene = false;
                           sceneState.eventState++;
                        }
                     }
                     break;
                  case 3:
                     if (mapName == "WorkingRoom")
                     {
                        angel.Update(gameTime);
                        darkAngel.Update(gameTime);
                        if (Distance(testPosition, new Vector3(8, 0, 17)) > 0.5f)
                        {
                           testPosition += Move(testPosition, new Vector3(8, 0, 17), 0.5f);
                           testRotation = new Vector3(0, (float)Math.Atan2(darkAngel.position.X - testPosition.X,
                               darkAngel.position.Z - testPosition.Z), 0);
                        }
                        else
                        {
                           finiteState3DEngine.aiWonderState = FiniteState3DEngine.AiWonderState.PlayCutScene;
                           isPlayCutScene = true;
                           sceneState.eventState++;
                           currentAnimationTest = ActionState.idle;

                        }
                     }
                     break;
                  case 4:
                     angel.Update(gameTime);
                     darkAngel.Update(gameTime);
                     effectWarp.Update(gameTime);
                     effectWarp.animationController.LoopEnabled = false;
                     if (effectWarp.animationController.HasFinished)
                     {
                        darkAngel.rotation = new Vector3(0, 90 * DEGREE2RADIAN, 0);
                        sceneState.eventState++;
                        effectWarp.animationController.StartClip(
                        models[effectWarp.modelName].AnimationClips.Values[0]);
                        effectWarp.animationController.LoopEnabled = true;
                     }
                     break;
                  case 5:
                     angel.Update(gameTime);
                     darkAngel.Update(gameTime);
                     if (finiteState3DEngine.aiWonderState != FiniteState3DEngine.AiWonderState.PlayCutScene)
                     {
                        if (Game1.bloom.Settings.BloomThreshold < -3)
                        {
                           darkAngel.position = new Vector3(-15, 0, 0);
                           darkAngel.rotation = new Vector3(0, 90 * DEGREE2RADIAN, 0);
                           enemy["Enemy01"] = darkAngel;
                           battle.Update(gameTime);
                           map = market;
                           isPlayCutScene = false;
                           runAnimation = true;
                           gameState = GameState.Battle;
                           //finiteState3DEngine.aiWonderState = GameFiniteState.AiWonderState.PlayCutScene;
                           SetCamera(CameraState.Manual, gameTime, new Vector3(33, 10, -10), new Vector3(3, -103, 0), Vector3.Zero, camera);
                           sceneState.eventState++;
                        }
                        else
                        {
                           Game1.bloom.Settings.BloomThreshold -= 0.05f;
                        }
                     }
                     break;
                  case 6:
                     //cameraStateCanChanged = true;
                     if (gameState == GameState.City)
                     {
                        if (Game1.bloom.Settings.BloomThreshold < -3)
                        {

                           enemy["Enemy01"] = new SkinnedModelObj("MonsterBig"
                               , content.Load<Model>("Model3D\\boundingCharacter"), models
                               , AIW.OS.graphicsDeviceManager, new Vector3(-15, 0, 0));
                           enemy["Enemy01"].rotation = new Vector3(0, 90 * DEGREE2RADIAN, 0);
                           //
                           darkAngel.position = new Vector3(4, -7, 17);
                           angel.position = new Vector3(4, 0, 17);
                           angel.rotation = Vector3.Zero;
                           angel.Update(gameTime);

                           SetCamera(CameraState.LookAt, gameTime, new Vector3(23, 8, 40)
                               , new Vector3(-4.5f, -20, 0), angel.position, camera);
                           isPlayCutScene = true;
                           sceneState.eventState++;
                        }
                        else
                        {
                           SetMapTo(townHall, Vector3.Zero + Vector3.Down * 8);
                           Game1.bloom.Settings.BloomThreshold -= 0.05f;
                        }
                     }
                     break;
                  case 7:
                     if (Game1.bloom.Settings.BloomThreshold < 0.25f)
                     {
                        Game1.bloom.Settings.BloomThreshold += 0.05f;
                     }
                     else
                     {
                        effectWarp.Update(gameTime);
                        effectWarp.animationController.LoopEnabled = false;
                        sceneState.eventState++;
                        angel.Update(gameTime);
                        finiteState3DEngine.aiWonderState = FiniteState3DEngine.AiWonderState.PlayCutScene;
                     }
                     break;
                  case 8:
                     angel.Update(gameTime);
                     if (finiteState3DEngine.aiWonderState != FiniteState3DEngine.AiWonderState.PlayCutScene)
                     {
                        sceneState.eventState++;
                     }
                     break;

                  default:
                     sceneState.scene++;
                     sceneState.eventState = 1;
                     currentAnimationTest = ActionState.idle;
                     isPlayCutScene = false;
                     //finiteState3DEngine.aiWonderState = GameFiniteState.AiWonderState.FieldMap;
                     break;
               }
               break;
            #endregion
            default:
               sceneState.scene = 1;
               sceneState.eventState = 1;
               sceneState.dialogue = 1;
               Game1.bloom.Settings.BloomThreshold = 0.25f;
               SetMapTo(bedroom, Vector3.Zero);
               break;
         }
      }
      public void DrawCutScene(SceneState _sceneState, string mapName, GameTime gameTime)
      {
         switch (_sceneState.scene)
         {
            #region scene01
            case 1:
               switch (_sceneState.eventState)
               {
                  case 4:
                     Kirk.Draw(models, ActionState.idle, camera, Kirk.position, new Vector3(0
                         , (float)Math.Atan2(pareak.position.X - Kirk.position.X
                         , pareak.position.Z - Kirk.position.Z), 0));
                     rainel.Draw(models, ActionState.idle, camera, rainel.position, new Vector3(0
                         , (float)Math.Atan2(Kirk.position.X - rainel.position.X
                         , Kirk.position.Z - rainel.position.Z), 0));
                     break;
                  case 5:

                     if (Distance(Kirk.position, warpResidence_Market.position) < 1)
                     {
                        Kirk.Draw(models, ActionState.idle, camera, Kirk.position
                        , new Vector3(0, (float)Math.Atan2(warpResidence_Market.position.X - Kirk.position.X
                        , warpResidence_Market.position.Z - Kirk.position.Z), 0));
                        rainel.Draw(models, ActionState.idle, camera, rainel.position, new Vector3(0
                        , (float)Math.Atan2(Kirk.position.X - rainel.position.X
                        , Kirk.position.Z - rainel.position.Z), 0));
                        
                        AIW.OS.graphicsDevice.RenderState.CullMode = CullMode.None;
                        AIW.OS.graphicsDevice.RenderState.AlphaBlendEnable = true;
                        AIW.OS.graphicsDevice.RenderState.ReferenceAlpha = 0;
                        AIW.OS.graphicsDevice.RenderState.DepthBufferWriteEnable = false;
                        effectWarp.Draw(models, ActionState.back, camera, Kirk.position, new Vector3(0
                                                , (float)Math.Atan2(camera.cameraPosition.X - effectWarp.position.X
                                                , camera.cameraPosition.Z - effectWarp.position.Z), 0));
                        AIW.OS.graphicsDevice.RenderState.AlphaBlendEnable = false;
                        AIW.OS.graphicsDevice.RenderState.DepthBufferWriteEnable = true;
                        AIW.OS.graphicsDevice.RenderState.CullMode = CullMode.CullCounterClockwiseFace;
                     }
                     else
                     {
                        Kirk.Draw(models, ActionState.walk, camera, Kirk.position
                        , new Vector3(0, (float)Math.Atan2(warpResidence_Market.position.X - Kirk.position.X
                        , warpResidence_Market.position.Z - Kirk.position.Z), 0));
                        rainel.Draw(models, ActionState.idle, camera, rainel.position, new Vector3(0
                        , (float)Math.Atan2(Kirk.position.X - rainel.position.X
                        , Kirk.position.Z - rainel.position.Z), 0));
                     }
                     break;
                  case 6:
                     rainel.Draw(models, ActionState.idle, camera, rainel.position
                     , new Vector3(0, (float)Math.Atan2(pareak.position.X - rainel.position.X
                     , pareak.position.Z - rainel.position.Z), 0));
                     break;
               }
               break;
            #endregion
            #region scene02
            case 2:
               switch (sceneState.eventState)
               {
                  case 2:
                     AIW.OS.graphicsDevice.RenderState.CullMode = CullMode.None;
                     AIW.OS.graphicsDevice.RenderState.AlphaBlendEnable = true;
                     AIW.OS.graphicsDevice.RenderState.ReferenceAlpha = 0;
                     AIW.OS.graphicsDevice.RenderState.DepthBufferWriteEnable = false;
                     effectWarp.Draw(models, ActionState.back, camera, new Vector3(70, 0, -57), effectWarp.rotation);
                     AIW.OS.graphicsDevice.RenderState.AlphaBlendEnable = false;
                     AIW.OS.graphicsDevice.RenderState.DepthBufferWriteEnable = true;
                     AIW.OS.graphicsDevice.RenderState.CullMode = CullMode.CullCounterClockwiseFace;
                     break;
                  case 3:
                     AIW.OS.graphicsDevice.RenderState.CullMode = CullMode.None;
                     AIW.OS.graphicsDevice.RenderState.AlphaBlendEnable = true;
                     AIW.OS.graphicsDevice.RenderState.ReferenceAlpha = 0;
                     AIW.OS.graphicsDevice.RenderState.DepthBufferWriteEnable = false;
                     if (map.modelName == "Market")
                        effectObj["Fire03_Effect"].Draw(models, ActionState.back, camera, Vector3.Left * 5
                            , new Vector3(0, (float)Math.Atan2(camera.cameraPosition.X, camera.cameraPosition.Z), 0));
                     AIW.OS.graphicsDevice.RenderState.AlphaBlendEnable = false;
                     AIW.OS.graphicsDevice.RenderState.DepthBufferWriteEnable = true;
                     AIW.OS.graphicsDevice.RenderState.CullMode = CullMode.CullCounterClockwiseFace;
                     break;
                  case 4:
                     monsterBig.Draw(models, ActionState.battleidle, camera, monsterBig.position, monsterBig.rotation);

                     AIW.OS.graphicsDevice.RenderState.CullMode = CullMode.None;
                     AIW.OS.graphicsDevice.RenderState.AlphaBlendEnable = true;
                     AIW.OS.graphicsDevice.RenderState.ReferenceAlpha = 0;
                     AIW.OS.graphicsDevice.RenderState.DepthBufferWriteEnable = false;
                     if (map.modelName == "Market")
                        effectObj["Fire03_Effect"].Draw(models, ActionState.back, camera, Vector3.Left * 5
                            , new Vector3(0, (float)Math.Atan2(camera.cameraPosition.X, camera.cameraPosition.Z), 0));
                     AIW.OS.graphicsDevice.RenderState.DepthBufferWriteEnable = true;
                     effectWarp.Draw(models, ActionState.back, camera, new Vector3(monsterBig.position.X, 0
                         , monsterBig.position.Z), effectWarp.rotation);
                     AIW.OS.graphicsDevice.RenderState.AlphaBlendEnable = false;
                     AIW.OS.graphicsDevice.RenderState.DepthBufferWriteEnable = true;
                     AIW.OS.graphicsDevice.RenderState.CullMode = CullMode.CullCounterClockwiseFace;
                     break;
                  case 5:
                     monsterBig.Draw(models, ActionState.battleidle, camera, monsterBig.position, monsterBig.rotation);
                     break;

                  case 7:
                     monsterBig.Draw(models, ActionState.standdeath, camera, monsterBig.position, monsterBig.rotation);
                     AIW.OS.graphicsDevice.RenderState.CullMode = CullMode.None;
                     AIW.OS.graphicsDevice.RenderState.AlphaBlendEnable = true;
                     AIW.OS.graphicsDevice.RenderState.ReferenceAlpha = 0;
                     AIW.OS.graphicsDevice.RenderState.DepthBufferWriteEnable = false;
                     effectWarp.Draw(models, ActionState.back, camera, new Vector3(monsterBig.position.X, 0
                         , monsterBig.position.Z), effectWarp.rotation);
                     AIW.OS.graphicsDevice.RenderState.AlphaBlendEnable = false;
                     AIW.OS.graphicsDevice.RenderState.DepthBufferWriteEnable = true;
                     AIW.OS.graphicsDevice.RenderState.CullMode = CullMode.CullCounterClockwiseFace;
                     break;
               }
               break;

            #endregion
            #region scene04
            case 4:
               switch (_sceneState.eventState)
               {
                  case 2:
                     monsterMedium.Draw(models, ActionState.battleidle, camera, monsterMedium.position
                         , monsterMedium.rotation);
                     AIW.OS.graphicsDevice.RenderState.CullMode = CullMode.None;
                     AIW.OS.graphicsDevice.RenderState.AlphaBlendEnable = true;
                     AIW.OS.graphicsDevice.RenderState.ReferenceAlpha = 0;
                     AIW.OS.graphicsDevice.RenderState.DepthBufferWriteEnable = false;
                     effectWarp.Draw(models, ActionState.back, camera, new Vector3(monsterMedium.position.X, 0
                         , monsterMedium.position.Z), effectWarp.rotation);
                     AIW.OS.graphicsDevice.RenderState.AlphaBlendEnable = false;
                     AIW.OS.graphicsDevice.RenderState.DepthBufferWriteEnable = true;
                     AIW.OS.graphicsDevice.RenderState.CullMode = CullMode.CullCounterClockwiseFace;
                     break;
                  case 4:
                     monsterMedium.Draw(models, ActionState.standdeath, camera, monsterMedium.position
                         , monsterMedium.rotation);
                     AIW.OS.graphicsDevice.RenderState.CullMode = CullMode.None;
                     AIW.OS.graphicsDevice.RenderState.AlphaBlendEnable = true;
                     AIW.OS.graphicsDevice.RenderState.ReferenceAlpha = 0;
                     AIW.OS.graphicsDevice.RenderState.DepthBufferWriteEnable = false;
                     effectWarp.Draw(models, ActionState.back, camera, new Vector3(monsterMedium.position.X, 0
                         , monsterMedium.position.Z), effectWarp.rotation);
                     AIW.OS.graphicsDevice.RenderState.AlphaBlendEnable = false;
                     AIW.OS.graphicsDevice.RenderState.DepthBufferWriteEnable = true;
                     AIW.OS.graphicsDevice.RenderState.CullMode = CullMode.CullCounterClockwiseFace;
                     break;
               }
               break;
            #endregion
            #region scene05
            case 5:
               switch (_sceneState.eventState)
               {
                  case 2:
                     Kirk.Draw(models, ActionState.idle, camera, Kirk.position, new Vector3(0
                         , (float)Math.Atan2(monsterSmall.position.X - Kirk.position.X
                         , monsterSmall.position.Z - Kirk.position.Z), 0));
                     monsterSmall.Draw(models, ActionState.battleidle, camera, monsterSmall.position
                         , new Vector3(0, (float)Math.Atan2(pareak.position.X - monsterSmall.position.X
                             , pareak.position.Z - monsterSmall.position.Z), 0));
                     break;
                  case 4:
                     monsterSmall.Draw(models, ActionState.standdeath, camera, monsterSmall.position
                          , new Vector3(0, (float)Math.Atan2(pareak.position.X - monsterSmall.position.X
                              , pareak.position.Z - monsterSmall.position.Z), 0));
                     AIW.OS.graphicsDevice.RenderState.CullMode = CullMode.None;
                     AIW.OS.graphicsDevice.RenderState.AlphaBlendEnable = true;
                     AIW.OS.graphicsDevice.RenderState.ReferenceAlpha = 0;
                     AIW.OS.graphicsDevice.RenderState.DepthBufferWriteEnable = false;
                     effectWarp.Draw(models, ActionState.back, camera, new Vector3(monsterSmall.position.X, 0
                         , monsterSmall.position.Z), effectWarp.rotation);
                     AIW.OS.graphicsDevice.RenderState.AlphaBlendEnable = false;
                     AIW.OS.graphicsDevice.RenderState.DepthBufferWriteEnable = true;
                     AIW.OS.graphicsDevice.RenderState.CullMode = CullMode.CullCounterClockwiseFace;
                     break;
               }
               break;
            #endregion
            #region scene06
            case 6:
               switch (_sceneState.eventState)
               {
                  case 2:
                     darkAngel.Draw(models, ActionState.idle, camera, darkAngel.position, darkAngel.rotation);
                     break;
                  case 3:

                     darkAngel.Draw(models, ActionState.idle, camera, darkAngel.position, darkAngel.rotation);
                     break;
                  case 4:
                     darkAngel.Draw(models, ActionState.idle, camera, darkAngel.position, darkAngel.rotation);
                     AIW.OS.graphicsDevice.RenderState.CullMode = CullMode.None;
                     AIW.OS.graphicsDevice.RenderState.AlphaBlendEnable = true;
                     AIW.OS.graphicsDevice.RenderState.ReferenceAlpha = 0;
                     AIW.OS.graphicsDevice.RenderState.DepthBufferWriteEnable = false;
                     effectWarp.Draw(models, ActionState.back, camera, darkAngel.position + Vector3.Left * 5, effectWarp.rotation);
                     AIW.OS.graphicsDevice.RenderState.AlphaBlendEnable = false;
                     AIW.OS.graphicsDevice.RenderState.DepthBufferWriteEnable = true;
                     AIW.OS.graphicsDevice.RenderState.CullMode = CullMode.CullCounterClockwiseFace;
                     break;
                  case 5:
                     darkAngel.Draw(models, ActionState.idle, camera, darkAngel.position, darkAngel.rotation);
                     break;

                  case 7:
                     //darkAngel.Draw(models, ActionState.idle, camera, darkAngel.position, darkAngel.rotation);
                     angel.Draw(models, ActionState.idle, camera, angel.position, angel.rotation);
                     //AIW.OS.graphicsDevice.RenderState.AlphaBlendEnable = true;
                     //AIW.OS.graphicsDevice.RenderState.ReferenceAlpha = 0;
                     //AIW.OS.graphicsDevice.RenderState.DepthBufferWriteEnable = false;
                     //effectWarp.Draw(models, ActionState.back, camera, new Vector3(darkAngel.position.X, 0
                     //    , darkAngel.position.Z), effectWarp.rotation);
                     //AIW.OS.graphicsDevice.RenderState.AlphaBlendEnable = false;
                     //AIW.OS.graphicsDevice.RenderState.DepthBufferWriteEnable = true;
                     //AIW.OS.graphicsDevice.RenderState.CullMode = CullMode.CullCounterClockwiseFace;
                     break;
                  case 8:
                     angel.Draw(models, ActionState.idle, camera, angel.position, angel.rotation);
                     break;
               }
               break;
            #endregion
         }
      }
   }
}
