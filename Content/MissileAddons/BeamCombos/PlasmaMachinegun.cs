using System;
using MetroidMod.Content.Projectiles;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;

namespace MetroidMod.Content.MissileAddons.BeamCombos
{
	public class PlasmaMachinegun : ModMissileAddon
	{
		public override bool AddOnlyAddonItem => false;
		public override Color PrimaryColor => MetroidMod.plaGreenColor;
		public override Color SecondaryColor => MetroidMod.plaGreenColor2;
		public override int ShotDust => DustID.GreenTorch;
		public override bool HoldFire => true;
		private Vector2 start = Vector2.Zero;
		private Vector2 startPos = Vector2.Zero;
		private static readonly int comboUseTime = 6;
		private int comboTime = 6;
		private SoundEffectInstance soundInstance;
		private int waveDir = -1;
		private readonly int miniGunAmt = 1;
		private float scalePlus = 0f;
		public override void SetStaticDefaults()
		{
			AddonSlot = MissileAddonSlotID.Charge;
			base.SetStaticDefaults();
		}
		public override void SetProjectileDefaults(MProjectile mProjectile)
		{
			base.SetProjectileDefaults(mProjectile);
			mProjectile.Projectile.width = 10;
			mProjectile.Projectile.height = 10;
			mProjectile.Projectile.scale = 1f;
			mProjectile.Projectile.penetrate = 15;
			mProjectile.Projectile.usesLocalNPCImmunity = true;
			mProjectile.Projectile.localNPCHitCooldown = 8;
			mProjectile.amplitude = 4f;
			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 1;
		}
		public override void HoldFireBehavior(Player player, Projectile lead)
		{
			Item item = player.HeldItem;
			Lead = lead;
			if (Lead.active)
			{
				if (comboTime <= 0)
				{
					SoundEngine.TryGetActiveSound(SoundEngine.PlaySound(new(ShotSound), player.Center), out ActiveSound result);
					soundInstance = result.Sound;
					if (soundInstance != null)
					{
						soundInstance.Volume *= 1f - (0.25f * (scalePlus / 20f));
					}

					float spray = 1f * (scalePlus / 20f);

					float scaleFactor2 = 14f;

					var entitySource = player.GetSource_ItemUse(item);
					for (int i = 0; i < miniGunAmt; i++)
					{
						float rot = Lead.velocity.ToRotation() + (float)Angle.ConvertToRadians(Main.rand.Next(18) * 10) - ((float)Math.PI / 2f);
						Vector2 vector3 = Lead.Center + (rot.ToRotationVector2() * 7f * spray);
						Vector2 vector5 = Vector2.Normalize(Lead.velocity) * scaleFactor2;
						vector5 = vector5.RotatedBy(((Main.rand.NextDouble() * 0.12) - 0.06) * spray, default(Vector2));
						if (float.IsNaN(vector5.X) || float.IsNaN(vector5.Y))
						{
							vector5 = -Vector2.UnitY; //this can turn the shots into a cursed flame candle with fargos hypermode and or/enough speed
						}
						int proj = Projectile.NewProjectile(entitySource, vector3.X, vector3.Y, vector5.X, vector5.Y, ProjectileType, item.damage, item.knockBack, player.whoAmI, 0f, 0f);
						Main.projectile[proj].ai[0] = Lead.whoAmI;
						MProjectile mProj = (MProjectile)Main.projectile[proj].ModProjectile;
						mProj.waveDir = waveDir;
					}

					waveDir *= -1;

					comboTime = comboUseTime;
				}
				else
				{
					comboTime--;
				}
				scalePlus = Math.Min(scalePlus + (2f / comboUseTime), 20f);
				//Initialized = true;
			}
		}
		public override void OnSpawn(MProjectile mProjectile, IEntitySource source)
		{
			Projectile P = mProjectile.Projectile;
			P.rotation = (float)Math.Atan2(P.velocity.Y, P.velocity.X) + MathHelper.PiOver2;
			for (int i = 0; i < P.oldPos.Length; i++)
			{
				P.oldPos[i] = P.position;
			}
			for (int i = 0; i < P.oldRot.Length; i++)
			{
				P.oldRot[i] = P.rotation;
			}
			start = P.Center - Lead.Center;
		}
		public override void AI(MProjectile mProjectile)
		{
			Projectile P = mProjectile.Projectile;
			Player O = Main.player[P.owner];

			Color color = MetroidMod.plaGreenColor;
			Lighting.AddLight(P.Center, color.R / 255f, color.G / 255f, color.B / 255f);

			//mProjectile.WaveBehavior(P, false);

			if (P.numUpdates == 0)
			{
				int dust = Dust.NewDust(P.position, P.width, P.height, 61, 0, 0, 100, default(Color), P.scale);
				Main.dust[dust].noGravity = true;

				P.frame++;
				if (P.frame >= Main.projFrames[ProjectileType])
				{
					P.frame = 0;
				}
			}

			Vector2 velocity = P.position - P.oldPos[0];
			if (Vector2.Distance(P.position, P.position + velocity) < Vector2.Distance(P.position, P.position + P.velocity))
			{
				velocity = P.velocity;
			}
			P.rotation = (float)Math.Atan2(velocity.Y, velocity.X) + MathHelper.PiOver2;

			startPos = Lead.Center + O.velocity + start;
		}
		public override bool PreDrawProjectile(MProjectile mProjectile, ref Color lightColor)
		{
			SpriteBatch sb = Main.spriteBatch;
			Projectile P = mProjectile.Projectile;
			Player O = Main.player[P.owner];
			Vector2 oPos = O.RotatedRelativePoint(O.MountedCenter, true);

			float scaleDrop = 0.5f;
			Color color = default(Color);

			Color color2 = Color.White;
			if (color != default(Color))
			{
				color2 = color;
			}
			SpriteEffects effects = SpriteEffects.None;
			if (P.spriteDirection == -1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}
			Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[P.type].Value;
			int height = tex.Height / Main.projFrames[P.type];
			int y4 = height * P.frame;

			float dist = 0f;
			if (Lead != null && Lead.active)
			{
				dist = Vector2.Distance(oPos, Lead.Center);
			}

			float vel = Math.Min(Vector2.Distance(P.Center, startPos), P.velocity.Length());

			int amt = 10;
			for (int i = amt - 1; i > -1; i--)
			{
				if (Vector2.Distance(oPos, P.oldPos[i] + (P.Size / 2f)) >= dist)
				{
					Color color23 = color2;
					color23 = P.GetAlpha(color23);
					color23 *= (amt - i) / ((float)amt);
					//color23.A = (byte)((float)color23.A * ((float)(amt - i) / (float)amt));
					float scale = MathHelper.Lerp(P.scale, P.scale * scaleDrop, (float)i / amt);

					float vel2 = Math.Min(Vector2.Distance(P.oldPos[i] + (P.Size / 2f), startPos), P.velocity.Length());
					if (vel2 > 0)
					{
						for (float j = vel2; j > 0; j--)
						{
							//Color color4 = color23;
							//color4 *= (float)(vel2 - j) / ((float)vel2);
							//color4.A = (byte)((float)color4.A * ((float)(vel2 - j) / (float)vel2));
							Vector2 oldPos = P.oldPos[i] + (P.Size / 2f) - (Vector2.Normalize(P.velocity) * j);
							sb.Draw(tex, oldPos - Main.screenPosition, new Rectangle?(new Rectangle(0, y4, tex.Width, height)),
							color23, P.oldRot[i], new Vector2(tex.Width / 2f, P.height / 2f), scale, effects, 0f);
						}
					}

					sb.Draw(tex, P.oldPos[i] + (P.Size / 2f) - Main.screenPosition, new Rectangle?(new Rectangle(0, y4, tex.Width, height)),
					color23, P.oldRot[i], new Vector2(tex.Width / 2f, P.height / 2f), scale, effects, 0f);
				}
			}
			if (vel > 0)
			{
				for (float j = vel; j > 0; j--)
				{
					//Color color3 = P.GetAlpha(color2);
					//color3 *= (float)(vel - j) / ((float)vel);
					//color3.A = (byte)((float)color3.A * ((float)(vel - j) / (float)vel));
					Vector2 pos = P.Center - (Vector2.Normalize(P.velocity) * j);
					sb.Draw(tex, pos - Main.screenPosition, new Rectangle?(new Rectangle(0, y4, tex.Width, height)),
					P.GetAlpha(color2), P.rotation, new Vector2(tex.Width / 2f, P.height / 2f), P.scale, effects, 0f);
				}
			}
			sb.Draw(tex, P.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, y4, tex.Width, height)),
			P.GetAlpha(color2), P.rotation, new Vector2(tex.Width / 2f, P.height / 2f), P.scale, effects, 0f);

			return false;
		}
		public override void OnKill(MProjectile mProjectile, int timeLeft)
		{
			mProjectile.DustyDeath(mProjectile.Projectile, 61);
		}

		public override void SetItemDefaults(Item item)
		{
			item.value = 50000;
			item.rare = ItemRarityID.LightRed;
			base.SetItemDefaults(item);
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.HallowedBar, 10)
				.AddIngredient(ItemID.CursedFlame, 10)
				.AddIngredient(ItemID.Emerald, 1)
				.AddIngredient(ItemID.SoulofMight, 5)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
