using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MetroidMod.Common.Systems;
using System.Collections.Generic;

namespace MetroidMod.Content.Tiles
{
	public class NorfairBrickNatural : ModTile
	{
		public override string Texture => $"{nameof(MetroidMod)}/Content/Tiles/NorfairBrick";
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;

			DustType = 87;
			MinPick = 100;
			HitSound = SoundID.Tink;

			AddMapEntry(new Color(168, 104, 87));
		}

		public override IEnumerable<Item> GetItemDrops(int i, int j)
		{
			yield return new Item(ModContent.ItemType<Items.Tiles.NorfairBrick>());
		}

		public override bool CanExplode(int i, int j) => MSystem.bossesDown.HasFlag(MetroidBossDown.downedKraid);

		public override bool CanKillTile(int i, int j, ref bool blockDamaged) => MSystem.bossesDown.HasFlag(MetroidBossDown.downedKraid) || WorldGen.generatingWorld;

		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			if (j > Main.UnderworldLayer)
			{
				Main.tile[i, j].Get<LiquidData>().LiquidType = LiquidID.Lava;
				Main.tile[i, j].Get<LiquidData>().Amount = 128;
			}
			base.KillTile(i, j, ref fail, ref effectOnly, ref noItem);
		}
	}
}
