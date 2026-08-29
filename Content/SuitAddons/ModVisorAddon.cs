using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;

namespace MetroidMod.Content.SuitAddons
{
	public interface IHelmetAddon : ISuitAddon
	{
		HelmetAddonSlot AddonSlot { get; }
	}

	/// <summary>
	/// An interface that describes how a Visor Upgrade will work.
	/// </summary>
	public interface IVisorAddon : IHelmetAddon
	{
		/// <summary>
		/// The path to the texture shown for the visor in the visor select. <br />
		/// Note: This is only used for Visors, such as the X-Ray Scope.
		/// </summary>
		string VisorSelectIcon { get; }

		/// <summary>
		/// The Sound to be played when the visor is in use. <br />
		/// Note: This is only used for Visors, such as the Scan Visor.
		/// </summary>
		SoundStyle? VisorBackgroundNoise { get; }

		/// <summary>
		/// The Color to set the hud to when the visor is in use. <br />
		/// Note: This is only used for Visors.
		/// </summary>
		Color VisorColor { get; }

		/// <summary>
		/// Allows you to do things when this visor is equipped and in use. <br />
		/// Note: This is only called for Visors, such as the X-Ray Scope.
		/// </summary>
		/// <param name="player">The player.</param>
		void DrawVisor(Player player);
	}

	public abstract class ModVisorAddon : ModSuitAddon, IVisorAddon
	{
		public virtual HelmetAddonSlot AddonSlot => HelmetAddonSlot.Unassigned;

		public virtual string VisorSelectIcon => TexturePath + "_Visor";

		public virtual SoundStyle? VisorBackgroundNoise => null;

		public virtual Color VisorColor => Color.LightBlue;

		public virtual void DrawVisor(Player player) { }
	}
}
