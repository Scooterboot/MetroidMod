using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.MorphBallAddons
{
	public class ShadowflameBomb : ModMBWeapon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/MBAddons/ShadowflameBomb/ShadowflameBombItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/ShadowflameBomb/ShadowflameBombTile";

		public override string ProjectileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/ShadowflameBomb/ShadowflameBombProjectile";

		public override bool AddOnlyAddonItem => false;

		public override bool CanGenerateOnChozoStatue(int x, int y) => WorldGen.drunkWorldGen;

		public override double GenerationChance(int x, int y) => 20;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Shadowflame Morph Ball Bombs");
			// ModProjectile.DisplayName.SetDefault("Shadowflame Morph Ball Bomb");
			/* Tooltip.SetDefault("-Right click to set off a bomb\n" +
			"Inflicts enemies with Shadowflame"); */
			ItemNameLiteral = true;
		}
		public override void SetItemDefaults(Item item)
		{
			item.damage = 59;
			item.value = Item.buyPrice(0, 2, 0, 0);
			item.rare = ItemRarityID.Pink;
		}

		public override void Kill(int timeLeft, ref int dustType, ref int dustType2, ref float dustScale, ref float dustScale2)
		{
			dustType = DustID.PurpleTorch;
			dustType2 = DustID.Shadowflame;
			dustScale2 = 3f;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(BuffID.ShadowFlame, 600);
		}
	}
}
