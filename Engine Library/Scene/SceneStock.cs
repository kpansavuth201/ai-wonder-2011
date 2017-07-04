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
    public class SceneStock
    {

        #region Model
        Old_Scene activeScene;
        Old_Scene inCommingScene;
        public Old_Scene ActiveScene { get { return activeScene; } }
        public void RemoveInCommingScene() { inCommingScene = null; }
        public Old_Scene InCommingScene { get { return inCommingScene; } }

        //Dictionary
        Dictionary<string, Old_Scene> scenes;
        public Dictionary<string, Old_Scene> Scenes
        {
            get { return scenes; }
        }

        bool checkIfSceneKeyExist(string name)
        {
            ICollection<string> index = scenes.Keys;
            foreach (string id in index)
            {
                if (name == id)
                {
                    return true;
                }
            }
            return false;
        }
        bool checkIfSameSceneKeyExist(string name)
        {
            if (checkIfSceneKeyExist(name))
            {
                AIW.SystemLog(this, "Same scene key : " + name + " exist and cannot be added.");
                return true;
            }
            return false;
        }
        public void AddScene(string name, Old_Scene tmp)
        {
            if (!checkIfSameSceneKeyExist(name))
            {
                scenes.Add(name, tmp);
            }
        }
        #endregion

        #region Initialize
        void setupRootScene(Old_Scene rootScene)
        {
            rootScene.state = TransitionState.TransitionOn;
            rootScene.ViewDidLoad();
            activeScene = rootScene;
        }
        void initScenes()
        {
            scenes = new Dictionary<string, Old_Scene>();

            ///Entry point

            //Add scene from model here
      

            AiWonder_FieldMapScene tmp = new AiWonder_FieldMapScene();
            Old_AiWonder_BattleScene battleScene = new Old_AiWonder_BattleScene();

            AddScene("FieldMapScene", tmp);
            AddScene("BattleScene", battleScene);

            setupRootScene(battleScene);
        }
        public SceneStock()
        {
            initScenes();

            AIW.SystemLog(this, "init");
        }
        #endregion

        #region Controller
        public bool ChangeScene(string destinationScene)
        {
            if (checkIfSceneKeyExist(destinationScene))
            {
                inCommingScene = scenes[destinationScene];
                inCommingScene.ViewDidLoad();
                activeScene.state = TransitionState.TransitionOff;
                return true;
            }
            else
            {
                AIW.SystemLog(this, "Error, Scane " + destinationScene +
                    " doesn't exist and cannot change to.");
                return false;
            }
        }
        public void ActiveSceneTransitionFinished()
        {
            if (inCommingScene != null)
            {
                activeScene = inCommingScene;
                activeScene.state = TransitionState.TransitionOn;
                inCommingScene = null;
            }
        }

        #endregion

    }
}
