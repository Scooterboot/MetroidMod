using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;
using MetroidModPorted.Default;
using MetroidModPorted.ID;

namespace MetroidModPorted
{
	public abstract class ModBeam : ModType
	{
		public int Type { get; private set; }
		internal void ChangeType(int type)
		{
			Type = type;
		}

		public ModItem Item;
		public ModTile Tile;
		public int ItemType { get; internal set; }
		public int TileType { get; internal set; }

		/// <summary>
		/// The translations for the display name of this item.
		/// </summary>
		public ModTranslation DisplayName { get; internal set; }

		/// <summary>
		/// The translations for the tooltip of this item.
		/// </summary>
		public ModTranslation Tooltip { get; internal set; }

		//public Asset<Texture2D>[] Textures { get; internal set; }

		public abstract string ItemTexture { get; }

		public abstract string TileTexture { get; }

		/// <summary>
		/// Path to the texture of the normal shot when fired.
		/// </summary>
		public abstract string ProjectileTexture { get; }

		/// <summary>
		/// Path to the texture of the 'sphere' in front of the cannon while charging the shot.
		/// </summary>
		public abstract string ChargeLeadTexture { get; }

		/// <summary>
		/// Path to the texture of the charge shot when fired.
		/// </summary>
		public abstract string ChargeProjectileTexture { get; }

		/// <summary>
		/// Path to the texture to display on the Power Beam if the beam is equipped. Don't set to anything if you want it to keep to its default value. <br/>
		/// Requires an "_alt" file as well.
		/// </summary>
		public abstract string PowerBeamTexture { get; }

		public abstract bool AddOnlyBeamItem { get; }

		public virtual int DustCount { get; set; } = -1;

		public virtual int DustType { get; set; } = -1;

		/// <summary>
		/// Path to the sound played upon normal shot.
		/// </summary>
		public virtual string ShotSound { get; set; } = null;

		/// <summary>
		/// Path to the sound played while charging the beam.
		/// </summary>
		public virtual string ChargingSound { get; set; } = null;

		/// <summary>
		/// Path to the sound played upon charged shot.
		/// </summary>
		public virtual string ChargeShotSound { get; set; } = null;

		public int AddonSlot { get; set; } = BeamAddonSlotID.None;
		public string GetAddonSlotName()
		{
			return AddonSlot switch
			{
				BeamAddonSlotID.Charge => "Charge",
				BeamAddonSlotID.Secondary => "Secondary",
				BeamAddonSlotID.Utility => "Utility",
				BeamAddonSlotID.PrimaryA => "Primary A",
				BeamAddonSlotID.PrimaryB => "Primary B",
				_ => "Unknown",
			};
		}
		public uint AddonVersion { get; set; } = 0;
		public string GetAddonVersionName()
		{
			return AddonVersion switch
			{
				0 => "[c/FFFFFF:Prototypical Power Beam Addon]",
				1 => "[c/9696FF:Power Beam Addon]",
				2 => "[c/FF9696:Power Beam Addon V2]",
				3 => "[c/FF9696:Power Beam Addon V3]",
				4 => "[c/96FF96:Power Beam Addon V4]",
				5 => "[c/96FFFF:Power Beam Addon V5]",
				_ => $"[c/969696:Power Beam Addon V{AddonVersion}]",
			};
		}
		/// <summary>
		/// Sets the damage multiplier for a charged shot. (CHARGE SLOT ONLY)
		/// </summary>
		public float AddonChargeDamage { get; set; } = 1f;
		/// <summary>
		/// Sets a normal damage multiplier (NON-CHARGE SLOT ONLY)
		/// </summary>
		public float AddonDamageMult { get; set; } = 0f;
		/// <summary>
		/// Sets the heat multiplier for a charged shot (CHARGE SLOT ONLY)
		/// </summary>
		public float AddonChargeHeat { get; set; } = 1f;
		/// <summary>
		/// Sets a normal heat multiplier (NON-CHARGE SLOT ONLY)
		/// </summary>
		public float AddonHeat { get; set; } = 0f;
		/// <summary>
		/// Sets a normal speed multiplier (NON-CHARGE SLOT ONLY)
		/// </summary>
		public float AddonSpeed { get; set; } = 0f;
		public Color BeamColor { get; set; } = MetroidModPorted.powColor;
		public int WaveDepth { get; set; } = 4;
		/// <summary>
		/// Determines if the addon can generate on Chozo Statues during world generation.
		/// </summary>
		public virtual bool CanGenerateOnChozoStatue(Tile tile) => false;

		public override sealed void SetupContent()
		{
			SetStaticDefaults();
			InternalStaticDefaults();
			Item.SetStaticDefaults();
		}

		internal virtual void InternalStaticDefaults() { }

		public override void Load()
		{
			Item = new BeamItem(this);
			Tile = new BeamTile(this);
			if (Item == null) { throw new Exception("WTF happened here? BeamItem is null!"); }
			if (Tile == null) { throw new Exception("WTF happened here? BeamTile is null!"); }
			Mod.AddContent(Item);
			Mod.AddContent(Tile);
		}

		protected override sealed void Register()
		{
			DisplayName = LocalizationLoader.CreateTranslation(Mod, $"BeamName.{Name}");
			Tooltip = LocalizationLoader.CreateTranslation(Mod, $"BeamTooltip.{Name}");
			if (!AddOnlyBeamItem)
			{
				Type = BeamLoader.BeamCount;
				if (Type > 127)
				{
					throw new Exception("Beams Limit Reached. (Max: 128)");
				}
				BeamLoader.beams.Add(this);
			}
			Mod.Logger.Info($"Register new Beam: {FullName}, OnlyBeamItem: {AddOnlyBeamItem}");
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
		}

		public virtual void SetItemDefaults(Item item) { }

		public virtual void OnHit(Entity entity) { }

		public virtual bool OnShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			return true;
		}

		public virtual bool CanUse(Player player)
		{
			return true;
		}

		/// <inheritdoc cref="ModItem.AddRecipes"/>
		public virtual void AddRecipes() { }

		public Recipe CreateRecipe(int amount = 1) => Item.CreateRecipe(amount);
	}
}
