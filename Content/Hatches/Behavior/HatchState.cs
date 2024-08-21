namespace MetroidMod.Content.Hatches
{
	/// <summary>
	/// This class contains the operational state of a hatch, upon which other components
	/// may use to reflect the hatch's state in the game world.
	/// 
	/// This state is generic and applies to all hatches, the special interactions hatches
	/// have with projectiles and the like must be done conditionally outside this class.
	/// </summary>
	internal class HatchState
	{
		/// <summary>
		/// Represents whether hatch should attempt to be open or closed. In the case of closing,
		/// this field becomes relevant because hatches can't close when there's something inside them.
		/// </summary>
		public HatchDesiredState DesiredState { get; set; }

		/// <summary>
		/// Stores whether the hatch is locked or unlocked. There's also a third, unlocked state variant, which accounts
		/// for whether the hatch was just unlocked, which accounts for the blinking behavior.
		/// </summary>
		public HatchLockStatus LockStatus { get; set; }

		/// <summary>
		/// Stores whether the hatch is capable of turning blue when hit by its appropiate projectile,
		/// or whether it has in fact turned blue aready. Meaningless for blue hatches...
		/// </summary>
		public HatchBlueConversionStatus BlueConversion { get; set; }

		/// <summary>
		/// Returns whether the hatch is currently turned blue and should behave as a blue hatch.
		/// </summary>
		public bool IsBlue => BlueConversion == HatchBlueConversionStatus.EnabledAndBlue;

		/// <summary>
		/// Returns whether the hatch is locked and thus can't be manually interacted with.
		/// </summary>
		public bool IsLocked => LockStatus == HatchLockStatus.Locked;

		public void Interact()
		{
			if(IsLocked)
			{
				return;
			}

			LockStatus = HatchLockStatus.Unlocked;
			Toggle();
		}

		public void Toggle()
		{
			switch (DesiredState)
			{
				case HatchDesiredState.Open:
					DesiredState = HatchDesiredState.Closed;
					break;
				case HatchDesiredState.Closed:
					DesiredState = HatchDesiredState.Open;
					break;
			}
		}

		public void HitProjectile()
		{
			DesiredState = HatchDesiredState.Open;

			if (BlueConversion == HatchBlueConversionStatus.EnabledAndRegular)
			{
				BlueConversion = HatchBlueConversionStatus.EnabledAndBlue;
			}
		}

		public void ToggleBlueConversion()
		{
			if(BlueConversion == HatchBlueConversionStatus.Disabled)
			{
				BlueConversion = HatchBlueConversionStatus.EnabledAndRegular;
			}
			else
			{
				BlueConversion = HatchBlueConversionStatus.Disabled;
			}
		}

		public void ToggleLocked()
		{
			if (IsLocked)
			{
				LockStatus = HatchLockStatus.UnlockedAndBlinking;
			}
			else
			{
				LockStatus = HatchLockStatus.Locked;
			}
		}

		public override string ToString()
		{
			return $"[{DesiredState} {LockStatus} {BlueConversion}]";
		}
	}

	internal enum HatchDesiredState
	{
		Closed,
		Open,
	}

	internal enum HatchLockStatus
	{
		Unlocked,
		UnlockedAndBlinking,
		Locked,
	}

	internal enum HatchBlueConversionStatus
	{
		Disabled,
		EnabledAndRegular,
		EnabledAndBlue,
	}

}
