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
        int i;
        public List<AUI_Base> aui_instances;
        AUI_Button button_test;


        public Title_Screen()
        {
            aui_instances = new List<AUI_Base>();

            button_test = new AUI_Button(
                16 * 5, 16 * 5, 16 * 5, "test");
            button_test.CenterText();
            aui_instances.Add(button_test);
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
            //update all ui items
            for(i = 0; i < aui_instances.Count; i++)
            { aui_instances[i].Update(); }

            #region Screen Display States

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
                    Input.cursorPos.X, Input.cursorPos.Y))
                {   //give button focus
                    button_test.focused = true;
                    //check for new left click
                    if (Input.IsLeftMouseBtnPress())
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
                //else { ScreenManager.ExitAndLoad(new Title_Screen()); }
            }

            #endregion

        }

        public override void Draw()
        {
            //draw all ui items
            for (i = 0; i < aui_instances.Count; i++)
            { aui_instances[i].Draw(); }
        }

    }
}