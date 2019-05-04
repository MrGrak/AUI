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

    public enum SliderValueType { Percentage, Float }

    public class AUI_Slider_Horizontal : AUI_Base
    {
        public AUI_Text text;
        public AUI_Window bkgLineWindow;
        public Int4 handle;
        public Int4 clickLine;
        public float clickLineAlpha = 0.0f;
        public Int4 hitbox;
        public int clickX;
        public float value = 0.0f;
        public float currValue = 0.0f;
        public int Width = 100;
        public SliderValueType valueType = SliderValueType.Percentage;

        public AUI_Slider_Horizontal(int X, int Y)
        {
            bkgLineWindow = new AUI_Window(X, Y, Width, 4);
            bkgLineWindow.rec_bkg.color = Assets.ForegroundColor;
            bkgLineWindow.rec_fore.color = Assets.BackgroundColor;

            text = new AUI_Text("0.15",
                X + 16 * 9 + 6, Y - 10, Assets.ForegroundColor);
            text.color = Assets.ForegroundColor;
            text.font = Assets.font;

            //this is what user clicks left/right of
            handle = new Int4(X, Y - 6, 4, 16);
            //this is what controls the clickable area
            hitbox = new Int4(X, Y - 10, Width, 24);
            //this gives user feedback they clicked somewhere
            clickLine = new Int4(X, Y - 3, 1, 10);

            //inits slider to start 0
            clickX = X + 4;
            //inits slider to end 1.0 max
            //clickX = X + Width - 8;

            displayState = DisplayState.Closed;
        }

        public void Open(float OpeningValue)
        {
            if (displayState == DisplayState.Closed)
            {
                bkgLineWindow.Open();
                //set internal value and animated display value
                currValue = 0.0f;
                value = OpeningValue;
                //set slider at 0 position
                handle.X = bkgLineWindow.rec_bkg.openedRec.X + 4;
                //convert opening value to clickX pos, so slider animates
                clickX = (bkgLineWindow.rec_bkg.openedRec.X);
                //slider is 100 pixels long, multiply normalized 0-1.0 (scale up)
                clickX += (int)(value * Width);
                //set the click line alpha, so user sees where handle is moving
                clickLineAlpha = 1.0f;
                //finally, open the screen
                displayState = DisplayState.Opening;
            }
        }

        public override void Close()
        {
            if (displayState == DisplayState.Opened
                || displayState == DisplayState.Opening)
            {
                displayState = DisplayState.Closing;
                bkgLineWindow.Close(); text.Close();
            }
        }

        public override void Update()
        {
            text.Update();
            bkgLineWindow.Update();

            //put the text over the handle each frame
            text.position.X = handle.X - 4;
            text.position.Y = handle.Y - 16;

            if (displayState == DisplayState.Opening)
            {
                if (bkgLineWindow.displayState == DisplayState.Opened)
                { displayState = DisplayState.Opened; }
            }
            else if (displayState == DisplayState.Opened)
            {
                //click line always follows clickX each frame
                clickLine.X = clickX;
                //fade click line out overtime
                if (clickLineAlpha > 0.0f)
                { clickLineAlpha -= 0.01f; }
                else { clickLineAlpha = 0.0f; }


                #region Increment currValue, which is what text draws

                if (currValue < value)
                {
                    currValue += 0.01f;
                }
                else if (currValue > value)
                {
                    currValue -= 0.01f;
                }
                else { currValue = value; }

                #endregion

                #region Move handle to click position over time

                if (handle.X + 2 < clickX)
                {
                    handle.X++;
                    value += 0.01f;
                    //limit value max
                    if (value > 1.0f) { value = 1.0f; }
                    //check for right boundary, limit
                    if (handle.X > bkgLineWindow.rec_bkg.openedRec.X + Width - 2)
                    {
                        handle.X = bkgLineWindow.rec_bkg.openedRec.X + Width - 2;
                        value = 1.0f; //max
                    }
                    UpdateHandleValue();
                }
                else if (handle.X + 2 > clickX)
                {
                    handle.X--;
                    value -= 0.01f;
                    //limit value min
                    if (value < 0.01f) { value = 0.01f; }
                    //check for left boundary, limit
                    if (handle.X < bkgLineWindow.rec_bkg.openedRec.X)
                    {
                        handle.X = bkgLineWindow.rec_bkg.openedRec.X;
                        value = 0.01f; //min
                    }
                    UpdateHandleValue();
                }

                #endregion


                //add in user input to move value/handle
                if (Input.IsLeftMouseBtnPress())
                {
                    //user must click inside hitbox to move slider/handle
                    if (!Functions.Contains(hitbox,
                        Input.cursorPos.X, Input.cursorPos.Y))
                    { return; }

                    //else, store click position, display click line
                    clickX = (int)Input.cursorPos.X;
                    clickLineAlpha = 1.0f;
                }
            }
            else if (displayState == DisplayState.Closing)
            {
                if (bkgLineWindow.displayState == DisplayState.Closed)
                { displayState = DisplayState.Closed; }
            }
            else if (displayState == DisplayState.Closed) { }
        }

        public override void Draw()
        {
            text.Draw();
            bkgLineWindow.Draw();

            if (displayState == DisplayState.Opened)
            {   //custom draw for int4 handle
                Rectangle locRec = new Rectangle(
                    handle.X, handle.Y, handle.W, handle.H);
                Assets.SB.Draw(Assets.recTex,
                    locRec, Assets.ForegroundColor * 1.0f);

                //custom draw for click line too
                locRec = new Rectangle(
                    clickLine.X, clickLine.Y, clickLine.W, clickLine.H);
                Assets.SB.Draw(Assets.recTex,
                    locRec, Assets.ForegroundColor * clickLineAlpha);
            }
            //draw hitbox (debug only)
            //Rectangle hitRec = new Rectangle(
            //    hitbox.X, hitbox.Y, hitbox.W, hitbox.H);
            //Data.SB.Draw(Data.lineTexture,
            //    hitRec, Data.Color_Orange6_Neon * 0.2f);
            
        }
    
        public void UpdateHandleValue()
        {
            if (valueType == SliderValueType.Percentage)
            { text.drawText = "" + currValue.ToString("0%"); }
            else if (valueType == SliderValueType.Float)
            { text.drawText = "" + currValue.ToString("0.0"); }
        }
    }
}