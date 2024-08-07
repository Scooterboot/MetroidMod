using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace MetroidMod.Content.Switches
{
	public abstract class ModBubbleSwitch : ModType
	{
		/// <summary>
		/// The color of this bubble switch in the minimap.
		/// </summary>
		public abstract Color MapColor { get; }

		public ModBubbleSwitchItem Item { get; private set; }
		public ModBubbleSwitchTile Tile { get; private set; }

		protected override sealed void Register()
		{
			Item = new(this);
			Mod.AddContent(Item);

			Tile = new(this);
			Mod.AddContent(Tile);
		}
	}
}
