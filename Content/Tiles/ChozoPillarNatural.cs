using System.Collections.Generic;
using MetroidMod.Common.Systems;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles
{
	public class ChozoPillarNatural : ModTile
	{
		public override string Texture => $"{nameof(MetroidMod)}/Content/Tiles/ChozoPillar";
		public override void SetStaticDefaults()
		{
			TileID.Sets.IsBeam[Type] = true;
			TileID.Sets.DrawsWalls[Type] = true;
			Main.tileDungeon[Type] = true;

			DustType = 87;
			MinPick = 65;
			HitSound = SoundID.Tink;

			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("Chozite Pillar");
			AddMapEntry(new Color(200, 160, 72), name);
		}

		public override IEnumerable<Item> GetItemDrops(int i, int j)
		{
			yield return new Item(ModContent.ItemType<Items.Tiles.ChozoPillar>());
		}

		public override bool CanExplode(int i, int j) => MSystem.bossesDown.HasFlag(MetroidBossDown.downedTorizo);

		public override bool CanKillTile(int i, int j, ref bool blockDamaged) => MSystem.bossesDown.HasFlag(MetroidBossDown.downedTorizo) || WorldGen.generatingWorld;
	}
}
