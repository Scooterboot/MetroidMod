using Terraria;
using Terraria.Audio;
using Terraria.ID;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetroidMod.Common.Players;

namespace MetroidMod.Content.Projectiles
{
	public class ShineBall : ModProjectile
	{
		private int ShineSoundStart = 0;
		public ActiveSound activeSound;
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Shine Ball");
		}
public override void SetDefaults()
		{
			Projectile.width = 48;
			Projectile.height = 48;
			Projectile.aiStyle = 0;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;//Projectile.melee = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 9000;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 7;
			Projectile.alpha = 255;
		}
		public override void AI()
		{
			Player P = Main.player[Projectile.owner];
			Projectile.position.X=P.Center.X-Projectile.width/2;
			Projectile.position.Y=P.Center.Y-Projectile.height/2;
			ShineSoundStart++;
			if(ShineSoundStart > 3 && ShineSoundStart < 5)
			{
				if (SoundEngine.TryGetActiveSound(SoundEngine.PlaySound(Sounds.Items.Weapons.ShineSpark, P.position), out activeSound))
				{
					ShineSoundStart = 6;
					if (ShineSoundStart > 6)
					{
						ShineSoundStart = 6;
					}
				}
			}
			MPlayer mp = P.GetModPlayer<MPlayer>();
			if(mp.shineDirection == 0 || !mp.shineActive || !mp.ballstate)
			{
				Projectile.Kill();
			}
			Lighting.AddLight((int)((float)Projectile.Center.X/16f), (int)((float)(Projectile.Center.Y)/16f), 1f, 0.85f, 0);

			if (activeSound != null)
			{
				activeSound.Position = P.Center;
			}
		}	
		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			damage += target.damage * 2;
		}
	}
}
