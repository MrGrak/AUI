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
    public class AUI_LineWithRecs : AUI_Base
    {   //line anims in/out
        public AUI_Line line;
        public AUI_Rectangle recA;
        public AUI_Rectangle recB;

        public AUI_LineWithRecs()
        {
            line = new AUI_Line();
            recA = new AUI_Rectangle(-16, -16, 16, 16, RecAnimType.WipeRight);
            recB = new AUI_Rectangle(-16, -16, 16, 16, RecAnimType.WipeRight);
            recA.openingRec.W = recA.openingRec.H = 3;
            recA.openedRec.W = recA.openedRec.H = 3;
            recA.closedRec.W = recA.closedRec.H = 3;
            recB.openingRec.W = recB.openingRec.H = 3;
            recB.openedRec.W = recB.openedRec.H = 3;
            recB.closedRec.W = recB.closedRec.H = 3;
        }

        public override void Open()
        {
            if (displayState == DisplayState.Closed)
            {
                displayState = DisplayState.Opening;
                line.Open(); recA.Open();
                recA.alpha = 1.0f; recB.alpha = 0.0f;
            }
        }

        public override void Close()
        {
            if (displayState == DisplayState.Opened
                || displayState == DisplayState.Opening)
            {
                displayState = DisplayState.Closing;
                line.Close();
                recA.Close(); recA.alpha = 0.0f;
                recB.Close(); recB.alpha = 0.0f;
            }
        }

        public override void Update()
        {
            line.Update(); recA.Update(); recB.Update();

            if (displayState == DisplayState.Opening)
            {
                if (line.displayState == DisplayState.Opened)
                {
                    displayState = DisplayState.Opened;
                    recB.Open(); recB.alpha = 1.0f;
                }
            }
            else if (displayState == DisplayState.Opened) { }
            else if (displayState == DisplayState.Closing)
            {
                if (recA.alpha > 0.0f) { recA.alpha -= 0.05f; }
                else { recA.alpha = 0.0f; }
                if (recB.alpha > 0.0f) { recB.alpha -= 0.05f; }
                else { recB.alpha = 0.0f; }

                if (line.displayState == DisplayState.Closed
                    & recA.alpha == 0.0f & recB.alpha == 0.0f)
                { displayState = DisplayState.Closed; }
            }
            else if (displayState == DisplayState.Closed) { }

            //Debug.WriteLine("linewDots ds: " + displayState);
            //Debug.WriteLine("line ds: " + line.displayState);
            //Debug.WriteLine("recA ds: " + recA.displayState);
            //Debug.WriteLine("recB ds: " + recB.displayState);
        }

        public override void Draw()
        {
            line.Draw();
            recA.Draw();
            recB.Draw();
        }

        //

        public void MoveTo(int X, int Y)
        {
            recA.MoveTo(X - 1, Y - 1);
            line.SetAnchor(X, Y);
        }

        public void SetTarget(int X, int Y)
        {
            recB.MoveTo(X - 1, Y - 1);
            line.SetTarget(X, Y);
        }

        public void SetColor(Color Col)
        {
            line.color = Col;
            recA.color = Col;
            recB.color = Col;
        }

    }
}