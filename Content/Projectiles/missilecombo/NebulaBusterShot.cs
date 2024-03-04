using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.ModLoader;
using Terraria.Enums;

namespace MetroidMod.Content.Projectiles.missilecombo
{
	public class NebulaBusterShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Nebula Singularity Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.scale = 1f;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.extraUpdates = 5;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 8*(1+Projectile.extraUpdates);
		}
		
		Vector2 targetPos;
		bool initialize = false;
		
		Projectile Lead;

		NPC target;
		
		const float Max_Range = 300f;
		float range = Max_Range;
		const float Max_Distance = 60f;
		float distance = Max_Distance;
		float accuracy = 11f;
		Vector2 oPos;
		Vector2 mousePos;
		
		bool soundPlayed = false;
		
		public override void AI()
		{
			
			Projectile P = Projectile;
			Player O = Main.player[P.owner];
			
			oPos = O.RotatedRelativePoint(O.MountedCenter, true);
			
			Lead = Main.projectile[(int)P.ai[0]];
			if(!Lead.active || Lead.owner != P.owner || Lead.type != ModContent.ProjectileType<NebulaComboShot>())
			{
				P.Kill();
				return;
			}
			
			if(!initialize)
			{
				targetPos = P.Center;
				
				initialize = true;
			}
			
			range = Max_Range;
			distance = Max_Distance;
			
			if (P.owner == Main.myPlayer)
			{
				P.netUpdate = true;
				
				float rot = ((float)Math.PI/2f * P.ai[1]) + (Lead.rotation / 2f);
				Vector2 rotPoint = Lead.Center + rot.ToRotationVector2() * distance * Lead.scale;
				
				target = null;
				for(int i = 0; i < Main.maxNPCs; i++)
				{
					if(Main.npc[i].active && Main.npc[i].lifeMax > 5 && !Main.npc[i].dontTakeDamage && !Main.npc[i].friendly)
					{
						NPC npc = Main.npc[i];
						
						if(Main.npc[i].CanBeChasedBy(P, false) && Vector2.Distance(npc.Center,rotPoint) < range)
						{
							if(target == null || !target.active)
							{
								target = npc;
							}
							else
							{
								if(npc != target && Vector2.Distance(npc.Center,rotPoint) < Vector2.Distance(target.Center,rotPoint))
								{
									target = npc;
								}
								
								if(Vector2.Distance(npc.Center,rotPoint) > range)
								{
									target = null;
								}
							}
						}
					}
				}
				
				if(target != null && target.active)
				{
					targetPos = target.Center;
					if(!soundPlayed)
					{
						Terraria.Audio.SoundEngine.PlaySound(Terraria.ID.SoundID.Item43,P.Center);
						soundPlayed = true;
					}
				}
				else
				{
					soundPlayed = false;
					if(P.numUpdates == 0)
					{
						targetPos = rotPoint;
						int r = 50;
						targetPos.X += Main.rand.Next(-r, r+1);
						targetPos.Y += Main.rand.Next(-r, r+1);
					}
				}
				
				float speed = Math.Max(8f,Vector2.Distance(targetPos,P.Center) * 0.025f);
				float targetAngle = (float)Math.Atan2((targetPos.Y - P.Center.Y), (targetPos.X - P.Center.X));
				P.velocity = targetAngle.ToRotationVector2() * speed;
			}
			
			if(O.controlUseItem)
			{
				P.timeLeft = 10;
			}
			else
			{
				P.Kill();
			}
		}
		
		public override void Kill(int timeLeft)
		{
			
		}

		public override void CutTiles()
		{
			if(Lead != null && Lead.active)
			{
				DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
				Utils.PlotTileLine(Lead.Center, Projectile.Center, (Projectile.width + 16) * Projectile.scale, DelegateMethods.CutTiles);
			}
		}
		
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if(Lead != null && Lead.active)
			{
				float point = 0f;
				return projHitbox.Intersects(targetHitbox) ||
					Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Lead.Center, Projectile.Center, Projectile.width, ref point);
			}
			return false;
		}
		
		public override bool PreDraw(ref Color lightColor)
		{
			return false;
		}
	}
}
