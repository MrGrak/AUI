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
    public class AUI_Text
    {
        public DisplayState displayState;
        public SpriteFont font;
        public String text;
        String temp;
        public String drawText;
        public Vector2 position;
        public Color color;
        public Color color_over;
        public Color color_up;
        public float alpha = 1.0f;
        public float rotation = 0.0f;
        public float scale = 1.0f;
        public float zDepth = Assets.Layer_3; //draw over recs
        public int speedOpen = 2; //chars per frame
        public float speedClosed = 0.2f; //alpha fade out per frame
        public Int4 hitbox = new Int4();
        int i;

        public AUI_Text(String Text, float X, float Y, Color Color)
        {   //calling code is responsible for setting text and hitbox size
            font = Assets.font;
            position = new Vector2(X, Y);
            hitbox.X = (int)X; hitbox.Y = (int)Y;
            text = Text; drawText = "";
            color = Color;
            color_over = Color;
            color_up = Color;
            displayState = DisplayState.Closed;
        }

        public void Open()
        {
            if (displayState == DisplayState.Opened
                || displayState == DisplayState.Opening) { return; }
            //clear anim text
            drawText = ""; temp = "" + text;
            displayState = DisplayState.Opening;
            alpha = 1.0f;
        }

        public void Close()
        {
            if (displayState == DisplayState.Closed
                || displayState == DisplayState.Closing) { return; }
            displayState = DisplayState.Closing;
            alpha = 1.0f;
        }

        public void Update()
        {
            if (displayState == DisplayState.Opening)
            {   //animate to open state
                for (i = 0; i < speedOpen; i++)
                {
                    if (temp.Length > 0)
                    {   //add first letter to text, then remove
                        drawText += temp[0].ToString();
                        temp = temp.Remove(0, 1);
                    }
                    else { displayState = DisplayState.Opened; }
                }
            }
            else if (displayState == DisplayState.Opened) { }
            else if (displayState == DisplayState.Closing)
            {   //animate to closed state
                if (alpha > 0.0f) { alpha -= speedClosed; }
                else
                {
                    alpha = 0.0f;
                    displayState = DisplayState.Closed;
                }
            }
            else if (displayState == DisplayState.Closed) { }
        }

        public void Draw(SpriteBatch SB)
        {
            if (font != null)
            {
                SB.DrawString(font, drawText, position,
                    color * alpha, rotation, Vector2.Zero,
                    scale, SpriteEffects.None, zDepth);
            }
        }

        //

        public Boolean Contains(int X, int Y)
        {   //if hitbox contains X,Y
            return ((((hitbox.X <= X) && (X < (hitbox.X + hitbox.W)))
                && (hitbox.Y <= Y)) && (Y < (hitbox.Y + hitbox.H)));
        }

        public void ChangeText(String Text)
        {
            text = Text;
            displayState = DisplayState.Closed;
            Open();
        }
    }

}