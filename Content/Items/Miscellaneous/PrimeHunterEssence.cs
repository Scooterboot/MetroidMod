using MetroidMod.Common.Players;
using MetroidMod.Common.GlobalItems;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MetroidMod.Content.Buffs;

namespace MetroidMod.Content.Items.Miscellaneous
{
	public class PrimeHunterEssence : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.ItemNoGravity[Type] = true;
			ItemID.Sets.IgnoresEncumberingStone[Type] = true;
			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(5, 8));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true;
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
			Item.width = 20;
			Item.height = 26;
			Item.value = 100;
			Item.rare = ItemRarityID.Purple;
		}
		public override bool ItemSpace(Player player) => true;
		public override bool CanPickup(Player player) => player.TryMetroidPlayer(out MPlayer mp) && mp.ShouldShowArmorUI;
		public override bool OnPickup(Player player)
		{
			if (!player.TryMetroidPlayer(out MPlayer mp))
			{
				return false;
			}

			// if we're not in the suit, don't apply. we shouldn't ever trigger that though.
			if (!mp.ShouldShowArmorUI)
			{
				return false;
			}

			Terraria.Audio.SoundEngine.PlaySound(Sounds.Suit.PrimeHunterActivate, player.position);
			player.AddBuff(ModContent.BuffType<PrimeHunterBuff>(), 20 * 60);
			mp.PrimeHunter = true;
			return false;
		}
	}
}
