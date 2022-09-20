using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;

using MetroidMod.Common.Systems;
using MetroidMod.Common.Players;
using MetroidMod.ID;

namespace MetroidMod.Content.SuitAddons
{
	public class XRayScope : ModSuitAddon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/XRayScope/XRayScopeItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/XRayScope/XRayScopeTile";

		public override string VisorSelectIcon => $"{Mod.Name}/Assets/Textures/SuitAddons/XRayScope/XRayScopeIcon";

		public override bool AddOnlyAddonItem => false;

		public override bool CanGenerateOnChozoStatue(int x, int y) => true;

		public override int GenerationChance(int x, int y) => WorldGen.drunkWorldGen ? 20 : 5;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("X-Ray Scope");
			Tooltip.SetDefault("Projects a wide ray of light if you are standing still");

			AddonSlot = SuitAddonSlotID.Visor_AltVision;
			ItemNameLiteral = true;
		}

		public override void SetItemDefaults(Item item)
		{
			item.value = Item.buyPrice(0, 40, 0, 0);
			item.rare = ItemRarityID.Pink;
			item.channel = true;
			item.mech = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddRecipeGroup(MetroidMod.T1HMBarRecipeGroupID, 10)
				.AddIngredient(ItemID.SpelunkerPotion)
				.AddIngredient(ItemID.GlowingMushroom, 30)
				.AddTile(TileID.Anvils)
				.Register();
		}

		bool isScope = false;

		public override bool ShowTileHover(Player player) => !isScope && base.ShowTileHover(player);

		public override bool AltFunctionUse(Player player) => true;

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				isScope = !isScope;
				return false;
			}
			if (isScope == false) // Placeable
			{
				Item.useStyle = ItemUseStyleID.Swing;
				Item.useAnimation = 15;
				Item.useTime = 15;
				Item.consumable = true;
				Item.createTile = TileType;
				Item.noUseGraphic = false;
			}
			else if (isScope == true) // Useable
			{
				Item.useStyle = ItemUseStyleID.Shoot;
				Item.useAnimation = 2;
				Item.useTime = 2;
				Item.consumable = false;
				Item.createTile = -1;
				Item.noUseGraphic = true;
			}
			return base.CanUseItem(player);
		}

		public override bool? UseItem(Player player)
		{
			if (isScope == false) { return base.UseItem(player); }
			DrawVisor(player);
			return true;
		}
		public override void DrawVisor(Player player)
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
			float targetrotation = (float)Math.Atan2((MY - player.Center.Y), (MX - player.Center.X));
			if (player.velocity.Y == 0 && player.velocity.X == 0)
			{
				for (int i = 0; i < 20; i++)
				{
					Vector2 lightPos = new Vector2(player.Center.X + (float)Math.Cos(targetrotation) * range * (2 + (2 * i)), player.position.Y + (float)Math.Sin(targetrotation) * range * (2 + (2 * i)) + 8);
					if (!player.dead && !mp.ballstate && player.velocity.Y == 0 && player.velocity.X == 0)
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
						if (Lighting.Mode == Terraria.Graphics.Light.LightMode.Trippy || Lighting.Mode == Terraria.Graphics.Light.LightMode.Color)
						{
							lightMult = 0.1f;
						}
						Lighting.AddLight((int)((float)lightPos.X / 16f), (int)((float)lightPos.Y / 16f), 0.75f + (0.25f * i) * lightMult, 0.75f + (0.25f * i) * lightMult, 0.75f + (0.25f * i) * lightMult);
						Vector2 tilePos = lightPos / 16f;
						MSystem.hit[(int)tilePos.X, (int)tilePos.Y] = true;
						MSystem.hit[(int)tilePos.X - 1, (int)tilePos.Y] = true;
						MSystem.hit[(int)tilePos.X + 1, (int)tilePos.Y] = true;
						MSystem.hit[(int)tilePos.X, (int)tilePos.Y - 1] = true;
						MSystem.hit[(int)tilePos.X, (int)tilePos.Y + 1] = true;
					}
				}
			}
		}
	}
}
