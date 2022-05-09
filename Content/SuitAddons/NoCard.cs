using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MetroidModPorted.ID;
using MetroidModPorted.Common.Players;

namespace MetroidModPorted.Content.SuitAddons
{
	public class NoCard : ModSuitAddon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/no";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/SpeedBooster/SpeedBoosterTile";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("No Card");
			Tooltip.SetDefault(/*"Allows the user to run insanely fast\n" +
				"Damages enemies while running\n" +
				"Damage scales off of enemy's contact damage\n" +
				"While active, press DOWN to charge a Shine Spark\n" +
				"Then press JUMP to activate the charge"*/"");
			AddonSlot = SuitAddonSlotID.Boots_Speed;
		}
		public override void OnUpdateArmorSet(Player player)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			player.accRunSpeed = (float)Math.Pow(2,16)-1;
			player.runAcceleration = (float)Math.Pow(2,13)-1;
			//mp.speedBooster = true;
			//mp.speedBoostDmg = Math.Max(player.GetWeaponDamage(Item.Item), mp.speedBoostDmg);
		}
	}
}
