using MetroidMod.Content.Buffs;
using MetroidMod.Content.Items.Accessories;
using MetroidMod.Content.Projectiles;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.MissileAddons
{
	internal class IceSuper : ModMissileAddon
	{
		public override bool AddOnlyAddonItem => false;

		public override Color PrimaryColor => MetroidMod.iceColor;

		public override Color SecondaryColor => MetroidMod.iceSecondaryColor;
		public override int ShotDust => DustID.IceTorch;
		public override string ShotSound => $"{Mod.Name}/Assets/Sounds/MissileAddons/SuperMissile/Shot";
		public override string ImpactSound => $"{Mod.Name}/Assets/Sounds/MissileAddons/IceMissile/Impact";
		public override void SetStaticDefaults()
		{
			AddonSlot = MissileAddonSlotID.Primary;

			InflictsBuff = ModContent.BuffType<InstantFreeze>();
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
			MissileAddonLoader.GetAddon<SuperMissile>().AI(mProjectile);
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddMissileAddon<SuperMissile>(1)
				.AddMissileAddon<IceMissile>(1)
				.AddIngredient(ItemID.Ectoplasm, 8)
				.AddIngredient(ItemID.BeetleHusk, 3)
				.AddIngredient<FrozenCore>(1)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
