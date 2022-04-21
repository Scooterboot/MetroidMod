namespace MetroidModPorted.Content.Beams
{
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
			//Tile = new Content.Tiles.ItemTile.Beam.SpazerTile();
			DisplayName.SetDefault("Spazer");
			ShotAmount = 3;
			AddonVersion = 1;
			AddonDamageMult = 0.25f;
			AddonHeat = 0.5f;
			AddonSpeed = 0.15f;
		}
	}
}
