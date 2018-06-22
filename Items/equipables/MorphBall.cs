using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace MetroidMod.Items.equipables
{
	public class MorphBall : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Morph Ball");
			Tooltip.SetDefault("Press the morph ball key to roll into a ball\n" + 
			"Morph Ball's colors are based on your shirt and undershirt colors");
		}
		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 28;
			item.maxStack = 1;
			item.value = 40000;
			item.rare = 2;
			//item.accessory = true;
			item.mountType = mod.MountType("MorphBallMount");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ChoziteBar", 20);
			recipe.AddIngredient(ItemID.Diamond, 3);
			recipe.AddIngredient(ItemID.GoldBar, 5);
			recipe.AddIngredient(ItemID.FallenStar, 3);
            recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ChoziteBar", 20);
			recipe.AddIngredient(ItemID.Diamond, 3);
			recipe.AddIngredient(ItemID.PlatinumBar, 5);
			recipe.AddIngredient(ItemID.FallenStar, 3);
            recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public BallUI ballUI;
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if(ballUI == null)
			{
				ballUI = new BallUI();
			}
			
			if(!Main.playerInventory || Main.EquipPage != 2)
			{
				ballUI.BallUIOpen = false;
			}
		}
		public override bool CanUseItem(Player player)
		{
			return (item == player.miscEquips[3]);
		}

		public override ModItem Clone(Item item)
		{
			ModItem clone = this.NewInstance(item);
			MorphBall ballClone = (MorphBall)clone;
			if(ballUI != null)
			{
				ballClone.ballUI = ballUI;
			}
			else
			{
				ballClone.ballUI = new BallUI();
			}
			return clone;
		}

		public override Color? GetAlpha(Color lightColor)
        {
			Player player = Main.player[Main.myPlayer];
			MPlayer mp = player.GetModPlayer<MPlayer>(mod);
            return mp.morphItemColor;
        }
		public override void PostDrawInInventory(SpriteBatch sb, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale )
		{
			Player player = Main.player[Main.myPlayer];
			MPlayer mp = player.GetModPlayer<MPlayer>(mod);
			itemColor = mp.morphItemColor;
			drawColor = mp.morphColorLights;
			Texture2D tex = mod.GetTexture("Items/equipables/MorphBall_Lights");
			sb.Draw(tex, position, frame, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
			/*int e = 3;
			for(int i = 3; i < 8 + player.extraAccessorySlots; i++)
			{
				if(ballUI != null)
				{
					if(player.armor[i] == item)
					{
						ballUI.Draw(sb);
						break;
					}
					else
					{
						e++;
						if(e >= 8 + player.extraAccessorySlots)
						{
							ballUI.BallUIOpen = false;
						}
					}
				}
			}*/
			if(ballUI != null)
			{
				if(player.miscEquips[3] == item && Main.EquipPage == 2)
				{
					ballUI.Draw(sb);
				}
				else
				{
					ballUI.BallUIOpen = false;
				}
			}
		}
		public static BallUI TempBallUI;
		public override void PreReforge()
		{
			if(ballUI != null)
			{
				TempBallUI = ballUI;
			}
		}
		public override void PostReforge()
		{
			ballUI = TempBallUI;
		}
		public override TagCompound Save()
		{
			if(ballUI != null)
			{
				MGlobalItem mi = item.GetGlobalItem<MGlobalItem>(mod);
				return new TagCompound
				{
					{"ballItem0", ItemIO.Save(ballUI.ballSlot[0].item)},
					{"ballItem1", ItemIO.Save(ballUI.ballSlot[1].item)},
					{"ballItem2", ItemIO.Save(ballUI.ballSlot[2].item)},
					{"ballItem3", ItemIO.Save(ballUI.ballSlot[3].item)},
					{"ballItem4", ItemIO.Save(ballUI.ballSlot[4].item)}
				};
			}
			return null;
		}
		public override void Load(TagCompound tag)
		{
			try
			{
				if(ballUI == null)
				{
					ballUI = new BallUI();
				}
				for(int i = 0; i < ballUI.ballSlot.Length ; i++)
				{
					Item item = tag.Get<Item>("ballItem"+i);
					ballUI.ballSlot[i].item = item;
				}
			}
			catch{}
		}
		public override void PostDrawInWorld(SpriteBatch sb, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI )
		{
			DrawLights(sb, Main.player[Main.myPlayer]);
			MPlayer mp = Main.player[Main.myPlayer].GetModPlayer<MPlayer>(mod);
			lightColor = mp.morphColorLights;
			alphaColor = lightColor;
		}
		public void DrawLights(SpriteBatch sb, Player player)
		{
			Texture2D tex = mod.GetTexture("Items/equipables/MorphBall_Lights");
			float rotation = item.velocity.X * 0.2f;
			float num3 = 1f;
			float num4 = (float)(item.height - tex.Height);
			float num5 = (float)(item.width / 2 - tex.Width / 2);
			MPlayer mp = player.GetModPlayer<MPlayer>(mod);
			sb.Draw(tex, new Vector2(item.position.X - Main.screenPosition.X + (float)(tex.Width / 2) + num5, item.position.Y - Main.screenPosition.Y + (float)(tex.Height / 2) + num4 + 2f), new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), mp.morphColorLights, rotation, new Vector2((float)(tex.Width / 2), (float)(tex.Height / 2)), num3, SpriteEffects.None, 0f);
		}
	}
}
