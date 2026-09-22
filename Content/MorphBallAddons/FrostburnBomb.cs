using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.MorphBallAddons
{
	public class FrostburnBomb : Bomb, IGeneratesOnStatues
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/MBAddons/FrostburnBomb/FrostburnBombItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/FrostburnBomb/FrostburnBombTile";

		public override string BombProjectileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/FrostburnBomb/FrostburnBombProjectile";

		public int ChanceToGenerateOnStatue(int x, int y, int statueType, bool chozoRoom)
		{
			return Common.Configs.MConfigMain.Instance.drunkWorldHasDrunkStatues || Main.hardMode
				? 1
				: 0
				;
		}

		public override void ItemSetDefaults()
		{
			Item.damage = 43;
			Item.value = Item.buyPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Orange;
		}

		public override void BombKill(ref int dustType, ref int dustType2, ref float dustScale, ref float dustScale2)
		{
			dustType = DustID.IceTorch;
			dustType2 = DustID.IceTorch;
			dustScale2 = 3f;
		}
		public override void BombOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(BuffID.Frostburn, 600);
		}
		public override void ItemAddRecipes()
		{
			GeneratedModItem.CreateRecipe(1)
				//.AddRecipeGroup(MBAddonLoader.BombsRecipeGroupID, 1)
				.AddRecipeGroup(MetroidMod.T1HMBarRecipeGroupID, 5)
				.AddIngredient(ItemID.FrostCore, 1)
				.AddTile(TileID.Anvils)
				//.AddDecraftCondition(Condition.Hardmode)
				.Register();
		}
	}
}
