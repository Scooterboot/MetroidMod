using System;
using System.Collections.Generic;
using System.IO;
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
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.BeamAddons
{
	public class Imperialist : ModBeamAddon
	{
		public override bool AddOnlyAddonItem => false;
		public override int ShotDust => 269;
		public override Color PrimaryColor => MetroidMod.powColor;
		//public override string ImpactSound => $"{Mod.Name}/Assets/Sounds/BeamAddons/VoltDriver/Impact";
		public override void SetStaticDefaults()
		{
			AddonSlot = BeamAddonSlotID.Primary;
			VIB = true;
			vibOverride = ModContent.ProjectileType<ImperialistShot>();
			ArrayPassive = false;
			ItemID.Sets.ShimmerTransformToItem[Type] = BeamAddonLoader.GetAddon<Judicator>().ItemType;
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
			ImperialistShot tasteTheRainbow = Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<ImperialistShot>(), damage, knockback).ModProjectile as ImperialistShot;
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
			/*CreateRecipe(1)
				.AddIngredient<Miscellaneous.ChoziteBar>(30)
				.AddIngredient<Miscellaneous.EnergyShard>(30)
				.AddIngredient(ItemID.ZapinatorGray, 1)
				.AddIngredient(ItemID.SoulofFright, 20)
				.AddIngredient(ItemID.Ruby, 20)
				.AddTile(TileID.Hellforge)
				.Register();

			CreateRecipe(1)
				.AddIngredient<Miscellaneous.ChoziteBar>(30)
				.AddIngredient<Miscellaneous.EnergyShard>(30)
				.AddIngredient(ItemID.ZapinatorOrange, 1)
				.AddIngredient(ItemID.SoulofFright, 20)
				.AddIngredient(ItemID.Ruby, 20)
				.AddTile(TileID.Hellforge)
				.Register();*/

			CreateRecipe(1)
				.AddIngredient<Items.Miscellaneous.ChoziteBar>(15)
				.AddIngredient<Items.Miscellaneous.EnergyShard>(2)
				.AddIngredient(ItemID.Lens, 5)
				.AddIngredient(ItemID.Ruby, 1)
				.AddIngredient(ItemID.IllegalGunParts, 1)
				.AddTile(TileID.Anvils)
				.Register();

		}
	}
	public class ImperialistShot : MProjectile
	{
		//what a total mess lmao --Dr
		public string fileMod = "";
		public override string Texture => $"{Mod.Name}/Assets/Textures/BeamAddons/Imperialist/Shot";
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
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 1;
		}
		private bool spaze = false;
		private int depth = 0;
		private float hitRange = 0;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.scale = 1f;
			mProjectile.amplitude = 7 * Projectile.scale;
			mProjectile.wavesPerSecond = 10f;
			mProjectile.delay = 0;
			//Projectile.tileCollide = false;
			Projectile.stopsDealingDamageAfterPenetrateHits = true;

		}
		private const float Max_Range = 2200f;
		private float beamLength = 0f;
		private float scaleUp = 0f;
		private int timer = 0;
		private readonly int[] nearestTargets = [.. Enumerable.Repeat(-1, 12)];
		public override void AI()
		{
			Projectile P = Projectile;
			P.velocity = Vector2.Normalize(P.velocity);
			P.rotation = P.velocity.ToRotation() - MathHelper.PiOver2;
			P.timeLeft = 100;
			P.usesLocalNPCImmunity = true;
			P.localNPCHitCooldown = -1;
			//if (spaze)
			//{
			//	mProjectile.WaveBehavior(P, true);
			//}
			if (shot.Contains("wave") || shot.Contains("nebula"))
			{
				depth = waveDepth;
			}

			for (P.ai[1] = 0f; P.ai[1] <= beamLength; P.ai[1] += 4f)
			{
				Vector2 end = P.Center + (P.velocity * P.ai[1]);
				Vector2 trueEnd = end + (P.velocity * depth * P.ai[1] * 8f);
				if (CollideMethods.CheckCollide(trueEnd, 0, 0) && hitRange == 0)
				{
					Projectile.ai[1] -= 4f;
					beamLength = Vector2.Distance(trueEnd, P.Center);
					break;
				}
				if (hitRange > 0)
				{
					Projectile.ai[1] -= 4f;
					beamLength = hitRange;
					break;
				}
				else
				{
					beamLength = Max_Range;
				}
			}

			if (P.numUpdates == 0)
			{
				if (timer == 0)
				{
					List<float> listDists = new List<float>();
					List<int> listNPCIndices = new List<int>();
					for (int i = 0; i < Main.npc.Length; i++)
					{
						NPC n = Main.npc[i];
						if (!n.friendly && !n.dontTakeDamage && (bool)Colliding(Projectile.Hitbox, n.Hitbox))
						{
							listDists.Add(Projectile.Distance(n.Center));
							listNPCIndices.Add(i);
						}
					}
					float[] arrayDists = [.. listDists];
					int[] arrayNPCIndices = [.. listNPCIndices];
					Array.Sort(arrayDists, arrayNPCIndices);

					for (int i = 0; i < nearestTargets.Length; i++)
					{
						if (i < arrayNPCIndices.Length)
						{
							//Main.NewText(arrayDists[i], Color.Pink);
							//Main.NewText(arrayNPCIndices[i], Color.Purple);
							nearestTargets[i] = arrayNPCIndices[i];
						}
						//Main.NewText(nearestTargets[i], Color.Green);
					}
				}

				timer++;
				float maxTime = 80f;
				float fade = 20f;
				scaleUp = (float)Math.Log10(timer) * (maxTime - timer) / (maxTime - fade);
				if (timer >= maxTime)
				{
					P.Kill();
				}
			}
			//P.damage *= (int)(1d + (mp.impStealth / 125d));

			if (timer >= 6)
			{
				Projectile.damage = 0;
			}
		}
		public override bool ShouldUpdatePosition()
		{
			if (spaze)
			{
				return true;
			}
			return false;
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(spaze);
			writer.Write(beamLength);
			//writer.Write(waveDepth);
			//writer.Write(hitRange);
			writer.Write(Projectile.penetrate);
			writer.Write(Projectile.maxPenetrate);
			base.SendExtraAI(writer);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			spaze = reader.ReadBoolean();
			beamLength = reader.ReadInt32();
			//waveDepth = reader.ReadInt32();
			//hitRange = reader.ReadInt32();
			Projectile.penetrate = reader.ReadInt32();
			Projectile.maxPenetrate = reader.ReadInt32();
			base.ReceiveExtraAI(reader);
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			Projectile P = Projectile;
			float visualBeamLength = beamLength - 14.5f;
			Vector2 centerFloored = P.Center.Floor() + (P.velocity * 16f);
			Vector2 endPosition = centerFloored + (P.velocity * visualBeamLength);
			float _ = float.NaN;
			if (projHitbox.Intersects(targetHitbox))
			{
				return true;
			}


			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), P.Center, endPosition, P.width, ref _);
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (target.friendly)
			{
				return false;
			}
			for (int i = 0; i < Projectile.maxPenetrate; i++)
			{
				if (nearestTargets[i] == target.whoAmI)
				{
					if (i == Projectile.maxPenetrate - 1)
					{
						hitRange = Vector2.Distance(target.Center, Projectile.Center);
					}
					return true;
				}
			}
			return false;
		}
		//public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		//{
		//	if(Projectile.penetrate == 1)
		//	{
		//		//hitRange = Vector2.Distance(target.Center, Projectile.Center);
		//	}
		//	base.OnHitNPC(target, hit, damageDone);
		//}
		public override bool PreDraw(ref Color lightColor)
		{
			Projectile P = Projectile;
			if (P.velocity == Vector2.Zero)
			{
				return false;
			}
			Texture2D texture = TextureAssets.Projectile[P.type].Value;
			float visualBeamLength = beamLength - 14.5f;
			Vector2 centerFloored = P.Center.Floor() + (P.velocity * 16f);
			Vector2 drawScale = new(scaleUp, 1f);
			DelegateMethods.f_1 = 1f;
			Vector2 startPosition = centerFloored - Main.screenPosition;
			Vector2 endPosition = startPosition + (P.velocity * visualBeamLength);


			drawScale *= 0.25f;
			DrawBeam(Main.spriteBatch, texture, startPosition, endPosition, drawScale, P.GetAlpha(new Color(240, 120, 100)));
			return false;
		}
		private void DrawBeam(SpriteBatch spriteBatch, Texture2D texture, Vector2 startPosition, Vector2 endPosition, Vector2 drawScale, Color beamColor)
		{
			Utils.LaserLineFraming lineFraming = new(DelegateMethods.RainbowLaserDraw);

			// c_1 is an unnamed decompiled variable which is the render color of the beam drawn by DelegateMethods.RainbowLaserDraw.
			DelegateMethods.c_1 = beamColor;

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);


			MiscShaderData shaderData = GameShaders.Misc["MetroidModLaserBeam"];
			shaderData.UseColor(Color.White);
			shaderData.UseSecondaryColor(beamColor);
			shaderData.UseImage0(TextureAssets.Projectile[Projectile.type]);
			shaderData.UseOpacity(4f);

			//Utils.DrawLaser(spriteBatch, texture, startPosition, endPosition, drawScale, lineFraming);

			//[Joost] Repeating the contents of Utils.DrawLaser to apply the shaderData to its drawData
			Vector2 vector = Utils.SafeNormalize(endPosition - startPosition, Vector2.UnitX);
			float num = (endPosition - startPosition).Length();
			float num2 = vector.ToRotation() - 1.57079637f;
			float num3;
			Rectangle rectangle;
			Vector2 vector2;
			Color color;
			lineFraming(0, startPosition, num, default(Rectangle), out num3, out rectangle, out vector2, out color);
			DrawData data = new DrawData(texture, startPosition, new Rectangle?(rectangle), color, num2, rectangle.Size() / 2f, drawScale, 0, 0f);
			shaderData.Apply(data);
			data.Draw(spriteBatch);

			num -= num3 * drawScale.Y;
			Vector2 vector3 = startPosition + (vector * (rectangle.Height - vector2.Y) * drawScale.Y);
			if (num > 0f)
			{
				float num4 = 0f;
				while (num4 + 1f < num)
				{
					lineFraming(1, vector3, num - num4, rectangle, out num3, out rectangle, out vector2, out color);
					if (num - num4 < rectangle.Height)
					{
						num3 *= (num - num4) / rectangle.Height;
						rectangle.Height = (int)(num - num4) + 1; //[Joost] Added a +1 to fix some minor empty space artifacting
					}
					DrawData data2 = new DrawData(texture, vector3, new Rectangle?(rectangle), color, num2, vector2, drawScale, 0, 0f);
					shaderData.Apply(data2);
					data2.Draw(spriteBatch);

					num4 += num3 * drawScale.Y;
					vector3 += vector * num3 * drawScale.Y;
				}
			}
			lineFraming(2, vector3, num, default(Rectangle), out num3, out rectangle, out vector2, out color);
			DrawData data3 = new DrawData(texture, vector3, new Rectangle?(rectangle), color, num2, vector2, drawScale, 0, 0f);
			shaderData.Apply(data3);
			data3.Draw(spriteBatch);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin();
		}
		public override void CutTiles()
		{
			// tilecut_0 is an unnamed decompiled variable which tells CutTiles how the tiles are being cut (in this case, via a Projectile).
			DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
			Utils.TileActionAttempt cut = new(DelegateMethods.CutTiles);
			float visualBeamLength = beamLength - 14.5f;
			Vector2 centerFloored = Projectile.Center.Floor() + (Projectile.velocity * 16f);
			//Vector2 beamStartPos = centerFloored - Main.screenPosition;
			Vector2 beamEndPos = centerFloored + (Projectile.velocity * visualBeamLength);

			// PlotTileLine is a function which performs the specified action to all tiles along a drawn line, with a specified width.
			// In this case, it is cutting all tiles which can be destroyed by Projectiles, for example grass or pots.
			Utils.PlotTileLine(Projectile.Center, beamEndPos, Projectile.width * Projectile.scale, cut);
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
}
