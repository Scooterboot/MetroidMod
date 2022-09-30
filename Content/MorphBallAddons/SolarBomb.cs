using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using MetroidMod.Common.Players;
using MetroidMod.Common.Systems;
using MetroidMod.Content.Tiles;
using MetroidMod.Content.Tiles.Hatch;

namespace MetroidMod.Content.MorphBallAddons
{
	public class SolarBomb : ModMBSpecial
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/MBAddons/SolarBomb/SolarBombItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/SolarBomb/SolarBombTile";

		public override string ProjectileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/SolarBomb/SolarBombProjectile";

		public override string ExplosionTexture => $"{Mod.Name}/Assets/Textures/MBAddons/SolarBomb/SolarBombExplosion";

		public override string ExplosionSound => $"{Mod.Name}/Assets/Sounds/SolarBombExplode";

		public override bool AddOnlyAddonItem => false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Solar Bomb");
			ModProjectile.DisplayName.SetDefault("Solar Bomb");
			ModExplosionProjectile.DisplayName.SetDefault("Solar Bomb");
			Tooltip.SetDefault("-Press the Power Bomb Key to set off a Solar Bomb (20 second cooldown)\n" +
			"-Solar Bombs create massive explosions which burn enemies and vacuum in items afterwards\n" +
			"-Solar Bombs ignore 50% of enemy defense and can deal ~7400 damage total");
			ItemNameLiteral = true;
		}
		public override void SetItemDefaults(Item item)
		{
			item.damage = 25;
			item.noMelee = true;
			item.value = Item.buyPrice(0, 3, 0, 0);
			item.rare = ItemRarityID.LightRed;
		}
		public override void SetExplosionProjectileDefaults(Projectile proj)
		{
			proj.width = 640;
			proj.height = 640;
			proj.scale = 0.01f;
			scale = 0.01f;
			speed = 4f;
			vacAlpha = 0f;
		}
		public override void UpdateEquip(Player player)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.PowerBomb(player, ProjectileType, player.GetWeaponDamage(Item), Item);
		}
		/*public override bool Kill(int timeLeft)
		{
			Terraria.Projectile.NewProjectile(Projectile.Projectile.GetSource_FromThis(), Projectile.Projectile.Center.X, Projectile.Projectile.Center.Y, 0, 0, ExplosionProjectileType, (int)Math.Floor(Projectile.Projectile.damage * DamageMultiplier), Knockback, Projectile.Projectile.owner);
			return false;
		}*/
		private float scale = 0.01f;
		private float speed = 4f;
		private const int maxDist = 74;
		private float vacAlpha = 0f;

		private const int width = 640;
		private const int height = 640;
		public override void ExplosionAI()
		{
			Projectile P = ExplosionProjectile;

			P.ai[0]++;
			if (P.ai[0] > maxDist / 4)
			{
				speed *= 0.955f;
			}
			if (P.ai[0] > maxDist)
			{
				if (vacAlpha < 1f)
				{
					float vacScale = 6f * (1f - vacAlpha);
					int vacW = (int)((float)width * vacScale);
					int vacH = (int)((float)height * vacScale);
					Rectangle vacRect = new Rectangle((int)(P.Center.X - vacW / 2), (int)(P.Center.Y - vacH / 2), vacW, vacH);
					for (int i = 0; i < Main.item.Length; i++)
					{
						if (!Main.item[i].active) continue;

						Item I = Main.item[i];
						if (vacRect.Intersects(I.Hitbox))
						{
							Vector2 center = new Vector2(P.Center.X, P.Center.Y - ((float)I.height / 2f));
							Vector2 velocity = Vector2.Normalize(center - I.Center) * Math.Min(20f, Vector2.Distance(center, I.Center));
							if (Vector2.Distance(center, I.Center) > 1f)
							{
								I.position += velocity;
								I.velocity *= 0f;
							}
						}
					}

					vacAlpha += 1f / maxDist;
				}
				else
				{
					P.damage = 0;
				}
			}
			else
			{
				float dScale = Math.Min(10f * (1f - P.ai[0] / maxDist), 4f);
				int num = (int)(100f * dScale);
				for (int i = 0; i < num; i++)
				{
					int dType = Utils.SelectRandom<int>(Main.rand, new int[]
					{
						6,
						259,
						158
					});

					float angle = (float)((Math.PI * 2) / num) * i;
					Vector2 position = P.Center - new Vector2(20, 20);
					position.X += (float)Math.Cos(angle) * ((float)P.width / 2f - 16f * P.scale);
					position.Y += (float)Math.Sin(angle) * ((float)P.height / 2f - 16f * P.scale);
					int num20 = Dust.NewDust(position, 40, 40, dType, 0f, 0f, 100, default(Color), 1f);
					Dust dust = Main.dust[num20];
					dust.velocity += Vector2.Normalize(P.Center - dust.position) * 2f;// * 5f * P.scale;
					dust.alpha = 200;
					dust.scale += Main.rand.NextFloat();
					dust.noGravity = true;
				}
			}
			scale += 0.04f * speed;

			P.scale = scale;
			P.position.X += (float)(P.width / 2);
			P.position.Y += (float)(P.height / 2);
			P.width = (int)((float)width * P.scale);
			P.height = (int)((float)height * P.scale);
			P.position.X -= (float)(P.width / 2);
			P.position.Y -= (float)(P.height / 2);

			if (P.alpha < 255)
			{
				P.alpha++;
				P.timeLeft = 2;
			}

			if (P.ai[0] == maxDist)
			{
				Rectangle tileRect = new Rectangle((int)(P.position.X / 16), (int)(P.position.Y / 16), (P.width / 16), (P.height / 16));
				for (int x = tileRect.X; x < tileRect.X + tileRect.Width; x++)
				{
					for (int y = tileRect.Y; y < tileRect.Y + tileRect.Height; y++)
					{
						if (x < 0 || y < 0) { continue; }
						if (Main.tile[x, y] != null && Main.tile[x, y].HasTile)
						{
							if (Main.tile[x, y].TileType == (ushort)ModContent.TileType<YellowHatch>())
								TileLoader.HitWire(x, y, ModContent.TileType<YellowHatch>());
							if (Main.tile[x, y].TileType == (ushort)ModContent.TileType<YellowHatchVertical>())
								TileLoader.HitWire(x, y, ModContent.TileType<YellowHatchVertical>());
							if (Main.tile[x, y].TileType == (ushort)ModContent.TileType<BlueHatch>())
								TileLoader.HitWire(x, y, ModContent.TileType<BlueHatch>());
							if (Main.tile[x, y].TileType == (ushort)ModContent.TileType<BlueHatchVertical>())
								TileLoader.HitWire(x, y, ModContent.TileType<BlueHatchVertical>());
							if (Main.tile[x, y].TileType == (ushort)ModContent.TileType<YellowSwitch>())
								Wiring.TripWire(x, y, 1, 1);
						}
						if (MSystem.mBlockType[x, y] != 0)
						{
							MSystem.hit[x, y] = true;
						}
						if (MSystem.mBlockType[x, y] == 3)
						{
							MSystem.AddRegenBlock(x, y);
						}
						if (MSystem.mBlockType[x, y] == 5)
						{
							MSystem.AddRegenBlock(x, y);
						}
						if (MSystem.mBlockType[x, y] == 7)
						{
							MSystem.AddRegenBlock(x, y);
						}
						if (MSystem.mBlockType[x, y] == 10)
						{
							MSystem.AddRegenBlock(x, y);
						}
					}
				}
			}
		}
		public override bool ExplosionPreDraw(ref Color lightColor)
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

			Projectile P = ExplosionProjectile;
			Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[P.type].Value;

			Main.spriteBatch.Draw(tex, P.Center - Main.screenPosition,
			new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)),
			P.GetAlpha(Color.White), P.rotation, new Vector2(tex.Width, tex.Height), P.scale, SpriteEffects.None, 0f);

			Main.spriteBatch.Draw(tex, P.Center - Main.screenPosition,
			new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)),
			P.GetAlpha(Color.White), P.rotation, new Vector2(0, tex.Height), P.scale, SpriteEffects.FlipHorizontally, 0f);

			Main.spriteBatch.Draw(tex, P.Center - Main.screenPosition,
			new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)),
			P.GetAlpha(Color.White), P.rotation, new Vector2(tex.Width, 0), P.scale, SpriteEffects.FlipVertically, 0f);

			Main.spriteBatch.Draw(tex, P.Center - Main.screenPosition,
			new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)),
			P.GetAlpha(Color.White), P.rotation, Vector2.Zero, P.scale, SpriteEffects.FlipHorizontally ^ SpriteEffects.FlipVertically, 0f);

			if (vacAlpha < 1f)
			{
				Texture2D tex2 = ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/MBAddons/SolarBomb/SolarBombVacuum").Value;
				float vacScale = 6f * (1f - vacAlpha);
				Color color = Color.White * (1f - vacAlpha * 0.5f);
				color.A = (byte)(255f * vacAlpha);

				Main.spriteBatch.Draw(tex2, P.Center - Main.screenPosition,
				new Rectangle?(new Rectangle(0, 0, tex2.Width, tex2.Height)),
				color, P.rotation, new Vector2(tex2.Width / 2, tex2.Height / 2), vacScale, SpriteEffects.None, 0f);
			}


			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
			return false;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.FragmentSolar, 18)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}
	}
}
