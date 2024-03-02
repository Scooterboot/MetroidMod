using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.MorphBallAddons
{
	public class BetsyBomb : ModMBWeapon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/MBAddons/BetsyBomb/BetsyBombItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/BetsyBomb/BetsyBombTile";

		public override string ProjectileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/BetsyBomb/BetsyBombProjectile";

		public override bool AddOnlyAddonItem => false;

		public override bool CanGenerateOnChozoStatue(int x, int y) => Common.Configs.MConfigMain.Instance.drunkWorldHasDrunkStatues;

		public override double GenerationChance(int x, int y) => 1;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Betsy Morph Ball Bombs");
			// ModProjectile.DisplayName.SetDefault("Betsy Morph Ball Bomb");
			/* Tooltip.SetDefault("-Right click to set off a bomb\n" +
			"Explodes in defense reducing miasma that also sets enemies on fire"); */
			ItemNameLiteral = true;
		}
		public override void SetItemDefaults(Item item)
		{
			item.damage = 120;
			item.value = Item.buyPrice(0, 6, 0, 0);
			item.rare = ItemRarityID.Cyan;
		}

		public override void Kill(int timeLeft, ref int dustType, ref int dustType2, ref float dustScale, ref float dustScale2)
		{
			dustType = 55;
			dustType2 = DustID.OrangeTorch;
			dustScale = 3f;
			dustScale2 = 3f;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(BuffID.BetsysCurse, 600);
			target.AddBuff(BuffID.Oiled, 600);
			target.AddBuff(BuffID.OnFire, 600);
		}
	}
}
