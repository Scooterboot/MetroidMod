using System;

using Microsoft.Xna.Framework.Graphics;

using ReLogic.Content;

using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;

using MetroidModPorted.Common.Systems;
using MetroidModPorted.Common.Players;
using MetroidModPorted.ID;

namespace MetroidModPorted.Content.SuitAddons
{
	public class ScanVisor : ModSuitAddon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/ScanVisor/ScanVisorItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/ScanVisor/ScanVisorTile";

		public override string VisorSelectIcon => $"{Mod.Name}/Assets/Textures/SuitAddons/ScanVisor/ScanVisorIcon";

		public override bool AddOnlyAddonItem => false;

		private Asset<Texture2D> scanTex;

		private Rectangle scanRect;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Scan Visor");
			Tooltip.SetDefault("Fills in the Bestiary entries of npcs you look at.");

			AddonSlot = SuitAddonSlotID.Visor_Scan;
			ItemNameLiteral = true;

			ModContent.RequestIfExists($"{Mod.Name}/Assets/Textures/PrimeScan", out scanTex, AssetRequestMode.ImmediateLoad);
		}

		public override void SetItemDefaults(Item item)
		{
			item.value = Item.buyPrice(0, 2, 0, 0);
			item.rare = ItemRarityID.Green;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddRecipeGroup(MetroidModPorted.EvilBarRecipeGroupID, 10)
				.AddIngredient(ItemID.SpelunkerPotion)
				.AddIngredient(ItemID.GlowingMushroom, 10)
				.AddTile(TileID.Anvils)
				.Register();
		}

		public override void DrawVisor(Player player)
		{
			if (scanTex == null) { return; }
			scanRect = new(
				Main.mouseX - (scanTex.Width() / 2),
				Main.mouseY - (scanTex.Height() / 2),
				scanTex.Width(),
				scanTex.Height()
			);
			Main.spriteBatch.Draw(scanTex.Value, scanRect, Color.CadetBlue);
		}
	}
}
