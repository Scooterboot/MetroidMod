using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using MetroidMod.Common.Players;
using MetroidMod.Content.Items.Weapons;

namespace MetroidMod.Content.Projectiles.hyperbeam
{
	public class ExtraHyperBeamShot : MProjectile
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/hyperbeam/ExtraHyperBeamShot";
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Hyper Beam Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.scale = 1f;
			mProjectile.amplitude = 10f * Projectile.scale * 2f;
			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 3;
		}

		bool initialized = false;
		float speed = 8f;
		public override void AI()
		{
			Projectile P = Projectile;
			MPlayer mp = Main.player[P.owner].GetModPlayer<MPlayer>();

			bool isWave = (shot.Contains("wave") || shot.Contains("nebula")),
isSpazer = shot.Contains("spazer") || shot.Contains("wide") || shot.Contains("vortex"),
isPlasma = shot.Contains("plasmagreen") || shot.Contains("nova") || shot.Contains("solar"),
isNebula = shot.Contains("nebula");


			if (!initialized)
			{
				speed = P.velocity.Length();
				initialized = true;
			}
			
			if(isSpazer || isWave)
			{
				mProjectile.WaveBehavior(Projectile, !isWave);
			}
			if(isNebula)
			{
				if(initialized)
				{
					mProjectile.HomingBehavior(P,speed);
				}
			}
			if (isPlasma)
			{
				Projectile.penetrate = -1;
				Projectile.usesLocalNPCImmunity = true;
				Projectile.localNPCHitCooldown = 10;
			}
			if (isWave)
			{
				Projectile.tileCollide = false;
			}
			Lighting.AddLight(P.Center, (float)mp.r/255f, (float)mp.g/255f, (float)mp.b/255f);
			
			Vector2 velocity = Projectile.position - Projectile.oldPos[0];
			if(Vector2.Distance(Projectile.position, Projectile.position+velocity) < Vector2.Distance(Projectile.position,Projectile.position+Projectile.velocity))
			{
				velocity = Projectile.velocity;
			}
			Projectile.rotation = (float)Math.Atan2(velocity.Y, velocity.X) + 1.57f;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			MPlayer mp = Main.player[Projectile.owner].GetModPlayer<MPlayer>();
			mProjectile.PlasmaDrawTrail(Projectile,Main.player[Projectile.owner],Main.spriteBatch,10,0.6f,new Color(mp.r, mp.g, mp.b, 128));
			return false;
		}
		public override void OnKill(int timeLeft)
		{
			MPlayer mp = Main.player[Projectile.owner].GetModPlayer<MPlayer>();
			mProjectile.DustyDeath(Projectile, 66, true, 1f, new Color(mp.r, mp.g, mp.b, 255));
		}
	}
}
