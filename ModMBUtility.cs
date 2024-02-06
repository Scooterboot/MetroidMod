using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
