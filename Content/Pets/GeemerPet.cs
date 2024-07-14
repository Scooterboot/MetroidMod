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

			// This code is needed to customize the vanity pet display in the player select screen. Quick explanation:
			// * It uses fluent API syntax, just like Recipe
			// * You start with ProjectileID.Sets.SimpleLoop, specifying the start and end frames as well as the speed, and optionally if it should animate from the end after reaching the end, effectively "bouncing"
			// * To stop the animation if the player is not highlighted/is standing, as done by most grounded pets, add a .WhenNotSelected(0, 0) (you can customize it just like SimpleLoop)
			// * To set offset and direction, use .WithOffset(x, y) and .WithSpriteDirection(-1)
			// * To further customize the behavior and animation of the pet (as its AI does not run), you have access to a few vanilla presets in DelegateMethods.CharacterPreview to use via .WithCode(). You can also make your own, showcased in MinionBossPetProjectile
			ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(0, Main.projFrames[Projectile.type], 12)
				.WithOffset(-10f, -10f)
				.WithSpriteDirection(-1)
				.WithCode(DelegateMethods.CharacterPreview.Float);
		}

		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.BabyDino); // Copy the stats of the Zephyr Fish
			Projectile.height = 52;
			
			AIType = ProjectileID.BabyDino; // Mimic as the Zephyr Fish during AI.
		}

		public override bool PreAI() {
			Player player = Main.player[Projectile.owner];

			player.dino = false; // Relic from AIType
			return true;
		}

		public override void AI() {
			Player player = Main.player[Projectile.owner];
			Projectile.spriteDirection = player.direction;
			//Projectile.spriteDirection = -Main.player[Projectile.owner].direction;
			// Keep the projectile from disappearing as long as the player isn't dead and has the pet buff.
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
