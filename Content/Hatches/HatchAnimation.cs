using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroidMod.Content.Hatches
{
	internal class HatchAnimation
	{
		public HatchAnimation(bool initiallyOpen) {
			isOpen = initiallyOpen;
			doorAnimationFrame = initiallyOpen ? doorAnimationFrameAmount : 0;
		}

		/// <summary>
		/// How closed or open the hatch is, visually. With the current spritesheets,
		/// a value of 0 indicates fully closed and a value of 4 indicates fully open.
		/// </summary>
		private int doorAnimationFrame;
		private const int doorAnimationFrameAmount = 4;

		/// <summary>
		/// Determines whether the hatch sprite should aim towards opening or closing.
		/// </summary>
		private bool isOpen;

		public int DoorAnimationFrame => doorAnimationFrame;

		public void Open()
		{
			isOpen = true;
		}

		public void Close()
		{
			isOpen = false;
		}

		/// <summary>
		/// Update the animation progress of the hatch.
		/// </summary>
		public void Update()
		{
			if (isOpen)
			{
				if (doorAnimationFrame < doorAnimationFrameAmount)
				{
					doorAnimationFrame += 1;
				}
			}
			else
			{
				if (doorAnimationFrame > 0)
				{
					doorAnimationFrame -= 1;
				}
			}
		}
	}
}
