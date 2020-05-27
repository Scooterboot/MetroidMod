using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.boss
{
	public class OmegaPirateSummon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phazon Reactor Core");
			Tooltip.SetDefault("Summons the Omega Pirate");
		}
		public override void SetDefaults()
		{
			item.maxStack = 20;
			item.consumable = true;
			item.width = 12;
			item.height = 12;
			item.useTime = 45;
			item.useAnimation = 45;
			item.useStyle = 4;
			item.noMelee = true;
			item.value = 1000;
			item.rare = 7;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.DemoniteBar, 5);
			recipe.AddIngredient(null, "PhazonBar", 1);
			recipe.AddIngredient(ItemID.LunarTabletFragment, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.CrimtaneBar, 5);
			recipe.AddIngredient(null, "PhazonBar", 1);
			recipe.AddIngredient(ItemID.LunarTabletFragment, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool CanUseItem(Player player)
		{
			return !NPC.AnyNPCs(mod.NPCType("OmegaPirate"));
		}
		public override bool UseItem(Player player)
		{
			NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("OmegaPirate"));
			Main.PlaySound(15,(int)player.position.X,(int)player.position.Y,0);
			return true;
		}
	}
}
