#region Using directives

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;

using MetroidModPorted.Common.Systems;

#endregion

namespace MetroidModPorted.Content.Items.Tools
{
	public class ChoziteCutter : ModItem
	{
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chozite Cutter");
			Tooltip.SetDefault("Removes weapon-destructable blocks. \nDoes not break wires.");
		}
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 1;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = 1;
			Item.rare = 1;
		}

		// Netsyncing ?
		public override bool? UseItem(Player Player)
		{
			Tile tile = Main.tile[Player.tileTargetX, Player.tileTargetY];
			Vector2 pos = new Vector2(Player.tileTargetX * 16, Player.tileTargetY * 16);
			if (MSystem.mBlockType[Player.tileTargetX, Player.tileTargetY] != 0)
			{
				if (MSystem.mBlockType[Player.tileTargetX, Player.tileTargetY] == BreakableTileID.CrumbleInstant)
				{
					Item.NewItem(new EntitySource_Parent(Player), pos, ModContent.ItemType<Tiles.Destroyable.CrumbleBlock>());
				}
				if (MSystem.mBlockType[Player.tileTargetX, Player.tileTargetY] == BreakableTileID.CrumbleSpeed)
				{
					Item.NewItem(new EntitySource_Parent(Player), pos, ModContent.ItemType<Tiles.Destroyable.CrumbleBlockSpeed>());
				}
				if (MSystem.mBlockType[Player.tileTargetX, Player.tileTargetY] == BreakableTileID.Bomb)
				{
					Item.NewItem(new EntitySource_Parent(Player), pos, ModContent.ItemType<Tiles.Destroyable.BombBlock>());
				}
				if (MSystem.mBlockType[Player.tileTargetX, Player.tileTargetY] == BreakableTileID.Missile)
				{
					Item.NewItem(new EntitySource_Parent(Player), pos, ModContent.ItemType<Tiles.Destroyable.MissileBlock>());
				}
				if (MSystem.mBlockType[Player.tileTargetX, Player.tileTargetY] == BreakableTileID.Fake)
				{
					Item.NewItem(new EntitySource_Parent(Player), pos, ModContent.ItemType<Tiles.Destroyable.FakeBlock>());
				}
				if (MSystem.mBlockType[Player.tileTargetX, Player.tileTargetY] == BreakableTileID.Boost)
				{
					Item.NewItem(new EntitySource_Parent(Player), pos, ModContent.ItemType<Tiles.Destroyable.BoostBlock>());
				}
				if (MSystem.mBlockType[Player.tileTargetX, Player.tileTargetY] == BreakableTileID.PowerBomb)
				{
					Item.NewItem(new EntitySource_Parent(Player), pos, ModContent.ItemType<Tiles.Destroyable.PowerBombBlock>());
				}
				if (MSystem.mBlockType[Player.tileTargetX, Player.tileTargetY] == BreakableTileID.SuperMissile)
				{
					Item.NewItem(new EntitySource_Parent(Player), pos, ModContent.ItemType<Tiles.Destroyable.SuperMissileBlock>());
				}
				if (MSystem.mBlockType[Player.tileTargetX, Player.tileTargetY] == BreakableTileID.ScrewAttack)
				{
					Item.NewItem(new EntitySource_Parent(Player), pos, ModContent.ItemType<Tiles.Destroyable.ScrewAttackBlock>());
				}
				if (MSystem.mBlockType[Player.tileTargetX, Player.tileTargetY] == BreakableTileID.FakeHint)
				{
					Item.NewItem(new EntitySource_Parent(Player), pos, ModContent.ItemType<Tiles.Destroyable.FakeBlockHint>());
				}
				if (MSystem.mBlockType[Player.tileTargetX, Player.tileTargetY] == BreakableTileID.CrumbleSlow)
				{
					Item.NewItem(new EntitySource_Parent(Player), pos, ModContent.ItemType<Tiles.Destroyable.CrumbleBlockSlow>());
				}
				if (MSystem.mBlockType[Player.tileTargetX, Player.tileTargetY] == BreakableTileID.BombChain)
				{
					Item.NewItem(new EntitySource_Parent(Player), pos, ModContent.ItemType<Tiles.Destroyable.BombBlockChain>());
				}
				MSystem.mBlockType[Player.tileTargetX, Player.tileTargetY] = 0;
				MSystem.dontRegen[Player.tileTargetX, Player.tileTargetY] = false;
				MSystem.hit[Player.tileTargetX, Player.tileTargetY] = false;
				Terraria.Audio.SoundEngine.PlaySound(0, Main.MouseWorld);
			}
			return base.UseItem(Player);
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.WireCutter)
				.AddIngredient<Miscellaneous.ChoziteBar>(5)
				.AddTile(TileID.Anvils)
				.Register();
			/*ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.WireCutter);
			recipe.AddIngredient(null, "ChoziteBar", 5);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();*/
		}
	}
}
