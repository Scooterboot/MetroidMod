using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroidMod.Common.Configs;
using Terraria.ModLoader;

namespace MetroidMod.Content.Hatches.Behavior
{
	/// <summary>
	/// Takes care of closing hatches automatically after a delay, if the config is enabled.
	/// Runs only on the server for simplicity, then syncs changes to all clients.
	/// </summary>
	internal class HatchAutocloseSystem : ModSystem
	{
		private bool AutocloseEnabled => ModContent.GetInstance<MConfigMain>().AutocloseHatchesEnabled;
		private int AutocloseTime => ModContent.GetInstance<MConfigMain>().AutocloseHatchesTime;

		private readonly Dictionary<HatchTileEntity, int> autocloseTimers = [];

		public override void ClearWorld()
		{
			autocloseTimers.Clear();
		}

		public override void PreUpdateWorld()
		{
			if(!AutocloseEnabled)
			{
				return;
			}

			foreach(HatchTileEntity hatch in HatchTileEntity.GetAll())
			{
				if(!hatch.IsPhysicallyOpen || hatch.State.DesiredState == HatchDesiredState.Closed)
				{
					autocloseTimers.Remove(hatch);
					continue;
				}

				int closeTimeInTicks = AutocloseTime * 60;
				if(autocloseTimers.TryAdd(hatch, closeTimeInTicks))
				{
					DebugAssist.NewTextMP("Hatch begins to autoclose");
				}

				if (autocloseTimers[hatch]-- <= 0)
				{
					autocloseTimers.Remove(hatch);
					DebugAssist.NewTextMP("Autoclosing hatch");
					hatch.State.DesiredState = HatchDesiredState.Closed;
					hatch.SyncState();
				}
			}
		}
	}
}
