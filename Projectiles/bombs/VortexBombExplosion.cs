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
	public class VortexBombExplosion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vortex Bomb");
		}
		public override void SetDefaults()
		{
			projectile.width = 640;
			projectile.height = 640;
			projectile.scale = 0.01f;
			projectile.aiStyle = -1;
			projectile.timeLeft = 200;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.ignoreWater = true;
			projectile.extraUpdates = 3;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 1*(1+projectile.extraUpdates);
		}
		float scale = 0.01f;
		float speed = 4f;
		const int maxDist = 60;
			
		float itemVacSpeed = 5f;
		float npcVacSpeed = 5f;
		
		const int width = 640;
		const int height = 640;
		public override void AI()
        {
            Projectile P = projectile;
			
			float vacSpeedIncr = 0.05f;
			
			P.ai[0] += 1f / (1+P.extraUpdates);
			if(P.ai[0] < maxDist)
			{
				if(P.numUpdates == 0)
				{
					speed *= 0.99f;
				}
				scale += 0.04f*speed / (1+P.extraUpdates);
			}
			else
			{
				if(P.numUpdates == 0)
				{
					speed *= 1.01f;
				}
				scale -= 0.04f*speed / (1+P.extraUpdates);
				vacSpeedIncr = 0.5f;
			}
			
			if(P.numUpdates == 0)
			{
				itemVacSpeed = Math.Min(itemVacSpeed + vacSpeedIncr,20f);
				npcVacSpeed = Math.Min(npcVacSpeed + vacSpeedIncr,40f);
				
				for(int i = 0; i < Main.item.Length; i++)
				{
					if (!Main.item[i].active) continue;

					Item I = Main.item[i];
					if(P.Hitbox.Intersects(I.Hitbox))
					{
						Vector2 center = new Vector2(P.Center.X,P.Center.Y-((float)I.height/2f));
						Vector2 velocity = Vector2.Normalize(center - I.Center) * Math.Min(itemVacSpeed,Vector2.Distance(center,I.Center));
						if(Vector2.Distance(center,I.Center) > 1f)
						{
							I.position += velocity;
							I.velocity *= 0f;
						}
					}
				}
				
				for(int i = 0; i < Main.npc.Length; i++)
				{
					if(Main.npc[i].CanBeChasedBy(P, false) && Main.npc[i].knockBackResist != 0f)
					{
						NPC N = Main.npc[i];
						if(P.Hitbox.Intersects(N.Hitbox))
						{
							Vector2 center = new Vector2(P.Center.X,P.Center.Y-((float)N.height/2f));
							Vector2 velocity = Vector2.Normalize(center - N.Center) * Math.Min(npcVacSpeed * N.knockBackResist,Vector2.Distance(center,N.Center));
							if(Vector2.Distance(center,N.Center) > 1f)
							{
								N.position += velocity;
								N.velocity *= 0f;
							}
						}
					}
				}
			}
			
			/*int num = (int)(100f*P.scale);
			for(int i = 0; i < num; i++)
			{
				float angle = (float)((Math.PI*2)/num)*i;
				Vector2 position = P.Center - new Vector2(20,20);
				position.X += (float)Math.Cos(angle)*((float)P.width/2f);
				position.Y += (float)Math.Sin(angle)*((float)P.height/2f);
				int num20 = Dust.NewDust(position, 40, 40, 229, 0f, 0f, 100, default(Color), 1f);
				Dust dust = Main.dust[num20];
				dust.velocity += Vector2.Normalize(P.Center - dust.position) * 10f;// * P.scale;
				dust.noGravity = true;
			}*/
			int num = 20;
			for(int i = 0; i < num; i++)
			{
				float angle = (float)((Math.PI*2)/num)*i;
				angle += ((float)Math.PI/20f) * P.ai[0];
				Vector2 position = P.Center;
				position.X += (float)Math.Cos(angle)*((float)P.width/2f);
				position.Y += (float)Math.Sin(angle)*((float)P.height/2f);
				int num20 = Dust.NewDust(position, 1, 1, 229, 0f, 0f, 100, default(Color), MathHelper.Clamp(P.scale,1f,3f));
				Dust dust = Main.dust[num20];
				dust.position = position;
				dust.velocity = Vector2.Normalize(P.Center - dust.position) * 10f;
				dust.noGravity = true;
			}
			
			if(scale > 0f)
			{
				P.timeLeft = 2;
			}

			P.scale = scale;
			P.position.X = P.position.X + (float)(P.width / 2);
			P.position.Y = P.position.Y + (float)(P.height / 2);
			P.width = (int)((float)width * P.scale);
			P.height = (int)((float)height * P.scale);
			P.position.X = P.position.X - (float)(P.width / 2);
			P.position.Y = P.position.Y - (float)(P.height / 2);

			if ((int)P.ai[0] == maxDist && P.numUpdates == 0)
			{
				Rectangle tileRect = new Rectangle((int)(P.position.X / 16), (int)(P.position.Y / 16), (P.width / 16), (P.height / 16));
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

        public override bool PreDraw(SpriteBatch sb, Color lightColor)
		{
			Projectile P = projectile;
			Texture2D tex = Main.projectileTexture[P.type];
			if(P.scale > 0f)
			{
				sb.Draw(tex, P.Center - Main.screenPosition + new Vector2(0f, P.gfxOffY), new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), Color.White, P.rotation, new Vector2(tex.Width/2, tex.Height/2), P.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
	}
}