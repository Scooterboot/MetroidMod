using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.MorphBallAddons
{
	public class ShadowflameBomb : Bomb, IGeneratesOnStatues
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/MBAddons/ShadowflameBomb/ShadowflameBombItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/ShadowflameBomb/ShadowflameBombTile";

		public override string BombProjectileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/ShadowflameBomb/ShadowflameBombProjectile";

		public int ChanceToGenerateOnStatue(int x, int y, int statueType, bool chozoRoom)
		{
			return Common.Configs.MConfigMain.Instance.drunkWorldHasDrunkStatues
				? 1
				: 0
				;
		}

		public override void ItemSetDefaults()
		{
			Item.damage = 59;
			Item.value = Item.buyPrice(0, 2, 0, 0);
			Item.rare = ItemRarityID.Pink;
		}

		public override void BombKill(ref int dustType, ref int dustType2, ref float dustScale, ref float dustScale2)
		{
			dustType = DustID.PurpleTorch;
			dustType2 = DustID.Shadowflame;
			dustScale2 = 3f;
		}
		public override void BombOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(BuffID.ShadowFlame, 600);
		}
	}
}
