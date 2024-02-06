using MetroidMod.ID;

namespace MetroidMod
{
	public abstract class ModMBBoost : ModMBAddon
	{
		internal override sealed void InternalStaticDefaults()
		{
			AddonSlot = MorphBallAddonSlotID.Boost;
		}
	}
}
