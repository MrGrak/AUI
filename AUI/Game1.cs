using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;

namespace AUI
{
    public class Game1 : Game
    {
        public Game1()
        {   //set asset refs
            GraphicsDeviceManager Graphics = new GraphicsDeviceManager(this);
            Graphics.SynchronizeWithVerticalRetrace = true;
            Graphics.GraphicsProfile = GraphicsProfile.HiDef;
            Assets.GDM = Graphics;
            Assets.CM = Content;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {   //spritebatch becomes available here
            Assets.SB = new SpriteBatch(GraphicsDevice);
            Assets.Load();
            ScreenManager.AddScreen(new Title_Screen());
        }

        protected override void UnloadContent() { }

        protected override void Update(GameTime gameTime)
        {
            ScreenManager.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            ScreenManager.DrawActiveScreens();
            base.Draw(gameTime);
        }
    }
}