using System;
using System.IO;
using MetroidMod.Common.GlobalItems;
using MetroidMod.Content.BeamAddons;
using MetroidMod.Content.Projectiles;
using MetroidMod.Default;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using static MetroidMod.Sounds;

namespace MetroidMod.Content.MissileAddons
{
	public class NovaLaser : ModMissileAddon
	{
		public override bool AddOnlyAddonItem => false;

		public override Color PrimaryColor => MetroidMod.novColor;
		public override int ShotFrames => 2;
		public override Color SecondaryColor => MetroidMod.novSecondaryColor;
		public override int ShotDust => DustID.GreenTorch;
		public override bool HoldFire => true;

		public override void SetStaticDefaults()
		{
			AddonSlot = MissileAddonSlotID.Charge;

			//All the stats are set outside of here up in Stat Values, lets me do fancy schmancy tooltip stuff
			base.SetStaticDefaults();
		}
		public override void SetProjectileDefaults(MProjectile mProjectile)
		{
			base.SetProjectileDefaults(mProjectile);
			mProjectile.Projectile.width = 26;
			mProjectile.Projectile.height = 26;
			mProjectile.Projectile.scale = 1f;
			mProjectile.Projectile.tileCollide = false;
			mProjectile.Projectile.penetrate = -1;
			mProjectile.Projectile.extraUpdates = 5;
			mProjectile.Projectile.usesLocalNPCImmunity = true;
			mProjectile.Projectile.localNPCHitCooldown = 8;
		}
		private float BeamLength
		{
			get => mProjectile.Projectile.localAI[1];
			set => mProjectile.Projectile.localAI[1] = value;
		}
		private const float Max_Range = 2200f;
		private float maxRange = 0f;

		private float scaleUp = 0f;

		private Projectile Lead;

		private SoundEffectInstance soundInstance;

		private bool initialize = false;
		public override void HoldFireBehavior(Player player, ChargeLead lead)
		{
			Item item = player.HeldItem;
			Vector2 oPos = player.RotatedRelativePoint(player.MountedCenter, true);

			Lead = lead.Projectile;
			if (!initialize)
			{
				float MY = Main.mouseY + Main.screenPosition.Y;
				float MX = Main.mouseX + Main.screenPosition.X;
				if (player.gravDir == -1f)
				{
					MY = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
				}
				float targetrotation = (float)Math.Atan2(MY - oPos.Y, MX - oPos.X);
				Vector2 velocity = targetrotation.ToRotationVector2() * Item.shootSpeed;
				Projectile.NewProjectile(player.GetSource_ItemUse(item), oPos.X, oPos.Y, velocity.X, velocity.Y, ProjectileType, 0, 0, player.whoAmI);
				//Main.projectile[oof].ai[0] = guideProj;
				//Lead = Main.projectile[(int)mProjectile.Projectile.ai[0]];
				initialize = true;
			}

		}
		public override void AI(MProjectile mpshot)
		{
			Projectile P = mpshot.Projectile;
			Player O = Main.player[P.owner];
			Vector2 oPos = O.RotatedRelativePoint(O.MountedCenter, true);
			//if (O.HeldItem.GetGlobalItem<MGlobalItem>().statMissiles <= 0)
			//{
			//	P.Kill();
			//}
			if (!Lead.active || Lead.owner != P.owner || Lead.type != ModContent.ProjectileType<ChargeLead>() || !O.controlUseItem || O.HeldItem.GetGlobalItem<MGlobalItem>().isBeam || O.dead)
			{
				initialize = false;
				P.Kill();
				return;
			}
			else
			{
				if (!initialize)
				{

					initialize = true;
				}

				if (P.owner == Main.myPlayer)
				{
					if (soundInstance == null || soundInstance.State != SoundState.Playing)
					{
						SoundEngine.TryGetActiveSound(SoundEngine.PlaySound(Sounds.Items.Weapons.NovaLaserLoop, O.position), out ActiveSound result);
						soundInstance = result.Sound;
						soundInstance.Volume = Main.soundVolume;
					}
				}
				P.velocity = Vector2.Normalize(Lead.velocity);
				P.Center = oPos;
				P.timeLeft = 2;
				P.rotation = P.velocity.ToRotation() - MathHelper.PiOver2;

				maxRange = Math.Min(maxRange + 16f, Max_Range);

				for (P.ai[1] = 0f; P.ai[1] <= maxRange; P.ai[1] += 4f)
				{
					Vector2 end = oPos + P.velocity * P.ai[1];
					if (CollideMethods.CheckCollide(end, 0, 0))
					{
						P.ai[1] -= 4f;
						break;
					}
				}

				float leadDist = Vector2.Distance(oPos, Lead.Center);
				for (float i = leadDist; i < P.ai[1]; i += P.width)
				{
					if (Main.rand.NextBool(25))
					{
						float k = Math.Min(i, P.ai[1]);
						Vector2 dPos = oPos - P.Size / 2 + P.velocity * k;
						Main.dust[Dust.NewDust(dPos, P.width, P.width, 75, 0, 0, 100, default(Color), 2f)].noGravity = true;
					}
				}

				Vector2 dustPos = oPos + P.velocity * P.ai[1];
				float num1 = P.velocity.ToRotation() + (Main.rand.NextBool(2) ? 1.0f : -1.0f) * MathHelper.PiOver2;
				float num2 = (float)(Main.rand.NextDouble() * 0.8f + 1.0f);
				Vector2 dustVel = new Vector2((float)Math.Cos(num1) * num2, (float)Math.Sin(num1) * num2);
				Dust dust = Main.dust[Dust.NewDust(dustPos, 0, 0, 75, dustVel.X, dustVel.Y, 100, default(Color), 2f)];
				dust.noGravity = true;
				dust.velocity *= 3f;
				dust.position = dustPos;

				Color color = MetroidMod.novColor;
				DelegateMethods.v3_1 = new Vector3(color.R / 255f, color.G / 255f, color.B / 255f);
				Utils.PlotTileLine(P.Center, P.Center + P.velocity * P.ai[1], 26, DelegateMethods.CastLight);

				if (P.numUpdates == 0)
				{
					scaleUp = Math.Min(scaleUp + 0.1f, 1f);
					P.frame++;
					if (P.frame >= Main.projFrames[P.type])
					{
						P.frame = 0;
					}
				}
			}
			P.netUpdate = true;
		}

