using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.MorphBallAddons
{
	public class VenomBomb : ModMorphBallBomb, IGeneratesOnStatues
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/MBAddons/VenomBomb/VenomBombItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/VenomBomb/VenomBombTile";

		public override string BombProjectileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/VenomBomb/VenomBombProjectile";

		public int ChanceToGenerateOnStatue(int x, int y, int statueType, bool chozoRoom)
		{
			return Common.Configs.MConfigMain.Instance.drunkWorldHasDrunkStatues || (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
				? 1
				: 0
				;
		}

		public override void ItemSetDefaults()
		{
			Item.damage = 85;
			Item.value = Item.buyPrice(0, 4, 0, 0);
			Item.rare = ItemRarityID.Lime;
		}

		public override void BombKill(ref int dustType, ref int dustType2, ref float dustScale, ref float dustScale2)
		{
			dustType = 171;
			dustType2 = 205;
			dustScale = 2.5f;
			dustScale2 = 2.5f;
		}
		public override void BombOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(BuffID.Venom, 600);
		}
		public override void ItemAddRecipes()
		{
			GeneratedModItem.CreateRecipe(1)
				//.AddRecipeGroup(MBAddonLoader.BombsRecipeGroupID, 1)
				.AddIngredient(ItemID.ChlorophyteBar, 5)
				.AddIngredient(ItemID.VialofVenom, 5)
				.AddTile(TileID.MythrilAnvil)
				//.AddDecraftCondition(Condition.DownedGolem)
				.Register();
		}
	}
}
