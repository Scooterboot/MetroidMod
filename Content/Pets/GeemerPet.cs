using System.Numerics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Pets
{
	public class GeemerPet : ModProjectile
	{
		public override void SetStaticDefaults() {
			Main.projFrames[Projectile.type] = 16;
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(10, 6, 12)
				.WithOffset(-10f, -10f)
				.WithSpriteDirection(-1)
				.WithCode(DelegateMethods.CharacterPreview.Float);
		}

		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.BabyDino);
			Projectile.height = 52;
			
			AIType = ProjectileID.BabyDino;
		}

		public override bool PreAI() {
			Player player = Main.player[Projectile.owner];

			player.dino = false;
			return true;
		}

		public override void AI() {
			Player player = Main.player[Projectile.owner];
			Projectile.spriteDirection = Projectile.velocity.X < 0? -1 :1;
			if (!player.dead && player.HasBuff(ModContent.BuffType<GeemerPetBuff>())) {
				Projectile.timeLeft = 2;
			}
			Projectile.frame++;
			if(Projectile.frame >= 12 && Projectile.velocity.X == 0)
			{
				Projectile.frame = 0;
			}
			if (Projectile.velocity.X != 0 && Projectile.frame >= 12)
			{
				Projectile.frame++;
				if(Projectile.frame >= Main.projFrames[Projectile.type])
				{
					Projectile.frame = 6;
				}
			}
		}
	}
}
