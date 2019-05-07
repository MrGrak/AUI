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
    //this screen shows off the crown button
    public class Screen_Example1 : Screen
    {
        public List<AUI_Base> aui_instances;
        AUI_Button button_back;

        public Screen_Example1()
        {
            aui_instances = new List<AUI_Base>();

            button_back = new AUI_Button(
                16 * 3, 16 * 2 + 8, 16 * 3, "< to title");
            button_back.CenterText();
            aui_instances.Add(button_back);

            //add some crown buttons for testing
            for (int g = 0; g < 5; g++)
            {
                AUI_CrownButton crown = new AUI_CrownButton(
                    16 * 6 + (g * 120), //x
                    (16 * 22),  //y
                    16 * 2 + 16 * g,  //width
                    "test " + g); //title

                //animate kids, except last one
                crown.wiggleChildren = true;
                if (g == 5) { crown.wiggleChildren = false; }
                aui_instances.Add(crown);
            }


            //add some sliders for testing
            for (int g = 0; g < 5; g++)
            {
                AUI_Slider_Horizontal slider =
                    new AUI_Slider_Horizontal(
                        16 * 6 + (g * 120), 
                        (16 * 7));
                slider.Open(g * 0.2f);
                //make the last few sliders display as float (not percentage)
                if (g > 2) { slider.valueType = SliderValueType.Float; }
                aui_instances.Add(slider);
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
                    //back button
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
        {   //draw all ui items
            for (i = 0; i < aui_instances.Count; i++)
            { aui_instances[i].Draw(); }
        }
    }
}