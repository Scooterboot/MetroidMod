using Terraria;
using Terraria.ID;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroidMod.Projectiles.speedbooster
{
	public class ShineSpark : ModProjectile
	{
		int ShineSoundStart = 0;
		public override void SetDefaults()
		{
			projectile.name = "Shine Spark";
			projectile.width = 48;
			projectile.height = 48;
			projectile.aiStyle = 0;
			projectile.tileCollide = false;
			projectile.friendly = true;
			projectile.melee = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 9000;
	ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
            projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 7;
			projectile.alpha = 255;
		}
		public override void AI()
		{
			Player P = Main.player[projectile.owner];
			projectile.position.X=P.Center.X-projectile.width/2;
			projectile.position.Y=P.Center.Y-projectile.height/2;
			ShineSoundStart++;
			if(ShineSoundStart > 3 && ShineSoundStart < 5)
			{
				Main.PlaySound(SoundLoader.customSoundType, (int)P.position.X, (int)P.position.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/ShineSpark"));
				ShineSoundStart = 6;
				if(ShineSoundStart > 6)
				{
					ShineSoundStart = 6;
				}
			}
			MPlayer mp = P.GetModPlayer<MPlayer>(mod);
			if(mp.shineDirection == 0 || !mp.shineActive || mp.ballstate)
			{
				projectile.Kill();
			}
			Lighting.AddLight((int)((float)projectile.Center.X/16f), (int)((float)(projectile.Center.Y)/16f), 255, 216, 0);
			float rotation = (float)Math.Atan2(P.position.Y-P.shadowPos[0].Y, P.position.X-P.shadowPos[0].X);
			float rotation1 = rotation+((float)Math.PI/2);
			float rotation2 = rotation-((float)Math.PI/2);
			Vector2 vect = P.Center - new Vector2(4,4) + rotation.ToRotationVector2()*24f;
			Vector2 vel = P.position-mp.oldPosition;
			Vector2 vel1 = rotation1.ToRotationVector2()*16f;
			Vector2 vel2 = rotation2.ToRotationVector2()*16f;
			int num20 = Dust.NewDust(vect-vel1, 1, 1, 57, vel1.X+vel.X, vel1.Y+vel.Y, 100, default(Color), 2f);
			Main.dust[num20].noGravity = true;
			int num21 = Dust.NewDust(vect-vel2, 1, 1, 57, vel2.X+vel.X, vel2.Y+vel.Y, 100, default(Color), 2f);
			Main.dust[num21].noGravity = true;
		}
		/*public override void DamageNPC(NPC npc, int hitDir, ref int damage, ref float knockback, ref bool crit, ref float critMult)
		{
			damage = (int)((double)damage + (double)npc.defense * 0.5);
		}*/
	}
}