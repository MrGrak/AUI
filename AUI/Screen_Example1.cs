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
    //this screen shows off the crown button
    public class Screen_Example1 : Screen
    {
        int i;
        public List<AUI_Base> aui_instances;
        AUI_Button button_back;
        AUI_Button button_crown_base;

        public List<AUI_ButtonWithLine> aui_crown_children;
        AUI_ButtonWithLine button_crown_child1;
        AUI_ButtonWithLine button_crown_child2;
        AUI_ButtonWithLine button_crown_child3;
        AUI_ButtonWithLine button_crown_child4;
        AUI_ButtonWithLine button_crown_child5;
        AUI_ButtonWithLine button_crown_child6;
        Boolean childOpen = false; //reps child state open/closed

        public Screen_Example1()
        {
            aui_instances = new List<AUI_Base>();

            button_back = new AUI_Button(
                16 * 3, 16 * 2 + 8, 16 * 3, "< to title");
            button_back.CenterText();
            aui_instances.Add(button_back);

            //setup crown button
            button_crown_base = new AUI_Button(
                16 * 18, 16 * 22, 16 * 12, "crown button");
            button_crown_base.CenterText();
            aui_instances.Add(button_crown_base);

            //setup crown children
            aui_crown_children = new List<AUI_ButtonWithLine>();
            button_crown_child1 = new AUI_ButtonWithLine(0, 0, 16 * 4, "test a");
            button_crown_child1.line.line.animType = LineAnimType.Reverse;
            aui_crown_children.Add(button_crown_child1);

            button_crown_child2 = new AUI_ButtonWithLine(0, 0, 16 * 4, "test b");
            button_crown_child2.line.line.animType = LineAnimType.Reverse;
            aui_crown_children.Add(button_crown_child2);

            button_crown_child3 = new AUI_ButtonWithLine(0, 0, 16 * 8, "test c");
            button_crown_child3.line.line.animType = LineAnimType.Reverse;
            aui_crown_children.Add(button_crown_child3);

            button_crown_child4 = new AUI_ButtonWithLine(0, 0, 16 * 7, "test d");
            button_crown_child4.line.line.animType = LineAnimType.Reverse;
            aui_crown_children.Add(button_crown_child4);

            button_crown_child5 = new AUI_ButtonWithLine(0, 0, 16 * 4, "test e");
            button_crown_child5.line.line.animType = LineAnimType.Reverse;
            aui_crown_children.Add(button_crown_child5);

            button_crown_child6 = new AUI_ButtonWithLine(0, 0, 16 * 4, "test f");
            button_crown_child6.line.line.animType = LineAnimType.Reverse;
            aui_crown_children.Add(button_crown_child6);

            PlaceChildren();
            CloseCrownChildren();
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
            //close any open children too
            for (i = 0; i < aui_crown_children.Count; i++)
            { aui_crown_children[i].Close(); }
        }

        public override void Update()
        {
            //update all ui items
            for (i = 0; i < aui_instances.Count; i++)
            { aui_instances[i].Update(); }
            for (i = 0; i < aui_crown_children.Count; i++)
            { aui_crown_children[i].Update(); }

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

                #region Back Btn Interaction

                if (Functions.Contains(
                    button_back.window.rec_bkg.openedRec,
                    Input.cursorPos.X, Input.cursorPos.Y))
                {   //give button focus
                    button_back.focused = true;
                    //check for new left click
                    if (Input.IsLeftMouseBtnPress())
                    { Close(ExitAction.Title); }
                }
                else { button_back.focused = false; }

                #endregion

                #region Crown Btn Interaction

                if (Functions.Contains(
                    button_crown_base.window.rec_bkg.openedRec,
                    Input.cursorPos.X, Input.cursorPos.Y))
                {   //give button focus
                    button_crown_base.focused = true;
                    //check for new left click
                    if (Input.IsLeftMouseBtnPress())
                    {   //open/close crown's children
                        if (childOpen == false)
                        { OpenCrownChildren(); }
                        else { CloseCrownChildren(); }
                    }
                }
                else { button_crown_base.focused = false; }

                #endregion

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
        {   //draw all ui items
            for (i = 0; i < aui_instances.Count; i++)
            { aui_instances[i].Draw(); }
            for (i = 0; i < aui_crown_children.Count; i++)
            { aui_crown_children[i].Draw(); }
        }

        //

        public void PlaceChildren()
        {
            //place crown ui buttons upon open
            aui_crown_children[0].button.MoveTo(
                button_crown_base.window.rec_bkg.openedRec.X - 16 * 4,
                button_crown_base.window.rec_bkg.openedRec.Y - 16 * 4);
            aui_crown_children[0].line.SetTarget(
                button_crown_base.window.rec_bkg.openedRec.X - 16 * 0,
                button_crown_base.window.rec_bkg.openedRec.Y - 16 * 3);
            aui_crown_children[0].line.MoveTo(
                button_crown_base.window.rec_bkg.openedRec.X,
                button_crown_base.window.rec_bkg.openedRec.Y);

            aui_crown_children[1].button.MoveTo(
                button_crown_base.window.rec_bkg.openedRec.X - 16 * 3,
                button_crown_base.window.rec_bkg.openedRec.Y - 16 * 6);
            aui_crown_children[1].line.SetTarget(
                button_crown_base.window.rec_bkg.openedRec.X + 16 * 1,
                button_crown_base.window.rec_bkg.openedRec.Y - 16 * 5);
            aui_crown_children[1].line.MoveTo(
                button_crown_base.window.rec_bkg.openedRec.X + 16 * 1,
                button_crown_base.window.rec_bkg.openedRec.Y);

            aui_crown_children[2].button.MoveTo(
                button_crown_base.window.rec_bkg.openedRec.X + 16 * 2,
                button_crown_base.window.rec_bkg.openedRec.Y - 16 * 8);
            aui_crown_children[2].line.SetTarget(
                button_crown_base.window.rec_bkg.openedRec.X + 16 * 2,
                button_crown_base.window.rec_bkg.openedRec.Y - 16 * 7);
            aui_crown_children[2].line.MoveTo(
                button_crown_base.window.rec_bkg.openedRec.X + 16 * 2,
                button_crown_base.window.rec_bkg.openedRec.Y);

            aui_crown_children[3].button.MoveTo(
                button_crown_base.window.rec_bkg.openedRec.X + 16 * 3,
                button_crown_base.window.rec_bkg.openedRec.Y - 16 * 6);
            aui_crown_children[3].line.SetTarget(
                button_crown_base.window.rec_bkg.openedRec.X + 16 * 10,
                button_crown_base.window.rec_bkg.openedRec.Y - 16 * 5);
            aui_crown_children[3].line.MoveTo(
                button_crown_base.window.rec_bkg.openedRec.X + 16 * 10,
                button_crown_base.window.rec_bkg.openedRec.Y);

            aui_crown_children[4].button.MoveTo(
                button_crown_base.window.rec_bkg.openedRec.X + 16 * 11,
                button_crown_base.window.rec_bkg.openedRec.Y - 16 * 6);
            aui_crown_children[4].line.SetTarget(
                button_crown_base.window.rec_bkg.openedRec.X + 16 * 11,
                button_crown_base.window.rec_bkg.openedRec.Y - 16 * 5);
            aui_crown_children[4].line.MoveTo(
                button_crown_base.window.rec_bkg.openedRec.X + 16 * 11,
                button_crown_base.window.rec_bkg.openedRec.Y);

            aui_crown_children[5].button.MoveTo(
                button_crown_base.window.rec_bkg.openedRec.X + 16 * 12,
                button_crown_base.window.rec_bkg.openedRec.Y - 16 * 4);
            aui_crown_children[5].line.SetTarget(
                button_crown_base.window.rec_bkg.openedRec.X + 16 * 12,
                button_crown_base.window.rec_bkg.openedRec.Y - 16 * 3);
            aui_crown_children[5].line.MoveTo(
                button_crown_base.window.rec_bkg.openedRec.X + 16 * 12,
                button_crown_base.window.rec_bkg.openedRec.Y);
        }





        public void OpenCrownChildren()
        {   
            for (i = 0; i < aui_crown_children.Count; i++)
            { aui_crown_children[i].Open(); }
            childOpen = true;
        }

        public void CloseCrownChildren()
        {
            for (i = 0; i < aui_crown_children.Count; i++)
            { aui_crown_children[i].Close(); }
            childOpen = false;
        }


    }

}