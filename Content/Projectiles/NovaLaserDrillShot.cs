using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using System.IO;
using MetroidModPorted.Common.GlobalItems;

namespace MetroidModPorted.Content.Projectiles
{
	public class NovaLaserDrillShot : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nova Laser Drill");
		}
		public override void SetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.aiStyle = -1;//75;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			//Projectile.hide = true;
			Projectile.DamageType = DamageClass.Melee;//Projectile.melee = true;
			Projectile.ignoreWater = true;
			Projectile.ownerHitCheck = true;
		}

		public override void AI()
		{
			Projectile P = Projectile;
			Player player = Main.player[P.owner];
			float num = (float)Math.PI / 2f;
			Vector2 vector = player.RotatedRelativePoint(player.MountedCenter);
			
			P.localAI[0] += 2f;//1f;
			if (P.localAI[0] >= 60f)
			{
				P.localAI[0] = 0f;
			}
			if (Vector2.Distance(vector, P.Center) >= 5f)
			{
				float num9 = P.localAI[0] / 60f;
				if (num9 > 0.5f)
				{
					num9 = 1f - num9;
				}
				Vector3 value4 = new Vector3(0f, 1f, 0.7f);
				Vector3 value5 = new Vector3(0f, 0.7f, 1f);
				Vector3 value6 = Vector3.Lerp(value4, value5, 1f - num9 * 2f) * 0.5f;
				if (Vector2.Distance(vector, P.Center) >= 30f)
				{
					Vector2 value7 = P.Center - vector;
					value7.Normalize();
					value7 *= Vector2.Distance(vector, P.Center) - 30f;
					DelegateMethods.v3_1 = value6 * 0.8f;
					Utils.PlotTileLine(P.Center - value7, P.Center, 8f, DelegateMethods.CastLightOpen);
				}
				Lighting.AddLight((int)P.Center.X / 16, (int)P.Center.Y / 16, value6.X, value6.Y, value6.Z);
			}
			if (Main.myPlayer == P.owner)
			{
				if (P.localAI[1] > 0f)
				{
					P.localAI[1] -= 1f;
				}
				if (!player.channel || player.noItems || player.CCed)
				{
					P.Kill();
				}
				else if (P.localAI[1] == 0f)
				{
					Vector2 value8 = vector;
					Vector2 vector2 = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY) - value8;
					if (player.gravDir == -1f)
					{
						vector2.Y = (float)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - value8.Y;
					}
					if (Main.tile[Player.tileTargetX, Player.tileTargetY].HasTile)
					{
						vector2 = new Vector2(Player.tileTargetX, Player.tileTargetY) * 16f + Vector2.One * 8f - value8;
						P.localAI[1] = 2f;
					}
					vector2 = Vector2.Lerp(vector2, P.velocity, 0.7f);
					if (float.IsNaN(vector2.X) || float.IsNaN(vector2.Y))
					{
						vector2 = -Vector2.UnitY;
					}
					float num10 = 30f;
					if (vector2.Length() < num10)
					{
						vector2 = Vector2.Normalize(vector2) * num10;
					}
					int tileBoost = player.inventory[player.selectedItem].tileBoost;
					int num11 = -Player.tileRangeX - tileBoost + 1;
					int num12 = Player.tileRangeX + tileBoost - 1;
					int num13 = -Player.tileRangeY - tileBoost;
					int num14 = Player.tileRangeY + tileBoost - 1;
					int num15 = 12;
					bool flag2 = false;
					if (vector2.X < (float)(num11 * 16 - num15))
					{
						flag2 = true;
					}
					if (vector2.Y < (float)(num13 * 16 - num15))
					{
						flag2 = true;
					}
					if (vector2.X > (float)(num12 * 16 + num15))
					{
						flag2 = true;
					}
					if (vector2.Y > (float)(num14 * 16 + num15))
					{
						flag2 = true;
					}
					if (flag2)
					{
						Vector2 value9 = Vector2.Normalize(vector2);
						float num16 = -1f;
						if (value9.X < 0f && ((float)(num11 * 16 - num15) / value9.X < num16 || num16 == -1f))
						{
							num16 = (float)(num11 * 16 - num15) / value9.X;
						}
						if (value9.X > 0f && ((float)(num12 * 16 + num15) / value9.X < num16 || num16 == -1f))
						{
							num16 = (float)(num12 * 16 + num15) / value9.X;
						}
						if (value9.Y < 0f && ((float)(num13 * 16 - num15) / value9.Y < num16 || num16 == -1f))
						{
							num16 = (float)(num13 * 16 - num15) / value9.Y;
						}
						if (value9.Y > 0f && ((float)(num14 * 16 + num15) / value9.Y < num16 || num16 == -1f))
						{
							num16 = (float)(num14 * 16 + num15) / value9.Y;
						}
						vector2 = value9 * num16;
					}
					if (vector2.X != P.velocity.X || vector2.Y != P.velocity.Y)
					{
						P.netUpdate = true;
					}
					P.velocity = vector2;
				}
			}
			
			P.position = player.RotatedRelativePoint(player.MountedCenter) - P.Size / 2f;
			P.rotation = P.velocity.ToRotation() + num;
			P.spriteDirection = P.direction;
			P.timeLeft = 2;
			player.ChangeDir(P.direction);
			player.heldProj = P.whoAmI;
			player.itemTime = 2;
			player.itemAnimation = 2;
			player.itemRotation = (float)Math.Atan2(P.velocity.Y * (float)P.direction, P.velocity.X * (float)P.direction);
			/*for (int num54 = 0; num54 < 2; num54++)
			{
				Dust obj = Main.dust[Dust.NewDust(P.position + P.velocity * 2f, P.width, P.height, 6, 0f, 0f, 100, Color.Transparent, 2f)];
				obj.noGravity = true;
				obj.velocity *= 2f;
				obj.velocity += P.localAI[0].ToRotationVector2();
				obj.fadeIn = 1.5f;
			}
			float num55 = 18f;
			for (int num56 = 0; (float)num56 < num55; num56++)
			{
				if (Main.rand.Next(4) == 0)
				{
					Vector2 position = P.position + P.velocity + P.velocity * ((float)num56 / num55);
					Dust obj2 = Main.dust[Dust.NewDust(P.position, P.width, P.height, 6, 0f, 0f, 100, Color.Transparent)];
					obj2.noGravity = true;
					obj2.fadeIn = 0.5f;
					obj2.velocity += P.localAI[0].ToRotationVector2();
					obj2.noLight = true;
				}
			}*/
		}
		
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(255, 255, 255, 128) * (1f - (float)Projectile.alpha / 255f);
		}
		
		public override bool PreDraw(ref Color lightColor)
		{
			Projectile P = Projectile;
			Player player = Main.player[P.owner];
			Vector2 mountedCenter = player.MountedCenter;
			Color color25 = Lighting.GetColor((int)((double)P.position.X + (double)P.width * 0.5) / 16, (int)(((double)P.position.Y + (double)P.height * 0.5) / 16.0));
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (Projectile.spriteDirection == -1)
			{
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
			
			Vector2 vector42 = P.position + new Vector2(P.width, P.height) / 2f + Vector2.UnitY * P.gfxOffY - Main.screenPosition;
			Texture2D texture2D36 = Terraria.GameContent.TextureAssets.Projectile[P.type].Value;//Main.projectileTexture[P.type];
			Color alpha7 = P.GetAlpha(color25);
			Vector2 vector43 = player.RotatedRelativePoint(mountedCenter) + Vector2.UnitY * player.gfxOffY;
			Vector2 vector44 = vector42 + Main.screenPosition - vector43;
			Vector2 value49 = Vector2.Normalize(vector44);
			float num274 = vector44.Length();
			float num275 = vector44.ToRotation() + (float)Math.PI / 2f;
			float num276 = -5f;
			float num277 = num276 + 30f;
			new Vector2(2f, num274 - num277);
			Vector2 value50 = Vector2.Lerp(vector42 + Main.screenPosition, vector43 + value49 * num277, 0.5f);
			Vector2 vector45 = -Vector2.UnitY.RotatedBy(P.localAI[0] / 60f * (float)Math.PI);
			Vector2[] array7 = new Vector2[4]
			{
				vector45,
				vector45.RotatedBy(1.5707963705062866),
				vector45.RotatedBy(3.1415927410125732),
				vector45.RotatedBy(4.71238899230957)
			};
			if (num274 > num277)
			{
				for (int num278 = 0; num278 < 4; num278++)
				{
					Color white3 = Color.LimeGreen;
					white3.A = 128;
					//white3 *= 0.5f;
					
					float mult = Math.Min(num274/24f + 4f, 10f);
					Vector2 value51 = new Vector2(array7[num278].X, 0f).RotatedBy(num275) * mult;//4f;
					Vector2 start = vector43 + value49 * num277;
					Vector2 midpos = start + value51 + value49 * num274 / 8f;
					Vector2 end = vector42 + Main.screenPosition;
					float rot = (start - end).ToRotation() - (float)Math.PI / 2f;
					float rot2 = (start - midpos).ToRotation() + (float)Math.PI / 2f;
					float rot3 = (midpos - end).ToRotation() + (float)Math.PI / 2f;
					
					if(num278 == 2)
					{
						Main.spriteBatch.Draw(texture2D36, end - Main.screenPosition, new Rectangle(0,0,texture2D36.Width,(int)(num274-num277)/2), alpha7, rot, new Vector2(texture2D36.Width, texture2D36.Height) / 2f, new Vector2(0.75f,2f), spriteEffects, 0f);
					}
					
					int dist = (int)Math.Ceiling(Vector2.Distance(start,midpos));
					Vector2[] pos = new Vector2[dist];
					for(int i = 0; i < dist-1; i++)
					{
						float t = (float)Math.PI/2 * (float)i/dist;
						float shift = mult * (float)Math.Sin(t);
						
						pos[i] = start + value49 * i;
						pos[i].X += (float)Math.Cos(rot) * shift * array7[num278].X;
						pos[i].Y += (float)Math.Sin(rot) * shift * array7[num278].X;
						
						rot2 = (start - pos[i]).ToRotation() - (float)Math.PI / 2f;
						if(i > 0)
						{
							rot2 = (pos[i-1] - pos[i]).ToRotation() - (float)Math.PI / 2f;
						}
						
						Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.MagicPixel.Value, pos[i] - Main.screenPosition, new Rectangle(0,0,1,1), white3, rot2, Vector2.One / 2f, new Vector2(2f,2f), spriteEffects, 0);
					}
					
					Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.MagicPixel.Value, midpos - Main.screenPosition, new Rectangle(0,0,1,(int)Vector2.Distance(midpos,end)), white3, rot3, Vector2.One / 2f, new Vector2(2f,1f), spriteEffects, 0);
				}
			}
			
			float num280 = P.localAI[0] / 60f;
			
			Texture2D texture2D37 = ModContent.Request<Texture2D>("Assets/Projectiles/NovaLaserDrill_Lead").Value;
			Main.spriteBatch.Draw(texture2D37, vector43 - Main.screenPosition + value49 * 30f, null, alpha7, num280 * (float)Math.PI*2, new Vector2((float)texture2D37.Width / 2f, (float)texture2D37.Height / 2f), 1.5f, spriteEffects, 0f);
			
			Item item = player.inventory[player.selectedItem];
			MGlobalItem mi = item.GetGlobalItem<MGlobalItem>();
			Texture2D texture2D38 = Terraria.GameContent.TextureAssets.Item[item.type].Value;//Main.itemTexture[item.type];
			if(mi.itemTexture != null)
			{
				texture2D38 = mi.itemTexture;
			}
			Color color61 = Lighting.GetColor((int)vector43.X / 16, (int)vector43.Y / 16);
			Main.spriteBatch.Draw(texture2D38, vector43 - Main.screenPosition + value49 * 20f, null, color61, P.rotation + (float)Math.PI / 2f + ((spriteEffects == SpriteEffects.None) ? ((float)Math.PI) : 0f), new Vector2((float)texture2D38.Width / 2f, (float)texture2D38.Height / 2f), player.inventory[player.selectedItem].scale, spriteEffects, 0f);
			return false;
		}
	}
}
