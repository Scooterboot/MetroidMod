using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.missilecombo
{
	public class VortexComboLead : MProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Vortex Combo Lead");
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
					P.netUpdate = true;
					
					if(soundInstance == null || soundInstance.State != SoundState.Playing)
					{
						SoundEngine.TryGetActiveSound(SoundEngine.PlaySound(Sounds.Items.Weapons.VortexComboSoundLoop, O.position), out ActiveSound result);
						soundInstance = result.Sound;
						if(Main.soundVolume > 0f)
						{
							soundInstance.Volume = 0f;
						}
					}
					else if(Main.soundVolume > 0f)
					{
						soundInstance.Volume = Math.Min(soundInstance.Volume + 0.05f * Main.soundVolume, 1f * Main.soundVolume);
					}
				}
				P.timeLeft = 2;
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
