using System.Collections.Generic;
using MetroidMod.Common.Systems;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles
{
	public class ChozoBrickNatural : ModTile
	{
		public override string Texture => $"{nameof(MetroidMod)}/Content/Tiles/ChozoBrick";
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			//Main.tileDungeon[Type] = true;
			//Main.tileMerge[Type][TileID.Sand] = true;
			//Main.tileMerge[TileID.Sand][Type] = true;

			DustType = 87;
			MinPick = 65;
			HitSound = SoundID.Tink;

			AddMapEntry(new Color(200, 160, 72));
		}

		public override IEnumerable<Item> GetItemDrops(int i, int j)
		{
			yield return new Item(ModContent.ItemType<Items.Tiles.ChozoBrick>());
		}

		public override bool CanExplode(int i, int j) => MSystem.bossesDown.HasFlag(MetroidBossDown.downedTorizo);

		public override bool CanKillTile(int i, int j, ref bool blockDamaged) => MSystem.bossesDown.HasFlag(MetroidBossDown.downedTorizo) || WorldGen.generatingWorld;
		public override bool CanReplace(int i, int j, int tileTypeBeingPlaced) => MSystem.bossesDown.HasFlag(MetroidBossDown.downedTorizo) || WorldGen.generatingWorld;
	}
}
