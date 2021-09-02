#region Using directives

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;

using MetroidMod.Common.Worlds;

#endregion

namespace MetroidMod.Items.tiles.Destroyable
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
			item.width = 16;
			item.height = 16;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.rare = 1;
		}

        // Netsyncing ?
        public override bool UseItem(Player player)
        {
            if (player.itemTime == 0 && player.itemAnimation > 0 && player.controlUseItem)
            {
				Vector2 pos = new Vector2(Player.tileTargetX * 16, Player.tileTargetY * 16);
                if (MWorld.mBlockType[Player.tileTargetX, Player.tileTargetY] == 0)
                {
                    MWorld.mBlockType[Player.tileTargetX, Player.tileTargetY] = placeType;
                    Main.PlaySound(SoundID.Dig, pos);
                    return true;
                }
            }
            return false;
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
		public override bool UseItem(Player player)
        {
			placeType = 10;
			return base.UseItem(player);
		}
	}
	public class BombBlock : FakeBlock
	{
        
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bomb Block");
            Tooltip.SetDefault("Deactivates a tile when hit by a bomb. \nUse the Chozite Cutter to break.");
		}
		public override bool UseItem(Player player)
        {
			placeType = 3;
			return base.UseItem(player);
		}
	}
	public class BombBlockChain : FakeBlock
	{
        
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bomb Block (Chain)");
            Tooltip.SetDefault("Deactivates a tile when hit by a bomb. \nTriggers adjacent BombBlocks. \nUse the Chozite Cutter to break.");
		}
		public override bool UseItem(Player player)
        {
			placeType = 12;
			return base.UseItem(player);
		}
	}
	public class CrumbleBlock : FakeBlock
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crumble Block (Instant)");
            Tooltip.SetDefault("Deactivates a tile when a player stands on it \nUse Chozite Cutters to break.");
		}
		public override bool UseItem(Player player)
        {
			placeType = 1;
			return base.UseItem(player);
		}
	}
	public class CrumbleBlockSpeed : FakeBlock
	{
        
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crumble Block (SpeedBoost)");
            Tooltip.SetDefault("Deactivates a tile shortly after a player stands on it \nUse the Chozite Cutter to break.");
		}
		public override bool UseItem(Player player)
        {
			placeType = 2;
			return base.UseItem(player);
		}
	}
	public class CrumbleBlockSlow : FakeBlock
	{
        
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crumble Block (Delayed)");
            Tooltip.SetDefault("Deactivates a tile shortly after a player stands on it \nUse the Chozite Cutter to break.");
		}
		public override bool UseItem(Player player)
        {
			placeType = 11;
			return base.UseItem(player);
		}
	}
	public class MissileBlock : FakeBlock
	{
        
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Missile Block");
            Tooltip.SetDefault("Deactivates a tile when hit by a missile. \nUse the Chozite Cutter to break");
		}
		public override bool UseItem(Player player)
        {
			placeType = 4;
			return base.UseItem(player);
		}
	}
	public class SuperMissileBlock : FakeBlock
	{
        
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Super Missile Block");
            Tooltip.SetDefault("Deactivates a tile when hit by a super missile. \nUse the Chozite Cutter to break.");
		}
		public override bool UseItem(Player player)
        {
			placeType = 8;
			return base.UseItem(player);
		}
	}
	public class PowerBombBlock : FakeBlock
	{
        
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Power Bomb Block");
            Tooltip.SetDefault("Deactivates a tile when hit by a power bomb. \nUse the Chozite Cutter to break.");
		}
		public override bool UseItem(Player player)
        {
			placeType = 7;
			return base.UseItem(player);
		}
	}
	public class BoostBlock : FakeBlock
	{
        
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Boost Block");
            Tooltip.SetDefault("Deactivates a tile when run into. \nUse the Chozite Cutter to break.");
		}
		public override bool UseItem(Player player)
        {
			placeType = 6;
			return base.UseItem(player);
		}
	}
	public class ScrewAttackBlock : FakeBlock
	{
        
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Screw Attack Block");
            Tooltip.SetDefault("Deactivates a tile when hit by a screw attack. \nUse the Chozite Cutter to break.");
		}
		public override bool UseItem(Player player)
        {
			placeType = 9;
			return base.UseItem(player);
		}
	}
}