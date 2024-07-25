using MetroidMod.Common.Systems;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Tiles
{
	public class ChozoStatueOrb : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 15;
		}

		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 134;
			Item.maxStack = 50;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));
			Item.rare = ItemRarityID.LightRed;
			Item.value = 50000;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Content.Tiles.ItemTile.ChozoStatueOrb>();
		}
		/*public override bool CanRightClick()
		{
			return true;
		}
		public override void RightClick(Player player)
		{
			player.QuickSpawnItem(player.GetSource_FromThis(), MSystem.OrbItem());
			//Item.NewItem(player.GetSource_FromThis(), player.position, (ushort)MSystem.OrbItem());
			//base.RightClick(player);
		}*/
	}
}
