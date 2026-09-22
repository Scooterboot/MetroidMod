using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.MorphBallAddons
{
	public class BetsyBomb : ModMorphBallBomb, IGeneratesOnStatues
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/MBAddons/BetsyBomb/BetsyBombItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/BetsyBomb/BetsyBombTile";

		public override string BombProjectileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/BetsyBomb/BetsyBombProjectile";

		public int ChanceToGenerateOnStatue(int x, int y, int statueType, bool chozoRoom)
		{
			return Common.Configs.MConfigMain.Instance.drunkWorldHasDrunkStatues
				? 1
				: 0
				;
		}

		public override void ItemSetDefaults()
		{
			Item.damage = 120;
			Item.value = Item.buyPrice(0, 6, 0, 0);
			Item.rare = ItemRarityID.Cyan;
		}

		public override void BombKill(ref int dustType, ref int dustType2, ref float dustScale, ref float dustScale2)
		{
			dustType = 55;
			dustType2 = DustID.OrangeTorch;
			dustScale = 3f;
			dustScale2 = 3f;
		}
		public override void BombOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(BuffID.BetsysCurse, 600);
			target.AddBuff(BuffID.Oiled, 600);
			target.AddBuff(BuffID.OnFire, 600);
		}
	}
}
