using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.Localization;

namespace MetroidMod.Content.Hatches
{
	public abstract class HatchItem : ModItem
	{
		public abstract ModHatch Hatch { get; }

		public override string Name => Hatch.Name;
		public override string Texture => base.Texture + "Item";
		public override LocalizedText DisplayName => Hatch.DisplayName;
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 40;
			Item.height = 36;
			Item.maxStack = 9999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.mech = true;
			Item.createTile = Hatch.GetTileType();
		}

		public override bool AltFunctionUse(Player player) => true;

		public override bool CanUseItem(Player player)
		{
			bool vertical = player.altFunctionUse == 2;
			Item.createTile = Hatch.GetTileType(vertical: vertical);
			return base.CanUseItem(player);
		}
	}
}
