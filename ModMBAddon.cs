using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;
using MetroidModPorted.ID;

namespace MetroidModPorted
{
	public abstract class ModMBAddon : ModType
	{
		public int Type { get; private set; }
		internal void ChangeType(int type) => Type = type;
		public int ItemType { get; internal set; }
		public int TileType { get; internal set; }

		public ModTranslation DisplayName { get; internal set; }

		public abstract string ItemTexture { get; }

		public abstract string TileTexture { get; }

		public abstract bool AddOnlyAddonItem { get; }

		public int AddonSlot { get; set; } = MorphBallAddonSlotID.None;
		public string GetAddonSlotName()
		{
			return AddonSlot switch
			{
				MorphBallAddonSlotID.Drill => "Drill",
				MorphBallAddonSlotID.Weapon => "Weapon",
				MorphBallAddonSlotID.Special => "Special",
				MorphBallAddonSlotID.Utility => "Utility",
				MorphBallAddonSlotID.Boost => "Boost",
				_ => "Unknown"
				/*0 => "Charge",
				1 => "Secondary",
				2 => "Utility",
				3 => "Primary A",
				4 => "Primary B",
				_ => "Unknown",*/
			};
		}

		public override sealed void SetupContent()
		{
			SetStaticDefaults();
		}

		protected override sealed void Register()
		{
			if (!AddOnlyAddonItem)
			{
				Type = MBAddonLoader.AddonCount;
				if (Type > 127)
				{
					throw new Exception("MB Addons Limit Reached. (Max: 128)");
				}
				MBAddonLoader.addons.Add(this);
			}
			//Mod.AddContent(new MBItem(this));
			//Mod.AddContent(new MBTile(this));
			MetroidModPorted.Instance.Logger.Info("Register new Morph Ball Addon: " + FullName + ", OnlyAddonItem: " + AddOnlyAddonItem);
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
		}
	}
}
