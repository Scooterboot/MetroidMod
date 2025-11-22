using System;
using MetroidMod.Common.GlobalItems;
using MetroidMod.Common.Players;
using MetroidMod.Content.BeamAddons;
using MetroidMod.Content.Items.Weapons;
using MetroidMod.Content.Projectiles;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Mono.Cecil;
using MonoMod.Core.Utils;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static MetroidMod.Sounds;

namespace MetroidMod.Content.MissileAddons
{
	internal class SeekerMissile : ModMissileAddon
	{
		public override bool AddOnlyAddonItem => false;

		public override Color PrimaryColor => MetroidMod.iceColor;

		public override Color SecondaryColor => MetroidMod.iceSecondaryColor;
		public override int ShotDust => DustID.IceTorch;

		public override bool IgnoreProjectile => true;
		public override bool NeedsCharging => false;
		public override bool HoldFire => true;
		private int targetNum = 0;
		private int targetingDelay = 0;
		public override void SetStaticDefaults()
		{
			AddonSlot = MissileAddonSlotID.Charge;

			//All the stats are set outside of here up in Stat Values, lets me do fancy schmancy tooltip stuff
			base.SetStaticDefaults();
		}
		public override void HoldFireBehavior(Player player, Projectile lead)
		{
			Item item = player.HeldItem;
			Lead = lead;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			MGlobalItem pb = item.GetGlobalItem<MGlobalItem>();
			float MY = Main.mouseY + Main.screenPosition.Y;
			float MX = Main.mouseX + Main.screenPosition.X;
			Rectangle mouse = new Rectangle((int)MX - 1, (int)MY - 1, 2, 2);
			Vector2 oPos = player.RotatedRelativePoint(player.MountedCenter, true);
			float targetrotation = (float)Math.Atan2(MY - oPos.Y, MX - oPos.X);
			Vector2 velocity = targetrotation.ToRotationVector2() * item.shootSpeed;
			var entitySource = player.GetSource_ItemUse(item);
			if (item.ModItem is ArmCannon armi && Lead.active && !Initialized)
			{
				//armi.Launch(player, entitySource, oPos, velocity, item.shoot, item.damage, item.knockBack, true);
				//int shotProj = Projectile.NewProjectile(entitySource, oPos.X, oPos.Y, velocity.X, velocity.Y, item.shoot, item.damage, item.knockBack, player.whoAmI);
				//MProjectile mProj = (MProjectile)Main.projectile[shotProj].ModProjectile;
				//MissileAddonLoader.GetAddon(armi.MissileAddonAccess[MissileAddonSlotID.Charge]).PreAI(mProj);
				Projectile.NewProjectileDirect(entitySource, lead.position, velocity, ModContent.ProjectileType<SeekerMissileLead>(), item.damage, item.knockBack, player.whoAmI);
				Initialized = true;
			}
		}
		public override void AI(MProjectile mProjectile)
		{
			Player player = Main.player[mProjectile.Projectile.owner];
			MPlayer mp = player.GetModPlayer<MPlayer>();
			Item item = player.HeldItem;
			MGlobalItem pb = item.GetGlobalItem<MGlobalItem>();
			//pb.seekerCharge = 0;
			//mProj.seeking = true;
			if (mProjectile.seeking)
			{
				Seeking(mProjectile, mProjectile.seekTarget);
			}
			//Projectile P = mProjectile.Projectile;
			//float num236 = P.position.X;
			//float num237 = P.position.Y;
			//bool flag5 = false;
			//P.ai[0] += 1f;
			//if (P.ai[0] > 5f && P.numUpdates <= 0)
			//{
			//	P.ai[0] = 5f;
			//	int num239 = mProjectile.seekTarget;
			//	if (Main.npc[num239].active)
			//	{
			//		num236 = Main.npc[num239].position.X + (Main.npc[num239].width / 2);
			//		num237 = Main.npc[num239].position.Y + (Main.npc[num239].height / 2);
			//		flag5 = true;
			//	}
			//	//else
			//	//{
			//	//	seekTarget = -1;
			//	//}
			//}
			//if (!flag5)
			//{
			//	num236 = P.position.X + (P.width / 2) + (P.velocity.X * 100f);
			//	num237 = P.position.Y + (P.height / 2) + (P.velocity.Y * 100f);
			//}
			//float num243 = 8f;
			//Vector2 vector22 = new Vector2(P.position.X + (P.width * 0.5f), P.position.Y + (P.height * 0.5f));
			//float num244 = num236 - vector22.X;
			//float num245 = num237 - vector22.Y;
			//float num246 = (float)Math.Sqrt((double)((num244 * num244) + (num245 * num245)));
			//num246 = num243 / num246;
			//num244 *= num246;
			//num245 *= num246;
			//P.velocity.X = ((P.velocity.X * 11f) + num244) / 12f;
			//P.velocity.Y = ((P.velocity.Y * 11f) + num245) / 12f;
		}
		private void Seeking(MProjectile mProjectile, int seekTarget)
		{
			Projectile P = mProjectile.Projectile;
			float num236 = P.position.X;
			float num237 = P.position.Y;
			bool flag5 = false;
			P.ai[0] += 1f;
			if (P.ai[0] > 5f && P.numUpdates <= 0)
			{
				P.ai[0] = 5f;
				int num239 = seekTarget;
				if (Main.npc[num239].active)
				{
					num236 = Main.npc[num239].position.X + (Main.npc[num239].width / 2);
					num237 = Main.npc[num239].position.Y + (Main.npc[num239].height / 2);
					flag5 = true;
				}
				//else
				//{
				//	seekTarget = -1;
				//}
			}
			if (!flag5)
			{
				num236 = P.position.X + (P.width / 2) + (P.velocity.X * 100f);
				num237 = P.position.Y + (P.height / 2) + (P.velocity.Y * 100f);
			}
			float num243 = 8f;
			Vector2 vector22 = new Vector2(P.position.X + (P.width * 0.5f), P.position.Y + (P.height * 0.5f));
			float num244 = num236 - vector22.X;
			float num245 = num237 - vector22.Y;
			float num246 = (float)Math.Sqrt((double)((num244 * num244) + (num245 * num245)));
			num246 = num243 / num246;
			num244 *= num246;
			num245 *= num246;
			P.velocity.X = ((P.velocity.X * 11f) + num244) / 12f;
			P.velocity.Y = ((P.velocity.Y * 11f) + num245) / 12f;
		}
		public override void SetItemDefaults(Item item)
		{
			item.value = 30000;
			item.rare = ItemRarityID.LightRed;
			base.SetItemDefaults(item);
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddRecipeGroup(MetroidMod.T1HMBarRecipeGroupID, 10)
				.AddIngredient(ItemID.SoulofNight, 1)
				.AddIngredient(ItemID.SoulofLight, 1)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
	public class SeekerMissileLead : MProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 8800;
			Projectile.ownerHitCheck = true;
			Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.tileCollide = false;
			Projectile.penetrate = 1;
			Projectile.ignoreWater = true;
			//Projectile.ranged = true;
		}

