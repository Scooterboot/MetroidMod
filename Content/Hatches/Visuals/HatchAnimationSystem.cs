using System.Collections.Generic;
using Terraria.ModLoader;

namespace MetroidMod.Content.Hatches.Visuals
{
	/// <summary>
	/// Takes care of animating the opening and texture of a hatch.
	/// Animation is a concept that needs be strictly visual, 
	/// meaning the server should be able to safely ignore it.
	/// This means this class should only be used on client code
	/// (aka rendering)
	/// </summary>
	[Autoload(Side = ModSide.Client)]
	internal class HatchAnimationSystem : ModSystem
	{
		private record HatchVisuals(HatchDoorAnimation Animation, HatchAppearance Appearance);
		private readonly Dictionary<HatchTileEntity, HatchVisuals> visuals = [];

		public override void ClearWorld()
		{
			visuals.Clear();
		}

		public override void PostUpdateEverything()
		{
			foreach (HatchTileEntity hatch in HatchTileEntity.GetAll())
			{
				HatchVisuals visuals = GetVisuals(hatch);
				visuals.Animation.Update(hatch.IsPhysicallyOpen);
				visuals.Appearance.Update();
			}
		}

		public HatchDoorAnimation GetHatchAnimation(HatchTileEntity hatch)
		{
			return GetVisuals(hatch).Animation;
		}

		public IHatchAppearance GetAppearance(HatchTileEntity hatch)
		{
			return GetVisuals(hatch).Appearance.Current;
		}

		private HatchVisuals GetVisuals(HatchTileEntity hatch)
		{
			if (!visuals.TryGetValue(hatch, out HatchVisuals value))
			{
				value = new(new(hatch.IsPhysicallyOpen), new(hatch.State, hatch.ModHatch.DefaultAppearance));
				visuals[hatch] = value;
				value.Appearance.Update();
			}

			return value;
		}
	}
}
