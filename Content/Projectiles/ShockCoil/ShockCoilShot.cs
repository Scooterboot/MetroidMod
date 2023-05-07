using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.Enums;
using System.IO;
using MetroidMod.Common.Players;
using MetroidMod.Content.Projectiles;
using MetroidMod.Common.GlobalItems;
namespace MetroidMod.Content.Projectiles.ShockCoil
{
	public class ShockCoilShot : MProjectile
	{
		private float iceSpeed = 0f;
		private float spazSpeed = 0f;
		private float plasSpeed = 0f;
		private float shots = 0f;
		private int overheat = Common.Configs.MConfigItems.Instance.overheatPowerBeam;
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("ShockCoil Shot");
            Main.projFrames[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.scale = 1f;
			Projectile.penetrate = -1;
            Projectile.extraUpdates = 5;
			Projectile.usesLocalNPCImmunity = true;
		}

		Vector2 targetPos;
        bool setTargetPos = false;

        Projectile Lead;

        NPC target;

        const float Max_Range = 300f;
        float range = Max_Range;
        const float Max_Distance = 300f;
        float distance = Max_Distance;

        Vector2 oPos;
        Vector2 mousePos;

        SoundEffectInstance soundInstance;
        bool soundPlayed = false;
        int soundDelay = 30;

		int ampSyncCooldown = 20;
        float[] amp = new float[3];
        float[] ampDest = new float[3];
		int useTime = Common.Configs.MConfigItems.Instance.useTimePowerBeam;

		public override void AI()
        {
			Projectile P = Projectile;
            Player O = Main.player[P.owner];
			MPlayer mp = O.GetModPlayer<MPlayer>();

			Lead = Main.projectile[(int)P.ai[0]];

			if (Projectile.Name.Contains("Wave") || Projectile.Name.Contains("Nebula"))
			{
				Projectile.tileCollide = false;
				//mProjectile.WaveBehavior(Projectile);
			}
			if (P.numUpdates == 0)
            {
                P.frame++;
            }
            if (P.frame > 1)
            {
                P.frame = 0;
            }

            range = Max_Range;
            distance = Max_Distance;

            oPos = O.RotatedRelativePoint(O.MountedCenter, true);

            /*if (Lead != null && Lead.active)
            {
                for (int k = 0; k < range; k++)
                {
                    float targetrot = (float)Math.Atan2((P.Center.Y - O.Center.Y), (P.Center.X - O.Center.X));
                    Vector2 tilePos = O.Center + targetrot.ToRotationVector2() * k;
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
            }*/
            if (P.owner == Main.myPlayer && !O.dead)
            {
                P.netUpdate = true;

                Vector2 diff = Main.MouseWorld - oPos;
                diff.Normalize();

                mousePos = oPos + diff * Math.Min(Vector2.Distance(oPos, Main.MouseWorld), range);

                target = null;
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    if (Main.npc[i].active && Main.npc[i].lifeMax > 5 && !Main.npc[i].dontTakeDamage && !Main.npc[i].friendly)
                    {
                        NPC npc = Main.npc[i];
                        Rectangle npcRect = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);

                        float point = 0f;
                        if (Vector2.Distance(oPos, npc.Center) < range &&
                        Collision.CheckAABBvLineCollision(npcRect.TopLeft(), npcRect.Size(), oPos, P.Center, P.width, ref point))
                        {
                            range = Vector2.Distance(oPos, npc.Center);
                            mousePos = oPos + diff * Math.Min(Vector2.Distance(oPos, Main.MouseWorld), range);
                        }

                        bool flag = (Vector2.Distance(oPos, npc.Center) <= range + distance && Vector2.Distance(npc.Center, mousePos) <= distance);

                        if (Main.npc[i].CanBeChasedBy(P, false))
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
                        targetPos.X += (float)Main.rand.Next(-15, 16) * (Vector2.Distance(oPos, P.Center) / Max_Range);
                        targetPos.Y += (float)Main.rand.Next(-15, 16) * (Vector2.Distance(oPos, P.Center) / Max_Range);
                    }
                }

