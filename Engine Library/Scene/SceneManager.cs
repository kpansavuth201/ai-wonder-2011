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
    public class SceneManager
    {
        #region Model
        SceneStock sceneStock;
        public SceneStock SceneStock
        { get { return sceneStock; } }

        #endregion

        #region Initialize
        void initSceneStock()
        {
            sceneStock = new SceneStock();
        }

        public SceneManager()
        {
            initSceneStock();

        }

        public void LoadContent()
        {
            ICollection<string> index = sceneStock.Scenes.Keys;
            foreach (string id in index)
            {
                sceneStock.Scenes[id].LoadContent();
            }
            AIW.SystemLog(this, "load content");
        }


        #endregion


        #region Controller
        public void HandleInput(GameTime gt)
        {
            if (sceneStock.ActiveScene.state == TransitionState.Active)
            {
                if (sceneStock.ActiveScene.RootLayer.TopChildLayer.isActive)
                {
                    sceneStock.ActiveScene.RootLayer.TopChildLayer.HandleInput(gt);
                    ICollection<string> index = sceneStock.ActiveScene.RootLayer.TopChildLayer.Nodes.Keys;
                    foreach (string id in index)
                    {
                        if (sceneStock.ActiveScene.RootLayer.TopChildLayer.Nodes[id].isActive)
                            sceneStock.ActiveScene.RootLayer.TopChildLayer.Nodes[id].HandleInput(gt);
                    }
                }
            }
        }
        public void Update(GameTime gt)
        {
            if (sceneStock.ActiveScene.state == TransitionState.Active)
            {

                sceneStock.ActiveScene.Update(gt);
            }
        }
        #endregion


        #region View
        public void Draw(GameTime gt)
        {

            if (sceneStock.ActiveScene.state != TransitionState.Hidden)
            {

                // if(sceneStock.ActiveScene.state == TransitionState.Active)
                sceneStock.ActiveScene.Draw(gt);

                if (sceneStock.ActiveScene.state == TransitionState.TransitionOff)
                {
                    if (sceneStock.ActiveScene.TransitionOff(gt))
                    {
                        sceneStock.ActiveScene.ViewDidUnload();
                        sceneStock.ActiveScene.state = TransitionState.Hidden;
                        sceneStock.ActiveSceneTransitionFinished();
                        //AIW.Log(this, "fin active tran off");
                    }

                }
                else if (sceneStock.ActiveScene.state == TransitionState.TransitionOn)
                {
                    if (sceneStock.ActiveScene.TransitionOn(gt))
                    {
                        sceneStock.ActiveScene.state = TransitionState.Active;
                        //sceneStock.ActiveScene.ViewDidLoad();                     
                    }

                }
            }
        }

        #endregion

    }
}
