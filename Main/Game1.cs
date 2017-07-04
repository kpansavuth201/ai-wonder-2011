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
   /// <summary>
   /// This is the main type for your game
   /// </summary>
   public class Game1 : Microsoft.Xna.Framework.Game
   {
      GraphicsDeviceManager graphics;

      public static BloomComponent bloom;
      

      public Game1()
      {
         graphics = new GraphicsDeviceManager(this);
         Content.RootDirectory = "Content";
         IsMouseVisible = false;
         
         graphics.PreferredDepthStencilFormat = SelectStencilMode();
         bloom = new BloomComponent(this);

         //Create AIWOS
         AIW.setAIWOperatingSystem(graphics, this);
         Components.Add(AIW.OS);
     

         //Add Bloom Effect
        //Components.Add(bloom);

       
      }

      //must have in Game1***use for set shadow to 0 depth
      public DepthFormat SelectStencilMode()
      {
         // Check stencil formats
         GraphicsAdapter adapter = GraphicsAdapter.DefaultAdapter;
         SurfaceFormat format = adapter.CurrentDisplayMode.Format;
         if (adapter.CheckDepthStencilMatch(DeviceType.Hardware, format,
             format, DepthFormat.Depth24Stencil8))
            return DepthFormat.Depth24Stencil8;
         else if (adapter.CheckDepthStencilMatch(DeviceType.Hardware, format,
             format, DepthFormat.Depth24Stencil8Single))
            return DepthFormat.Depth24Stencil8Single;
         else if (adapter.CheckDepthStencilMatch(DeviceType.Hardware, format,
             format, DepthFormat.Depth24Stencil4))
            return DepthFormat.Depth24Stencil4;
         else if (adapter.CheckDepthStencilMatch(DeviceType.Hardware, format,
             format, DepthFormat.Depth15Stencil1))
            return DepthFormat.Depth15Stencil1;
         else
            throw new InvalidOperationException(
                "Could Not Find Stencil Buffer for Default Adapter");
      }


      /// <summary>
      /// Allows the game to perform any initialization it needs to before starting to run.
      /// This is where it can query for any required services and load any non-graphic
      /// related content.  Calling base.Initialize will enumerate through any components
      /// and initialize them as well.
      /// </summary>
      protected override void Initialize()
      {
         // TODO: Add your initialization logic here
          graphics.PreferredBackBufferWidth = (int)AIW.ScreenSize.X;
          graphics.PreferredBackBufferHeight = (int)AIW.ScreenSize.Y;
         graphics.ApplyChanges();
         base.Initialize();
      }
   }
}
