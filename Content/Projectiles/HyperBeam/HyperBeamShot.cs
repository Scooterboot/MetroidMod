using System;
using MetroidMod.Common.Players;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using static Terraria.ModLoader.PlayerDrawLayer;

namespace MetroidMod.Content.Projectiles.HyperBeam
{
	public class HyperBeamShot : MProjectile
	{
		//todo: this is dumb
		//rewrite like most of this
		public override string Texture => $"{Mod.Name}/Assets/Textures/BeamAddons/HyperBeam/Shot";

		public ModBeamAddon[] beamAddons = new ModBeamAddon[BeamAddonSlotID.Count - 2]; //Hyper Beam doesn't need the charge slot (since it's already known) or the ammo slot (doesn't use ammo)

		/// <summary>
		/// This string is appended to the end of the shot's texturepath to find unique textures for a specific combination of beams.
		/// </summary>
		public string fileMod = "";



		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Hyper Beam Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.scale = 2f;
		}

		float scale = 0f;
		public void OnInitialized(IEntitySource source)
		{
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + MathHelper.PiOver2;
			scale = Projectile.scale;

			//Gather data from installed addons.

			//First, call method to calculate tileinteract total.
			TileInteract = BeamAddonLoader.InteractStacker(beamAddons, true, 2.5f);
			//Then, call method to calculate entityinteract total.
			EntityInteract = BeamAddonLoader.InteractStacker(beamAddons, false, 2.5f);


			BeamAddonLoader.AddonOnInitialized(beamAddons, mProjectile, source);
		}

		public override void AI()
		{
			Projectile P = Projectile;
			MPlayer mp = Main.player[P.owner].GetModPlayer<MPlayer>();

			P.rotation = (float)Math.Atan2((double)P.velocity.Y, (double)P.velocity.X) + MathHelper.PiOver2;

			Lighting.AddLight(P.Center, (float)mp.r / 255f, (float)mp.g / 255f, (float)mp.b / 255f);

			P.localAI[0] = Math.Min(P.localAI[0] + 0.075f, 1f);
			P.localAI[1] = Math.Min(P.localAI[1] + 0.025f, 1f);

			P.scale = scale * P.localAI[0];
		}

		public override bool PreDraw(ref Color lightColor)
		{
			float scale = 0.65f;
			if (Projectile.Name.Contains("Plasma"))
			{
				scale = 1f;
			}
			MPlayer mp = Main.player[Projectile.owner].GetModPlayer<MPlayer>();
			mProjectile.PlasmaDrawTrail(Projectile, Main.player[Projectile.owner], Main.spriteBatch, 10, scale * Projectile.localAI[0] * Projectile.localAI[1], new Color(mp.r, mp.g, mp.b, 128));
			return false;
		}
		public override void OnKill(int timeLeft)
		{
			MPlayer mp = Main.player[Projectile.owner].GetModPlayer<MPlayer>();
			mProjectile.DustyDeath(Projectile, 66, true, 1f, new Color(mp.r, mp.g, mp.b, 255));
		}
		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			modifiers.ArmorPenetration += 50;
			base.ModifyHitNPC(target, ref modifiers);
		}

	}
}
