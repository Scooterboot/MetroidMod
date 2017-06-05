using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Terraria;

namespace MetroidMod
{
    class UIParameters
    {
        public static string MODNAME = "MetroidMod";
        public static KeyboardState oldState;
        public static KeyboardState newState;
        public static MouseState mouseState;
        public static MouseState lastMouseState;
        public static int backSpace = 0;

        public static Rectangle mouseRect
        {
            get
            {
                return new Rectangle(Main.mouseX, Main.mouseY, 1, 1);
            }
        }

        public static Vector2 mousePos
        {
            get
            {
                return new Vector2(Main.mouseX, Main.mouseY);
            }
        }

        public static string TranslateChar(Keys key, bool shift, bool capsLock, bool numLock)
        {
            switch (key)
            {
                case Keys.A: return TranslateAlphabetic('a', shift, capsLock);
                case Keys.B: return TranslateAlphabetic('b', shift, capsLock);
                case Keys.C: return TranslateAlphabetic('c', shift, capsLock);
                case Keys.D: return TranslateAlphabetic('d', shift, capsLock);
                case Keys.E: return TranslateAlphabetic('e', shift, capsLock);
                case Keys.F: return TranslateAlphabetic('f', shift, capsLock);
                case Keys.G: return TranslateAlphabetic('g', shift, capsLock);
                case Keys.H: return TranslateAlphabetic('h', shift, capsLock);
                case Keys.I: return TranslateAlphabetic('i', shift, capsLock);
                case Keys.J: return TranslateAlphabetic('j', shift, capsLock);
                case Keys.K: return TranslateAlphabetic('k', shift, capsLock);
                case Keys.L: return TranslateAlphabetic('l', shift, capsLock);
                case Keys.M: return TranslateAlphabetic('m', shift, capsLock);
                case Keys.N: return TranslateAlphabetic('n', shift, capsLock);
                case Keys.O: return TranslateAlphabetic('o', shift, capsLock);
                case Keys.P: return TranslateAlphabetic('p', shift, capsLock);
                case Keys.Q: return TranslateAlphabetic('q', shift, capsLock);
                case Keys.R: return TranslateAlphabetic('r', shift, capsLock);
                case Keys.S: return TranslateAlphabetic('s', shift, capsLock);
                case Keys.T: return TranslateAlphabetic('t', shift, capsLock);
                case Keys.U: return TranslateAlphabetic('u', shift, capsLock);
                case Keys.V: return TranslateAlphabetic('v', shift, capsLock);
                case Keys.W: return TranslateAlphabetic('w', shift, capsLock);
                case Keys.X: return TranslateAlphabetic('x', shift, capsLock);
                case Keys.Y: return TranslateAlphabetic('y', shift, capsLock);
                case Keys.Z: return TranslateAlphabetic('z', shift, capsLock);

                case Keys.D0: return (shift) ? ")" : "0";
                case Keys.D1: return (shift) ? "!" : "1";
                case Keys.D2: return (shift) ? "@" : "2";
                case Keys.D3: return (shift) ? "#" : "3";
                case Keys.D4: return (shift) ? "$" : "4";
                case Keys.D5: return (shift) ? "%" : "5";
                case Keys.D6: return (shift) ? "^" : "6";
                case Keys.D7: return (shift) ? "&" : "7";
                case Keys.D8: return (shift) ? "*" : "8";
                case Keys.D9: return (shift) ? "(" : "9";

                case Keys.Add: return "+";
                case Keys.Divide: return "/";
                case Keys.Multiply: return "*";
                case Keys.Subtract: return "-";

                case Keys.Space: return " ";
                case Keys.Tab: return "    ";

                case Keys.Decimal: if (numLock && !shift) return "."; break;
                case Keys.NumPad0: if (numLock && !shift) return "0"; break;
                case Keys.NumPad1: if (numLock && !shift) return "1"; break;
                case Keys.NumPad2: if (numLock && !shift) return "2"; break;
                case Keys.NumPad3: if (numLock && !shift) return "3"; break;
                case Keys.NumPad4: if (numLock && !shift) return "4"; break;
                case Keys.NumPad5: if (numLock && !shift) return "5"; break;
                case Keys.NumPad6: if (numLock && !shift) return "6"; break;
                case Keys.NumPad7: if (numLock && !shift) return "7"; break;
                case Keys.NumPad8: if (numLock && !shift) return "8"; break;
                case Keys.NumPad9: if (numLock && !shift) return "9"; break;

                case Keys.OemBackslash: return shift ? "|" : "\\";
                case Keys.OemCloseBrackets: return shift ? "}" : "]";
                case Keys.OemComma: return shift ? "<" : ",";
                case Keys.OemMinus: return shift ? "_" : "-";
                case Keys.OemOpenBrackets: return shift ? "{" : "[";
                case Keys.OemPeriod: return shift ? ">" : ".";
                case Keys.OemPipe: return shift ? "|" : "\\";
                case Keys.OemPlus: return shift ? "+" : "=";
                case Keys.OemQuestion: return shift ? "?" : "/";
                case Keys.OemQuotes: return shift ? "\"" : "\'";
                case Keys.OemSemicolon: return shift ? ":" : ";";
                case Keys.OemTilde: return shift ? "~" : "`";
            }
            return "";
        }

        //This is stolen from a forum somewhere but who cares it works and I love it

        public static string TranslateAlphabetic(char baseChar, bool shift, bool capsLock)
        {
            return (capsLock ^ shift) ? string.Concat(char.ToUpper(baseChar)) : string.Concat(baseChar);
        }

        public static bool NoChildrenIntersect(UIObject obj, Rectangle rect)
        {
            bool flag = true;
            foreach (UIObject ob in obj.children)
            {
                if (ob.GetType() != typeof(UILabel))
                {
                    if (ob.rectangle.Intersects(rect))
                    {
                        flag = false;
                    }
                }
            }
            return flag;
        }

        public static bool LeftMouseClick(Rectangle r)
        {
            if (mouseRect.Intersects(r) && UIParameters.mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released && UIParameters.lastMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                return true;
            }
            return false;
        }

        public static void SwitchItems(ref Item item1, ref Item item2)
        {
            if ((item1.type == 0 || item1.stack < 1) && (item2.type != 0 || item2.stack > 0)) //if item2 is mouseitem, then if item slot is empty and item is picked up
            {
                item1 = item2;
                item2 = new Item();
                item2.type = 0;
                item2.stack = 0;
            }
            else if ((item1.type != 0 || item1.stack > 0) && (item2.type == 0 || item2.stack < 1)) //if item2 is mouseitem, then if item slot is empty and item is picked up
            {
                item2 = item1;
                item1 = new Item();
                item1.type = 0;
                item1.stack = 0;
            }
            else if ((item1.type != 0 || item1.stack > 0) && (item2.type != 0 || item2.stack > 0)) //if item2 is mouseitem, then if item slot is empty and item is picked up
            {
                Item item3 = item2;
                item2 = item1;
                item1 = item3;
            }
        }
    }
}
