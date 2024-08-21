using MetroidMod.Content.Hatches.Behavior;
using MetroidMod.Content.Hatches.Variants;
using Terraria.ModLoader;

namespace MetroidMod.Content.Hatches.Visuals
{
	internal class HatchAppearance(HatchState hatch, IHatchAppearance hatchAppearance)
	{
		private record Parameters(HatchLockStatus LockState, bool IsBlue);

		public IHatchAppearance Current { get; private set; }
		private Parameters currentParameters;


		public void Update()
		{
			Parameters parameters = new(hatch.LockStatus, hatch.IsBlue);

			// If the appereance parameters are the same, we don't need to change it. Update it to make it flow instead
			if (parameters == currentParameters)
			{
				Current.Update();
				return;
			}

			// The appereance has changed, so create a new one. This will reset the animation, if any
			IHatchAppearance appereance = parameters.IsBlue ? ModContent.GetInstance<BlueHatch>().DefaultAppearance : hatchAppearance;
			switch (parameters.LockState)
			{
				case HatchLockStatus.Unlocked:
					break;
				case HatchLockStatus.UnlockedAndBlinking:
					appereance = new HatchBlinkingAppearance(appereance, new HatchStaticAppearance("LockedHatch"));
					break;
				case HatchLockStatus.Locked:
					appereance = new HatchStaticAppearance("LockedHatch");
					break;
			}

			Current = appereance;
			currentParameters = parameters;
		}
	}
}
