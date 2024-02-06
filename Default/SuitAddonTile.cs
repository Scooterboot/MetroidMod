using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.DataStructures;

namespace MetroidMod.Default
{
	[Autoload(false)]
	internal class SuitAddonTile : ModTile
	{
		public ModSuitAddon modSuitAddon;

		public override string Texture => modSuitAddon.TileTexture;

		public override string Name => modSuitAddon.Name + "Tile";

		public SuitAddonTile(ModSuitAddon modSuitAddon)
		{
			this.modSuitAddon = modSuitAddon;
		}

		public override void SetStaticDefaults()
		{
			modSuitAddon.TileType = Type;
			//ItemDrop= modSuitAddon.ItemType;
			Main.tileFrameImportant[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileSpelunker[Type] = true;
			Main.tileNoAttach[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.Table | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.addTile(Type);
			TileID.Sets.DisableSmartCursor[Type] = true;
		}

		public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
		public override bool Slope(int i, int j) => false;
		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = modSuitAddon.ItemType;
		}
		public override bool RightClick(int i, int j)
		{
			WorldGen.KillTile(i, j, false, false, false);
			if (Main.netMode == NetmodeID.MultiplayerClient && !Main.tile[i, j].HasTile)
			{
				NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, (float)i, (float)j, 0f, 0, 0, 0);
			}
			return true;
		}
	}
}
