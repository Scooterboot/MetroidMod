using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace MetroidMod.Content.Switches
{
	public class ModBubbleSwitchTile(ModBubbleSwitch bubbleSwitch) : ModTile
	{
		public override string Name => bubbleSwitch.Name;
		public override string Texture => $"{nameof(MetroidMod)}/Content/Switches/Variants/{Name}Tile";

		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = false;

			AddMapEntry(bubbleSwitch.MapColor, CreateMapEntryName());

			TileObjectData.newTile.CopyFrom(TileObjectData.StyleTorch);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);

			TileObjectData.newAlternate.CopyFrom(TileObjectData.StyleTorch);
			TileObjectData.newAlternate.AnchorLeft = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, TileObjectData.newAlternate.Height, 0);
			TileObjectData.addAlternate(2);

			TileObjectData.newAlternate.CopyFrom(TileObjectData.StyleTorch);
			TileObjectData.newAlternate.AnchorRight = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, TileObjectData.newAlternate.Height, 0);
			TileObjectData.addAlternate(3);

			TileObjectData.newAlternate.CopyFrom(TileObjectData.StyleTorch);
			TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, TileObjectData.newAlternate.Width, 0);
			TileObjectData.addAlternate(1);

			TileObjectData.addTile(Type);
		}
	}
}
