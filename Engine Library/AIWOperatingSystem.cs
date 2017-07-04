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
    static class AIW
    {
        static AIWOperatingSystem instanceOfAIWOperatingSystem;

        public static void setAIWOperatingSystem(GraphicsDeviceManager graphicsDeviceManager,
            Game game)
        {
            if (instanceOfAIWOperatingSystem == null)
            {
                instanceOfAIWOperatingSystem = new AIWOperatingSystem(
                    graphicsDeviceManager, game);
            }
        }
        public static AIWOperatingSystem OS
        {
            get
            {
                return instanceOfAIWOperatingSystem;
            }
        }

        #region Log
        static void Log(string text)
        {
            Console.WriteLine(text);
        }

        public static void Log(Object tmp, string text)
        {
            Log("[" + tmp.ToString().Remove(0, 13) + "]" + " : " + text);
        }
        public static void SystemLog(Object tmp, string text)
        {
            Log("[" + tmp.ToString().Remove(0, 13) + "]" + " : " + text);
        }
        public static void DebugLog(Object tmp, string text)
        {
            Log("[" + tmp.ToString().Remove(0, 13) + "]" + " : " + text);
        }
        #endregion

        #region Screen

        //Screen Attribute
        const int screenWidth = 1024;
        const int screenHeight = 768;

        public static int ScreenWidth { get { return screenWidth; } }
        public static int ScreenHeight { get { return screenHeight; } }
        public static int ScreenWidthOver2 { get { return screenWidth / 2; } }
        public static int ScreenHeightOver2 { get { return screenHeight / 2; } }
        public static Vector2 ScreenCenter { get { return new Vector2(screenWidth / 2, screenHeight / 2); } }
        public static Vector2 ScreenSize { get { return new Vector2(screenWidth, screenHeight); } }
        public static Vector2 ScreenPosByRatio(float ratioX, float ratioY)
        {
            return new Vector2(ratioX * ScreenWidth, ratioY * ScreenHeight);
        }
        public static Vector2 ScreenPosByRatio(double ratioX, double ratioY)
        {
            return new Vector2((float)ratioX * ScreenWidth, (float)ratioY * ScreenHeight);
        }
        public static Vector2 ScreenPosByRatio(float ratioX, float ratioY, float offsetX, float offsetY)
        {
            return new Vector2(ratioX * ScreenWidth + offsetX, ratioY * ScreenHeight + offsetY);
        }
        public static Vector2 RatioByScreenPos(float posX, float posY)
        {
            return new Vector2(posX / ScreenWidth, posY / ScreenHeight);
        }

        public static bool changeScene(string nextScene)
        {
            return AIW.OS.SceneManager.SceneStock.ChangeScene(nextScene);
        }

        #endregion

        //Texture2D
        #region Texture2D
        public static float TextureWidth(string name)
        {
            return OS.Graphic2DEngine.Texture2DStock.TextureWidth(name);
        }
        public static float TextureHeight(string name)
        {
            return OS.Graphic2DEngine.Texture2DStock.TextureHeight(name);
        }
        public static Vector2 TextureOrigin(string name)
        {
            return OS.Graphic2DEngine.Texture2DStock.TextureOrigin(name);
        }
        public static void addTexture2D(string name)
        {
            OS.Graphic2DEngine.Texture2DStock.AddTexture2D(name);
        }
        public static void addTexture2D(string folderName, string name)
        {
            OS.Graphic2DEngine.Texture2DStock.AddTexture2D(folderName, name);
        }
        public static void addTexture2D(string folderName, string subFolder, string name)
        {
            OS.Graphic2DEngine.Texture2DStock.AddTexture2D(folderName, subFolder, name);
        }
        #endregion

        //depth
        public static void resetDepth()
        {
            OS.Graphic2DEngine.DepthManager.ResetDepth();
        }
        public static float z
        { get { return OS.Graphic2DEngine.DepthManager.RequestDepth(); } }


        //input
        public static InputState Input
        { get { return OS.InputState; } }

        //random number
        public static int seedIndex = 0;
        public static string seedString = "23450872987245097121231098734250985612340987";
        public static int arc4random(int seed)
        {
            seedIndex++;
            if (seedIndex >= seedString.Length)
                seedIndex = 0;
            seed += (int)Convert.ToDouble(seedString.Substring(seedIndex, 1));
            Random rand = new Random(DateTime.Now.Millisecond + seed + System.Environment.TickCount);
            return rand.Next(DateTime.Now.Millisecond);

        }
        public static int arc4random()
        {
            int seed = 0;
            seedIndex++;
            if (seedIndex >= seedString.Length)
                seedIndex = 0;
            seed += (int)Convert.ToDouble(seedString.Substring(seedIndex, 1));
            Random rand = new Random(DateTime.Now.Millisecond + seed + System.Environment.TickCount);
            return rand.Next(DateTime.Now.Millisecond);

        }
    }
    public class AIWOperatingSystem : DrawableGameComponent
    {
        #region Model
        #region Graphic Model
        public GraphicsDeviceManager graphicsDeviceManager;
        public GraphicsDevice graphicsDevice;
        SpriteBatch spriteBatch;
        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }
        Graphic2DEngine graphic2DEngine;
        public Graphic2DEngine Graphic2DEngine
        {
            get { return graphic2DEngine; }
        }

        public Draw3DManager graphic3DEngine;
        public Draw3DManager Graphic3DEngine { get { return graphic3DEngine; } }

        public bool isShowWallpaper = true;
        public bool isShowGameInfo = false;
        const bool isUse3DEngine_ = true;
        public bool isUse3DEngine { get { return isUse3DEngine_; } }
        #endregion

        public SceneManager SceneManager;
    
        InputState inputState;
        public InputState InputState { get { return inputState; } }
        GraphicHelper graphicHelper;
        public GraphicHelper GraphicHelper { get { return graphicHelper; } }
        FPS fps;
        public FPS FPS { get { return fps; } }

        #endregion


        #region Initialize
        bool isInitialize_ = false;
        public AIWOperatingSystem(GraphicsDeviceManager _graphicsDeviceManager,
            Game _game)
            : base(_game)
        {
            this.graphicsDeviceManager = _graphicsDeviceManager;
            SceneManager = new SceneManager();
            if (isUse3DEngine_) { initGraphic3DEngine(); }
            
        }
        void initSceneManager()
        {

            SceneManager.LoadContent();
        }
        void initGraphic2DEngine()
        {
            graphic2DEngine = new Graphic2DEngine();
        }
        void initGraphic3DEngine()
        {
            graphic3DEngine = new Draw3DManager();

        }
        void initInputState()
        {
            inputState = new InputState();
        }
        void initGraphicHelper()
        {
            graphicHelper = new GraphicHelper();
        }
        void initFPS() { fps = new FPS(); }
        void initDirector()
        {
            //TestScene scene = new TestScene();
            //BattleScene scene = new BattleScene();
            //ViewParticleScene scene = new ViewParticleScene();
            DisclaimerPage scene = new DisclaimerPage();
            //FieldMap scene = new FieldMap();
            Director.sharedDirector.pushScene(scene);
        }
        void loadContentSoundEngine()
        {
            SoundEngine.sharedSoundEngine.LoadContent();
        }
        protected override void LoadContent()
        {
            base.LoadContent();
            graphicsDevice = graphicsDeviceManager.GraphicsDevice;

            spriteBatch = new SpriteBatch(GraphicsDevice);
            initGraphic2DEngine();
            initSceneManager();
            initInputState();
            initGraphicHelper();
            initFPS();
            if (isUse3DEngine_)
            {
                graphic3DEngine.LoadContent();
            }
            initDirector();
            loadContentSoundEngine();

            AIW.SystemLog(this, "load content");
            isInitialize_ = true;
          

        }
        #endregion


        #region Controller

        void checkInputExitGame()
        {
            if (inputState.isKeyDown(Keys.Escape))
            {
                Game.Exit();
            }
        }
        void checkInputShowGameInfo()
        {
            if (inputState.isKeyDown(Keys.Tab))
            {
                isShowGameInfo = true;
            }
            else isShowGameInfo = false;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (!isInitialize_) return;

            SoundEngine.sharedSoundEngine.Update(gameTime);
            inputState.Update();
            checkInputExitGame();
            SceneManager.HandleInput(gameTime);
            SceneManager.Update(gameTime);
            Director.sharedDirector.update(gameTime);
            if (isUse3DEngine_)
            {
                graphic3DEngine.Update(gameTime);
            }
            checkInputShowGameInfo();
        }
        #endregion


        #region View
        void DrawOSBackground()
        {
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.BackToFront, SaveStateMode.None);
            graphic2DEngine.Old_Sprite2D("moonBG",
            AIW.ScreenCenter, AIW.TextureOrigin("moonBG"),
            0, Vector2.One, Color.White, 1, AIW.z);
            spriteBatch.End();
        }
        void drawGameInfo(GameTime gameTime)
        {
            fps.DisplayFPS(gameTime);
            if (isShowGameInfo)
                graphicHelper.DrawAllGridInfo();
            //graphicHelper.PlaceSprite2D("squareBtn1");
        }
        void draw3DEngineInfo()
        {
            int yOffset = -10;
          
                graphic2DEngine.Old_Label(kFont.Font, "NPC :" + graphic3DEngine.nearestNpc,
                new Vector2(10, AIW.ScreenHeight - 20+yOffset), 0,
                1, Color.White, 1,
                  AIW.OS.Graphic2DEngine.DepthManager.RequestDepthDeveloper());
                yOffset -= 10;


                graphic2DEngine.Old_Label(kFont.Font, "Map : " + graphic3DEngine.map.modelName,
                new Vector2(10, AIW.ScreenHeight - 20 + yOffset), 0,
                1, Color.White, 1,
                  AIW.OS.Graphic2DEngine.DepthManager.RequestDepthDeveloper());
                yOffset -= 10;

                graphic2DEngine.Old_Label(kFont.Font, "whatRoom : " + graphic3DEngine.whatRoom.ToString(),
                    new Vector2(10, AIW.ScreenHeight - 20 + yOffset), 0,
                    1, Color.White, 1,
                      AIW.OS.Graphic2DEngine.DepthManager.RequestDepthDeveloper());
                yOffset -= 10;

                graphic2DEngine.Old_Label(kFont.Font, "scene : " + graphic3DEngine.sceneState.scene,
                       new Vector2(10, AIW.ScreenHeight - 20 + yOffset), 0,
                       1, Color.White, 1,
                         AIW.OS.Graphic2DEngine.DepthManager.RequestDepthDeveloper());
                yOffset -= 10;

                graphic2DEngine.Old_Label(kFont.Font, "eventState : " + graphic3DEngine.sceneState.eventState,
                         new Vector2(10, AIW.ScreenHeight - 20 + yOffset), 0,
                         1, Color.White, 1,
                           AIW.OS.Graphic2DEngine.DepthManager.RequestDepthDeveloper());
                yOffset -= 10;

                graphic2DEngine.Old_Label(kFont.Font, "dialogue : " + graphic3DEngine.sceneState.dialogue,
                           new Vector2(10, AIW.ScreenHeight - 20 + yOffset), 0,
                           1, Color.White, 1,
                             AIW.OS.Graphic2DEngine.DepthManager.RequestDepthDeveloper());
                yOffset -= 10;

                graphic2DEngine.Old_Label(kFont.Font, "isPlayCutScene : " + graphic3DEngine.isPlayCutScene,
                              new Vector2(10, AIW.ScreenHeight - 20 + yOffset), 0,
                              1, Color.White, 1,
                                AIW.OS.Graphic2DEngine.DepthManager.RequestDepthDeveloper());
                yOffset -= 10;

                graphic2DEngine.Old_Label(kFont.Font, "FiniteState : " + graphic3DEngine.finiteState3DEngine.aiWonderState.ToString(),
                              new Vector2(10, AIW.ScreenHeight - 20 + yOffset), 0,
                              1, Color.White, 1,
                                AIW.OS.Graphic2DEngine.DepthManager.RequestDepthDeveloper());
                yOffset -= 10;

                graphic2DEngine.Old_Label(kFont.Font, "Player pos : (" + graphic3DEngine.pareak.position.X+","+
             graphic3DEngine.pareak.position.Y+","+graphic3DEngine.pareak.position.Z+")",
                              new Vector2(10, AIW.ScreenHeight - 20 + yOffset), 0,
                              1, Color.White, 1,
                                AIW.OS.Graphic2DEngine.DepthManager.RequestDepthDeveloper());

        }
        void drawMouseCursor()
        {
            string filename = "mouseCursor";
            Vector2 position = inputState.MousePosition;
            float rotation = MathHelper.PiOver2 - MathHelper.PiOver4 / 2;
            graphic2DEngine.Old_Sprite2D(filename,
          position, new Vector2(7, 50),
           rotation, Vector2.One, Color.White, 1, AIW.z);
        }
        void drawScene(GameTime gt)
        {
            //SceneManager.Draw(gt);
            if (inputState.isKeyPressed(Keys.F1))
            {
                Director.sharedDirector.pauseOrResume();
            }                
            Director.sharedDirector.draw(gt);
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            if (!isInitialize_) return;
            AIW.resetDepth();
            GraphicsDevice.Clear(Color.Black);
            if (isShowWallpaper) DrawOSBackground();

            if (isUse3DEngine_)
            {
                graphic3DEngine.TestDraw(gameTime);
            }

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend,
             SpriteSortMode.FrontToBack, SaveStateMode.None);
            
            //if(!isShowGameInfo) drawMouseCursor();
            drawGameInfo(gameTime);
            if (isUse3DEngine_)
            {
                draw3DEngineInfo();
            }
            drawScene(gameTime);

            spriteBatch.End();

        }


        #endregion

        #region Dealloc

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }
        #endregion
    }
}
