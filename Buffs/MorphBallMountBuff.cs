#region Using directives

using Terraria;
using Terraria.ModLoader;

#endregion

namespace MetroidMod.Buffs
{
	public class MorphBallMountBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Morph Ball");
			Description.SetDefault("Because you apparently can't crawl");
			
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.mount.SetMount(ModContent.MountType<Mounts.MorphBallMount>(), player);
			player.buffTime[buffIndex] = 10;
		}
	}
}
