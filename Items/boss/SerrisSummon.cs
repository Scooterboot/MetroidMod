using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.boss
{
	public class SerrisSummon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Serris Bait");
			Tooltip.SetDefault("Summons Serris at the ocean");
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
			recipe.AddIngredient(ItemID.PurificationPowder, 20);
			recipe.AddIngredient(ItemID.Bone, 3);
			recipe.AddIngredient(ItemID.Worm, 1);
			recipe.AddTile(TileID.DemonAltar);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool CanUseItem(Player player)
		{
			return !NPC.AnyNPCs(mod.NPCType("Serris_Head")) && player.ZoneBeach;
		}
		public override bool UseItem(Player player)
		{
			NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("Serris_Head"));
			Main.PlaySound(SoundLoader.customSoundType, (int)player.Center.X, (int)player.Center.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/SerrisSummon"));
			return true;
		}
	}
}