using System;
using System.IO;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

using MetroidMod.Items.damageclass;
using MetroidMod.Projectiles;
using MetroidMod.Projectiles.chargelead;

namespace MetroidMod.Items.weapons
{
	public class MissileLauncher : HunterDamageItem//ModItem
	{
		// Failsaves.
		private Item[] _missileMods;
		public Item[] missileMods
		{
			get
			{
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

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Missile Launcher");
			Tooltip.SetDefault("Select this item in your hotbar and open your inventory to open the Missile Addon UI");
		}
		//public override void SetDefaults()
		public override void SafeSetDefaults()
		{
			item.damage = 30;
			item.ranged = true;
			item.width = 24;
			item.height = 16;
			item.scale = 0.8f;
			item.useTime = 9;
			item.useAnimation = 9;
			item.useStyle = 5;
			item.noMelee = true;
			item.knockBack = 5.5f;
			item.value = 20000;
			item.rare = 2;
			item.UseSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/MissileSound");
			item.autoReuse = false;
			item.shoot = mod.ProjectileType("MissileShot");
			item.shootSpeed = 8f;
			item.crit = 10;
			
			MGlobalItem mi = item.GetGlobalItem<MGlobalItem>();
			mi.statMissiles = 5;
			mi.maxMissiles = 5;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ChoziteBar", 10);
			recipe.AddIngredient(null, "EnergyTank", 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		
		public override void UseStyle(Player P)
		{
			P.itemLocation.X = P.MountedCenter.X - (float)item.width * 0.5f;
			P.itemLocation.Y = P.MountedCenter.Y - (float)item.height * 0.5f;
		}
		
		public override bool CanUseItem(Player player)
		{
			MGlobalItem mi = item.GetGlobalItem<MGlobalItem>();
			if(player.whoAmI == Main.myPlayer && item.type == Main.mouseItem.type)
			{
				return false;
			}
			return (player.whoAmI == Main.myPlayer && mi.statMissiles > 0);
		}
		
		int finalDmg = 0;
		
		int useTime = 9;
		
		string shot = "MissileShot";
		string chargeShot = "DiffusionMissileShot";
		string shotSound = "MissileSound";
		string chargeShotSound = "SuperMissileSound";
		string chargeUpSound = "ChargeStartup_Power";
		string chargeTex = "ChargeLead_PlasmaRed";
		int dustType = 6;
		Color dustColor = default(Color);
		Color lightColor = MetroidMod.plaRedColor;
		
		float comboKnockBack = 5.5f;

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
		
		string altTexture => this.Texture + "_Alt";
		string texture = "";

		public override void UpdateInventory(Player P)
		{
			MGlobalItem mi = item.GetGlobalItem<MGlobalItem>();
			MPlayer mp = P.GetModPlayer<MPlayer>();

			int ic = mod.ItemType("IceMissileAddon");
			int sm = mod.ItemType("SuperMissileAddon");
			int icSm = mod.ItemType("IceSuperMissileAddon");
			int st = mod.ItemType("StardustMissileAddon");
			int ne = mod.ItemType("NebulaMissileAddon");
			
			int se = mod.ItemType("SeekerMissileAddon");
			
			Item slot1 = missileMods[0];
			Item slot2 = missileMods[1];
			Item exp = missileMods[2];
			
			int damage = 30;
			useTime = 9;
			shot = "MissileShot";
			chargeShot = "";
			shotSound = "MissileSound";
			chargeShotSound = "SuperMissileSound";
			chargeUpSound = "";
			chargeTex = "";
			dustType = 0;
			dustColor = default(Color);
			lightColor = Color.White;
			
			texture = "";
			
			comboKnockBack = item.knockBack;
			
			isSeeker = (slot1.type == se);
			isCharge = (!slot1.IsAir && !isSeeker);
			isHeldCombo = 0;
			chargeCost = 5;
			comboSound = 0;
			comboDrain = 5f;
			noSomersault = false;
			useFlameSounds = false;
			useVortexSounds = false;
			
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
			
			mi.maxMissiles = 5 + (5*exp.stack);
			if(mi.statMissiles > mi.maxMissiles)
			{
				mi.statMissiles = mi.maxMissiles;
			}

			// Default Combos
			
			if(slot2.type == sm)
			{
				shot = "SuperMissileShot";
			}
			else if(slot2.type == ic)
			{
				shot = "IceMissileShot";
			}
			else if(slot2.type == icSm)
			{
				shot = "IceSuperMissileShot";
			}
			else if(slot2.type == st)
			{
				shot = "StardustMissileShot";
			}
			else if(slot2.type == ne)
			{
				shot = "NebulaMissileShot";
			}
			
			int wb = mod.ItemType("WavebusterAddon");
			int icSp = mod.ItemType("IceSpreaderAddon");
			int sp = mod.ItemType("SpazerComboAddon");
			int ft = mod.ItemType("FlamethrowerAddon");
			int pl = mod.ItemType("PlasmaMachinegunAddon");
			int nv = mod.ItemType("NovaComboAddon");
			
			int di = mod.ItemType("DiffusionMissileAddon");
			
			// Charge Combos
			if(slot1.type == wb)
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
				texture = "Wavebuster_Item";
			}
			if(slot1.type == icSp)
			{
				chargeShot = "IceSpreaderShot";
				chargeShotSound = "IceSpreaderSound";
				chargeUpSound = "ChargeStartup_Ice";
				chargeTex = "ChargeLead_Ice";
				dustType = 59;
				lightColor = MetroidMod.iceColor;
				texture = "IceSpreader_Item";
			}
			if(slot1.type == sp)
			{
				isShotgun = true;
				chargeShot = shot;
				chargeUpSound = "ChargeStartup_Power";
				chargeTex = "ChargeLead_Spazer";
				dustType = 64;
				lightColor = MetroidMod.powColor;
				texture = "SpazerCombo_Item";
			}
			if(slot1.type == ft)
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
				texture = "Flamethrower_Item";
			}
			if(slot1.type == pl)
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
				texture = "PlasmaMachinegun_Item";
			}
			if(slot1.type == nv)
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
				texture = "NovaLaser_Item";
			}
			
