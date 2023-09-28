using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using MetroidMod.Common.Players;
using MetroidMod.Content.Items.Weapons;

namespace MetroidMod.Content.Projectiles.hyperbeam
{
	public class HyperBeamShot : MProjectile
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/hyperbeam/HyperBeamShot";
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Hyper Beam Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.scale = 2f;
		}

		bool spawned = false;
		float scale = 0f;
		public override bool PreAI()
		{
			if(!spawned)
			{
				scale = Projectile.scale;
				spawned = true;
			}
			return true;
		}
		public override void AI()
		{
			Projectile P = Projectile;
			MPlayer mp = Main.player[P.owner].GetModPlayer<MPlayer>();
			string S  = PowerBeam.SetCondition();

			bool isWave = S.Contains("wave") || S.Contains("nebula"),
			isSpazer = S.Contains("spazer") || S.Contains("wide") || S.Contains("vortex"),
			isPlasma = S.Contains("plasmagreen") || S.Contains("nova") || S.Contains("solar");

			P.rotation = (float)Math.Atan2((double)P.velocity.Y, (double)P.velocity.X) + 1.57f;
			
			Lighting.AddLight(P.Center, (float)mp.r/255f, (float)mp.g/255f, (float)mp.b/255f);
			
			P.localAI[0] = Math.Min(P.localAI[0]+0.075f,1f);
			P.localAI[1] = Math.Min(P.localAI[1]+0.025f,1f);
			
			P.scale = scale * P.localAI[0];

			if (isWave)
			{
				Projectile.tileCollide = false;
			}
			if (isPlasma)
			{
				Projectile.penetrate = -1;
				Projectile.usesLocalNPCImmunity = true;
				Projectile.localNPCHitCooldown = 10;
			}
			if (isSpazer || isWave)
			{
				mProjectile.WaveBehavior(Projectile, !isWave);
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			float scale = 0.65f;
			if(Projectile.Name.Contains("Plasma"))
			{
				scale = 1f;
			}
			MPlayer mp = Main.player[Projectile.owner].GetModPlayer<MPlayer>();
			mProjectile.PlasmaDrawTrail(Projectile,Main.player[Projectile.owner],Main.spriteBatch,10,scale*Projectile.localAI[0]*Projectile.localAI[1],new Color(mp.r, mp.g, mp.b, 128));
			return false;
		}
		public override void Kill(int timeLeft)
		{
			MPlayer mp = Main.player[Projectile.owner].GetModPlayer<MPlayer>();
			mProjectile.DustyDeath(Projectile, 66, true, 1f, new Color(mp.r, mp.g, mp.b, 255));
		}
	}
	public class PlasmaHyperBeamShot : HyperBeamShot
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/hyperbeam/PlasmaHyperBeamShot";
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Plasma Hyper Beam Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.scale = 3f;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 4;
		}
	}
}
