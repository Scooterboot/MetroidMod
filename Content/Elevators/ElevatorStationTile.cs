using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace MetroidMod.Content.Elevators
{
	internal class ElevatorStationTile : ModTile
	{
		public virtual int Height => 2;
		public virtual bool Animated => true;
		public virtual int FrameAmount => 3;
		public virtual Point16 Origin => new(1, 1);

		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileLighted[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = false;
			Main.tileFrameImportant[Type] = true;

			DustType = DustID.Stone;
			MinPick = 100;
			AddMapEntry(Color.Yellow);

			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.StyleHorizontal = false;
			TileObjectData.newTile.Width = 4;
			TileObjectData.newTile.Height = Height;
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinateHeights = Enumerable.Repeat(16, Height).ToArray();
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.Origin = Origin;
			TileObjectData.addTile(Type);

			AnimationFrameHeight = Height * 18;
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			Color color = Color.Yellow;
			float intensity = 0.75f / 255f;
			r = color.R * intensity;
			g = color.G * intensity;
			b = color.B * intensity;
		}

		public override void AnimateTile(ref int frame, ref int frameCounter)
		{
			if (!Animated) return;
			if (++frameCounter >= 8)
			{
				frameCounter = 0;
				frame = (++frame) % FrameAmount;
			}
		}
	}
}
