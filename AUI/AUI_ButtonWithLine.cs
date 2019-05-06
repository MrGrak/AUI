using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AUI
{
    public class AUI_ButtonWithLine : AUI_Base
    {
        public AUI_Button button;
        public AUI_LineWithRecs line;
        public float floatX = 0;
        public float floatY = 0;

        public int offsetX = 0;
        public int offsetY = 0;

        public AUI_ButtonWithLine(int X, int Y, int W, string Txt)
        {
            button = new AUI_Button(X, Y, W, Txt);
            button.displayState = DisplayState.Closed;
            line = new AUI_LineWithRecs();
            line.displayState = DisplayState.Closed;
        }

        public override void Open()
        {
            line.Open();
        }

        public override void Close()
        {
            button.Close(); line.Close();
        }

        public override void Update()
        {
            button.Update();
            //match the target to the button each frame
            line.SetTarget(
                button.window.rec_bkg.openedRec.X + offsetX,
                button.window.rec_bkg.openedRec.Y + offsetY);
            //insta match the line's length (bypass animation)
            if (displayState == DisplayState.Opened)
            { line.line.animLength = line.line.length; }
            //else we allow the lines to close
            line.Update();

            //wait for line to complete opening before opening button
            if(line.displayState == DisplayState.Opened)
            { button.Open(); }

            //set display state based on instances
            if (button.displayState == DisplayState.Opening ||
                line.displayState == DisplayState.Opening)
            { displayState = DisplayState.Opening; }

            else if(button.displayState == DisplayState.Opened &
                line.displayState == DisplayState.Opened)
            { displayState = DisplayState.Opened; }

            else if (button.displayState == DisplayState.Closing ||
                line.displayState == DisplayState.Closing)
            { displayState = DisplayState.Closing; }

            else if (button.displayState == DisplayState.Closed &
                line.displayState == DisplayState.Closed)
            { displayState = DisplayState.Closed; }
        }

        public override void Draw()
        {
            if(displayState != DisplayState.Closed)
            { button.Draw(); line.Draw(); }
        }

        //
        public void Float()
        {
            if(Functions.Random.Next(0, 100) > 90)
            {
                int amnt = Functions.Random.Next(-1, 2);
                //float button
                button.MoveTo( //maybe add or subtract a pixel distance
                    (button.window.rec_bkg.openedRec.X + amnt),
                    (button.window.rec_bkg.openedRec.Y + amnt));
                //float line
                line.line.SetTarget(
                    (line.line.Xa + amnt),
                    (line.line.Ya + amnt));
                //float rec
                line.recB.MoveTo(
                    line.recB.openedRec.X + amnt,
                    line.recB.openedRec.Y + amnt);
            }
        }

        public void ResetFloat()
        {
            floatX = 0f; floatY = 0f;
        }

    }
}