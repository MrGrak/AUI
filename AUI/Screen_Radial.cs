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
    public class Screen_Radial : Screen
    {
        int i;
        public List<AUI_Base> aui_instances;
        AUI_Button button_back;


        Boolean radialOpen = false;
        AUI_ButtonWithLine radial_TopLeft;
        AUI_ButtonWithLine radial_MidLeft;
        AUI_ButtonWithLine radial_BotLeft;

        AUI_ButtonWithLine radial_TopRight;
        AUI_ButtonWithLine radial_MidRight;
        AUI_ButtonWithLine radial_BotRight;

        AUI_Text introText;





        public Screen_Radial()
        {
            aui_instances = new List<AUI_Base>();

            button_back = new AUI_Button(
                16 * 3, 16 * 2 + 8, 16 * 3, "< to title");
            button_back.CenterText();
            aui_instances.Add(button_back);

            int btnWidth = 16 * 5;

            //left set
            radial_TopLeft = new AUI_ButtonWithLine(-256, -256, btnWidth, "btn a");
            radial_TopLeft.offsetX = btnWidth;
            radial_TopLeft.offsetY = 8;
            aui_instances.Add(radial_TopLeft);

            radial_MidLeft = new AUI_ButtonWithLine(-256, -256, btnWidth, "btn b");
            radial_MidLeft.offsetX = btnWidth;
            radial_MidLeft.offsetY = 8;
            aui_instances.Add(radial_MidLeft);

            radial_BotLeft = new AUI_ButtonWithLine(-256, -256, btnWidth, "btn c");
            radial_BotLeft.offsetX = btnWidth;
            radial_BotLeft.offsetY = 8;
            aui_instances.Add(radial_BotLeft);

            //right set
            radial_TopRight = new AUI_ButtonWithLine(-256, -256, btnWidth, "btn d");
            radial_TopRight.offsetX = 0;
            radial_TopRight.offsetY = 8;
            aui_instances.Add(radial_TopRight);

            radial_MidRight = new AUI_ButtonWithLine(-256, -256, btnWidth, "btn e");
            radial_MidRight.offsetX = 0;
            radial_MidRight.offsetY = 8;
            aui_instances.Add(radial_MidRight);

            radial_BotRight = new AUI_ButtonWithLine(-256, -256, btnWidth, "btn f");
            radial_BotRight.offsetX = 0;
            radial_BotRight.offsetY = 8;
            aui_instances.Add(radial_BotRight);

            introText = new AUI_Text(
                "press and hold spacebar to open the radial menu",
                16 * 7, 16 * 2 + 8, Assets.TextColor);
            aui_instances.Add(introText);
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
                    if (Functions.Contains(
                        button_back.window.rec_bkg.openedRec,
                        Input.cursorPos.X, Input.cursorPos.Y))
                    {
                        Close(ExitAction.Title);
                    }
                }
                //toggle radial open/closed
                if (Input.currentKeyboardState.IsKeyDown(Keys.Space))
                { OpenRadial(); } else { CloseRadial(); }
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
        }

        //

        public void OpenRadial()
        {
            if(radialOpen == false)
            {
                radialOpen = true;
                //left, place radial elements based on cursor pos
                radial_TopLeft.button.MoveTo(
                    (int)Input.cursorPos.X - 16 * 6,
                    (int)Input.cursorPos.Y - 16 * 2);
                radial_MidLeft.button.MoveTo(
                    (int)Input.cursorPos.X - 16 * 7,
                    (int)Input.cursorPos.Y - 16 * 0 - 8);
                radial_BotLeft.button.MoveTo(
                    (int)Input.cursorPos.X - 16 * 6,
                    (int)Input.cursorPos.Y + 16 * 1);
                //right
                radial_TopRight.button.MoveTo(
                    (int)Input.cursorPos.X + 16 * 1,
                    (int)Input.cursorPos.Y - 16 * 2);
                radial_MidRight.button.MoveTo(
                    (int)Input.cursorPos.X + 16 * 2,
                    (int)Input.cursorPos.Y - 16 * 0 - 8);
                radial_BotRight.button.MoveTo(
                    (int)Input.cursorPos.X + 16 * 1,
                    (int)Input.cursorPos.Y + 16 * 1);

                //all radials set their lines to mouse pos
                radial_TopLeft.line.MoveTo(
                    (int)Input.cursorPos.X,
                    (int)Input.cursorPos.Y);
                radial_MidLeft.line.MoveTo(
                    (int)Input.cursorPos.X,
                    (int)Input.cursorPos.Y);
                radial_BotLeft.line.MoveTo(
                    (int)Input.cursorPos.X,
                    (int)Input.cursorPos.Y);
                //right
                radial_TopRight.line.MoveTo(
                    (int)Input.cursorPos.X,
                    (int)Input.cursorPos.Y);
                radial_MidRight.line.MoveTo(
                    (int)Input.cursorPos.X,
                    (int)Input.cursorPos.Y);
                radial_BotRight.line.MoveTo(
                    (int)Input.cursorPos.X,
                    (int)Input.cursorPos.Y);


                //open all radial buttons
                radial_TopLeft.Open();
                radial_MidLeft.Open();
                radial_BotLeft.Open();

                radial_TopRight.Open();
                radial_MidRight.Open();
                radial_BotRight.Open();
            }
        }

        public void CloseRadial()
        {
            radial_TopLeft.Close();
            radial_MidLeft.Close();
            radial_BotLeft.Close();

            radial_TopRight.Close();
            radial_MidRight.Close();
            radial_BotRight.Close();

            radialOpen = false;
        }

    }
}