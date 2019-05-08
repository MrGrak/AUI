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
    public enum RecAnimType
    {
        WipeLeft, WipeRight
    }

    public class AUI_Rectangle : AUI_Base
    {
        public RecAnimType animType = RecAnimType.WipeRight;
        //can be set directly, or set to up/over refs
        public Color color = Assets.ForegroundColor;
        public Color colorUp = Assets.ForegroundColor;
        public Color colorOver = Assets.OverColor;

        public Boolean active = true; //flag for pool use
        public Int4 openingRec;
        public Int4 openedRec;
        public Int4 closedRec; //can point to opening rec
        public Rectangle drawRec = new Rectangle(); //drawn to screen
        public int speedOpen = 15, speedClosed = 15, i;
        public float alpha = 1.0f;
        public float zDepth = Assets.Layer_WindowBack; //draw over windows/recs
        public Vector2 texOrigin = new Vector2(0, 0);
        Rectangle texRec = new Rectangle(0, 0, 1, 1); //tex rec


        public AUI_Rectangle(int X, int Y, int W, int H, RecAnimType Type)
        {
            animType = Type;
            openedRec = new Int4(X, Y, W, H);

            if (animType == RecAnimType.WipeRight)
            {   //setup wipe right anim targets
                openingRec = new Int4(X, Y, 0, H);
                closedRec = new Int4(X + W, Y, 0, H);
            }
            else if (animType == RecAnimType.WipeLeft)
            {   //setup wipe left anim targets
                openingRec = new Int4(X + W, Y, 0, H);
                closedRec = new Int4(X, Y, 0, H);
            }

            //set draw to closed rec values
            drawRec.X = closedRec.X; drawRec.Y = closedRec.Y;
            drawRec.Width = closedRec.W; drawRec.Height = closedRec.H;
        }

        public override void Open()
        {
            if (displayState != DisplayState.Closed) { return; }
            displayState = DisplayState.Opening;
            drawRec.X = openingRec.X; drawRec.Y = openingRec.Y;
            drawRec.Width = openingRec.W; drawRec.Height = openingRec.H;
        }

        public override void Close()
        {
            if (displayState == DisplayState.Closed
                || displayState == DisplayState.Closing) { return; }
            displayState = DisplayState.Closing;
        }

        public override void Update()
        {
            if (displayState == DisplayState.Opening)
            {
                for (i = 0; i < speedOpen; i++) { Animate_Open(); }
                //check to see if draw rec matches opened rec params
                if (//if true, then this ui is open
                    drawRec.X == openedRec.X & drawRec.Y == openedRec.Y &
                    drawRec.Width == openedRec.W & drawRec.Height == openedRec.H
                    )//open children, allows cascading
                { displayState = DisplayState.Opened; }
            }
            else if (displayState == DisplayState.Opened) { }
            else if (displayState == DisplayState.Closing)
            {
                for (i = 0; i < speedClosed; i++) { Animate_Close(); }
                //check to see if draw rec matches closed rec params
                if (//if true, then this ui is closed
                    drawRec.X == closedRec.X & drawRec.Y == closedRec.Y &
                    drawRec.Width == closedRec.W & drawRec.Height == closedRec.H
                    )//close children, allows cascading
                { displayState = DisplayState.Closed; }
            }
            else if (displayState == DisplayState.Closed) { }
        }

        public override void Draw()
        {
            //zDepth
            Assets.SB.Draw(
                Assets.recTex,
                drawRec, //draw rec
                texRec, //texture rec
                color * alpha, 
                0.0f,
                texOrigin,
                SpriteEffects.None,
                zDepth
            );
        }

        //

        public void MoveTo(int X, int Y)
        {
            //set all to pos
            openedRec.X = X; openedRec.Y = Y;
            closedRec.X = X; closedRec.Y = Y;
            openingRec.X = X; openingRec.Y = Y;
            //assume fn called prior to open(), drawrec to opening rec
            drawRec.X = openingRec.X; drawRec.Y = openingRec.Y;
        }

        private void Animate_Open()
        {   //animate draw rec pos to opened rec pos
            if (drawRec.X < openedRec.X) { drawRec.X++; }
            else if (drawRec.X > openedRec.X) { drawRec.X--; }
            if (drawRec.Y < openedRec.Y) { drawRec.Y++; }
            else if (drawRec.Y > openedRec.Y) { drawRec.Y--; }
            //animate draw rec size to opened rec size
            if (drawRec.Width < openedRec.W) { drawRec.Width++; }
            else if (drawRec.Width > openedRec.W) { drawRec.Width--; }
            if (drawRec.Height < openedRec.H) { drawRec.Height++; }
            else if (drawRec.Height > openedRec.H) { drawRec.Height--; }
            Snap(openedRec, speedOpen);
        }

        private void Animate_Close()
        {   //animate draw rec pos to closed rec pos
            if (drawRec.X < closedRec.X) { drawRec.X++; }
            else if (drawRec.X > closedRec.X) { drawRec.X--; }
            if (drawRec.Y < closedRec.Y) { drawRec.Y++; }
            else if (drawRec.Y > closedRec.Y) { drawRec.Y--; }
            //animate draw rec size to closed rec size
            if (drawRec.Width < closedRec.W) { drawRec.Width++; }
            else if (drawRec.Width > closedRec.W) { drawRec.Width--; }
            if (drawRec.Height < closedRec.H) { drawRec.Height++; }
            else if (drawRec.Height > closedRec.H) { drawRec.Height--; }
            Snap(closedRec, speedClosed);
        }

        private void Snap(Int4 targetRec, int speed)
        {   //snap if we get close enough
            if (Math.Abs(drawRec.X - targetRec.X) <= speed)
            { drawRec.X = targetRec.X; }
            if (Math.Abs(drawRec.Y - targetRec.Y) <= speed)
            { drawRec.Y = targetRec.Y; }
            if (Math.Abs(drawRec.Width - targetRec.W) <= speed)
            { drawRec.Width = targetRec.W; }
            if (Math.Abs(drawRec.Height - targetRec.H) <= speed)
            { drawRec.Height = targetRec.H; }
        }

    }
}