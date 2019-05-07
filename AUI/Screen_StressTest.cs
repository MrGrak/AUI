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
    //this is just an example of how to write a basic screen with a back button
    public class Screen_StressTest : Screen
    {
        int i;
        public List<AUI_Base> aui_instances;
        public AUI_Button button_back;

        public Stopwatch timer =new Stopwatch();
        public AUI_Text timer_text;


        public Screen_StressTest()
        {
            aui_instances = new List<AUI_Base>();

            button_back = new AUI_Button(
                16 * 3, 16 * 2 + 8, 16 * 3, "< to title");
            button_back.CenterText();
            aui_instances.Add(button_back);

            timer_text = new AUI_Text("time",
                16 * 6 + 8, 16 * 2 + 5, 
                Microsoft.Xna.Framework.Color.White);
            aui_instances.Add(timer_text);

            int g = 0; //counter/id
            for (int x = 0; x < 28; x++)
            {
                for (int y = 0; y < 16; y++)
                {
                    AUI_Button button = new AUI_Button(
                        48 + (x * 25), //x
                        64 + (25 * y),  //y
                        24,  //width
                        "" + g); //title
                    button.CenterText();
                    button.draggable = true;
                    aui_instances.Add(button);
                    g++;
                }
            }
        }

        public override void Open()
        {
            displayState = DisplayState.Opening;
            for (i = 0; i < aui_instances.Count; i++)
            { aui_instances[i].Open(); }
        }

        public override void Close(ExitAction EA)
        {
            exitAction = EA;
            displayState = DisplayState.Closing;
            for (i = 0; i < aui_instances.Count; i++)
            { aui_instances[i].Close(); }
        }

        public override void Update()
        {
            timer.Restart();

            //update all ui items
            for (i = 0; i < aui_instances.Count; i++)
            { aui_instances[i].Update(); }

            #region Screen Display States

            if (displayState == DisplayState.Opening)
            {
                if (button_back.displayState == DisplayState.Opened)
                {
                    displayState = DisplayState.Opened;
                }
            }
            else if (displayState == DisplayState.Opened)
            {
                //handle main input here
                if (Input.IsLeftMouseBtnPress())
                {
                    if (Functions.Contains(
                        button_back.window.rec_bkg.openedRec,
                        Input.cursorPos.X, Input.cursorPos.Y))
                    {
                        Close(ExitAction.Title);
                    }
                }
            }
            else if (displayState == DisplayState.Closing)
            {
                //ensure all aui items are closed
                Boolean allClosed = true; //assume true, prove false
                for (i = 0; i < aui_instances.Count; i++)
                {
                    if (aui_instances[i].displayState != DisplayState.Closed)
                    { allClosed = false; }
                }
                if (allClosed) { displayState = DisplayState.Closed; }
            }
            else if (displayState == DisplayState.Closed)
            {
                if (exitAction == ExitAction.Title) { }
                //this screen only closes one way
                ScreenManager.ExitAndLoad(new Screen_Title());
            }

            #endregion

        }

        public override void Draw()
        {
            //draw all ui items
            for (i = 0; i < aui_instances.Count; i++)
            { aui_instances[i].Draw(); }

            timer.Stop();
            timer_text.drawText = "frame ms: " + timer.ElapsedMilliseconds;
            timer_text.drawText += "\nticks: " + timer.ElapsedTicks;
        }

    }

}