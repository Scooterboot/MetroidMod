using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using MetroidModPorted.Common.Players;

namespace MetroidModPorted.Content.Projectiles
{
	class ChargeLead : Default.BeamShot
	{
		public override string Texture => $"{Mod.Name}/Assets/Texture/ChargeLead/ChargeLead";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Charge Attack");
		}
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
		}

		public string ChargeUpSound = "Assets/Sounds/ChargeStartup_Power",
				ChargeTex = "Assets/Texture/ChargeLead/ChargeLead",
				ShotSound = "none",
				ChargeShotSound = "none";
		public Mod ChargeUpSoundMod = MetroidModPorted.Instance,
				ChargeTexMod = MetroidModPorted.Instance,
				ShotSoundMod = MetroidModPorted.Instance,
				ChargeShotSoundMod = MetroidModPorted.Instance;
		public int ChargeShotAmt = 1,
				DustType = 64;
		public Color DustColor = default(Color),
				LightColor = MetroidModPorted.powColor;
		public bool canPsuedoScrew = false;
		public bool missile = false;
		public int comboSound = 0;
		public bool noSomersault = false;
		public float extraScale = 0f;
		public float aimSpeed = 0f;

		bool soundPlayed = false;
		SoundEffectInstance soundInstance;
		int negateUseTime = 0;
		public override void AI()
		{
			Projectile P = Projectile;
			Player O = Main.player[P.owner];			
			MPlayer mp = O.GetModPlayer<MPlayer>();
			
			mp.chargeColor = LightColor;

			P.scale = mp.statCharge / MPlayer.maxCharge;
			P.scale += extraScale;
			Item I = O.inventory[O.selectedItem];

			if (negateUseTime < I.useTime)
			{
				negateUseTime++;
			}
			
			float dmgMult = (1f+((float)mp.statCharge*0.04f));
			//int damage = (int)((float)I.damage*O.rangedDamage*O.allDamage);
			int damage = O.GetWeaponDamage(I);

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
			{
				P.alpha = 0;
			}

			if (mp.statCharge == 10)
			{
				soundInstance = SoundEngine.PlaySound(SoundLoader.CustomSoundType, (int)P.Center.X, (int)P.Center.Y, SoundLoader.GetSoundSlot(ChargeUpSoundMod, ChargeUpSound));
			}
			else if(comboSound == 1)
			{
				if(mp.statCharge >= MPlayer.maxCharge-20 && !soundPlayed)
				{
					SoundEngine.PlaySound(SoundLoader.CustomSoundType, (int)P.Center.X, (int)P.Center.Y, SoundLoader.GetSoundSlot(Mod, "Assets/Sounds/ChargeComboActivate"));
					soundPlayed = true;
				}
			}
			else if(mp.statCharge >= MPlayer.maxCharge && !soundPlayed)
			{
				SoundEngine.PlaySound(SoundLoader.CustomSoundType, (int)P.Center.X, (int)P.Center.Y, SoundLoader.GetSoundSlot(Mod, "Assets/Sounds/ChargeMax"));
				if (soundInstance != null)
				{
					soundInstance.Stop(true);
				}
				soundPlayed = true;
			}
			if(noSomersault)
			{
				mp.disableSomersault = true;
			}
			
			O.itemTime = 2;
			O.itemAnimation = 2;
			
			Vector2 oPos = O.RotatedRelativePoint(O.MountedCenter, true);

			if(O.controlUseItem && !mp.ballstate && !mp.shineActive && !O.dead && !O.noItems)
			{
				if (P.owner == Main.myPlayer)
				{
					/*float MY = Main.mouseY + Main.screenPosition.Y;
					float MX = Main.mouseX + Main.screenPosition.X;
					if (O.gravDir == -1f)
						MY = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;

					Vector2 oPos = O.RotatedRelativePoint(O.MountedCenter, true);
					float targetrotation = (float)Math.Atan2((MY - oPos.Y), (MX - oPos.X));

					Vector2 newVelocity = targetrotation.ToRotationVector2() * 26;*/
					
					Vector2 mousePos = Main.MouseWorld;
					Vector2 diff = Vector2.Normalize(mousePos - oPos);
					if (float.IsNaN(diff.X) || float.IsNaN(diff.Y))
					{
						diff = -Vector2.UnitY;
					}
					
					if(aimSpeed > 0f && mp.statCharge >= MPlayer.maxCharge)
					{
						diff = Vector2.Normalize(Vector2.Lerp(diff, Vector2.Normalize(P.velocity), aimSpeed));
					}
					
					Vector2 newVelocity = diff * ((30f - 24f*I.scale) + I.scale*I.width);

					if (newVelocity.X != P.velocity.X || newVelocity.Y != P.velocity.Y)
					{
						P.netUpdate = true;
					}

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
			
			P.friendly = false;
			P.damage = 0;
			if(mp.somersault)
			{
				P.alpha = 255;
				if(canPsuedoScrew && mp.statCharge >= MPlayer.maxCharge)
				{
					P.friendly = true;
					P.damage = damage*5*ChargeShotAmt;
				}
				P.position.X = oPos.X-P.width/2;
				P.position.Y = oPos.Y-P.height/2;
				P.velocity = Vector2.Zero;
				if(O.controlLeft)
				{
					O.direction = -1;
				}
				if(O.controlRight)
				{
					O.direction = 1;
				}
			}
			else
			{
				P.position = O.RotatedRelativePoint(O.MountedCenter) - P.Size / 2f;
				P.alpha = 0;
				if(P.velocity.X < 0)
				{
					P.direction = -1;
				}
				else
				{
					P.direction = 1;
				}
				O.ChangeDir(P.direction);
				
				P.position += Vector2.Normalize(P.velocity) * 8f * extraScale;
			}
			P.position.Y += O.gfxOffY;
			P.position.X += (float)(P.width / 2);
			P.position.Y += (float)(P.height / 2);
			P.width = mp.somersault?50:16;
			P.height = mp.somersault?60:16;
			P.position.X -= (float)(P.width / 2);
			P.position.Y -= (float)(P.height / 2);

			P.rotation += 0.5f * P.direction;
			P.spriteDirection = P.direction;
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
			MPlayer mp = Main.player[Projectile.owner].GetModPlayer<MPlayer>();

			if (!mp.ballstate && !mp.shineActive)
			{
				if(Projectile.penetrate > 0)
				{
					// Charged shot sounds played here for network purposes.
					if(comboSound == 0 || mp.statCharge < MPlayer.maxCharge)
					{
						if (((mp.statCharge >= (MPlayer.maxCharge * 0.5) && !missile) || (mp.statCharge >= MPlayer.maxCharge && missile)) && ChargeShotSound != "none")
						{
							SoundEngine.PlaySound(SoundLoader.CustomSoundType, (int)Projectile.position.X, (int)Projectile.position.Y, SoundLoader.GetSoundSlot(ChargeShotSoundMod, ChargeShotSound));
						}
						else if ((mp.statCharge >= 30 || missile) && ShotSound != "none")
						{
							SoundEngine.PlaySound(SoundLoader.CustomSoundType, (int)Projectile.position.X, (int)Projectile.position.Y, SoundLoader.GetSoundSlot(ShotSoundMod, ShotSound));
						}
					}
				}

				mp.statCharge = 0;
			}
			if (soundInstance != null)
			{
				soundInstance.Stop(true);
			}
			soundPlayed = false;
		}

		//public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D tex = ChargeTexMod.Assets.Request<Texture2D>(ChargeTex).Value;
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (Projectile.spriteDirection == -1)
			{
				spriteEffects = SpriteEffects.FlipHorizontally;
			}

			Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), Projectile.GetAlpha(Color.White), Projectile.rotation, new Vector2((float)tex.Width / 2, (float)tex.Height / 2), Projectile.scale, spriteEffects, 0);
			//Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), Projectile.GetAlpha(Color.White), Projectile.rotation, new Vector2((float)tex.Width/2, (float)tex.Height/2), Projectile.scale, spriteEffects, 0f);
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
