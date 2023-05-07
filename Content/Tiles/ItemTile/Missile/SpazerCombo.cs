﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles.ItemTile.Missile
{
	public class SpazerCombo : MissileAbst
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("Missile Array");
			AddMapEntry(new Color(207, 167, 73), name);
			ItemDrop = ModContent.ItemType<Items.MissileAddons.BeamCombos.SpazerComboAddon>();
			DustType = 1;
		}
	}
}
