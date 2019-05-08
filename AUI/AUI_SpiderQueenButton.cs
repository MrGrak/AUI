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
        public int wanderTotal = 60;
        public int wanderCounter = 0;
        
        //setup way to move button in a direction
        public Vector2 magnitude = new Vector2();
        public float friction = 0.96f;
        public byte movement = 4;

        List<AUI_Base> ListRef;
        int babyCounter = 0;

        public AUI_SpiderQueenButton(int X, int Y, 
            String Text, List<AUI_Base> LR)
        {
            button = new AUI_Button(X, Y, 16 * 4, Text);
            button.CenterText();

            lines = new List<AUI_Line>();
            //create 8 legs, update will match their positions
            for(i = 0; i < 8; i++)
            {
                AUI_Line line = new AUI_Line();
                line.displayState = DisplayState.Closed;
                lines.Add(line);
            }
            displayState = DisplayState.Closed;
            ListRef = LR;
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

            //miniphysics: apply friction to magnitude per axis
            magnitude.X = magnitude.X * friction;
            magnitude.Y = magnitude.Y * friction;

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
                            ChooseDirection();
                        }
                        //spawn a child as feedback
                        SpawnChildren();
                    }
                }

                #endregion

                #region Wandering Routine

                //if spider is wandering around, the legs should be
                //open and wiggling about
                if (wandering)
                {   //randomly choose a target to move button to
                    button.MoveTo(
                        (int)(button.window.rec_bkg.openedRec.X + magnitude.X),
                        (int)(button.window.rec_bkg.openedRec.Y + magnitude.Y));
                    
                    //count wandering frames
                    wanderCounter++;
                    if(wanderCounter > wanderTotal)
                    {   //goto resting state
                        wanderCounter = 0;
                        wandering = false;
                        //close all legs
                        for (i = 0; i < 8; i++) { lines[i].Close(); }
                    }

                    //randomly modify moving direction
                    if(Functions.Random.Next(0,101) > 98)
                    {
                        ChooseDirection();
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


            
            //work that happens when button isn't closed
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
                    button.window.rec_bkg.openedRec.Y - 8);
                lines[1].SetTarget(
                    button.window.rec_bkg.openedRec.X + 21,
                    button.window.rec_bkg.openedRec.Y - 8);
                lines[2].SetTarget(
                    button.window.rec_bkg.openedRec.X + 43,
                    button.window.rec_bkg.openedRec.Y - 8);
                lines[3].SetTarget(
                    button.window.rec_bkg.openedRec.X + 60,
                    button.window.rec_bkg.openedRec.Y - 8);
                //bottom of button
                lines[4].SetTarget(
                    button.window.rec_bkg.openedRec.X + 5,
                    button.window.rec_bkg.openedRec.Y + 16 + 8);
                lines[5].SetTarget(
                    button.window.rec_bkg.openedRec.X + 21,
                    button.window.rec_bkg.openedRec.Y + 16 + 8);
                lines[6].SetTarget(
                    button.window.rec_bkg.openedRec.X + 43,
                    button.window.rec_bkg.openedRec.Y + 16 + 8);
                lines[7].SetTarget(
                    button.window.rec_bkg.openedRec.X + 60,
                    button.window.rec_bkg.openedRec.Y + 16 + 8);

                #endregion

                #region leg animation routine

                if(wandering)
                {
                    //sort button over other buttons
                    button.window.rec_bkg.zDepth = Assets.Layer_WindowFront;
                    button.window.rec_fore.zDepth = Assets.Layer_WindowFront;

                    //modify leg positions each frame as if moving
                    for (i = 0; i < 8; i++)
                    {
                        lines[i].SetTarget(
                            lines[i].Xa + Functions.Random.Next(-4, 5),
                            lines[i].Ya + Functions.Random.Next(-4, 5));
                        lines[i].zDepth = Assets.Layer_WindowFront;
                    }
                }

                #endregion

                else
                {   //set button and lines on lower layers (behind)
                    button.window.rec_bkg.zDepth = Assets.Layer_WindowBack;
                    button.window.rec_fore.zDepth = Assets.Layer_WindowBack;
                    for (i = 0; i < 8; i++)
                    {
                        lines[i].zDepth = Assets.Layer_Lines;
                    }
                }

                #region Keep button onscreen (push back to center)

                if (button.window.rec_bkg.openedRec.X > Assets.GDM.PreferredBackBufferWidth)
                { magnitude.X = -3; }
                if (button.window.rec_bkg.openedRec.X < 0)
                { magnitude.X = 3; }
                if (button.window.rec_bkg.openedRec.Y > Assets.GDM.PreferredBackBufferHeight)
                { magnitude.Y = -3; }
                if (button.window.rec_bkg.openedRec.Y < 0)
                { magnitude.Y = 3; }

                #endregion

            }


        }

        public override void Draw()
        {
            for (i = 0; i < 8; i++) { lines[i].Draw(); }
            button.Draw();
        }

        //

        public void ChooseDirection()
        {   
            //set a magnitude direction randomly
            magnitude.X += Functions.Random.Next(-movement, movement+2);
            magnitude.Y += Functions.Random.Next(-movement, movement+2);
            //orig way returns values close to 0, making movement hard to predict

            /*
            //set initial positive values > 0
            magnitude.X = Functions.Random.Next(1, movement + 1);
            magnitude.Y = Functions.Random.Next(1, movement + 1);
            //randomly invert (per axis)
            if (Functions.Random.Next(0,101) > 50)
            { magnitude.X *= -1; }
            if (Functions.Random.Next(0, 101) > 50)
            { magnitude.Y *= -1; }
            */
        }

        public void SpawnChildren()
        {
            //create spider babies
            for(int g = 0; g < 30; g++)
            {
                babyCounter++;
                //create a baby spider button
                AUI_SpiderBabyButton spiderBaby =
                    new AUI_SpiderBabyButton(
                        button.window.rec_bkg.openedRec.X + 16 * 2,
                        button.window.rec_bkg.openedRec.Y,
                        "" + babyCounter, ListRef);
                spiderBaby.button.CenterText();
                spiderBaby.Open();
                spiderBaby.wandering = true;
                spiderBaby.ChooseDirection();
                ListRef.Add(spiderBaby);
            }
        }





    }
}