using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MetroidMod.ID;
using MetroidMod.Common.Players;

namespace MetroidMod.Content.SuitAddons
{
	public class SpaceJump : ModSuitAddon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/SpaceJump/SpaceJumpItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/SpaceJump/SpaceJumpTile";

		public override bool AddOnlyAddonItem => false;

		public override bool CanGenerateOnChozoStatue(int x, int y) => ((WorldGen.drunkWorldGen || WorldGen.everythingWorldGen) && Common.Configs.MConfigMain.Instance.drunkWorldHasDrunkStatues) || (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3);

		public override double GenerationChance(int x, int y) => 4;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Space Jump");
			/* Tooltip.SetDefault("'Somersault continuously in the air!'\n" +
				"Allows somersaulting\n" +
				"Allows the user to jump up to 10 times in a row\n" +
				"Jumps recharge mid-air\n" +
				"Increases jump height and prevents fall damage"); */
			AddonSlot = SuitAddonSlotID.Boots_Jump;
		}
		public override void SetItemDefaults(Item item)
		{
			item.width = 16;
			item.height = 16;
			item.value = Item.buyPrice(0, 4, 0, 0);
			item.rare = ItemRarityID.Lime;
			item.accessory = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(SuitAddonLoader.GetAddon<SpaceJumpBoots>().ItemType, 1)
				.AddIngredient(ItemID.HallowedBar, 10)
				.AddIngredient(ItemID.SoulofFlight, 10)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (!Common.Configs.MConfigMain.Instance.spaceJumpRocketBoots) { player.rocketTimeMax = 0; }
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.spaceJump = true;
			//mp.hiJumpBoost = true;
			player.noFallDmg = true;
		}
		public override void OnUpdateArmorSet(Player player, int stack)
		{
			if (!Common.Configs.MConfigMain.Instance.spaceJumpRocketBoots) { player.rocketTimeMax = 0; }
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.spaceJump = true;
			//mp.hiJumpBoost = true;
			player.noFallDmg = true;
		}
	}
}