		private Color LightColor = Color.Cyan;//MetroidMod.powColor;
		private bool soundPlayed = false;
		private readonly SoundEffectInstance soundInstance;
		private int dustDelay = 0;
		private int negateUseTime = 0;
		private int targetNum = 0;
		private int targetingDelay = 0;
		public override void AI()
		{
			Projectile P = Projectile;
			Player O = Main.player[P.owner];

			Item I = O.HeldItem;

			MPlayer mp = O.GetModPlayer<MPlayer>();
			MGlobalItem mi = I.GetGlobalItem<MGlobalItem>();

			float MY = Main.mouseY + Main.screenPosition.Y;
			float MX = Main.mouseX + Main.screenPosition.X;
			Rectangle mouse = new Rectangle((int)MX - 1, (int)MY - 1, 2, 2);
			Vector2 oPos = O.RotatedRelativePoint(O.MountedCenter, true);
			float targetrotation = (float)Math.Atan2(MY - oPos.Y, MX - oPos.X);
			Vector2 velocity = targetrotation.ToRotationVector2() * I.shootSpeed;
			var entitySource = O.GetSource_ItemUse(I);
			if (O.gravDir == -1f)
			{
				MY = Main.screenPosition.Y + Main.screenHeight - Main.mouseY;
			}

			P.scale = mi.seekerCharge / (float)MGlobalItem.seekerMaxCharge * (0.25f + (0.75f * ((mi.numSeekerTargets + 1) / 6f)));
			P.rotation += 0.5f * P.direction;
			O.itemTime = 2;
			O.itemAnimation = 2;

			int range = I.width + 4;
			int width = (I.width / 2) - (P.width / 2);
			int height = (I.height / 2) - (P.height / 2);

			if (negateUseTime < I.useTime)
			{
				negateUseTime++;
			}

			Vector2 iPos = O.itemLocation;

			P.friendly = false;
			P.damage = 0;
			P.position = new Vector2(iPos.X + ((float)Math.Cos(targetrotation) * range) + width, iPos.Y + ((float)Math.Sin(targetrotation) * range) + height);
			P.alpha = 0;
			if (P.velocity.X < 0)
			{
				P.direction = -1;
			}
			else
			{
				P.direction = 1;
			}
			P.spriteDirection = P.direction;
			O.direction = P.direction;

			O.heldProj = P.whoAmI;
			O.itemRotation = (float)Math.Atan2((MY - oPos.Y) * O.direction, (MX - oPos.X) * O.direction) - O.fullRotation;

			P.position -= P.velocity;
			P.timeLeft = 60;
			if (O.whoAmI == Main.myPlayer)
			{
				//if (mi.seekerCharge == 10 && SoundEngine.TryGetActiveSound(SoundEngine.PlaySound(Sounds.Items.Weapons.ChargeStartup_Seeker, oPos), out ActiveSound result))
				//{
				//	soundInstance = result.Sound;
				//}
			}
			if (mi.seekerCharge >= MGlobalItem.seekerMaxCharge)
			{
				if (dustDelay <= mi.numSeekerTargets)
				{
					int dust = Dust.NewDust(P.position + P.velocity, P.width, P.height, 63, 0, 0, 100, Color.Cyan, 2.0f);
					Main.dust[dust].noGravity = true;
					dustDelay = 5;
				}
			}
			dustDelay = Math.Max(dustDelay - 1, 0);
			Lighting.AddLight(P.Center, LightColor.R / 255f * P.scale, LightColor.G / 255f * P.scale, LightColor.B / 255f * P.scale);
			if (O.controlUseItem && !mp.ballstate && !mp.shineActive && !O.dead && !O.noItems)
			{
				if (P.owner == Main.myPlayer)
				{
					P.velocity = targetrotation.ToRotationVector2() * O.HeldItem.shootSpeed;
				}
				if (O.controlUseItem)
				{

					if (mi.seekerCharge < MGlobalItem.seekerMaxCharge)
					{
						mi.seekerCharge = Math.Min(mi.seekerCharge + 1, MGlobalItem.seekerMaxCharge);
					}
					else
					{
						foreach (NPC who in Main.ActiveNPCs)
						{
							NPC npc = Main.npc[who.whoAmI];
							if (npc.active && npc.chaseable && !npc.dontTakeDamage && !npc.friendly && npc.lifeMax > 5)// && !npc.immortal)
							{
								Rectangle npcRect = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);
								bool flag = false;
								for (int j = 0; j < mi.seekerTarget.Length; j++)
								{
									if (mi.seekerTarget[j] == npc.whoAmI)
									{
										flag = true;
									}
								}

								Vector2 delta = new Vector2(MX, MY);
								delta.X -= MathHelper.Clamp(MX, npcRect.X, npcRect.X + npcRect.Width);
								delta.Y -= MathHelper.Clamp(MY, npcRect.Y, npcRect.Y + npcRect.Height);
								bool colFlag = delta.Length() < 50;
								if (colFlag && mi.seekerTarget[targetNum] <= -1 && ((targetingDelay <= 0 && mouse.Intersects(npcRect)) || !flag) /*&& pb.statMissiles > pb.numSeekerTargets*/)
								{
									mi.seekerTarget[targetNum] = npc.whoAmI;
									targetNum++;
									if (targetNum > 4)
									{
										targetNum = 0;
									}
									targetingDelay = 40;
									//SoundEngine.PlaySound(Sounds.Items.Weapons.SeekerLockSound, oPos);
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
			
			}
			else
			{
				if (mi.seekerCharge >= MGlobalItem.seekerMaxCharge)
				{
					O.itemTime = I.useTime;
					O.itemAnimation = I.useAnimation;
				}
				else
				{
					O.itemTime = I.useTime - negateUseTime;
					O.itemAnimation = I.useAnimation - negateUseTime;
				}
				if (O.whoAmI == Main.myPlayer)
				{
					if (soundInstance != null)
					{
						soundInstance.Stop(true);
					}
					soundPlayed = false;
				}
				//mi.seekerCharge = 0;
				//mi.numSeekerTargets = 0;
				//for (int k = 0; k < mi.seekerTarget.Length; k++)
				//{
				//	mi.seekerTarget[k] = -1;
				//}
				targetNum = 0;
				targetingDelay = 0;
				P.Kill();
			}
		}
		public override void OnKill(int timeLeft)
		{
			Projectile P = Projectile;
			Player O = Main.player[P.owner];
			Item I = O.HeldItem;
			float MY = Main.mouseY + Main.screenPosition.Y;
			float MX = Main.mouseX + Main.screenPosition.X;
			Vector2 oPos = O.RotatedRelativePoint(O.MountedCenter, true);
			float targetrotation = (float)Math.Atan2(MY - oPos.Y, MX - oPos.X);
			Vector2 velocity = targetrotation.ToRotationVector2() * I.shootSpeed;
			var entitySource = O.GetSource_ItemUse(I);
			MGlobalItem mi = I.GetGlobalItem<MGlobalItem>();
			if (I.ModItem is ArmCannon armi)
			{
				if (mi.seekerCharge <= 0 && P.active)
				{
					mi.seekerCharge++;
				}
				if (mi.seekerCharge >= MGlobalItem.seekerMaxCharge && mi.numSeekerTargets > 0)
				{
					for (int i = 0; i < mi.seekerTarget.Length; i++)
					{
						if (mi.seekerTarget[i] > -1)
						{
							int shotProj = Projectile.NewProjectile(entitySource, oPos.X, oPos.Y, velocity.X, velocity.Y, I.shoot, I.damage, I.knockBack, O.whoAmI);
							MProjectile mProj = (MProjectile)Main.projectile[shotProj].ModProjectile;
							//armi.Launch(player, entitySource, oPos, velocity, item.shoot, item.damage, item.knockBack, true);
							armi.Launch(O, entitySource, oPos, velocity, I.shoot, I.damage, I.knockBack, true);
							if(mProj is MissileShot miss)
							{
								miss.groupSize = i;
							}
							mProj.seekTarget = mi.seekerTarget[i];
							mProj.seeking = true;
							//Seeking(mProj, mProj.seekTarget);
							mProj.Projectile.netUpdate2 = true;
							//MissileAddonLoader.GetAddon<SuperMissile>().AI(mProj);
							//pb.statMissiles = Math.Max(pb.statMissiles -= (int)Math.Round(MGlobalItem.AmmoUsage(player, 1)), 0);
						}
					}

					SoundEngine.PlaySound(Sounds.Items.Weapons.SeekerMissileSound, oPos);
				}
				//else if (pb.seekerCharge > 0)
				//{
				//	Projectile.NewProjectile(entitySource, oPos.X, oPos.Y, velocity.X, velocity.Y, item.shoot, item.damage, item.knockBack, player.whoAmI);
				//	//SoundEngine.PlaySound(new($"{Mod.Name}/Assets/Sounds/{shotSound}"), oPos);
				//	//pb.statMissiles -= 1;
				//}
				//if (!P.active)
				//{
				//	mi.seekerCharge = 0;
				//	//Initialized = false;
				//}
				//mi.numSeekerTargets = 0;
				for (int k = 0; k < mi.seekerTarget.Length; k++)
				{
					mi.seekerTarget[k] = -1;
				}
				//targetNum = 0;
				//targetingDelay = 0;
				//Initialized = false;
				//armi.Launch(O, entitySource, oPos, velocity, I.shoot, I.damage, I.knockBack, true);		
			}
			mi.seekerCharge = 0;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.DrawCentered(Projectile, Main.spriteBatch);
			return false;
		}
	}
}
