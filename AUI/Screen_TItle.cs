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
    public class Screen_Title : Screen
    {
        int i;
        public List<AUI_Base> aui_instances;
        AUI_Button button_screen1;
        AUI_Button button_screen2;
        AUI_Button button_screen3;
        AUI_Button button_screen4;
        AUI_Button button_screen5;
        AUI_Text titleText;
        AUI_Text descText;


        public Screen_Title()
        {
            aui_instances = new List<AUI_Base>();

            button_screen1 = new AUI_Button(
                16 * 8, 16 * 20, 16 * 5, "crown example");
            button_screen1.CenterText();
            aui_instances.Add(button_screen1);

            button_screen2 = new AUI_Button(
                16 * 15, 16 * 20, 16 * 5, "stress test");
            button_screen2.CenterText();
            aui_instances.Add(button_screen2);

            button_screen3 = new AUI_Button(
                16 * 22, 16 * 20, 16 * 5, "example 3");
            button_screen3.CenterText();
            aui_instances.Add(button_screen3);

            button_screen4 = new AUI_Button(
                16 * 29, 16 * 20, 16 * 5, "example 4");
            button_screen4.CenterText();
            aui_instances.Add(button_screen4);

            button_screen5 = new AUI_Button(
                16 * 36, 16 * 20, 16 * 5, "example 5");
            button_screen5.CenterText();
            aui_instances.Add(button_screen5);

            titleText = new AUI_Text("grak's aui lab",
                16 * 8, 16 * 5, Assets.ForegroundColor);
            titleText.scale = 10.0f;
            aui_instances.Add(titleText);

            descText = new AUI_Text(
                "this monogame laboratory showcases a variety of animated ui." +
                "\nclick a button below to see an example.",
                16 * 8, 16 * 14, Assets.ForegroundColor);
            descText.scale = 2.0f;
            aui_instances.Add(descText);

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
                if (button_screen1.displayState == DisplayState.Opened)
                {
                    displayState = DisplayState.Opened;
                }
            }
            else if (displayState == DisplayState.Opened)
            {
                //handle main input here
                if (Input.IsLeftMouseBtnPress())
                {
                    //button 1
                    if (Functions.Contains(
                        button_screen1.window.rec_bkg.openedRec,
                        Input.cursorPos.X, Input.cursorPos.Y))
                    {
                        Close(ExitAction.Example1);
                    }
                    //button 2
                    if (Functions.Contains(
                        button_screen2.window.rec_bkg.openedRec,
                        Input.cursorPos.X, Input.cursorPos.Y))
                    {
                        Close(ExitAction.StressTest);
                    }
                    //button 3
                    if (Functions.Contains(
                        button_screen3.window.rec_bkg.openedRec,
                        Input.cursorPos.X, Input.cursorPos.Y))
                    {
                        Close(ExitAction.Example1);
                    }
                    //button 4
                    if (Functions.Contains(
                        button_screen4.window.rec_bkg.openedRec,
                        Input.cursorPos.X, Input.cursorPos.Y))
                    {
                        Close(ExitAction.Example1);
                    }
                    //button 5
                    if (Functions.Contains(
                        button_screen5.window.rec_bkg.openedRec,
                        Input.cursorPos.X, Input.cursorPos.Y))
                    {
                        Close(ExitAction.Example1);
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
                if (exitAction == ExitAction.Title)
                { ScreenManager.ExitAndLoad(new Screen_Title()); }
                else if (exitAction == ExitAction.Example1)
                { ScreenManager.ExitAndLoad(new Screen_Example1()); }
                else if (exitAction == ExitAction.StressTest)
                { ScreenManager.ExitAndLoad(new Screen_StressTest()); }
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