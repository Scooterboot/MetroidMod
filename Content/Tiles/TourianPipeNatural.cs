using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MetroidMod.Common.Systems;
using System.Collections.Generic;

namespace MetroidMod.Content.Tiles
{
	public class TourianPipeNatural : ModTile
	{
		public override string Texture => $"{nameof(MetroidMod)}/Content/Tiles/TourianPipeAccent";
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileDungeon[Type] = true;

			DustType = 87;
			MinPick = 205;
			HitSound = SoundID.Tink;

			AddMapEntry(new Color(149, 149, 174));
		}

		public override IEnumerable<Item> GetItemDrops(int i, int j)
		{
			yield return new Item(ModContent.ItemType<Items.Tiles.TourianPipe>());
		}

		public override bool CanExplode(int i, int j) => NPC.downedMoonlord;//MSystem.bossesDown.HasFlag(MetroidBossDown.MotherBrain);

		public override bool CanKillTile(int i, int j, ref bool blockDamaged) => NPC.downedMoonlord || WorldGen.generatingWorld;//MSystem.bossesDown.HasFlag(MetroidBossDown.MotherBrain) || WorldGen.generatingWorld;
	}
}
