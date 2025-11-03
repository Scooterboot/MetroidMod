using System;
using System.Linq;
using MetroidMod.Common.GlobalItems;
using MetroidMod.Common.Players;
using MetroidMod.Content.Items.Weapons;
using MetroidMod.Content.Projectiles;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.BeamAddons
{
	/// <summary>
	/// the baby
	/// </summary>
	internal class HyperBeam : ModBeamAddon
	{
		//As this is the first VIB addon, it will serve as an example.

		#region Stat values
		private readonly int bd = 200;
		private readonly float dm = 0;
		private readonly int bs = 0;
		private readonly float sm = 0;
		#endregion

		public override Color PrimaryColor => Color.White;
		public override float CoreSaturation => 0.5f;

		public override int ShotDust => DustID.RainbowTorch;

		public override bool AddOnlyAddonItem => false;

		public override void SetStaticDefaults()
		{
			AddonSlot = BeamAddonSlotID.Primary;
			VIB = true;
			vibOverride = ModContent.ProjectileType<HyperBeamShot>();
			ArrayPassive = false;

			BaseDamage = bd;
			DamageMult = dm;
			BaseSpeed = bs;
			SpeedMult = sm;
		}

		public override void SetItemDefaults(Item item)
		{

			item.rare = ItemRarityID.Expert;
		}

		#region taste the rainbow
		//All the code for the rainbow effects on the sprite

		//TODO: Rainbow effect doesn't work on item/tile previews
		public override void PostDrawInInventory(SpriteBatch sb, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D tex = ModContent.Request<Texture2D>(ItemTexture + "Rainbow").Value;
			drawColor = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
			sb.Draw(tex, position, frame, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
		}
		public override void PostDrawInWorld(Item item, SpriteBatch sb, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			DrawColors(item, sb);//, Main.player[Item.owner]);
			lightColor = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
			alphaColor = lightColor;
		}
		public void DrawColors(Item item, SpriteBatch sb)//, Player player)
		{
			//MetroidMod.Instance.Logger.Debug("CONSOLESPAM");
			Texture2D tex = ModContent.Request<Texture2D>(ItemTexture + "Rainbow").Value;
			float rotation = item.velocity.X * 0.2f;
			float num3 = 1f;
			float num4 = item.height - tex.Height;
			float num5 = (item.width / 2) - (tex.Width / 2);
			sb.Draw(tex, new Vector2(item.position.X - Main.screenPosition.X + (tex.Width / 2) + num5, item.position.Y - Main.screenPosition.Y + (tex.Height / 2) + num4 + 2f), new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), rotation, new Vector2(tex.Width / 2, tex.Height / 2), num3, SpriteEffects.None, 0f);
		}

		public override void PostDrawTile(int i, int j, SpriteBatch sb)
		{
			Tile tile = Main.tile[i, j];
			Vector2 zero = new(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			sb.Draw(ModContent.Request<Texture2D>(TileTexture + "Rainbow").Value,
				new Vector2((i * 16) - (int)Main.screenPosition.X, (j * 16) - (int)Main.screenPosition.Y) + zero,
				new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16),
				new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB),
				0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
		}

		public override void PostDrawPlacementPreview(int i, int j, SpriteBatch sb, Rectangle frame, Vector2 position, Color color, bool validPlacement, SpriteEffects spriteEffects)
		{
			Tile tile = Main.tile[i, j];
			Vector2 zero = new(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			if (validPlacement)
			{
				sb.Draw(ModContent.Request<Texture2D>(TileTexture + "Rainbow").Value,
					position,
					new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16),
					new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB),
					0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
		}
		#endregion

		#region The juicy stuff

		//If you use a custom projectile, you're gonna have to do more legwork to get static combos to work
		//See HyperBeamShot for a better idea
		public override string SetStaticCombos(Item[] addons)
		{
			ModBeamAddon[] beamAddons = addons
				.Select(BeamAddonLoader.GetAddon)
				.ToArray();
			bool hasPlasma = false;


			if (beamAddons[BeamAddonSlotID.Secondary] == BeamAddonLoader.GetAddon<PlasmaBeam>())
			{
				hasPlasma = true;
			}

			if (hasPlasma)
			{
				return "Plasma";
			}
			else
			{
				return "";
			}
		}

		public override void VIBShoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, string bonusFileMod = "", float multiplier = 1)
		{
			//copypaste most of the SpawnBeam stuff here
			MPlayer mp = player.GetModPlayer<MPlayer>(); //finds the current player's MPlayer data for later modification
			MGlobalItem ac = Item.GetGlobalItem<MGlobalItem>();
			Vector2 oPos = player.RotatedRelativePoint(player.MountedCenter, true);
			float speedX = velocity.X;
			float speedY = velocity.Y;

			ArmCannon wepon = (ArmCannon)item.ModItem;

			string fileMod = SetStaticCombos(wepon.BeamAddonAccess);
			float[] edgeCaseStuff = BeamAddonLoader.EdgeCaseStacker(wepon.BeamAddonAccess, wepon.AdditionalBeamStats, fileMod);
			int theShootsingAmount = (int)wepon.AdditionalBeamStats[9] + (int)edgeCaseStuff[4]; //The arm cannon adds an extra one to account for the base shot. Hyper don't need that.

			//for what's mostly just copy-pasting stuff that's already been written this was surprisingly annoying to implement

			//Generate the "mother" projectile first.
			//This one's bigger and stronger than the babies.
			HyperBeamShot tasteTheRainbow = Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<HyperBeamShot>(), damage, knockback).ModProjectile as HyperBeamShot;
			tasteTheRainbow.beamAddons = wepon.BeamAddonAccess
				.Select(i => BeamAddonLoader.GetAddon(i))
				.Select(i => i?.Clone())
				.ToArray();
			tasteTheRainbow.fileMod = SetStaticCombos(wepon.BeamAddonAccess);
			tasteTheRainbow.OnInitialized(source);

			if (theShootsingAmount > 0)
			{
				//tasteTheRainbow.groupSize = theShootsingAmount + 1;
				//Now we create the "baby" projectiles.
				//Yeah yeah obvious joke is obvious this horse has been dead for ages
				for (int i = 0; i < theShootsingAmount; i++)
				{
					MetroidMod.Instance.Logger.Info("Non-canon! " + (i + 1) + "/" + theShootsingAmount);
					HyperBeamExtraShot stray = Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<HyperBeamExtraShot>(), damage, knockback).ModProjectile as HyperBeamExtraShot;
					stray.beamAddons = wepon.BeamAddonAccess
						.Select(i => BeamAddonLoader.GetAddon(i))
						.Select(i => i?.Clone())
						.ToArray();
					stray.mother = tasteTheRainbow;
					stray.groupSize = theShootsingAmount;
					stray.groupID = i;

					stray.OnInitialized(source);
				}
			}

			//Setting this value above 0 will make you go rainbowy for that many frames upon firing.
			mp.hyperColors = 80;
		}


		#endregion
	}
	//I failed to resist the temptation to move this into the Hyper Beam file
	//if I did it with charge lead I'm obligated by rule of consistency to do it here
	//My apologies for my crimes against good coding practices

	//Anyway here's the custom projectile for the Hyper Beam
	//It does a handful of things that are useful to know about if you want to make a custom VIB projectile
	public class HyperBeamShot : MProjectile
	{
		public override string Texture => $"{Mod.Name}/Assets/Textures/BeamAddons/HyperBeam/Shot";

		public ModBeamAddon[] beamAddons = new ModBeamAddon[BeamAddonSlotID.Count - 1]; //Hyper Beam doesn't need the ammo slot (doesn't use ammo)

		/// <summary>
		/// This string is appended to the end of the shot's texturepath to find unique textures for a specific combination of beams.
		/// </summary>
		public string fileMod = "";



		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Hyper Beam Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.scale = 2f;
		}

		#region Projectile AI
		private float scale = 0f;
		public void OnInitialized(IEntitySource source)
		{
			//Gather data from installed addons.

			//First, call method to calculate tileinteract total.
			TileInteract = BeamAddonLoader.InteractStacker(beamAddons, true, 2f);
			//Then, call method to calculate entityinteract total.
			EntityInteract = BeamAddonLoader.InteractStacker(beamAddons, false, 2f);


			BeamAddonLoader.AddonOnInitialized(beamAddons, mProjectile, source);
		}
		public override void OnSpawn(IEntitySource source)
		{
			base.OnSpawn(source);
			Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.PiOver2;
			scale = Projectile.scale;
		}
		public override void AI()
		{
			MPlayer mp = Main.player[Projectile.owner].GetModPlayer<MPlayer>();

			Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.PiOver2;

			Lighting.AddLight(Projectile.Center, mp.r / 255f, mp.g / 255f, mp.b / 255f);

			Projectile.localAI[0] = Math.Min(Projectile.localAI[0] + 0.075f, 1f);
			Projectile.localAI[1] = Math.Min(Projectile.localAI[1] + 0.025f, 1f);

			Projectile.scale = scale * Projectile.localAI[0];

			BeamAddonLoader.AddonAI(beamAddons, mProjectile);
		}

		//No PostAI injection because we don't want the main shot to do movement patterns
		public override void PostAI()
		{
			base.PostAI();
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			return BeamAddonLoader.AddonTileCollideStyle(beamAddons, mProjectile, ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return BeamAddonLoader.AddonOnTileCollide(beamAddons, mProjectile, oldVelocity);
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			BeamAddonLoader.AddonOnHitNPC(beamAddons, mProjectile, target, hit, damageDone);
		}
		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			BeamAddonLoader.AddonOnHitPlayer(beamAddons, mProjectile, target, info);
		}

		public override void OnKill(int timeLeft)
		{
			MPlayer mp = Main.player[Projectile.owner].GetModPlayer<MPlayer>();
			mProjectile.DustyDeath(Projectile, 66, true, 1f, new Color(mp.r, mp.g, mp.b, 255));
		}
		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			modifiers.ArmorPenetration += 50;
			base.ModifyHitNPC(target, ref modifiers);
		}
		#endregion

		public override bool PreDraw(ref Color lightColor)
		{
			MPlayer mp = Main.player[Projectile.owner].GetModPlayer<MPlayer>();
			mProjectile.PlasmaDrawTrail(Projectile, Main.player[Projectile.owner], Main.spriteBatch, Texture + fileMod, 10, scale * Projectile.localAI[0] * Projectile.localAI[1], new Color(mp.r, mp.g, mp.b, 128));
			return false;
		}

	}

	public class HyperBeamExtraShot : MProjectile
	{
		public override string Texture => $"{Mod.Name}/Assets/Textures/BeamAddons/HyperBeam/ExtraShot";

		public ModBeamAddon[] beamAddons = new ModBeamAddon[BeamAddonSlotID.Count - 1]; //Hyper Beam doesn't need the ammo slot (doesn't use ammo)

		/// <summary>
		/// This string is appended to the end of the shot's texturepath to find unique textures for a specific combination of beams.
		/// </summary>
		public string fileMod = "";

		/// <summary>
		/// The extra-shot's parent (or "mother") projectile.
		/// </summary>
		public HyperBeamShot mother;



		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.scale = 1f;
		}

		#region Projectile AI

		public void OnInitialized(IEntitySource source)
		{
			Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.PiOver2;

			corePosition = mother.corePosition;
			MetroidMod.Instance.Logger.Info("WHY THIS NOT SPAWNING: corePos? " + corePosition);
			//First, call method to calculate tileinteract total.
			TileInteract = BeamAddonLoader.InteractStacker(beamAddons, true, 2f);
			//Then, call method to calculate entityinteract total.
			EntityInteract = BeamAddonLoader.InteractStacker(beamAddons, false, 2f);

			BeamAddonLoader.AddonOnInitialized(beamAddons, mProjectile, source);
		}

		public override void AI()
		{
			MPlayer mp = Main.player[Projectile.owner].GetModPlayer<MPlayer>();

			Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.PiOver2;

			Lighting.AddLight(Projectile.Center, mp.r / 255f, mp.g / 255f, mp.b / 255f);
			BeamAddonLoader.AddonAI(beamAddons, mProjectile);
		}

		public override void PostAI()
		{
			//need to make an altered copy of the base PostAI so the corePos updates off of the mother

			for (int i = Projectile.oldPos.Length - 1; i > 0; i--)
			{
				Projectile.oldPos[i] = Projectile.oldPos[i - 1];
			}
			Projectile.oldPos[0] = Projectile.position;


			for (int i = Projectile.oldRot.Length - 1; i > 0; i--)
			{
				Projectile.oldRot[i] = Projectile.oldRot[i - 1];
			}
			Projectile.oldRot[0] = Projectile.rotation;

			corePosition += mother.Projectile.velocity;

			BeamAddonLoader.AddonPostAI(beamAddons, mProjectile);
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			return BeamAddonLoader.AddonTileCollideStyle(beamAddons, mProjectile, ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return BeamAddonLoader.AddonOnTileCollide(beamAddons, mProjectile, oldVelocity);
		}

		public override void OnKill(int timeLeft)
		{
			MetroidMod.Instance.Logger.Info("AAAAAAUUUUUUUGGGHHH 2");
			MPlayer mp = Main.player[Projectile.owner].GetModPlayer<MPlayer>();
			mProjectile.DustyDeath(Projectile, 66, true, 1f, new Color(mp.r, mp.g, mp.b, 255));
		}
		#endregion

		public override bool PreDraw(ref Color lightColor)
		{
			MPlayer mp = Main.player[Projectile.owner].GetModPlayer<MPlayer>();
			mProjectile.PlasmaDrawTrail(Projectile, Main.player[Projectile.owner], Main.spriteBatch, null, 10, Projectile.scale, new Color(mp.r, mp.g, mp.b, 128));

			return false;
		}
	}
}
