using System;
using System.IO;
using MetroidMod.Content.Items.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.Imperialist
{
	public class ImperialistShot : MProjectile
	{
		//what a total mess lmao --Dr
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 100;
		}
		private bool spaze = false;
		private int depth = 0;
		private float hitRange = 0;
		public override void OnSpawn(IEntitySource source)
		{
			if (source is EntitySource_Parent parent && parent.Entity is Player player && player.HeldItem.type == ModContent.ItemType<PowerBeam>())
			{
				if (player.HeldItem.ModItem is PowerBeam hold)
				{
					shot = hold.shotEffect.ToString();
					if (hold.shotAmt > 1)
					{
						spaze = true;
					}
					if (shot.Contains("wave") || shot.Contains("nebula"))
					{
						depth = waveDepth;
						mProjectile.WaveBehavior(Projectile, true);
					}
				}
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
			base.OnSpawn(source);
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 8;
			Projectile.height = 32;
			Projectile.scale = 1f;
			mProjectile.amplitude = 7 * Projectile.scale;
			mProjectile.wavesPerSecond = 10f;
			mProjectile.delay = 0;
			//Projectile.tileCollide = false;

		}
		private float BeamLength
		{
			get {
				return Projectile.localAI[1];
			}

			set {
				Projectile.localAI[1] = value;
			}
		}
		private const float Max_Range = 2200f;
		private float maxRange = 0f;
		private float scaleUp = 0f;
		public override void AI()
		{
			Projectile P = Projectile;
			P.timeLeft = 100;
			P.velocity = Vector2.Normalize(P.velocity);
			P.rotation = P.velocity.ToRotation() - 1.57f;
			P.usesLocalNPCImmunity = true;
			P.localNPCHitCooldown = 18;
			P.stopsDealingDamageAfterPenetrateHits = true;
			if (shot.Contains("wave") || shot.Contains("nebula"))
			{
				depth = waveDepth;
				//mProjectile.WaveBehavior(Projectile, true);
			}

			if (P.numUpdates == 0)
			{
				scaleUp = Math.Min(scaleUp + 0.1f, 1f);
				P.frame++;
				if (P.frame >= Main.projFrames[P.type])
				{
					P.Kill();
					//P.frame = 0;
				}
			}
			P.scale = 0.75f * scaleUp;
			if (P.frame >= 6)
			{
				P.damage = 0;
			}
			//P.damage *= (int)(1d + (mp.impStealth / 125d));
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
			writer.Write(BeamLength);
			writer.Write(depth);
			writer.Write(hitRange);
			writer.Write(Projectile.penetrate);
			writer.Write(Projectile.maxPenetrate);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			spaze = reader.ReadBoolean();
			BeamLength = reader.ReadInt32();
			depth = reader.ReadInt32();
			hitRange = reader.ReadInt32();
			Projectile.penetrate = reader.ReadInt32();
			Projectile.maxPenetrate = reader.ReadInt32();
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			Projectile P = Projectile;
			float visualBeamLength = maxRange - 14.5f;
			Vector2 centerFloored = P.Center.Floor() + P.velocity * 16f;
			Vector2 endPosition = centerFloored + P.velocity * visualBeamLength;
			float _ = float.NaN;
			if (projHitbox.Intersects(targetHitbox))
			{
				return true;
			}
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), P.Center, endPosition, P.width, ref _);
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if(Projectile.penetrate == 1)
			{
				hitRange = Vector2.Distance(target.Center, Projectile.Center);
			}
			base.OnHitNPC(target, hit, damageDone);
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Projectile P = Projectile;
			if (P.velocity == Vector2.Zero)
			{
				return false;
			}
			if (spaze)
			{
				mProjectile.WaveBehavior(P, true);
			}
			Texture2D texture = TextureAssets.Projectile[P.type].Value;
			float visualBeamLength = maxRange - 14.5f;
			Vector2 centerFloored = P.Center.Floor() + P.velocity * 16f;
			Vector2 drawScale = new(scaleUp, 1f);
			DelegateMethods.f_1 = 1f;
			Vector2 startPosition = centerFloored - Main.screenPosition;
			Vector2 endPosition = startPosition + P.velocity * visualBeamLength;

			for (P.ai[1] = 0f; P.ai[1] <= maxRange; P.ai[1] += 4f)
			{
				Vector2 end = P.Center + P.velocity * P.ai[1];
				Vector2 trueEnd = end + P.velocity * depth * P.ai[1] * 8f;
				if (CollideMethods.CheckCollide(trueEnd, 0, 0) && hitRange == 0)
				{
					P.ai[1] -= 4f;
					maxRange = Vector2.Distance(trueEnd, P.Center);
					break;
				}
				if(hitRange > 0)
				{
					P.ai[1] -= 4f;
					maxRange = hitRange;
					break;
				}
				else
				{
					maxRange = Max_Range;
				}
			}
			drawScale *= 0.25f;
			DrawBeam(Main.spriteBatch, texture, startPosition, endPosition, drawScale, P.GetAlpha(Color.White));

			return false;
		}
		private static void DrawBeam(SpriteBatch spriteBatch, Texture2D texture, Vector2 startPosition, Vector2 endPosition, Vector2 drawScale, Color beamColor)
		{
			Utils.LaserLineFraming lineFraming = new(DelegateMethods.RainbowLaserDraw);

			// c_1 is an unnamed decompiled variable which is the render color of the beam drawn by DelegateMethods.RainbowLaserDraw.
			DelegateMethods.c_1 = beamColor;
			Utils.DrawLaser(spriteBatch, texture, startPosition, endPosition, drawScale, lineFraming);
		}
		public override void CutTiles()
		{
			// tilecut_0 is an unnamed decompiled variable which tells CutTiles how the tiles are being cut (in this case, via a Projectile).
			DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
			Utils.TileActionAttempt cut = new(DelegateMethods.CutTiles);
			float visualBeamLength = maxRange - 14.5f;
			Vector2 centerFloored = Projectile.Center.Floor() + Projectile.velocity * 16f;
			//Vector2 beamStartPos = centerFloored - Main.screenPosition;
			Vector2 beamEndPos = centerFloored + Projectile.velocity * visualBeamLength;

			// PlotTileLine is a function which performs the specified action to all tiles along a drawn line, with a specified width.
			// In this case, it is cutting all tiles which can be destroyed by Projectiles, for example grass or pots.
			Utils.PlotTileLine(Projectile.Center, beamEndPos, Projectile.width * Projectile.scale, cut);
		}
	}
}

