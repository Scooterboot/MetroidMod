using System.Linq;
using MetroidMod.Common.GlobalItems;
using MetroidMod.Common.Players;
using MetroidMod.Content.Items.Weapons;
using MetroidMod.Content.Projectiles;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.BeamAddons
{
	public class MagMaul : ModBeamAddon
	{
		public override bool AddOnlyAddonItem => false;
		public override int ShotDust => 286;
		public override Color PrimaryColor => MetroidMod.plaRedColor;
		public override string ImpactSound => $"{Mod.Name}/Assets/Sounds/BeamAddons/MagMaul/Impact";
		public override void SetStaticDefaults()
		{
			AddonSlot = BeamAddonSlotID.Primary;
			VIB = true;
			vibOverride = ModContent.ProjectileType<MagMaulShot>();
			ArrayPassive = false;
			ItemID.Sets.ShimmerTransformToItem[Type] = BeamAddonLoader.GetAddon<Imperialist>().ItemType;

			Item.ResearchUnlockCount = 1;
		}
		public override void SetItemDefaults(Item Item)
		{
			Item.width = 10;
			Item.height = 14;
			Item.maxStack = 1;
			Item.value = 50000;
			Item.rare = ItemRarityID.LightRed;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<Items.Miscellaneous.ChoziteBar>(15)
				.AddIngredient<Items.Miscellaneous.EnergyShard>(2)
				.AddIngredient(ItemID.HellstoneBar, 15)
				.AddIngredient(ItemID.PinkGel, 25)
				.AddIngredient(ItemID.Amber, 1)
				.AddTile(TileID.Hellforge)
				.Register();
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
			MagMaulShot tasteTheRainbow = Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<MagMaulShot>(), damage, knockback).ModProjectile as MagMaulShot;
			tasteTheRainbow.beamAddons = [.. wepon.BeamAddonAccess
				.Select(BeamAddonLoader.GetAddon)
				.Select(i => i?.Clone())];
			tasteTheRainbow.fileMod = SetStaticCombos(wepon.BeamAddonAccess);
			tasteTheRainbow.OnInitialized(source);

			//if (theShootsingAmount > 0)
			//{
			//	//tasteTheRainbow.groupSize = theShootsingAmount + 1;
			//	//Now we create the "baby" projectiles.
			//	//Yeah yeah obvious joke is obvious this horse has been dead for ages
			//	for (int i = 0; i < theShootsingAmount; i++)
			//	{
			//		MetroidMod.Instance.Logger.Info("Non-canon! " + (i + 1) + "/" + theShootsingAmount);
			//		HyperBeamExtraShot stray = Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<HyperBeamExtraShot>(), damage, knockback).ModProjectile as HyperBeamExtraShot;
			//		stray.beamAddons = wepon.BeamAddonAccess
			//			.Select(i => BeamAddonLoader.GetAddon(i))
			//			.Select(i => i?.Clone())
			//			.ToArray();
			//		stray.mother = tasteTheRainbow;
			//		stray.groupSize = theShootsingAmount;
			//		stray.groupID = i;

			//		stray.OnInitialized(source);
			//	}
			//}
		}
		public class MagMaulShot : MProjectile
		{
			public string fileMod = "";
			public override string Texture => $"{Mod.Name}/Assets/Textures/BeamAddons/MagMaul/Shot";
			public ModBeamAddon[] beamAddons = new ModBeamAddon[BeamAddonSlotID.Count - 1];
			public override void SetDefaults()
			{
				base.SetDefaults();
				Projectile.width = 20;
				Projectile.height = 20;
				Projectile.scale = 1f;
				Projectile.penetrate = 1;
				Projectile.aiStyle = 14;
				Projectile.tileCollide = true;
				Projectile.usesLocalNPCImmunity = true;
				Projectile.localNPCHitCooldown = 1;
			}
			public void OnInitialized(IEntitySource source)
			{
				//Gather data from installed addons.

				//First, call method to calculate tileinteract total.
				TileInteract = BeamAddonLoader.InteractStacker(beamAddons, true, 2f);
				//Then, call method to calculate entityinteract total.
				EntityInteract = BeamAddonLoader.InteractStacker(beamAddons, false, 2f);


				BeamAddonLoader.AddonOnInitialized(beamAddons, mProjectile, source);
			}
			public override void AI()
			{
				//Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + MathHelper.PiOver2;
				Color color = MetroidMod.powColor;
				Lighting.AddLight(Projectile.Center, color.R / 255f, color.G / 255f, color.B / 255f);

				if (Projectile.numUpdates == 0)
				{
					int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 286, 0, 0, 100, default(Color), Projectile.scale);
					Main.dust[dust].noGravity = true;
				}
			}

			public override void OnKill(int timeLeft)
			{
				SoundEngine.PlaySound(new($"{Mod.Name}/Assets/Sounds/BeamAddons/MagMaul/Impact"), Projectile.position);
				//if(timeLeft <=0) [Joost] Let's be real, waiting for the timeout just for the explosion is never happening
				//{
				//mProjectile.Explode(Luminite ? 80 : DiffBeam ? 60 : 40, 1.6f);
				//}
				mProjectile.Diffuse(Projectile, 286);
			}
			public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
			{
				BeamAddonLoader.AddonOnHitNPC(beamAddons, mProjectile, target, hit, damageDone);
			}
			public override void OnHitPlayer(Player target, Player.HurtInfo info)
			{
				BeamAddonLoader.AddonOnHitPlayer(beamAddons, mProjectile, target, info);
			}
			public override bool PreDraw(ref Color lightColor)
			{
				mProjectile.DrawCentered(Projectile, Main.spriteBatch);
				return false;
			}
		}
	}
	public class MagMaulChargeShot : MProjectile
	{
		public string fileMod = "";
		public override string Texture => $"{Mod.Name}/Assets/Textures/BeamAddons/MagMaul/Shot";
		public ModBeamAddon[] beamAddons = new ModBeamAddon[BeamAddonSlotID.Count - 1];
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.scale = 1.5f;
			Projectile.aiStyle = 1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 1;
		}
		public void OnInitialized(IEntitySource source)
		{
			//Gather data from installed addons.

			//First, call method to calculate tileinteract total.
			TileInteract = BeamAddonLoader.InteractStacker(beamAddons, true, 2f);
			//Then, call method to calculate entityinteract total.
			EntityInteract = BeamAddonLoader.InteractStacker(beamAddons, false, 2f);


			BeamAddonLoader.AddonOnInitialized(beamAddons, mProjectile, source);
		}
		public override void AI()
		{
			Color color = MetroidMod.powColor;
			Lighting.AddLight(Projectile.Center, color.R / 255f, color.G / 255f, color.B / 255f);
			Projectile.rotation += 0.5f * Projectile.direction;
			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 286, 0, 0, 100, default(Color), Projectile.scale);
			Main.dust[dust].noGravity = true;
		}

		public override void OnKill(int timeLeft)
		{
			SoundEngine.PlaySound(new($"{Mod.Name}/Assets/Sounds/BeamAddons/MagMaul/Impact"), Projectile.position);
			//mProjectile.Explode(Luminite ? 80 : DiffBeam ? 60 : 40, 1f/*, Luminite || DiffBeam ? .59f : .53f*/);
			mProjectile.DustyDeath(Projectile, 286);
		}
		public override bool? CanHitNPC(NPC target)
		{
			if (Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, target.position, target.width, target.height) && Projectile.Hitbox.Intersects(target.Hitbox))
			{
				return null;
			}
			return false;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			//if (Luminite || DiffBeam)
			//{
			//	target.AddBuff(24, Luminite ? 800 : 600);
			//}
			BeamAddonLoader.AddonOnHitNPC(beamAddons, mProjectile, target, hit, damageDone);
		}
		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			BeamAddonLoader.AddonOnHitPlayer(beamAddons, mProjectile, target, info);
		}
		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.DrawCentered(Projectile, Main.spriteBatch);
			return false;
		}
	}
}

