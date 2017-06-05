using System;
using System.Collections.Generic;
using System.Text;

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
    public class UILabel : UIObject
    {
        public string text;
        public Color color;
        public Color borderColor;
        public delegate string GetText();
        public GetText Update;
        public SpriteFont font;
        public UILabel(Vector2 pos, SpriteFont font, Vector2 size, Color color, Color borderColour, GetText updateText, UIObject parent = null) : base(pos, size, parent)
        {
            this.color = color;
            this.borderColor = borderColour;
            this.font = font;
            this.Update = updateText;
        }
        public override void Draw(SpriteBatch sb)
        {
            Vector2 position = this.position;
            if (parent != null)
            {
                position += this.parent.position;
            }
            this.rectangle = new Rectangle((int)position.X, (int)position.Y, (int)this.size.X, (int)this.size.Y);
            this.text = this.Update();
            string text = this.WrapText(this.font, this.text, 430f);
            Utils.DrawBorderStringFourWay(sb, this.font, text, position.X, position.Y, this.color, this.borderColor, default(Vector2));
            base.Draw(sb);
        }
        //Credit to Alina B. On StackOverflow for this code. :)
        //http://stackoverflow.com/questions/15986473/how-do-i-implement-word-wrap
        public string WrapText(SpriteFont spriteFont, string text, float maxLineWidth)
        {
            string[] words = text.Split(' ');
            StringBuilder sb = new StringBuilder();
            float lineWidth = 0f;
            float spaceWidth = spriteFont.MeasureString(" ").X;
            foreach (string word in words)
            {
                Vector2 size = spriteFont.MeasureString(word);

                if (lineWidth + size.X < maxLineWidth)
                {
                    sb.Append(word + " ");
                    lineWidth += size.X + spaceWidth;
                }
                else
                {
                    sb.Append("\n" + word + " ");
                    lineWidth = size.X + spaceWidth;
                }
            }
            return sb.ToString();
        }
    }
}
