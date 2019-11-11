using Terraria;
using Terraria.ID;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroidMod.Projectiles.speedbooster
{
	public class ShineBall : ModProjectile
	{
		int ShineSoundStart = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shine Ball");
		}
public override void SetDefaults()
		{
			projectile.width = 48;
			projectile.height = 48;
			projectile.aiStyle = 0;
			projectile.tileCollide = false;
			projectile.friendly = true;
			projectile.melee = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 9000;
            projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 7;
			projectile.alpha = 255;
		}
		public override void AI()
		{
			Player P = Main.player[projectile.owner];
			projectile.position.X=P.Center.X-projectile.width/2;
			projectile.position.Y=P.Center.Y-projectile.height/2;
			ShineSoundStart++;
			if(ShineSoundStart > 3 && ShineSoundStart < 5)
			{
				Main.PlaySound(SoundLoader.customSoundType, (int)P.position.X, (int)P.position.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/ShineSpark"));

				ShineSoundStart = 6;
				if(ShineSoundStart > 6)
				{
					ShineSoundStart = 6;
				}
			}
			MPlayer mp = P.GetModPlayer<MPlayer>();
			if(mp.shineDirection == 0 || !mp.shineActive || !mp.ballstate)
			{
				projectile.Kill();
			}
			Lighting.AddLight((int)((float)projectile.Center.X/16f), (int)((float)(projectile.Center.Y)/16f), 1f, 0.85f, 0);
		}	
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
		    damage += target.damage * 2;
		}
	}
}
