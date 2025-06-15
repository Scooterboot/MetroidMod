using MetroidMod.Content.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Weapons
{
	public class TorizoSpitter : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.SetWeaponValues(12, 6);
			Item.DamageType = DamageClass.Ranged;
			Item.width = 46;
			Item.height = 24;
			Item.useTime = 48;
			Item.useAnimation = 48;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.value = 15000;
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item108;
			Item.shoot = ModContent.ProjectileType<ChoziteChunk>();
			Item.shootSpeed = 5f;
			Item.useAmmo = AmmoID.Sand;
			Item.autoReuse = true;
		}
	}
}
