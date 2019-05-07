using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AUI
{
    public class AUI_CrownButton : AUI_Base
    {
        public AUI_Button button;
        public Boolean childOpen = false; //reps child state open/closed
        public List<AUI_ButtonWithLine> aui_crown_children;
        public AUI_ButtonWithLine button_crown_child1;
        public AUI_ButtonWithLine button_crown_child2;
        public AUI_ButtonWithLine button_crown_child3;
        public AUI_ButtonWithLine button_crown_child4;
        public AUI_ButtonWithLine button_crown_child5;
        public AUI_ButtonWithLine button_crown_child6;

        int i;
        public Boolean wiggleChildren = true;


        public AUI_CrownButton(int X, int Y, int W, string Title)
        {
            button = new AUI_Button(
                X, Y, W, Title);
            button.CenterText();

            //setup crown children
            aui_crown_children = new List<AUI_ButtonWithLine>();
            button_crown_child1 = new AUI_ButtonWithLine(0, 0, 16 * 4, "test a");
            button_crown_child1.line.line.animType = LineAnimType.Reverse;
            button_crown_child1.button.draggable = true;
            aui_crown_children.Add(button_crown_child1);

            button_crown_child2 = new AUI_ButtonWithLine(0, 0, 16 * 4, "test b");
            button_crown_child2.line.line.animType = LineAnimType.Reverse;
            button_crown_child2.button.draggable = true;
            aui_crown_children.Add(button_crown_child2);

            button_crown_child3 = new AUI_ButtonWithLine(0, 0, 16 * 4, "test c");
            button_crown_child3.line.line.animType = LineAnimType.Reverse;
            button_crown_child3.button.draggable = true;
            aui_crown_children.Add(button_crown_child3);

            button_crown_child4 = new AUI_ButtonWithLine(0, 0, 16 * 4, "test d");
            button_crown_child4.line.line.animType = LineAnimType.Reverse;
            button_crown_child4.button.draggable = true;
            aui_crown_children.Add(button_crown_child4);

            button_crown_child5 = new AUI_ButtonWithLine(0, 0, 16 * 4, "test e");
            button_crown_child5.line.line.animType = LineAnimType.Reverse;
            button_crown_child5.button.draggable = true;
            aui_crown_children.Add(button_crown_child5);

            button_crown_child6 = new AUI_ButtonWithLine(0, 0, 16 * 4, "test f");
            button_crown_child6.line.line.animType = LineAnimType.Reverse;
            button_crown_child6.button.draggable = true;
            aui_crown_children.Add(button_crown_child6);

            //place crown children
            PlaceChildren();
            CloseChildren();
        }

        public override void Open()
        {
            button.Open();
            displayState = DisplayState.Opening;
        }

        public override void Close()
        {
            button.Close();
            CloseChildren();
            displayState = DisplayState.Closing;
        }

        public override void Update()
        {
            button.Update();
            
            if (wiggleChildren) { AnimateChildren(); }

            for (i = 0; i < aui_crown_children.Count; i++)
            { aui_crown_children[i].Update(); }

            //ui display states
            if (displayState == DisplayState.Opening)
            {
                if (button.displayState == DisplayState.Opened)
                {
                    displayState = DisplayState.Opened;
                }
            }
            else if (displayState == DisplayState.Opened)
            {
                //open/close crown children
                if (Input.IsLeftMouseBtnPress())
                {
                    if (Functions.Contains(
                        button.window.rec_bkg.openedRec,
                        Input.cursorPos.X, Input.cursorPos.Y))
                    {
                        //open/close crown's children
                        if (childOpen == false)
                        { OpenChildren(); }
                        else { CloseChildren(); }
                    }
                }
            }
            else if (displayState == DisplayState.Closing)
            {
                if (button.displayState == DisplayState.Closed)
                { displayState = DisplayState.Closed; }
            }
            else if (displayState == DisplayState.Closed) { }
        }

        public override void Draw()
        {
            button.Draw();
            for (i = 0; i < aui_crown_children.Count; i++)
            { aui_crown_children[i].Draw(); }
        }

        //

        public void AnimateChildren()
        {
            for (i = 0; i < aui_crown_children.Count; i++)
            { aui_crown_children[i].Float(); }
        }

        public void PlaceChildren()
        {
            //left half
            aui_crown_children[0].button.MoveTo(
                button.window.rec_bkg.openedRec.X - 16 * 4,
                button.window.rec_bkg.openedRec.Y - 16 * 4);
            aui_crown_children[0].line.MoveTo(
                button.window.rec_bkg.openedRec.X,
                button.window.rec_bkg.openedRec.Y);
            //setup offsets for button line
            aui_crown_children[0].offsetX =
                aui_crown_children[0].button.window.rec_bkg.openedRec.W;
            aui_crown_children[0].offsetY =
                aui_crown_children[0].button.window.rec_bkg.openedRec.H;

            aui_crown_children[1].button.MoveTo(
                button.window.rec_bkg.openedRec.X - 16 * 3,
                button.window.rec_bkg.openedRec.Y - 16 * 6);
            aui_crown_children[1].line.MoveTo(
                button.window.rec_bkg.openedRec.X + 16 * 1,
                button.window.rec_bkg.openedRec.Y);
            //setup offsets for button line
            aui_crown_children[1].offsetX =
                aui_crown_children[1].button.window.rec_bkg.openedRec.W;
            aui_crown_children[1].offsetY =
                aui_crown_children[1].button.window.rec_bkg.openedRec.H;

            aui_crown_children[2].button.MoveTo(
                button.window.rec_bkg.openedRec.X - 16 * 2,
                button.window.rec_bkg.openedRec.Y - 16 * 8);
            aui_crown_children[2].line.MoveTo(
                button.window.rec_bkg.openedRec.X + 16 * 2,
                button.window.rec_bkg.openedRec.Y);
            //setup offsets for button line
            aui_crown_children[2].offsetX =
                aui_crown_children[2].button.window.rec_bkg.openedRec.W;
            aui_crown_children[2].offsetY =
                aui_crown_children[2].button.window.rec_bkg.openedRec.H;

            //right half
            aui_crown_children[3].button.MoveTo(
                button.window.rec_bkg.openedRec.X +
                    button.window.rec_bkg.openedRec.W - 16 * 2,
                button.window.rec_bkg.openedRec.Y - 16 * 8);
            aui_crown_children[3].line.MoveTo(
                button.window.rec_bkg.openedRec.X +
                    button.window.rec_bkg.openedRec.W - 16 * 2,
                button.window.rec_bkg.openedRec.Y);

            aui_crown_children[4].button.MoveTo(
                button.window.rec_bkg.openedRec.X +
                    button.window.rec_bkg.openedRec.W - 16 * 1,
                button.window.rec_bkg.openedRec.Y - 16 * 6);
            aui_crown_children[4].line.MoveTo(
                button.window.rec_bkg.openedRec.X +
                    button.window.rec_bkg.openedRec.W - 16 * 1,
                button.window.rec_bkg.openedRec.Y);

            aui_crown_children[5].button.MoveTo(
                button.window.rec_bkg.openedRec.X +
                    button.window.rec_bkg.openedRec.W,
                button.window.rec_bkg.openedRec.Y - 16 * 4);
            aui_crown_children[5].line.MoveTo(
                button.window.rec_bkg.openedRec.X +
                    button.window.rec_bkg.openedRec.W,
                button.window.rec_bkg.openedRec.Y);
        }

        public void OpenChildren()
        {
            for (i = 0; i < aui_crown_children.Count; i++)
            { aui_crown_children[i].Open(); }
            childOpen = true;
        }

        public void CloseChildren()
        {
            for (i = 0; i < aui_crown_children.Count; i++)
            { aui_crown_children[i].Close(); }
            childOpen = false;
        }



    }
}
