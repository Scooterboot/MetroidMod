using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.missilecombo
{
	public class FlamethrowerLead : MProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Flamethrower Lead");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.scale = 1f;
			Projectile.timeLeft = 10;
			Projectile.extraUpdates = 0;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.friendly = false;
		}
		Projectile Lead;
		//int useTime = 0;
		SoundEffectInstance soundInstance;
		bool soundPlayed = false;
		int soundDelay = 0;
		public override void AI()
		{
			Projectile P = Projectile;
			Player O = Main.player[P.owner];
			Vector2 oPos = O.RotatedRelativePoint(O.MountedCenter, true);
			
			Lead = Main.projectile[(int)P.ai[0]];
			if(!Lead.active || Lead.owner != P.owner || Lead.type != ModContent.ProjectileType<ChargeLead>())
			{
				P.Kill();
				return;
			}
			else
			{
				if (P.owner == Main.myPlayer)
				{
					Vector2 diff = Main.MouseWorld - oPos;
					diff.Normalize();
					P.velocity = diff * 8f;
					P.netUpdate = true;
					
					if(soundDelay <= 0)
					{
						if(!soundPlayed)
						{
							SoundEngine.TryGetActiveSound(SoundEngine.PlaySound(Sounds.Items.Weapons.FlamethrowerStart, O.position), out ActiveSound result);
							soundInstance = result.Sound;
							soundPlayed = true;
							soundDelay = 132;
						}
						else
						{
							if(soundInstance != null)
							{
								soundInstance.Stop(true);
							}
							SoundEngine.TryGetActiveSound(SoundEngine.PlaySound(Sounds.Items.Weapons.FlamethrowerLoop, O.position), out ActiveSound result);
							soundInstance = result.Sound;
							soundDelay = 117;
						}
					}
					else
					{
						soundDelay--;
					}
				}
				P.Center = oPos;
				P.timeLeft = 2;
				/*if(useTime <= 0)
				{
					int proj = Projectile.NewProjectile(oPos.X, oPos.Y, P.velocity.X, P.velocity.Y, mod.ProjectileType("FlamethrowerShot"), P.damage, P.knockBack, P.owner);
					useTime = 2;
				}
				else
				{
					useTime--;
				}*/
			}
		}
		public override void Kill(int timeLeft)
		{
			if(soundInstance != null)
			{
				soundInstance.Stop(true);
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			return false;
		}
	}
}
