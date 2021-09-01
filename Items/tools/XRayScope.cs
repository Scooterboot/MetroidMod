using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;

using MetroidMod.Common.Worlds;

namespace MetroidMod.Items.tools
{
	public class XRayScope : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("X-Ray Scope");
			Tooltip.SetDefault("Projects a wide ray of light if you are standing still");
		}
		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 20;
			item.height = 20;
			item.noUseGraphic = true;
			item.useStyle = 5;
			item.useTime = 2;
			item.useAnimation = 2;
			item.autoReuse = true;
			item.value = 400000;
			item.rare = 5;
			item.channel = true;
			item.mech = true;
		}

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod); 
            recipe.AddIngredient(ItemID.CobaltBar, 10);
            recipe.AddIngredient(ItemID.SpelunkerPotion);
            recipe.AddIngredient(ItemID.GlowingMushroom, 30); 
            recipe.AddTile(TileID.Anvils);   
            recipe.SetResult(this);
            recipe.AddRecipe();

			recipe = new ModRecipe(mod); 
            recipe.AddIngredient(ItemID.PalladiumBar, 10);
            recipe.AddIngredient(ItemID.SpelunkerPotion);
            recipe.AddIngredient(ItemID.GlowingMushroom, 30); 
            recipe.AddTile(TileID.Anvils);   
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

		/*public static XRayScope xray = new XRayScope();
		public override void Initialize()
		{
			xray = this;
		}*/

		public override bool UseItem(Player player)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			//mp.xrayequipped = true;
			int range = 16;
			float MY = Main.mouseY + Main.screenPosition.Y;
			float MX = Main.mouseX + Main.screenPosition.X;
			if (player.gravDir == -1f)
			{
				MY = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
			}
			float targetrotation = (float)Math.Atan2((MY-player.Center.Y),(MX-player.Center.X));
			if (player.velocity.Y == 0 && player.velocity.X == 0)
			{
				for(int i = 0; i < 20; i++)
				{
					Vector2 lightPos = new Vector2(player.Center.X+(float)Math.Cos(targetrotation)*range*(2+(2*i)),player.position.Y+(float)Math.Sin(targetrotation)*range*(2+(2*i))+8);
					if(!player.dead && !mp.ballstate && player.velocity.Y == 0 && player.velocity.X == 0)
					{
						if ((Main.mouseX + Main.screenPosition.X) > player.position.X)
						{
							player.direction = 1;
						}
						if ((Main.mouseX + Main.screenPosition.X) < player.position.X)
						{
							player.direction = -1;
						}
						Lighting.AddLight((int)((float)lightPos.X/16f), (int)((float)lightPos.Y/16f), 0.75f+(0.25f*i), 0.75f+(0.25f*i), 0.75f+(0.25f*i));
                        Vector2 tilePos = lightPos / 16f;
                        MWorld.hit[(int)tilePos.X, (int)tilePos.Y] = true;
                        MWorld.hit[(int)tilePos.X - 1, (int)tilePos.Y] = true;
                        MWorld.hit[(int)tilePos.X + 1, (int)tilePos.Y] = true;
                        MWorld.hit[(int)tilePos.X, (int)tilePos.Y - 1] = true;
                        MWorld.hit[(int)tilePos.X, (int)tilePos.Y + 1] = true;
					}
				}
			}
			return true;
		}

	/*	public bool XRayActive(Player player)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			if(MBase.KeyPressed(MBase.XRayKey) && !mp.ballstate && player.velocity.Y == 0 && player.velocity.X == 0 && player.itemAnimation == 0)
			{
				return true;
			}
			return false;
		}*/
	}
}