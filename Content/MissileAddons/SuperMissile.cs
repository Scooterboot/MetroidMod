using MetroidMod.Content.Projectiles;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace MetroidMod.Content.MissileAddons
{
	internal class SuperMissile : ModMissileAddon
	{
		public override bool AddOnlyAddonItem => false;
		public override Color PrimaryColor => MetroidMod.powColor;
		public override Color SecondaryColor => MetroidMod.powSecondaryColor;
		public override int ShotDust => 6;
		public override string ShotSound => $"{Mod.Name}/Assets/Sounds/MissileAddons/SuperMissile/Shot";
		public override string ImpactSound => $"{Mod.Name}/Assets/Sounds/MissileAddons/SuperMissile/Impact";
		public override void SetStaticDefaults()
		{
			AddonSlot = MissileAddonSlotID.Primary;

			//All the stats are set outside of here up in Stat Values, lets me do fancy schmancy tooltip stuff
			base.SetStaticDefaults();
		}
		public override void SetItemDefaults(Item item)
		{
			item.value = 50000;
			item.rare = ItemRarityID.LightRed;
			base.SetItemDefaults(item);
		}
		public override void AI(MProjectile mProjectile)
		{
			float scale = 2f;
			Projectile Projectile = mProjectile.Projectile;
			mProjectile.DustLine(Projectile.Center - (Projectile.velocity * 0.5f), Projectile.velocity, Projectile.rotation, 5, 3, ShotDust, scale);

			Projectile.ai[0] += 1f;
			if (Projectile.ai[0] > (5f + Projectile.extraUpdates) && Projectile.extraUpdates < 10)
			{
				Projectile.extraUpdates++;
				Projectile.ai[0] = 0f;
			}
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddRecipeGroup(MetroidMod.T2HMBarRecipeGroupID, 8)
				.AddIngredient(ItemID.SoulofNight, 5)
				//.AddIngredient<Tiles.MissileExpansion>(1)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
