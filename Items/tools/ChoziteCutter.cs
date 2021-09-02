#region Using directives

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;

using MetroidMod.Common.Worlds;

#endregion

namespace MetroidMod.Items.tools
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
			item.width = 16;
			item.height = 16;
			item.maxStack = 1;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.rare = 1;
		}
        
        // Netsyncing ?
        public override bool UseItem(Player player)
        {
            Tile tile = Main.tile[Player.tileTargetX, Player.tileTargetY];
            Vector2 pos = new Vector2(Player.tileTargetX * 16, Player.tileTargetY * 16);
            if (MWorld.mBlockType[Player.tileTargetX, Player.tileTargetY] != 0)
            {
                if (MWorld.mBlockType[Player.tileTargetX, Player.tileTargetY] == 1)
                {
                    Item.NewItem(pos, mod.ItemType("CrumbleBlock"));
                }
                if (MWorld.mBlockType[Player.tileTargetX, Player.tileTargetY] == 2)
                {
                    Item.NewItem(pos, mod.ItemType("CrumbleBlockSpeed"));
                }
                if (MWorld.mBlockType[Player.tileTargetX, Player.tileTargetY] == 3)
                {
                    Item.NewItem(pos, mod.ItemType("BombBlock"));
                }
                if (MWorld.mBlockType[Player.tileTargetX, Player.tileTargetY] == 4)
                {
                    Item.NewItem(pos, mod.ItemType("MissileBlock"));
                }
                if (MWorld.mBlockType[Player.tileTargetX, Player.tileTargetY] == 5)
                {
                    Item.NewItem(pos, mod.ItemType("FakeBlock"));
                }
                if (MWorld.mBlockType[Player.tileTargetX, Player.tileTargetY] == 6)
                {
                    Item.NewItem(pos, mod.ItemType("BoostBlock"));
                }
                if (MWorld.mBlockType[Player.tileTargetX, Player.tileTargetY] == 7)
                {
                    Item.NewItem(pos, mod.ItemType("PowerBombBlock"));
                }
                if (MWorld.mBlockType[Player.tileTargetX, Player.tileTargetY] == 8)
                {
                    Item.NewItem(pos, mod.ItemType("SuperMissileBlock"));
                }
                if (MWorld.mBlockType[Player.tileTargetX, Player.tileTargetY] == 9)
                {
                    Item.NewItem(pos, mod.ItemType("ScrewAttackBlock"));
                }
                if (MWorld.mBlockType[Player.tileTargetX, Player.tileTargetY] == 10)
                {
                    Item.NewItem(pos, mod.ItemType("FakeBlockHint"));
                }
                if (MWorld.mBlockType[Player.tileTargetX, Player.tileTargetY] == 11)
                {
                    Item.NewItem(pos, mod.ItemType("CrumbleBlockSlow"));
                }
                if (MWorld.mBlockType[Player.tileTargetX, Player.tileTargetY] == 12)
                {
                    Item.NewItem(pos, mod.ItemType("BombBlockChain"));
                }
                MWorld.mBlockType[Player.tileTargetX, Player.tileTargetY] = 0;
                MWorld.dontRegen[Player.tileTargetX, Player.tileTargetY] = false;
				MWorld.hit[Player.tileTargetX, Player.tileTargetY] = false;
                Main.PlaySound(0, Main.MouseWorld);
            }
            return base.UseItem(player);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.WireCutter);
            recipe.AddIngredient(null, "ChoziteBar", 5);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}