			if(slot1.type == di)
			{
				chargeShot = "DiffusionMissileShot";
				chargeUpSound = "ChargeStartup_Power";
				chargeTex = "ChargeLead_PlasmaRed";
				dustType = 6;
				lightColor = MetroidMod.plaRedColor;
				texture = "DiffusionMissile_Item";
				
				if(slot2.type == ic || slot2.type == icSm)
				{
					chargeShot = "IceDiffusionMissileShot";
					chargeUpSound = "ChargeStartup_Ice";
					chargeTex = "ChargeLead_Ice";
					dustType = 135;
					lightColor = MetroidMod.iceColor;
				}
				if(slot2.type == st)
				{
					chargeShot = "StardustDiffusionMissileShot";
					chargeUpSound = "ChargeStartup_Ice";
					chargeTex = "ChargeLead_Stardust";
					dustType = 87;
					lightColor = MetroidMod.iceColor;
				}
				if(slot2.type == ne)
				{
					chargeShot = "NebulaDiffusionMissileShot";
					chargeUpSound = "ChargeStartup_Wave";
					chargeTex = "ChargeLead_Nebula";
					dustType = 255;
					lightColor = MetroidMod.waveColor;
				}
			}
			if(isSeeker)
			{
				texture = "SeekerMissile_Item";
			}
			
			int sd = mod.ItemType("StardustComboAddon");
			int nb = mod.ItemType("NebulaComboAddon");
			int vt = mod.ItemType("VortexComboAddon");
			int sl = mod.ItemType("SolarComboAddon");
			
