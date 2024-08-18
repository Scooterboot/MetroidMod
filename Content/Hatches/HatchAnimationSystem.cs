using System.Collections.Generic;
using Terraria.ModLoader;

namespace MetroidMod.Content.Hatches
{
	/// <summary>
	/// Animation is a concept that needs be strictly visual, 
	/// meaning the server should be able to safely ignore it.
	/// This means this class should only be used on client code
	/// (aka rendering)
	/// </summary>
	[Autoload(Side = ModSide.Client)]
	internal class HatchAnimationSystem : ModSystem
	{
		private readonly Dictionary<HatchTileEntity, HatchAnimation> animations = [];

		public override void PostUpdateEverything()
		{
			foreach(HatchTileEntity hatch in HatchTileEntity.GetAll())
			{
				HatchAnimation animation = GetHatchAnimation(hatch);
				animation.IsOpen = hatch.IsOpen;
				animation.Update();
			}
		}

		public HatchAnimation GetHatchAnimation(HatchTileEntity hatch)
		{
			if (!animations.ContainsKey(hatch))
			{
				animations[hatch] = new HatchAnimation(hatch.IsOpen);
			}

			return animations[hatch];
		}
	}
}