                if (P.numUpdates == 0)
                {
                    if (soundDelay <= 0)
                    {
                        if (!soundPlayed)
                        {
                            SoundEngine.TryGetActiveSound(SoundEngine.PlaySound(Sounds.Items.Weapons.ShockCoilSound, O.position), out ActiveSound result);
                            soundInstance = result.Sound;
                            soundPlayed = true;
                            soundDelay = 50;
                        }
						if (mp.statCharge == MPlayer.maxCharge && mp.statOverheat < mp.maxOverheat)
						{
							SoundEngine.TryGetActiveSound(SoundEngine.PlaySound(Sounds.Items.Weapons.ShockCoilAffinity2, O.position), out ActiveSound result);
							soundInstance = result.Sound;
							soundDelay = 40;
						}

						else
                        {
                            if (soundInstance != null)
                            {
                                soundInstance.Stop(true);
                            }
                            SoundEngine.TryGetActiveSound(SoundEngine.PlaySound(Sounds.Items.Weapons.ShockCoilSound, O.position), out ActiveSound result);
                            soundInstance = result.Sound;
                            soundDelay = 40;
                        }
                    }
                    else
                    {
                        soundDelay--;
                    }
                    for (int i = 0; i < 3; i++)
                    {
                        ampDest[i] = Main.rand.Next(-15, 16);
                    }
                }

                if (ampSyncCooldown-- <= 0)
                {
                    ampSyncCooldown = 20;
                    Projectile.netUpdate2 = true;
                }
            }

            float speed = Math.Max(8f, Vector2.Distance(targetPos, P.Center) * 0.25f);
            float targetAngle = (float)Math.Atan2((targetPos.Y - P.Center.Y), (targetPos.X - P.Center.X));
            P.velocity = targetAngle.ToRotationVector2() * speed;

			if (O.controlUseItem)
			{
				P.timeLeft = 5;
			}
			else
			{
				P.Kill();
			}

			if (P.numUpdates == 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    ampDest[i] = Main.rand.Next(-15, 16);
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
			if (mp.statOverheat > mp.maxOverheat || (mp.statCharge == MPlayer.maxCharge && mp.statOverheat == mp.maxOverheat))
			{
				P.Kill();
				//SoundEngine.PlaySound(Sounds.Items.Weapons.ShockCoilLoad, Projectile.position);
			}

		}

