namespace MetroidMod.Content.Hatches.Visuals
{
	internal class HatchDoorAnimation
	{
		public int DoorAnimationFrame => doorAnimationFrame;

		public HatchDoorAnimation(bool initiallyOpen)
		{
			doorAnimationFrame = initiallyOpen ? doorAnimationFrameAmount : 0;
		}

		public void Update(bool isOpen)
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

		/// <summary>
		/// How closed or open the hatch is, visually. With the current spritesheets,
		/// a value of 0 indicates fully closed and a value of 4 indicates fully open.
		/// </summary>
		private int doorAnimationFrame;
		private const int doorAnimationFrameAmount = 4;
	}
}
