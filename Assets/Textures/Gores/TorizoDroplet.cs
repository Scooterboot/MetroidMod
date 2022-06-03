using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidModPorted.Assets.Textures.Gores
{
	public class TorizoDroplet : ModGore
	{
		public override void OnSpawn(Gore gore, IEntitySource source)
		{
			gore.numFrames = 8;//15;
			gore.behindTiles = true;
			gore.timeLeft = Gore.goreTime * 3;
		}

		public override bool Update(Gore gore)
		{
			int frameDuration = 4;
			gore.frameCounter++;
			if (gore.frame <= 2)
			{
				frameDuration = 6;
				gore.velocity.Y += 0.2f;
				if (gore.velocity.Y < 0.5f)
				{
					gore.velocity.Y = 0.5f;
				}
				if (gore.velocity.Y > 12f)
				{
					gore.velocity.Y = 12f;
				}
				if ((int)gore.frameCounter >= frameDuration)
				{
					gore.frameCounter = 0;
					gore.frame += 1;
				}
				if (gore.frame > 2)
				{
					gore.frame = 0;
				}
			}
			else
			{
				gore.velocity.Y += 0.1f;
				if ((int)gore.frameCounter >= frameDuration)
				{
					gore.frameCounter = 0;
					gore.frame += 1;
				}
				gore.velocity *= 0f;
				if (gore.frame > 7)
				{
					gore.active = false;
				}
			}

			Vector2 oldVelocity = gore.velocity;
			gore.velocity = Collision.TileCollision(gore.position, gore.velocity, 16, 14, false, false, 1);
			if (gore.velocity != oldVelocity)
			{
				if (gore.frame < 3)
				{
					gore.frame = 3;
					gore.frameCounter = 0;
					SoundEngine.PlaySound(Sounds.NPCs.TorizoDropletDrip1, gore.position + new Vector2(8, 8));
				}
			}
			else if (Collision.WetCollision(gore.position + gore.velocity, 16, 14))
			{
				if (gore.frame < 3)
				{
					gore.frame = 3;
					gore.frameCounter = 0;
					SoundEngine.PlaySound(Sounds.NPCs.TorizoDropletDrip2, gore.position + new Vector2(8, 8));
				}
				int tileX = (int)(gore.position.X + 8f) / 16;
				int tileY = (int)(gore.position.Y + 14f) / 16;
				if (Main.tile[tileX, tileY] != null && Main.tile[tileX, tileY].LiquidType > 0)
				{
					gore.velocity *= 0f;
					gore.position.Y = tileY * 16 - (int)(Main.tile[tileX, tileY].LiquidAmount / 16);
				}
			}

			gore.position += gore.velocity;
			return false;
		}
	}
}
