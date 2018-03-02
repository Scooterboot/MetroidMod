using System;
using System.Collections.Generic;

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
    public class UIButton : UIObject
    {
        public bool hover;
        public Texture2D texture;
        public Texture2D textureH;
        public Texture2D textureC;
        public Action Function;
        public UIButton(Vector2 pos, Vector2 size, Action function, UIObject parent = null, Texture2D texture = null, Texture2D textureH = null, Texture2D textureC = null) : base(pos, size, parent)
        {
            this.Function += function;
            this.texture = texture;
            this.textureH = textureH;
            this.textureC = textureC;
        }
		bool flag = false;
        public override void Draw(SpriteBatch sb)
        {
            Vector2 position = this.position;
            if (parent != null)
            {
                position += this.parent.position;
            }
            this.rectangle = new Rectangle((int)position.X, (int)position.Y, (int)this.size.X, (int)this.size.Y);
			Texture2D tex = this.texture;
            Color color = Color.White;
            if (new Rectangle(Main.mouseX, Main.mouseY, 1, 1).Intersects(this.rectangle))
            {
				Main.player[Main.myPlayer].mouseInterface = true;
                this.hover = true;
				if(this.texture != null && this.textureH != null)
				{
					tex = this.textureH;
				}
				else
				{
					color = Color.LightGray;
				}
				if(!flag)
				{
					if (UIParameters.mouseState.LeftButton == ButtonState.Pressed && UIParameters.mouseRect.Intersects(new Rectangle((int)position.X, (int)position.Y, (int)this.size.X, (int)this.size.Y)))
					{
						if(this.texture != null && this.textureC != null)
						{
							tex = this.textureC;
						}
						else
						{
							color = new Color(167, 167, 167, 255);
						}
					}
					if (UIParameters.LeftMouseClick(new Rectangle((int)position.X, (int)position.Y, (int)this.size.X, (int)this.size.Y)))
					{
						this.Function();
					}
				}
				else
				{
					if(UIParameters.mouseState.LeftButton != ButtonState.Pressed)
					{
						flag = false;
					}
				}
            }
			else
			{
				flag = (UIParameters.mouseState.LeftButton == ButtonState.Pressed);
			}
			
            if (tex == null)
                BaseTextureDrawing.DrawRectangleBox(sb, color, Color.Black, this.rectangle, 1);
            else
                sb.Draw(tex, this.rectangle, color);
            base.Draw(sb);
        }
    }
}
