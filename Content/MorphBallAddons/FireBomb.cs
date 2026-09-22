using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.MorphBallAddons
{
	public class FireBomb : ModMorphBallBomb, IGeneratesOnStatues
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/MBAddons/FireBomb/FireBombItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/FireBomb/FireBombTile";

		public override string BombProjectileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/FireBomb/FireBombProjectile";

		public int ChanceToGenerateOnStatue(int x, int y, int statueType, bool chozoRoom)
		{
			return NPC.downedBoss2 || Common.Configs.MConfigMain.Instance.drunkWorldHasDrunkStatues || Main.LocalPlayer.ZoneUnderworldHeight
				? 1
				: 0
				;
		}
		public override void ItemSetDefaults()
		{
			Item.damage = 32;
			Item.value = Item.buyPrice(0, 0, 75, 0);
			Item.rare = ItemRarityID.Orange;
		}

		public override void BombKill(ref int dustType, ref int dustType2, ref float dustScale, ref float dustScale2)
		{
			dustType = DustID.Torch;
			dustType2 = DustID.Torch;
			dustScale2 = 3f;
		}
		public override void BombOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(BuffID.OnFire, 600);
		}
		public override void ItemAddRecipes()
		{
			GeneratedModItem.CreateRecipe(1)
				//.AddRecipeGroup(MBAddonLoader.BombsRecipeGroupID, 1)
				.AddIngredient(ItemID.HellstoneBar, 5)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
