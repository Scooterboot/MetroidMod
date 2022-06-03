using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;

using MetroidModPorted.Common.Systems;
using MetroidModPorted.Common.Players;

namespace MetroidModPorted.Content.Items.Tools
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
			Item.maxStack = 1;
			Item.width = 20;
			Item.height = 20;
			Item.noUseGraphic = true;
			Item.useStyle = 5;
			Item.useTime = 2;
			Item.useAnimation = 2;
			Item.autoReuse = true;
			Item.value = 400000;
			Item.rare = 5;
			Item.channel = true;
			Item.mech = true;
		}

		public override void AddRecipes()
		{
			/*ModRecipe recipe = new ModRecipe(mod); 
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
			recipe.AddRecipe();*/
		}

		/*public static XRayScope xray = new XRayScope();
		public override void Initialize()
		{
			xray = this;
		}*/

		public override bool? UseItem(Player player)
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
						float lightMult = 1f;
						if(Lighting.Mode == Terraria.Graphics.Light.LightMode.Trippy || Lighting.Mode == Terraria.Graphics.Light.LightMode.Color)
						{
							lightMult = 0.1f;
						}
						Lighting.AddLight((int)((float)lightPos.X/16f), (int)((float)lightPos.Y/16f), 0.75f+(0.25f*i)*lightMult, 0.75f+(0.25f*i)*lightMult, 0.75f+(0.25f*i)*lightMult);
						Vector2 tilePos = lightPos / 16f;
						MSystem.hit[(int)tilePos.X, (int)tilePos.Y] = true;
						MSystem.hit[(int)tilePos.X - 1, (int)tilePos.Y] = true;
						MSystem.hit[(int)tilePos.X + 1, (int)tilePos.Y] = true;
						MSystem.hit[(int)tilePos.X, (int)tilePos.Y - 1] = true;
						MSystem.hit[(int)tilePos.X, (int)tilePos.Y + 1] = true;
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
