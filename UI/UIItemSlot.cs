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
using Terraria.UI;

namespace MetroidMod
{
    public class UIItemSlot : UIObject
    {
        //variables
        public Item item = new Item();
        public Condition conditions;
        public DrawInItemSlot drawBack;
        public DrawInItemSlot drawItem;
        public DrawInItemSlot postDrawItem;
        public bool drawAsNormalItemSlot = true;
        public int contextForItemSlot;

        //Delegates, voids
        public delegate void DrawInItemSlot(SpriteBatch sb, UIItemSlot obj);
        public delegate bool Condition(Item item);

        public UIItemSlot(Vector2 pos, UIObject parent = null, Condition con = null, DrawInItemSlot db = null, DrawInItemSlot di = null, DrawInItemSlot pdi = null, bool drawAsNormalItemSlot = false, int contextForItemSlot = 1)
            : base(pos, new Vector2(52), parent)
        {
            this.item = new Item();

            this.conditions = con;

            this.size = new Vector2(52); 
            //If you want to change the size of the item slot you can change it here or by changing it in your UIWindow file :)
            //The size is 52 by 52 as that is the size of InventoryBackTexture, you can change it to your own however.

            this.drawBack = db; 
            //This is called when the background is to be drawn. Leave as null to use default draw.

            this.drawItem = di;
            //This is called when the item is drawn. It might be good to leave this null, or copy and paste the default code to change the origin.

            this.postDrawItem = pdi;
            //This is called after the item is drawn. Used to draw other things, such as the item tooltip.
            //To draw the item tooltip, use UIParameters.mousePos + Vector2(12) as your starting position for the text :)

            this.drawAsNormalItemSlot = drawAsNormalItemSlot;

            this.contextForItemSlot = contextForItemSlot;
        }
        public void Handle()
        {
            if (Main.mouseLeftRelease && Main.mouseLeft)
            {
                ItemSlot.LeftClick(ref this.item, 0);
                Recipe.FindRecipes();
            }
            else if (Main.mouseRightRelease && Main.mouseRight)
            {
                ItemSlot.RightClick(ref this.item, 0);
            }
        }
        public override void Draw(SpriteBatch sb)
        {
            Vector2 position = this.position;
            if (parent != null)
            {
                position += this.parent.position;
            }
            this.rectangle = new Rectangle((int)position.X, (int)position.Y, (int)this.size.X, (int)this.size.Y);
            if (new Rectangle(Main.mouseX, Main.mouseY, 1, 1).Intersects(this.rectangle))
            {
				Main.player[Main.myPlayer].mouseInterface = true;
                if (this.conditions != null)
                {
                    if (this.conditions(Main.mouseItem))
                    {
                        Handle();
                    }
                }
                else
                {
                    Handle();
                }
            }
            /*if (drawAsNormalItemSlot)
            {
                ItemSlot.Draw(sb, ref this.item, contextForItemSlot, this.position);
            }
            else
            {
                if (drawBack != null)
                {
                    drawBack(sb, this);
                }
                else
                {
                    sb.Draw(Main.inventoryBackTexture, this.rectangle, Color.White); //Draws as default texture.
                }
            }
            if (this.item.type > 0)
            {
                if (drawItem != null)
                {
                    drawItem(sb, this);
                }
                else
                {
                    Vector2 origin = new Vector2(Main.itemTexture[this.item.type].Width / 2, Main.itemTexture[this.item.type].Height / 2);
                    sb.Draw(Main.itemTexture[this.item.type], new Vector2(this.rectangle.X + this.rectangle.Width / 2, this.rectangle.Y + this.rectangle.Height / 2), null, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
                }
            }
            if (postDrawItem != null)
            {
                postDrawItem(sb, this);
            }*/
			float num = Main.inventoryScale;
			Main.inventoryScale = 1f;
			ItemSlot.Draw(sb, ref this.item, contextForItemSlot, new Vector2(this.rectangle.X, this.rectangle.Y));
			Main.inventoryScale = num;
            base.Draw(sb);
        }
		public void DrawItemText()
		{
			if (!item.IsAir && new Rectangle(Main.mouseX, Main.mouseY, 1, 1).Intersects(this.rectangle) && Main.mouseItem.IsAir)
			{
				Main.HoverItem = item;
				string text = item.AffixName();
				if (item.stack > 1)
				{
					object obj = text;
					text = string.Concat(new object[]
					{
						obj,
						" (",
						item.stack,
						")"
					});
				}
				int rare = item.rare;
				if (item.expert)
				{
					rare = -12;
				}
				Main.instance.MouseText(text, rare, 0);
			}
		}
    }
}
