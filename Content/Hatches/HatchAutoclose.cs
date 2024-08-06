using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroidMod.Common.Configs;
using Terraria.ModLoader;

namespace MetroidMod.Content.Hatches
{
	internal class HatchAutoclose(HatchTileEntity tileEntity)
	{
		private bool AutocloseEnabled => ModContent.GetInstance<MConfigMain>().AutocloseHatchesEnabled;
		private int AutocloseTime => ModContent.GetInstance<MConfigMain>().AutocloseHatchesTime;
		
		private int autocloseTimer;


		public void Update()
		{
			if (AutocloseEnabled && tileEntity.IsOpen)
			{
				if(autocloseTimer > 0)
				{
					autocloseTimer -= 1;
					return;
				}

				tileEntity.Behavior.Close();
			}
		}

		public void Open()
		{
			if(AutocloseEnabled)
			{
				// TODO converting seconds to frames is far from precise,
				// if it's needed that the cooldown be accurate this
				// should probably be changed
				autocloseTimer = AutocloseTime * 60;
			}
		}
	}
}
