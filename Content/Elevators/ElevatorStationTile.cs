using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using System.Linq;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

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

			AnimationFrameHeight = TileObjectData.newTile.CoordinateFullHeight;
        }

		public override void AnimateTile(ref int frame, ref int frameCounter)
		{
			if (!Animated) return;
			if(++frameCounter >= 8)
			{
				frameCounter = 0;
				frame = (++frame) % FrameAmount;
			}
		}
	}
}
