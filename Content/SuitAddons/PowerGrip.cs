//using System.Collections.Generic; //	This is for ModifyTooltips.
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MetroidMod.ID;
using MetroidMod.Common.Players;

namespace MetroidMod.Content.SuitAddons
{
	public class PowerGrip : ModSuitAddon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/PowerGrip/PowerGripItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/PowerGrip/PowerGripTile";

		public override bool AddOnlyAddonItem => false;

		//public override bool CanGenerateOnChozoStatue(int x, int y) => true;

		//public override double GenerationChance(int x, int y) => 20;

		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Power Grip");
			//Tooltip.SetDefault("Allows the user to grab onto ledges\nDoes not need to be equipped; works while in inventory");
			AddonSlot = SuitAddonSlotID.Misc_Grip;
		}
		//	This will be made possible later on.
		/*public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine LedgeClimb = new TooltipLine(Mod, "LedgeClimb", $"Allows the user to grab onto ledges");
			TooltipLine WallJump = new TooltipLine(Mod, "WallJump", $"Allows the user to wall-jump");
			if (Common.Configs.MConfigItems.Instance.enableLedgeClimbPowerGrip)
			{
				tooltips.Add(LedgeClimb);
			}
			if (Common.Configs.MConfigItems.Instance.enableWallJumpPowerGrip)
			{
				tooltips.Add(WallJump);
			}
		}*/
		public override void SetItemDefaults(Item item)
		{
			item.width = 11;
			item.height = 15;
			item.value = Item.buyPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Green;
			item.accessory = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<Items.Miscellaneous.ChoziteBar>(4)
				.AddIngredient(ItemID.Sapphire, 1)
				.AddTile(TileID.Anvils)
				.Register();
			CreateRecipe(1)
				.AddIngredient<Items.Accessories.PowerGrip>(1)
				.Register();
		}
		public override void OnUpdateArmorSet(Player player, int stack)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			if (Common.Configs.MConfigItems.Instance.enableLedgeClimbPowerGrip)
			{
				mp.powerGrip = true;
			}
			if (Common.Configs.MConfigItems.Instance.enableWallJumpPowerGrip)
			{
				mp.EnableWallJump = true;
			}
		}
		public override void UpdateInventory(Player player)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			if (Common.Configs.MConfigItems.Instance.enableLedgeClimbPowerGrip)
			{
				mp.powerGrip = true;
			}
			if (Common.Configs.MConfigItems.Instance.enableWallJumpPowerGrip)
			{
				mp.EnableWallJump = true;
			}
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			if (Common.Configs.MConfigItems.Instance.enableLedgeClimbPowerGrip)
			{
				mp.powerGrip = true;
			}
			if (Common.Configs.MConfigItems.Instance.enableWallJumpPowerGrip)
			{
				mp.EnableWallJump = true;
			}
		}
	}
}
