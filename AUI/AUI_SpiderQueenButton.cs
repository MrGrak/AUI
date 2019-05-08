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
    public class AUI_SpiderQueenButton : AUI_Base
    {
        //button 
        //8 legs (just lines)
        public AUI_Button button;
        public List<AUI_Line> lines;
        int i;


        //behavior:
        //randomly walks around, using it's legs
        //when clicked on, it spawns a baby spider
        //and moves around for awhile
        public Boolean wandering = false;
        public int wanderTotal = 300;
        public int wanderCounter = 0;



        public AUI_SpiderQueenButton(int X, int Y, String Text)
        {
            button = new AUI_Button(X, Y, 16 * 5, Text);
            lines = new List<AUI_Line>();
            //create 8 legs, update will match their positions
            for(i = 0; i < 8; i++)
            {
                AUI_Line line = new AUI_Line();
                lines.Add(line);
            }
            displayState = DisplayState.Closed;
        }

        public override void Open()
        {
            button.Open();
            displayState = DisplayState.Opening;
        }

        public override void Close()
        {
            button.Close();
            //close all legs
            for (i = 0; i < 8; i++) { lines[i].Close(); }
            displayState = DisplayState.Closing;
        }

        public override void Update()
        {
            button.Update();
            for (i = 0; i < 8; i++) { lines[i].Update(); }


            



            //display states
            if (displayState == DisplayState.Opening)
            {
                if (button.displayState == DisplayState.Opened)
                { displayState = DisplayState.Opened; }
            }
            else if (displayState == DisplayState.Opened)
            {

                #region Button Click Input

                //on click, spider wanders around
                if (Input.IsLeftMouseBtnPress())
                {
                    if (Functions.Contains(
                        button.window.rec_bkg.openedRec,
                        Input.cursorPos.X, Input.cursorPos.Y))
                    {
                        if (wandering == false)
                        {
                            //open all legs start wandering around
                            for (i = 0; i < 8; i++) { lines[i].Open(); }
                            wandering = true;
                        }
                    }
                }

                #endregion

                #region Wandering Routine

                //if spider is wandering around, the legs should be
                //open and wiggling about
                if (wandering)
                {   //randomly choose a target to move button to
                    button.MoveTo(
                        button.window.rec_bkg.openedRec.X + Functions.Random.Next(-1, 2),
                        button.window.rec_bkg.openedRec.Y + Functions.Random.Next(-1, 2));
                    
                    //count wandering frames
                    wanderCounter++;
                    if(wanderCounter > wanderTotal)
                    {   //goto resting state
                        wanderCounter = 0;
                        wandering = false;
                        //close all legs
                        for (i = 0; i < 8; i++) { lines[i].Close(); }
                    }
                }

                #endregion

            }
            else if (displayState == DisplayState.Closing)
            {
                if (button.displayState == DisplayState.Closed)
                { displayState = DisplayState.Closed; }
            }
            else if (displayState == DisplayState.Closed) { }





            //place legs onto button (after button has moved)
            if (displayState != DisplayState.Closed)
            {
                #region Anchor legs to button

                //top of button
                lines[0].SetAnchor(
                    button.window.rec_bkg.openedRec.X + 8,
                    button.window.rec_bkg.openedRec.Y);
                lines[1].SetAnchor(
                    button.window.rec_bkg.openedRec.X + 24,
                    button.window.rec_bkg.openedRec.Y);
                lines[2].SetAnchor(
                    button.window.rec_bkg.openedRec.X + 40,
                    button.window.rec_bkg.openedRec.Y);
                lines[3].SetAnchor(
                    button.window.rec_bkg.openedRec.X + 56,
                    button.window.rec_bkg.openedRec.Y);
                //bottom of button
                lines[4].SetAnchor(
                    button.window.rec_bkg.openedRec.X + 8,
                    button.window.rec_bkg.openedRec.Y + 16);
                lines[5].SetAnchor(
                    button.window.rec_bkg.openedRec.X + 24,
                    button.window.rec_bkg.openedRec.Y + 16);
                lines[6].SetAnchor(
                    button.window.rec_bkg.openedRec.X + 40,
                    button.window.rec_bkg.openedRec.Y + 16);
                lines[7].SetAnchor(
                    button.window.rec_bkg.openedRec.X + 56,
                    button.window.rec_bkg.openedRec.Y + 16);
                #endregion


                #region Place legs away from body like spider

                //top of button
                lines[0].SetTarget(
                    button.window.rec_bkg.openedRec.X + 5,
                    button.window.rec_bkg.openedRec.Y - 16 * 2);
                lines[1].SetTarget(
                    button.window.rec_bkg.openedRec.X + 21,
                    button.window.rec_bkg.openedRec.Y - 16 * 2);
                lines[2].SetTarget(
                    button.window.rec_bkg.openedRec.X + 43,
                    button.window.rec_bkg.openedRec.Y - 16 * 2);
                lines[3].SetTarget(
                    button.window.rec_bkg.openedRec.X + 60,
                    button.window.rec_bkg.openedRec.Y - 16 * 2);
                //bottom of button
                lines[4].SetTarget(
                    button.window.rec_bkg.openedRec.X + 5,
                    button.window.rec_bkg.openedRec.Y + 16 + 16 * 2);
                lines[5].SetTarget(
                    button.window.rec_bkg.openedRec.X + 21,
                    button.window.rec_bkg.openedRec.Y + 16 + 16 * 2);
                lines[6].SetTarget(
                    button.window.rec_bkg.openedRec.X + 43,
                    button.window.rec_bkg.openedRec.Y + 16 + 16 * 2);
                lines[7].SetTarget(
                    button.window.rec_bkg.openedRec.X + 60,
                    button.window.rec_bkg.openedRec.Y + 16 + 16 * 2);

                #endregion


                if(wandering)
                {   //modify leg positions each frame as if moving
                    for (i = 0; i < 8; i++)
                    {
                        lines[i].SetTarget(
                            lines[i].Xa + Functions.Random.Next(-1, 2),
                            lines[i].Ya + Functions.Random.Next(-1, 2));
                    }
                }
            }


        }

        public override void Draw()
        {
            button.Draw();
            for (i = 0; i < 8; i++) { lines[i].Draw(); }
        }

        //

        public void SpawnChild()
        {
            //create a baby spider button
        }



    }
}