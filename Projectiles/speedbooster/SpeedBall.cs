using Terraria;
using Terraria.ID;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace MetroidMod.Projectiles.speedbooster
{
	public class SpeedBall : ModProjectile
	{
		int SpeedSound = 0;
		public SoundEffectInstance soundInstance;
		public override void SetDefaults()
		{
			projectile.name = "Mock Ball";
			projectile.width = 18;
			projectile.height = 18;
			projectile.aiStyle = 0;
			projectile.tileCollide = false;
			projectile.friendly = true;
			projectile.melee = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 9000;
	ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
            projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 7;
			projectile.alpha = 255;
		}
		public override void AI()
		{
			Player P = Main.player[projectile.owner];
			projectile.position.X=P.Center.X-projectile.width/2;
			projectile.position.Y=P.Center.Y-projectile.height/2;
			
			SpeedSound++;
			if(SpeedSound == 4)
			{
				soundInstance = Main.PlaySound(SoundLoader.customSoundType, (int)P.position.X, (int)P.position.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/SpeedBoosterStartup"));
			}
			if(soundInstance != null && SpeedSound == 82)
			{
				soundInstance = Main.PlaySound(SoundLoader.customSoundType, (int)P.position.X, (int)P.position.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/SpeedBoosterLoop"));
				SpeedSound = 68;
			}
			MPlayer mp = P.GetModPlayer<MPlayer>(mod);
			if(!mp.ballstate || !mp.speedBoosting || mp.SMoveEffect > 0)
			{
				if(soundInstance != null)
				{
					soundInstance.Stop(true);
				}
				projectile.Kill();
			}
			foreach(Terraria.Projectile Pr in Main.projectile) if (Pr!= null)
			{
				if(Pr.active && (Pr.type == mod.ProjectileType("ShineBall") || Pr.type == mod.ProjectileType("SpeedBoost")))
				{
					if(soundInstance != null)
					{
						soundInstance.Stop(true);
					}
					projectile.Kill();
					return;
				}
			}
			Lighting.AddLight((int)((float)projectile.Center.X/16f), (int)((float)(projectile.Center.Y)/16f), 0, 191, 255);
		}
		/*public override void DamageNPC(NPC npc, int hitDir, ref int damage, ref float knockback, ref bool crit, ref float critMult)
		{
			damage = (int)((double)damage + (double)npc.defense * 0.5);
		}*/
	}
}