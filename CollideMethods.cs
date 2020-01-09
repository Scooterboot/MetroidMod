using System;
using Microsoft.Xna.Framework;
using Terraria;

namespace MetroidMod
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
					if (Main.tile[i, j] != null && !Main.tile[i, j].inActive() && Main.tile[i, j].active() && Main.tileSolid[(int)Main.tile[i, j].type] && !Main.tileSolidTop[(int)Main.tile[i, j].type])
					{
						Vector2 vector;
						vector.X = (float)(i * 16);
						vector.Y = (float)(j * 16);
						int num5 = 16;
						if (Main.tile[i, j].halfBrick())
						{
							vector.Y += 8f;
							num5 -= 8;
						}
						if (Position.X + (float)Width > vector.X && Position.X < vector.X + 16f && Position.Y + (float)Height > vector.Y && Position.Y < vector.Y + (float)num5)
						{
							if(Main.tile[i, j].slope() > 0)
							{
								if (Main.tile[i, j].slope() > 2)
								{
									if(Main.tile[i, j].slope() == 3 && Position.Y < vector.Y + (float)num5 - Math.Max(Position.X - vector.X, 0f))
									{
										return true;
									}
									if(Main.tile[i, j].slope() == 4 && Position.Y < vector.Y + (float)num5 - Math.Max((vector.X + 16f) - (Position.X + (float)Width), 0f))
									{
										return true;
									}
								}
								else
								{
									if(Main.tile[i, j].slope() == 1 && Position.Y + (float)Height > vector.Y + Math.Max(Position.X - vector.X, 0f))
									{
										return true;
									}
									if(Main.tile[i, j].slope() == 2 && Position.Y + (float)Height > vector.Y + Math.Max((vector.X + 16f) - (Position.X + (float)Width), 0f))
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
	}
}