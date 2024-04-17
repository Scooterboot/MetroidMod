using System.IO;
using MetroidMod.Common.Players;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace MetroidMod.Content.Items.Armors
{
	[AutoloadEquip(EquipType.Body)]
	public class PowerSuitBreastplate : ModItem
	{
		// Failsaves.
		private Item[] _suitAddons;
		public Item[] SuitAddons
		{
			get {
				if (_suitAddons == null)
				{
					_suitAddons = new Item[SuitAddonSlotID.Suit_Primary + 1];
					for (int i = 0; i < _suitAddons.Length; i++)
					{
						_suitAddons[i] = new Item();
						_suitAddons[i].TurnToAir();
					}
				}

				return _suitAddons;
			}
			set { _suitAddons = value; }
		}
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Power Suit Breastplate");
			/* Tooltip.SetDefault("+15 overheat capacity\n" +
			"10% decreased overheat use"); */

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.rare = ItemRarityID.Green;
			Item.value = 9000;
			Item.defense = Common.Configs.MConfigItems.Instance.defensePowerSuitBreastplate;
		}
		public override void UpdateEquip(Player player)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.maxOverheat += 15;
			mp.overheatCost -= 0.10f;
			mp.tankCapacity += 4;
			mp.IsPowerSuitBreastplate = true;
			if (Common.Configs.MConfigItems.Instance.enableLedgeClimbPowerSuitBreastplate)
			{
				mp.powerGrip = true;
			}
			#region Handle old data
			if (SuitAddons.Length > SuitAddonSlotID.Suit_Primary + 1)
			{
				if (!SuitAddons[4].IsAir)
				{ player.QuickSpawnItem(new EntitySource_OverfullInventory(player), SuitAddons[4], SuitAddons[4].stack); SuitAddons[4].TurnToAir(true); }
				if (!SuitAddons[5].IsAir)
				{ player.QuickSpawnItem(new EntitySource_OverfullInventory(player), SuitAddons[5], SuitAddons[5].stack); SuitAddons[5].TurnToAir(true); }
				if (!SuitAddons[6].IsAir)
				{ player.QuickSpawnItem(new EntitySource_OverfullInventory(player), SuitAddons[6], SuitAddons[6].stack); SuitAddons[6].TurnToAir(true); }
				if (!SuitAddons[7].IsAir)
				{ player.QuickSpawnItem(new EntitySource_OverfullInventory(player), SuitAddons[7], SuitAddons[7].stack); SuitAddons[7].TurnToAir(true); }
				Item[] items = new Item[4];
				for (int i = 0; i < items.Length; i++)
				{
					items[i] = SuitAddons[i];
				}
				SuitAddons = items;
			}
			#endregion
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == ModContent.ItemType<PowerSuitHelmet>() && body.type == ModContent.ItemType<PowerSuitBreastplate>() && legs.type == ModContent.ItemType<PowerSuitGreaves>();
		}
		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Allows the ability to Sense Move" + "\n" +
							"Double tap a direction (when enabled)" + "\n" +
							"Right click the Sense Move button to access Addon Menu";// + 
																					 //SuitAddonLoader.GetSetBonusText(player);
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.EnergyExpenseEfficiency += Common.Configs.MConfigItems.Instance.energyExpenseEfficiency;
			mp.senseMove = true;
			mp.ShouldShowArmorUI = true;
			SuitAddonLoader.OnUpdateArmorSet(player);
		}
		public override void ArmorSetShadows(Player player)
		{
			SuitAddonLoader.ArmorSetShadows(player);
		}
		public override void UpdateVanitySet(Player P)
		{
			MPlayer mp = P.GetModPlayer<MPlayer>();
			mp.isPowerSuit = true;
			mp.visorGlowColor = new Color(0, 248, 112);
			if (P.velocity.Y != 0f && ((P.controlRight && P.direction == 1) || (P.controlLeft && P.direction == -1) || mp.SMoveEffect > 0) && mp.shineDirection == 0 && !mp.shineActive && !mp.ballstate)
			{
				mp.jet = true;
			}
			else if (mp.shineDirection == 0 || mp.shineDirection == 5)
			{
				mp.jet = false;
			}
			SuitAddonLoader.OnUpdateVanitySet(P);
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<ChoziteBreastplate>(1)
				.AddIngredient(SuitAddonLoader.GetAddon<SuitAddons.EnergyTank>().ItemType, 1)
				.AddRecipeGroup(MetroidMod.EvilBarRecipeGroupID, 20)
				.AddRecipeGroup(MetroidMod.EvilMaterialRecipeGroupID, 20)
				.AddTile(TileID.Anvils)
				.Register();
		}
		public override void SaveData(TagCompound tag)
		{
			for (int i = 0; i < SuitAddons.Length; ++i)
			{
				// Failsave check.
				if (SuitAddons[i] == null)
				{
					SuitAddons[i] = new Item();
				}
				tag.Add("SuitAddons" + i, ItemIO.Save(SuitAddons[i]));
			}
		}
		public override void LoadData(TagCompound tag)
		{
			try
			{
				if (tag.ContainsKey("SuitAddons4"))
				{
					LoadLegacyData(tag);
				}
				else
				{
					SuitAddons = new Item[SuitAddonSlotID.Suit_Primary + 1];
					for (int i = 0; i < SuitAddons.Length; i++)
					{
						Item item = tag.Get<Item>("SuitAddons" + i);
						SuitAddons[i] = item;
					}
				}
			}
			catch { }
		}
		/// <summary>
		/// Loads (and readies) pre-rework save data. The pre-rework save data in question follows a format such that:<br/>
		/// 0 = Reserve Tanks<br/>
		/// 1 = Energy Tanks<br/>
		/// 2 = Varia/Varia V2<br/>
		/// 3 = Gravity/Dark/PED<br/>
		/// 4 = Light/Terra Gravity/Phazon/Hazard Shield<br/>
		/// 5 = Lunar<br/>
		/// </summary>
		/// <param name="tag">The TagCompound to load data from.</param>
		public void LoadLegacyData(TagCompound tag)
		{
			try
			{
				SuitAddons = new Item[8];
				// varia and gravity are fine as-is, they'll just go into "Barrier" and "Primary" respectively
				// however we'll want to spit out IDs 4 through 7
				for (int i = 0; i < 8; i++)
				{
					Item item = tag.Get<Item>("SuitAddons" + i);
					SuitAddons[i] = item;
				}
			}
			catch { }
		}
		public override void NetSend(BinaryWriter writer)
		{
			for (int i = 0; i < SuitAddons.Length; ++i)
			{
				ItemIO.Send(SuitAddons[i], writer);
			}
		}
		public override void NetReceive(BinaryReader reader)
		{
			for (int i = 0; i < SuitAddons.Length; ++i)
			{
				SuitAddons[i] = ItemIO.Receive(reader);
			}
		}

		public override ModItem Clone(Item newEntity)
		{
			PowerSuitBreastplate obj = (PowerSuitBreastplate)base.Clone(newEntity);
			obj.SuitAddons = SuitAddons;
			return obj;
		}

		public override void OnResearched(bool fullyResearched)
		{
			foreach (Item item in SuitAddons)
			{
				if (item == null || item.IsAir) { continue; }
				IEntitySource itemSource_OpenItem = Main.LocalPlayer.GetSource_OpenItem(Type);
				Main.LocalPlayer.QuickSpawnItem(itemSource_OpenItem, item, item.stack);
			}
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class PowerSuitGreaves : ModItem
	{
		// Failsaves.
		private Item[] _suitAddons;
		/// <summary>
		/// HEY! This is just here for pre-rework addon formats. These will only be spat out.
		/// </summary>
		public Item[] SuitAddons
		{
			get {
				if (_suitAddons == null)
				{
					_suitAddons = new Item[3];
					for (int i = 0; i < _suitAddons.Length; i++)
					{
						_suitAddons[i] = new Item();
						_suitAddons[i].TurnToAir();
					}
				}

				return _suitAddons;
			}
			set { _suitAddons = value; }
		}
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Power Suit Greaves");
			/* Tooltip.SetDefault("Allows somersaulting & wall jumping\n" +
			"Negates fall damage"); */

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.rare = ItemRarityID.Green;
			Item.value = 6000;
			Item.defense = Common.Configs.MConfigItems.Instance.defensePowerSuitGreaves;
		}
		public override void UpdateEquip(Player player)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.IsPowerSuitGreaves = true;
			if (Common.Configs.MConfigItems.Instance.enableWallJumpPowerSuitGreaves)
			{
				mp.EnableWallJump = true;
			}
			if (Common.Configs.MConfigItems.Instance.enableNoFallDamagePowerSuitGreaves)
			{
				player.noFallDmg = true;
			}
			#region Old data handling
			if (SuitAddons != null && SuitAddons.Length > 0)
			{
				if (!SuitAddons[0].IsAir)
				{ player.QuickSpawnItem(new EntitySource_OverfullInventory(player), SuitAddons[0], SuitAddons[0].stack); SuitAddons[0].TurnToAir(true); }
				if (!SuitAddons[1].IsAir)
				{ player.QuickSpawnItem(new EntitySource_OverfullInventory(player), SuitAddons[1], SuitAddons[1].stack); SuitAddons[1].TurnToAir(true); }
				if (!SuitAddons[2].IsAir)
				{ player.QuickSpawnItem(new EntitySource_OverfullInventory(player), SuitAddons[2], SuitAddons[2].stack); SuitAddons[2].TurnToAir(true); }
			}
			#endregion
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<ChoziteGreaves>(1)
				.AddIngredient(SuitAddonLoader.GetAddon<SuitAddons.EnergyTank>().ItemType, 1)
				.AddRecipeGroup(MetroidMod.EvilBarRecipeGroupID, 15)
				.AddRecipeGroup(MetroidMod.EvilMaterialRecipeGroupID, 15)
				.AddTile(TileID.Anvils)
				.Register();
		}
		// no corresponding SaveData because we're attempting loading legacy data
		public override void LoadData(TagCompound tag)
		{
			try
			{
				if (!tag.ContainsKey("SuitAddons0")) { return; }
				// load pre-rework data since it's there
				for (int i = 0; i < 3; i++)
				{
					Item item = tag.Get<Item>("SuitAddons" + i);
					SuitAddons[i] = item;
				}
			}
			catch { }
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public class PowerSuitHelmet : ModItem
	{
		// Failsaves.
		private Item[] _suitAddons;
		public Item[] SuitAddons
		{
			get {
				if (_suitAddons == null)
				{
					_suitAddons = new Item[3];
					for (int i = 0; i < _suitAddons.Length; i++)
					{
						_suitAddons[i] = new Item();
						_suitAddons[i].TurnToAir();
					}
				}

				return _suitAddons;
			}
			set { _suitAddons = value; }
		}
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Power Suit Helmet");
			/* Tooltip.SetDefault("10% increased hunter damage\n" +
			"Emits light and grants improved night vision\n" +
			"30% increased underwater breathing"); */

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.rare = ItemRarityID.Green;
			Item.value = 6000;
			Item.defense = Common.Configs.MConfigItems.Instance.defensePowerSuitHelmet;
		}
		public override void UpdateEquip(Player player)
		{
			HunterDamagePlayer.ModPlayer(player).HunterDamageMult += 0.10f;
			player.nightVision = true;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.breathMult = 1.3f;
			mp.visorGlow = true;
			mp.IsPowerSuitHelmet = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<ChoziteHelmet>(1)
				.AddIngredient(SuitAddonLoader.GetAddon<SuitAddons.EnergyTank>().ItemType, 1)
				.AddRecipeGroup(MetroidMod.EvilBarRecipeGroupID, 10)
				.AddRecipeGroup(MetroidMod.EvilMaterialRecipeGroupID, 10)
				.AddTile(TileID.Anvils)
				.Register();
		}

		public override void OnCreated(ItemCreationContext context)
		{
			if (context is RecipeItemCreationContext)
			{
				Mod.Logger.Debug("HEY! MAKE DA SCAN VISOR");
				_suitAddons = new Item[3];
				for (int i = 0; i < _suitAddons.Length; i++)
				{
					_suitAddons[i] = new Item();
					_suitAddons[i].TurnToAir();
				}
				_suitAddons[0] = SuitAddonLoader.GetAddon<SuitAddons.ScanVisor>().ModItem.Item.Clone();
				//Mod.Logger.Debug(_suitAddons[0].ModItem.DisplayName.GetDefault());
			}
		}

		public override void SaveData(TagCompound tag)
		{
			for (int i = 0; i < SuitAddons.Length; ++i)
			{
				// Failsave check.
				if (SuitAddons[i] == null)
				{
					SuitAddons[i] = new Item();
				}
				tag.Add("SuitAddons" + i, ItemIO.Save(SuitAddons[i]));
			}
		}
		public override void LoadData(TagCompound tag)
		{
			try
			{
				SuitAddons = new Item[3];
				for (int i = 0; i < SuitAddons.Length; i++)
				{
					Item item = tag.Get<Item>("SuitAddons" + i);
					SuitAddons[i] = item;
				}
			}
			catch { }
		}
		public override void NetSend(BinaryWriter writer)
		{
			for (int i = 0; i < SuitAddons.Length; ++i)
			{
				ItemIO.Send(SuitAddons[i], writer);
			}
		}
		public override void NetReceive(BinaryReader reader)
		{
			for (int i = 0; i < SuitAddons.Length; ++i)
			{
				SuitAddons[i] = ItemIO.Receive(reader);
			}
		}

		public override ModItem Clone(Item newEntity)
		{
			PowerSuitHelmet obj = (PowerSuitHelmet)base.Clone(newEntity);
			obj.SuitAddons = SuitAddons;
			return obj;
		}

		public override void OnResearched(bool fullyResearched)
		{
			foreach (Item item in SuitAddons)
			{
				if (item == null || item.IsAir) { continue; }
				IEntitySource itemSource_OpenItem = Main.LocalPlayer.GetSource_OpenItem(Type);
				Main.LocalPlayer.QuickSpawnItem(itemSource_OpenItem, item, item.stack);
			}
		}
	}
}
