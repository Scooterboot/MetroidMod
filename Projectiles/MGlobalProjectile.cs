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

using MetroidMod.Common.Worlds;

namespace MetroidMod.Projectiles
{
    public class MGlobalProjectile : GlobalProjectile
    {
		public override bool InstancePerEntity
		{
			get
			{
				return true;
			}
		}
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
		bool init = false;
		float projSpeed = 1f;
        public override bool PreAI(Projectile projectile)
        {
            if (!projectile.tileCollide && projectile.aiStyle != 26 && !projectile.minion && projectile.damage > 0 && projectile.friendly)
            {
                int x = (int)MathHelper.Clamp(projectile.Center.X / 16,0,Main.maxTilesX-2);
                int y = (int)MathHelper.Clamp(projectile.Center.Y / 16,0,Main.maxTilesY-2);
                if (Main.tile[x, y] != null && Main.tile[x, y].active())
                {
                    if (Main.tile[x, y].type == (ushort)mod.TileType("BlueHatch"))
                    {
                        TileLoader.HitWire(x, y, mod.TileType("BlueHatch"));
                    }
                    if (Main.tile[x, y].type == (ushort)mod.TileType("BlueHatchVertical"))
                    {
                        TileLoader.HitWire(x, y, mod.TileType("BlueHatchVertical"));
                    }
                    if (Main.tile[x, y].type == (ushort)mod.TileType("BlueSwitch"))
                    {
                        Wiring.TripWire(x, y, 1, 1);
                    }
                    if (MWorld.mBlockType[x, y] == 5)
                    {
                        MWorld.AddRegenBlock(x, y);
                    }
                    if (MWorld.mBlockType[x, y] == 10)
                    {
                        MWorld.AddRegenBlock(x, y);
                    }
                }
            }
			if(!init)
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
			if(projectile.active && projectile.penetrate > 0 && projectile.damage > 0 && projectile.friendly && projectile.velocity != Vector2.Zero && projectile.aiStyle != 3 && projectile.aiStyle != 15 && !Main.projPet[projectile.type])
			{
				for(int i = 0; i < Main.npc.Length; i++)
				{
					NPC npc = Main.npc[i];
					if(npc != null && npc.active && npc.type == mod.NPCType("OmegaPirateAbsorbField") && npc.ai[3] != 1)
					{
						if(Vector2.Distance(npc.Center,projectile.Center) < distance*npc.ai[1])
						{
							counter++;
						}
						
						if(counter > 5)
						{
							Vector2 vec = npc.Center - projectile.Center;
							float num2 = vec.Length();
							num2 = projSpeed / num2;
							vec *= num2;
							projectile.velocity.X = (projectile.velocity.X * accuracy + vec.X) / (accuracy + 1f);
							projectile.velocity.Y = (projectile.velocity.Y * accuracy + vec.Y) / (accuracy + 1f);
							
							if(Vector2.Distance(npc.Center,projectile.Center+projectile.velocity) < 55f)
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
                    if (Main.tile[x, y] != null && Main.tile[x, y].active())
                    {
                        if (Main.tile[x, y].type == (ushort)mod.TileType("BlueHatch"))
                        {
                            TileLoader.HitWire(x, y, mod.TileType("BlueHatch"));
                        }
                        if (Main.tile[x, y].type == (ushort)mod.TileType("BlueHatchVertical"))
                        {
                            TileLoader.HitWire(x, y, mod.TileType("BlueHatchVertical"));
                        }
                        if (projectile.Name.Contains("Screw Attack"))
                        {
                            if (MWorld.mBlockType[x, y] == 3)
                            {
                                MWorld.AddRegenBlock(x, y);
                            }
                            if (MWorld.mBlockType[x, y] == 5)
                            {
                                MWorld.AddRegenBlock(x, y);
                            }
                            if (MWorld.mBlockType[x, y] == 10)
                            {
                                MWorld.AddRegenBlock(x, y);
                            }
                            if (MWorld.mBlockType[x, y] == 9)
                            {
                                MWorld.AddRegenBlock(x, y);
                            }
                        }
                        else if(!projectile.Name.Contains("Charge Attack"))
                        {
                            if (Main.tile[x, y].type == (ushort)mod.TileType("BlueSwitch"))
                            {
                                Wiring.TripWire(x, y, 1, 1);
                            }
                            if (MWorld.mBlockType[x, y] != 0)
                            {
                                MWorld.hit[x, y] = true;
                            }
                            if (MWorld.mBlockType[x, y] == 5)
                            {
                                MWorld.AddRegenBlock(x, y);
                            }
                            if (MWorld.mBlockType[x, y] == 10)
                            {
                                MWorld.AddRegenBlock(x, y);
                            }
                            if (projectile.Name.Contains("Bomb"))
                            {
                                if (MWorld.mBlockType[x, y] == 3)
                                {
                                    MWorld.AddRegenBlock(x, y);
                                }
                                if (MWorld.mBlockType[x, y] == 12)
                                {
                                    MWorld.AddRegenBlock(x, y);
                                }
                            }
                        }
                        if (projectile.Name.Contains("Missile"))
                        {
                            if (Main.tile[x, y].type == (ushort)mod.TileType("RedHatch"))
                            {
                                TileLoader.HitWire(x, y, mod.TileType("RedHatch"));
                            }
                            if (Main.tile[x, y].type == (ushort)mod.TileType("RedHatchVertical"))
                            {
                                TileLoader.HitWire(x, y, mod.TileType("RedHatchVertical"));
                            }
                            if (MWorld.mBlockType[x, y] == 4)
                            {
                                MWorld.AddRegenBlock(x, y);
                            }
                            if (Main.tile[x, y].type == (ushort)mod.TileType("RedSwitch"))
                            {
                                Wiring.TripWire(x, y, 1, 1);
                            }
                            if (projectile.Name.Contains("Super") || projectile.Name.Contains("Nebula") || projectile.Name.Contains("Stardust"))
                            {
                                if (Main.tile[x, y].type == (ushort)mod.TileType("GreenHatch"))
                                {
                                    TileLoader.HitWire(x, y, mod.TileType("GreenHatch"));
                                }
                                if (Main.tile[x, y].type == (ushort)mod.TileType("GreenHatchVertical"))
                                {
                                    TileLoader.HitWire(x, y, mod.TileType("GreenHatchVertical"));
                                }
                                if (Main.tile[x, y].type == (ushort)mod.TileType("GreenSwitch"))
                                {
                                    Wiring.TripWire(x, y, 1, 1);
                                }
                                if (MWorld.mBlockType[x, y] == 8)
                                {
                                    MWorld.AddRegenBlock(x, y);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
