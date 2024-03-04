#region Using directives

using MetroidMod.Common.Systems;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

#endregion

namespace MetroidMod.Content.Tiles
{
	public class PhazonCore : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileLighted[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileMerge[Type][ModContent.TileType<PhazonTile>()] = true;

			DustType = 28;
			MinPick = 1000;//215;
			HitSound = SoundID.Tink;

			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("Phazon Core");
			AddMapEntry(new Color(255, 65, 0), name);
		}

		public override bool CanExplode(int x, int y) => false;

		public override void NumDust(int x, int y, bool fail, ref int num) => num = fail ? 1 : 3;

		public override void ModifyLight(int x, int y, ref float r, ref float g, ref float b)
		{
			r = (255f / 255f);
			g = (105f / 255f);
			b = 0f;
		}

		public override void KillTile(int x, int y, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				return;
			}

			if (!fail && !effectOnly)
			{
				noItem = true;
				for (int i = x - 16; i < x + 16; i++)
				{
					for (int j = y - 16; j < y + 16; j++)
					{
						if (Main.tile[i, j].TileType == ModContent.TileType<PhazonCore>())
						{
							for (int k = 0; k < 10; k++)
							{
								int dustID = Dust.NewDust(new Vector2((x * 16) - 11, (y * 16) - 11), 22, 22, DustID.BlueCrystalShard, Main.rand.Next(10) - 5, Main.rand.Next(10) - 5, 100, Color.White, 7f);
								Main.dust[dustID].noGravity = true;
							}
							Main.tile[i, j].Get<TileWallWireStateData>().HasTile = false;
							Projectile.NewProjectile(new EntitySource_TileBreak(x, y), x * 16, y * 16, Main.rand.Next(10) - 5, Main.rand.Next(10) - 5, ModContent.ProjectileType<Projectiles.PhazonExplosion>(), 20, 0.1f, Main.myPlayer);
						}
						WorldGen.SquareTileFrame(i, j);
					}
				}
				NetMessage.SendTileSquare(-1, x - 16, y - 16, 32, 32, TileChangeType.None);

				for (int i = 0; i < 10; i++)
				{
					int dustID = Dust.NewDust(new Vector2((x * 16) - 11, (y * 16) - 11), 22, 22, DustID.BlueCrystalShard, Main.rand.Next(10) - 5, Main.rand.Next(10) - 5, 100, Color.White, 7f);
					Main.dust[dustID].noGravity = true;
				}

				Projectile.NewProjectile(new EntitySource_TileBreak(x, y), x * 16, y * 16, Main.rand.Next(10) - 5, Main.rand.Next(10) - 5, ModContent.ProjectileType<Projectiles.PhazonExplosion>(), 0, 0.1f, Main.myPlayer);

				if (Main.netMode != NetmodeID.Server)
				{
					SoundEngine.PlaySound(SoundID.Item14, new(x * 16, y * 16));
					Main.NewText(Language.GetTextValue($"Mods.{nameof(MetroidMod)}.WorldEvents.PhazonCorruption", $"{Main.worldName}"), new Color(50, 125, 255));
				}
				else
				{
					ChatHelper.BroadcastChatMessage(NetworkText.FromKey($"Mods.{nameof(MetroidMod)}.WorldEvents.PhazonCorruption", $"{Main.worldName}"), new Color(50, 125, 255));
				}

				int Amount_Of_Spawns = 100 + (int)(Main.maxTilesY * 0.2f);
				for (int i = 0; i < Amount_Of_Spawns; i++)
				{
					if (MSystem.PhazonSpawn != true)
					{
						Common.Systems.MSystem.AddPhazon();
						MSystem.PhazonSpawn = true;
					}
				}
			}
		}
	}
}
