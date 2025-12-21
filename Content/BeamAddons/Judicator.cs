using System;
using System.IO;
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
	public class Judicator : ModBeamAddon
	{
		public override bool AddOnlyAddonItem => false;
		public override int ShotDust => 135;
		public override Color PrimaryColor => MetroidMod.waveSecondaryColor;
		public override string ImpactSound => $"{Mod.Name}/Assets/Sounds/BeamAddons/Judicator/Impact";
		public override void SetStaticDefaults()
		{
			AddonSlot = BeamAddonSlotID.Primary;
			VIB = true;
			vibOverride = ModContent.ProjectileType<JudicatorShot>();
			ArrayPassive = false;
			ItemID.Sets.ShimmerTransformToItem[Type] = BeamAddonLoader.GetAddon<ShockCoil>().ItemType;
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
			JudicatorShot tasteTheRainbow = Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<JudicatorShot>(), damage, knockback).ModProjectile as JudicatorShot;
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

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<Items.Miscellaneous.ChoziteBar>(15)
				.AddIngredient<Items.Miscellaneous.EnergyShard>(2)
				.AddIngredient(ItemID.Wire, 25)
				.AddIngredient(ItemID.Amethyst, 1)
				.AddIngredient(ItemID.IceBlock, 99)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
	public class JudicatorShot : MProjectile
	{
		public string fileMod = "";
		public override string Texture => $"{Mod.Name}/Assets/Textures/BeamAddons/Judicator/Shot";
		public ModBeamAddon[] beamAddons = new ModBeamAddon[BeamAddonSlotID.Count - 1];
		public void OnInitialized(IEntitySource source)
		{
			//if (source is EntitySource_Parent parent && parent.Entity is Player player && (player.HeldItem.type == ModContent.ItemType<PowerBeam>() || player.HeldItem.type == ModContent.ItemType<ArmCannon>()))
			//{
			//	//if (player.HeldItem.ModItem is PowerBeam hold)
			//	//{
			//	//	shot = hold.shotEffect.ToString();
			//	//}
			//	//else if (player.HeldItem.ModItem is ArmCannon hold2)
			//	//{
			//	//	shot = hold2.shotEffect.ToString();
			//	//}
			//}
			if (shot.Contains("red"))
			{
				Projectile.penetrate = 2;
				Projectile.maxPenetrate = 2;
			}
			if (shot.Contains("green"))
			{
				Projectile.penetrate = 6;
				Projectile.maxPenetrate = 6;
			}
			if (shot.Contains("nova"))
			{
				Projectile.penetrate = 8;
				Projectile.maxPenetrate = 8;
			}
			if (shot.Contains("solar"))
			{
				Projectile.penetrate = 12;
				Projectile.maxPenetrate = 12;
			}
			//Gather data from installed addons.

			//First, call method to calculate tileinteract total.
			TileInteract = BeamAddonLoader.InteractStacker(beamAddons, true, 2f);
			//Then, call method to calculate entityinteract total.
			EntityInteract = BeamAddonLoader.InteractStacker(beamAddons, false, 2f);


			BeamAddonLoader.AddonOnInitialized(beamAddons, mProjectile, source);
			base.OnSpawn(source);
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 5; //10
			Projectile.height = 11; //22
			Projectile.scale = 1f;
			//Projectile.penetrate = 1;
			//Projectile.aiStyle = 0;
			Projectile.timeLeft = 90;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;

		}

		public override void AI()
		{
			Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.PiOver2;
			Color color = MetroidMod.powColor;
			Lighting.AddLight(Projectile.Center, color.R / 255f, color.G / 100f, color.B / 255f);
			if (Projectile.numUpdates == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 135, 0, 0, 100, default(Color), Projectile.scale);
				Main.dust[dust].noGravity = true;
			}
		}


		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.penetrate <= 0)
			{
				Projectile.Kill();
			}
			else
			{
				Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
				SoundEngine.PlaySound(new($"{Mod.Name}/Assets/Sounds/BeamAddons/Judicator/Impact"), Projectile.position);

				if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon)
				{
					Projectile.velocity.X = -oldVelocity.X;
				}

				if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon)
				{
					Projectile.velocity.Y = -oldVelocity.Y;
				}
			}

			return false;
		}

		public override void OnKill(int timeLeft)
		{
			for (var i = 0; i < 5; i++)
			{
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 135, 0, 0, 100, default(Color), Projectile.scale);
				//SoundEngine.PlaySound(Sounds.Items.Weapons.JudicatorImpactSound, Projectile.position);
			}
			mProjectile.DustyDeath(Projectile, 135);
			SoundEngine.PlaySound(new($"{Mod.Name}/Assets/Sounds/BeamAddons/Judicator/Impact"), Projectile.position);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.DrawCentered(Projectile, Main.spriteBatch);
			return false;
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(Projectile.penetrate);
			writer.Write(Projectile.maxPenetrate);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Projectile.penetrate = reader.ReadInt32();
			Projectile.maxPenetrate = reader.ReadInt32();
		}
	}
	public class JudicatorChargeShot : MProjectile
	{
		public string fileMod = "";
		public override string Texture => $"{Mod.Name}/Assets/Textures/BeamAddons/Judicator/ShotCharged";
		public ModBeamAddon[] beamAddons = new ModBeamAddon[BeamAddonSlotID.Count - 1];
		//todo: add balance for luminite
		private int GetDepth(MProjectile mp)
		{
			return mp.waveDepth;
		}
		private int yeet = 1;
		private Vector2 LineStart;
		private Vector2 LineEnd;
		public void OnInitalized(IEntitySource source)
		{
			base.OnSpawn(source);
			//if (source is EntitySource_Parent parent && parent.Entity is Player player && (player.HeldItem.type == ModContent.ItemType<PowerBeam>() || player.HeldItem.type == ModContent.ItemType<ArmCannon>()))
			//{
			//	if (player.HeldItem.ModItem is PowerBeam hold)
			//	{
			//		shot = hold.shotEffect.ToString();
			//	}
			//	else if (player.HeldItem.ModItem is ArmCannon hold2)
			//	{
			//		shot = hold2.shotEffect.ToString();
			//	}
			//}
			if (shot.Contains("red"))
			{
				Projectile.penetrate = 5;
				Projectile.maxPenetrate = 5;
				yeet = 5;
			}
			if (shot.Contains("green"))
			{
				Projectile.penetrate = 6;
				Projectile.maxPenetrate = 6;
				yeet = 6;
			}
			if (shot.Contains("nova"))
			{
				Projectile.penetrate = 8;
				Projectile.maxPenetrate = 8;
				yeet = 8;
			}
			if (shot.Contains("solar"))
			{
				Projectile.penetrate = 12;
				Projectile.maxPenetrate = 12;
				yeet = 12;
			}
			Projectile.timeLeft = 40;// Luminite ? 60 : 40;
		}
		public override void SetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 20;
			Projectile.scale = 1f;
			Projectile.timeLeft = 40;// Luminite ? 60 : 40;
			base.SetDefaults();
		}

		public override void AI()
		{
			Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.PiOver2;
			//WaveBehavior(Projectile);
			if (Projectile.numUpdates == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 135, 0, 0, 100, default(Color), Projectile.scale);
				Main.dust[dust].noGravity = true;
			}

			//if (Projectile.timeLeft == (Luminite ? 60 : 40)) //shadowfreeze
			//{
			//	Projectile.penetrate = -1;
			//	Projectile.tileCollide = false;
			//	/*if (shot.Contains("wave") || shot.Contains("nebula"))
			//	{
			//		Projectile.tileCollide = false;
			//	}*/
			//	//Projectile.velocity.Normalize();
			//	//Projectile.Center.Floor();
			//	//Projectile.width += (int)Math.Abs((Projectile.velocity.Y * GetDepth(meep)) * Projectile.width);
			//	//Projectile.height += (int)Math.Abs((Projectile.velocity.X * GetDepth(meep)) * Projectile.height);
			//	//Projectile.Center = Main.player[Projectile.owner].Center;
			//	LineStart = new(Projectile.position.X + (Projectile.velocity.Y * GetDepth(mProjectile)), Projectile.position.Y + (Projectile.velocity.X * GetDepth(mProjectile) * 16f));
			//	LineEnd = new(Projectile.position.X - (Projectile.velocity.Y * GetDepth(mProjectile)), Projectile.position.Y - (Projectile.velocity.X * GetDepth(mProjectile) * 16f));
			//}
			//else
			//{
			//	Projectile.tileCollide = true;
			//	Projectile.penetrate = yeet;
			//}
			Color color = MetroidMod.powColor;
			Lighting.AddLight(Projectile.Center, color.R / 255f, color.G / 255f, color.B / 255f);
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			float _ = float.NaN;
			//if (Projectile.timeLeft == (Luminite ? 60 : 40))
			//{
			//	return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), LineStart, LineEnd, Projectile.width, ref _);
			//}
			return base.Colliding(projHitbox, targetHitbox);
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			SoundEngine.PlaySound(new($"{Mod.Name}/Assets/Sounds/BeamAddons/Judicator/Freeze"), Projectile.position);
			target.AddBuff(ModContent.BuffType<Buffs.InstantFreeze>(), 300);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.DrawCentered(Projectile, Main.spriteBatch);
			return false;
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(Projectile.penetrate);
			writer.Write(Projectile.maxPenetrate);
			base.SendExtraAI(writer);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Projectile.penetrate = reader.ReadInt32();
			Projectile.maxPenetrate = reader.ReadInt32();
			base.ReceiveExtraAI(reader);
		}
	}
}
