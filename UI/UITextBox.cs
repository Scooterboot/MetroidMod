using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace MetroidMod
{
    public class UITextBox : UIObject
    {
        public string text;
        public bool selected;
        public SpriteFont font;
        public UITextBox(Vector2 pos, Vector2 size, SpriteFont font, UIObject parent = null) : base(pos, size, parent)
        {
            this.font = font;
            this.text = "";
        }
        public string NeedsEnd()
        {
            if (!this.selected)
            {
                return "";
            }
            if (DateTime.Now.Millisecond < 500)
            {
                return "|";
            }
            return "";
        }
        public override void Draw(SpriteBatch sb)
        {
            Vector2 position = this.position;
            if (parent != null)
            {
                position += this.parent.position;
            }
            this.rectangle = new Rectangle((int)position.X, (int)position.Y, (int)this.size.X, (int)this.size.Y);
            if (UIParameters.LeftMouseClick(this.rectangle))
            {
                if (this.selected)
                {
                    this.selected = false;
                    Main.blockInput = false;
                }
                else
                {
                    this.selected = true;
                    Main.blockInput = true;
                }
            }
            if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
            {
                this.selected = false;
                Main.blockInput = false;
            }
            if (this.selected)
            {
                Main.blockInput = true;
                Microsoft.Xna.Framework.Input.Keys[] oldPressed = UIParameters.oldState.GetPressedKeys();
                Microsoft.Xna.Framework.Input.Keys[] newPressed = UIParameters.newState.GetPressedKeys();
                bool shift = false;
                bool capsLock = false;
                bool numLock = false;
                if (this.text.Length > 0 && UIParameters.oldState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Back) && UIParameters.newState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Back))
                {
                    if (UIParameters.backSpace == 0)
                    {
                        this.text = this.text.Remove(this.text.Length - 1);
                        UIParameters.backSpace = 9;
                    }
                    UIParameters.backSpace--;
                    goto End;
                }
                else
                {
                    UIParameters.backSpace = 0;
                }
                if ((UIParameters.oldState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift) && UIParameters.newState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift)) || (UIParameters.oldState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightShift) && UIParameters.newState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightShift)))
                {
                    shift = true;
                }
                if (Control.IsKeyLocked(System.Windows.Forms.Keys.CapsLock))
                {
                    capsLock = true;
                }
                if (Control.IsKeyLocked(System.Windows.Forms.Keys.NumLock))
                {
                    numLock = true;
                }
                for (int i = 0; i < newPressed.Length; i++)
                {
                    if (newPressed[i] == Microsoft.Xna.Framework.Input.Keys.Back)
                    {
                        goto Skip;
                    }
                    bool flag = true;
                    for (int k = 0; k < oldPressed.Length; k++)
                    {
                        if (newPressed[i] == oldPressed[k])
                        {
                            flag = false;
                        }
                    }
                    if (this.font.MeasureString(this.text).X >= this.size.X - 12)
                    {
                        flag = false;
                    }
                    if (flag)
                    {
                        this.text += UIParameters.TranslateChar(newPressed[i], shift, capsLock, numLock);
                    }
                    Skip:;
                }
                End:;
            }
            BaseTextureDrawing.DrawRectangleBox(sb, Color.DarkGray, Color.White, this.rectangle, 2);
            sb.DrawString(this.font, this.text + NeedsEnd(), position + new Vector2(2), Color.Black);
            base.Draw(sb);
        }
    }
}
