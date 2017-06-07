using Terraria;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroidMod.Projectiles
{
	public class ScrewAttackProj : ModProjectile
	{
		bool initialized = false;
		int init = 0;
		public override void SetDefaults()
		{
				projectile.name = "Screw Attack";
				projectile.width = 64;
				projectile.height = 64;
				projectile.aiStyle = 0;
				projectile.friendly = true;
				projectile.hostile = false;
				projectile.penetrate = -1;
				projectile.melee = true;
				projectile.alpha = 255;
				projectile.tileCollide = false;
				projectile.ownerHitCheck = true;
				Main.projFrames[projectile.type] = 4;
				projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 7;

		}
		public void initialize()
		{
			Player P = Main.player[projectile.owner];
			MPlayer mp = P.GetModPlayer<MPlayer>(mod);
			int dustType = 15;
			if(mp.screwAttackSpeedEffect > 30)
			{
				dustType = 57;
			}
			for(float i = 0f; i < (float)(Math.PI*2); i += (float)(Math.PI/16))
			{
				Vector2 position = projectile.Center + i.ToRotationVector2()*24;
				int num20 = Dust.NewDust(position, 1, 1, dustType, 0f, 0f, 100, default(Color), 2f);
				Main.dust[num20].position = position;
				Main.dust[num20].velocity -= P.velocity;
				Main.dust[num20].velocity *= 0.3f;
				Main.dust[num20].noGravity = true;
			}
			initialized = true;
		}
		int DelayTime = 0;
		bool initialSoundPlayed = false;
		Vector2 lastVel = Vector2.Zero;
		public override void AI()
		{
			projectile.scale = 0.9f;
			//projectile.penetrateImmuneTime = 7;
			Player P = Main.player[projectile.owner];
			MPlayer mp = P.GetModPlayer<MPlayer>(mod);
			projectile.position.X = P.Center.X-projectile.width/2;
			projectile.position.Y = P.Center.Y-projectile.height/2;
			projectile.direction = P.direction;
			projectile.spriteDirection = P.direction;
			//projectile.rotation += mp.rotation;
			projectile.rotation = 0f;
			DelayTime++;
			if(DelayTime > 16 || !initialSoundPlayed)
			{
				Main.PlaySound(SoundLoader.customSoundType, (int)P.position.X, (int)P.position.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/ScrewAttackSound"));
				initialSoundPlayed = true;
				DelayTime = 0;
			}

			if(!mp.somersault)
			{
				projectile.Kill();
			}
			else
			{
				lastVel = P.velocity;
			}
			foreach(Projectile Pr in Main.projectile) if (Pr!= null)
			{
				if(Pr.active && (Pr.type == mod.ProjectileType("ShineSpark") || Pr.type ==mod.ProjectileType("SpeedBall")))
				{
					  projectile.Kill();
					  return;
				}
			}
			projectile.frameCounter++;
			if(projectile.frameCounter >= 3)
			{
				projectile.frame++;
				projectile.frameCounter = 0;
			}
			if (projectile.frame >= 4)
			{
				projectile.frame = 0;
			}
			Vector3 color = new Vector3(0.85f, 0.92f, 1);
			if(projectile.frame == 1)
			{
				color = new Vector3(1, 1, 0.85f);
			}
			if(projectile.frame == 2)
			{
				color = new Vector3(1, 0.85f, 1);
			}
			if(projectile.frame == 3)
			{
				color = new Vector3(0.85f, 1, 0.85f);
			}
			if(mp.screwAttackSpeedEffect > 30)
			{
				color = new Vector3(1, 0.85f, 0);
			}
			Vector2 pos = new Vector2(projectile.Center.X, projectile.Center.Y);
			Lighting.AddLight(pos, color);
			init++;
			if(init > 2)
			{
				if(!initialized)
				{
					initialize();
				}
				init = 3;
			}
		}
		/*public override void DamageNPC(NPC npc, int hitDir, ref int damage, ref float knockback, ref bool crit, ref float critMult)
		{
			damage = (int)((double)damage + (double)npc.defense * 0.5);
		}*/
		public override void Kill(int timeLeft)
		{
			Player P = Main.player[projectile.owner];
			MPlayer mp = P.GetModPlayer<MPlayer>(mod);
			int dustType = 15;
			if(mp.screwAttackSpeedEffect > 30)
			{
				dustType = 57;
			}
			for(float i = 0f; i < (float)(Math.PI*2); i += (float)(Math.PI/16))
			{
				Vector2 position = projectile.Center + i.ToRotationVector2()*24;
				int num20 = Dust.NewDust(position, 1, 1, dustType, 0f, 0f, 100, default(Color), 2f);
				Main.dust[num20].position = position;
				Main.dust[num20].velocity += lastVel;
				Main.dust[num20].velocity *= 0.3f;
				Main.dust[num20].noGravity = true;
			}
		}
	}
}
