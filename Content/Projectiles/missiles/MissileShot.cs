using System;
using MetroidMod.Content.DamageClasses;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles
{
	internal class MissileShot : MProjectile
	{
		/// <summary>
		/// Stores the winning slot numbers from the Visual Priority check.
		/// </summary>
		public int[] VisualWinners = [-1, -1, 0, 0];
		public ModMissileAddon[] missileAddons = new ModMissileAddon[MissileAddonSlotID.Count];
		public float missileScale = 0.75f;
		public int missileDust = DustID.YellowTorch;
		/// <summary>
		/// This string is appended to the end of the shot's texturepath to find unique textures for a specific combination of missiles.
		/// </summary>
		public string fileMod = "";
		/// <summary>
		/// This string is used to change the projectile's display name to match installed addons.
		/// </summary>
		public string nameChanger;
		/// <summary>
		/// The number of animation frames the shot has.
		/// </summary>
		public int ShotFrames = 1;
		public float multiplier = 1f;
		/// <summary>
		/// Suppresses default dust behavior.
		/// <br/><br/>Defaults to <b>false</b>.
		/// </summary>
		public bool dustSuppress = false;

		/// <summary>
		/// This missile shot's impact sound effect.
		/// <br/><br/>Defaults to the <b>fallback impact SFX</b>.
		/// </summary>
		public SoundStyle Impact = MetroidMod.MissileImpactFallbackSFX;
		/// <summary>
		/// This missile shot's texture after addons are applied.
		/// </summary>
		public Asset<Texture2D> ModTexture;

		private float currentFrame
		{
			get => Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}
		private float shotNumber
		{
			get => Projectile.ai[1];
			set => Projectile.ai[1] = value;
		}
		public override string Texture => $"{nameof(MetroidMod)}/Assets/Textures/MissileAddons/Expansion/Shot";
		Color color = MetroidMod.powColor; //todo: learn shaders        -Z
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.scale = 2f;
			Projectile.timeLeft = 1000;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 1;
			Projectile.friendly = true;
			//Projectile.tileCollide = true;
			Projectile.ignoreWater = true;
			Projectile.penetrate = 1;
			Projectile.DamageType = ModContent.GetInstance<HunterDamageClass>();

		}
		public override void SetStaticDefaults()
		{
			//Main.projFrames[Type] = 16;
			//Bit worried commenting this out may cause issues in the future, but it seems to be fine for now so maybe it won't		-Z
		}

		public void OnInitialized(IEntitySource source)
		{
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + MathHelper.PiOver2;
			//MetroidMod.Instance.Logger.Info("put something here later");


			//Gather data from installed addons.
			/*MetroidMod.Instance.Logger.Info("missile addons: " + missileAddons[0] + " " + missileAddons[1] + " " + missileAddons[2] + " " + missileAddons[3] + " " + missileAddons[4]
				+ "\nShot " + groupID + "/" + groupSize);*/

			//First, call method to calculate tileinteract total.
			TileInteract = MissileAddonLoader.InteractStacker(missileAddons, true, multiplier);
			//Then, call method to calculate entityinteract total.
			EntityInteract = MissileAddonLoader.InteractStacker(missileAddons, false, multiplier);


			MissileAddonLoader.AddonOnInitialized(missileAddons, mProjectile, source);
		}

		public override bool PreAI()
		{
			return MissileAddonLoader.AddonPreAI(missileAddons, mProjectile);
		}
		int dustTimer = 5;
		public override void AI() //TODO: make a whole-ass thing         -Z
		{
			Lighting.AddLight(Projectile.Center, color.R / 255f, color.G / 255f, color.B / 255f);


			#region Animation code
			//If this shot has more than 1 frame, run animation code
			if (ShotFrames > 1)
			{
				//increment the frame counter
				Projectile.frameCounter++;

				//if the required amount of time has passed progress the frame
				if (Projectile.frameCounter > 2)
				{
					Projectile.frame++;
					if (Projectile.frame >= ShotFrames)
					{
						Projectile.frame = 0;
					}
					Projectile.frameCounter = 0;
				}
				//if we're at the frame count reset
			}
			#endregion
			if (VisualWinners[0] != -1)
			{
				missileAddons[VisualWinners[0]].ShapeBehavior(mProjectile);
			}

			//Put the dustline shit here later

			if (dustTimer < 1 && !dustSuppress)
			{
				//MetroidMod.Instance.Logger.Info("Oh hey this actually updates lmao");
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, missileDust, 0, 0, 100, default(Color), Projectile.scale);
				Main.dust[dust].noGravity = true;
				dustTimer = 5;
			}
			else { dustTimer--; }

			MissileAddonLoader.AddonAI(missileAddons, mProjectile);
		}
		public override void PostAI()
		{
			base.PostAI();
			MissileAddonLoader.AddonPostAI(missileAddons, mProjectile);
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			return MissileAddonLoader.AddonTileCollideStyle(missileAddons, mProjectile, ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			//Inject tileinteract code here?
			if (!dustSuppress)
			{
				Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
			}

			return MissileAddonLoader.AddonOnTileCollide(missileAddons, mProjectile, oldVelocity);
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			//inject onhitnpc code here
			MissileAddonLoader.AddonOnHitNPC(missileAddons, mProjectile, target, hit, damageDone);
			if (!SuppressBuff && VisualWinners[1] != -1)
			{
				target.AddBuff(missileAddons[VisualWinners[1]].InflictsBuff, 600);
			}
		}
		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			//Could do some cool shit here.
			MissileAddonLoader.AddonOnHitPlayer(missileAddons, mProjectile, target, info);
		}


		public override void OnKill(int timeLeft)
		{
			Vector2 pos = Projectile.position;

			//Copied from MProjectile.DustyDeath()
			int freq = 20;
			bool noGravity = true;
			for (int i = 0; i < freq; i++)
			{
				int dust = Dust.NewDust(pos, Projectile.width, Projectile.height, missileDust, 0, 0, 100, color, Projectile.scale * missileScale);
				Main.dust[dust].velocity = new Vector2((Main.rand.Next(freq) - (freq / 2)) * 0.125f, (Main.rand.Next(freq) - (freq / 2)) * 0.125f);
				Main.dust[dust].noGravity = noGravity;
			}
			MissileAddonLoader.AddonOnKill(missileAddons, mProjectile, timeLeft);
			SoundEngine.PlaySound(Impact, Projectile.position);
		}



		public override bool PreDraw(ref Color lightColor)
		{
			//if (VisualWinners[0] == -1 || VisualWinners[1] == -1 || missileAddons == null) { return true; }
			//ModMissileAddon missileShape = missileAddons[VisualWinners[0]];
			//ModMissileAddon missileColor = missileAddons[VisualWinners[1]];
			//color = missileColor.PrimaryColor;
			//Color color2 = missileColor.SecondaryColor;
			if (ModTexture != null)
			{
				Rectangle renderFrame = ModTexture.Frame(1, ShotFrames, 0, Projectile.frame);
				Main.EntitySpriteDraw(ModTexture.Value, Projectile.Center - Main.screenPosition, renderFrame, Color.White, Projectile.rotation,
				  new Vector2(ModTexture.Width() / 2, ModTexture.Height() / 2), missileScale, SpriteEffects.None);/*
				//This here rectangle is the chunk of the texture that the sprite actually uses
				Rectangle renderFrame = ModTexture.Frame(1, ShotFrames, 0, Projectile.frame);

				//Shift it down to properly select the correct frame
				renderFrame.Y = 0 + (renderFrame.Height * Projectile.frame);
				if (VisualWinners[0] != VisualWinners[1]) //Color and shape do not match. Begin applying shader.
				{
					Main.spriteBatch.End();
					Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);

					//The code for applying the default color shader to the projectile.

					DrawData data = new DrawData(ModTexture.Value, Projectile.Center - Main.screenPosition, renderFrame, Color.White, Projectile.rotation,
									  new Vector2(ModTexture.Width() / 2, ModTexture.Height() / 2), missileScale, SpriteEffects.None);

					MiscShaderData shaderData = GameShaders.Misc["MetroidModPaletteShader"];
					shaderData.UseColor(color); //Primary color is the bright colors
					shaderData.UseSecondaryColor(color2); //Secondary is the dark colors
					shaderData.UseOpacity(1f); //Affects brightness of the 'core' (the white of the texture)
											   //Defaulting to 1f to keep the core bright
					shaderData.UseSaturation(0f); //Affects saturation of the 'core'
												  //0 to keep the core white instead of being the primary color
					shaderData.UseImage0(ModTexture);

					shaderData.Apply(data);
					data.Draw(Main.spriteBatch);

					Main.spriteBatch.End();
					Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
				} //If the color and shape don't match, apply the recoloring shader
				else //Color and shape match. Shader is not needed and will not be applied.
				{
					Main.EntitySpriteDraw(ModTexture.Value, Projectile.Center - Main.screenPosition, renderFrame, Color.White, Projectile.rotation,
									  new Vector2(ModTexture.Width() / 2, ModTexture.Height() / 2), missileScale, SpriteEffects.None);
				} //If they do, no shader is necessary.*/
			}
			else
			{
				ModTexture = ModContent.Request<Texture2D>(Texture);
				Main.EntitySpriteDraw(ModTexture.Value, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, ModTexture.Width(), ModTexture.Height()), color, Projectile.rotation,
								  new Vector2(ModTexture.Width() / 2, ModTexture.Height() / 2), missileScale, SpriteEffects.None);
			}


			//TODO: Shaders instead of flat coloration
			return false;
		}
	}
}
