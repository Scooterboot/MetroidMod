using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles
{
    public class MGlobalProjectile : GlobalProjectile
    {
        /*public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            if (projectile.aiStyle != 26 && !projectile.minion && projectile.damage > 0 && projectile.friendly && projectile.active)
            {
                Rectangle tileRect = new Rectangle((int)(projectile.position.X/16) - 1, (int)(projectile.position.Y/16) - 1, (projectile.width/16) + 1, (projectile.height/16) + 1);
                for (int x = tileRect.X; x < tileRect.X + tileRect.Width; x++)
                {
                    for (int y = tileRect.Y; y < tileRect.Y + tileRect.Height; y++)
                    {
                        if (Main.tile[x, y].active())
                        {
                            if (Main.tile[x, y].type == (ushort)mod.TileType("BlueHatch"))
                            {
                                TileLoader.HitWire(x, y, mod.TileType("BlueHatch"));
                            }
                            if (Main.tile[x, y].type == (ushort)mod.TileType("BlueHatchVertical"))
                            {
                                TileLoader.HitWire(x, y, mod.TileType("BlueHatchVertical"));
                            }
                        }
                    }
                }
            }
            return base.OnTileCollide(projectile, oldVelocity);
        }*/
        public override bool PreAI(Projectile projectile)
        {
            if (!projectile.tileCollide && projectile.aiStyle != 26 && !projectile.minion && projectile.damage > 0 && projectile.friendly)
            {
                int x = (int)MathHelper.Clamp(projectile.Center.X / 16,0,Main.maxTilesX);
                int y = (int)MathHelper.Clamp(projectile.Center.Y / 16,0,Main.maxTilesY);
                if (Main.tile[x, y].active())
                {
                    if (Main.tile[x, y].type == (ushort)mod.TileType("BlueHatch"))
                    {
                        TileLoader.HitWire(x, y, mod.TileType("BlueHatch"));
                    }
                    if (Main.tile[x, y].type == (ushort)mod.TileType("BlueHatchVertical"))
                    {
                        TileLoader.HitWire(x, y, mod.TileType("BlueHatchVertical"));
                    }
                }
            }
            return base.PreAI(projectile);
        }
        public override void Kill(Projectile projectile, int timeLeft)
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
                    if (Main.tile[x, y].active())
                    {
                        if (Main.tile[x, y].type == (ushort)mod.TileType("BlueHatch"))
                        {
                            TileLoader.HitWire(x, y, mod.TileType("BlueHatch"));
                        }
                        if (Main.tile[x, y].type == (ushort)mod.TileType("BlueHatchVertical"))
                        {
                            TileLoader.HitWire(x, y, mod.TileType("BlueHatchVertical"));
                        }
                        if (projectile.Name.Contains("Missile"))
                        {
                            if (Main.tile[x, y].type == (ushort)mod.TileType("RedHatch"))
                            {
                                TileLoader.HitWire(x, y, mod.TileType("RedHatch"));
                            }
                            if (Main.tile[x, y].type == (ushort)mod.TileType("RedVertical"))
                            {
                                TileLoader.HitWire(x, y, mod.TileType("RedVertical"));
                            }
                            if (projectile.Name.Contains("Super"))
                            {
                                if (Main.tile[x, y].type == (ushort)mod.TileType("GreenHatch"))
                                {
                                    TileLoader.HitWire(x, y, mod.TileType("GreenHatch"));
                                }
                                if (Main.tile[x, y].type == (ushort)mod.TileType("GreenHatchVertical"))
                                {
                                    TileLoader.HitWire(x, y, mod.TileType("GreenHatchVertical"));
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
