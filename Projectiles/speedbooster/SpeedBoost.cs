using Terraria;
using Terraria.ID;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace MetroidMod.Projectiles.speedbooster
{
	public class SpeedBoost : ModProjectile
	{
		int SpeedSound = 0;
		public SoundEffectInstance soundInstance;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Speed Booster");
		}
		public override void SetDefaults()
		{
			projectile.width = 48;
			projectile.height = 48;
			projectile.aiStyle = 0;
			projectile.tileCollide = false;
			projectile.friendly = true;
			projectile.melee = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 9000;
            projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 7;
			projectile.alpha = 255;
		}
		public override void AI()
		{
			
			Player P = Main.player[projectile.owner];
			projectile.position.X = P.Center.X - projectile.width/2;
			projectile.position.Y = P.Center.Y - projectile.height/2;

			SpeedSound++;
			if(SpeedSound == 4)
			{
				soundInstance = Main.PlaySound(SoundLoader.customSoundType, (int)P.position.X, (int)P.position.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/SpeedBoosterStartup"));

			}
			if(soundInstance != null && SpeedSound == 82)
			{
				soundInstance = Main.PlaySound(SoundLoader.customSoundType, (int)P.position.X, (int)P.position.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/SpeedBoosterLoop"));

				SpeedSound = 68;
			}
			MPlayer mp = P.GetModPlayer<MPlayer>(mod);
			if(mp.ballstate || !mp.speedBoosting || mp.SMoveEffect > 0)
			{
				if(soundInstance != null)
				{
					soundInstance.Stop(true);
				}
				projectile.Kill();
			}
			foreach(Terraria.Projectile Pr in Main.projectile) if (Pr!= null)
			{
				if(Pr.active && (Pr.type == mod.ProjectileType("ShineSpark") || Pr.type == mod.ProjectileType("SpeedBall")))
				{
					if(soundInstance != null)
					{
						soundInstance.Stop(true);
					}
					projectile.Kill();
					return;
				}
			}
			Lighting.AddLight((int)((float)projectile.Center.X/16f), (int)((float)(projectile.Center.Y)/16f),  0, 0.75f, 1f);
			float rotation = (float)Math.Atan2(P.position.Y-P.shadowPos[0].Y, P.position.X-P.shadowPos[0].X);
			float rotation1 = rotation+((float)Math.PI/2);
			float rotation2 = rotation-((float)Math.PI/2);
			Vector2 vect = P.Center - new Vector2(4,4) + rotation.ToRotationVector2()*24f;
			Vector2 vel = P.position-mp.oldPosition;
			Vector2 vel1 = rotation1.ToRotationVector2()*Math.Abs(P.velocity.X);
			Vector2 vel2 = rotation2.ToRotationVector2()*Math.Abs(P.velocity.X);
			int num20 = Dust.NewDust(vect-vel1, 1, 1, 67, vel1.X+vel.X, vel1.Y+vel.Y, 100, default(Color), 2f);
			Main.dust[num20].noGravity = true;
			int num21 = Dust.NewDust(vect-vel2, 1, 1, 67, vel2.X+vel.X, vel2.Y+vel.Y, 100, default(Color), 2f);
			Main.dust[num21].noGravity = true;
		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
		    damage += (int)(target.damage * 1.5f);
		}
	}
}
