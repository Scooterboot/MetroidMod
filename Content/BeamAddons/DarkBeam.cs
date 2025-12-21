using System;
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
	public class DarkBeam : ModBeamAddon
	{
		public override bool AddOnlyAddonItem => false;
		public override int ShotDust => 269;
		public override Color PrimaryColor => MetroidMod.powColor;
		//public override string ImpactSound => $"{Mod.Name}/Assets/Sounds/BeamAddons/VoltDriver/Impact";
		public override void SetStaticDefaults()
		{
			AddonSlot = BeamAddonSlotID.Primary;
			VIB = true;
			vibOverride = ModContent.ProjectileType<DarkBeamShot>();
			ArrayPassive = false;
			//ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<MagMaulAddon>();
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

		//public override void AddRecipes()
		//{
		//	CreateRecipe(1)
		//		.AddIngredient<Items.Miscellaneous.ChoziteBar>(15)
		//		.AddIngredient<Items.Miscellaneous.EnergyShard>(2)
		//		.AddIngredient(ItemID.CopperBar, 10)
		//		.AddIngredient(ItemID.Topaz, 1)
		//		.AddIngredient(ItemID.Wire, 30)
		//		.AddTile(TileID.Anvils)
		//		.Register();
		//	CreateRecipe(1)
		//		.AddIngredient<Items.Miscellaneous.ChoziteBar>(15)
		//		.AddIngredient<Items.Miscellaneous.EnergyShard>(2)
		//		.AddIngredient(ItemID.TinBar, 10)
		//		.AddIngredient(ItemID.Topaz, 1)
		//		.AddIngredient(ItemID.Wire, 30)
		//		.AddTile(TileID.Anvils)
		//		.Register();
		//}
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
			DarkBeamShot tasteTheRainbow = Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<DarkBeamShot>(), damage, knockback).ModProjectile as DarkBeamShot;
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
	}
	public class DarkBeamShot : MProjectile
	{
		public string fileMod = "";
		public override string Texture => $"{Mod.Name}/Assets/Textures/BeamAddons/DarkBeam/Shot";
		public ModBeamAddon[] beamAddons = new ModBeamAddon[BeamAddonSlotID.Count - 1];
		public void OnInitialized(IEntitySource source)
		{
			//Gather data from installed addons.

			//First, call method to calculate tileinteract total.
			TileInteract = BeamAddonLoader.InteractStacker(beamAddons, true, 2f);
			//Then, call method to calculate entityinteract total.
			EntityInteract = BeamAddonLoader.InteractStacker(beamAddons, false, 2f);


			BeamAddonLoader.AddonOnInitialized(beamAddons, mProjectile, source);
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 12;//22
			Projectile.height = 12; //22
			Projectile.scale = 1f;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
			//Projectile.extraUpdates = 3;
		}
		public override void AI()
		{
			Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.PiOver2;
			Color color = MetroidMod.powColor;
			Lighting.AddLight(Projectile.Center, color.R / 255f, color.G / 255f, color.B / 75f);
			if (Projectile.numUpdates == 0)
			{
				Projectile.frame++;
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 269, 0, 0, 100, default(Color), Projectile.scale);
				Main.dust[dust].noGravity = true;
			}
			if (Projectile.frame > 3)
			{
				Projectile.frame = 0;
			}
		}
		public override void OnKill(int timeLeft)
		{
			mProjectile.DustyDeath(Projectile, 269);
			SoundEngine.PlaySound(new($"{Mod.Name}/Assets/Sounds/BeamAddons/VoltDriver/Impact"), Projectile.position);
		}

		public override bool PreDraw(ref Color DarkColor)
		{
			mProjectile.DrawCentered(Projectile, Main.spriteBatch);
			return false;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			BeamAddonLoader.AddonOnHitNPC(beamAddons, mProjectile, target, hit, damageDone);
		}
		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			BeamAddonLoader.AddonOnHitPlayer(beamAddons, mProjectile, target, info);
		}
	}
	public class DarkBeamChargeShot : MProjectile
	{
		public string fileMod = "";
		public override string Texture => $"{Mod.Name}/Assets/Textures/BeamAddons/DarkBeam/ShotCharged";
		public ModBeamAddon[] beamAddons = new ModBeamAddon[BeamAddonSlotID.Count - 1];
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 4;
		}
		public override void SetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.scale = 1f;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 1;
			Projectile.penetrate = 1;
			Projectile.extraUpdates = 0;
			base.SetDefaults();
		}

		public override void AI()
		{
			//float shootSpeed = Luminite ? 4f : 2f;
			Color color = MetroidMod.powColor;
			Lighting.AddLight(Projectile.Center, color.R / 255f, color.G / 255f, color.B / 75f);
			if (Projectile.numUpdates == 0)
			{
				Projectile.rotation += 0.5f * Projectile.direction;
				Projectile.frame++;
			}
			if (Projectile.frame > 3)
			{
				Projectile.frame = 0;
			}
			mProjectile.DustLine(Projectile.Center, Projectile.velocity, Projectile.rotation, 5, 3, DustID.Sandnado, 2f);
			//mProjectile.HomingBehavior(Projectile, shootSpeed, 11f, !DiffBeam && !Luminite ? 0f : 300f);
			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Sandnado, 0, 0, 100, default(Color), Projectile.scale);
			Main.dust[dust].noGravity = true;
		}

		public override void OnKill(int timeLeft)
		{
			//if (Luminite || DiffBeam)
			//{
			//	mProjectile.Explode(Luminite ? 88 : DiffBeam ? 44 : 22, Luminite ? 4f : DiffBeam ? 3f : 2f);
			//}
			mProjectile.DustyDeath(Projectile, 269);
			SoundEngine.PlaySound(Sounds.Items.Weapons.VoltDriverChargeImpactSound, Projectile.position);
		}
		public override bool? CanHitNPC(NPC target)
		{
			if (Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, target.position, target.width, target.height) && Projectile.Hitbox.Intersects(target.Hitbox))
			{
				return null;
			}
			return false;
		}
		public override bool PreDraw(ref Color DarkColor)
		{
			mProjectile.DrawCentered(Projectile, Main.spriteBatch);
			return false;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			//if (target.active && !target.buffImmune[31] && (Luminite || DiffBeam))
			//{
			//	SoundEngine.PlaySound(Sounds.Items.Weapons.VoltDriverDaze, target.position);
			//	target.AddBuff(31, 180);
			//}
			base.OnHitNPC(target, hit, damageDone);
		}
		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			//if (target.active && !target.buffImmune[31] && (Luminite || DiffBeam))
			//{
			//	SoundEngine.PlaySound(Sounds.Items.Weapons.VoltDriverDaze, target.position);
			//	target.AddBuff(31, 180);
			//}
			base.OnHitPlayer(target, info);
		}
	}
}
