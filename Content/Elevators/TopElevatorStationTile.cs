using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace MetroidMod.Content.Elevators
{
	internal class TopElevatorStationTile : ModTile
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
			TileObjectData.newTile.Height = 1;
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinateHeights = [16, 16];
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.Origin = new(1, 0);
			TileObjectData.addTile(Type);

			AnimationFrameHeight = 18;
		}
	}
}
