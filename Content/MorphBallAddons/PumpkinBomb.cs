using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.MorphBallAddons
{
	public class PumpkinBomb : Bomb, IGeneratesOnStatues
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/MBAddons/PumpkinBomb/PumpkinBombItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/PumpkinBomb/PumpkinBombTile";

		public override string BombProjectileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/PumpkinBomb/PumpkinBombProjectile";

		public int ChanceToGenerateOnStatue(int x, int y, int statueType, bool chozoRoom)
		{
			return Common.Configs.MConfigMain.Instance.drunkWorldHasDrunkStatues
				? 4
				: 0
				;
		}

		public override void ItemSetDefaults()
		{
			Item.damage = 100;
			Item.value = Item.buyPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.Yellow;
		}

		public override void BombKill( ref int dustType, ref int dustType2, ref float dustScale, ref float dustScale2)
		{
			dustType = 6;
			dustType2 = 6;

			int max = 3;
			float angle = Main.rand.Next(360 / max);
			for (int i = 0; i < max; i++)
			{
				float rot = (float)Angle.ConvertToRadians(angle + (360f / max * i));
				Vector2 vel = rot.ToRotationVector2() * 5f;
				Projectile proj = Main.projectile[Projectile.NewProjectile(BombProjectile.Projectile.GetSource_FromThis(), BombProjectile.Projectile.Center, vel, ProjectileID.JackOLantern, BombProjectile.Projectile.damage / max, BombProjectile.Projectile.knockBack + 3, BombProjectile.Projectile.owner)];
				proj.timeLeft = 60;
			}
		}
		public override void BombOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			// Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.DirectionTo(target.Center) * 8, ProjectileID.FlamingJack, (int)(damage * 1.5f), knockback + 3, Projectile.owner, target.whoAmI);
		}
	}
}
