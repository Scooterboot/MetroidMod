using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles.missilecombo
{
	public class FlamethrowerLead : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flamethrower Lead");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.width = 16;
			projectile.height = 16;
			projectile.scale = 1f;
			projectile.timeLeft = 10;
			projectile.extraUpdates = 0;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.friendly = false;
		}
		Projectile Lead;
		//int useTime = 0;
		SoundEffectInstance soundInstance;
		bool soundPlayed = false;
		int soundDelay = 0;
		public override void AI()
		{
			Projectile P = projectile;
			Player O = Main.player[P.owner];
			Vector2 oPos = O.RotatedRelativePoint(O.MountedCenter, true);
			
			Lead = Main.projectile[(int)P.ai[0]];
			if(!Lead.active || Lead.owner != P.owner || Lead.type != mod.ProjectileType("ChargeLead"))
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
							soundInstance = Main.PlaySound(SoundLoader.customSoundType, (int)O.position.X, (int)O.position.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/FlamethrowerStart"));
							soundPlayed = true;
							soundDelay = 132;
						}
						else
						{
							if(soundInstance != null)
							{
								soundInstance.Stop(true);
							}
							soundInstance = Main.PlaySound(SoundLoader.customSoundType, (int)O.position.X, (int)O.position.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/FlamethrowerLoop"));
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
		public override bool PreDraw(SpriteBatch sb, Color lightColor)
		{
			return false;
		}
	}
}
