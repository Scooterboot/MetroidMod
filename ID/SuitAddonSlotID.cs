namespace MetroidMod.ID
{
	public class SuitAddonSlotID
	{
		/// <summary>
		/// Assigning this ID to the slot type will make it not equipable.
		/// </summary>
		public const short None = -1;
		/// <summary>
		/// Slot for Energy Tanks.
		/// </summary>
		public const short Tanks_Energy = 0;
		/// <summary>
		/// Slot for Reserve Tanks.
		/// </summary>
		public const short Tanks_Reserve = 1;
		/* Old, still here for reference
		/// <summary>
		/// Slot for the Varia Suit and the Varia Suit V2.
		/// </summary>
		public const short Suit_Varia = 2;
		/// <summary>
		/// Slot for the Dark Suit, the Gravity Suit, and the P.E.D Suit.
		/// </summary>
		public const short Suit_Utility = 3;
		/// <summary>
		/// Slot for the Light Suit, the Terra Gravity Suit, the Phazon Suit, and the Hazard Shield Suit.
		/// </summary>
		public const short Suit_Augment = 4;
		/// <summary>
		/// Slot for the Lunar Augments.
		/// </summary>
		public const short Suit_LunarAugment = 5;
		*/
		/// <summary>
		/// Slot for "Barrier" suits (Varia, Varia V2, Haz.)
		/// </summary>
		public const short Suit_Barrier = 2;
		/// <summary>
		/// Slot for Gravity, Phazon, etc. suits.
		/// </summary>
		public const short Suit_Primary = 3;
		/* Old, still here for reference
		/// <summary>
		/// Slot for the Power Grip.
		/// </summary>
		public const short Misc_Grip = 4;
		/// <summary>
		/// Slot for the Screw Attack.
		/// </summary>
		public const short Misc_Attack = 5;
		/// <summary>
		/// Slot for the Hi-Jump Boots and the Space Jump Boots.
		/// </summary>
		public const short Boots_JumpHeight = 6;
		/// <summary>
		/// Slot for the Space Jump.
		/// </summary>
		public const short Boots_Jump = 7;
		/// <summary>
		/// Slot for the Speed Booster.
		/// </summary>
		public const short Boots_Speed = 8;
		*/
		/// <summary>
		/// Slot for the Scan Visor.
		/// </summary>
		public const short Visor_Scan = 4;
		/// <summary>
		/// Slot for the Thermal Visor, the Dark Visor, and the Command Visor.
		/// </summary>
		public const short Visor_Utility = 5;
		/// <summary>
		/// Slot for the X-Ray Visor and the Echo Visor.
		/// </summary>
		public const short Visor_AltVision = 6;

		public const short Count = 7;
	}
}
