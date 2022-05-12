using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using Terraria;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MetroidModPorted.Common.Systems;
using MetroidModPorted.Content.Tiles;
using MetroidModPorted.Content.Tiles.Hatch;
using MetroidModPorted.ID;

namespace MetroidModPorted.Default
{
	[Autoload(false)]
	internal class MBSpecialExplosion : ModProjectile
	{
		public ModMBSpecial modMBAddon;
		public MBSpecialExplosion(ModMBSpecial modMBAddon)
		{
			this.modMBAddon = modMBAddon;
		}
		public override string Texture => modMBAddon.ExplosionTexture;
		public override string Name => $"{modMBAddon.Name}Explosion";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(modMBAddon.DisplayName.GetDefault());
		}
		public override void SetDefaults()
		{
			modMBAddon.ExplosionProjectileType = Type;
			Projectile.width = 1000;
			Projectile.height = 750;
			Projectile.scale = 0.02f;
			Projectile.localNPCHitCooldown = 1;
			Projectile.timeLeft = 200;
			Projectile.penetrate = -1;
			modMBAddon.SetExplosionProjectileDefaults(Projectile);
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.usesLocalNPCImmunity = true;
		}

		private float scaleSize = 1f;
		private Color colory = Color.Gold;
		private const int width = 1000;
		private const int height = 750;
		private const int maxDistance = 55;
		public override void AI()
		{
			float speed = 2f;
			colory = Color.Yellow;
			if (modMBAddon.ExplosionAI(ref scaleSize, ref speed, ref colory))
			{
				Projectile.timeLeft = 60;
				Projectile.frameCounter++;

				if (Projectile.frameCounter < maxDistance)
				{
					scaleSize += speed;

					int num = (int)(50f * Projectile.scale);
					for (int i = 0; i < num; i++)
					{
						float angle = (float)((Math.PI * 2) / num) * i;
						Vector2 position = Projectile.Center - new Vector2(10, 10);
						position.X += (float)Math.Cos(angle) * ((float)Projectile.width / 2f);
						position.Y += (float)Math.Sin(angle) * ((float)Projectile.height / 2f);
						int num20 = Dust.NewDust(position, 20, 20, 57, 0f, 0f, 100, default(Color), 3f);
						Dust dust = Main.dust[num20];
						dust.velocity += Vector2.Normalize(Projectile.Center - dust.position) * 5f * Projectile.scale;
						dust.noGravity = true;
					}
				}
				else
				{
					scaleSize -= speed;
					colory = Color.Black;
					Projectile.damage = 0;
					for (int i = 0; i < Main.item.Length; i++)
					{
						if (!Main.item[i].active) continue;

						Item I = Main.item[i];
						if (Projectile.Hitbox.Intersects(I.Hitbox))
						{
							Vector2 center = new Vector2(Projectile.Center.X, Projectile.Center.Y - ((float)I.height / 2f));
							Vector2 velocity = Vector2.Normalize(center - I.Center) * Math.Min(20f, Vector2.Distance(center, I.Center));
							if (Vector2.Distance(center, I.Center) > 1f)
							{
								I.position += velocity;
								I.velocity *= 0f;
							}
						}
					}
				}
				if (Projectile.frameCounter >= (maxDistance * 2))
				{
					scaleSize = 1f;
					colory = Color.Gold;
					Projectile.frameCounter = 0;
					Projectile.Kill();
				}

				Projectile.scale = scaleSize * 0.02f;
				Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
				Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
				Projectile.width = (int)((float)width * Projectile.scale);
				Projectile.height = (int)((float)height * Projectile.scale);
				Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
				Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
				Projectile.netUpdate = true;

				if (Projectile.frameCounter == maxDistance)
				{
					Rectangle tileRect = new Rectangle((int)(Projectile.position.X / 16), (int)(Projectile.position.Y / 16), (Projectile.width / 16), (Projectile.height / 16));
					for (int x = tileRect.X; x < tileRect.X + tileRect.Width; x++)
					{
						for (int y = tileRect.Y; y < tileRect.Y + tileRect.Height; y++)
						{
							if (Main.tile[x, y] != null && Main.tile[x, y].HasTile)
							{
								if (Main.tile[x, y].TileType == ModContent.TileType<YellowHatch>()) { TileLoader.HitWire(x, y, ModContent.TileType<YellowHatch>()); }
								if (Main.tile[x, y].TileType == ModContent.TileType<YellowHatchVertical>()) { TileLoader.HitWire(x, y, ModContent.TileType<YellowHatchVertical>()); }
								if (Main.tile[x, y].TileType == ModContent.TileType<BlueHatch>()) { TileLoader.HitWire(x, y, ModContent.TileType<BlueHatch>()); }
								if (Main.tile[x, y].TileType == ModContent.TileType<BlueHatchVertical>()) { TileLoader.HitWire(x, y, ModContent.TileType<BlueHatchVertical>()); }
								if (Main.tile[x, y].TileType == ModContent.TileType<YellowSwitch>()) { Wiring.TripWire(x, y, 1, 1); }
							}
							if (MSystem.mBlockType[x, y] != BreakableTileID.None)
							{
								MSystem.hit[x, y] = true;
							}
							if (MSystem.mBlockType[x, y] == BreakableTileID.Bomb)
							{
								MSystem.AddRegenBlock(x, y);
							}
							if (MSystem.mBlockType[x, y] == BreakableTileID.Fake)
							{
								MSystem.AddRegenBlock(x, y);
							}
							if (MSystem.mBlockType[x, y] == BreakableTileID.PowerBomb)
							{
								MSystem.AddRegenBlock(x, y);
							}
							if (MSystem.mBlockType[x, y] == BreakableTileID.FakeHint)
							{
								MSystem.AddRegenBlock(x, y);
							}
						}
					}
				}
			}
		}

		public override void ModifyHitNPC(NPC npc, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if (npc.defense < 1000) { damage = (int)(damage + npc.defense * 0.5); }
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			modMBAddon.OnHitNPC(target, damage, knockback, crit);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			bool temp = modMBAddon.ExplosionPreDraw(ref lightColor);
			if (temp)
			{
				Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
				Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), colory, Projectile.rotation, new Vector2(tex.Width / 2, tex.Height / 2), Projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
	}
}
