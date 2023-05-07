using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MetroidMod.Common.Systems;
using MetroidMod.Common.Players;
using MetroidMod.Content.Tiles;
using MetroidMod.Content.Tiles.Hatch;
using MetroidMod.ID;

namespace MetroidMod.Content.MorphBallAddons
{
	public class PowerBomb : ModMBSpecial
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/MBAddons/PowerBomb/PowerBombItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/PowerBomb/PowerBombTile";

		public override string ProjectileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/PowerBomb/PowerBombProjectile";

		public override string ExplosionTexture => $"{Mod.Name}/Assets/Textures/MBAddons/PowerBomb/PowerBombExplosion";

		public override bool AddOnlyAddonItem => false;

		public override bool CanGenerateOnChozoStatue(int x, int y) => WorldGen.drunkWorldGen;

		public override double GenerationChance(int x, int y) => 20;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Power Bomb");
			// ModProjectile.DisplayName.SetDefault("Power Bomb");
			// ModExplosionProjectile.DisplayName.SetDefault("Power Bomb");
			/* Tooltip.SetDefault("-Press the Power Bomb Key to set off a Power Bomb (20 second cooldown)\n" +
			"-Power Bombs create large explosions that vacuum in items afterwards\n" +
			"-Power Bombs ignore 50% of enemy defense and can deal ~1400 damage total"); */
			ItemNameLiteral = true;
		}
		public override void SetItemDefaults(Item item)
		{
			item.damage = 25;
			item.noMelee = true;
			item.value = Item.buyPrice(0, 3, 0, 0);
			item.rare = ItemRarityID.LightRed;
		}
		public override void UpdateEquip(Player player)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.PowerBomb(player, ProjectileType, player.GetWeaponDamage(Item), Item);
		}
		public override void SetExplosionProjectileDefaults(Projectile proj)
		{
			scaleSize = 1f;
			colory = Color.Gold;
		}

		private float scaleSize = 1f;
		private Color colory = Color.Gold;
		private const int width = 1000;
		private const int height = 750;
		private const int maxDistance = 55;
		public override void ExplosionAI()
		{
			float speed = 2f;
			colory = Color.Yellow; ExplosionProjectile.timeLeft = 60;
			ExplosionProjectile.frameCounter++;

			if (ExplosionProjectile.frameCounter < maxDistance)
			{
				scaleSize += speed;

				int num = (int)(50f * ExplosionProjectile.scale);
				for (int i = 0; i < num; i++)
				{
					float angle = (float)((Math.PI * 2) / num) * i;
					Vector2 position = ExplosionProjectile.Center - new Vector2(10, 10);
					position.X += (float)Math.Cos(angle) * ((float)ExplosionProjectile.width / 2f);
					position.Y += (float)Math.Sin(angle) * ((float)ExplosionProjectile.height / 2f);
					int num20 = Dust.NewDust(position, 20, 20, 57, 0f, 0f, 100, default(Color), 3f);
					Dust dust = Main.dust[num20];
					dust.velocity += Vector2.Normalize(ExplosionProjectile.Center - dust.position) * 5f * ExplosionProjectile.scale;
					dust.noGravity = true;
				}
			}
			else
			{
				scaleSize -= speed;
				colory = Color.Black;
				ExplosionProjectile.damage = 0;
				for (int i = 0; i < Main.item.Length; i++)
				{
					if (!Main.item[i].active) continue;

					Item I = Main.item[i];
					if (ExplosionProjectile.Hitbox.Intersects(I.Hitbox))
					{
						Vector2 center = new Vector2(ExplosionProjectile.Center.X, ExplosionProjectile.Center.Y - ((float)I.height / 2f));
						Vector2 velocity = Vector2.Normalize(center - I.Center) * Math.Min(20f, Vector2.Distance(center, I.Center));
						if (Vector2.Distance(center, I.Center) > 1f)
						{
							I.position += velocity;
							I.velocity *= 0f;
						}
					}
				}
			}
			if (ExplosionProjectile.frameCounter >= (maxDistance * 2))
			{
				scaleSize = 1f;
				colory = Color.Gold;
				ExplosionProjectile.frameCounter = 0;
				ExplosionProjectile.Kill();
			}

			ExplosionProjectile.scale = scaleSize * 0.02f;
			ExplosionProjectile.position.X = ExplosionProjectile.position.X + (float)(ExplosionProjectile.width / 2);
			ExplosionProjectile.position.Y = ExplosionProjectile.position.Y + (float)(ExplosionProjectile.height / 2);
			ExplosionProjectile.width = (int)((float)width * ExplosionProjectile.scale);
			ExplosionProjectile.height = (int)((float)height * ExplosionProjectile.scale);
			ExplosionProjectile.position.X = ExplosionProjectile.position.X - (float)(ExplosionProjectile.width / 2);
			ExplosionProjectile.position.Y = ExplosionProjectile.position.Y - (float)(ExplosionProjectile.height / 2);
			ExplosionProjectile.netUpdate = true;

			if (ExplosionProjectile.frameCounter == maxDistance)
			{
				Rectangle tileRect = new Rectangle((int)(ExplosionProjectile.position.X / 16), (int)(ExplosionProjectile.position.Y / 16), (ExplosionProjectile.width / 16), (ExplosionProjectile.height / 16));
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
		public override bool ExplosionPreDraw(ref Color lightColor)
		{
			Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[ExplosionProjectileType].Value;
			Main.spriteBatch.Draw(tex, ExplosionProjectile.Center - Main.screenPosition + new Vector2(0f, ExplosionProjectile.gfxOffY), new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), colory, ExplosionProjectile.rotation, new Vector2(tex.Width / 2, tex.Height / 2), ExplosionProjectile.scale, SpriteEffects.None, 0f);
			return false;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.HallowedBar, 15)
				.AddIngredient(ItemID.SoulofMight, 10)
				.AddIngredient(ItemID.SoulofFright, 10)
				.AddIngredient(ItemID.SoulofSight, 10)
				.Register();
		}
	}
}
