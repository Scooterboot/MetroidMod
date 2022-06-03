using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.MorphBallAddons
{
	public class CrystalBomb : ModMBWeapon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/MBAddons/CrystalBomb/CrystalBombItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/CrystalBomb/CrystalBombTile";

		public override string ProjectileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/CrystalBomb/CrystalBombProjectile";

		public override bool AddOnlyAddonItem => false;

		public override bool CanGenerateOnChozoStatue(Tile tile) => true;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crystal Morph Ball Bombs");
			ModProjectile.DisplayName.SetDefault("Crystal Morph Ball Bomb");
			Tooltip.SetDefault("-Right click to set off a bomb\n" +
			"Fires off Crystal shards on detonation");
			ItemNameLiteral = true;
		}
		public override void SetItemDefaults(Item item)
		{
			item.damage = 52;
			item.value = Item.buyPrice(0, 3, 0, 0);
			item.rare = ItemRarityID.LightPurple;
		}

		public override void Kill(int timeLeft, ref int dustType, ref int dustType2, ref float dustScale, ref float dustScale2)
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
				float rot = (float)Angle.ConvertToRadians(angle + ((360f / max) * i));
				Vector2 vel = rot.ToRotationVector2() * 10f;
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, vel, ProjectileID.CrystalShard, Projectile.damage / 2, 1, Projectile.owner);
			}
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddRecipeGroup(MBAddonLoader.BombsRecipeGroupID, 1)
				.AddIngredient(ItemID.HallowedBar, 5)
				.AddIngredient(ItemID.CrystalShard, 5)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
