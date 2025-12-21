using System;
using System.Linq;
using MetroidMod.Common.GlobalItems;
using MetroidMod.Common.Players;
using MetroidMod.Content.DamageClasses;
using MetroidMod.Content.Dusts;
using MetroidMod.Content.Items.Weapons;
using MetroidMod.Content.Projectiles;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.BeamAddons
{
	public class OmegaCannon : ModBeamAddon
	{
		public override bool AddOnlyAddonItem => false;
		public override int ShotDust => 64;
		public override Color PrimaryColor => MetroidMod.powColor;
		public override void SetStaticDefaults()
		{
			AddonSlot = BeamAddonSlotID.Primary;
			VIB = true;
			vibOverride = ModContent.ProjectileType<OmegaCannonShot>();
			ArrayPassive = false;
			Item.ResearchUnlockCount = 1;
		}
		public override void SetItemDefaults(Item Item)
		{
			Item.width = 10;
			Item.height = 14;
			Item.maxStack = 1;
			Item.value = 500000;
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
				.AddIngredient(ItemID.LunarBar, 15)
				.AddIngredient(ItemID.FragmentVortex, 30)
				.AddIngredient(ItemID.FragmentSolar, 30)
				.AddIngredient(ItemID.FragmentNebula, 30)
				.AddIngredient(ItemID.FragmentStardust, 30)
				.AddIngredient(ItemID.Diamond, 30)
				//.AddIngredient<Addons.Hunters.JudicatorAddon>(1)
				//.AddIngredient<MetroidMod.Content.BeamAddons.MPH.BattleHammerAddon>(1)
				//.AddIngredient<Addons.Hunters.VoltDriverAddon>(1)
				//.AddIngredient<Addons.Hunters.MagMaulAddon>(1)
				//.AddIngredient<Addons.Hunters.ImperialistAddon>(1)
				//.AddIngredient<Addons.Hunters.ShockCoilAddon>(1)
				.AddTile(TileID.LunarCraftingStation)
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
			OmegaCannonShot tasteTheRainbow = Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<OmegaCannonShot>(), damage, knockback).ModProjectile as OmegaCannonShot;
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
	public class OmegaCannonShot : MProjectile
	{
		public string fileMod = "";
		public override string Texture => $"{Mod.Name}/Assets/Textures/BeamAddons/OmegaCannon/Shot";
		public ModBeamAddon[] beamAddons = new ModBeamAddon[BeamAddonSlotID.Count - 1];
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Omega Cannon Shot");
			Main.projFrames[Projectile.type] = 2;

		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.scale = 1f;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 1;
		}
		//public override bool OnTileCollide(Vector2 oldVelocity)
		//{
		//	if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon)
		//	{
		//		Projectile.velocity.X = -oldVelocity.X;
		//	}

		//	if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon)
		//	{
		//		Projectile.velocity.Y = -oldVelocity.Y;
		//	}
		//	Projectile.timeLeft -= 120;

		//	return false;
		//}
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
			Projectile.rotation = 0;
			Color color = MetroidMod.powColor;
			Lighting.AddLight(Projectile.Center, color.R / 255f, color.G / 255f, color.B / 255f);

			if (Projectile.ai[1] == 0)
			{
				Projectile.scale = 1.5f;
			}
			if (Projectile.numUpdates == 0)
			{
				Projectile.rotation += 0.5f * Projectile.direction;
				Projectile.frame++;

				if (Projectile.timeLeft < 32 * (Projectile.extraUpdates + 1))
				{
					Projectile.velocity *= 0.95f;
				}
				if (Projectile.timeLeft % 7 == 0)
				{
					Dust.NewDust(Projectile.position + (Projectile.Size / 4), Projectile.width / 2, Projectile.height / 2, ModContent.DustType<OmegaCannonTrail>(), 0, 0, 255, Color.White, Projectile.scale);
				}
			}
			if (Projectile.frame > 1)
			{
				Projectile.frame = 0;
			}
			int dustType = 64;
			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 64, 0, 0, 100, default(Color), Projectile.scale);
			Main.dust[dust].noGravity = true;
			mProjectile.DustLine(Projectile.Center, Projectile.velocity, Projectile.rotation, 5, 1, dustType, 2f);
		}
		public override void OnKill(int timeLeft)
		{
			Projectile.penetrate = -1;
			mProjectile.Explode(2368);
			int shootNum = 15;
			float baseSpeed = 15f;
			int damage = Projectile.damage / 2;
			float knockBack = Projectile.knockBack / 2;
			int lifeTime = 90;
			float scale = Projectile.scale / 1.5f;
			if (Projectile.ai[1] != 0)
			{
				shootNum = 8;
				lifeTime = 70;
				baseSpeed = 12f;
			}

			float shootSpread = 360f;
			float spread = shootSpread * 0.0174f;
			double startAngle = Main.rand.NextFloat() * 3.14f;
			double deltaAngle = spread / shootNum;
			for (int i = 0; i < shootNum; i++)
			{
				double offsetAngle = startAngle + (deltaAngle * i);
				Vector2 vel = new Vector2(baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle));
				Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, vel, ModContent.ProjectileType<OmegaCannonFrag>(), damage, knockBack, Projectile.owner, lifeTime, scale);
			}
			Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<OmegaCannonTrail>(), Vector2.Zero, 255, Color.White, Projectile.scale + 1f);

		}
		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			if (Projectile.timeLeft >= 1)
				modifiers.ArmorPenetration += 50;
			base.ModifyHitNPC(target, ref modifiers);
		}
		public override bool? CanCutTiles()
		{
			if (Projectile.timeLeft <= 1)
			{
				return false;
			}
			return null;
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
	public class OmegaCannonFrag : ModProjectile
	{
		public override string Texture => $"{Mod.Name}/Assets/Textures/BeamAddons/OmegaCannon/Frag";
		public override void SetDefaults()
		{
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 600;
			Projectile.friendly = true;
			Projectile.ignoreWater = true;
			Projectile.DamageType = ModContent.GetInstance<HunterDamageClass>();

			Projectile.width = 80;
			Projectile.height = 80;
			Projectile.scale = 1f;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 60;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;

			Main.projFrames[Type] = 2;
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			hitbox = new Rectangle((int)(Projectile.Center.X - (40 * Projectile.scale)), (int)(Projectile.Center.Y - (40 * Projectile.scale)),
				(int)(80 * Projectile.scale), (int)(80 * Projectile.scale));

		}

		//public override void ModifyDamageHitbox(ref Rectangle hitbox)
		//{
		//	if (Projectile.timeLeft > 5)
		//	{
		//		hitbox = new Rectangle((int)Projectile.Center.X - 14, (int)Projectile.Center.Y - 14, 28, 28);
		//	}
		//	else
		//	{
		//		int amount = 20;
		//		hitbox = new Rectangle((int)Projectile.position.X - amount, (int)Projectile.position.Y - amount, Projectile.width + amount, Projectile.height + amount);
		//	}
		//}
		//public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		//{
		//	if (Projectile.timeLeft > 5)
		//	{
		//		Projectile.timeLeft = 5;
		//	}
		//}
		//public override void OnKill(int timeLeft)
		//{
		//	if (Projectile.ai[0] > 10)
		//	{
		//		int freq = 20;
		//		for (int i = 0; i < freq; i++)
		//		{
		//			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 64, 0, 0, 100, default(Color), 2);
		//			Main.dust[dust].velocity = new Vector2((Main.rand.Next(freq) - (freq / 2)) * 0.125f, (Main.rand.Next(freq) - (freq / 2)) * 0.125f);
		//			Main.dust[dust].noGravity = true;
		//		}
		//		SoundStyle sound = new($"{MetroidMod.Instance.Name}/Assets/Sounds/BeamImpactSound");
		//		SoundEngine.PlaySound(sound, Projectile.Center);
		//	}
		//}


		public override void AI()
		{
			Projectile P = Projectile;
			Projectile.scale = Projectile.ai[1];
			if (Projectile.timeLeft > 5)
			{
				Projectile.timeLeft = 100;
			}

			Projectile.ai[0]--;
			if ((int)Projectile.ai[0] % 5 == 0)
			{
				Projectile.frame = (Projectile.frame + 1) % 2;
			}
			if (Projectile.ai[0] < 64)
			{
				Projectile.velocity *= 0.925f;
				Projectile.alpha += 4;
			}
			if (Projectile.ai[0] < 0)
			{
				Projectile.Kill();
			}

			Color color = MetroidMod.powColor;
			Lighting.AddLight(Projectile.Center, color.R / 255f, color.G / 255f, color.B / 255f);

			Vector2 velocity = Projectile.position - Projectile.oldPos[0];
			if (Vector2.Distance(Projectile.position, Projectile.position + velocity) < Vector2.Distance(Projectile.position, Projectile.position + Projectile.velocity))
			{
				velocity = Projectile.velocity;
			}
			Projectile.rotation = (float)Math.Atan2(velocity.Y, velocity.X) + MathHelper.PiOver2;
		}
	}
}
