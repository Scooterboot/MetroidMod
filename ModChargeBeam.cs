namespace MetroidMod
{
	/*
	public abstract class ModChargeBeam : ModBeam
	{
		public bool IsTraditionalCharge { get; set; } = true;

		public override sealed void Load()
		{
			base.Load();
			AddonSlot = BeamAddonSlotID.Charge;
		}

		internal override sealed void InternalStaticDefaults()
		{
			AddonSlot = BeamAddonSlotID.Charge;
		}

		public override bool OnShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			int ch = Projectile.NewProjectile(new EntitySource_ByProjectileSourceId(Type), position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<ChargeLead>(), damage, knockback, player.whoAmI);
			ChargeLead cl = (ChargeLead)Main.projectile[ch].ModProjectile;
			cl.ChargeUpSound = chargeUpSound;
			cl.ChargeTex = chargeTex;
			cl.ChargeShotAmt = chargeShotAmt;
			cl.DustType = dustType;
			cl.DustColor = dustColor;
			cl.LightColor = lightColor;
			cl.canPsuedoScrew = mp.psuedoScrewActive;
			cl.ShotSound = shotSound;
			cl.ChargeShotSound = chargeShotSound;
			cl.projectile.netUpdate = true;

			chargeLead = ch;

			return false;
		}
	}
	*/
}
