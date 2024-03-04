using Terraria;
using Terraria.ID;

namespace MetroidMod.Content.Beams
{
	/*
	public class WaveBeam : ModUtilityBeam
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/Beams/Wave/WaveBeamItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/Beams/Wave/WaveBeamTile";

		public override string ProjectileTexture => $"{Mod.Name}/Assets/Textures/Beams/Wave/WaveBeamShot";

		public override string ChargeLeadTexture => $"{Mod.Name}/Assets/Textures/ChargeLead/ChargeLead_Wave";

		public override string ChargeProjectileTexture => $"{Mod.Name}/Assets/Textures/Beams/Wave/WaveBeamChargeShot";

		public override string PowerBeamTexture => $"{Mod.Name}/Assets/Textures/PowerBeam/WaveBeam";

		public override bool AddOnlyBeamItem => false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wave Beam");
			ShotAmount = 2;
			AddonVersion = 1;
			AddonDamageMult = 0.5f;
			AddonHeat = 0.5f;
			AddonSpeed = 0;
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
				.AddRecipeGroup(MetroidMod.EvilBarRecipeGroupID, 8)
				.AddIngredient(ItemID.Amethyst, 10)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
	*/
}
