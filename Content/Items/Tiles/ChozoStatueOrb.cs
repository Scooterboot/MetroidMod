using MetroidMod.Common.Systems;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Tiles
{
	public abstract class ChozoStatueOrbBuilder : ModItem
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
		}
		public override bool CanRightClick()
		{
			return true;
		}
	}
	public class ChozoStatueOrb : ChozoStatueOrbBuilder
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.createTile = ModContent.TileType<Content.Tiles.ItemTile.ChozoStatueOrb>();
		}
		public override void RightClick(Player player)
		{
			player.QuickSpawnItem(player.GetSource_FromThis(), MSystem.OORB()); //T1
			//Item.NewItem(player.GetSource_FromThis(), player.position, (ushort)MSystem.OrbItem());
			base.RightClick(player);
		}
	}
	public class ChozoStatueOrb2 : ChozoStatueOrbBuilder
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.createTile = ModContent.TileType<Content.Tiles.ItemTile.ChozoStatueOrb2>();
		}
		public override void RightClick(Player player)
		{
			player.QuickSpawnItem(player.GetSource_FromThis(), MSystem.OORB());//T2
			//Item.NewItem(player.GetSource_FromThis(), player.position, (ushort)MSystem.OrbItem());
			//RightClick(player);
		}
	}
	public class ChozoStatueOrb3 : ChozoStatueOrbBuilder
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.height = 32;
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(1, 1));
			Item.createTile = ModContent.TileType<Content.Tiles.ItemTile.ChozoStatueOrb3>();
		}
		public override void RightClick(Player player)
		{
			player.QuickSpawnItem(player.GetSource_FromThis(), MSystem.OORB());//T3
			//Item.NewItem(player.GetSource_FromThis(), player.position, (ushort)MSystem.OrbItem());
			//base.RightClick(player);
		}
	}
}
