using Terraria;
using Terraria.ID;

namespace MetroidMod.Content.ArmCannonAddons
{
	public class MissileExpansion : ModArmCannonAddon
	{
		public override string ItemTexture => $"{Mod.Name}/Content/Items/Tiles/MissileExpansion";

		public override string TileTexture => $"{Mod.Name}/Content/Tiles/ItemTile/MissileExpansionTile";

		public override void ItemSetStaticDefaults()
		{
			GeneratedModItem.Item.ResearchUnlockCount = 50;
		}
		public override void TileSetStaticDefaults()
		{
			base.TileSetStaticDefaults();

			TileID.Sets.FriendlyFairyCanLureTo[TileType] = true;
		}

		public override void ItemSetDefaults()
		{
			GeneratedModItem.Item.DefaultToPlaceableTile(TileType);

			GeneratedModItem.Item.width = 32;
			GeneratedModItem.Item.height = 32;
			GeneratedModItem.Item.maxStack = 50;
			GeneratedModItem.Item.value = Item.buyPrice(0, 10, 0, 0);
			GeneratedModItem.Item.rare = ItemRarityID.LightRed;
		}
	}
}
