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
using Terraria.WorldBuilding;

namespace MetroidMod.Content.BeamAddons
{
	public class BattleHammer : ModBeamAddon
	{
		public override bool AddOnlyAddonItem => false;
		public override int ShotDust => 110;
		public override Color PrimaryColor => MetroidMod.plaGreenColor;
		public override string ImpactSound => $"{Mod.Name}/Assets/Sounds/BeamAddons/BattleHammer/Impact";
		public override void SetStaticDefaults()
		{
			AddonSlot = BeamAddonSlotID.Primary;
			VIB = true;
			vibOverride = ModContent.ProjectileType<BattleHammerShot>();
			ArrayPassive = false;
			ItemID.Sets.ShimmerTransformToItem[Type] = BeamAddonLoader.GetAddon<VoltDriver>().ItemType;
			Item.ResearchUnlockCount = 1;
		}
		public override void SetItemDefaults(Item item)
		{
			item.width = 10;
			item.height = 14;
			item.maxStack = 1;
			item.value = 50000;
			item.rare = ItemRarityID.LightRed;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.Swing;
			item.consumable = true;

		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.GoldBar, 8)
				.AddIngredient<Items.Miscellaneous.EnergyShard>(2)
				.AddIngredient<Items.Miscellaneous.ChoziteBar>(15)
				.AddIngredient(ItemID.JungleSpores, 15)
				.AddIngredient(ItemID.Emerald, 1)
				.AddIngredient(ItemID.Grenade, 20)
				.AddTile(TileID.Anvils)
				.Register();
			CreateRecipe(1)
				.AddIngredient(ItemID.PlatinumBar, 8)
				.AddIngredient<Items.Miscellaneous.EnergyShard>(2)
				.AddIngredient<Items.Miscellaneous.ChoziteBar>(15)
				.AddIngredient(ItemID.JungleSpores, 15)
				.AddIngredient(ItemID.Emerald, 1)
				.AddIngredient(ItemID.Grenade, 20)
				.AddTile(TileID.Anvils)
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
			BattleHammerShot tasteTheRainbow = Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<BattleHammerShot>(), damage, knockback).ModProjectile as BattleHammerShot;
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
	public class BattleHammerShot : MProjectile
	{
		public string fileMod = "";
		public override string Texture => $"{Mod.Name}/Assets/Textures/BeamAddons/BattleHammer/Shot";
		public ModBeamAddon[] beamAddons = new ModBeamAddon[BeamAddonSlotID.Count - 1];
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.scale = .75f;
			Projectile.aiStyle = ProjAIStyleID.Arrow;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 1;
		}
		private bool oof = true;
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
			Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.PiOver2;
			Color color = MetroidMod.powColor;
			Lighting.AddLight(Projectile.Center, color.R / 50f, color.G / 255f, color.B / 50f);

			if (Projectile.numUpdates == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PureSpray, 0, 0, 100, default(Color), Projectile.scale);
				Main.dust[dust].noGravity = true;
			}
		}
		public override void OnKill(int timeLeft)
		{
			SoundEngine.PlaySound(new($"{Mod.Name}/Assets/Sounds/BeamAddons/BattleHammer/Impact"), Projectile.position);
			//mProjectile.Explode(Luminite ? 80 : DiffBeam ? 60 : 20, 3);
			mProjectile.Explode(60, 3);
			mProjectile.Diffuse(Projectile, 110);
			mProjectile.Diffuse(Projectile, 55);
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			oof = false;
			return base.OnTileCollide(oldVelocity);
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			return BeamAddonLoader.AddonTileCollideStyle(beamAddons, mProjectile, ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}
		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			if (oof)
			{
				modifiers.ArmorPenetration += 15;// Luminite ? 15 : DiffBeam ? 10 : 5;
			}

			base.ModifyHitNPC(target, ref modifiers);
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			BeamAddonLoader.AddonOnHitNPC(beamAddons, mProjectile, target, hit, damageDone);
		}
		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			BeamAddonLoader.AddonOnHitPlayer(beamAddons, mProjectile, target, info);
		}
		public override bool? CanHitNPC(NPC target)
		{
			if (Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, target.position, target.width, target.height) && Projectile.Hitbox.Intersects(target.Hitbox))
			{
				return null;
			}
			return false;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.DrawCentered(Projectile, Main.spriteBatch);
			return false;
		}
	}
}
