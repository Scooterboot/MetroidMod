using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.DataStructures;

namespace MetroidMod.Tiles
{
	public class ScrewAttackTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileSpelunker[Type] = true;
			Main.tileNoAttach[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.Table | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Screw Attack");
			AddMapEntry(new Color(252, 244, 100), name);
			drop = mod.ItemType("ScrewAttack");
			dustType = 1;
			disableSmartCursor = true;
		}
		public override void RightClick(int i, int j)
		{
		    WorldGen.KillTile(i, j, false, false, false);
		    if (Main.netMode == 1 && !Main.tile[i, j].active())
		    {
			NetMessage.SendData(17, -1, -1, null, 4, (float)i, (float)j, 0f, 0, 0, 0);
		    }
		}
public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.showItemIcon = true;
			player.showItemIcon2 = mod.ItemType("ScrewAttack");
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}

	}
}
