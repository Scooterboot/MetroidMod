using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MetroidMod.Common.Players;
using MetroidMod.ID;
using Terraria.ModLoader.IO;
using System.IO;

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
			DisplayName.SetDefault("Power Suit Breastplate");
			Tooltip.SetDefault("+15 overheat capacity\n" +
			"10% decreased overheat use");

			SacrificeTotal = 1;
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
			mp.IsPowerSuitBreastplate = true;
			if (Common.Configs.MConfigItems.Instance.enableLedgeClimbPowerSuitBreastplate)
			{
				mp.powerGrip = true;
			}
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
			mp.EnergyDefenseEfficiency += Common.Configs.MConfigItems.Instance.energyDefenseEfficiency;
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
			if(P.velocity.Y != 0f && ((P.controlRight && P.direction == 1) || (P.controlLeft && P.direction == -1) || mp.SMoveEffect > 0) && mp.shineDirection == 0 && !mp.shineActive && !mp.ballstate)
			{
				mp.jet = true;
			}
			else if(mp.shineDirection == 0 || mp.shineDirection == 5)
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
				SuitAddons = new Item[SuitAddonSlotID.Suit_Primary + 1];
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
				Main.LocalPlayer.QuickSpawnClonedItem(itemSource_OpenItem, item, item.stack);
			}
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class PowerSuitGreaves : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Power Suit Greaves");
			Tooltip.SetDefault("Allows somersaulting & wall jumping\n" +
			"Negates fall damage");

			SacrificeTotal = 1;
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
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<ChoziteGreaves>(1)
				.AddIngredient(SuitAddonLoader.GetAddon<SuitAddons.EnergyTank>().ItemType, 1)
				.AddRecipeGroup(MetroidMod.EvilBarRecipeGroupID, 15)
				.AddTile(TileID.Anvils)
				.Register();
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
			DisplayName.SetDefault("Power Suit Helmet");
			Tooltip.SetDefault("10% increased hunter damage\n" +
			"Emits light and grants improved night vision\n" +
			"30% increased underwater breathing");

			SacrificeTotal = 1;
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
				.AddTile(TileID.Anvils)
				.Register();
		}

		public override void OnCreate(ItemCreationContext context)
		{
			if (context is RecipeCreationContext)
			{
				Mod.Logger.Debug("HEY! MAKE DA SCAN VISOR");
				_suitAddons = new Item[3];
				for (int i = 0; i < _suitAddons.Length; i++)
				{
					_suitAddons[i] = new Item();
					_suitAddons[i].TurnToAir();
				}
				_suitAddons[0] = SuitAddonLoader.GetAddon<SuitAddons.ScanVisor>().ModItem.Item.Clone();
				Mod.Logger.Debug(_suitAddons[0].ModItem.DisplayName.GetDefault());
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
				Main.LocalPlayer.QuickSpawnClonedItem(itemSource_OpenItem, item, item.stack);
			}
		}
	}
}
