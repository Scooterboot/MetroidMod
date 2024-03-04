using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.missilecombo
{
	public class IceSpreaderDiffusionShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Ice Spreader Shot");
		}
		
		bool initialised = false;
		float radius = 0.0f;
		public float spin = 0.0f;
		Vector2 basePosition = new Vector2(0f,0f);
		Vector2 prevPosition = new Vector2(0f,0f);
		
		float alpha = 1f;
		
		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 80;//40;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.extraUpdates = 2;
		}

		public void initialise()
		{
			basePosition = Projectile.Center;
			initialised = true;
		}
		public override void AI()
		{
			if(!initialised)
			{
				initialise();
			}
			Projectile P = Projectile;
			radius = Math.Min(radius + 8f, 320f);
			spin += (float)(Math.PI/32);
			P.rotation = 0f;
			P.position = (basePosition - new Vector2(P.width/2,P.height/2)) + spin.ToRotationVector2()*radius;
			
			int dust = Dust.NewDust(P.position, P.width, P.height, 135, 0, 0, 100, default(Color), 3f + 3f * ((float)P.timeLeft / 40f));
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity = new Vector2((Main.rand.Next(50)-25)*0.1f, (Main.rand.Next(50)-25)*0.1f);
			
			if(P.timeLeft < 40)
			{
				alpha -= 1f / 40;
			}
		}
		
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{	
			target.AddBuff(ModContent.BuffType<Buffs.InstantFreeze>(),600,true);
		}
		
		public override Color? GetAlpha(Color lightColor)
		{
			return lightColor * alpha;
		}
		
		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.DrawCenteredTrail(Projectile, Main.spriteBatch);
			return false;
		}
	}
}
