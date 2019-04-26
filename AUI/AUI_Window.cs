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

    
    public class UI_Window
    {
        public DisplayState displayState = DisplayState.Closed;
        public AUI_Rectangle rec_bkg;
        public AUI_Rectangle rec_fore;
        public int openWait = 2, openCounter;
        public int closeWait = 2, closeCounter;

        public UI_Window(int X, int Y, int W, int H)
        {
            rec_bkg = new AUI_Rectangle(
                X, Y, W, H, RecAnimType.WipeRight);
            rec_bkg.color = Assets.BackgroundColor;
            rec_fore = new AUI_Rectangle(
                X + 1, Y + 1, W - 2, H - 2, RecAnimType.WipeRight);
            rec_fore.color = Assets.ForegroundColor;
        }

        public void Open()
        {
            if (displayState != DisplayState.Closed) { return; }
            displayState = DisplayState.Opening;
            rec_bkg.Open(); openCounter = 0; //bkg, then fore
        }

        public void Close()
        {
            if (displayState == DisplayState.Closed
                || displayState == DisplayState.Closing) { return; }
            displayState = DisplayState.Closing;
            rec_fore.Close(); closeCounter = 0; //fore, then bkg
        }

        public void Update()
        {
            rec_bkg.Update(); rec_fore.Update();

            if (displayState == DisplayState.Opening)
            {
                openCounter++;
                if (openCounter >= openWait)
                { rec_fore.Open(); }

                if (rec_bkg.displayState == DisplayState.Opened &
                    rec_fore.displayState == DisplayState.Opened)
                { displayState = DisplayState.Opened; }
            }
            else if (displayState == DisplayState.Opened) { }
            else if (displayState == DisplayState.Closing)
            {
                closeCounter++;
                if (closeCounter >= closeWait)
                { rec_bkg.Close(); }

                if (rec_fore.displayState == DisplayState.Closed &
                    rec_bkg.displayState == DisplayState.Closed)
                { displayState = DisplayState.Closed; }
            }
            else if (displayState == DisplayState.Closed) { }
        }

        public void Draw(SpriteBatch SB)
        {
            rec_bkg.Draw(SB); rec_fore.Draw(SB);
        }

        public void MoveTo(int X, int Y)
        {
            rec_bkg.MoveTo(X, Y);
            rec_fore.MoveTo(X + 1, Y + 1);
        }

        public void SetAnimType(RecAnimType Type)
        {
            rec_bkg.animType = Type;
            rec_fore.animType = Type;
        }

        public void SetOpeningRec(Int4 Rec)
        {
            rec_bkg.openingRec = Rec;
            rec_fore.openingRec.X = rec_bkg.openingRec.X + 1;
            rec_fore.openingRec.Y = rec_bkg.openingRec.Y + 1;
            rec_fore.openingRec.W = rec_bkg.openingRec.W - 2;
            rec_fore.openingRec.H = rec_bkg.openingRec.H - 2;
        }

        public void SetOpenedRec(Int4 Rec)
        {
            rec_bkg.openedRec = Rec;
            rec_fore.openedRec.X = rec_bkg.openedRec.X + 1;
            rec_fore.openedRec.Y = rec_bkg.openedRec.Y + 1;
            rec_fore.openedRec.W = rec_bkg.openedRec.W - 2;
            rec_fore.openedRec.H = rec_bkg.openedRec.H - 2;
        }

        public void SetClosedRec(Int4 Rec)
        {
            rec_bkg.closedRec = Rec;
            rec_fore.closedRec.X = rec_bkg.closedRec.X + 1;
            rec_fore.closedRec.Y = rec_bkg.closedRec.Y + 1;
            rec_fore.closedRec.W = rec_bkg.closedRec.W - 2;
            rec_fore.closedRec.H = rec_bkg.closedRec.H - 2;
        }

    }
    
}
