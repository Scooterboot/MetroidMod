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
using static Terraria.ModLoader.PlayerDrawLayer;

namespace MetroidMod.Content.BeamAddons
{
	/// <summary>
	/// the baby
	/// </summary>
    class HyperBeam : ModBeamAddon
    {
		//As this is the first VIB addon, it will serve as an example.

		#region Stat values
		int bd = 200;
		float dm = 0;
		int bs = 0;
		float sm = 0;
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
			float num4 = (float)(item.height - tex.Height);
			float num5 = (float)(item.width / 2 - tex.Width / 2);
			sb.Draw(tex, new Vector2(item.position.X - Main.screenPosition.X + (float)(tex.Width / 2) + num5, item.position.Y - Main.screenPosition.Y + (float)(tex.Height / 2) + num4 + 2f), new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), rotation, new Vector2((float)(tex.Width / 2), (float)(tex.Height / 2)), num3, SpriteEffects.None, 0f);
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
				new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero,
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
				//Choose something unique that describes the particular combination to make things easier on yourself.
				//For instance, this keyword is "Fuck" because  F U C K .
				return "Plasma";
			}
			else
			{
				//Return blank if it doesn't get anything, keeps things clean.
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

			float[] edgeCaseStuff = [0, 0, 0, 0, 0];
			int theShootsingAmount = (int)wepon.AdditionalBeamStats[9];

			HyperBeamShot tasteTheRainbow = (Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<HyperBeamShot>(), damage, knockback).ModProjectile) as HyperBeamShot;
			tasteTheRainbow.beamAddons = wepon.BeamAddonAccess
				.Select(i => BeamAddonLoader.GetAddon(i))
				.Select(i => i?.Clone())
				.ToArray();
			tasteTheRainbow.fileMod = SetStaticCombos(wepon.BeamAddonAccess);
			tasteTheRainbow.OnInitialized(source);

			if (theShootsingAmount > 0)
			{
				for (int i = 0; i < theShootsingAmount; i++)
				{
					HyperBeamExtraShot stray = (Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<HyperBeamExtraShot>(), damage, knockback).ModProjectile) as HyperBeamExtraShot;
					stray.mother = tasteTheRainbow;
					stray.beamAddons = wepon.BeamAddonAccess
						.Select(i => BeamAddonLoader.GetAddon(i))
						.Select(i => i?.Clone())
						.ToArray();
					stray.groupSize = theShootsingAmount;
					stray.groupID = i;

					stray.OnInitialized(source);
				}
			}

			//I think also this is where the, uh, rainbow-y code goes?? for making you go all rainbow?
			mp.hyperColors = 60;
		}


		#endregion
	}
	//I failed to resist the temptation to move this into the Hyper Beam file
	//if I did it with charge lead I'm obligated by rule of consistency to do it here
	//My apologies for my crimes against good coding practices

	public class HyperBeamShot : MProjectile
	{
		//todo: this is dumb
		//rewrite like most of this
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
		float scale = 0f;
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
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + MathHelper.PiOver2;
			scale = Projectile.scale;
		}
		public override void AI()
		{
			MPlayer mp = Main.player[Projectile.owner].GetModPlayer<MPlayer>();

			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + MathHelper.PiOver2;

			Lighting.AddLight(Projectile.Center, (float)mp.r / 255f, (float)mp.g / 255f, (float)mp.b / 255f);

			Projectile.localAI[0] = Math.Min(Projectile.localAI[0] + 0.075f, 1f);
			Projectile.localAI[1] = Math.Min(Projectile.localAI[1] + 0.025f, 1f);

			Projectile.scale = scale * Projectile.localAI[0];

			BeamAddonLoader.AddonAI(beamAddons, mProjectile);
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			return BeamAddonLoader.AddonTileCollideStyle(beamAddons, mProjectile, ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return BeamAddonLoader.AddonOnTileCollide(beamAddons, mProjectile, oldVelocity);
		}

		//public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		//{
		//	BeamAddonLoader.AddonOnHitNPC(beamAddons, mProjectile, target, hit, damageDone);
		//}
		//public override void OnHitPlayer(Player target, Player.HurtInfo info)
		//{
		//	BeamAddonLoader.AddonOnHitPlayer(beamAddons, mProjectile, target, info);
		//}

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
			float scale = 0.65f;
			if (fileMod.Contains("Plasma"))
			{
				scale = 1f;
			}
			MPlayer mp = Main.player[Projectile.owner].GetModPlayer<MPlayer>();
			mProjectile.PlasmaDrawTrail(Projectile, Main.player[Projectile.owner], Main.spriteBatch, 10, scale * Projectile.localAI[0] * Projectile.localAI[1], new Color(mp.r, mp.g, mp.b, 128));
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
		public MProjectile mother;



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
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + MathHelper.PiOver2;

			mProjectile.corePosition = mother.corePosition;

			//First, call method to calculate tileinteract total.
			TileInteract = BeamAddonLoader.InteractStacker(beamAddons, true, 2f);
			//Then, call method to calculate entityinteract total.
			EntityInteract = BeamAddonLoader.InteractStacker(beamAddons, false, 2f);

			BeamAddonLoader.AddonOnInitialized(beamAddons, mProjectile, source);
		}

		public override void AI()
		{
			MPlayer mp = Main.player[Projectile.owner].GetModPlayer<MPlayer>();

			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + MathHelper.PiOver2;

			Lighting.AddLight(Projectile.Center, (float)mp.r / 255f, (float)mp.g / 255f, (float)mp.b / 255f);

			BeamAddonLoader.AddonAI(beamAddons, mProjectile);
		}

		public override void PostAI()
		{
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
			MPlayer mp = Main.player[Projectile.owner].GetModPlayer<MPlayer>();
			mProjectile.DustyDeath(Projectile, 66, true, 1f, new Color(mp.r, mp.g, mp.b, 255));
		}
		#endregion

		public override bool PreDraw(ref Color lightColor)
		{
			MPlayer mp = Main.player[Projectile.owner].GetModPlayer<MPlayer>();
			mProjectile.PlasmaDrawTrail(Projectile, Main.player[Projectile.owner], Main.spriteBatch, 10, Projectile.scale * Projectile.localAI[0] * Projectile.localAI[1], new Color(mp.r, mp.g, mp.b, 128));
			return false;
		}
	}
}
