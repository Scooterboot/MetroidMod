using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles
{
	public class NESCrateriaBlock : ModTile
	{
		public override string Texture => $"{Mod.Name}/Content/Tiles/PhazonTile";
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
		}
	}
}
