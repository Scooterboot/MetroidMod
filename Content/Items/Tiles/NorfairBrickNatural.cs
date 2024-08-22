﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Tiles
{
	public class NorfairBrickNatural : ModItem
	{
		public override string Texture => $"{nameof(MetroidMod)}/Content/Items/Tiles/NorfairBrick";
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Norfair Brick (Natural)");
			// Tooltip.SetDefault("'Welcome to hell.'");

			Item.ResearchUnlockCount = 100;
		}
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 9999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			ItemID.Sets.DrawUnsafeIndicator[Item.type] = true; //Hey so apparently they just have a thingy to make the unsafe skull show up.    -Z
			Item.createTile = ModContent.TileType<Content.Tiles.NorfairBrickNatural>();
		}
	}
}
