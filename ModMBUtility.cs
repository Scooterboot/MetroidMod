using MetroidMod.ID;

namespace MetroidMod
{
	public abstract class ModMBUtility : ModMBAddon
	{
		internal override sealed void InternalStaticDefaults()
		{
			AddonSlot = MorphBallAddonSlotID.Utility;
		}
	}
}
