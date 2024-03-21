using MetroidMod.ID;

namespace MetroidMod
{
	public abstract class ModMBDrill : ModMBAddon
	{
		//public int DrillPower { get; set; } = 0;
		internal sealed override void InternalStaticDefaults()
		{
			AddonSlot = MorphBallAddonSlotID.Drill;
		}

		public override void UpdateEquip(Terraria.Player player)
		{
			player.GetModPlayer<Common.Players.MPlayer>().Drill(player);
		}
	}
}
