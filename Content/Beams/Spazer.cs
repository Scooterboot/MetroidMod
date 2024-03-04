namespace MetroidMod.Content.Beams
{
	/*
	public class Spazer : ModPrimaryABeam
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/Beams/Spazer/SpazerItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/Beams/Spazer/SpazerTile";

		public override string ProjectileTexture => $"{Mod.Name}/Assets/Textures/Beams/Spazer/SpazerShot";

		public override string ChargeLeadTexture => $"{Mod.Name}/Assets/Textures/ChargeLead/ChargeLead_Spazer";

		public override string ChargeProjectileTexture => $"{Mod.Name}/Assets/Textures/Beams/Spazer/SpazerChargeShot";

		public override string PowerBeamTexture => $"{Mod.Name}/Assets/Textures/PowerBeam/Spazer";

		public override bool AddOnlyBeamItem => false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spazer");
			ShotAmount = 3;
			AddonVersion = 1;
			AddonDamageMult = 0.25f;
			AddonHeat = 0.5f;
			AddonSpeed = 0.15f;
		}
		public override void SetItemDefaults(Item item)
		{
			item.value = Terraria.Item.buyPrice(0, 0, 25, 0);
			item.rare = ItemRarityID.LightRed;
		}
		public override bool CanGenerateOnChozoStatue(int x, int y) => true;
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<Items.Miscellaneous.ChoziteBar>(3)
				.AddIngredient(ItemID.Stinger, 12)
				.AddIngredient(ItemID.JungleSpores, 12)
				.AddIngredient(ItemID.Topaz, 1)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
	*/
}
