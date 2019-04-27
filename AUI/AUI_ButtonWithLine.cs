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
            button.Update(); line.Update();
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

    }
}