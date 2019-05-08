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
using System.IO;
using System.Reflection;

namespace AUI
{
    //constants the ui needs to function and draw properly
    public static class Assets
    {
        public static GraphicsDeviceManager GDM;
        public static ContentManager CM;
        public static SpriteBatch SB;

        //contains color scheme and texture ui uses to draw
        public static Color GameBkgColor = new Color(35, 35, 35, 255);

        //Color.DimGray;
        public static Color ForegroundColor = Color.Gray;
        public static Color BackgroundColor = GameBkgColor;
        public static Color TextColor = Color.Gray;
        public static Color OverColor = Color.White;

        public static Texture2D recTex;
        public static SpriteFont font;

        public static float Layer_0 = 0.999990f; //furthest 'back'
        public static float Layer_Lines = 0.999980f;
        public static float Layer_WindowBack = 0.999970f;
        public static float Layer_WindowFront = 0.999960f;
        public static float Layer_Text = 0.999950f;



        public static void Load()
        {
            recTex = new Texture2D(GDM.GraphicsDevice, 1, 1);
            recTex.SetData<Color>(new Color[] { Color.White });
            font = CM.Load<SpriteFont>("pixelFont");
        }
    }

    public static class Input
    {   //there can only be 1 keyboard input at a time
        public static KeyboardState currentKeyboardState = new KeyboardState();
        public static KeyboardState lastKeyboardState = new KeyboardState();
        public static MouseState currentMouseState = new MouseState();
        public static MouseState lastMouseState = new MouseState();
        //all ui checks against cursorPos
        public static Vector2 cursorPos = new Vector2(0, 0);
        //^ makes collision checking much easier to do
        public static void Update()
        {   //store the last input state
            lastKeyboardState = currentKeyboardState;
            lastMouseState = currentMouseState;
            //get the current input states + cursor position
            currentKeyboardState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();
            cursorPos.X = currentMouseState.X;
            cursorPos.Y = currentMouseState.Y;
        }

        public static bool IsLeftMouseBtnPress()
        {   //check to see if L mouse button was pressed
            return (currentMouseState.LeftButton == ButtonState.Pressed &&
                    lastMouseState.LeftButton == ButtonState.Released);
        }

        public static bool IsRightMouseBtnPress()
        {   //check to see if L mouse button was pressed
            return (currentMouseState.RightButton == ButtonState.Pressed &&
                    lastMouseState.RightButton == ButtonState.Released);
        }

        public static bool IsNewKeyPress(Keys key)
        {
            return (currentKeyboardState.IsKeyDown(key)
                && lastKeyboardState.IsKeyUp(key));
        }
    }

    public class Int4
    {   //used to draw, animate, and collision check ui
        public int X, Y, W, H = 0;
        public Int4() { }
        public Int4(int x, int y, int w, int h)
        {
            X = x; Y = y;
            W = w; H = h;
        }
    }

    public enum DisplayState
    {
        Opening, Opened, Closing, Closed
    }

    public static class Functions
    {   //efficient drawing, collision checking, and random int generation
        public static Random Random = new Random();

        public static Boolean Contains(Int4 int4, float x, float y)
        {
            return ((((int4.X <= x) && (x < (int4.X + int4.W)))
                && (int4.Y <= y)) && (y < (int4.Y + int4.H)));
        }

        public static Boolean Contains(Rectangle Rec, float x, float y)
        {
            return ((((Rec.X <= x) && (x < (Rec.X + Rec.Width)))
                && (Rec.Y <= y)) && (y < (Rec.Y + Rec.Height)));
        }
        
        static Rectangle DrawRec = new Rectangle();
        public static void DrawInt4(SpriteBatch SB, Int4 int4, Color color, float alpha)
        {   //match draw rec to int4
            DrawRec.X = int4.X;
            DrawRec.Y = int4.Y;
            DrawRec.Width = int4.W;
            DrawRec.Height = int4.H;
            //draw with draw rec at color at alpha
            SB.Draw(Assets.recTex, DrawRec, color * alpha);
        }
    }

}