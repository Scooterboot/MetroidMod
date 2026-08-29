using System;
using MetroidMod.Common.GlobalNPCs;
using MetroidMod.Common.Players;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.SuitAddons
{
	public class ScanVisor : ModVisorAddon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/ScanVisor/ScanVisorItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/ScanVisor/ScanVisorTile";

		public override string VisorSelectIcon => $"{Mod.Name}/Assets/Textures/SuitAddons/ScanVisor/ScanVisorIcon";

		public override SoundStyle? VisorBackgroundNoise => Sounds.Suit.Visors.ScanVisorBackgroundNoise;

		public override HelmetAddonSlot AddonSlot => HelmetAddonSlot.Scan;


		private Asset<Texture2D> scanTex;

		private Rectangle scanRect;

		private Asset<Texture2D> barTex;

		private Rectangle barRect;

		private Asset<Texture2D> barBorderTex;

		private Rectangle barBorderRect;

		public override void SetStaticDefaults()
		{
			ModContent.RequestIfExists($"{Mod.Name}/Assets/Textures/PrimeScan", out scanTex, AssetRequestMode.ImmediateLoad);
			ModContent.RequestIfExists($"{Mod.Name}/Assets/Textures/SpaceJumpBar", out barTex, AssetRequestMode.ImmediateLoad);
			ModContent.RequestIfExists($"{Mod.Name}/Assets/Textures/SpaceJumpBarBorder", out barBorderTex, AssetRequestMode.ImmediateLoad);
		}

		public override void ItemSetDefaults(Items.GeneratedModItem generatedModItem)
		{
            base.ItemSetDefaults(generatedModItem);

			generatedModItem.Item.width = 16;
			generatedModItem.Item.height = 16;
			generatedModItem.Item.value = Item.buyPrice(0, 2, 0, 0);
			generatedModItem.Item.rare = ItemRarityID.Green;
		}

		public override void ItemAddRecipes(Items.GeneratedModItem generatedModItem)
		{
			generatedModItem.CreateRecipe(1)
				.AddRecipeGroup(MetroidMod.EvilBarRecipeGroupID, 10)
				.AddIngredient(ItemID.SpelunkerPotion)
				.AddIngredient(ItemID.GlowingMushroom, 10)
				.AddTile(TileID.Anvils)
				.Register();
		}

		public override void DrawVisor(Player player)
		{
			if (scanTex == null || barTex == null || barBorderTex == null || !player.TryGetModPlayer(out MPlayer mp)) { return; }

			scanRect = new(
				Main.mouseX - (scanTex.Width() / 2),
				Main.mouseY - (scanTex.Height() / 2),
				scanTex.Width(),
				scanTex.Height()
			);
			barBorderRect = new(
				Main.mouseX - (barBorderTex.Width() / 2),
				Main.mouseY - (barBorderTex.Width() / 2) + scanRect.Height,
				barBorderTex.Width(),
				barBorderTex.Height()
			);
			barRect = new(
				Main.mouseX - (barTex.Width() / 2),
				Main.mouseY - (barTex.Width() / 2) + scanRect.Height,
				(int)(Math.Floor(barTex.Width() / 2 * mp.ScanProgress) * 2),
				barTex.Height()
			);
			Main.spriteBatch.Draw(scanTex.Value, scanRect, mp.HUDColor);

			Main.spriteBatch.Draw(barBorderTex.Value, barBorderRect, Color.White);
			Main.spriteBatch.Draw(barTex.Value, barRect, mp.HUDColor);
			if (ScanVisorGlobalNPC.sound != null && ScanVisorGlobalNPC.sound.IsPlaying && !ScanVisorGlobalNPC.soundShouldPlay)
			{
				ScanVisorGlobalNPC.sound.Sound.Stop(true);
				ScanVisorGlobalNPC.soundIsPlaying = false;
			}

			ScanVisorGlobalNPC.soundShouldPlay = false;

			// See Common/GlobalNPCs/ScanVisorGlobalNPC.cs for the functional part of the scan visor
		}
	}
}
