using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles.missilecombo
{
	public class VortexComboLead : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vortex Combo Lead");
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
					P.netUpdate = true;
					
					if(soundInstance == null || soundInstance.State != SoundState.Playing)
					{
						soundInstance = Main.PlaySound(SoundLoader.customSoundType, (int)O.position.X, (int)O.position.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/VortexComboSoundLoop"));
						soundInstance.Volume = 0f;
					}
					else
					{
						soundInstance.Volume = Math.Min(soundInstance.Volume + 0.05f, 1f);
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
		public override bool PreDraw(SpriteBatch sb, Color lightColor)
		{
			return false;
		}
	}
}