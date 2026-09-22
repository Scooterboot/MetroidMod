using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.MorphBallAddons
{
	public class CrystalBomb : ModMorphBallBomb, IGeneratesOnStatues
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/MBAddons/CrystalBomb/CrystalBombItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/CrystalBomb/CrystalBombTile";

		public override string BombProjectileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/CrystalBomb/CrystalBombProjectile";


		public override void ItemSetDefaults()
		{
			Item.damage = 69;
			Item.value = Item.buyPrice(0, 3, 0, 0);
			Item.rare = ItemRarityID.LightPurple;
		}

		public override void BombKill(ref int dustType, ref int dustType2, ref float dustScale, ref float dustScale2)
		{
			dustType = 70;
			dustScale = 3f;
			dustType2 = 70;
			dustScale2 = 2f;

			int max = 9;
			float angle = Main.rand.Next(360 / max);
			for (int i = 0; i < max; i++)
			{
				//Vector2 vel = Main.rand.NextVector2CircularEdge(5f, 5f);
				float rot = (float)Angle.ConvertToRadians(angle + (360f / max * i));
				Vector2 vel = rot.ToRotationVector2() * 10f;
				Projectile.NewProjectile(BombProjectile.Projectile.GetSource_FromThis(), BombProjectile.Projectile.Center, vel, ProjectileID.CrystalShard, BombProjectile.Projectile.damage / 2, 1, BombProjectile.Projectile.owner);
			}
		}
		public override void ItemAddRecipes()
		{
			GeneratedModItem.CreateRecipe(1)
				//.AddRecipeGroup(MBAddonLoader.BombsRecipeGroupID, 1)
				.AddIngredient(ItemID.HallowedBar, 5)
				.AddIngredient(ItemID.CrystalShard, 5)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}

		public int ChanceToGenerateOnStatue(int x, int y, int statueType, bool chozoRoom)
		{
			return Common.Configs.MConfigMain.Instance.drunkWorldHasDrunkStatues || (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
				? 1
				: 0
				;
		}
	}
}
