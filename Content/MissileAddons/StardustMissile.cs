using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace MetroidMod.Content.MissileAddons
{
	internal class StardustMissile : ModMissileAddon
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

			//All the stats are set outside of here up in Stat Values, lets me do fancy schmancy tooltip stuff
			base.SetStaticDefaults();
		}
		public override void SetItemDefaults(Item item)
		{
			item.value = 30000;
			item.rare = ItemRarityID.LightRed;
			base.SetItemDefaults(item);
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.FragmentStardust, 18)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}
	}
}
