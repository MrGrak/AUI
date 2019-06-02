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
    public class Screen_Spider : Screen
    {
        //int i;
        public List<AUI_Base> aui_instances;
        AUI_Button button_back;
        



        public Screen_Spider()
        {
            aui_instances = new List<AUI_Base>();

            button_back = new AUI_Button(
                16 * 3, 16 * 2 + 8, 16 * 3, "< to title");
            button_back.CenterText();
            aui_instances.Add(button_back);

            /*
            for(int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 1; y++)
                {
                    //create test spider button
                    AUI_SpiderQueenButton spiderQueen =
                        new AUI_SpiderQueenButton(
                            16 * 5 + ((16 * 4 + 6) * x),
                            16 * 17 + (16 * 4 * y), 
                            "test", aui_instances);
                    aui_instances.Add(spiderQueen);
                }
            }
            */

            
            //single
            AUI_SpiderQueenButton test =
                new AUI_SpiderQueenButton(
                16 * 22, 16 * 18, "hmm...", aui_instances);
            aui_instances.Add(test);
            

            /*
            AUI_LineWithRecs line0 = new AUI_LineWithRecs();
            line0.MoveTo(16 * 5, 16 * 15);
            line0.SetTarget(16 * 45, 16 * 15);
            aui_instances.Add(line0);

            AUI_LineWithRecs line1 = new AUI_LineWithRecs();
            line1.MoveTo(16 * 5, 16 * 20);
            line1.SetTarget(16 * 45, 16 * 20);
            aui_instances.Add(line1);
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
                if (Input.IsLeftMouseBtnPress())
                {
                    //dump the list count for debugging
                    Debug.WriteLine(
                        "aui instances: " +
                        aui_instances.Count);
                    //this allows verification of baby removal

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
            {
                aui_instances[i].Draw();
            }
        }

    }

}