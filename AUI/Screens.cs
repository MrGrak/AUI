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
using System.Drawing;
using System.IO;
using System.Reflection;

namespace AUI
{
    public enum ExitAction
    {
        Reload
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

        public static void DrawActiveScreens()
        {   
            Assets.GDM.GraphicsDevice.SetRenderTarget(null);
            Assets.GDM.GraphicsDevice.Clear(Assets.GameBkgColor);
            Assets.SB.Begin(SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.AnisotropicClamp);

            foreach (Screen screen in screens) { screen.Draw(); }

            Assets.SB.End();
        }
    }

    public class Title_Screen : Screen
    {
        AUI_Button button_test;
        MouseState currentMouseState = new MouseState();
        MouseState lastMouseState = new MouseState();

        public Title_Screen()
        {
            button_test = new AUI_Button(
                16 * 5, 16 * 5, 16 * 5, "test");
            button_test.CenterText();
        }

        public override void Open()
        {
            displayState = DisplayState.Opening;
            button_test.Open();
        }

        public override void Close(ExitAction EA)
        {
            button_test.Close();
            displayState = DisplayState.Closing;
        }

        public override void Update()
        {
            lastMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
            button_test.Update();


            if (displayState == DisplayState.Opening)
            {
                if (button_test.displayState == DisplayState.Opened)
                {
                    displayState = DisplayState.Opened;
                }
            }
            else if (displayState == DisplayState.Opened)
            {
                //handle main input here

                #region Test Btn Interaction

                if (Functions.Contains(
                    button_test.window.rec_bkg.openedRec,
                    currentMouseState.X, currentMouseState.Y))
                {
                    //give button focus
                    button_test.focused = true;
                    //check for new left click
                    if (currentMouseState.LeftButton == ButtonState.Pressed &&
                        lastMouseState.LeftButton == ButtonState.Released)
                    {
                        button_test.text.ChangeText(
                            "button test passed");
                        button_test.CenterText();
                        Close(ExitAction.Reload);
                    }
                }
                else { button_test.focused = false; }

                #endregion

            }
            else if (displayState == DisplayState.Closing)
            {
                if (button_test.displayState == DisplayState.Closed)
                {
                    displayState = DisplayState.Closed;
                }
            }
            else if (displayState == DisplayState.Closed)
            {
                if (exitAction == ExitAction.Reload)
                { ScreenManager.ExitAndLoad(new Title_Screen()); }
                //anything else case:
                //else { ScreenManager.ExitAndLoad(Screens.Board); }
            }
        }

        public override void Draw()
        {
            button_test.Draw(Assets.SB);
        }

    }
}