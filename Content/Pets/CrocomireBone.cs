using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Pets
{
	public class CrocomireBone : ModItem
	{
		// Names and descriptions of all ExamplePetX classes are defined using .hjson files in the Localization folder
		public override void SetDefaults() {
			Item.CloneDefaults(ItemID.ZephyrFish); // Copy the Defaults of the Zephyr Fish Item.

			Item.shoot = ModContent.ProjectileType<CrocomirePet>(); // "Shoot" your pet projectile.
			Item.buffType = ModContent.BuffType<CrocomirePetBuff>(); // Apply buff upon usage of the Item.
		}

        public override bool? UseItem(Player player)
        {
			if (player.whoAmI == Main.myPlayer) {
				player.AddBuff(Item.buffType, 3600);
			}
   			return true;
		}

	}
}
