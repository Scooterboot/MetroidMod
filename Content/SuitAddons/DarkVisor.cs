using MetroidMod.Common.GlobalNPCs;
using MetroidMod.Common.Players;
using System;
using MetroidMod.ID;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Effects;

namespace MetroidMod.Content.SuitAddons
{
	public class DarkVisor : ModSuitAddon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/DarkVisor/DarkVisorItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/DarkVisor/DarkVisorTile";

		public override string VisorSelectIcon => $"{Mod.Name}/Assets/Textures/SuitAddons/DarkVisor/DarkVisorIcon";

		public override SoundStyle? VisorBackgroundNoise => Sounds.Suit.Visors.DarkVisorBackgroundNoise;

		public override string VisorShaderFilterName => "MetroidMod:DarkVisor";

		public override bool AddOnlyAddonItem => false;

		public override void SetStaticDefaults()
		{
			AddonSlot = SuitAddonSlotID.Visor_Utility;
			ItemNameLiteral = true;
		}

		public override void SetItemDefaults(Item item)
		{
			item.width = 16;
			item.height = 16;
			item.value = Item.buyPrice(0, 2, 0, 0);
			item.rare = ItemRarityID.Green;
		}

		public override void DrawVisor(Player player)
		{

		}
	}
}
