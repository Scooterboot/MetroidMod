#region Using directives

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;

using MetroidModPorted.Common.Systems;

#endregion

namespace MetroidModPorted.Content.Items.Tiles.Destroyable
{
	public class FakeBlock : ModItem
	{
        
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fake Block");
            Tooltip.SetDefault("Deactivates a tile when hit by anything. \nUse the Chozite Cutter to break.");
		}
		public ushort placeType = 5;
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.rare = ItemRarityID.Blue;
		}

		// Netsyncing ?
		public override void UseItemHitbox(Player player, ref Rectangle hitbox, ref bool noHitbox)
		{
            if (player.itemTime == 0 && player.itemAnimation > 0 && player.controlUseItem)
            {
				Vector2 pos = new Vector2(Player.tileTargetX * 16, Player.tileTargetY * 16);
                if (MSystem.mBlockType[Player.tileTargetX, Player.tileTargetY] == BreakableTileID.None)
                {
                    MSystem.mBlockType[Player.tileTargetX, Player.tileTargetY] = placeType;
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Dig, pos);
                }
            }
        }

        /*public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Bomb);
            recipe.AddIngredient(ItemID.SandBlock);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }*/
    }
	public class FakeBlockHint : FakeBlock
	{
        
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fake Block (Transparent)");
            Tooltip.SetDefault("Deactivates a tile when hit by anything. \nUse the Chozite Cutter to break.");
		}
		public override void UseItemHitbox(Player player, ref Rectangle hitbox, ref bool noHitbox)
        {
			placeType = 10;
			base.UseItemHitbox(player, ref hitbox, ref noHitbox);
		}
	}
	public class BombBlock : FakeBlock
	{
        
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bomb Block");
            Tooltip.SetDefault("Deactivates a tile when hit by a bomb. \nUse the Chozite Cutter to break.");
		}
		public override void UseItemHitbox(Player player, ref Rectangle hitbox, ref bool noHitbox)
        {
			placeType = 3;
			base.UseItemHitbox(player, ref hitbox, ref noHitbox);
		}
	}
	public class BombBlockChain : FakeBlock
	{
        
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bomb Block (Chain)");
            Tooltip.SetDefault("Deactivates a tile when hit by a bomb. \nTriggers adjacent BombBlocks. \nUse the Chozite Cutter to break.");
		}
		public override void UseItemHitbox(Player player, ref Rectangle hitbox, ref bool noHitbox)
        {
			placeType = 12;
			base.UseItemHitbox(player, ref hitbox, ref noHitbox);
		}
	}
	public class CrumbleBlock : FakeBlock
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crumble Block (Instant)");
            Tooltip.SetDefault("Deactivates a tile when a player stands on it \nUse Chozite Cutters to break.");
		}
		public override void UseItemHitbox(Player player, ref Rectangle hitbox, ref bool noHitbox)
        {
			placeType = 1;
			base.UseItemHitbox(player, ref hitbox, ref noHitbox);
		}
	}
	public class CrumbleBlockSpeed : FakeBlock
	{
        
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crumble Block (SpeedBoost)");
            Tooltip.SetDefault("Deactivates a tile shortly after a player stands on it \nUse the Chozite Cutter to break.");
		}
		public override void UseItemHitbox(Player player, ref Rectangle hitbox, ref bool noHitbox)
        {
			placeType = 2;
			base.UseItemHitbox(player, ref hitbox, ref noHitbox);
		}
	}
	public class CrumbleBlockSlow : FakeBlock
	{
        
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crumble Block (Delayed)");
            Tooltip.SetDefault("Deactivates a tile shortly after a player stands on it \nUse the Chozite Cutter to break.");
		}
		public override void UseItemHitbox(Player player, ref Rectangle hitbox, ref bool noHitbox)
        {
			placeType = 11;
			base.UseItemHitbox(player, ref hitbox, ref noHitbox);
		}
	}
	public class MissileBlock : FakeBlock
	{
        
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Missile Block");
            Tooltip.SetDefault("Deactivates a tile when hit by a missile. \nUse the Chozite Cutter to break");
		}
		public override void UseItemHitbox(Player player, ref Rectangle hitbox, ref bool noHitbox)
        {
			placeType = 4;
			base.UseItemHitbox(player, ref hitbox, ref noHitbox);
		}
	}
	public class SuperMissileBlock : FakeBlock
	{
        
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Super Missile Block");
            Tooltip.SetDefault("Deactivates a tile when hit by a super missile. \nUse the Chozite Cutter to break.");
		}
		public override void UseItemHitbox(Player player, ref Rectangle hitbox, ref bool noHitbox)
        {
			placeType = 8;
			base.UseItemHitbox(player, ref hitbox, ref noHitbox);
		}
	}
	public class PowerBombBlock : FakeBlock
	{
        
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Power Bomb Block");
            Tooltip.SetDefault("Deactivates a tile when hit by a power bomb. \nUse the Chozite Cutter to break.");
		}
		public override void UseItemHitbox(Player player, ref Rectangle hitbox, ref bool noHitbox)
        {
			placeType = 7;
			base.UseItemHitbox(player, ref hitbox, ref noHitbox);
		}
	}
	public class BoostBlock : FakeBlock
	{
        
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Boost Block");
            Tooltip.SetDefault("Deactivates a tile when run into. \nUse the Chozite Cutter to break.");
		}
		public override void UseItemHitbox(Player player, ref Rectangle hitbox, ref bool noHitbox)
        {
			placeType = 6;
			base.UseItemHitbox(player, ref hitbox, ref noHitbox);
		}
	}
	public class ScrewAttackBlock : FakeBlock
	{
        
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Screw Attack Block");
            Tooltip.SetDefault("Deactivates a tile when hit by a screw attack. \nUse the Chozite Cutter to break.");
		}
		public override void UseItemHitbox(Player player, ref Rectangle hitbox, ref bool noHitbox)
        {
			placeType = 9;
			base.UseItemHitbox(player, ref hitbox, ref noHitbox);
		}
	}
}
