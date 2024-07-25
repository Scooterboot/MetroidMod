using System;
using MetroidMod.Common.Players;
using MetroidMod.Common.Systems;
using MetroidMod.Content.Tiles;
using MetroidMod.Content.Tiles.Hatch;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.MorphBallAddons
{
	public class VortexBomb : ModMBSpecial
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/MBAddons/VortexBomb/VortexBombItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/VortexBomb/VortexBombTile";

		public override string ProjectileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/VortexBomb/VortexBombProjectile";

		public override string ExplosionTexture => $"{Mod.Name}/Assets/Textures/MBAddons/VortexBomb/VortexBombExplosion";

		public override bool AddOnlyAddonItem => false;

		public override bool CanGenerateOnChozoStatue() => Common.Configs.MConfigMain.Instance.drunkWorldHasDrunkStatues || NPC.downedAncientCultist;

		public override double GenerationChance() => 1;
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Vortex Bomb");
			// ModProjectile.DisplayName.SetDefault("Vortex Bomb");
			// ModExplosionProjectile.DisplayName.SetDefault("Vortex Bomb");
			/* Tooltip.SetDefault("-Press the Power Bomb Key to set off a Vortex Bomb (20 second cooldown)\n" +
			"-Vortex Bombs create massive explosions which vacuum in foes and items\n" +
			"-Vortex Bombs can deal ~1400 damage total"); */
			ItemNameLiteral = true;
		}
		public override void SetItemDefaults(Item item)
		{
			item.damage = 20;
			item.noMelee = true;
			item.value = Item.buyPrice(0, 3, 0, 0);
			item.rare = ItemRarityID.LightRed;
		}
		public override void SetExplosionProjectileDefaults(Projectile proj)
		{
			proj.width = 640;
			proj.height = 640;
			proj.scale = 0.01f;
			proj.extraUpdates = 3;
			proj.localNPCHitCooldown = 1 * (1 + proj.extraUpdates);
			scale = 0.01f;
			speed = 4f;
			itemVacSpeed = 5f;
			npcVacSpeed = 5f;
		}
		private float scale = 0.01f;
		private float speed = 4f;
		private const int maxDist = 60;

		private float itemVacSpeed = 5f;
		private float npcVacSpeed = 5f;

		private const int width = 640;
		private const int height = 640;
		public override void ExplosionAI()
		{
			Projectile P = ExplosionProjectile;

			float vacSpeedIncr = 0.05f;

			P.ai[0] += 1f / (1 + P.extraUpdates);
			if (P.ai[0] < maxDist)
			{
				if (P.numUpdates == 0)
				{
					speed *= 0.99f;
				}
				scale += 0.04f * speed / (1 + P.extraUpdates);
			}
			else
			{
				if (P.numUpdates == 0)
				{
					speed *= 1.01f;
				}
				scale -= 0.04f * speed / (1 + P.extraUpdates);
				vacSpeedIncr = 0.5f;
			}

			if (P.numUpdates == 0)
			{
				itemVacSpeed = Math.Min(itemVacSpeed + vacSpeedIncr, 20f);
				npcVacSpeed = Math.Min(npcVacSpeed + vacSpeedIncr, 40f);

				for (int i = 0; i < Main.item.Length; i++)
				{
					if (!Main.item[i].active) continue;

					Item I = Main.item[i];
					if (P.Hitbox.Intersects(I.Hitbox))
					{
						Vector2 center = new Vector2(P.Center.X, P.Center.Y - ((float)I.height / 2f));
						Vector2 velocity = Vector2.Normalize(center - I.Center) * Math.Min(itemVacSpeed, Vector2.Distance(center, I.Center));
						if (Vector2.Distance(center, I.Center) > 1f)
						{
							I.position += velocity;
							I.velocity *= 0f;
						}
					}
				}

				foreach (NPC who in Main.ActiveNPCs)
				{
					NPC npc = Main.npc[who.whoAmI];
					if (npc.CanBeChasedBy(P, false) && npc.knockBackResist != 0f)
					{
						if (P.Hitbox.Intersects(npc.Hitbox))
						{
							Vector2 center = new Vector2(P.Center.X, P.Center.Y - ((float)npc.height / 2f));
							Vector2 velocity = Vector2.Normalize(center - npc.Center) * Math.Min(npcVacSpeed * npc.knockBackResist, Vector2.Distance(center, npc.Center));
							if (Vector2.Distance(center, npc.Center) > 1f)
							{
								npc.position += velocity;
								npc.velocity *= 0f;
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
			for (int i = 0; i < num; i++)
			{
				float angle = (float)((Math.PI * 2) / num) * i;
				angle += ((float)Math.PI / 20f) * P.ai[0];
				Vector2 position = P.Center;
				position.X += (float)Math.Cos(angle) * ((float)P.width / 2f);
				position.Y += (float)Math.Sin(angle) * ((float)P.height / 2f);
				int num20 = Dust.NewDust(position, 1, 1, 229, 0f, 0f, 100, default(Color), MathHelper.Clamp(P.scale, 1f, 3f));
				Dust dust = Main.dust[num20];
				dust.position = position;
				dust.velocity = Vector2.Normalize(P.Center - dust.position) * 10f;
				dust.noGravity = true;
			}

			if (scale > 0f)
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
						if (x < 0 || y < 0) { continue; }
						if (Main.tile[x, y].HasTile)
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
			Projectile P = ExplosionProjectile;
			Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[P.type].Value;
			if (P.scale > 0f)
			{
				Main.spriteBatch.Draw(tex, P.Center - Main.screenPosition + new Vector2(0f, P.gfxOffY), new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), Color.White, P.rotation, new Vector2(tex.Width / 2, tex.Height / 2), P.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override void UpdateEquip(Player player)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.PowerBomb(player, ProjectileType, player.GetWeaponDamage(Item), Item);
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.FragmentVortex, 18)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}
	}
}
