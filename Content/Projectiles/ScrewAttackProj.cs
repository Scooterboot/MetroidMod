using Terraria;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetroidMod.Common.Players;

namespace MetroidMod.Content.Projectiles
{
	public class ScrewAttackProj : ModProjectile
	{
		private bool Initialized = false;
		private int Init = 0;
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Screw Attack");
			Main.projFrames[Projectile.type] = 4;
		}
		public override void SetDefaults()
		{
				Projectile.width = 64;
				Projectile.height = 64;
				Projectile.aiStyle = 0;
				Projectile.friendly = true;
				Projectile.hostile = false;
				Projectile.penetrate = -1;
				Projectile.DamageType = DamageClass.Melee;//Projectile.melee = true;
				Projectile.alpha = 255;
				Projectile.tileCollide = false;
				Projectile.ownerHitCheck = true;
				Projectile.usesLocalNPCImmunity = true;
				Projectile.localNPCHitCooldown = 7;

		}
		public void Initialize()
		{
			Player P = Main.player[Projectile.owner];
			MPlayer mp = P.GetModPlayer<MPlayer>();
			int dustType = 15;
			if(mp.screwAttackSpeedEffect > 30)
			{
				dustType = 57;
			}
			for(float i = 0f; i < (float)(Math.PI*2); i += (float)(Math.PI/16))
			{
				Vector2 position = Projectile.Center + i.ToRotationVector2()*24;
				int num20 = Dust.NewDust(position, 1, 1, dustType, 0f, 0f, 100, default(Color), 2f);
				Main.dust[num20].position = position;
				Main.dust[num20].velocity -= P.velocity;
				Main.dust[num20].velocity *= 0.3f;
				Main.dust[num20].noGravity = true;
			}
			Initialized = true;
		}
		int DelayTime = 0;
		bool initialSoundPlayed = false;
		Vector2 lastVel = Vector2.Zero;
		public override void AI()
		{
			Projectile.scale = 0.9f;
			//Projectile.penetrateImmuneTime = 7;
			Player P = Main.player[Projectile.owner];
			MPlayer mp = P.GetModPlayer<MPlayer>();
			Projectile.position.X = P.Center.X-Projectile.width/2;
			Projectile.position.Y = P.Center.Y-Projectile.height/2;
			Projectile.direction = P.direction;
			Projectile.spriteDirection = P.direction;
			//Projectile.rotation += mp.rotation;
			Projectile.rotation = 0f;
			DelayTime++;
			if(DelayTime > 16 || !initialSoundPlayed)
			{
				Terraria.Audio.SoundEngine.PlaySound(Sounds.Items.Weapons.ScrewAttack, P.position);
				initialSoundPlayed = true;
				DelayTime = 0;
			}

			if(!mp.somersault || !mp.screwAttack)
			{
				Projectile.Kill();
			}
			else
			{
				lastVel = P.velocity;
			}
			foreach(Projectile Pr in Main.projectile) if (Pr!= null)
			{
				if(Pr.active && (Pr.type == ModContent.ProjectileType<ShineSpark>() || Pr.type == ModContent.ProjectileType<SpeedBall>()))
				{
					  Projectile.Kill();
					  return;
				}
			}
			Projectile.frameCounter++;
			if(Projectile.frameCounter >= 3)
			{
				Projectile.frame++;
				Projectile.frameCounter = 0;
			}
			if (Projectile.frame >= 4)
			{
				Projectile.frame = 0;
			}
			Vector3 color = new Vector3(0.85f, 0.92f, 1);
			if(Projectile.frame == 1)
			{
				color = new Vector3(1, 1, 0.85f);
			}
			if(Projectile.frame == 2)
			{
				color = new Vector3(1, 0.85f, 1);
			}
			if(Projectile.frame == 3)
			{
				color = new Vector3(0.85f, 1, 0.85f);
			}
			if(mp.screwAttackSpeedEffect > 30)
			{
				color = new Vector3(1, 0.85f, 0);
			}
			Vector2 pos = new Vector2(Projectile.Center.X, Projectile.Center.Y);
			Lighting.AddLight(pos, color);
			Init++;
			if(Init > 2)
			{
				if(!Initialized)
				{
					Initialize();
				}
				Init = 3;
			}
		}
		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			modifiers.FinalDamage += target.damage * 2;
		}
		public override void Kill(int timeLeft)
		{
			Player P = Main.player[Projectile.owner];
			MPlayer mp = P.GetModPlayer<MPlayer>();
			int dustType = 15;
			if(mp.screwAttackSpeedEffect > 30)
			{
				dustType = 57;
			}
			for(float i = 0f; i < (float)(Math.PI*2); i += (float)(Math.PI/16))
			{
				Vector2 position = Projectile.Center + i.ToRotationVector2()*24;
				int num20 = Dust.NewDust(position, 1, 1, dustType, 0f, 0f, 100, default(Color), 2f);
				Main.dust[num20].position = position;
				Main.dust[num20].velocity += lastVel;
				Main.dust[num20].velocity *= 0.3f;
				Main.dust[num20].noGravity = true;
			}
		}
	}
}
