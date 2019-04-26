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

    public enum ExitAction
    {
        Title, Board, Summary,
        ExitScreen, ExitGame, Ai
    }
    
    public abstract class Screen
    {
        public DisplayState displayState;
        public ExitAction exitAction;
        public int i;
        public Screen() { }
        public virtual void Open() { }
        public virtual void Close(ExitAction EA) { }
        public virtual void Update() { }
        public virtual void Draw() { }
    }

    public static class ScreenManager
    {   
        public static List<Screen> screens = new List<Screen>();
        public static Screen activeScreen; 

        public static void AddScreen(Screen screen)
        {
            screen.Open();
            screens.Add(screen);
        }

        public static void RemoveScreen(Screen screen)
        {
            screens.Remove(screen);
        }

        public static void ExitAndLoad(Screen screenToLoad)
        {   //remove every screen on screens list
            while (screens.Count > 0)
            { screens.Remove(screens[0]); }
            AddScreen(screenToLoad);
            screenToLoad.Open();
        }

        public static void Update()
        {
            //Input.Update();
            if (screens.Count > 0)
            {   //the only 'active screen' is the last one (top one)
                activeScreen = screens[screens.Count - 1];
                activeScreen.Update();
            }
        }

        static Texture2D bloom;
        public static void DrawActiveScreens()
        {   
            Assets.GDM.GraphicsDevice.SetRenderTarget(null);
            Assets.GDM.GraphicsDevice.Clear(Color.CornflowerBlue);
            Assets.SB.Begin(SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.AnisotropicClamp);

            foreach (Screen screen in screens) { screen.Draw(); }

            Assets.SB.End();
        }
    }
    



    public class Title_Screen : Screen
    {





        public Title_Screen()
        {

        }

        public override void Open()
        {
            Debug.WriteLine("title screen opened");
        }

        public override void Close(ExitAction EA)
        {

        }

        public override void Update()
        {

        }

        public override void Draw()
        {

        }
    }










}