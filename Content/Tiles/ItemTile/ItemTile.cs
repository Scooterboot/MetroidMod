using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace MetroidMod.Content.Tiles.ItemTile
{
	public abstract class ItemTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.LavaDeath = false;

			AnchorType anchors = AnchorType.EmptyTile | AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.Table | AnchorType.SolidSide;
			TileObjectData.newTile.AnchorBottom = new AnchorData(anchors, TileObjectData.newTile.Width, 0);
			//but y tho? its not unusual for pickups to be floating in metroid -- DR
			
			TileObjectData.addTile(Type);
			TileID.Sets.DisableSmartCursor[Type] = true;
			Main.tileLavaDeath[Type] = false;
			Main.tileObsidianKill[Type] = false;
		}

		public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = TileLoader.GetItemDropFromTypeAndStyle(Type);
		}

		public override bool RightClick(int i, int j)
		{
			WorldGen.KillTile(i, j, false, false, false);
			if (Main.netMode == NetmodeID.MultiplayerClient && !Main.tile[i, j].HasTile)
			{
				NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, (float)i, (float)j, 0f, 0, 0, 0);
			}
			return (true);
		}
	}
}
