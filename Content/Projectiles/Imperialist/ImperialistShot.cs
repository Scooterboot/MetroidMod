using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using MetroidMod.Content.Items.Weapons;
using System.IO;
using MetroidMod.Common.Players;
using Terraria.GameContent;
using Terraria.Enums;

namespace MetroidMod.Content.Projectiles.Imperialist
{
	public class ImperialistShot : MProjectile
	{
		//what a total mess lmao --Dr
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 100;
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

			string S  = PowerBeam.SetCondition(Main.player[Projectile.owner]);
			if (S.Contains("green"))
			{
				Projectile.penetrate = 6;
			}
			if (S.Contains("nova"))
			{
				Projectile.penetrate = 8;
			}
			if (S.Contains("solar"))
			{
				Projectile.penetrate = 12;
			}
		}
		private float BeamLength
		{
			get 
			{
				return Projectile.localAI[1];
			}

			set 
			{
				Projectile.localAI[1] = value;
			}
		}
		private bool spaze;
		private const float Max_Range = 2200f;
		private float maxRange = 0f;
		private float scaleUp = 0f;
		public override void AI()
		{
			Projectile P = Projectile;
			P.timeLeft = 100;
			P.velocity = Vector2.Normalize(P.velocity);
			P.rotation = P.velocity.ToRotation() - 1.57f;
			P.stopsDealingDamageAfterPenetrateHits = true;

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
			//P.damage *= (int)(1d + (mp.impStealth / 125d));
		}
		public override bool ShouldUpdatePosition()
		{
			PowerBeam held = Main.player[Projectile.owner].inventory[MetroidMod.Instance.selectedItem].ModItem as PowerBeam;
			if (held.shotsy > 1)
			{
				spaze = true;
				return true;
			}
			spaze = false;
			return false;
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(spaze);
			writer.Write(BeamLength);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			BeamLength = reader.ReadSingle();
			spaze = reader.ReadBoolean();
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			Projectile P = Projectile;
			float visualBeamLength = maxRange - 14.5f;
			Vector2 centerFloored = P.Center.Floor() + P.velocity * 16f;
			Vector2 endPosition = centerFloored + P.velocity * visualBeamLength;
			float _ = float.NaN;
			/*if (projHitbox.Intersects(targetHitbox))
			{
				return true;
			}*/
			if (P.frame <= 5)
			{
				return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), P.Center, endPosition, P.width, ref _);
			}
			return false;
		}
		private int GetDepth(MProjectile mp)
		{
			string S = PowerBeam.SetCondition(Main.player[Projectile.owner]);
			if (S.Contains("wave") || S.Contains("nebula"))
			{
				return mp.waveDepth;
			}
			return 0;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			if (Projectile.velocity == Vector2.Zero)
			{
				return false;
			}
			Projectile P = Projectile;
			PowerBeam held = Main.player[P.owner].inventory[MetroidMod.Instance.selectedItem].ModItem as PowerBeam;
			if (held.shotsy > 1)
			{
				mProjectile.WaveBehavior(P, true);
			}
			MProjectile meep = mProjectile;
			Texture2D texture = TextureAssets.Projectile[P.type].Value;
			float visualBeamLength = maxRange - 14.5f;
			Vector2 centerFloored = P.Center.Floor() + P.velocity * 16f;
			Vector2 drawScale = new(scaleUp, 1f);
			DelegateMethods.f_1 = 1f;
			Vector2 startPosition = centerFloored - Main.screenPosition;
			Vector2 endPosition = startPosition + P.velocity * visualBeamLength;

			for (P.ai[1] = 0f; P.ai[1] <= Max_Range; P.ai[1] += 4f)
			{
				Vector2 end = P.Center + P.velocity * P.ai[1];
				Vector2 trueEnd = end + P.velocity * GetDepth(meep) * P.ai[1] * 8f;
				if (CollideMethods.CheckCollide(trueEnd, 0, 0))
				{
					P.ai[1] -= 4f;
					maxRange = Vector2.Distance(trueEnd, P.Center);
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

