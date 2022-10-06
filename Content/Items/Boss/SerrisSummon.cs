using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Boss
{
	public class SerrisSummon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Serris Bait");
			Tooltip.SetDefault("Summons Serris at the ocean");

			SacrificeTotal = 3;
		}
		public override void SetDefaults()
		{
			Item.maxStack = 20;
			Item.consumable = true;
			Item.width = 12;
			Item.height = 12;
			Item.useTime = 45;
			Item.useAnimation = 45;
			Item.useStyle = 4;
			Item.noMelee = true;
			Item.value = 1000;
			Item.rare = 7;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.PurificationPowder, 20)
				.AddIngredient(ItemID.Bone, 3)
				.AddIngredient(ItemID.Worm, 1)
				.AddTile(TileID.DemonAltar)
				.Register();
		}
		
		public override bool ConsumeItem(Player player)
		{
			if (Common.Configs.MConfigMain.Instance.enableBossSummonConsumption)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		
		public override bool CanUseItem(Player player)
		{
			return !NPC.AnyNPCs(ModContent.NPCType<NPCs.Serris.Serris_Head>()) && player.ZoneBeach;
		}
		public override bool? UseItem(Player player)
		{
			NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<NPCs.Serris.Serris_Head>());
			SoundEngine.PlaySound(Sounds.Items.SerrisSummon, player.Center);
			return true;
		}
	}
}
