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

    public class AUI_Button : AUI_Base
    {
        public AUI_Window window;
        public AUI_Text text;
        
        public Color color_over;
        public Color color_normal;
        public Color color_over_text;
        public Color color_normal_text;

        public Boolean draggable = false;
        public Boolean beingDragged = false;


        public AUI_Button(int X, int Y, int W, string Text)
        {
            color_over = Assets.OverColor;
            color_normal = Assets.ForegroundColor;
            color_over_text = Assets.OverColor;
            color_normal_text = Assets.TextColor;

            window = new AUI_Window(X, Y, W, 16);
            window.rec_bkg.color = color_normal;
            window.rec_fore.color = Assets.BackgroundColor;
            text = new AUI_Text(Text,
                X, Y, new Color(255, 255, 255, 0));
            displayState = DisplayState.Closed;
        }

        public override void Open()
        {
            if (displayState == DisplayState.Closed)
            {
                displayState = DisplayState.Opening;
                text.font = Assets.font;
                window.Open();
            }
        }

        public override void Close()
        {
            if (displayState == DisplayState.Opened
                || displayState == DisplayState.Opening)
            {
                displayState = DisplayState.Closing;
                window.Close(); text.Close();
            }
        }

        public override void Update()
        {
            window.Update(); text.Update();

            if (displayState == DisplayState.Opening)
            {
                window.Open();
                if (window.displayState == DisplayState.Opened)
                {
                    text.Open();
                    if (text.displayState == DisplayState.Opened)
                    { displayState = DisplayState.Opened; }
                }
            }
            else if (displayState == DisplayState.Opened)
            {   
                //handle user focusing/hovering over button via cursor
                if (Functions.Contains(
                    window.rec_bkg.openedRec,
                    Input.cursorPos.X, Input.cursorPos.Y))
                {
                    window.rec_bkg.color = color_over;
                    text.color = color_over_text;
                    //pulse text alpha
                    if (text.alpha >= 1.1f) { text.alpha = 0.7f; }
                    else { text.alpha += 0.01f; }
                    //pickup button
                    if (Input.IsLeftMouseBtnPress())
                    {   //check for new left click, start dragging state
                        if (draggable) { beingDragged = true; }
                    }
                }
                else
                {
                    window.rec_bkg.color = color_normal;
                    text.color = color_normal_text;
                }

                //if button was picked up, match cursor's pos
                if (Input.currentMouseState.LeftButton == ButtonState.Pressed)
                {
                    if (draggable & beingDragged)
                    {
                        MoveTo(
                        (int)Input.cursorPos.X - window.rec_bkg.openedRec.W / 2,
                        (int)Input.cursorPos.Y - window.rec_bkg.openedRec.H / 2);
                    }
                }
                //drop button to screen
                if (Input.currentMouseState.LeftButton == ButtonState.Released)
                {
                    
                    if (draggable & beingDragged)
                    {
                        beingDragged = false;
                    }
                }
            }
            else if (displayState == DisplayState.Closing)
            {
                if (window.displayState == DisplayState.Closed)
                { displayState = DisplayState.Closed; }
            }
            else if (displayState == DisplayState.Closed) { }

            //Debug.WriteLine("obj ds: " + displayState);
            //Debug.WriteLine("window ds: " + window.displayState);
            //Debug.WriteLine("text ds: " + text.displayState);
        }

        public override void Draw()
        { window.Draw(); text.Draw(); }

        //

        public void CenterText()
        {   //measure width of the button's text
            int textWidth = (int)Assets.font.MeasureString(text.text).X;
            //center text to button, prevent half pixel offsets
            text.position.X = (window.rec_bkg.openedRec.X +
                window.rec_bkg.openedRec.W / 2) - (textWidth / 2);
            //center text vertically
            text.position.Y = window.rec_bkg.openedRec.Y - 0;
        }

        public void MoveTo(int X, int Y)
        {
            window.MoveTo(X, Y);
            CenterText();
        }

    }

}
