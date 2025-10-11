using MetroidMod.Content.Projectiles;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace MetroidMod.Content.MissileAddons.BeamCombos
{
	public class SpazerCombo : ModMissileAddon
	{
		public override Color PrimaryColor => MetroidMod.powColor;
		public override Color SecondaryColor => MetroidMod.powSecondaryColor;
		public override int ShotDust => DustID.YellowTorch;

		public override bool AddOnlyAddonItem => false;

		public override void SetStaticDefaults()
		{
			AddonSlot = MissileAddonSlotID.Charge;

			//All the stats are set outside of here up in Stat Values, lets me do fancy schmancy tooltip stuff
			base.SetStaticDefaults();
		}
		public override void OnSpawn(MProjectile mProjectile, IEntitySource source)
		{
			Projectile p = mProjectile.Projectile;
			if (source is EntitySource_Parent parent && parent.Entity is Player player && mProjectile is MissileShot oof)
			{
				if (oof.fileMod.Contains("Charge"))
				{
					for (int i = 0; i < 5; i++)
					{

						Vector2 oPos = player.RotatedRelativePoint(player.MountedCenter, true);
						int k = i - (5 / 2);
						Vector2 shotGunVel = Vector2.Normalize(p.velocity * 4f);
						double rot = Angle.ConvertToRadians(4.0 * k);
						shotGunVel = shotGunVel.RotatedBy(rot, default(Vector2));
						if (float.IsNaN(shotGunVel.X) || float.IsNaN(shotGunVel.Y))
						{
							shotGunVel = -Vector2.UnitY;
						}
						Projectile.NewProjectile(source, oPos.X, oPos.Y, shotGunVel.X, shotGunVel.Y, player.HeldItem.shoot, p.damage, p.knockBack, player.whoAmI);
					}
				}
			}
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.HellstoneBar, 10)
				.AddIngredient(ItemID.Topaz, 1)
				.AddIngredient(ItemID.Bone, 10)
				.AddTile(TileID.Anvils)
				//.AddDecraftCondition(Condition.DownedSkeletron)
				.Register();
		}
	}
}
