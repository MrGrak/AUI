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
    public enum LineAnimType
    {
        WipeRight, FadeInOut
    }

    public class AUI_Line : AUI_Base
    {
        public int Xa, Ya, Xb, Yb;
        public Color color = Assets.ForegroundColor;
        public bool active = true;
        float angle = 0.0f;
        public int animLength = 0, length = 0;
        public Rectangle drawRec = new Rectangle(0, 0, 0, 1); //drawn to screen
        Rectangle texRec = new Rectangle(0, 0, 1, 1); //tex rec
        Vector2 texOrigin = new Vector2(0, 0);
        public int speedOpen = 15, speedClosed = 15, i;
        public LineAnimType animType = LineAnimType.WipeRight;
        public float alpha = 1.0f;

        public AUI_Line() { Xa = Ya = Xb = Yb = 0; }

        public AUI_Line(int startX, int startY, int endX, int endY)
        {
            Xa = startX; Ya = startY;
            Xb = endX; Yb = endY;
        }

        public override void Open()
        {
            if (displayState != DisplayState.Closed) { return; }
            displayState = DisplayState.Opening;
            animLength = 0; GetLength();
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
                if(animType == LineAnimType.WipeRight)
                {   //animate to open state
                    for (i = 0; i < speedOpen; i++) { animLength++; }
                    if (animLength >= length)
                    {   //check for opened state
                        animLength = length;
                        displayState = DisplayState.Opened;
                    }
                }
                else if(animType == LineAnimType.FadeInOut)
                {   //animate to open state
                    if(alpha > 100)
                    {
                        alpha = 100;
                        displayState = DisplayState.Opened;
                    }
                    else
                    {   //alpha < 100, fade in
                        alpha += speedOpen * 0.1f;
                    }
                }
            }
            else if (displayState == DisplayState.Opened) { }
            else if (displayState == DisplayState.Closing)
            {
                if (animType == LineAnimType.WipeRight)
                {   //animate to closed state
                    for (i = 0; i < speedClosed; i++)
                    { animLength--; Xb++; }
                    if (animLength <= 0)
                    {   //check for closed state
                        animLength = 0;
                        displayState = DisplayState.Closed;
                    }
                }
                else if (animType == LineAnimType.FadeInOut)
                {   //animate to closed state
                    if (alpha < 0.11f)
                    {
                        alpha = 0;
                        displayState = DisplayState.Closed;
                    }
                    else
                    {   //alpha > 0, fade out
                        alpha -= speedClosed * 0.1f;
                    }
                }
            }
            else if (displayState == DisplayState.Closed) { }
        }

        public override void Draw()
        {
            drawRec = new Rectangle(Xb, Yb, animLength, 1);
            Assets.SB.Draw(
                Assets.recTex,
                drawRec, //draw rec
                texRec, //texture rec
                color, //at 100% alpha
                angle,
                texOrigin, //wat is this Vector2 origin parameter?
                SpriteEffects.None,
                Assets.Layer_0
            );
        }

        //

        public void GetLength()
        {
            angle = (float)Math.Atan2((Ya - Yb), (Xa - Xb));
            //set length
            length = (int)Vector2.Distance(
                new Vector2(Xb, Yb),
                new Vector2(Xa, Ya));
        }

        public void SetAnchor(int X, int Y)
        {
            Xb = X; Yb = Y;
            GetLength();
        }

        public void SetTarget(int X, int Y)
        {
            Xa = X; Ya = Y;
            GetLength();
        }

    }
}