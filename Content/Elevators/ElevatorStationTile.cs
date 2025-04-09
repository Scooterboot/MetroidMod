using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework.Graphics;

namespace MetroidMod.Content.Elevators
{
	internal class ElevatorStationTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = false;
            Main.tileFrameImportant[Type] = true;

            DustType = DustID.Stone;
            AddMapEntry(new(200, 200, 200));

            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.StyleHorizontal = false;
            TileObjectData.newTile.Width = 4;
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinateHeights = [16, 16];
            TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.Origin = new(1, 1);
            TileObjectData.addTile(Type);

			AnimationFrameHeight = 36;
        }

		public override void AnimateTile(ref int frame, ref int frameCounter)
		{
			if(++frameCounter >= 8)
			{
				frameCounter = 0;
				frame = (++frame) % 3;
			}
		}
	}
}
