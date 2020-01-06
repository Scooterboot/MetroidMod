using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using MetroidMod.Projectiles;
using System.IO;

namespace MetroidMod.Projectiles.chargelead
{
	public class ChargeLead : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Charge Attack");
		}
		public override void SetDefaults()
		{
			projectile.width = 16;
			projectile.height = 16;
			projectile.aiStyle = -1;
			projectile.timeLeft = 8800;
			projectile.ownerHitCheck = true;
			projectile.friendly = false;
			projectile.hostile = false;
			projectile.tileCollide = false;
			projectile.penetrate = 1;
			projectile.ignoreWater = true;
			projectile.ranged = true;
		}

		public string ChargeUpSound = "ChargeStartup_Power",
				ChargeTex = "ChargeLead",
				ShotSound = "none",
				ChargeShotSound = "none";
		public int ChargeShotAmt = 1,
				DustType = 64;
		public Color DustColor = default(Color),
				LightColor = MetroidMod.powColor;
		public bool canPsuedoScrew = false;
		public bool missile = false;

		bool soundPlayed = false;
		SoundEffectInstance soundInstance;
		int negateUseTime = 0;
		public override void AI()
		{
			Projectile P = projectile;
			Player O = Main.player[P.owner];			
			MPlayer mp = O.GetModPlayer<MPlayer>(mod);
			
			mp.chargeColor = LightColor;

			P.scale = mp.statCharge / MPlayer.maxCharge;
			Item I = O.inventory[O.selectedItem];
			
			if(negateUseTime < I.useTime-2)
				negateUseTime++;
			
			float dmgMult = (1f+((float)mp.statCharge*0.04f));
			int damage = (int)((float)I.damage*O.rangedDamage);

			P.friendly = false;
			P.damage = 0;
			if (mp.somersault)
			{
				P.alpha = 255;
				if (canPsuedoScrew && mp.statCharge >= MPlayer.maxCharge)
				{
					P.friendly = true;
					P.damage = damage * 5 * ChargeShotAmt;
					//mp.overheatDelay = (I.useTime*2);
				}
			}
			else
				P.alpha = 0;

			if (mp.statCharge == 10)
			{
				soundInstance = Main.PlaySound(SoundLoader.customSoundType, (int)P.Center.X, (int)P.Center.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/"+ChargeUpSound));
			}
			else if(mp.statCharge >= MPlayer.maxCharge && !soundPlayed)
			{
				Main.PlaySound(SoundLoader.customSoundType, (int)P.Center.X, (int)P.Center.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/ChargeMax"));
				if(soundInstance != null)
					soundInstance.Stop(true);
				soundPlayed = true;
			}

			if(O.controlUseItem && !mp.ballstate && !mp.shineActive && !O.dead && !O.noItems)
			{
				if (P.owner == Main.myPlayer)
				{
					float MY = Main.mouseY + Main.screenPosition.Y;
					float MX = Main.mouseX + Main.screenPosition.X;
					if (O.gravDir == -1f)
						MY = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;

					Vector2 oPos = O.RotatedRelativePoint(O.MountedCenter, true);
					float targetrotation = (float)Math.Atan2((MY - oPos.Y), (MX - oPos.X));

					Vector2 newVelocity = targetrotation.ToRotationVector2() * 26;

					if (newVelocity.X != P.velocity.X || newVelocity.Y != P.velocity.Y)
						P.netUpdate = true;

					P.velocity = newVelocity;
				}
			}
			else
			{
				if(mp.statCharge >= 30)
				{
					O.itemTime = I.useTime;
					O.itemAnimation = I.useAnimation;
				}
				else
				{
					O.itemTime = I.useTime-negateUseTime;
					O.itemAnimation = I.useAnimation-negateUseTime;
				}
				P.Kill();
			}

			P.position = O.RotatedRelativePoint(O.MountedCenter) - P.Size / 2f;
			P.rotation = P.velocity.ToRotation();
			P.spriteDirection = P.direction;
			O.ChangeDir(P.direction);
			O.itemTime = 2;
			O.itemAnimation = 2;
			P.timeLeft = 2;
			O.heldProj = P.whoAmI;
			O.itemRotation = (float)Math.Atan2(P.velocity.Y * O.direction, P.velocity.X * O.direction) - O.fullRotation;

			if (mp.statCharge >= MPlayer.maxCharge && !mp.somersault)
			{
				int dust = Dust.NewDust(P.position + P.velocity, P.width, P.height, DustType, 0, 0, 100, DustColor, 2.0f);
				Main.dust[dust].noGravity = true;
			}
			Lighting.AddLight(P.Center, (LightColor.R / 255f) * P.scale, (LightColor.G / 255f) * P.scale, (LightColor.B / 255f) * P.scale);
		}

		public override void Kill(int timeLeft)
		{
			MPlayer mp = Main.player[projectile.owner].GetModPlayer<MPlayer>(mod);

			if (!mp.ballstate)
			{
				// Charged shot sounds played here for network purposes.
				if (((mp.statCharge >= (MPlayer.maxCharge * 0.5) && !missile) || (mp.statCharge >= MPlayer.maxCharge && missile)) && ChargeShotSound != "none")
					Main.PlaySound(SoundLoader.customSoundType, (int)projectile.position.X, (int)projectile.position.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/" + ChargeShotSound));
				else if ((mp.statCharge >= 30 || missile) && ShotSound != "none")
					Main.PlaySound(SoundLoader.customSoundType, (int)projectile.position.X, (int)projectile.position.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/" + ShotSound));

				mp.statCharge = 0;
			}
			if (soundInstance != null)
				soundInstance.Stop(true);
			soundPlayed = false;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = ModLoader.GetMod(UIParameters.MODNAME).GetTexture("Projectiles/chargelead/" + ChargeTex);
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (projectile.spriteDirection == -1)
				spriteEffects = SpriteEffects.FlipHorizontally;

			Main.spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), projectile.GetAlpha(Color.White), projectile.rotation, new Vector2((float)tex.Width/2, (float)tex.Height/2), projectile.scale, spriteEffects, 0f);
			return false;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(DustType);
			writer.Write(ChargeTex);
			writer.Write(ShotSound);
			writer.Write(ChargeUpSound);
			writer.Write(ChargeShotSound);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			DustType = reader.ReadInt32();
			ChargeTex = reader.ReadString();
			ShotSound = reader.ReadString();
			ChargeUpSound = reader.ReadString();
			ChargeShotSound = reader.ReadString();
		}
	}
}
