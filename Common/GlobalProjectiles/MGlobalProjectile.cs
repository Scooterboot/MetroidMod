using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ModLoader;

using MetroidMod.Common.Systems;
using MetroidMod.Content.Projectiles.Paralyzer;

namespace MetroidMod.Common.GlobalProjectiles
{
	public class MGlobalProjectile : GlobalProjectile
	{
		public override bool InstancePerEntity => true;
		bool init = false;
		float projSpeed = 1f;
		public override bool PreAI(Projectile projectile)
		{
			if (!projectile.tileCollide && projectile.aiStyle != 26 && !projectile.minion && projectile.damage > 0 && projectile.friendly)
			{
				int x = (int)MathHelper.Clamp(projectile.Center.X / 16, 0, Main.maxTilesX - 2);
				int y = (int)MathHelper.Clamp(projectile.Center.Y / 16, 0, Main.maxTilesY - 2);
				if (Main.tile[x, y] != null && Main.tile[x, y].HasTile)
				{
					if (Main.tile[x, y].TileType == (ushort)ModContent.TileType<Content.Tiles.Hatch.BlueHatch>())
					{
						TileLoader.HitWire(x, y, ModContent.TileType<Content.Tiles.Hatch.BlueHatch>());
					}
					if (Main.tile[x, y].TileType == (ushort)ModContent.TileType<Content.Tiles.Hatch.BlueHatchVertical>())
					{
						TileLoader.HitWire(x, y, ModContent.TileType<Content.Tiles.Hatch.BlueHatchVertical>());
					}
					if (Main.tile[x, y].TileType == (ushort)ModContent.TileType<Content.Tiles.BlueSwitch>())
					{
						Wiring.TripWire(x, y, 1, 1);
					}
					if (MSystem.mBlockType[x, y] == 5)
					{
						MSystem.AddRegenBlock(x, y);
					}
					if (MSystem.mBlockType[x, y] == 10)
					{
						MSystem.AddRegenBlock(x, y);
					}
				}
			}
			if (!init)
			{
				projSpeed = projectile.velocity.Length();
				init = true;
			}
			return base.PreAI(projectile);
		}
		int counter = 0;
		public override void AI(Projectile projectile)
		{
			float accuracy = 3f;
			float distance = 250f;
			if (projectile.active && projectile.penetrate > 0 && projectile.damage > 0 && projectile.friendly && projectile.velocity != Vector2.Zero && projectile.aiStyle != 3 && projectile.aiStyle != 15 && !Main.projPet[projectile.type])
			{
				for (int i = 0; i < Main.npc.Length; i++)
				{
					NPC npc = Main.npc[i];
					if (npc != null && npc.active && npc.type == ModContent.NPCType<Content.NPCs.OmegaPirate.OmegaPirateAbsorbField>() && npc.ai[3] != 1)
					{
						if (Vector2.Distance(npc.Center, projectile.Center) < distance * npc.ai[1])
						{
							counter++;
						}

						if (counter > 5)
						{
							Vector2 vec = npc.Center - projectile.Center;
							float num2 = vec.Length();
							num2 = projSpeed / num2;
							vec *= num2;
							projectile.velocity.X = (projectile.velocity.X * accuracy + vec.X) / (accuracy + 1f);
							projectile.velocity.Y = (projectile.velocity.Y * accuracy + vec.Y) / (accuracy + 1f);

							if (Vector2.Distance(npc.Center, projectile.Center + projectile.velocity) < 55f)
							{
								projectile.penetrate = 0;
								npc.ai[1] = 1.5f;
								npc.ai[2] += (int)((float)projectile.damage / 10f);
							}
						}
						else
						{
							projSpeed = projectile.velocity.Length();
						}
					}
				}
			}
		}
		public override void OnKill(Projectile projectile, int timeLeft)
		{
			int tilex = (int)(projectile.position.X / 16) - 1;
			int tiley = (int)(projectile.position.Y / 16) - 1;
			int tilex2 = (int)((projectile.position.X + projectile.width) / 16) + 1;
			int tiley2 = (int)((projectile.position.Y + projectile.height) / 16) + 1;
			if (tilex < 0)
			{
				tilex = 0;
			}
			if (tilex2 > Main.maxTilesX)
			{
				tilex2 = Main.maxTilesX;
			}
			if (tiley < 0)
			{
				tiley = 0;
			}
			if (tiley2 > Main.maxTilesY)
			{
				tiley2 = Main.maxTilesY;
			}
			for (int x = tilex; x < tilex2; x++)
			{
				for (int y = tiley; y < tiley2; y++)
				{
					if (Main.tile[x, y] != null && Main.tile[x, y].HasTile)
					{
						if (Main.tile[x, y].TileType == (ushort)ModContent.TileType<Content.Tiles.Hatch.BlueHatch>())
						{
							TileLoader.HitWire(x, y, ModContent.TileType<Content.Tiles.Hatch.BlueHatch>());
						}
						if (Main.tile[x, y].TileType == (ushort)ModContent.TileType<Content.Tiles.Hatch.BlueHatchVertical>())
						{
							TileLoader.HitWire(x, y, ModContent.TileType<Content.Tiles.Hatch.BlueHatchVertical>());
						}
						if (projectile.Name.Contains("Screw Attack"))
						{
							if (MSystem.mBlockType[x, y] == 3)
							{
								MSystem.AddRegenBlock(x, y);
							}
							if (MSystem.mBlockType[x, y] == 5)
							{
								MSystem.AddRegenBlock(x, y);
							}
							if (MSystem.mBlockType[x, y] == 10)
							{
								MSystem.AddRegenBlock(x, y);
							}
							if (MSystem.mBlockType[x, y] == 9)
							{
								MSystem.AddRegenBlock(x, y);
							}
						}
						else if (!projectile.Name.Contains("Charge Attack"))
						{
							if (Main.tile[x, y].TileType == (ushort)ModContent.TileType<Content.Tiles.BlueSwitch>())
							{
								Wiring.TripWire(x, y, 1, 1);
							}
							if (MSystem.mBlockType[x, y] != 0)
							{
								MSystem.hit[x, y] = true;
							}
							if (MSystem.mBlockType[x, y] == 5)
							{
								MSystem.AddRegenBlock(x, y);
							}
							if (MSystem.mBlockType[x, y] == 10)
							{
								MSystem.AddRegenBlock(x, y);
							}
							if (projectile.Name.Contains("Bomb"))
							{
								if (MSystem.mBlockType[x, y] == 3)
								{
									MSystem.AddRegenBlock(x, y);
								}
								if (MSystem.mBlockType[x, y] == 12)
								{
									MSystem.AddRegenBlock(x, y);
								}
							}
							if (projectile.type == ModContent.ProjectileType<ParalyzerShot>())
							{
								if (MSystem.mBlockType[x, y] == 3 || MSystem.mBlockType[x, y] == 12)
								{
									MSystem.AddRegenBlock(x, y);
								}
							}
						}
						if (projectile.Name.Contains("Missile"))
						{
							if (Main.tile[x, y].TileType == (ushort)ModContent.TileType<Content.Tiles.Hatch.RedHatch>())
							{
								TileLoader.HitWire(x, y, ModContent.TileType<Content.Tiles.Hatch.RedHatch>());
							}
							if (Main.tile[x, y].TileType == (ushort)ModContent.TileType<Content.Tiles.Hatch.RedHatchVertical>())
							{
								TileLoader.HitWire(x, y, ModContent.TileType<Content.Tiles.Hatch.RedHatchVertical>());
							}
							if (MSystem.mBlockType[x, y] == 4)
							{
								MSystem.AddRegenBlock(x, y);
							}
							if (Main.tile[x, y].TileType == (ushort)ModContent.TileType<Content.Tiles.RedSwitch>())
							{
								Wiring.TripWire(x, y, 1, 1);
							}
							if (projectile.Name.Contains("Super") || projectile.Name.Contains("Nebula") || projectile.Name.Contains("Stardust"))
							{
								if (Main.tile[x, y].TileType == (ushort)ModContent.TileType<Content.Tiles.Hatch.GreenHatch>())
								{
									TileLoader.HitWire(x, y, ModContent.TileType<Content.Tiles.Hatch.GreenHatch>());
								}
								if (Main.tile[x, y].TileType == (ushort)ModContent.TileType<Content.Tiles.Hatch.GreenHatchVertical>())
								{
									TileLoader.HitWire(x, y, ModContent.TileType<Content.Tiles.Hatch.GreenHatchVertical>());
								}
								if (Main.tile[x, y].TileType == (ushort)ModContent.TileType<Content.Tiles.GreenSwitch>())
								{
									Wiring.TripWire(x, y, 1, 1);
								}
								if (MSystem.mBlockType[x, y] == 8)
								{
									MSystem.AddRegenBlock(x, y);
								}
							}
						}
					}
				}
			}
		}
	}
}
