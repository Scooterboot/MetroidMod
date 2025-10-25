using System;
using System.IO;
using MetroidMod.Common.GlobalItems;
using MetroidMod.Content.BeamAddons;
using MetroidMod.Content.Projectiles;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.MissileAddons.BeamCombos
{
	public class Wavebuster : ModMissileAddon
	{
		public override bool AddOnlyAddonItem => false;
		public override Color PrimaryColor => MetroidMod.powColor;
		public override Color SecondaryColor => MetroidMod.powSecondaryColor;
		public override int ShotDust => DustID.PurpleTorch;
		public override bool HoldFire => true;
		public override int ShotFrames => 2;
		private string Loop => $"{Mod.Name}/Assets/Sounds/MissileAddons/{Name}/Loop";
		private string ShotTexture2 => $"{Mod.Name}/Assets/Textures/MissileAddons/{Name}/Shot2";

		private Vector2 targetPos;
		private bool setTargetPos = false;

		private NPC target;

		private int dmg = 0;
		private int immuneTime = 0;
		private const float Max_Range = 300f;
		private float range = Max_Range;
		private const float Max_Distance = 300f;
		private float distance = Max_Distance;

		private Vector2 oPos;
		private Vector2 mousePos;

		private SoundEffectInstance soundInstance;
		private bool soundPlayed = false;
		private int soundDelay = 0;

		private int ampSyncCooldown = 20;
		private readonly float[] amp = new float[3];
		private readonly float[] ampDest = new float[3];
		public override void SetStaticDefaults()
		{
			AddonSlot = MissileAddonSlotID.Charge;
			base.SetStaticDefaults();
		}
		public override void SetItemDefaults(Item item)
		{
			Item.value = 50000;
			Item.rare = ItemRarityID.LightRed;
			base.SetItemDefaults(item);
		}
		public override void SetProjectileDefaults(MProjectile mProjectile)
		{
			base.SetProjectileDefaults(mProjectile);
			mProjectile.Projectile.width = 8;
			mProjectile.Projectile.height = 8;
			mProjectile.Projectile.scale = 1f;
			mProjectile.Projectile.tileCollide = false;
			mProjectile.Projectile.penetrate = -1;
			mProjectile.Projectile.extraUpdates = 5;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddRecipeGroup(MetroidMod.T2HMBarRecipeGroupID, 10)
				.AddIngredient(ItemID.SoulofNight, 1)
				.AddIngredient(ItemID.Amethyst, 1)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
		public override void OnSpawn(MProjectile mProjectile, IEntitySource source)
		{
			dmg = mProjectile.Projectile.damage;
			base.OnSpawn(mProjectile, source);
		}
		public override void HoldFireBehavior(Player player, ChargeLead lead)
		{
			Item item = player.HeldItem;
			Vector2 mPos = player.RotatedRelativePoint(player.MountedCenter, true);
			Lead = lead.Projectile;
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
				Projectile.NewProjectile(player.GetSource_ItemUse(item), mPos.X, mPos.Y, velocity.X, velocity.Y, ProjectileType, 0, 0, player.whoAmI);
				Initialized = true;
			}
		}
		public override void AI(MProjectile mProjectile)
		{

			Projectile P = mProjectile.Projectile;
			Player O = Main.player[P.owner];
			//if (O.HeldItem.GetGlobalItem<MGlobalItem>().statMissiles <= 0 || O.HeldItem.GetGlobalItem<MGlobalItem>().isBeam)
			//{
			//	P.Kill();
			//}
			if (!Lead.active || Lead.owner != P.owner || Lead.type != ModContent.ProjectileType<ChargeLead>())
			{
				P.Kill();
				return;
			}

			if (P.numUpdates == 0)
			{
				P.frame++;
			}
			if (P.frame > 1)
			{
				P.frame = 0;
			}
			if (immuneTime > 0)
			{
				P.damage = 0;
				immuneTime--;
			}
			else
			{
				P.damage = dmg;
			}
			range = Max_Range;
			distance = Max_Distance;

			oPos = O.RotatedRelativePoint(O.MountedCenter, true);

			if (Lead != null && Lead.active)
			{
				for (int k = 0; k < range; k++)
				{
					float targetrot = (float)Math.Atan2((P.Center.Y - Lead.Center.Y), (P.Center.X - Lead.Center.X));
					Vector2 tilePos = Lead.Center + targetrot.ToRotationVector2() * k;
					int i = (int)MathHelper.Clamp(tilePos.X / 16, 0, Main.maxTilesX - 2);
					int j = (int)MathHelper.Clamp(tilePos.Y / 16, 0, Main.maxTilesY - 2);

					if (Main.tile[i, j] != null && Main.tile[i, j].HasTile && Main.tileSolid[Main.tile[i, j].TileType] && !Main.tileSolidTop[Main.tile[i, j].TileType])
					{
						range = Math.Max(range - 1, 1);
						distance = Math.Max(distance - 1, 1);
					}
					else
					{
						range = Math.Min(range + 1, Max_Range);
						distance = Math.Min(distance + 1, Max_Distance);
					}
				}
			}

			if (P.owner == Main.myPlayer)
			{
				P.netUpdate = true;

				Vector2 diff = Main.MouseWorld - oPos;
				diff.Normalize();

				mousePos = oPos + diff * Math.Min(Vector2.Distance(oPos, Main.MouseWorld), range);

				target = null;
				foreach (NPC who in Main.ActiveNPCs)
				{
					NPC npc = Main.npc[who.whoAmI];
					if (npc.lifeMax > 5 && !npc.dontTakeDamage && !npc.friendly)
					{
						Rectangle npcRect = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);

						float point = 0f;
						if (Vector2.Distance(oPos, npc.Center) < range &&
						Collision.CheckAABBvLineCollision(npcRect.TopLeft(), npcRect.Size(), oPos, P.Center, P.width, ref point))
						{
							range = Vector2.Distance(oPos, npc.Center);
							mousePos = oPos + diff * Math.Min(Vector2.Distance(oPos, Main.MouseWorld), range);
						}

						bool flag = (Vector2.Distance(oPos, npc.Center) <= range + distance && Vector2.Distance(npc.Center, mousePos) <= distance);

						if (npc.CanBeChasedBy(P, false))
						{
							if (target == null || !target.active)
							{
								if (flag)
								{
									target = npc;
								}
							}
							else
							{
								if (npc != target && flag && Vector2.Distance(npc.Center, mousePos) < Vector2.Distance(target.Center, mousePos))
								{
									target = npc;
								}

								if (Vector2.Distance(oPos, target.Center) > range + distance || Vector2.Distance(target.Center, mousePos) > distance)
								{
									target = null;
								}
							}
						}
					}
				}

				if (!setTargetPos)
				{
					targetPos = P.Center;
					setTargetPos = true;
					return;
				}
				else if (target != null && target.active)
				{
					targetPos = target.Center;
				}
				else
				{
					if (P.numUpdates == 0)
					{
						//targetPos = new Vector2(mousePos.X + Main.rand.Next(-30, 31), mousePos.Y + Main.rand.Next(-30, 31));
						targetPos = oPos + diff * range;
						targetPos.X += (float)Main.rand.Next(-30, 31) * (Vector2.Distance(oPos, P.Center) / Max_Range);
						targetPos.Y += (float)Main.rand.Next(-30, 31) * (Vector2.Distance(oPos, P.Center) / Max_Range);
					}
				}

				if (P.numUpdates == 0)
				{
					if (soundDelay <= 0)
					{
						if (!soundPlayed)
						{
							SoundEngine.TryGetActiveSound(SoundEngine.PlaySound(new(ShotSound), O.position), out ActiveSound result);
							soundInstance = result.Sound;
							soundPlayed = true;
							soundDelay = 50;
						}
						else
						{
							if (soundInstance != null)
							{
								soundInstance.Stop(true);
							}
							SoundEngine.TryGetActiveSound(SoundEngine.PlaySound(new(Loop), O.position), out ActiveSound result);
							soundInstance = result.Sound;
							soundDelay = 138;
						}
					}
					else
					{
						soundDelay--;
					}
					for (int i = 0; i < 3; i++)
					{
						ampDest[i] = Main.rand.Next(-30, 31);
					}
				}

				if (ampSyncCooldown-- <= 0)
				{
					ampSyncCooldown = 20;
					P.netUpdate2 = true;
				}
			}

			float speed = Math.Max(8f, Vector2.Distance(targetPos, P.Center) * 0.025f);
			float targetAngle = (float)Math.Atan2((targetPos.Y - P.Center.Y), (targetPos.X - P.Center.X));
			P.velocity = targetAngle.ToRotationVector2() * speed;

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
				for (int i = 0; i < 3; i++)
				{
					ampDest[i] = Main.rand.Next(-30, 31);
				}
			}

			for (int i = 0; i < 3; i++)
			{
				if (amp[i] < ampDest[i])
				{
					amp[i] += 3;
				}
				else
				{
					amp[i] -= 3;
				}
			}
		}
		public override void OnKill(MProjectile mProjectile, int timeLeft)
		{
			soundInstance?.Stop(true);
		}
		public override bool? Colliding(MProjectile projectile, Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (Lead != null && Lead.active)
			{
				float point = 0f;
				return projHitbox.Intersects(targetHitbox) ||
					Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Lead.Center, mProjectile.Projectile.Center, mProjectile.Projectile.width, ref point);
			}
			return false;
		}
		public override bool? CanHitNPC(MProjectile projectile, NPC target3)
		{
			if (target != target3 || immuneTime > 0)
			{
				return false;
			}
			return projectile.Projectile.ModProjectile.CanHitNPC(target3);
		}
		public override void CutTiles(MProjectile mProjectile)
		{
			if (Lead != null && Lead.active)
			{
				DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
				Utils.PlotTileLine(Lead.Center, mProjectile.Projectile.Center, (mProjectile.Projectile.width + 16) * mProjectile.Projectile.scale, DelegateMethods.CutTiles);
			}
		}
		public override bool PreDrawProjectile(MProjectile mProjectile, ref Color lightColor)
		{
			SpriteBatch sb = Main.spriteBatch;
			Projectile P = mProjectile.Projectile;

			Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[P.type].Value;
			int num108 = tex.Height / Main.projFrames[P.type];
			int y4 = num108 * P.frame;

			Texture2D tex2 = ModContent.Request<Texture2D>(ShotTexture2).Value;
			int numH = tex2.Height / 4;

			if (Lead != null && Lead.active)
			{
				float targetrot = (float)Math.Atan2((P.Center.Y - Lead.Center.Y), (P.Center.X - Lead.Center.X));
				float dist = Math.Max(Vector2.Distance(Lead.Center, P.Center), 1);

				double trot = targetrot + Math.PI / 2;

				float shift = 0;
				int num = (int)Math.Max(Math.Ceiling(dist / 8), 1);
				float num4 = num / 4;
				Vector2[] pos = new Vector2[num];
				for (int i = 0; i < num; i++)
				{
					float scale = P.scale;
					if (P.frame == 0)
					{
						scale *= 0.8f;
					}

					if (num4 >= 1)
					{
						if (i < num4)
						{
							shift = MathHelper.Lerp(0, amp[0], (i / num4));
						}
						else if (i < num / 2)
						{
							shift = MathHelper.Lerp(amp[0], amp[1], ((i - num4) / num4));
						}
						else if (i < num4 * 3)
						{
							shift = MathHelper.Lerp(amp[1], amp[2], ((i - num / 2) / num4));
						}
						else
						{
							shift = MathHelper.Lerp(amp[2], 0, ((i - num4 * 3) / num4));
							scale *= (num4 - (i - num4 * 3) * 0.5f) / num4;
						}
					}

					pos[i] = Lead.Center + targetrot.ToRotationVector2() * (dist / num) * i;
					pos[i].X += (float)Math.Cos(trot) * shift * (Vector2.Distance(oPos, P.Center) / Max_Range);
					pos[i].Y += (float)Math.Sin(trot) * shift * (Vector2.Distance(oPos, P.Center) / Max_Range);

					float rot = (float)Math.Atan2((pos[i].Y - Lead.Center.Y), (pos[i].X - Lead.Center.X)) + (float)Math.PI / 2;
					if (i > 0)
					{
						rot = (float)Math.Atan2((pos[i].Y - pos[i - 1].Y), (pos[i].X - pos[i - 1].X)) + (float)Math.PI / 2;
					}
					sb.Draw(tex, pos[i] - Main.screenPosition, new Rectangle?(new Rectangle(0, y4, tex.Width, num108)), P.GetAlpha(Color.White), rot, new Vector2((float)tex.Width / 2f, (float)num108 / 2), new Vector2(scale, 1f), SpriteEffects.None, 0f);

					sb.Draw(tex2, pos[i] - Main.screenPosition, new Rectangle?(new Rectangle(0, numH * Main.rand.Next(4), tex2.Width, numH)), P.GetAlpha(Color.White), rot, new Vector2((float)tex2.Width / 2, (float)numH / 2), (float)(Main.rand.Next(21) / 10), SpriteEffects.None, 0f);

					Lighting.AddLight(pos[i], (MetroidMod.waveColor2.R / 255f) * P.scale, (MetroidMod.waveColor2.G / 255f) * P.scale, (MetroidMod.waveColor2.B / 255f) * P.scale);

					if (Main.rand.NextBool(25))
					{
						Vector2 dPos = pos[i] - new Vector2(tex.Width / 2, tex.Width / 2);
						Main.dust[Dust.NewDust(dPos, tex.Width, tex.Width, 62, 0, 0, 100, default(Color), 2f)].noGravity = true;
					}
				}
			}

			return false;
		}
		public override void OnHitNPC(MProjectile mProjectile, NPC target, NPC.HitInfo hit, int damageDone)
		{
			Player O = Main.player[mProjectile.Projectile.owner];
			if (damageDone > 0)
			{
				immuneTime += O.HeldItem.useTime;
				mProjectile.Projectile.localNPCHitCooldown = O.HeldItem.useTime;
			}
			base.OnHitNPC(mProjectile, target, hit,damageDone);
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.WriteVector2(targetPos);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			targetPos = reader.ReadVector2();
		}
	}
}
