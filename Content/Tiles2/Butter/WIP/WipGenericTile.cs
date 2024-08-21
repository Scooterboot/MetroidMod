using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles2.Butter.WIP
{
	/// <summary>
	/// Ideally this class should be gone and each of the new tiles should be given its own specific properties
	/// But since I've been told the goal right now is to get them all in ASAP, this will automate adding all those
	/// tiles that don't have an item or any specific attributes to them yet.
	/// </summary>
	internal class WipGenericTile(string name) : GenericTile
	{
		public override string Name => name;
		public override string ItemTexture => $"{nameof(MetroidMod)}/Content/Tiles2/Butter/WIP/WipGenericTileItem";
		public override Color MapColor => Color.Red;
		public override SoundStyle HitSound => SoundID.Tink;
		public override int DustType => DustID.Firework_Red;
	}

	public class WipGenericTiles : ILoadable
	{
		public void Load(Mod mod)
		{
			string[] tileNames = [
				"Bluestar",
				"CrateriaGrass",
				"CrateriaSoil",
				"NorfairStone",
				"DeathlySpike",
				"DeepNorfair",
				"Greenstar",
				"IdkSob",
				"IdkSob2",
				"Pinkstar",
				"SapphireMetal",
				"Sizzling",
				"Spicy",
				"Wrecked",
			];

			foreach(string tileName in tileNames)
			{
				mod.AddContent(new WipGenericTile(tileName));
			}
		}

		public void Unload()
		{
		}
	}
}
