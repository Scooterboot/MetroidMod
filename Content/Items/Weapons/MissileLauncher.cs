using System;
using System.Collections.Generic;
using System.IO;
using MetroidMod.Common.GlobalItems;
using MetroidMod.Common.Players;
using MetroidMod.Content.DamageClasses;
using MetroidMod.Content.Items.MissileAddons;
using MetroidMod.Content.Items.MissileAddons.BeamCombos;
using MetroidMod.Content.Projectiles;
using MetroidMod.Content.Projectiles.missilecombo;
using MetroidMod.Content.Projectiles.missiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;
//using MetroidMod.Projectiles.chargelead;

namespace MetroidMod.Content.Items.Weapons
{
	public class MissileLauncher : ModItem
	{
		// Failsaves.
		private Item[] _missileMods;
		private Item[] _missileChange;
		public Item[] MissileMods
		{
			get {
				if (_missileMods == null)
				{
					_missileMods = new Item[MetroidMod.missileSlotAmount];
					for (int i = 0; i < _missileMods.Length; ++i)
					{
						_missileMods[i] = new Item();
						_missileMods[i].TurnToAir();
					}
				}

				return _missileMods;
			}
			set { _missileMods = value; }
		}
		public Item[] MissileChange
		{
			get {
				if (_missileChange == null)
				{
					_missileChange = new Item[13];
					for (int i = 0; i < _missileChange.Length; ++i)
					{
						_missileChange[i] = new Item();
						_missileChange[i].TurnToAir();
					}
				}

				return _missileChange;
			}
			set { _missileChange = value; }
		}
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Missile Launcher");
			// Tooltip.SetDefault("Select this item in your hotbar and open your inventory to open the Missile Addon UI");

			Item.ResearchUnlockCount = 1;
		}
		//public override void SetDefaults()
		public override void SetDefaults()
		{
			Item.damage = Common.Configs.MConfigItems.Instance.damageMissileLauncher;
			Item.DamageType = ModContent.GetInstance<HunterDamageClass>();
			Item.width = 24;
			Item.height = 16;
			Item.scale = 0.8f;
			Item.useTime = Common.Configs.MConfigItems.Instance.useTimeMissileLauncher;
			Item.useAnimation = Common.Configs.MConfigItems.Instance.useTimeMissileLauncher;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 5.5f;
			Item.value = 20000;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = Sounds.Items.Weapons.MissileShoot;
			Item.autoReuse = false;
			Item.shoot = ModContent.ProjectileType<MissileShot>();
			Item.shootSpeed = 8f;
			Item.crit = 10;

			MGlobalItem mi = Item.GetGlobalItem<MGlobalItem>();
			mi.statMissiles = Common.Configs.MConfigItems.Instance.ammoMissileLauncher;
			mi.maxMissiles = Common.Configs.MConfigItems.Instance.ammoMissileLauncher;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<Miscellaneous.ChoziteBar>(10)
				.AddIngredient(SuitAddonLoader.GetAddon<SuitAddons.EnergyTank>().ItemType, 1)
				.AddTile(TileID.Anvils)
				.Register();
		}

