using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles.missilecombo
{
	public class IceSpreaderDiffusionShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Spreader Shot");
		}
		
		bool initialised = false;
		float radius = 0.0f;
		public float spin = 0.0f;
		Vector2 basePosition = new Vector2(0f,0f);
		Vector2 prevPosition = new Vector2(0f,0f);
		
		public override void SetDefaults()
		{
			projectile.width = 16;
			projectile.height = 16;
			projectile.aiStyle = -1;
			projectile.timeLeft = 40;
			projectile.friendly = true;
			projectile.tileCollide = false;//true;
			projectile.penetrate = -1;
			projectile.ignoreWater = true;
			projectile.ranged = true;
			projectile.extraUpdates = 3;//4;
		}

		public void initialise()
		{
			basePosition = projectile.Center;
			initialised = true;
		}
		public override void AI()
		{
			if(!initialised)
			{
				initialise();
			}
			Projectile P = projectile;
			radius += 8f;
			spin += (float)(Math.PI/32);
			P.rotation = 0f;
			P.position = (basePosition - new Vector2(P.width/2,P.height/2)) + spin.ToRotationVector2()*radius;
			
			int dust = Dust.NewDust(P.position, P.width, P.height, 135, 0, 0, 100, default(Color), 3f + 3f * ((float)P.timeLeft / 40f));
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity = new Vector2((Main.rand.Next(50)-25)*0.1f, (Main.rand.Next(50)-25)*0.1f);
		}
		
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{	
			target.AddBuff(mod.BuffType("InstantFreeze"),600,true);
		}
		
		public override bool PreDraw(SpriteBatch sb, Color lightColor)
		{
			mProjectile.DrawCenteredTrail(projectile, sb);
			return false;
		}
	}
}