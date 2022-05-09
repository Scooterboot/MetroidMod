using Terraria;
using Terraria.ID;

namespace MetroidModPorted.Content.Beams
{
	public class IceBeam : ModSecondaryBeam
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/Beams/Ice/IceBeamItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/Beams/Ice/IceBeamTile";

		public override string ProjectileTexture => $"{Mod.Name}/Assets/Textures/Beams/Ice/IceBeamShot";

		public override string ChargeLeadTexture => $"{Mod.Name}/Assets/Textures/ChargeLead/ChargeLead_Ice";

		public override string ChargeProjectileTexture => $"{Mod.Name}/Assets/Textures/Beams/Ice/IceBeamChargeShot";

		public override string PowerBeamTexture => $"{Mod.Name}/Assets/Textures/PowerBeam/IceBeam";

		public override bool AddOnlyBeamItem => false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Beam");
			AddonVersion = 1;
			AddonDamageMult = 0.75f;
			AddonHeat = 0.25f;
			AddonSpeed = -0.3f;
		}
		public override void SetItemDefaults(Item item)
		{
			item.value = Terraria.Item.buyPrice(0, 0, 25, 0);
			item.rare = ItemRarityID.LightRed;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<Items.Miscellaneous.ChoziteBar>(3)
				.AddIngredient(ItemID.IceBlock, 25)
				.AddIngredient(ItemID.Bone, 10)
				.AddIngredient(ItemID.Sapphire, 1)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
