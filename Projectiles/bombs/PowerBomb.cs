using Terraria;
using Terraria.ID;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroidMod.Projectiles.bombs
{
	public class PowerBomb : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Power Bomb");
			Main.projFrames[projectile.type] = 6;
		}
		public override void SetDefaults()
		{
			projectile.width = 10;
			projectile.height = 10;
			projectile.aiStyle = 0;
			projectile.timeLeft = 138;
			projectile.ownerHitCheck = true;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.tileCollide = true;
			projectile.penetrate = 1;
			projectile.ignoreWater = true;
			projectile.ranged = true;
			projectile.light = 0.2f;
		}
		public override void AI()
		{
			//float scalez = 0.2f;
			//Lighting.AddLight((int)((projectile.position.X + (float)(projectile.width / 2)) / 16f), (int)((projectile.position.Y + (float)(projectile.height / 2)) / 16f), scalez, scalez, scalez);  
			#region frames
			projectile.frameCounter++;
			if (projectile.frameCounter >= (int)((float)projectile.timeLeft/7.5f))
			{
				projectile.frame++;
				projectile.frameCounter = 0;
			}
			if (projectile.frame >= 6)
			{
				projectile.frame = 0;
			}
			#endregion
			if (projectile.timeLeft == 108)
			{
Main.PlaySound(SoundLoader.customSoundType, (int)projectile.position.X, (int)projectile.position.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/PowerBombCharge2"));
			}

			if (projectile.timeLeft == 78)
			{
Main.PlaySound(SoundLoader.customSoundType, (int)projectile.position.X, (int)projectile.position.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/PowerBombCharge3"));
			}

			if (projectile.timeLeft == 48)
			{
Main.PlaySound(SoundLoader.customSoundType, (int)projectile.position.X, (int)projectile.position.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/PowerBombCharge4"));
			}

			if (projectile.timeLeft == 18)
			{
Main.PlaySound(SoundLoader.customSoundType, (int)projectile.position.X, (int)projectile.position.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/PowerBombCharge5"));
			}
		}
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundLoader.customSoundType, (int)projectile.position.X, (int)projectile.position.Y,  mod.GetSoundSlot(SoundType.Custom, "Sounds/PowerBombExplode"));
			// new
			int proj = Terraria.Projectile.NewProjectile(projectile.Center.X,projectile.Center.Y,0,0,mod.ProjectileType("PowerBombExplosion"),projectile.damage,3,projectile.owner);
			// old
			/*Projectile P = projectile;
			Vector2 PC = P.position+new Vector2(P.width/2,P.height/2);
			float DTX = 500f; //detonation radius x
			float DTY = 300f; //detonation radius y
			int Dmg = 300; //damage
			float KB = 3f; // knockback

			Rectangle MB = new Rectangle((int)(PC.X-DTX),(int)(PC.Y-DTY),(int)(DTX*2),(int)(DTY*2)); //my box
			foreach(NPC N in Main.npc)
			{
				//this is to skip some enemies
				if(!N.active) continue;
				if(N.life <= 0) continue;
				if(N.friendly) continue;
				if(N.dontTakeDamage) continue;

				Rectangle NB = new Rectangle((int)N.position.X,(int)N.position.Y,N.width,N.height); //npc box
				if(MB.Intersects(NB))  //if they touch each other somehow
				{
					N.StrikeNPC(Dmg,KB,(int)P.direction); //strike the npc
				}
			}
			for (int num70 = 0; num70 < 25; num70++)
			{
				int num71 = Dust.NewDust(new Vector2(this.projectile.position.X-DTY, this.projectile.position.Y-DTY), this.projectile.width+(int)DTY*2, this.projectile.height+(int)DTY*2, 57, 0f, 0f, 100, default(Color), 5f);
				Main.dust[num71].velocity *= 1.4f;
				Main.dust[num71].noGravity = true;
				int num72 = Dust.NewDust(new Vector2(this.projectile.position.X-DTY, this.projectile.position.Y-DTY), this.projectile.width+(int)DTY*2, this.projectile.height+(int)DTY*2, 57, 0f, 0f, 100, default(Color), 5f);
				Main.dust[num72].velocity *= 1.4f;
				Main.dust[num72].noGravity = true;
			}*/
			projectile.active = false;
		}
	}
}