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
    public class Screen_Example1 : Screen
    {
        int i;
        public List<AUI_Base> aui_instances;
        AUI_Button button_back;


        public Screen_Example1()
        {
            aui_instances = new List<AUI_Base>();

            button_back = new AUI_Button(
                16 * 3, 16 * 2 + 8, 16 * 3, "< to title");
            button_back.CenterText();
            aui_instances.Add(button_back);
            
                /*
            AUI_LineWithRecs line0 = new AUI_LineWithRecs();
            line0.MoveTo(16 * 8, 16 * 6);
            line0.SetTarget(16 * 41, 16 * 6);
            line0.line.speedOpen = 25;
            line0.line.speedClosed = 25;
            aui_instances.Add(line0);

            AUI_LineWithRecs line1 = new AUI_LineWithRecs();
            line1.MoveTo(16 * 8, 16 * 13);
            line1.SetTarget(16 * 41, 16 * 13);
            line1.line.speedOpen = 20;
            line1.line.speedClosed = 20;
            aui_instances.Add(line1);

            AUI_LineWithRecs line2 = new AUI_LineWithRecs();
            line2.MoveTo(16 * 8, 16 * 18);
            line2.SetTarget(16 * 41, 16 * 18);
            line2.line.speedOpen = 15;
            line2.line.speedClosed = 15;
            aui_instances.Add(line2);

            AUI_LineWithRecs line3 = new AUI_LineWithRecs();
            line3.MoveTo(16 * 8, 16 * 23);
            line3.SetTarget(16 * 41, 16 * 23);
            line3.line.speedOpen = 10;
            line3.line.speedClosed = 10;
            aui_instances.Add(line3);
            */
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

                #region Screen1 Btn Interaction

                if (Functions.Contains(
                    button_back.window.rec_bkg.openedRec,
                    Input.cursorPos.X, Input.cursorPos.Y))
                {   //give button focus
                    button_back.focused = true;
                    //check for new left click
                    if (Input.IsLeftMouseBtnPress())
                    { Close(ExitAction.Title); }
                }
                else { button_back.focused = false; }

                #endregion

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
                if (exitAction == ExitAction.Title)
                { ScreenManager.ExitAndLoad(new Screen_Title()); }
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