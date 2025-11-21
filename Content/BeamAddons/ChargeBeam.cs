using System;
using MetroidMod.Common.GlobalItems;
using MetroidMod.Common.Players;
using MetroidMod.Content.Items.Weapons;
using MetroidMod.Content.Projectiles;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.BeamAddons
{
	public class ChargeBeam : ModBeamAddon
	{
		//So fun fact, this is the first ModBeamAddon ever made!      -Z
		public override bool AddOnlyAddonItem => false; //Idk why you'd ever want to enable this

		#region Projectile visuals
		public override Color PrimaryColor => new(248, 248, 110); //This should hopefully only be for light color in the future, assuming I make shaders

		public override Color SecondaryColor => MetroidMod.powSecondaryColor;
		public override int ShotDust => 64;

		public override string ShotSound => $"{Mod.Name}/Assets/Sounds/ArmCannon/Shot";
		public override string ImpactSound => $"{Mod.Name}/Assets/Sounds/ArmCannon/BeamImpactSound";

		#endregion
		/// <summary>
		/// The stat multiplier applied to a beam shot at <b>full charge.</b>
		/// </summary>
		public float chargeMultiplier = 3f;
		/// <summary>
		/// Makes the Charge Beam take a second to actually start charging
		/// </summary>
		public float chargeDelay = 0f;


		public override void SetStaticDefaults()
		{

			AddonSlot = BeamAddonSlotID.Primary;
			//these values determine how the addon will interact with the dynamic visual system
			ShapePriority = 0;
			ColorPriority = 0;
			SoundOverride = false;
			HoldFire = true;

			//This is where you set your numbers
			BaseDamage = 5;
			DamageMult = 5f;
			BaseOverheat = 5;
			OverheatMult = 0f;
			BaseSpeed = 0;
			SpeedMult = 50f;
			BaseVelocity = 0;
			VelocityMult = 0f;
			CritChance = 0;
		}

		public override void SetItemDefaults(Item item)
		{
			item.rare = ItemRarityID.Blue;
		}

		//This is a little bit complicated.
		//Essentially, combo keywords are used to get assets for edge-case shenanigans.
		//Odds are the only one you'll want to worry about is the one for charged shots,
		//but even then you'll only need it if your charge shot sprite has a different amount of animation frames from the default.
		//Below is the absolute simplest method to check if your shot is charged.
		public override int[] ComboVisualsGet(string modifier)
		{
			//Literally all you need for this method is a single switch. This is practically what switches were made for.
			//If I see anyone try to use if-else chains here I will be very upset
			switch (modifier)
			{
				case "Charged": //This is the dynamic keyword for a charged shot.
					return [2, -1];

				default:
					return base.ComboVisualsGet(modifier);
			}
			/*if (if-else)
				{
					return veryupset;
				}
				else
					return;*/
		}

		private int chInt = -1;
		public Projectile chProj;
		public ChargeLead chargio;
		//"This is where the fun begins" -Anakin Skywalker
		public override void HoldFireBehavior(Player player)
		{
			//This needs to be here otherwise the game's gonna keep on trying to call shit inside of this method without the arm cannon if you switch items too fast
			if (player.lastVisualizedSelectedItem.type != ModContent.ItemType<ArmCannon>()) { return; }


			//Get all the relevant data about the player first.
			MPlayer mp = player.GetModPlayer<MPlayer>(); //finds the current player's MPlayer data for later modification
			Item item = player.HeldItem;// Main.LocalPlayer.inventory[mp.selectedItem]; //Grab the Arm Cannon from the player's selected item. A little worried this could break?
			ArmCannon wepon = (ArmCannon)item.ModItem; //john freeman then looked on the ground and found wepon so he pickd it up and fired fast at zombie goasts in front of a house
			if (wepon == null) { return; } //I stg I have to have one of these for every goddamn step of conversion just to make sure it goes through properly //WTF WHY IS THAT TRUE --DR
			MGlobalItem ac = item.GetGlobalItem<MGlobalItem>();
			Color chargioColor = BeamAddonLoader.GetAddon(wepon.BeamAddonAccess[wepon.VisualDinners[1]]).PrimaryColor;
			Color chargioColor2 = BeamAddonLoader.GetAddon(wepon.BeamAddonAccess[wepon.VisualDinners[1]]).SecondaryColor;
			float chargioBrightness = BeamAddonLoader.GetAddon(wepon.BeamAddonAccess[wepon.VisualDinners[1]]).CoreBrightness;
			float chargioSaturation = BeamAddonLoader.GetAddon(wepon.BeamAddonAccess[wepon.VisualDinners[1]]).CoreSaturation;
			ModBeamAddon soundSource = BeamAddonLoader.GetAddon(wepon.BeamAddonAccess[wepon.VisualDinners[(wepon.VisualDinners[3] == 1) ? 1 : 0]]);
			//there's a tiny part of me that wants it to not hardcodedly check for an arm cannon but that's probably dumb so


			//Now get all the relevant locational data.
			Vector2 oPos = player.RotatedRelativePoint(player.MountedCenter, true);
			float MY = Main.mouseY + Main.screenPosition.Y;
			float MX = Main.mouseX + Main.screenPosition.X;
			if (player.gravDir == -1f) { MY = Main.screenPosition.Y + Main.screenHeight - Main.mouseY; }
			float targetrotation = (float)Math.Atan2(MY - oPos.Y, MX - oPos.X);
			Vector2 velocity = targetrotation.ToRotationVector2() * item.shootSpeed;

			//important control variables

			bool dontCharge = !ac.isBeam && MissileAddonLoader.GetAddon(wepon.MissileAddonAccess[MissileAddonSlotID.Charge]) != null && !MissileAddonLoader.GetAddon(wepon.MissileAddonAccess[MissileAddonSlotID.Charge]).NeedsCharging;
			bool canCharge = !player.noItems && !mp.ballstate && !mp.shineActive && !player.dead && !player.CCed && (player.whoAmI == Main.myPlayer);
			float currentMultiplier = 0f;

			//here's the part where all the charging happens
			if (player.controlUseItem && canCharge && (ac.isBeam || MissileAddonLoader.GetAddon(wepon.MissileAddonAccess[MissileAddonSlotID.Charge]) != null))
			{
				if (chargeDelay == item.useTime - 1)
				{
					//Specific thresholds of charge at which certain things happen
					switch (mp.statCharge)
					{
						case 0.0f:
							//spawn the chargelead
							//ChargeLead chargio = Projectile.NewProjectileDirect(player.GetSource_ItemUse(item), oPos, targetrotation.ToRotationVector2() * ac.barrelOffset, ModContent.ProjectileType<ChargeLead>(), item.damage, 0, player.whoAmI).ModProjectile as ChargeLead;
							//MetroidMod.Instance.Logger.Info(player.name + " spawned charge lead");
							//chInt = Projectile.NewProjectile(player.GetSource_ItemUse(item), oPos.X, oPos.Y, velocity.X, velocity.Y, ModContent.ProjectileType<ChargeLead>(), item.damage, item.knockBack, player.whoAmI);
							chProj = Projectile.NewProjectileDirect(player.GetSource_ItemUse(item), oPos, velocity, ModContent.ProjectileType<ChargeLead>(), item.damage, item.knockBack, player.whoAmI);
							chargio = (ChargeLead)chProj.ModProjectile;
							chInt = chProj.whoAmI;
							mp.disableSomersault = true;
							chargio.sourceItem = item;
							chargio.sourceAddon = this;
							chargio.ballColor = chargioColor;
							chargio.ballColor2 = chargioColor2;
							chargio.coreBrightness = chargioBrightness;
							chargio.coreSaturation = chargioSaturation;
							chargio.dontCharge = dontCharge;
							Main.projectile[chInt].ai[0] = chInt;
							MetroidMod.Instance.Logger.Info(item);
							//play charge noise
							break;
						case 1.0f:
							if (dontCharge) //thanks seekers doesnt work at 0 charge for now
							{
								MissileAddonLoader.GetAddon(wepon.MissileAddonAccess[MissileAddonSlotID.Charge]).HoldFireBehavior(player, chProj);
								break;
							}
							break;
						case 99f:
							//Charging is done. Play charge complete sound effect if applicable.
							MetroidMod.Instance.Logger.Info(player.name + " is charging beam shot! 100%");

							//If there's a holdfire in the Charge Slot and we're in missile mode, don't play the charge max sound effect.
							if (!ac.isBeam && !wepon.MissileAddonAccess[MissileAddonSlotID.Charge].IsAir
									&& MissileAddonLoader.GetAddon(wepon.MissileAddonAccess[MissileAddonSlotID.Charge]).HoldFire)
							{
								break;
							}

							SoundEngine.PlaySound(new SoundStyle($"{Mod.Name}/Assets/Sounds/ArmCannon/ChargeMax"));
							break;

						case >= 100f:
							//AFAIK there's nothing the Power Beam would need to constantly do at max charge so
							//Check if the cannon's anything but ready to fire a missile holdfire
							if (ac.isBeam || wepon.MissileAddonAccess[MissileAddonSlotID.Charge].IsAir || !MissileAddonLoader.GetAddon(wepon.MissileAddonAccess[MissileAddonSlotID.Charge]).HoldFire || dontCharge)
							{
								break;
							}

							//Let the missile holdfire do its thing
							MissileAddonLoader.GetAddon(wepon.MissileAddonAccess[MissileAddonSlotID.Charge]).HoldFireBehavior(player, chProj);
							break;
						default:
							if ((mp.statCharge > 75f) && ac.isBeam)
							{
								//Officially over the limit as to what's legally considered charged (only applies to beams)
								//also begin juicing up the current multiplier
								currentMultiplier = (mp.statCharge - 25f) / 100f;
								//Ideally it should still scale fairly naturally while still letting there be a bit of a bump at full to make there be a difference

								//enable pseudo screw if beam
							}
							else
							{
								//if it's not beam turn pseudo screw back off
							}
							if ((mp.statCharge % 25f == 0f) && (mp.statCharge != 100f))
							{
								//MetroidMod.Instance.Logger.Info(player.name + " is charging beam shot! " + mp.statCharge + "%");
							}
							break;
					}
					if ((mp.statCharge < 100f && !dontCharge) || (dontCharge && mp.statCharge < 1.0f))
					{
						mp.statCharge += 1f;
					}
					mp.chargeColor = chargioColor;

				} //the delay has ended, charging can begin
				else
				{
					chargeDelay += 1f;
					if (chargeDelay % 10 == 0) { MetroidMod.Instance.Logger.Info("delay is at " + chargeDelay + "/" + item.useTime); }
					if (chargeDelay > item.useTime) { chargeDelay = item.useTime; }
				} //not allowed to charge just yet
			}//Check if the player is currently trying to charge with a compatible weapon
			else if (!dontCharge && canCharge && (ac.isBeam || !wepon.MissileAddonAccess[MissileAddonSlotID.Charge].IsAir) && mp.statCharge > 5f)
			{
				MetroidMod.Instance.Logger.Info("jobs done");
				if (mp.statCharge >= 100f)
				{
					//spawn that fully charged beam my man
					if (ac.isBeam)
					{
						MetroidMod.Instance.Logger.Info(player.name + " released the kraken!!!");
						wepon.SpawnBeam(player, player.GetSource_ItemUse(item), oPos, velocity * (chargeMultiplier / 2.5f), item.shoot, (int)(item.damage * chargeMultiplier), item.knockBack, "Charged");
					}
					else if (!wepon.MissileAddonAccess[MissileAddonSlotID.Charge].IsAir && !MissileAddonLoader.GetAddon(wepon.MissileAddonAccess[MissileAddonSlotID.Charge]).HoldFire)
					{
						//MetroidMod.Instance.Logger.Info(player.name + " launched the nukes!!!");
						wepon.Launch(player, player.GetSource_ItemUse(item), oPos, velocity, item.shoot, item.damage, item.knockBack, true);
					}
					//alternatively shoot that missile combo if it's not a held
				}
				else if (mp.statCharge > 75f && ac.isBeam)
				{
					//spawn that mostly charged beam my man
					wepon.SpawnBeam(player, player.GetSource_ItemUse(item), oPos, velocity * (1.5f * (mp.statCharge / 100f)), item.shoot, (int)(item.damage * (1f + currentMultiplier)), item.knockBack, "Charged");
					MetroidMod.Instance.Logger.Info(player.name + " released the... uh... slightly-less-charged beam!!!");
				}
				else if (mp.statCharge > 5f)
				{
					//spawn that normal-ass beam my man
					if (ac.isBeam)
					{
						wepon.SpawnBeam(player, player.GetSource_ItemUse(item), oPos, velocity, item.shoot, item.damage, item.knockBack);
						MetroidMod.Instance.Logger.Info(player.name + " didn't bother charging the beam all the way");
					}
					//alternatively shoot that normal-ass missile
					else
					{
						wepon.Launch(player, player.GetSource_ItemUse(item), oPos, velocity, item.shoot, item.damage, item.knockBack);
					}
				}
				player.itemTime = 20;
				player.itemAnimation = 20;
				mp.statCharge = 0f;
				chargeDelay = 0f;
				MissileAddonLoader.GetAddon(wepon.MissileAddonAccess[MissileAddonSlotID.Charge]).Initialized = false;
			}//Check if there's any charge to release
			else
			{
				//MetroidMod.Instance.Logger.Info("jobs never startd");
				mp.statCharge = 0;
				chargeDelay = 0;
				MissileAddonLoader.GetAddon(wepon.MissileAddonAccess[MissileAddonSlotID.Charge]).Initialized = false;
			}//Cancel out any leftover charge
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<Items.Miscellaneous.ChoziteBar>(3)
				.AddIngredient(ItemID.ManaCrystal, 1)
				.AddIngredient(ItemID.FallenStar, 2)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}



	//What I am about to do is probably incredibly stupid. Be warned.
	//note for the future: the "what I am about to do" part is making this projectile part of the charge beam's file
	public class ChargeLead : MProjectile
	{
		public override string Texture => $"{Mod.Name}/Assets/Textures/BeamAddons/ChargeLead";
		public string ChargingSound => $"{Mod.Name}/Assets/Sounds/ArmCannon/BeamChargingSound";
		public string MaxChargeSound => $"{Mod.Name}/Assets/Sounds/ArmCannon/ChargeMax";

		public Color ballColor = MetroidMod.powColor;

		public Color ballColor2 = MetroidMod.powSecondaryColor;

		public float coreBrightness = 1f;

		public float coreSaturation = 0f;

		public Item sourceItem;

		public ChargeBeam sourceAddon;

		public bool dontCharge = false;

		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.timeLeft = 8800;
			Projectile.ownerHitCheck = true;
			Projectile.friendly = false; //Keeps it from hurting enemies
			Projectile.hostile = false; //Keeps it from hurting friends
			Projectile.tileCollide = false;
			Projectile.penetrate = 1;
			Projectile.ignoreWater = true;
		}

		public override void OnSpawn(IEntitySource source)
		{
			//play the charging sound effect I guess
		}

		public override void AI()
		{
			//THE BARE MINIMUM OF WHAT I WANT THIS TO DO:
			//* Increase in size with charge stat  - Done
			//* Glue itself to the end of the arm cannon - Pretty much done, just needs to render a layer higher
			//* Delete itself upon releasing fire - Done
			//* Function with both the beam and missiles - Not done
			//* Color itself to either match the current beam or the current charge combo, depending on the context - Not done


			Player player = Main.player[Projectile.owner];
			MPlayer mp = player.GetModPlayer<MPlayer>();
			MGlobalItem ac = sourceItem.GetGlobalItem<MGlobalItem>();
			Vector2 oPos = player.RotatedRelativePoint(player.MountedCenter, true);
			Vector2 ballPos = player.GetFrontHandPosition(Player.CompositeArmStretchAmount.Full, player.itemRotation - ((float)(Math.PI / 2f) * player.direction));

			bool isCharging = player.controlUseItem && !player.noItems && !player.dead && !mp.ballstate && !mp.shineActive && !player.CCed;

			//This sucker needs to exist so the sound effects can properly cut each other off
			ReLogic.Utilities.SlotId soundInstance;

			//maybe put the thing in OnSpawn? 
			//To have the thing change dynamically I'm gonna have to do a touch of finangling
			//May need to include a few extra properties and variables?
			//Or alternatively I could do all the asset checking back in the charge beam itself
			//and relay the results into the variables that already exist -Z

			//MetroidMod.Instance.Logger.Info(player.name + " spawned charge lead!!!");
			if (Projectile.owner == Main.myPlayer)
			{
				if (isCharging && ((!mp.somersault && mp.pseudoScrewActive) || (!mp.pseudoScrewActive) || (mp.pseudoScrewActive && mp.statCharge < 75) || dontCharge))
				{
					BarrelGlue(player, ballPos);
					BarrelAim(player, ballPos, ac.barrelOffset);

					Projectile.rotation += 0.5f;
					//Projectile.scale = Math.Max(mp.statCharge / 100, 0.5f);
					Projectile.scale = Math.Max(MathHelper.Lerp(0f, 1f, mp.statCharge / 100f), 0.5f); //Idrk what the difference is but I've heard lerps are useful so I'm trying em out			-Z

					if (!mp.pseudoScrewActive || (mp.pseudoScrewActive && mp.statCharge < 75f) || !ac.isBeam)
					{
						mp.disableSomersault = true;
					}
					else
					{
						mp.disableSomersault = false;
						//MetroidMod.Instance.Logger.Info(ballColor);
					}
					Projectile.hide = false;
					Projectile.friendly = false;
				}
				else if (isCharging && mp.pseudoScrewActive && ac.isBeam && mp.statCharge > 75f)
				{
					Projectile.hide = true;
					Projectile.damage = (int)(sourceItem.damage * ((mp.statCharge == 100f) ? sourceAddon.chargeMultiplier : (((mp.statCharge - 25f) / 100f) + 1f)));
					Projectile.friendly = true;
					Projectile.Center = oPos;
					player.itemTime = 2;

				} //pseudo-screw
				else
				{
					MetroidMod.Instance.Logger.Info("There goes the chargelead the big ball is gone");
					mp.disableSomersault = false;
					Projectile.Kill();
				}
				//if (MSystem.ACSwitch.JustPressed)
				//{
				//	mp.disableSomersault = false;
				//	Projectile.Kill();
				//}
			}
			//make sure the projectile doesn't expire naturally
			Projectile.timeLeft = 2;
		}

		public override void OnKill(int timeLeft)
		{
			Player player = Main.player[Projectile.owner];
			MPlayer mp = player.GetModPlayer<MPlayer>();
			MGlobalItem ac = sourceItem.GetGlobalItem<MGlobalItem>();
			ArmCannon wepon = (ArmCannon)sourceItem.ModItem;
			if (Projectile.owner == Main.myPlayer && Projectile.friendly)
			{
				mp.statCharge = 0;
				player.itemTime = 0;
				sourceAddon.chargeDelay = 0;
			}

			//play the shot sound here I guess
		}
		/// <summary>
		/// Determines where the charge lead is positioned relative to the player's arm.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="playerHandPos"></param>
		private void BarrelGlue(Player player, Vector2 playerHandPos)
		{
			//A lot of this is pretty much outta the ExampleMod last prism clone. Unsurprisingly I suppose.
			Projectile.position = player.GetFrontHandPosition(Player.CompositeArmStretchAmount.Full, player.itemRotation - ((float)(Math.PI / 2) * player.direction)) - (Projectile.Size / 2f);
			Projectile.spriteDirection = Projectile.direction;

			player.ChangeDir(Projectile.direction);
			player.heldProj = Projectile.whoAmI;
			player.itemTime = 2;
			player.itemAnimation = 2;

			player.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();
		}

		/// <summary>
		/// Lets the charge lead (and by extension, the player's arm) update the aim in real time.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="speed"></param>
		private void BarrelAim(Player p, Vector2 source, float speed)
		{
			//TODO: this makes the ball rotate in reverse???
			Vector2 aim = Vector2.Normalize(p.RotatedRelativePoint(Main.MouseWorld, true) - source);
			if (aim.HasNaNs())
			{
				aim = -Vector2.UnitY;
			}

			aim = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(Projectile.velocity), aim, 1f));
			aim *= speed;

			if (aim != Projectile.velocity)
			{
				Projectile.netUpdate = true;
			}
			Projectile.velocity = aim;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D ballTex = ModContent.Request<Texture2D>(Texture).Value;
			SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			//All this makes sure that the texture properly draws centered so it rotates and doesn't speen

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);

			DrawData data = new DrawData(ballTex, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, ballTex.Width, ballTex.Height)), ballColor, Projectile.rotation, new Vector2((float)ballTex.Width / 2, (float)ballTex.Height / 2), Projectile.scale, effects, 0);

			MiscShaderData shaderData = GameShaders.Misc["MetroidModPaletteShader"];
			shaderData.UseColor(ballColor); //Primary color is the bright colors
			shaderData.UseSecondaryColor(ballColor2); //Secondary is the dark colors
			shaderData.UseOpacity(coreBrightness); //Affects brightness of the 'core' (the white of the texture)
												   //Defaulting to 1f to keep the core bright
			shaderData.UseSaturation(coreSaturation); //Affects saturation of the 'core'
													  //0 to keep the core white instead of being the primary color
			shaderData.UseImage0(TextureAssets.Projectile[Projectile.type]);

			shaderData.Apply(data);
			data.Draw(Main.spriteBatch);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);

			return false;
		}
	}
}

