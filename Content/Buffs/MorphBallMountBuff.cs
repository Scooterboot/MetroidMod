#region Using directives

using Terraria;
using Terraria.ModLoader;

#endregion

namespace MetroidMod.Content.Buffs
{
	public class MorphBallMountBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Morph Ball");
			Description.SetDefault("Because you apparently can't crawl");

			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.mount.SetMount(ModContent.MountType<Content.Mounts.MorphBallMount>(), player);
			player.buffTime[buffIndex] = 10;
		}
	}
}