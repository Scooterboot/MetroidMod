using MetroidMod.Content.Projectiles;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace MetroidMod.Content.MissileAddons
{
	internal class HomingMissile : ModMissileAddon
	{
		public override bool AddOnlyAddonItem => false;
		public override float DamageMult => 2f;
		public override Color PrimaryColor => MetroidMod.powColor;
		public override Color SecondaryColor => MetroidMod.powSecondaryColor;
		public override int ShotDust => DustID.YellowTorch;

		public override bool IgnoreProjectile => true;
		public override void SetStaticDefaults()
		{
			AddonSlot = MissileAddonSlotID.Charge;

			//All the stats are set outside of here up in Stat Values, lets me do fancy schmancy tooltip stuff
			base.SetStaticDefaults();
		}
		public override void AI(MProjectile mpshot)
		{
			Projectile Projectile = mpshot.Projectile;
			mpshot.HomingBehavior(Projectile);
		}
		public override void SetItemDefaults(Item item) //TO DO SOMETHING WITH THIS
		{
			item.value = 30000;
			item.rare = ItemRarityID.LightRed;
			base.SetItemDefaults(item);
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient<Items.Miscellaneous.ChoziteBar>(8)
				.AddIngredient(ItemID.IceBlock, 25)
				.AddIngredient(ItemID.Bone, 10)
				.AddIngredient(ItemID.Sapphire)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