			if(slot1.type == sd)
			{
				chargeShot = "StardustComboShot";
				chargeShotSound = "IceSpreaderSound";
				chargeUpSound = "ChargeStartup_Ice";
				chargeTex = "ChargeLead_Stardust";
				dustType = 87;
				lightColor = MetroidMod.iceColor;
				texture = "StardustCombo_Item";
			}
			if(slot1.type == nb)
			{
				isHeldCombo = 1;
				comboSound = 1;
				noSomersault = true;
				chargeShot = "NebulaComboShot";
				chargeUpSound = "ChargeStartup_Wave";
				chargeTex = "ChargeLead_Nebula";
				dustType = 255;
				lightColor = MetroidMod.waveColor;
				texture = "NebulaCombo_Item";
			}
			if(slot1.type == vt)
			{
				isHeldCombo = 2;
				comboSound = 2;
				noSomersault = true;
				
				comboUseTime = 10;
				comboShotAmt = 3;
				
				useVortexSounds = true;
				
				chargeShot = "VortexComboShot";
				chargeShotSound = "PlasmaMachinegunSound";
				chargeUpSound = "ChargeStartup_Power";
				chargeTex = "ChargeLead_Vortex";
				dustType = 229;
				lightColor = MetroidMod.lumColor;
				texture = "VortexCombo_Item";
			}
			if(slot1.type == sl)
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
				texture = "SolarCombo_Item";
			}
			
			if(!slot1.IsAir)
			{
				MGlobalItem mItem = slot1.GetGlobalItem<MGlobalItem>();
				chargeMult = mItem.addonChargeDmg;
				chargeCost = mItem.addonMissileCost;
				comboDrain = mItem.addonMissileDrain;
			}
			comboCostUseTime = (int)Math.Round(60.0 / (double)comboDrain);
			
			float addonDmg = 0f;
			float addonSpeed = 0f;
			if(!slot2.IsAir)
			{
				MGlobalItem mItem = slot2.GetGlobalItem<MGlobalItem>();
				addonDmg = mItem.addonDmg;
				addonSpeed = mItem.addonSpeed;
			}
			finalDmg = (int)Math.Round((double)((float)damage * (1f + addonDmg)));
			
			float shotsPerSecond = (60f / useTime) * (1f + addonSpeed);
			useTime = (int)Math.Max(Math.Round(60.0 / (double)shotsPerSecond), 2);
			
			item.damage = finalDmg;
			item.useTime = useTime;
			item.useAnimation = useTime;
			item.shoot = mod.ProjectileType(shot);
			item.UseSound = null;//mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/"+shotSound);

			item.shootSpeed = 8f;
			item.reuseDelay = 0;
			item.mana = 0;
			item.knockBack = 5.5f;
			item.scale = 0.8f;
			item.crit = 10;
			item.value = 20000;
			
			item.rare = 2;
			
