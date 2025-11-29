using System;
using MetroidMod.Common.GlobalItems;
using MetroidMod.Content.BeamAddons;
using MetroidMod.Content.Projectiles;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using MonoMod.Core.Utils;
using Terraria;
using Terraria.Audio;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.MissileAddons.BeamCombos
{
	public class NebulaCombo : ModMissileAddon
	{
		public override bool AddOnlyAddonItem => false;
		public override Color PrimaryColor => MetroidMod.waveColor;
		public override Color SecondaryColor => MetroidMod.waveColor2;
		public override int ShotDust => DustID.YellowTorch;
		const float Max_Range = 300f;
		float range = Max_Range;
		const float Max_Distance = 300f;
		float distance = Max_Distance;
		float accuracy = 11f;
		Vector2 oPos;
		Vector2 mousePos;
		SoundEffectInstance soundInstance;
		Projectile[] buster = new Projectile[4];
		public override void SetStaticDefaults()
		{
			AddonSlot = MissileAddonSlotID.Charge;

			//All the stats are set outside of here up in Stat Values, lets me do fancy schmancy tooltip stuff
			base.SetStaticDefaults();
		}
		public override void SetProjectileDefaults(MProjectile mProjectile)
		{
			Projectile Projectile = mProjectile.Projectile;
			Projectile.width = 100;
			Projectile.height = 100;
			Projectile.scale = 0f;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.extraUpdates = 5;
			Projectile.alpha = 255;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 8 * (1 + Projectile.extraUpdates);
			base.SetProjectileDefaults(mProjectile);
		}
		public override void HoldFireBehavior(Player player, Projectile lead)
		{
			Item item = player.HeldItem;
			Vector2 mPos = player.RotatedRelativePoint(player.MountedCenter, true);
			Lead = lead;
			if (!Initialized && Lead.active)
			{
				float MY = Main.mouseY + Main.screenPosition.Y;
				float MX = Main.mouseX + Main.screenPosition.X;
				if (player.gravDir == -1f)
				{
					MY = Main.screenPosition.Y + Main.screenHeight - Main.mouseY;
				}
				float targetrotation = (float)Math.Atan2(MY - oPos.Y, MX - mPos.X);
				Vector2 velocity = targetrotation.ToRotationVector2() * item.shootSpeed;
				Projectile.NewProjectile(player.GetSource_ItemUse(item), mPos.X, mPos.Y, velocity.X, velocity.Y, ModContent.ProjectileType<NebulaBusterShot>(), 0, 0, player.whoAmI);
				Initialized = true;
			}
		}
		public override void AI(MProjectile mProjectile)
		{
			Projectile P = mProjectile.Projectile;
			Player O = Main.player[P.owner];

			oPos = O.RotatedRelativePoint(O.MountedCenter, true);

			if (!Lead.active || Lead.owner != P.owner || Lead.type != ModContent.ProjectileType<ChargeLead>() || O.HeldItem.GetGlobalItem<MGlobalItem>().isBeam)
			{
				P.Kill();
				return;
			}

			if (!Initialized && P.owner == Main.myPlayer)
			{
				var entitySource = P.GetSource_FromAI();
				for (int i = 0; i < buster.Length; i++)
				{
					int b = Projectile.NewProjectile(entitySource, P.Center.X, P.Center.Y, 0f, 0f, ProjectileType, (int)(P.damage * 0.25f), P.knockBack, P.owner);
					buster[i] = Main.projectile[b];
					buster[i].ai[0] = P.whoAmI;
					buster[i].ai[1] = i;
					buster[i].netUpdate = true;
				}

				Initialized = true;
				mProjectile.Projectile.netUpdate = true;
			}

			range = Max_Range;
			distance = Max_Distance;

			if (P.owner == Main.myPlayer)
			{
				P.netUpdate = true;

				Vector2 diff = Main.MouseWorld - oPos;
				diff.Normalize();
				if (float.IsNaN(diff.X) || float.IsNaN(diff.Y))
				{
					diff = -Vector2.UnitY;
				}

				Vector2 targetPos = oPos + O.velocity + diff * Math.Min(Vector2.Distance(oPos, Main.MouseWorld), range);

				float speed = Math.Max(2f, Vector2.Distance(targetPos, P.Center) * 0.025f) * (0.5f + 0.5f * P.scale);
				float num244 = targetPos.X - P.Center.X;
				float num245 = targetPos.Y - P.Center.Y;
				float num246 = (float)Math.Sqrt((double)(num244 * num244 + num245 * num245));
				num246 = speed / num246;
				num244 *= num246;
				num245 *= num246;
				Vector2 vel = new Vector2((P.velocity.X * accuracy + num244) / (accuracy + 1f), (P.velocity.Y * accuracy + num245) / (accuracy + 1f));
				if (float.IsNaN(vel.X) || float.IsNaN(vel.Y))
				{
					vel = -Vector2.UnitY;
				}
				P.velocity = vel;

				//if (soundInstance == null || soundInstance.State != SoundState.Playing)
				//{
				//	SoundEngine.TryGetActiveSound(SoundEngine.PlaySound(Sounds.Items.Weapons.NebulaComboSoundLoop, O.position), out ActiveSound result);
				//	SoundEngine.PlaySound(new($"{Mod.Name}/Assets/Sounds/NebulaComboSoundStart"), O.position);
				//	soundInstance = result.Sound;
				//	if (Main.soundVolume > 0f)
				//	{
				//		soundInstance.Volume = 0f;
				//	}
				//}
				//else if (P.numUpdates == 0 && Main.soundVolume > 0f)
				//{
				//	soundInstance.Volume = Math.Min(soundInstance.Volume + 0.05f * Main.soundVolume, 1f * Main.soundVolume);
				//}
			}

			if (O.controlUseItem)
			{
				P.timeLeft = 10;
			}
			else
			{
				P.Kill();
			}

			if (P.numUpdates == 0)
			{
				P.rotation -= 0.104719758f * 2f;
				P.scale = Math.Min(P.scale + 0.05f, 1f);
				P.alpha = Math.Max(P.alpha - 15, 0);
			}

			P.position.X += P.width / 2f;
			P.position.Y += P.height / 2f;
			P.width = (int)(100f * P.scale);
			P.height = (int)(100f * P.scale);
			P.position.X -= P.width / 2f;
			P.position.Y -= P.height / 2f;

			if (P.numUpdates == 0)
			{
				float dist = Vector2.Distance(Lead.Center, P.Center);
				Vector2 diff2 = Vector2.Normalize(P.Center - Lead.Center);
				if (float.IsNaN(diff2.X) || float.IsNaN(diff2.Y))
				{
					diff2 = -Vector2.UnitY;
				}
				Vector2 diff3 = Vector2.Normalize(Lead.velocity);
				if (float.IsNaN(diff3.X) || float.IsNaN(diff3.Y))
				{
					diff3 = -Vector2.UnitY;
				}

				for (float i = 0f; i < dist; i += 30f)
				{
					Vector2 pos1 = Lead.Center + diff3 * i;
					Vector2 pos2 = Lead.Center + diff2 * i;

					float scale = MathHelper.Lerp(0.1f, P.scale, i / dist);

					int dWidth = (int)(100f * scale);
					int dHeight = (int)(100f * scale);

					Vector2 dustPos = Vector2.Lerp(pos1, pos2, i / dist) - new Vector2(dWidth, dHeight) / 2f;
					int num891 = Dust.NewDust(dustPos, dWidth, dHeight, 255, 0f, 0f, 100, default(Color), 2f);
					Main.dust[num891].noGravity = true;
				}

				P.frame++;
				if (P.frame > 1)
				{
					P.frame = 0;
				}
			}
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.FragmentNebula, 15)
				.AddIngredient(ItemID.LunarBar, 5)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}
	}
	public class NebulaBusterShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Nebula Singularity Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.scale = 1f;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.extraUpdates = 5;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 2 * (1 + Projectile.extraUpdates);
		}

		Vector2 targetPos;
		bool initialize = false;

		Projectile Lead;

		NPC target;

		//const float Max_Range = 300f;
		float range = Max_Range;
		//const float Max_Distance = 300f;
		float distance = Max_Distance;
		const float Max_Range = 300f;

		const float Max_Distance = 60f;

		float accuracy = 11f;
		Vector2 oPos;
		Vector2 mousePos;

		bool soundPlayed = false;

		public override void AI()
		{

			Projectile P = Projectile;
			Player O = Main.player[P.owner];

			oPos = O.RotatedRelativePoint(O.MountedCenter, true);

			if (!Lead.active || Lead.owner != P.owner || O.HeldItem.GetGlobalItem<MGlobalItem>().isBeam)
			{
				P.Kill();
				return;
			}

			if (!initialize)
			{
				targetPos = P.Center;

				initialize = true;
			}

			range = Max_Range;
			distance = Max_Distance;

			if (P.owner == Main.myPlayer)
			{
				P.netUpdate = true;

				float rot = (float)Math.PI / 2f * P.ai[1] + Lead.rotation / 2f;
				Vector2 rotPoint = Lead.Center + rot.ToRotationVector2() * distance * Lead.scale;

				target = null;
				foreach (NPC who in Main.ActiveNPCs)
				{
					NPC npc = Main.npc[who.whoAmI];
					if (npc.lifeMax > 5 && npc.dontTakeDamage && !npc.friendly)
					{

						if (npc.CanBeChasedBy(P, false) && Vector2.Distance(npc.Center, rotPoint) < range)
						{
							if (target == null || !target.active)
							{
								target = npc;
							}
							else
							{
								if (npc != target && Vector2.Distance(npc.Center, rotPoint) < Vector2.Distance(target.Center, rotPoint))
								{
									target = npc;
								}

								if (Vector2.Distance(npc.Center, rotPoint) > range)
								{
									target = null;
								}
							}
						}
					}
				}

				if (target != null && target.active)
				{
					targetPos = target.Center;
					//if (!soundPlayed)
					//{
					//	Terraria.Audio.SoundEngine.PlaySound(Terraria.ID.SoundID.Item43, P.Center);
					//	soundPlayed = true;
					//}
				}
				else
				{
					//soundPlayed = false;
					if (P.numUpdates == 0)
					{
						targetPos = rotPoint;
						int r = 50;
						targetPos.X += Main.rand.Next(-r, r + 1);
						targetPos.Y += Main.rand.Next(-r, r + 1);
					}
				}

				float speed = Math.Max(8f, Vector2.Distance(targetPos, P.Center) * 0.025f);
				float targetAngle = (float)Math.Atan2(targetPos.Y - P.Center.Y, targetPos.X - P.Center.X);
				P.velocity = targetAngle.ToRotationVector2() * speed;
			}

			if (O.controlUseItem)
			{
				P.timeLeft = 10;
			}
			else
			{
				P.Kill();
			}
		}

		public override void OnKill(int timeLeft)
		{

		}

		public override void CutTiles()
		{
			if (Lead != null && Lead.active)
			{
				DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
				Utils.PlotTileLine(Lead.Center, Projectile.Center, (Projectile.width + 16) * Projectile.scale, DelegateMethods.CutTiles);
			}
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (Lead != null && Lead.active)
			{
				float point = 0f;
				return projHitbox.Intersects(targetHitbox) ||
					Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Lead.Center, Projectile.Center, Projectile.width, ref point);
			}
			return false;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			return false;
		}
	}
}
