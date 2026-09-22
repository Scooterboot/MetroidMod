using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.MorphBallAddons
{
	public class CursedFlameBomb : Bomb, IGeneratesOnStatues
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/MBAddons/CursedFlameBomb/CursedFlameBombItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/CursedFlameBomb/CursedFlameBombTile";

		public override string BombProjectileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/CursedFlameBomb/CursedFlameBombProjectile";

		public int ChanceToGenerateOnStatue(int x, int y, int statueType, bool chozoRoom)
		{
			return Common.Configs.MConfigMain.Instance.drunkWorldHasDrunkStatues || Main.hardMode
				? 1
				: 0
				;
		}

		public override void ItemSetDefaults()
		{
			Item.damage = 51;
			Item.value = Item.buyPrice(0, 1, 50, 0);
			Item.rare = ItemRarityID.LightRed;
		}

		public override void BombKill(ref int dustType, ref int dustType2, ref float dustScale, ref float dustScale2)
		{
			dustType = DustID.CursedTorch;
			dustType2 = DustID.CursedTorch;
			dustScale2 = 3f;
		}

		public override void BombOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(BuffID.CursedInferno, 600);
		}

		public override void ItemAddRecipes()
		{
			GeneratedModItem.CreateRecipe(1)
				//.AddRecipeGroup(MBAddonLoader.BombsRecipeGroupID, 1)
				.AddRecipeGroup(MetroidMod.T2HMBarRecipeGroupID, 5)
				.AddIngredient(ItemID.CursedFlame, 5)
				.AddTile(TileID.MythrilAnvil)
				//.AddDecraftCondition(Condition.Hardmode)
				.Register();
		}
	}
}
