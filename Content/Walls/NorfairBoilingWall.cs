using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MetroidMod.Content.Walls
{
	public class NorfairBoilingWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			DustType = 87;

			AddMapEntry(new Color(10, 10, 100));
		}
		/// <summary>
		/// Allows you to animate your wall. Use frameCounter to keep track of how long the current frame has been active, and use frame to change the current frame. Walls are drawn every 4 frames.
		/// </summary>
		public override void AnimateWall(ref byte frame, ref byte frameCounter)
		{
			frameCounter++;
			if (frameCounter >= 6)  // Change frame every 6 ticks
			{
				frameCounter = 0;
				frame++;
				if (frame >= 8)  // 8 frames
				{
					frame = 0;
				}
			}
		}

		/// <summary>
		/// Called whenever this wall updates due to being placed or being next to a wall that is changed. Return false to stop the game from carrying out its default WallFrame operations. If you return false, make sure to set <see cref="Tile.WallFrameNumber"/>, <see cref="Tile.WallFrameX"/>, and <see cref="Tile.WallFrameY"/> according to the your desired custom framing design. Returns true by default.
		/// </summary>
		/// <param name="i">The x position in tile coordinates.</param>
		/// <param name="j">The y position in tile coordinates.</param>
		/// <param name="randomizeFrame">True if the calling code intends that the frameNumber be randomly changed, such as when placing the wall initially or loading the world, but not when updating due to nearby tile or wall placements</param>
		/// <param name="style">The style or orientation that will be applied</param>
		/// <param name="frameNumber">The random style that will be applied</param>
		public override bool WallFrame(int i, int j, bool randomizeFrame, ref int style, ref int frameNumber)
		{
			return true;
		}
	}
}



