using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace MetroidModPorted
{
	public struct CollideMethods
	{
		public static bool CheckCollide(Vector2 Position, int Width, int Height)
		{
			int num = (int)(Position.X / 16f) - 1;
			int num2 = (int)((Position.X + (float)Width) / 16f) + 2;
			int num3 = (int)(Position.Y / 16f) - 1;
			int num4 = (int)((Position.Y + (float)Height) / 16f) + 2;
			num = Utils.Clamp<int>(num, 0, Main.maxTilesX - 1);
			num2 = Utils.Clamp<int>(num2, 0, Main.maxTilesX - 1);
			num3 = Utils.Clamp<int>(num3, 0, Main.maxTilesY - 1);
			num4 = Utils.Clamp<int>(num4, 0, Main.maxTilesY - 1);
			for (int i = num; i < num2; i++)
			{
				for (int j = num3; j < num4; j++)
				{
					if (Main.tile[i, j] != null && !Main.tile[i, j].IsActuated && Main.tile[i, j].HasTile && Main.tileSolid[(int)Main.tile[i, j].TileType] && !Main.tileSolidTop[(int)Main.tile[i, j].TileType])
					{
						Vector2 vector;
						vector.X = (float)(i * 16);
						vector.Y = (float)(j * 16);
						int num5 = 16;
						if (Main.tile[i, j].IsHalfBlock)
						{
							vector.Y += 8f;
							num5 -= 8;
						}
						if (Position.X + (float)Width > vector.X && Position.X < vector.X + 16f && Position.Y + (float)Height > vector.Y && Position.Y < vector.Y + (float)num5)
						{
							if (Main.tile[i, j].Slope > SlopeType.Solid)
							{
								if (Main.tile[i, j].Slope > SlopeType.SlopeDownRight)
								{
									if (Main.tile[i, j].Slope == SlopeType.SlopeUpLeft && Position.Y < vector.Y + (float)num5 - Math.Max(Position.X - vector.X, 0f))
									{
										return true;
									}
									if (Main.tile[i, j].Slope == SlopeType.SlopeUpRight && Position.Y < vector.Y + (float)num5 - Math.Max((vector.X + 16f) - (Position.X + (float)Width), 0f))
									{
										return true;
									}
								}
								else
								{
									if (Main.tile[i, j].Slope == SlopeType.SlopeDownLeft && Position.Y + (float)Height > vector.Y + Math.Max(Position.X - vector.X, 0f))
									{
										return true;
									}
									if (Main.tile[i, j].Slope == SlopeType.SlopeDownRight && Position.Y + (float)Height > vector.Y + Math.Max((vector.X + 16f) - (Position.X + (float)Width), 0f))
									{
										return true;
									}
								}
							}
							else
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		public static Tile GetTile(Vector2 Position)
		{
			int x = (int)(Position.X / 16f);
			int y = (int)(Position.Y / 16f);
			x = Utils.Clamp<int>(x, 0, Main.maxTilesX - 1);
			y = Utils.Clamp<int>(y, 0, Main.maxTilesY - 1);

			return Main.tile[x, y];
		}

		public static bool SolidCollision(Vector2 pos, Vector2 vel, int Width, int Height, bool checkPlatforms)
		{
			Vector2 Position = pos + vel;
			int num = (int)(Position.X / 16f) - 1;
			int num2 = (int)((Position.X + (float)Width) / 16f) + 2;
			int num3 = (int)(Position.Y / 16f) - 1;
			int num4 = (int)((Position.Y + (float)Height) / 16f) + 2;
			num = Utils.Clamp<int>(num, 0, Main.maxTilesX - 1);
			num2 = Utils.Clamp<int>(num2, 0, Main.maxTilesX - 1);
			num3 = Utils.Clamp<int>(num3, 0, Main.maxTilesY - 1);
			num4 = Utils.Clamp<int>(num4, 0, Main.maxTilesY - 1);
			for (int i = num; i < num2; i++)
			{
				for (int j = num3; j < num4; j++)
				{
					if (Main.tile[i, j] != null && !Main.tile[i, j].IsActuated && Main.tile[i, j].HasTile && Main.tileSolid[(int)Main.tile[i, j].TileType] && (!Main.tileSolidTop[(int)Main.tile[i, j].TileType] || checkPlatforms))
					{
						Vector2 vector;
						vector.X = (float)(i * 16);
						vector.Y = (float)(j * 16);
						int num5 = 16;
						if (Main.tile[i, j].IsHalfBlock)
						{
							vector.Y += 8f;
							num5 -= 8;
						}
						if (!checkPlatforms || !Main.tileSolidTop[(int)Main.tile[i, j].TileType] || pos.Y + (float)Height <= vector.Y)
						{
							if (Position.X + (float)Width > vector.X && Position.X < vector.X + 16f && Position.Y + (float)Height > vector.Y && Position.Y < vector.Y + (float)num5)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}
	}
}
