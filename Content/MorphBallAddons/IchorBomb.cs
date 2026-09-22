using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.MorphBallAddons
{
	public class IchorBomb : ModMorphBallBomb, IGeneratesOnStatues
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/MBAddons/IchorBomb/IchorBombItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/IchorBomb/IchorBombTile";

		public override string BombProjectileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/IchorBomb/IchorBombProjectile";

		public int ChanceToGenerateOnStatue(int x, int y, int statueType, bool chozoRoom)
		{
			return Common.Configs.MConfigMain.Instance.drunkWorldHasDrunkStatues || Main.hardMode
				? 1
				: 0
				;
		}

		public override void ItemSetDefaults()
		{
			Item.damage = 40;
			Item.value = Item.buyPrice(0, 1, 50, 0);
			Item.rare = ItemRarityID.LightRed;
		}

		public override void BombKill(ref int dustType, ref int dustType2, ref float dustScale, ref float dustScale2)
		{
			dustType = DustID.IchorTorch;
			dustType2 = 170;
			dustScale = 4f;
			dustScale2 = 2f;
		}

		public override void BombOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(BuffID.Ichor, 600);
		}

		public override void ItemAddRecipes()
		{
			GeneratedModItem.CreateRecipe(1)
				//.AddRecipeGroup(MBAddonLoader.BombsRecipeGroupID, 1)
				.AddRecipeGroup(MetroidMod.T2HMBarRecipeGroupID, 5)
				.AddIngredient(ItemID.Ichor, 5)
				.AddTile(TileID.MythrilAnvil)
				//.AddDecraftCondition(Condition.Hardmode)
				.Register();
		}
	}
}
