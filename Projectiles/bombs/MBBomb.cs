using Terraria;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroidMod.Projectiles.bombs
{
	public class MBBomb : ModProjectile
	{
		public override void SetDefaults()
		{
				projectile.name = "Morph Ball Bomb";
			projectile.width = 10;
			projectile.height = 10;
			projectile.aiStyle = 14;
			projectile.timeLeft = 30;
			projectile.ownerHitCheck = true;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.tileCollide = true;
			projectile.penetrate = 1;
			projectile.ignoreWater = true;
			projectile.ranged = true;
			projectile.light = 0.2f;
			Main.projFrames[projectile.type] = 6;
		}
		public override void AI()
		{
			if (projectile.owner == Main.myPlayer && projectile.timeLeft <= 3)
			{
				projectile.tileCollide = false;
				// Set to transparant. This projectile technically lives as  transparant for about 3 frames
				projectile.alpha = 255;
				// change the hitbox size, centered about the original projectile center. This makes the projectile damage enemies during the explosion.
				projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
				projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
				projectile.width = 100;
				projectile.height = 100;
				projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
				projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
				projectile.damage = 10;
				projectile.knockBack = 4f;
			}
			//float scalez = 0.2f;
			//Lighting.AddLight((int)((projectile.position.X + (float)(projectile.width / 2)) / 16f), (int)((projectile.position.Y + (float)(projectile.height / 2)) / 16f), scalez, scalez, scalez);  
			#region frames
			projectile.frameCounter++;
			if (projectile.frameCounter >= (int)((float)projectile.timeLeft/3.75f))
			{
				projectile.frame++;
				projectile.frameCounter = 0;
			}
			if (projectile.frame >= 6)
			{
				projectile.frame = 0;
			}
			#endregion
		}
		public override void Kill(int timeLeft)
		{
			projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
			projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
			projectile.width = 8;
			projectile.height = 8;
			projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
			projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
			projectile.active = false;
			//ModPlayer.grappled = false;
			Main.PlaySound(SoundLoader.customSoundType, (int)projectile.position.X, (int)projectile.position.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/BombExplode"));
			
			float Xthreshold = 10f; //max speed
			float BombRadius = 50f; //max speed
			for (int num70 = 0; num70 < 25; num70++)
			{
				int num71 = Dust.NewDust(new Vector2(this.projectile.position.X-BombRadius, this.projectile.position.Y-BombRadius), this.projectile.width+(int)BombRadius*2, this.projectile.height+(int)BombRadius*2, 59, 0f, 0f, 100, default(Color), 5f);
				Main.dust[num71].velocity *= 1.4f;
				Main.dust[num71].noGravity = true;
				int num72 = Dust.NewDust(new Vector2(this.projectile.position.X-BombRadius, this.projectile.position.Y-BombRadius), this.projectile.width+(int)BombRadius*2, this.projectile.height+(int)BombRadius*2, 61, 0f, 0f, 100, default(Color), 5f);
				Main.dust[num72].velocity *= 1.4f;
				Main.dust[num72].noGravity = true;
			}
			Rectangle rect = new Rectangle((int)(projectile.position.X-BombRadius), (int)(projectile.position.Y-BombRadius), (int)(projectile.width+BombRadius*2), (int)(projectile.height+BombRadius*2));
			//MProjectile mpr = projectile.GetSubClass<MProjectile>();
			//mpr.CreateDamageHitBox(projectile, rect, projectile.damage);
			foreach (NPC n in Main.npc)
			if (n.active && !n.friendly && !n.dontTakeDamage && n.type != 488 && !n.boss)
			{
				float rotation = (float)Math.Atan2(n.Center.Y - projectile.Center.Y, n.Center.X - projectile.Center.X);
				Vector2 MyVec = rotation.ToRotationVector2();
				float num197 = Vector2.Distance(n.position,projectile.position);
				if (num197 < BombRadius)
				{
					n.velocity += MyVec * (BombRadius - num197);
					//n.StrikeNPC(this.projectile.damage, this.projectile.knockBack, this.projectile.direction, false, false);
					
				if (n.velocity.X > Xthreshold)
				{
					n.velocity.X = Xthreshold;
				}
				if (n.velocity.X < -Xthreshold)
				{
					n.velocity.X = -Xthreshold;
				}
				if (n.velocity.Y > Xthreshold)
				{
					n.velocity.Y = Xthreshold;
				}
				if (n.velocity.Y < -Xthreshold)
				{
					n.velocity.Y = -Xthreshold;
				}
				}
			}
			foreach (Player n in Main.player)
			if (n.active && n.hostile && n.team != Main.player[projectile.owner].team)
			{
				float rotation = (float)Math.Atan2(n.Center.Y - projectile.Center.Y, n.Center.X - projectile.Center.X);
				Vector2 MyVec = rotation.ToRotationVector2();
				float num197 = Vector2.Distance(n.position,projectile.position);
				if (num197 < BombRadius)
				{
					n.velocity += MyVec * (BombRadius - num197);
					//n.Hurt((int)projectile.damage, (int)projectile.knockBack,true,false," was slain...",false);
				
				if (n.velocity.X > Xthreshold)
				{
					n.velocity.X = Xthreshold;
				}
				if (n.velocity.X < -Xthreshold)
				{
					n.velocity.X = -Xthreshold;
				}
				if (n.velocity.Y > Xthreshold)
				{
					n.velocity.Y = Xthreshold;
				}
				if (n.velocity.Y < -Xthreshold)
				{
					n.velocity.Y = -Xthreshold;
				}
				}
			}
			Player O = Main.player[projectile.owner];
			float targetrotation = (float)Math.Atan2(O.Center.Y - projectile.Center.Y, O.Center.X - projectile.Center.X);
			Vector2 MyVe1c = targetrotation.ToRotationVector2();
			float num1917 = Vector2.Distance(O.position,projectile.position);
			if (num1917 < BombRadius)
			{
				O.velocity += MyVe1c * (BombRadius - num1917);
				MPlayer mp = O.GetModPlayer<MPlayer>(mod);
				if(mp.spiderball)
				{
					mp.spiderball = false;
				}

				if (O.velocity.X > Xthreshold)
				{
					O.velocity.X = Xthreshold;
				}
				if (O.velocity.X < -Xthreshold)
				{
					O.velocity.X = -Xthreshold;
				}
				if (O.velocity.Y > Xthreshold)
				{
					O.velocity.Y = Xthreshold;
				}
				if (O.velocity.Y < -Xthreshold)
				{
					O.velocity.Y = -Xthreshold;
				}
			}
		}
	}
}