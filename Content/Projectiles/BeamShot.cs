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

		private float frameCounter
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
			//Main.projFrames[Type] = ShotFrames;
			//So cool little thing: that value is static. Setting it to a variable means jack shit
			//Which means I need to set that thing to like 1 and then manually count out frames myself
			//and THEN I have to make shaders apply to it since I'm finally going out of my way to learn the damn things
			//And that means I ALSO have to make the ENTIRE CUSTOM VISUAL SYSTEM RIGHT FUCKING NOW in order to make framecounts that vary on charged shots!!!
			//"Why not just make it hardcoded?" Because I'm in too deep already. I already made charge beam modular and not hardcoded.
			//...
			//well at the very least I'll infodump about my plans to try and get them in order
			//so to do this I'm prolly gonna need to have a method inside of ModBeamAddons where you can put your special edge-case scenarios
			//can strings be the crux of switches? I hope, cause that'd be a real neat and tidy way to handle that
			//well at least for the top level of keywords. this would have to create the bottom level.
			//I guess like, for getting array data, you could plug it in
			//maybe I should make a loader method that you can plug an addon array and an addon into and it gives you the array without that addon, that'd make things easier
		}
		//public override bool PreAI()
		//{
		//	return true;
		//}
		public override void AI() //TODO: make a whole-ass thing         -Z
		{
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + MathHelper.PiOver2;
			Lighting.AddLight(Projectile.Center, color.R / 255f, color.G / 255f, color.B / 255f);

			/*if (Main.projFrames[Type] > 1)
			{

			}*/


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
				//TODO: Add an exception-catcher for if there's no asset found, make it default to the normal impact sound          -Z
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
			Texture2D beamTex;
			if (ModContent.RequestIfExists(beamShape.ShotTexture + fileMod, out Asset<Texture2D>modShot))
			{
				beamTex = modShot.Value;
			} //Check if there's an asset for the shot w/ any keywords that may be applied
			else if (ModContent.RequestIfExists(beamShape.ShotTexture, out Asset<Texture2D> noModShot))
			{
				beamTex = noModShot.Value;
			} //Otherwise check if the asset w/o keywords exists
			else
			{
				//Failsafe file
				beamTex = ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/BeamAddons/PowerBeam/Shot").Value;
			} //If it doesn't, bring out the failsafe
			lightColor = beamColor.ShotColor;
			beamDust = beamColor.ShotDust;
			Main.EntitySpriteDraw(beamTex, Projectile.Center - Main.screenPosition, null, beamColor.ShotColor, Projectile.rotation, 
								  new Vector2(beamTex.Width / 2, beamTex.Height / 2), beamScale, SpriteEffects.None);

			//TODO: make a system to handle framecounts
			return false;
		}
	}
}