		public override void CutTiles()
		{
			Player p = Main.player[Projectile.owner];
			if (p.controlUseItem)
			{
				DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
				Utils.PlotTileLine(p.Center, Projectile.Center, (Projectile.width + 16) * Projectile.scale, DelegateMethods.CutTiles);
			}
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			Player p = Main.player[Projectile.owner];
			if (p.controlUseItem)
			{
				float point = 0f;
				return projHitbox.Intersects(targetHitbox) ||
					Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), p.Center, Projectile.Center, Projectile.width, ref point);
			}
			return false;
		}

		public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            Projectile P = Projectile;
			Color color = MetroidMod.powColor;
			Player p = Main.player[Projectile.owner];
			MPlayer mp = p.GetModPlayer<MPlayer>();

			Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[P.type].Value;
            int num108 = tex.Height / Main.projFrames[P.type];
            int y4 = num108 * P.frame;


            if (p.controlUseItem && !p.dead)
            {
				float targetrot = (float)Math.Atan2((P.Center.Y - p.Center.Y), (P.Center.X - p.Center.X));
                float dist = Math.Max(Vector2.Distance(p.Center, P.Center), 1);

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

                    pos[i] = p.Center + targetrot.ToRotationVector2() * (dist / num) * i;
                    pos[i].X += (float)Math.Cos(trot) * shift * (Vector2.Distance(oPos, P.Center) / Max_Range);
                    pos[i].Y += (float)Math.Sin(trot) * shift * (Vector2.Distance(oPos, P.Center) / Max_Range);

                    float rot = (float)Math.Atan2((pos[i].Y - p.Center.Y), (pos[i].X - p.Center.X)) + (float)Math.PI / 2;
                    if (i > 0)
                    {
                        rot = (float)Math.Atan2((pos[i].Y - pos[i - 1].Y), (pos[i].X - pos[i - 1].X)) + (float)Math.PI / 2;
                    }
                    sb.Draw(tex,
                    pos[i] - Main.screenPosition,
                    new Rectangle?(new Rectangle(0, y4, tex.Width, num108)),
                    P.GetAlpha(Color.White),
                    rot,
                    new Vector2((float)tex.Width / 2f, (float)num108 / 2),
                    new Vector2(scale, 1f),
                    SpriteEffects.None,
                    0f);


					Lighting.AddLight(Projectile.Center, color.R / 255f, color.G / 255f, color.B / 255f);

				}
            }

            return false;
        }

		public override void Kill(int timeLeft)
		{
			if (soundInstance != null)
			{
				soundInstance.Stop(true);
			}
		}
		public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(targetPos);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            targetPos = reader.ReadVector2();
        }
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			iceSpeed = 1f;
			spazSpeed = 1f;
			plasSpeed = 1f;
			shots = 1f;
			Player p = Main.player[Projectile.owner];
			MPlayer mp = p.GetModPlayer<MPlayer>();
			

			if (Projectile.Name.Contains("Vortex"))
			{
				shots = 5f;
				spazSpeed = .75f;
			}
			if (Projectile.Name.Contains("Spazer"))
			{
				shots = 3f;
				spazSpeed = 1.15f;
			}
			//mp.statOverheat += Math.Max(((int)((float)overheat * mp.overheatCost) / shots), 6 / shots);
			if (mp.statCharge < MPlayer.maxCharge && mp.statOverheat < mp.maxOverheat)
			{
				mp.statCharge += 10 / shots;
				//mp.statCharge = Math.Min(((mp.statCharge + 7) / shots), MPlayer.maxCharge);
			}
			if (mp.statCharge == MPlayer.maxCharge && mp.statOverheat < mp.maxOverheat || mp.statCharge >= MPlayer.maxCharge && mp.statOverheat < mp.maxOverheat)
			{
				int healingAmount = Math.Min(damage / 20, 5);
				p.statLife += healingAmount;
				p.HealEffect(healingAmount, true);
				mp.Energy += Math.Min(damage / 20, 5);
			}
			SoundEngine.PlaySound(Sounds.Items.Weapons.ShockCoilAffinity1, Projectile.position);
			if (Projectile.Name.Contains("Plasma"))
			{
				if (Projectile.Name.Contains("Ice"))
				{
					target.AddBuff(44, 300);
				}
				else
				{
					target.AddBuff(24, 300);
				}
			}
			if (Projectile.Name.Contains("Nova"))
			{
				if (Projectile.Name.Contains("Ice"))
				{
					target.AddBuff(44, 300);
				}
				else
				{
					target.AddBuff(39, 300);
				}
			}
			if (Projectile.Name.Contains("Ice") || Projectile.Name.Contains("Stardust"))
			{
				string buffName = "IceFreeze";
				iceSpeed = 1.3f;
				target.AddBuff(Mod.Find<ModBuff>(buffName).Type, 300);
			}
			if (Projectile.Name.Contains("Solar"))
			{
				target.AddBuff(189, 300);
				plasSpeed = 1.15f;
			}
			if (Projectile.Name.Contains("Plasma"))
			{
				plasSpeed = 1.15f;
			}
			Projectile.localNPCHitCooldown = (int)Math.Round((double)(useTime * 7) * iceSpeed * spazSpeed * plasSpeed);
		}
	}
	public class IceShockCoilShot : ShockCoilShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice ShockCoil Shot";
		}
	}
	public class IceWaveShockCoilShot : ShockCoilShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave ShockCoil Shot";
		}
	}
	public class IceWaveSpazerShockCoilShot : ShockCoilShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Spazer ShockCoil Shot";
		}
	}
	public class IceWaveSpazerPlasmaRedShockCoilShot : ShockCoilShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Spazer Plasma Red ShockCoil Shot";
		}
	}
	public class IceWavePlasmaRedShockCoilShot : ShockCoilShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Plasma Red ShockCoil Shot";
		}
	}
	public class IceSpazerShockCoilShot : ShockCoilShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Spazer ShockCoil Shot";
		}
	}
	public class IceSpazerPlasmaRedShockCoilShot : ShockCoilShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Spazer Plasma Red ShockCoil Shot";
		}
	}
	public class IcePlasmaRedShockCoilShot : ShockCoilShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Plasma Red ShockCoil Shot";
		}
	}
	public class WaveShockCoilShot : ShockCoilShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave ShockCoil Shot";
		}
	}
	public class WaveSpazerShockCoilShot : ShockCoilShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Spazer ShockCoil Shot";
		}
	}
	public class WaveSpazerPlasmaRedShockCoilShot : ShockCoilShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Spazer Plasma Red ShockCoil Shot";
		}
	}
	public class WavePlasmaRedShockCoilShot : ShockCoilShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Plasma Red ShockCoil Shot";
		}
	}
	public class SpazerShockCoilShot : ShockCoilShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Spazer ShockCoil Shot";
		}
	}
	public class SpazerPlasmaRedShockCoilShot : ShockCoilShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Spazer Plasma Red ShockCoil Shot";
		}
	}
	public class PlasmaRedShockCoilShot : ShockCoilShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Red ShockCoil Shot";
		}
	}
	public class IceWaveWideNovaShockCoilShot : ShockCoilShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Wide Nova ShockCoil Shot";
		}
	}
	public class IceWaveNovaShockCoilShot : ShockCoilShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Nova ShockCoil Shot";
		}
	}
	public class IceWideNovaShockCoilShot : ShockCoilShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wide Nova ShockCoil Shot";
		}
	}
	public class IceNovaShockCoilShot : ShockCoilShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Nova ShockCoil Shot";
		}
	}
	public class WaveWideShockCoilShot : ShockCoilShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Wide ShockCoil Shot";
		}
	}
	public class WaveWideNovaShockCoilShot : ShockCoilShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Wide Nova ShockCoil Shot";
		}
	}
	public class WaveNovaShockCoilShot : ShockCoilShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Nova ShockCoil Shot";
		}
	}
	public class WideNovaShockCoilShot : ShockCoilShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wide Nova ShockCoil Shot";
		}
	}
	public class NovaShockCoilShot : ShockCoilShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Nova ShockCoil Shot";
		}
	}
	public class StardustNebulaVortexShockCoilShot : ShockCoilShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Stardust Nebula Vortex ShockCoil Shot";
		}
	}
	public class StardustNebulaVortexSolarShockCoilShot : ShockCoilShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Stardust Nebula Vortex Solar ShockCoil Shot";
		}
	}
	public class StardustNebulaSolarShockCoilShot : ShockCoilShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Stardust Nebula Solar ShockCoil Shot";
		}
	}
	public class StardustVortexSolarShockCoilShot : ShockCoilShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Stardust Vortex Solar ShockCoil Shot";
		}
	}
	public class StardustSolarShockCoilShot : ShockCoilShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Stardust Solar ShockCoil Shot";
		}
	}
	public class NebulaVortexSolarShockCoilShot : ShockCoilShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Nebula Vortex Solar ShockCoil Shot";
		}
	}
	public class NebulaSolarShockCoilShot : ShockCoilShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Nebula Solar ShockCoil Shot";
		}
	}
	public class VortexSolarShockCoilShot : ShockCoilShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Vortex Solar ShockCoil Shot";
		}
	}
	public class SolarShockCoilShot : ShockCoilShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Solar ShockCoil Shot";
		}
	}
}
