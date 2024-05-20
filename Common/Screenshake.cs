using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.Graphics.CameraModifiers;
using MetroidMod.Common.Configs;
using MetroidMod.Content.Projectiles;
using MetroidMod.Content.Projectiles.powerbeam;
using MetroidMod.Content.Projectiles.icebeam;
using MetroidMod.Content.Projectiles.wavebeam;
using MetroidMod.Content.Projectiles.spazer;
using MetroidMod.Content.Projectiles.plasmabeamgreen;
using MetroidMod.Content.Projectiles.plasmabeamred;
using MetroidMod.Content.Projectiles.missiles;

namespace MetroidMod.Common{
	public class MScreenshakeGlobalItem : GlobalItem{}
	public class MScreenshakeGlobalProjectile : GlobalProjectile{
		//	[Antinous]: Jopojelly said screenshake properties might be better suited as a dedicated function, and I think he's right.
		//	GabeHasWon also said I can write it as static, which makes it accessible by other classes. I'll need to do this at some point.
		//	NOTE: Remember to add screenshake for Power Bombs and Metroid Prime: Hunters weapons.
		public void DoScreenshake(Projectile projectile, int intensity){
			PunchCameraModifier ShakeVeryWeak = new PunchCameraModifier(projectile.Center, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), 1f, 6f, 10, 100f, FullName);
			PunchCameraModifier ShakeWeak = new PunchCameraModifier(projectile.Center, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), 2.5f, 6f, 20, 100f, FullName);
			PunchCameraModifier ShakeNormal = new PunchCameraModifier(projectile.Center, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), 5f, 6f, 20, 100f, FullName);
			PunchCameraModifier ShakeStrong = new PunchCameraModifier(projectile.Center, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), 10f, 6f, 20, 250f, FullName);
			PunchCameraModifier ShakeVeryStrong = new PunchCameraModifier(projectile.Center, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), 15f, 6f, 20, 500f, FullName);
			PunchCameraModifier ShakeDefault = new PunchCameraModifier(projectile.Center, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), 20f, 6f, 20, 1000f, FullName);
			if(intensity == 1){Main.instance.CameraModifiers.Add(ShakeVeryWeak);}
			if(intensity == 2){Main.instance.CameraModifiers.Add(ShakeWeak);}
			if(intensity == 3){Main.instance.CameraModifiers.Add(ShakeNormal);}
			if(intensity == 4){Main.instance.CameraModifiers.Add(ShakeStrong);}
			if(intensity == 5){Main.instance.CameraModifiers.Add(ShakeVeryStrong);}
			if(intensity == 6){Main.instance.CameraModifiers.Add(ShakeDefault);}
		}
		//	[Antinous]: This function may be necessary to avoid copy-pasting the boolean list.
		public void AddScreenshake(Projectile projectile, int behavior){
			//	[Antinous]: I'd like to add this list to the MScreenshakeGlobalProjectile class itself, but I'm not sure how yet..
			bool configFire = (MConfigClient.Instance.WeaponFireScreenshake);
			bool configCollide = (MConfigClient.Instance.WeaponCollideScreenshake);
			bool BeamShot = (projectile.Name.Contains("Beam") && projectile.Name.Contains("Shot"));
			bool ChargeShot = (projectile.Name.Contains("Charge") && projectile.Name.Contains("Shot"));
			bool MissileShot = (projectile.Name.Contains("Missile") && projectile.Name.Contains("Shot"));
			bool SuperMissile = (projectile.Name.Contains("Super") && projectile.Name.Contains("Missile") || projectile.Name.Contains("Nebula") && projectile.Name.Contains("Missile") || projectile.Name.Contains("Stardust") && projectile.Name.Contains("Missile"));
			bool Bomb = (projectile.Name.Contains("Bomb"));
			bool isBeamSmall = (BeamShot || projectile.Name.Contains("Spazer"));
			bool isBeamLarge = (ChargeShot);
			bool isMissile = (MissileShot);
			bool isSuperMissile = (SuperMissile);
			bool isBomb = (Bomb);
			//	[Antinous]: I need to figure out how to exclude every other projectile except for Metroid Mod's.
			if(isBeamSmall){
				if(behavior==1 && configFire){DoScreenshake(projectile,1);}
				if(behavior==3 && configCollide){DoScreenshake(projectile,2);}
			}
			if(isBeamLarge){
				if(behavior==1 && configFire){DoScreenshake(projectile,2);}
				if(behavior==3 && configCollide){DoScreenshake(projectile,3);}
			}
			if(isMissile){
				if(behavior==1 && configFire){DoScreenshake(projectile,2);}
				if(behavior==3 && configCollide){DoScreenshake(projectile,4);}
			}
			if(isSuperMissile){
				if(behavior==1 && configFire){DoScreenshake(projectile,3);}
				if(behavior==3 && configCollide){DoScreenshake(projectile,5);}
			}
			if(isBomb){
				if(behavior==3 && configCollide){DoScreenshake(projectile,4);}
			}
		}
		public override void OnSpawn(Projectile projectile, IEntitySource source){
			AddScreenshake(projectile,1);
		}
		public override void AI(Projectile projectile){}
		public override void OnKill(Projectile projectile, int timeLeft){
			AddScreenshake(projectile,3);
		}
	}
	public class MScreenshakeGlobalNPC : GlobalNPC{
		//	[Antinous]: To be used for Metroid Mod's NPCs at some point, such as the Sidehoppers.
		public override void OnSpawn(NPC npc, IEntitySource source){}
		public override void AI(NPC npc){}
		public override void OnKill(NPC npc){}
	}
}