			item.Prefix(item.prefix);
		}
		public override bool PreDrawInWorld(SpriteBatch sb, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			MGlobalItem mi = item.GetGlobalItem<MGlobalItem>();
			Texture2D tex = Main.itemTexture[item.type];
			this.setTexture(mi);
			if(mi.itemTexture != null)
			{
				tex = mi.itemTexture;
			}
			float num5 = (float)(item.height - tex.Height);
			float num6 = (float)(item.width / 2 - tex.Width / 2);
			sb.Draw(tex, new Vector2(item.position.X - Main.screenPosition.X + (float)(tex.Width / 2) + num6, item.position.Y - Main.screenPosition.Y + (float)(tex.Height / 2) + num5 + 2f),
			new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), alphaColor, rotation, new Vector2((float)(tex.Width / 2), (float)(tex.Height / 2)), scale, SpriteEffects.None, 0f);
			return false;
		}
		public override bool PreDrawInInventory(SpriteBatch sb, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			MGlobalItem mi = item.GetGlobalItem<MGlobalItem>();
			Texture2D tex = Main.itemTexture[item.type];
			this.setTexture(mi);
			if(mi.itemTexture != null)
			{
				tex = mi.itemTexture;
			}
			sb.Draw(tex, position, new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
			return false;
		}
		void setTexture(MGlobalItem mi)
		{
			if(texture != "")
			{
				string alt = "";
				if(MetroidMod.UseAltWeaponTextures)
				{
					alt = "_alt";
				}
				mi.itemTexture = mod.GetTexture("Items/weapons/missileTextures"+alt+"/"+texture);
			}
			else
			{
				if(MetroidMod.UseAltWeaponTextures)
				{
					mi.itemTexture = ModContent.GetTexture(altTexture);
				}
				else
				{
					mi.itemTexture = Main.itemTexture[item.type];
				}
			}
			
			if(mi.itemTexture != null)
			{
				item.width = mi.itemTexture.Width;
				item.height = mi.itemTexture.Height;
			}
		}
		
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			base.ModifyTooltips(tooltips);
			
			Player P = Main.player[Main.myPlayer];
			MPlayer mp = P.GetModPlayer<MPlayer>();
			
			if(item == Main.HoverItem)
			{
				item.modItem.UpdateInventory(Main.player[Main.myPlayer]);
			}
			
			int cost = (int)((float)chargeCost * (mp.missileCost+0.001f));
			string ch = "Charge shot consumes "+cost+" missiles";
			if(isHeldCombo > 0)
			{
				ch = "Charge initially costs "+cost+" missiles";
			}
			TooltipLine mCost = new TooltipLine(mod, "ChargeMissileCost", ch);
			
			float drain = (float)Math.Round(comboDrain * mp.missileCost, 2);
			TooltipLine mDrain = new TooltipLine(mod, "ChargeMissileDrain", "Drains "+drain+" missiles per second");
			
			for (int k = 0; k < tooltips.Count; k++)
			{
				if(tooltips[k].Name == "Knockback" && !missileMods[0].IsAir && !isSeeker)
				{
					tooltips.Insert(k + 1, mCost);
					if(isHeldCombo > 0)
					{
						tooltips.Insert(k + 2, mDrain);
					}
				}
				
				if(tooltips[k].Name == "PrefixDamage")
				{
					double num19 = (double)((float)item.damage - (float)finalDmg);
					num19 = num19 / (double)((float)finalDmg) * 100.0;
					num19 = Math.Round(num19);
					if (num19 > 0.0)
					{
						tooltips[k].text = "+" + num19 + Lang.tip[39].Value;
					}
					else
					{
						tooltips[k].text = num19 + Lang.tip[39].Value;
					}
				}
				if(tooltips[k].Name == "PrefixSpeed")
				{
					double num20 = (double)((float)item.useAnimation - (float)useTime);
					num20 = num20 / (double)((float)useTime) * 100.0;
					num20 = Math.Round(num20);
					num20 *= -1.0;
					if (num20 > 0.0)
					{
						tooltips[k].text = "+" + num20 + Lang.tip[40].Value;
					}
					else
					{
						tooltips[k].text = num20 + Lang.tip[40].Value;
					}
				}
			}
		}
		
		public override ModItem Clone(Item item)
		{
			ModItem clone = this.NewInstance(item);
			MissileLauncher missileClone = (MissileLauncher)clone;
			missileClone.missileMods = new Item[MetroidMod.missileSlotAmount];
			for (int i = 0; i < MetroidMod.missileSlotAmount; ++i)
			{
				missileClone.missileMods[i] = this.missileMods[i];
			}

			return clone;
		}
		
		int chargeLead = -1;
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			MGlobalItem mi = item.GetGlobalItem<MGlobalItem>();
			if(isCharge)
			{
				int ch = Projectile.NewProjectile(position.X,position.Y,speedX,speedY,mod.ProjectileType("ChargeLead"),damage,knockBack,player.whoAmI);
				ChargeLead cl = (ChargeLead)Main.projectile[ch].modProjectile;
				cl.ChargeUpSound = chargeUpSound;
				cl.ChargeTex = chargeTex;
				cl.DustType = dustType;
				cl.DustColor = dustColor;
				cl.LightColor = lightColor;
				cl.ShotSound = shotSound;
				cl.ChargeShotSound = chargeShotSound;
				cl.projectile.netUpdate = true;
				cl.missile = true;
				cl.comboSound = comboSound;
				cl.noSomersault = noSomersault;
				cl.aimSpeed = leadAimSpeed;

				chargeLead = ch;
				return false;
			}
			else if(isSeeker)
			{
				int ch = Projectile.NewProjectile(position.X,position.Y,speedX,speedY,mod.ProjectileType("SeekerMissileLead"),damage,knockBack,player.whoAmI);
				chargeLead = ch;
				return false;
			}
			else
			{
				mi.statMissiles -= 1;
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/" + shotSound), player.position);
			}
			return true;
		}
		
		bool leadActive(Player player, int type)
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
			if (player.whoAmI == Main.myPlayer)
			{
				MPlayer mp = player.GetModPlayer<MPlayer>();
				MGlobalItem mi = item.GetGlobalItem<MGlobalItem>();
				
				int chCost = (int)((float)chargeCost * (mp.missileCost+0.001f));
				comboCostUseTime = (int)Math.Round(60.0 / (double)(comboDrain * mp.missileCost));
				isCharge &= (mi.statMissiles >= chCost || (isHeldCombo > 0 && initialShot));
				
				item.autoReuse = (isCharge || isSeeker);

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

						Vector2 velocity = targetrotation.ToRotationVector2() * item.shootSpeed;

						float dmgMult = chargeMult;
						int damage = player.GetWeaponDamage(item);
						
						//if (player.controlUseItem && chargeLead != -1 && Main.projectile[chargeLead].active && Main.projectile[chargeLead].owner == player.whoAmI && Main.projectile[chargeLead].type == mod.ProjectileType("ChargeLead"))
						if(player.controlUseItem && leadActive(player,mod.ProjectileType("ChargeLead")))
						{
							if (mp.statCharge < MPlayer.maxCharge)
							{
								mp.statCharge = Math.Min(mp.statCharge + 1, MPlayer.maxCharge);
							}
							if(isHeldCombo > 0)
							{
								if(mi.statMissiles > 0)
								{
									if(mp.statCharge >= MPlayer.maxCharge)
									{
										if(isMiniGun)
										{
											this.MiniGunShoot(player, item, Main.projectile[chargeLead], mod.ProjectileType(chargeShot), (int)((float)damage * dmgMult), comboKnockBack, chargeShotSound);
										}
										else
										{
											if(comboTime <= 0)
											{
												for(int i = 0; i < comboShotAmt; i++)
												{
													int proj = Projectile.NewProjectile(oPos.X, oPos.Y, velocity.X, velocity.Y, mod.ProjectileType(chargeShot), (int)((float)damage * dmgMult), comboKnockBack, player.whoAmI);
													Main.projectile[proj].ai[0] = chargeLead;
												}
												comboTime = comboUseTime;
											}
											
											if(isHeldCombo == 2 && comboTime > 0)
											{
												comboTime--;
											}
										}
										
										if(!initialShot)
										{
											if(useFlameSounds || useVortexSounds)
											{
												int type = mod.ProjectileType("FlamethrowerLead");
												if(useVortexSounds)
												{
													type = mod.ProjectileType("VortexComboLead");
												}
												int proj = Projectile.NewProjectile(oPos.X, oPos.Y, velocity.X, velocity.Y, type, 0, 0, player.whoAmI);
												Main.projectile[proj].ai[0] = chargeLead;
											}
											
											mi.statMissiles = Math.Max(mi.statMissiles-chCost,0);
											
											initialShot = true;
										}
										
										if(comboCostUseTime > 0)
										{
											//if(comboCostTime <= 0)
											if(comboCostTime > comboCostUseTime)
											{
												mi.statMissiles = Math.Max(mi.statMissiles-1,0);
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
							if(mp.statCharge <= 0 && leadActive(player,mod.ProjectileType("ChargeLead")))
							{
								mp.statCharge++;
							}
							if(isHeldCombo <= 0 || mp.statCharge < MPlayer.maxCharge)
							{
								if (mp.statCharge >= MPlayer.maxCharge && mi.statMissiles >= chCost)
								{
									if(isShotgun)
									{
										for(int i = 0; i < shotgunAmt; i++)
										{
											int k = i - (shotgunAmt/2);
											Vector2 shotGunVel = Vector2.Normalize(velocity) * (item.shootSpeed + 4f);
											double rot = Angle.ConvertToRadians(4.0*k);
											shotGunVel = shotGunVel.RotatedBy(rot, default(Vector2));
											if (float.IsNaN(shotGunVel.X) || float.IsNaN(shotGunVel.Y))
											{
												shotGunVel = -Vector2.UnitY;
											}
											int chargeProj = Projectile.NewProjectile(oPos.X, oPos.Y, shotGunVel.X, shotGunVel.Y, mod.ProjectileType(chargeShot), (int)((float)damage * dmgMult), item.knockBack, player.whoAmI);
										}
									}
									else
									{
										int chargeProj = Projectile.NewProjectile(oPos.X, oPos.Y, velocity.X, velocity.Y, mod.ProjectileType(chargeShot), (int)((float)damage * dmgMult), item.knockBack, player.whoAmI);
									}
									mi.statMissiles -= chCost;
								}
								else if (mp.statCharge > 0)
								{
									int shotProj = Projectile.NewProjectile(oPos.X, oPos.Y, velocity.X, velocity.Y, mod.ProjectileType(shot), damage, item.knockBack, player.whoAmI);
									mi.statMissiles -= 1;
								}
							}

							if (!leadActive(player,mod.ProjectileType("ChargeLead")))
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
					Vector2 velocity = targetrotation.ToRotationVector2() * item.shootSpeed;
					int damage = player.GetWeaponDamage(item);
					//if (player.controlUseItem && chargeLead != -1 && Main.projectile[chargeLead].active && Main.projectile[chargeLead].owner == player.whoAmI && Main.projectile[chargeLead].type == mod.ProjectileType("SeekerMissileLead"))
					if(player.controlUseItem && leadActive(player,mod.ProjectileType("SeekerMissileLead")))
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
										if(mi.seekerTarget[j] == npc.whoAmI)
										{
											flag = true;
										}
									}
									
									Vector2 delta = new Vector2(MX,MY);
									delta.X -= MathHelper.Clamp(MX,npcRect.X,npcRect.X+npcRect.Width);
									delta.Y -= MathHelper.Clamp(MY,npcRect.Y,npcRect.Y+npcRect.Height);
									bool colFlag = (delta.Length() < 50);
									if(colFlag && mi.seekerTarget[targetNum] <= -1 && ((targetingDelay <= 0 && mouse.Intersects(npcRect)) || !flag) && mi.statMissiles > mi.numSeekerTargets)
									{
										mi.seekerTarget[targetNum] = npc.whoAmI;
										targetNum++;
										if (targetNum > 4)
										{
											targetNum = 0;
										}
										targetingDelay = 40;
										Main.PlaySound(SoundLoader.customSoundType, (int)oPos.X, (int)oPos.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/SeekerLockSound"));
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
						if(mi.seekerCharge <= 0 && leadActive(player,mod.ProjectileType("SeekerMissileLead")))
						{
							mi.seekerCharge++;
						}
						if (mi.seekerCharge >= MGlobalItem.seekerMaxCharge && mi.numSeekerTargets > 0)
						{
							for (int i = 0; i < mi.seekerTarget.Length; i++)
							{
								if (mi.seekerTarget[i] > -1)
								{
									int shotProj = Projectile.NewProjectile(oPos.X, oPos.Y, velocity.X, velocity.Y, mod.ProjectileType(shot), damage, item.knockBack, player.whoAmI);
									MProjectile mProj = (MProjectile)Main.projectile[shotProj].modProjectile;
									mProj.seekTarget = mi.seekerTarget[i];
									mProj.seeking = true;
									mProj.projectile.netUpdate2 = true;
									mi.statMissiles = Math.Max(mi.statMissiles - 1, 0);
								}
							}

							Main.PlaySound(SoundLoader.customSoundType, (int)oPos.X, (int)oPos.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/SeekerMissileSound"));
						}
						else if (mi.seekerCharge > 0)
						{
							int shotProj = Projectile.NewProjectile(oPos.X, oPos.Y, velocity.X, velocity.Y, mod.ProjectileType(shot), damage, item.knockBack, player.whoAmI);
							Main.PlaySound(SoundLoader.customSoundType, (int)oPos.X, (int)oPos.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/" + shotSound));

							mi.statMissiles -= 1;
						}
						if (!leadActive(player,mod.ProjectileType("SeekerMissileLead")))
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
			if(comboTime <= 0)
			{
				soundInstance = Main.PlaySound(SoundLoader.customSoundType, (int)player.Center.X, (int)player.Center.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/" + sound));
				if(soundInstance != null)
				{
					soundInstance.Volume *= 1f - 0.25f*(scalePlus / 20f);
				}
				
				float spray = 1f * (scalePlus / 20f);
				
				float scaleFactor2 = 14f;
				for(int i = 0; i < miniGunAmt; i++)
				{
					float rot = Lead.velocity.ToRotation() + (float)Angle.ConvertToRadians(Main.rand.Next(18)*10) - (float)Math.PI/2f;
					Vector2 vector3 = Lead.Center + rot.ToRotationVector2() * 7f * spray;
					Vector2 vector5 = Vector2.Normalize(Lead.velocity) * scaleFactor2;
					vector5 = vector5.RotatedBy((Main.rand.NextDouble() * 0.12 - 0.06)*spray, default(Vector2));
					if (float.IsNaN(vector5.X) || float.IsNaN(vector5.Y))
					{
						vector5 = -Vector2.UnitY;
					}
					int proj = Projectile.NewProjectile(vector3.X, vector3.Y, vector5.X, vector5.Y, projType, damage, knockBack, player.whoAmI, 0f, 0f);
					Main.projectile[proj].ai[0] = Lead.whoAmI;
					MProjectile mProj = (MProjectile)Main.projectile[proj].modProjectile;
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
			ChargeLead chLead = (ChargeLead)Lead.modProjectile;
			chLead.extraScale = 0.3f * (scalePlus / 20f);
		}
		
		public override TagCompound Save()
		{
			TagCompound tag = new TagCompound();
			for (int i = 0; i < missileMods.Length; ++i)
				tag.Add("missileItem" + i, ItemIO.Save(missileMods[i]));

			MGlobalItem mi = item.GetGlobalItem<MGlobalItem>();
			tag.Add("statMissiles", mi.statMissiles);
			tag.Add("maxMissiles", mi.maxMissiles);

			return tag;
		}
		public override void Load(TagCompound tag)
		{
			try
			{
				missileMods = new Item[MetroidMod.missileSlotAmount];
				for (int i = 0; i < missileMods.Length; i++)
				{
					Item item = tag.Get<Item>("missileItem" + i);
					missileMods[i] = item;
				}
				
				MGlobalItem mi = this.item.GetGlobalItem<MGlobalItem>();
				mi.statMissiles = tag.GetInt("statMissiles");
				mi.maxMissiles = tag.GetInt("maxMissiles");
			}
			catch{}
		}

		public override void NetSend(BinaryWriter writer)
		{
			for (int i = 0; i < missileMods.Length; ++i)
			{
				writer.WriteItem(missileMods[i]);
			}
			writer.Write(chargeLead);
		}
		public override void NetRecieve(BinaryReader reader)
		{
			for (int i = 0; i < missileMods.Length; ++i)
			{
				missileMods[i] = reader.ReadItem();
			}
			chargeLead = reader.ReadInt32();
		}
	}
}
