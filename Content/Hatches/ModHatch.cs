﻿using MetroidMod.Content.Hatches.Visuals;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MetroidMod.Content.Hatches
{
	public abstract class ModHatch : ModType, ILocalizedModType
	{
		/// <summary>
		/// The item type associated with this hatch, which will drop when mined.
		/// </summary>
		public abstract int ItemType { get; }

		/// <summary>
		/// The main color of the hatch, used primarily as the tile color on the minimap.
		/// </summary>
		public abstract Color PrimaryColor { get; }
		
		/// <summary>
		/// Whether the hatch can be initially opened via right-click.
		/// </summary>
		public abstract bool InteractableByDefault { get; }

		/// <summary>
		/// The appearance this hatch will have by default (ie. when placed.)
		/// </summary>
		public virtual IHatchAppearance DefaultAppearance => new HatchStaticAppearance(Name);

		/// <summary>
		/// The translations to display for the name of this hatch.
		/// This name will be synced across both the tiles and items this hatch covers.
		/// </summary>
		public LocalizedText DisplayName => this.GetLocalization(nameof(DisplayName));

		public string LocalizationCategory => "Hatches";

		private readonly HatchTile[] hatchTiles = new HatchTile[4];

		protected override void Register()
		{
			hatchTiles[0] = new(this, false, false);
			hatchTiles[1] = new(this, true, false);
			hatchTiles[2] = new(this, false, true);
			hatchTiles[3] = new(this, true, true);
			
			foreach(HatchTile hatchTile in hatchTiles)
			{
				Mod.AddContent(hatchTile);
			}
		}


		public int GetTileType(bool open = false, bool vertical = false)
		{
			return hatchTiles[open.ToInt() + vertical.ToInt() * 2].Type;
		}
	}
}
