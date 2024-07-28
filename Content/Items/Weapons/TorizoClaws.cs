using MetroidMod.Content.NPCs.Torizo;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Weapons
{
	public class TorizoClaws : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.staff[Type] = true;
			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.SetWeaponValues(44, 12);
			Item.DamageType = DamageClass.Magic;//Item.ranged = true;
			Item.DefaultToStaff(ModContent.ProjectileType<Projectiles.OrbBomb>(), 5, 50, 12);
			Item.width = 50;
			Item.height = 50;
			//Item.useTime = 18;
			//Item.useAnimation = 12;
			/*Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.value = 15000;
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item43;
			Item.shoot = ProjectileID.Shuriken;
			Item.shootSpeed = 6.7f;
			Item.useAmmo = AmmoID.Arrow;*/
			Item.autoReuse = true;
		}

	}
}
