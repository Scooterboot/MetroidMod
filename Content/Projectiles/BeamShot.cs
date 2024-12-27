using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroidMod.Content.DamageClasses;
using MetroidMod.ID;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles
{
	internal class BeamShot : MProjectile
	{
		/// <summary>
		/// Stores the winning slot numbers from the Visual Priority check.
		/// </summary>
		public int[] VisualWinners = [-1, -1, 0, 0];
		public ModBeamAddon[] beamAddons = new ModBeamAddon[BeamAddonSlotID.Count]; 
		public float beamScale = 0.75f;
		public int beamDust = DustID.YellowTorch;
		/// <summary>
		/// This string is appended to the end of the shot's texturepath to find unique textures for a specific combination of beams.
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
		public bool canPhase = false;
		/// <summary>
		/// The amount of tile interactions the shot can perform before dying.
		/// </summary>
		public int TileInteract = 0;
		/// <summary>
		/// The amount of entity interactions the shot can perform before dying.
		/// </summary>
		public int EntityInteract = 0;

		/// <summary>
		/// This beam shot's impact sound effect.
		/// <br/><br/>Defaults to the <b>fallback impact SFX</b>.
		/// </summary>
		public SoundStyle Impact = MetroidMod.BeamImpactFallbackSFX;
		/// <summary>
		/// This beam shot's texture after addons are applied.
		/// </summary>
		public Texture2D ModTexture;

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
		public override string Texture => $"{nameof(MetroidMod)}/Assets/Textures/BeamAddons/PowerBeam/Shot";
		Color color = MetroidMod.powColor; //todo: learn shaders        -Z
		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.scale = 0.75f;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.ignoreWater = true;
			Projectile.penetrate = 1;
			Projectile.DamageType = ModContent.GetInstance<HunterDamageClass>();

		}
		public override void SetStaticDefaults()
		{
			//Main.projFrames[Type] = 16;
		}

		//public override bool PreAI()
		//{
		//	return true;
		//}
		public override void AI() //TODO: make a whole-ass thing         -Z
		{
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + MathHelper.PiOver2;
			Lighting.AddLight(Projectile.Center, color.R / 255f, color.G / 255f, color.B / 255f);

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


			//Put the dustline shit here later

			if (Projectile.numUpdates == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, beamDust, 0, 0, 100, default(Color), Projectile.scale);
				Main.dust[dust].noGravity = true;
			}
		}
		//public override void PostAI()
		//{
		//	base.PostAI();
		//}
		public override void OnKill(int timeLeft)
		{
			Vector2 pos = Projectile.position;
			
			//Copied from MProjectile.DustyDeath()
			int freq = 20;
			bool noGravity = true;
			for (int i = 0; i < freq; i++)
			{
				int dust = Dust.NewDust(pos, Projectile.width, Projectile.height, beamDust, 0, 0, 100, color, Projectile.scale * beamScale);
				Main.dust[dust].velocity = new Vector2((Main.rand.Next(freq) - (freq / 2)) * 0.125f, (Main.rand.Next(freq) - (freq / 2)) * 0.125f);
				Main.dust[dust].noGravity = noGravity;
			}
			if (VisualWinners[0] != -1)
			{
				if (ModContent.RequestIfExists(beamAddons[VisualWinners[(VisualWinners[3] == 1) ? 1 : 0]].ImpactSound, out Asset<SoundEffect> asset))
				{
					SoundStyle sound = new($"{Mod.Name}/" + asset.Name);
					SoundEngine.PlaySound(sound, Projectile.Center); 
				}
				else 
				{ 
					SoundStyle sound = new($"{Mod.Name}/Assets/Sounds/ArmCannon/BeamImpactSound");
					SoundEngine.PlaySound(sound, Projectile.Center); 
				}
			}
		}

		//public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		//{
		//	//inject onhitnpc code here
		//	base.OnHitNPC(target, hit, damageDone);
		//}
		//public override void OnHitPlayer(Player target, Player.HurtInfo info)
		//{
		//	//Could do some cool shit here.
		//	base.OnHitPlayer(target, info);
		//}

		public override bool PreDraw(ref Color lightColor)
		{
			if (VisualWinners[0] == -1 || VisualWinners[1] == -1 || beamAddons == null){ return true; }
			ModBeamAddon beamShape = beamAddons[VisualWinners[0]];
			ModBeamAddon beamColor = beamAddons[VisualWinners[1]];
			lightColor = beamColor.ShotColor;
			beamDust = beamColor.ShotDust;
			if (ModTexture != null)
			{
				MetroidMod.Instance.Logger.Info("We ain't null apparently");
				//This here rectangle is the chunk of the texture that the sprite actually uses
				Rectangle renderFrame = new Rectangle(0, 0, ModTexture.Width, ModTexture.Height / ShotFrames);

				//Shift it down to properly select the correct frame
				renderFrame.Y = 0 + (renderFrame.Height * Projectile.frame);

				Main.EntitySpriteDraw(ModTexture, Projectile.Center - Main.screenPosition, renderFrame, beamColor.ShotColor, Projectile.rotation,
								  new Vector2(ModTexture.Width / 2, ModTexture.Height / 2), beamScale, SpriteEffects.None);
			}
			else
			{
				ModTexture = (Texture2D)ModContent.Request<Texture2D>(Texture);
				MetroidMod.Instance.Logger.Info("We nullin. Texture path: " + Texture);
				Main.EntitySpriteDraw(ModTexture, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, ModTexture.Width, ModTexture.Height), beamColor.ShotColor, Projectile.rotation,
								  new Vector2(ModTexture.Width / 2, ModTexture.Height / 2), beamScale, SpriteEffects.None);
			}
			

			//TODO: make a system to handle framecounts
			return false;
		}
	}
}
