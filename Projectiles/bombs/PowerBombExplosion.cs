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
			DisplayName.SetDefault("Power Bomb");
		}
		public override void SetDefaults()
		{
			projectile.width = 1000;
			projectile.height = 750;
			projectile.scale = 0.02f;
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
		const int width = 1000;
		const int height = 750;
		const int maxDistance = 55;
		public override void AI()
        {
            float speed = 2f;
            colory = Color.Yellow;
			projectile.timeLeft = 60;
            projectile.frameCounter++;

			if (projectile.frameCounter < maxDistance)
			{
				scaleSize += speed;
				
				int num = (int)(50f*projectile.scale);
				for(int i = 0; i < num; i++)
				{
					float angle = (float)((Math.PI*2)/num)*i;
					Vector2 position = projectile.Center - new Vector2(10,10);
					position.X += (float)Math.Cos(angle)*((float)projectile.width/2f);
					position.Y += (float)Math.Sin(angle)*((float)projectile.height/2f);
					int num20 = Dust.NewDust(position, 20, 20, 57, 0f, 0f, 100, default(Color), 3f);
					Dust dust = Main.dust[num20];
					dust.velocity += Vector2.Normalize(projectile.Center - dust.position) * 5f * projectile.scale;
					dust.noGravity = true;
				}
			}
			else
			{
				scaleSize -= speed;
				colory = Color.Black;
				projectile.damage = 0;
				for(int i = 0; i < Main.item.Length; i++)
				{
                    if (!Main.item[i].active) continue;

					Item I = Main.item[i];
					if(projectile.Hitbox.Intersects(I.Hitbox))
					{
						Vector2 center = new Vector2(projectile.Center.X,projectile.Center.Y-((float)I.height/2f));
						Vector2 velocity = Vector2.Normalize(center - I.Center) * Math.Min(20f,Vector2.Distance(center,I.Center));
						if(Vector2.Distance(center,I.Center) > 1f)
						{
							I.position += velocity;
							I.velocity *= 0f;
						}
					}
				}
			}
			if (projectile.frameCounter >= (maxDistance*2))
			{
				scaleSize = 1f;
				colory = Color.Gold;
				projectile.frameCounter = 0;
				projectile.Kill();
			}

			projectile.scale = scaleSize*0.02f;
			projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
			projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
			projectile.width = (int)((float)width * projectile.scale);
			projectile.height = (int)((float)height * projectile.scale);
			projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
			projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
			projectile.netUpdate = true;

			if (projectile.frameCounter == maxDistance)
			{
				Rectangle tileRect = new Rectangle((int)(projectile.position.X / 16), (int)(projectile.position.Y / 16), (projectile.width / 16), (projectile.height / 16));
                for (int x = tileRect.X; x < tileRect.X + tileRect.Width; x++)
                {
                    for (int y = tileRect.Y; y < tileRect.Y + tileRect.Height; y++)
                    {
                        if (Main.tile[x, y] != null && Main.tile[x, y].active())
                        {
                            if (Main.tile[x, y].type == (ushort)mod.TileType("YellowHatch"))
                                TileLoader.HitWire(x, y, mod.TileType("YellowHatch"));
                            if (Main.tile[x, y].type == (ushort)mod.TileType("YellowHatchVertical"))
                                TileLoader.HitWire(x, y, mod.TileType("YellowHatchVertical"));
                            if (Main.tile[x, y].type == (ushort)mod.TileType("BlueHatch"))
                                TileLoader.HitWire(x, y, mod.TileType("BlueHatch"));
                            if (Main.tile[x, y].type == (ushort)mod.TileType("BlueHatchVertical"))
                                TileLoader.HitWire(x, y, mod.TileType("BlueHatchVertical"));
                        }
                    }
                }
			}
        }

        public override void ModifyHitNPC(NPC npc, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (npc.defense < 1000)
                damage = (int)(damage + npc.defense * 0.5);
        }

        public override bool PreDraw(SpriteBatch sb, Color lightColor)
		{
			Texture2D tex = Main.projectileTexture[projectile.type];
			sb.Draw(tex, projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), colory, projectile.rotation, new Vector2(tex.Width/2, tex.Height/2), projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
	}
}