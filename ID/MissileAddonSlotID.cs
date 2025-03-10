namespace MetroidMod.ID
{
	public class MissileAddonSlotID
	{
		/// <summary>
		/// Not a real slot, used as a placeholder.
		/// </summary>
		public const short None = -1;
		/// <summary>
		/// Used to hold <b>Charge Combos</b>.<br/>Charge combos can go into the <b>Charge Combo Quick-Swap</b> array.
		/// <br/><br/>Examples: Wavebuster, Storm Missile, Nova Laser
		/// </summary>
		public const short Charge = 0;
		/// <summary>
		/// Used to hold upgrades to the <b>base missile projectile.</b>
		/// <br/><br/>Things like the <b>Super Missile</b> go here.
		/// </summary>
		public const short Primary = 1;
		/// <summary>
		/// Used <i>exclusively</i> for <b>Missile Tanks</b>.
		/// </summary>
		public const short Expansion = 2;
		/// <summary>
		/// Not a real slot, used to count the total number of slots.
		/// </summary>
		public const short Count = 3;
	}
}
