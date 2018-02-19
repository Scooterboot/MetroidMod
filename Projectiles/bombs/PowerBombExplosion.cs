using Terraria;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroidMod.Projectiles.bombs
{
	public class PowerBombExplosion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Power Bomb Explosion");
		}
		public override void SetDefaults()
		{
			projectile.width = 1000;
			projectile.height = 750;
			projectile.aiStyle = -1;
			projectile.timeLeft = 200;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.ignoreWater = true;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 1;
		}
		float scaleSize = 1f;
		Color colory = Color.Gold;
		int width = 1000;
		int height = 750;
		int maxDistance = 55;
		public override void AI()
		{
			colory = Color.Gold;
			projectile.timeLeft = 60;
			projectile.frameCounter++;
			//projectile.penetrateImmuneTime = 0;
			float speed = 2f;
			if (projectile.frameCounter < maxDistance)
			{
				scaleSize += speed;
				colory = Color.Yellow;
			}
			else
			{
				scaleSize -= speed;
				colory = Color.Black;
				projectile.damage = 0;
				for(int i = 0; i < Main.item.Length; i++)
				{
					if(Main.item[i].active)
					{
						Item I = Main.item[i];
						if(projectile.Hitbox.Intersects(I.Hitbox))
						{
							float angle = (float)Math.Atan2((projectile.Center.Y-I.Center.Y),(projectile.Center.X-I.Center.X));
							I.velocity = angle.ToRotationVector2()*8;
							I.position += I.velocity;
						}
					}
				}
			}
			if (projectile.frameCounter >= (maxDistance*2))
			{
				scaleSize = 1f;
				colory = Color.Gold;
				projectile.frameCounter = 0;
				//projectile.active = false;
				projectile.Kill();
			}
			projectile.scale = scaleSize*0.02f;
			projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
			projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
			projectile.width = (int)((float)width * projectile.scale);
			projectile.height = (int)((float)height * projectile.scale);
			projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
			projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
			if (projectile.frameCounter == maxDistance)
			{
				Rectangle tileRect = new Rectangle((int)(projectile.position.X / 16), (int)(projectile.position.Y / 16), (projectile.width / 16), (projectile.height / 16));
				for (int x = tileRect.X; x < tileRect.X + tileRect.Width; x++)
				{
				    for (int y = tileRect.Y; y < tileRect.Y + tileRect.Height; y++)
				    {
					if (Main.tile[x, y].active())
					{
					    if (Main.tile[x, y].type == (ushort)mod.TileType("YellowHatch"))
					    {
						TileLoader.HitWire(x, y, mod.TileType("YellowHatch"));
					    }
					    if (Main.tile[x, y].type == (ushort)mod.TileType("YellowHatchVertical"))
					    {
						TileLoader.HitWire(x, y, mod.TileType("YellowHatchVertical"));
					    }
					    if (Main.tile[x, y].type == (ushort)mod.TileType("BlueHatch"))
					    {
						TileLoader.HitWire(x, y, mod.TileType("BlueHatch"));
					    }
					    if (Main.tile[x, y].type == (ushort)mod.TileType("BlueHatchVertical"))
					    {
						TileLoader.HitWire(x, y, mod.TileType("BlueHatchVertical"));
					    }
					}
				    }
				}
			}
		}
		public override bool PreDraw(SpriteBatch sb, Color lightColor)
		{
			Texture2D tex = Main.projectileTexture[projectile.type];
			sb.Draw(tex, projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), colory, projectile.rotation, new Vector2(tex.Width/2, tex.Height/2), projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
		public override void ModifyHitNPC(NPC npc, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if(npc.defense < 1000)
			{
				damage = (int)((double)damage + (double)npc.defense * 0.5);
			}
		}
	}
}
