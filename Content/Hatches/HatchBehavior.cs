using Terraria;

namespace MetroidMod.Content.Hatches
{
	// Why so many interfaces? I have no idea what I'm doing!!!! TODO???
	internal class HatchBehavior
		(IHatchProvider hatchProvider, IHatchOpenController openController, IHatchVisualController Visual)
	{

		/// <summary>
		/// The logical state of the hatch, that determines whether entities
		/// should be able to pass through it at this specific moment. It is
		/// separate from the visual state of the hatch.
		/// </summary>
		public bool IsOpen => openController.IsOpen;

		/// <summary>
		/// Hatches can be turned blue after being opened with a projectile
		/// by using the Chozo Wrench on them. After a hatch turns blue,
		/// it will be functionally similar to a blue hatch, meaning it will
		/// be openable with any projectile and also right click.
		/// This setting must persist across world loads.
		/// </summary>
		public HatchBlueConversionStatus BlueConversion { get; set; }

		/// <summary>
		/// Whether the hatch is turned blue and should behave like a blue hatch.
		/// </summary>
		public bool IsTurnedBlue => BlueConversion == HatchBlueConversionStatus.Blue;

		/// <summary>
		/// You can make a hatch locked by activating the metalic part with wire.
		/// If a hatch is locked, it will refuse to be opened by ANY means.
		/// This includes activating the bubble part to open it via wire.
		/// The only way to open a locked hatch is to unlock it first via wire.
		/// This setting must persist across world loads.
		/// </summary>
		public bool Locked;

		/// <summary>
		/// The type of hatch that this behavior controls.
		/// </summary>
		public ModHatch Hatch => hatchProvider.Hatch;

		/// <summary>
		/// Directly open the hatch, without any checks.
		/// </summary>
		public void Open()
		{
			openController.Open();
		}

		/// <summary>
		/// Directly close the hatch, without any checks.
		/// </summary>
		public void Close()
		{
			openController.Close();
			MakeCurrentColor();
		}

		/// <summary>
		/// Toggle the hatch between open and closed.
		/// </summary>
		public void Toggle()
		{

			if (IsOpen)
			{
				Close();
			}
			else
			{
				Open();
			}
		}

		/// <summary>
		/// Hit the hatch with a player right click.
		/// </summary>
		/// <param name="withKeycard">Whether the user has a keycard capable of opening this hatch.</param>
		public void HitRightClick(bool withKeycard)
		{
			if(Locked)
			{
				return;
			}

			bool isBlue = Hatch.InteractableByDefault || IsTurnedBlue;
			
			if (!isBlue && !withKeycard)
			{
				return;
			}

			Toggle();
		}

		/// <summary>
		/// Hit the hatch with a Chozo Wrench, which will toggle the ability of the hatch
		/// to turn into a Blue Hatch after being opened with its respective projectile.
		/// </summary>
		public void HitChozoWrench()
		{
			ToggleBlueConversion();
		}

		public void WireToggleDoor()
		{
			Toggle();
		}

		public void WireToggleBlue()
		{
			ToggleBlueConversion();
		}

		public void WireToggleLocked()
		{
			if(Locked)
			{
				Unlock();
			}
			else
			{
				Lock();
			}
		}

		public void Lock()
		{
			Locked = true;
			Visual.SetVisualState(HatchVisualState.Locked);
		}

		public void Unlock(bool withBlinking = true)
		{
			Locked = false;

			if (withBlinking)
			{
				Visual.SetVisualState(HatchVisualState.Blinking);
			}
			else
			{
				MakeCurrentColor();
			}
		}
		

		/// <summary>
		/// Hit the hatch with a projectile that is capable of opening it under normal conditions.
		/// For example, a Green Hatch would have this method called by a Super Missile.
		/// </summary>
		public void HitProjectile()
		{
			if (Locked)
			{
				return;
			}

			ConvertToBlue();
			Open();
		}

		private void ToggleBlueConversion()
		{
			if (BlueConversion == HatchBlueConversionStatus.Disabled)
			{
				BlueConversion = HatchBlueConversionStatus.Enabled;
			}
			else
			{
				BlueConversion = HatchBlueConversionStatus.Disabled;
			}

			MakeCurrentColor();
		}

		private void ConvertToBlue()
		{
			if (BlueConversion != HatchBlueConversionStatus.Enabled)
			{
				return;
			}

			BlueConversion = HatchBlueConversionStatus.Blue;
		}

		private void MakeCurrentColor()
		{
			if (Locked)
			{
				Visual.SetVisualState(HatchVisualState.Locked);
				return;
			}

			Visual.SetVisualState(HatchVisualState.Current);
		}
	}

	internal enum HatchBlueConversionStatus
	{
		/// <summary>
		/// The hatch won't turn blue when hit by a projectile.
		/// </summary>
		Disabled,
		/// <summary>
		/// The hatch will turn blue when hit by a projecitle.
		/// </summary>
		Enabled,
		/// <summary>
		/// The hatch has turned blue.
		/// </summary>
		Blue,
	}

	internal interface IHatchOpenController
	{
		bool IsOpen { get; }
		void Open();
		void Close();
	}

	internal interface IHatchVisualController
	{
		void SetVisualState(HatchVisualState state);
	}

	internal interface IHatchProvider
	{
		ModHatch Hatch { get; }
	}

	internal enum HatchVisualState
	{
		Current,
		Locked,
		Blinking
	}
}