		public override bool ShouldUpdatePosition()
		{
			return false;
		}
		public override void SendExtraAI(BinaryWriter writer) => writer.Write(BeamLength);
		public override void ReceiveExtraAI(BinaryReader reader) => BeamLength = reader.ReadInt32();
		public override void CutTiles(MProjectile m)
		{
			Projectile P = m.Projectile;
			DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
			Utils.PlotTileLine(P.Center, P.Center + P.velocity * (P.ai[1] + 4f), (P.width + 16) * P.scale, DelegateMethods.CutTiles);
		}

		public override void OnHitNPC(MProjectile mProjectile, NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(39, 600, true);
			target.immune[mProjectile.Projectile.owner] = 4;
		}

		public override bool? Colliding(MProjectile mp, Rectangle projHitbox, Rectangle targetHitbox)
		{
			Projectile P = mp.Projectile;
			float point = 0f;
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), P.Center,
				P.Center + P.velocity * P.ai[1], P.width, ref point);
		}

		public override void OnKill(MProjectile mProjectile, int timeLeft)
		{
			soundInstance?.Stop(true);
		}

		public override bool PreDrawProjectile(MProjectile mProjectile, ref Color lightColor)
		{
			SpriteBatch sb = Main.spriteBatch;
			if (Lead.active && Lead != null)
			{
				Projectile P = mProjectile.Projectile;
				Player O = Main.player[P.owner];
				Vector2 oPos = O.RotatedRelativePoint(O.MountedCenter, true);

				Texture2D tex = ModContent.Request<Texture2D>(ShotTexture).Value;

				int tHeight = tex.Height / Main.projFrames[P.type];

				Vector2 scale = new Vector2(scaleUp, 1f);

				int bodyFrameCount = 3;

				int tailHeight = 22;
				int headHeight = 22;
				int bodyHeight = 30 / bodyFrameCount;

				float leadDist = Vector2.Distance(oPos, Lead.Center);

				for (float i = leadDist; i <= P.ai[1]; i += bodyHeight)
				{
					Vector2 pos = P.Center + P.velocity * i;

					int height = Math.Min(bodyHeight, (int)(P.ai[1] - i - headHeight / 2));

					int frame = Main.rand.Next(bodyFrameCount);

					if (height > 0)
					{
						sb.Draw(tex, pos - Main.screenPosition,
						new Rectangle?(new Rectangle(0, tailHeight + 2 + (tHeight * P.frame) + (bodyFrameCount * frame), tex.Width, height)),
						P.GetAlpha(Color.White), P.rotation,
						new Vector2((float)tex.Width / 2f, 0f),
						scale, SpriteEffects.None, 0f);
					}
				}

				if (P.ai[1] > leadDist + headHeight / 2)
				{
					Vector2 pos2 = P.Center + P.velocity * P.ai[1];
					sb.Draw(tex, pos2 - Main.screenPosition,
					new Rectangle?(new Rectangle(0, tailHeight + 2 + (bodyHeight * bodyFrameCount) + 2 + (tHeight * P.frame), tex.Width, headHeight)),
					P.GetAlpha(Color.White), P.rotation,
					new Vector2((float)tex.Width / 2f, headHeight - 3),
					scale, SpriteEffects.None, 0f);
				}
			}

			return false;// base.PreDrawProjectile(mProjectile, ref lightColor);
		}
		public override void SetItemDefaults(Item item)
		{
			item.value = 70000;
			item.rare = ItemRarityID.LightRed;
			base.SetItemDefaults(item);
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.ChlorophyteBar, 10)
				.AddIngredient(ItemID.Emerald)
				.AddIngredient(ItemID.LunarTabletFragment)
				.AddIngredient(ItemID.BeetleHusk, 2)
				//.AddTile(TileID.LunarCraftingStation);
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
