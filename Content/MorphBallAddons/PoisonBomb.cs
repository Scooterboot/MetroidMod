using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace MetroidMod.Content.MorphBallAddons
{
	public class PoisonBomb : ModMBWeapon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/MBAddons/PoisonBomb/PoisonBombItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/PoisonBomb/PoisonBombTile";

		public override string ProjectileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/PoisonBomb/PoisonBombProjectile";

		public override bool AddOnlyAddonItem => false;

		public override bool CanGenerateOnChozoStatue(int x, int y) => ((WorldGen.drunkWorldGen || WorldGen.everythingWorldGen) && Common.Configs.MConfigMain.Instance.drunkWorldHasDrunkStatues) || NPC.downedQueenBee;
		public override double GenerationChance(int x, int y) => 1;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Poison Morph Ball Bombs");
			// ModProjectile.DisplayName.SetDefault("Poison Morph Ball Bomb");
			/* Tooltip.SetDefault("-Right click to set off a bomb\n" +
			"Poisons foes"); */
			ItemNameLiteral = true;
		}
		public override void SetItemDefaults(Item item)
		{
			item.damage = 21;
			item.value = Item.buyPrice(0, 0, 50, 0);
			item.rare = ItemRarityID.Green;
		}

		public override void Kill(int timeLeft, ref int dustType, ref int dustType2, ref float dustScale, ref float dustScale2)
		{
			dustType = DustID.Dirt;
			dustScale = 2f;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(BuffID.Poisoned, 600);
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddRecipeGroup(MBAddonLoader.BombsRecipeGroupID, 1)
				.AddIngredient(ItemID.JungleSpores, 5)
				.AddIngredient(ItemID.Stinger, 3)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
