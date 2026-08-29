using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.SuitAddons
{
	/// <summary>
	/// An interface that would apply to an Addon that belongs in the breastplate.
	/// </summary>
	public interface IBreastplateAddon : ISuitAddon
	{
		BreastplateAddonSlot AddonSlot { get; }
	}

	// We're extending IBreastplateAddon because we don't expect a suit appearance-changing addon
	// anywhere other than the breastplate. If we wanted to, we could change this. But,
	// unfortunately, I'm lazy and don't see any use cases for that. - Armipotent
	/// <summary>
	/// An interface that describes how a Suit Upgrade will work.
	/// </summary>
	public interface ISuitUpgrade : IBreastplateAddon
	{
		string ArmorTextureHead { get; }
		string ArmorTextureTorso { get; }
		/// <summary>
		/// Main visible shoulder texture location. <br />
		/// Only used by Barrier addons.
		/// </summary>
		string OnShoulderTexture { get; }
		/// <summary>
		/// Semi-hidden visible shoulder texture location. <br />
		/// Only used by Barrier addons.
		/// </summary>
		string OffShoulderTexture { get; }
		string ArmorTextureArmsGlow { get; }
		string ArmorTextureShouldersGlow { get; }
		string ArmorTextureLegs { get; }

		static bool ShouldOverrideShoulders;
	}

	/// <summary>
	/// This is either a Barrier or Primary addon for the Power Suit Breastplate.
	/// </summary>
	public abstract class ModSuitUpgrade : ModSuitAddon, ISuitUpgrade
	{
		public virtual string ArmorTextureHead => TexturePath + "_Head";

		public virtual string ArmorTextureTorso => TexturePath + "_Body";

		public virtual string OnShoulderTexture => TexturePath + "_OnShoulder";

		public virtual string OffShoulderTexture => TexturePath + "_OffShoulder";

		public virtual string ArmorTextureArmsGlow => TexturePath + "_Arms_Glow";

		public virtual string ArmorTextureShouldersGlow => TexturePath + "_Shoulders_Glow";

		public virtual string ArmorTextureLegs => TexturePath + "_Legs";
		
		/// <summary>
		/// Used for Barrier addons. Set to true if the suit should override the shoulders of the Primary addon.
		/// </summary>
		public static bool ShouldOverrideShoulders = false;

		public virtual BreastplateAddonSlot AddonSlot => BreastplateAddonSlot.Unassigned;

		public override void Load()
		{
			base.Load();
			if (Main.netMode != NetmodeID.Server)
			{
				EquipLoader.AddEquipTexture(Mod, ArmorTextureHead, EquipType.Head, name: Name);
				EquipLoader.AddEquipTexture(Mod, ArmorTextureTorso, EquipType.Body, name: Name);
				EquipLoader.AddEquipTexture(Mod, ArmorTextureLegs, EquipType.Legs, name: Name);
			}
		}

		public override void SetStaticDefaults()
		{
			SetupDrawing();
			base.SetStaticDefaults();
		}

		private void SetupDrawing()
		{
			if (Main.netMode == NetmodeID.Server) { return; }
			int equipSlotHead = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
			int equipSlotBody = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body);
			int equipSlotLegs = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Legs);

			ArmorIDs.Head.Sets.DrawHead[equipSlotHead] = false;
			ArmorIDs.Body.Sets.HidesTopSkin[equipSlotBody] = true;
			//ArmorIDs.Body.Sets.HidesArms[equipSlotBody] = true;
			ArmorIDs.Legs.Sets.HidesBottomSkin[equipSlotLegs] = true;
		}
	}
}