		public override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			player.itemLocation.X = player.MountedCenter.X - (float)Item.width * 0.5f;
			player.itemLocation.Y = player.MountedCenter.Y - (float)Item.height * 0.5f;
		}

		public override bool CanUseItem(Player player)
		{
			if (!Item.TryGetGlobalItem(out MGlobalItem mi) || player.whoAmI == Main.myPlayer && Item.type == Main.mouseItem.type)
			{
				return false;
			}
			return player.whoAmI == Main.myPlayer && mi.statMissiles > 0;
		}

		public override int ChoosePrefix(UnifiedRandom rand)
		{
			int output = Item.prefix;
			switch (rand.Next(14))
			{
				case 0: output = 36; break;
				case 1: output = 37; break;
				case 2: output = 38; break;
				case 3: output = 53; break;
				case 4: output = 54; break;
				case 5: output = 55; break;
				case 6: output = 39; break;
				case 7: output = 40; break;
				case 8: output = 56; break;
				case 9: output = 41; break;
				case 10: output = 57; break;
				case 11: output = 59; break;
				case 12: output = 60; break;
				case 13: output = 61; break;
			}
			//PrefixLoader.Roll(Item, ref output, 14, rand, new PrefixCategory[] { PrefixCategory.AnyWeapon, PrefixCategory.Custom });
			return output;
		}

		public override void OnResearched(bool fullyResearched)
		{
			foreach (Item item in MissileMods)
			{
				if (item == null || item.IsAir) { continue; }
				IEntitySource itemSource_OpenItem = Main.LocalPlayer.GetSource_OpenItem(Type);
				Main.LocalPlayer.QuickSpawnItem(itemSource_OpenItem, item, item.stack);
			}
			foreach (Item item in MissileChange)
			{
				if (item == null || item.IsAir) { continue; }
				IEntitySource itemSource_OpenItem = Main.LocalPlayer.GetSource_OpenItem(Type);
				Main.LocalPlayer.QuickSpawnItem(itemSource_OpenItem, item, item.stack);
			}
		}

		public override bool CanReforge()/* tModPorter Note: Use CanReforge instead for logic determining if a reforge can happen. */
		{
			foreach (Item item in MissileMods)
			{
				if (item == null || item.IsAir) { continue; }
				IEntitySource itemSource_OpenItem = Main.LocalPlayer.GetSource_OpenItem(Type);
				Main.LocalPlayer.QuickSpawnItem(itemSource_OpenItem, item, item.stack);
			}
			MissileMods = new Item[5];
			foreach (Item item in MissileChange)
			{
				if (item == null || item.IsAir) { continue; }
				IEntitySource itemSource_OpenItem = Main.LocalPlayer.GetSource_OpenItem(Type);
				Main.LocalPlayer.QuickSpawnItem(itemSource_OpenItem, item, item.stack);
			}
			MissileChange = new Item[5];
			return base.CanReforge();
		}

		int finalDmg = 0;

		int useTime = Common.Configs.MConfigItems.Instance.useTimeMissileLauncher;

		string shot = "MissileShot";
		string chargeShot = "DiffusionMissileShot";
		string shotSound = "MissileShoot";
		string chargeShotSound = "SuperMissileSound";
		string chargeUpSound = "ChargeStartup_Power";
		string chargeTex = "ChargeLead_PlasmaRed";
		int dustType = 6;
		Color dustColor = default(Color);
		Color lightColor = MetroidMod.plaRedColor;

		float comboKnockBack = 5.5f;

		bool isHoming = false;
		bool isCharge = false;
		bool isSeeker = false;
		int isHeldCombo = 0;
		int chargeCost = 5;
		int comboSound = 0;
		float comboDrain = 5f;
		bool noSomersault = false;
		bool useFlameSounds = false;
		bool useVortexSounds = false;

		bool isShotgun = false;
		int shotgunAmt = 5;

		bool isMiniGun = false;
		int miniRateIncr = 2;
		int miniGunCostReduct = 2;
		int miniGunAmt = 1;

		int comboUseTime = 4;
		int comboCostUseTime = 12;
		int comboShotAmt = 1;
		float chargeMult = 1f;

		float leadAimSpeed = 0f;

		string altTexture => this.Texture + "_alt";
		string texture = "";

		public override void UpdateInventory(Player P)
		{
			if (Item == null || !Item.TryGetGlobalItem(out MGlobalItem mi)) { return; }
			MPlayer mp = P.GetModPlayer<MPlayer>();

			int ic = ModContent.ItemType<IceMissileAddon>();
			int sm = ModContent.ItemType<SuperMissileAddon>();
			int icSm = ModContent.ItemType<IceSuperMissileAddon>();
			int st = ModContent.ItemType<StardustMissileAddon>();
			int ne = ModContent.ItemType<NebulaMissileAddon>();

			int se = ModContent.ItemType<SeekerMissileAddon>();

			Item slot1 = MissileMods[0];
			Item slot2 = MissileMods[1];
			Item exp = MissileMods[2];

			int damage = Common.Configs.MConfigItems.Instance.damageMissileLauncher;
			useTime = Common.Configs.MConfigItems.Instance.useTimeMissileLauncher;
			shot = "MissileShot";
			chargeShot = "";
			shotSound = "MissileShoot";
			chargeShotSound = "SuperMissileSound";
			chargeUpSound = "";
			chargeTex = "";
			dustType = 0;
			dustColor = default(Color);
			lightColor = Color.White;

			texture = "";

			comboKnockBack = Item.knockBack;

			isSeeker = (slot1.type == se);
			isCharge = (!slot1.IsAir && !isSeeker);
			isHeldCombo = 0;
			chargeCost = 5;
			comboSound = 0;
			comboDrain = 5f;
			noSomersault = false;
			useFlameSounds = false;
			useVortexSounds = false;

			isHoming = false;
			isShotgun = false;
			shotgunAmt = 5;

			isMiniGun = false;
			miniRateIncr = 2;
			miniGunCostReduct = 2;
			miniGunAmt = 1;

			comboUseTime = 4;
			comboCostUseTime = 12;
			comboShotAmt = 1;

			leadAimSpeed = 0f;

			mi.maxMissiles = Common.Configs.MConfigItems.Instance.ammoMissileLauncher + (Common.Configs.MConfigItems.Instance.ammoMissileTank * exp.stack);
			if (mi.statMissiles > mi.maxMissiles)
			{
				mi.statMissiles = mi.maxMissiles;
			}

			// Default Combos

			if (slot2.type == sm)
			{
				shot = "SuperMissileShot";
				shotSound = "SuperMissileShoot";
			}
			else if (slot2.type == ic)
			{
				shot = "IceMissileShot";
				shotSound = "MissileShoot";
			}
			else if (slot2.type == icSm)
			{
				shot = "IceSuperMissileShot";
				shotSound = "IceMissileShoot";
			}
			else if (slot2.type == st)
			{
				shot = "StardustMissileShot";
				shotSound = "SuperMissileShoot";
			}
			else if (slot2.type == ne)
			{
				shot = "NebulaMissileShot";
				shotSound = "SuperMissileShoot";
			}

			int wb = ModContent.ItemType<WavebusterAddon>();
			int icSp = ModContent.ItemType<IceSpreaderAddon>();
			int sp = ModContent.ItemType<SpazerComboAddon>();
			int ft = ModContent.ItemType<FlamethrowerAddon>();
			int pl = ModContent.ItemType<PlasmaMachinegunAddon>();
			int nv = ModContent.ItemType<NovaComboAddon>();
			int hm = ModContent.ItemType<HomingMissileAddon>();

			int di = ModContent.ItemType<DiffusionMissileAddon>();

			// Charge Combos
			if (slot1.type == wb)
			{
				isHeldCombo = 1;
				comboSound = 1;
				noSomersault = true;
				chargeShot = "WavebusterShot";
				chargeUpSound = "ChargeStartup_Wave";
				chargeTex = "ChargeLead_WaveV2";
				dustType = 62;
				lightColor = MetroidMod.waveColor2;
				comboKnockBack = 0f;
				texture = "Wavebuster";
			}
			if (slot1.type == hm)
			{
				isHoming = true;
				chargeShot = shot;
				chargeUpSound = "ChargeStartup_HomingMissile";
				chargeShotSound = "HomingMissileShoot";
				chargeTex = "ChargeLead_Spazer";
				dustType = 64;
				lightColor = MetroidMod.powColor;
				texture = "SpazerCombo";
			}
			if (slot1.type == icSp)
			{
				chargeShot = "IceSpreaderShot";
				chargeShotSound = "IceSpreaderSound";
				chargeUpSound = "ChargeStartup_Ice";
				chargeTex = "ChargeLead_Ice";
				dustType = 59;
				lightColor = MetroidMod.iceColor;
				texture = "IceSpreader";
			}
			if (slot1.type == sp)
			{
				isShotgun = true;
				chargeShot = shot;
				chargeUpSound = "ChargeStartup_Power";
				chargeTex = "ChargeLead_Spazer";
				dustType = 64;
				lightColor = MetroidMod.powColor;
				texture = "SpazerCombo";
			}
			if (slot1.type == ft)
			{
				isHeldCombo = 2;
				comboSound = 1;
				noSomersault = true;
				useFlameSounds = true;
				chargeShot = "FlamethrowerShot";
				chargeUpSound = "ChargeStartup_PlasmaRed";
				chargeTex = "ChargeLead_PlasmaRed";
				dustType = 6;
				lightColor = MetroidMod.plaRedColor;
				texture = "Flamethrower";
			}
			if (slot1.type == pl)
			{
				isHeldCombo = 2;
				comboSound = 2;
				noSomersault = true;
				isMiniGun = true;
				chargeShot = "PlasmaMachinegunShot";
				chargeShotSound = "PlasmaMachinegunSound";
				chargeUpSound = "ChargeStartup_Power";
				chargeTex = "ChargeLead_PlasmaGreen";
				dustType = 61;
				lightColor = MetroidMod.plaGreenColor;
				texture = "PlasmaMachinegun";
			}
			if (slot1.type == nv)
			{
				isHeldCombo = 1;
				comboSound = 1;
				noSomersault = true;
				leadAimSpeed = 0.85f;
				chargeShot = "NovaLaserShot";
				chargeUpSound = "ChargeStartup_Nova";
				chargeTex = "ChargeLead_Nova";
				dustType = 75;
				lightColor = MetroidMod.novColor;
				texture = "NovaLaser";
			}

			if (slot1.type == di)
			{
				chargeShot = "DiffusionMissileShot";
				chargeUpSound = "ChargeStartup_Power";
				chargeTex = "ChargeLead_PlasmaRed";
				dustType = 6;
				lightColor = MetroidMod.plaRedColor;
				texture = "DiffusionMissile";

				if (slot2.type == ic || slot2.type == icSm)
				{
					chargeShot = "IceDiffusionMissileShot";
					chargeUpSound = "ChargeStartup_Ice";
					chargeTex = "ChargeLead_Ice";
					dustType = 135;
					lightColor = MetroidMod.iceColor;
				}
				if (slot2.type == st)
				{
					chargeShot = "StardustDiffusionMissileShot";
					chargeUpSound = "ChargeStartup_Ice";
					chargeTex = "ChargeLead_Stardust";
					dustType = 87;
					lightColor = MetroidMod.iceColor;
				}
				if (slot2.type == ne)
				{
					chargeShot = "NebulaDiffusionMissileShot";
					chargeUpSound = "ChargeStartup_Wave";
					chargeTex = "ChargeLead_Nebula";
					dustType = 255;
					lightColor = MetroidMod.waveColor;
				}
			}
			if (isSeeker)
			{
				texture = "SeekerMissile";
			}

			int sd = ModContent.ItemType<StardustComboAddon>();
			int nb = ModContent.ItemType<NebulaComboAddon>();
			int vt = ModContent.ItemType<VortexComboAddon>();
			int sl = ModContent.ItemType<SolarComboAddon>();

			if (slot1.type == sd)
			{
				chargeShot = "StardustComboShot";
				chargeShotSound = "IceSpreaderSound";
				chargeUpSound = "ChargeStartup_Ice";
				chargeTex = "ChargeLead_Stardust";
				dustType = 87;
				lightColor = MetroidMod.iceColor;
				texture = "StardustCombo";
			}
			if (slot1.type == nb)
			{
				isHeldCombo = 1;
				comboSound = 1;
				noSomersault = true;
				chargeShot = "NebulaComboShot";
				chargeUpSound = "ChargeStartup_Wave";
				chargeTex = "ChargeLead_Nebula";
				dustType = 255;
				lightColor = MetroidMod.waveColor;
				texture = "NebulaCombo";
			}
			if (slot1.type == vt)
			{
				isHeldCombo = 2;
				comboSound = 2;
				noSomersault = true;

				comboUseTime = 10;
				comboShotAmt = 6;

				useVortexSounds = true;

				chargeShot = "VortexComboShot";
				chargeShotSound = "PlasmaMachinegunSound";
				chargeUpSound = "ChargeStartup_Power";
				chargeTex = "ChargeLead_Vortex";
				dustType = 229;
				lightColor = MetroidMod.lumColor;
				texture = "VortexCombo";
			}
			if (slot1.type == sl)
			{
				isHeldCombo = 1;
				comboSound = 1;
				noSomersault = true;
				leadAimSpeed = 0.9f;
				chargeShot = "SolarLaserShot";
				chargeUpSound = "ChargeStartup_PlasmaRed";
				chargeTex = "ChargeLead_SolarCombo";
				dustType = 6;
				lightColor = MetroidMod.plaRedColor;
				texture = "SolarCombo";
			}

			if (!slot1.IsAir)
			{
				MGlobalItem mItem = slot1.GetGlobalItem<MGlobalItem>();
				chargeMult = mItem.addonChargeDmg;
				chargeCost = mItem.addonMissileCost;
				comboDrain = mItem.addonMissileDrain;
			}
			comboCostUseTime = (int)Math.Round(60.0 / (double)comboDrain);

			float addonDmg = 0f;
			float addonSpeed = 0f;
			if (!slot2.IsAir)
			{
				MGlobalItem mItem = slot2.GetGlobalItem<MGlobalItem>();
				addonDmg = mItem.addonDmg;
				addonSpeed = mItem.addonSpeed;
			}
			finalDmg = (int)Math.Round((double)((float)damage * (1f + addonDmg)));

			float shotsPerSecond = (60f / useTime) * (1f + addonSpeed);
			useTime = (int)Math.Max(Math.Round(60.0 / (double)shotsPerSecond), 2);

			Item.damage = finalDmg;
			Item.useTime = useTime;
			Item.useAnimation = useTime;
			Item.shoot = Mod.Find<ModProjectile>(shot).Type;
			Item.UseSound = null;//mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/"+shotSound);

			Item.shootSpeed = 8f;
			Item.reuseDelay = 0;
			Item.mana = 0;
			Item.knockBack = 5.5f;
			Item.scale = 0.8f;
			Item.crit = 10;
			Item.value = 20000;

			Item.rare = ItemRarityID.Green;

			Item.Prefix(Item.prefix);
		}
		public override bool PreDrawInWorld(SpriteBatch sb, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			if (Item == null || !Item.TryGetGlobalItem(out MGlobalItem mi)) { return true; }
			Texture2D tex = Terraria.GameContent.TextureAssets.Item[Type].Value;//Main.itemTexture[Item.type];
			SetTexture(mi);
			if (mi.itemTexture != null)
			{
				tex = mi.itemTexture;
			}
			float num5 = (float)(Item.height - tex.Height);
			float num6 = (float)(Item.width / 2 - tex.Width / 2);
			sb.Draw(tex, new Vector2(Item.position.X - Main.screenPosition.X + (float)(tex.Width / 2) + num6, Item.position.Y - Main.screenPosition.Y + (float)(tex.Height / 2) + num5 + 2f),
			new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), alphaColor, rotation, new Vector2((float)(tex.Width / 2), (float)(tex.Height / 2)), scale, SpriteEffects.None, 0f);
			return false;
		}
		public override bool PreDrawInInventory(SpriteBatch sb, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			if (Item == null || !Item.TryGetGlobalItem(out MGlobalItem mi)) { return true; }
			Texture2D tex = Terraria.GameContent.TextureAssets.Item[Type].Value;//Main.itemTexture[Item.type];
			SetTexture(mi);
			if (mi.itemTexture != null)
			{
				tex = mi.itemTexture;
			}
			sb.Draw(tex, position, new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
			return false;
		}
		void SetTexture(MGlobalItem mi)
		{
			if (texture != "")
			{
				string alt = "";
				if (MetroidMod.UseAltWeaponTextures)
				{
					alt = "_alt";
				}
				mi.itemTexture = ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/MissileLauncher/{texture + alt}").Value;// + "/" + texture).Value;
			}
			else
			{
				if (MetroidMod.UseAltWeaponTextures)
				{
					mi.itemTexture = ModContent.Request<Texture2D>(altTexture).Value;
				}
				else
				{
					mi.itemTexture = Terraria.GameContent.TextureAssets.Item[Type].Value;//Main.itemTexture[Item.type];
				}
			}

			if (mi.itemTexture != null)
			{
				Item.width = mi.itemTexture.Width;
				Item.height = mi.itemTexture.Height;
			}
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			base.ModifyTooltips(tooltips);

			Player P = Main.player[Main.myPlayer];
			MPlayer mp = P.GetModPlayer<MPlayer>();

			if (Item == Main.HoverItem)
			{
				Item.ModItem.UpdateInventory(Main.player[Main.myPlayer]);
			}

			int cost = (int)((float)chargeCost * (mp.missileCost + 0.001f));
			string ch = "Charge shot consumes " + cost + " missiles";
			if (isHeldCombo > 0)
			{
				ch = "Charge initially costs " + cost + " missiles";
			}
			TooltipLine mCost = new(Mod, "ChargeMissileCost", ch);

			float drain = (float)Math.Round(comboDrain * mp.missileCost, 2);
			TooltipLine mDrain = new(Mod, "ChargeMissileDrain", "Drains " + drain + " missiles per second");

			for (int k = 0; k < tooltips.Count; k++)
			{
				if (tooltips[k].Name == "Knockback" && !MissileMods[0].IsAir && !isSeeker)
				{
					tooltips.Insert(k + 1, mCost);
					if (isHeldCombo > 0)
					{
						tooltips.Insert(k + 2, mDrain);
					}
				}

				if (tooltips[k].Name == "PrefixDamage")
				{
					double num19 = (double)((float)Item.damage - (float)finalDmg);
					num19 = num19 / (double)((float)finalDmg) * 100.0;
					num19 = Math.Round(num19);
					if (num19 > 0.0)
					{
						tooltips[k].Text = "+" + num19 + Lang.tip[39].Value;
					}
					else
					{
						tooltips[k].Text = num19 + Lang.tip[39].Value;
					}
				}
				if (tooltips[k].Name == "PrefixSpeed")
				{
					double num20 = (double)((float)Item.useAnimation - (float)useTime);
					num20 = num20 / (double)((float)useTime) * 100.0;
					num20 = Math.Round(num20);
					num20 *= -1.0;
					if (num20 > 0.0)
					{
						tooltips[k].Text = "+" + num20 + Lang.tip[40].Value;
					}
					else
					{
						tooltips[k].Text = num20 + Lang.tip[40].Value;
					}
				}
			}
		}

		int chargeLead = -1;
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockBack)
		{
			float speedX = velocity.X;
			float speedY = velocity.Y;
			if (!Item.TryGetGlobalItem(out MGlobalItem mi)) { return true; }
			if (isCharge)
			{
				int ch = Projectile.NewProjectile(source, position.X, position.Y, speedX, speedY, ModContent.ProjectileType<ChargeLead>(), damage, knockBack, player.whoAmI);
				ChargeLead cl = (ChargeLead)Main.projectile[ch].ModProjectile;
				cl.ChargeUpSound = chargeUpSound;
				cl.ChargeTex = chargeTex;
				cl.DustType = dustType;
				cl.DustColor = dustColor;
				cl.LightColor = lightColor;
				cl.ShotSound = shotSound;
				cl.ChargeShotSound = chargeShotSound;
				cl.Projectile.netUpdate = true;
				cl.missile = true;
				cl.comboSound = comboSound;
				cl.noSomersault = noSomersault;
				cl.aimSpeed = leadAimSpeed;

				chargeLead = ch;
				return false;
			}
			else if (isSeeker)
			{
				int ch = Projectile.NewProjectile(source, position.X, position.Y, speedX, speedY, ModContent.ProjectileType<SeekerMissileLead>(), damage, knockBack, player.whoAmI);
				chargeLead = ch;
				return false;
			}
			else
			{
				mi.statMissiles -= 1;
				SoundEngine.PlaySound(new($"{Mod.Name}/Assets/Sounds/{shotSound}"), player.position);
			}
			return true;
		}

		bool LeadActive(Player player, int type)
		{
			return (chargeLead != -1 && Main.projectile[chargeLead].active && Main.projectile[chargeLead].owner == player.whoAmI && Main.projectile[chargeLead].type == type);
		}

		bool initialShot = false;
		int comboTime = 0;
		int comboCostTime = 0;
		float scalePlus = 0f;
		int targetingDelay = 0;
		int targetNum = 0;
		public override void HoldItem(Player player)
		{
			if (player.whoAmI == Main.myPlayer && Item.TryGetGlobalItem(out MGlobalItem mi))
			{
				MPlayer mp = player.GetModPlayer<MPlayer>();

				int chCost = (int)((float)chargeCost * (mp.missileCost + 0.001f));
				comboCostUseTime = (int)Math.Round(60.0 / (double)(comboDrain * mp.missileCost));
				isCharge &= (mi.statMissiles >= chCost || (isHeldCombo > 0 && initialShot));

				Item.autoReuse = (isCharge || isSeeker);

				if (isCharge)
				{
					if (!mp.ballstate && !mp.shineActive && !player.dead && !player.noItems)
					{
						Vector2 oPos = player.RotatedRelativePoint(player.MountedCenter, true);

						float MY = Main.mouseY + Main.screenPosition.Y;
						float MX = Main.mouseX + Main.screenPosition.X;
						if (player.gravDir == -1f)
							MY = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;

						float targetrotation = (float)Math.Atan2((MY - oPos.Y), (MX - oPos.X));

						Vector2 velocity = targetrotation.ToRotationVector2() * Item.shootSpeed;

						float dmgMult = chargeMult;
						int damage = player.GetWeaponDamage(Item);

						//if (player.controlUseItem && chargeLead != -1 && Main.projectile[chargeLead].active && Main.projectile[chargeLead].owner == player.whoAmI && Main.projectile[chargeLead].type == mod.ProjectileType("ChargeLead"))
						if (player.controlUseItem && LeadActive(player, ModContent.ProjectileType<ChargeLead>()))
						{
							if (mp.statCharge < MPlayer.maxCharge)
							{
								mp.statCharge = Math.Min(mp.statCharge + 1, MPlayer.maxCharge);
							}
							if (isHeldCombo > 0)
							{
								if (mi.statMissiles > 0)
								{
									if (mp.statCharge >= MPlayer.maxCharge)
									{
										if (isMiniGun)
										{
											this.MiniGunShoot(player, Item, Main.projectile[chargeLead], Mod.Find<ModProjectile>(chargeShot).Type, (int)((float)damage * dmgMult), comboKnockBack, chargeShotSound);
										}
										else
										{
											if (comboTime <= 0)
											{
												var entitySource = player.GetSource_ItemUse(Item);
												for (int i = 0; i < comboShotAmt; i++)
												{
													int proj = Projectile.NewProjectile(entitySource, oPos.X, oPos.Y, velocity.X, velocity.Y, Mod.Find<ModProjectile>(chargeShot).Type, (int)((float)damage * dmgMult), comboKnockBack, player.whoAmI);
													Main.projectile[proj].ai[0] = chargeLead;
												}
												comboTime = comboUseTime;
											}

											if (isHeldCombo == 2 && comboTime > 0)
											{
												comboTime--;
											}
										}

										if (!initialShot)
										{
											if (useFlameSounds || useVortexSounds)
											{
												int type = ModContent.ProjectileType<FlamethrowerLead>();
												if (useVortexSounds)
												{
													type = ModContent.ProjectileType<VortexComboLead>();
												}
												int proj = Projectile.NewProjectile(player.GetSource_ItemUse(Item), oPos.X, oPos.Y, velocity.X, velocity.Y, type, 0, 0, player.whoAmI);
												Main.projectile[proj].ai[0] = chargeLead;
											}

											mi.statMissiles = Math.Max(mi.statMissiles - chCost, 0);

											initialShot = true;
										}

										if (comboCostUseTime > 0)
										{
											//if(comboCostTime <= 0)
											if (comboCostTime > comboCostUseTime)
											{
												mi.statMissiles = Math.Max(mi.statMissiles - 1, 0);
												//comboCostTime = comboCostUseTime;
												comboCostTime = 0;
											}
											else
											{
												//comboCostTime--;
												comboCostTime++;
											}
										}
									}
								}
								else
								{
									Main.projectile[chargeLead].Kill();
								}
							}
						}
						else
						{
							if (mp.statCharge <= 0 && LeadActive(player, ModContent.ProjectileType<ChargeLead>()))
							{
								mp.statCharge++;
							}
							if (isHeldCombo <= 0 || mp.statCharge < MPlayer.maxCharge)
							{
								if (mp.statCharge >= MPlayer.maxCharge && mi.statMissiles >= chCost)
								{
									if (isShotgun)
									{
										var entitySource = player.GetSource_ItemUse(Item);
										for (int i = 0; i < shotgunAmt; i++)
										{
											int k = i - (shotgunAmt / 2);
											Vector2 shotGunVel = Vector2.Normalize(velocity) * (Item.shootSpeed + 4f);
											double rot = Angle.ConvertToRadians(4.0 * k);
											shotGunVel = shotGunVel.RotatedBy(rot, default(Vector2));
											if (float.IsNaN(shotGunVel.X) || float.IsNaN(shotGunVel.Y))
											{
												shotGunVel = -Vector2.UnitY;
											}
											int chargeProj = Projectile.NewProjectile(entitySource, oPos.X, oPos.Y, shotGunVel.X, shotGunVel.Y, Mod.Find<ModProjectile>(chargeShot).Type, (int)((float)damage * dmgMult), Item.knockBack, player.whoAmI);
										}
									}
									if (isHoming)
									{
										var entitySource = player.GetSource_ItemUse(Item);
										int shotProj = Projectile.NewProjectile(entitySource, oPos.X, oPos.Y, velocity.X, velocity.Y, Mod.Find<ModProjectile>(shot).Type, damage * 2, Item.knockBack, player.whoAmI);
										MProjectile mProj = (MProjectile)Main.projectile[shotProj].ModProjectile;
										mProj.homing = true;
										mProj.Projectile.netUpdate2 = true;
										mi.statMissiles = Math.Max(mi.statMissiles - 1, 0);
									}
									else
									{
										var entitySource = player.GetSource_ItemUse(Item);
										int chargeProj = Projectile.NewProjectile(entitySource, oPos.X, oPos.Y, velocity.X, velocity.Y, Mod.Find<ModProjectile>(chargeShot).Type, (int)((float)damage * dmgMult), Item.knockBack, player.whoAmI);
									}
									mi.statMissiles -= chCost;
								}
								else if (mp.statCharge > 0)
								{
									var entitySource = player.GetSource_ItemUse(Item);
									int shotProj = Projectile.NewProjectile(entitySource, oPos.X, oPos.Y, velocity.X, velocity.Y, Mod.Find<ModProjectile>(shot).Type, damage, Item.knockBack, player.whoAmI);
									mi.statMissiles -= 1;
								}
							}

							if (!LeadActive(player, ModContent.ProjectileType<ChargeLead>()))
							{
								mp.statCharge = 0;
							}

							comboTime = 0;
							comboCostTime = 0;
							scalePlus = 0f;
							initialShot = false;
						}
					}
					else if (!mp.ballstate)
					{
						mp.statCharge = 0;
						comboTime = 0;
						comboCostTime = 0;
						scalePlus = 0f;
						initialShot = false;
					}
				}
				else
				{
					mp.statCharge = 0;
					comboTime = 0;
					comboCostTime = 0;
					scalePlus = 0f;
					initialShot = false;
				}

				if (targetingDelay > 0)
					targetingDelay--;

				if (isSeeker && !mp.ballstate && !mp.shineActive && !player.dead && !player.noItems)
				{
					Vector2 oPos = player.RotatedRelativePoint(player.MountedCenter, true);
					float MY = Main.mouseY + Main.screenPosition.Y;
					float MX = Main.mouseX + Main.screenPosition.X;
					if (player.gravDir == -1f)
					{
						MY = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
					}
					Rectangle mouse = new Rectangle((int)MX - 1, (int)MY - 1, 2, 2);
					float targetrotation = (float)Math.Atan2((MY - oPos.Y), (MX - oPos.X));
					Vector2 velocity = targetrotation.ToRotationVector2() * Item.shootSpeed;
					int damage = player.GetWeaponDamage(Item);
					//if (player.controlUseItem && chargeLead != -1 && Main.projectile[chargeLead].active && Main.projectile[chargeLead].owner == player.whoAmI && Main.projectile[chargeLead].type == mod.ProjectileType("SeekerMissileLead"))
					if (player.controlUseItem && LeadActive(player, ModContent.ProjectileType<SeekerMissileLead>()))
					{
						if (mi.seekerCharge < MGlobalItem.seekerMaxCharge)
						{
							mi.seekerCharge = Math.Min(mi.seekerCharge + 1, MGlobalItem.seekerMaxCharge);
						}
						else
						{
							for (int i = 0; i < Main.maxNPCs; i++)
							{
								NPC npc = Main.npc[i];
								if (npc.active && npc.chaseable && !npc.dontTakeDamage && !npc.friendly)// && !npc.immortal)
								{
									Rectangle npcRect = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);
									bool flag = false;
									for (int j = 0; j < mi.seekerTarget.Length; j++)
									{
										if (mi.seekerTarget[j] == npc.whoAmI)
										{
											flag = true;
										}
									}

									Vector2 delta = new Vector2(MX, MY);
									delta.X -= MathHelper.Clamp(MX, npcRect.X, npcRect.X + npcRect.Width);
									delta.Y -= MathHelper.Clamp(MY, npcRect.Y, npcRect.Y + npcRect.Height);
									bool colFlag = (delta.Length() < 50);
									if (colFlag && mi.seekerTarget[targetNum] <= -1 && ((targetingDelay <= 0 && mouse.Intersects(npcRect)) || !flag) && mi.statMissiles > mi.numSeekerTargets)
									{
										mi.seekerTarget[targetNum] = npc.whoAmI;
										targetNum++;
										if (targetNum > 4)
										{
											targetNum = 0;
										}
										targetingDelay = 40;
										SoundEngine.PlaySound(Sounds.Items.Weapons.SeekerLockSound, oPos);
									}
								}
							}

							int num = 10;
							while (mi.seekerTarget[targetNum] > -1 && num > 0)
							{
								targetNum++;
								if (targetNum > 4)
								{
									targetNum = 0;
								}
								num--;
							}

							mi.numSeekerTargets = 0;
							for (int i = 0; i < mi.seekerTarget.Length; i++)
							{
								if (mi.seekerTarget[i] > -1)
								{
									mi.numSeekerTargets++;

									if (!Main.npc[mi.seekerTarget[i]].active)
									{
										mi.seekerTarget[i] = -1;
									}
								}
							}
						}
					}
					else
					{
						if (mi.seekerCharge <= 0 && LeadActive(player, ModContent.ProjectileType<SeekerMissileLead>()))
						{
							mi.seekerCharge++;
						}
						if (mi.seekerCharge >= MGlobalItem.seekerMaxCharge && mi.numSeekerTargets > 0)
						{
							var entitySource = player.GetSource_ItemUse(Item);
							for (int i = 0; i < mi.seekerTarget.Length; i++)
							{
								if (mi.seekerTarget[i] > -1)
								{
									int shotProj = Projectile.NewProjectile(entitySource, oPos.X, oPos.Y, velocity.X, velocity.Y, Mod.Find<ModProjectile>(shot).Type, damage, Item.knockBack, player.whoAmI);
									MProjectile mProj = (MProjectile)Main.projectile[shotProj].ModProjectile;
									mProj.seekTarget = mi.seekerTarget[i];
									mProj.seeking = true;
									mProj.Projectile.netUpdate2 = true;
									mi.statMissiles = Math.Max(mi.statMissiles - 1, 0);
								}
							}

							SoundEngine.PlaySound(Sounds.Items.Weapons.SeekerMissileSound, oPos);
						}
						else if (mi.seekerCharge > 0)
						{
							var entitySource = player.GetSource_ItemUse(Item);
							int shotProj = Projectile.NewProjectile(entitySource, oPos.X, oPos.Y, velocity.X, velocity.Y, Mod.Find<ModProjectile>(shot).Type, damage, Item.knockBack, player.whoAmI);
							SoundEngine.PlaySound(new($"{Mod.Name}/Assets/Sounds/{shotSound}"), oPos);

							mi.statMissiles -= 1;
						}
						if (!LeadActive(player, ModContent.ProjectileType<SeekerMissileLead>()))
						{
							mi.seekerCharge = 0;
						}
						mi.numSeekerTargets = 0;
						for (int k = 0; k < mi.seekerTarget.Length; k++)
						{
							mi.seekerTarget[k] = -1;
						}
						targetNum = 0;
						targetingDelay = 0;
					}
				}
				else
				{
					mi.seekerCharge = 0;
					mi.numSeekerTargets = 0;
					for (int k = 0; k < mi.seekerTarget.Length; k++)
					{
						mi.seekerTarget[k] = -1;
					}
					targetNum = 0;
					targetingDelay = 0;
				}
			}
		}
		int waveDir = 1;
		SoundEffectInstance soundInstance;
		public void MiniGunShoot(Player player, Item item, Projectile Lead, int projType, int damage, float knockBack, string sound)
		{
			if (comboTime <= 0)
			{
				SoundEngine.TryGetActiveSound(SoundEngine.PlaySound(new($"{Mod.Name}/Assets/Sounds/{sound}"), player.Center), out ActiveSound result);
				soundInstance = result.Sound;
				if (soundInstance != null)
				{
					soundInstance.Volume *= 1f - 0.25f * (scalePlus / 20f);
				}

				float spray = 1f * (scalePlus / 20f);

				float scaleFactor2 = 14f;

				var entitySource = Lead.GetSource_FromAI();
				for (int i = 0; i < miniGunAmt; i++)
				{
					float rot = Lead.velocity.ToRotation() + (float)Angle.ConvertToRadians(Main.rand.Next(18) * 10) - (float)Math.PI / 2f;
					Vector2 vector3 = Lead.Center + rot.ToRotationVector2() * 7f * spray;
					Vector2 vector5 = Vector2.Normalize(Lead.velocity) * scaleFactor2;
					vector5 = vector5.RotatedBy((Main.rand.NextDouble() * 0.12 - 0.06) * spray, default(Vector2));
					if (float.IsNaN(vector5.X) || float.IsNaN(vector5.Y))
					{
						vector5 = -Vector2.UnitY;
					}
					int proj = Projectile.NewProjectile(entitySource, vector3.X, vector3.Y, vector5.X, vector5.Y, projType, damage, knockBack, player.whoAmI, 0f, 0f);
					Main.projectile[proj].ai[0] = Lead.whoAmI;
					MProjectile mProj = (MProjectile)Main.projectile[proj].ModProjectile;
					mProj.waveDir = waveDir;
				}

				waveDir *= -1;

				comboTime = comboUseTime;
			}
			else
			{
				comboTime--;
			}
			scalePlus = Math.Min(scalePlus + (2f / comboUseTime), 20f);
			ChargeLead chLead = (ChargeLead)Lead.ModProjectile;
			chLead.extraScale = 0.3f * (scalePlus / 20f);
		}

		public override ModItem Clone(Item item)
		{
			MissileLauncher clone = (MissileLauncher)NewInstance(item);//this.NewInstance(item);
																	   //MissileLauncher missileClone = (MissileLauncher)clone;
			clone._missileMods = new Item[MetroidMod.missileSlotAmount];
			clone._missileChange = new Item[MetroidMod.missileChangeSlotAmount];
			for (int i = 0; i < MetroidMod.missileSlotAmount; ++i)
			{
				clone.MissileMods[i] = this.MissileMods[i];
				if (_missileMods == null || _missileMods[i] == null)
				{
					clone._missileMods[i] = new Item();
					clone._missileMods[i].TurnToAir();
				}
				else
				{
					clone._missileMods[i] = _missileMods[i];
				}
			}
			for (int i = 0; i < MetroidMod.missileChangeSlotAmount; ++i)
			{
				clone.MissileChange[i] = this.MissileChange[i];
				if (_missileChange == null || _missileChange[i] == null)
				{
					clone._missileChange[i] = new Item();
					clone._missileChange[i].TurnToAir();
				}
				else
				{
					clone._missileChange[i] = _missileChange[i];
				}
			}

			return clone;
		}

		public override void SaveData(TagCompound tag)
		{
			for (int i = 0; i < MissileMods.Length; ++i)
				tag.Add("missileItem" + i, ItemIO.Save(MissileMods[i]));

			if (Item.TryGetGlobalItem(out MGlobalItem mi))
			{
				tag.Add("statMissiles", mi.statMissiles);
				tag.Add("maxMissiles", mi.maxMissiles);
			}
			else
			{
				tag.Add("statMissiles", 0);
				tag.Add("maxMissiles", 0);
			}
			for (int i = 0; i < MissileChange.Length; ++i)
			{
				// Failsave check.
				if (MissileChange[i] == null)
				{
					MissileChange[i] = new Item();
				}
				tag.Add("MissileChange" + i, ItemIO.Save(MissileChange[i]));
			}
		}
		public override void LoadData(TagCompound tag)
		{
			try
			{
				MissileMods = new Item[MetroidMod.missileSlotAmount];
				for (int i = 0; i < MissileMods.Length; i++)
				{
					Item item = tag.Get<Item>("missileItem" + i);
					MissileMods[i] = item;
				}

				MGlobalItem mi = Item.GetGlobalItem<MGlobalItem>();
				mi.statMissiles = tag.GetInt("statMissiles");
				mi.maxMissiles = tag.GetInt("maxMissiles");
				MissileChange = new Item[MetroidMod.missileChangeSlotAmount];
				for (int i = 0; i < MissileChange.Length; i++)
				{
					Item item = tag.Get<Item>("MissileChange" + i);
					MissileChange[i] = item;
				}
			}
			catch { }
		}

		public override void NetSend(BinaryWriter writer)
		{
			for (int i = 0; i < MissileMods.Length; ++i)
			{
				ItemIO.Send(MissileMods[i], writer);
			}
			for (int i = 0; i < MissileChange.Length; ++i)
			{
				ItemIO.Send(MissileChange[i], writer);
			}
			writer.Write(chargeLead);
		}
		public override void NetReceive(BinaryReader reader)
		{
			for (int i = 0; i < MissileMods.Length; ++i)
			{
				MissileMods[i] = ItemIO.Receive(reader);
			}
			for (int i = 0; i < MissileChange.Length; ++i)
			{
				MissileChange[i] = ItemIO.Receive(reader);
			}
			chargeLead = reader.ReadInt32();
		}
	}
}
