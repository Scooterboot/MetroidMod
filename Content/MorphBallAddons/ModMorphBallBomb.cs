using System;
using MetroidMod.Common.Players;
using MetroidMod.Content.DamageClasses;
using MetroidMod.Content.Items;
using MetroidMod.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MetroidMod.Content.MorphBallAddons
{
	public interface IMorphBallBombAddon
	{
		Mod Mod { get; }
		string Name { get; }

		MorphBallBombProjectile BombProjectile { get; }
		string BombProjectileTexture { get; }

		void BombKill(ref int dustType, ref int dustType2, ref float dustScale, ref float dustScale2);
		void BombOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone);
		void BombOnHitPlayer(Player target, Player.HurtInfo info);
	}

	[Autoload(false)]
	public class MorphBallBombProjectile : IGeneratesModProjectile
	{
		public IMorphBallBombAddon producer;

		public string Name => producer.Name + "Bomb";

		public GeneratedModProjectile GeneratedModProjectile { get; internal set; }

		public int ProjectileType { get; internal set; }

		public Projectile Projectile => GeneratedModProjectile.Projectile;

		public LocalizedText ProjectileDisplayName => producer.Mod.GetLocalization($"MorphBallAddons.{producer.Name}.Bomb.DisplayName");

		public string ProjectileTexture => producer.BombProjectileTexture;
		
		public MorphBallBombProjectile(IMorphBallBombAddon producer)
		{
			this.producer = producer;
		}

		internal readonly float light_scale = 0.2f;

		internal readonly float Xthreshold = 8f; //max speed
		internal readonly float BombRadius = 50f; //max speed

		public virtual void ProjectileAI()
		{
			if (Projectile.ai[0] == 0)
			{
				if (Projectile.ai[1]++ > 5)
				{
					Projectile.ai[1] = 6;
					if (Projectile.velocity.Y == 0F && Projectile.velocity.X != 0f)
					{
						Projectile.velocity.X *= .97f;
						if (Projectile.velocity.X > -.01f && Projectile.velocity.X < .01f)
						{
							Projectile.velocity.X = 0;
							Projectile.netUpdate = true;
						}
					}
					Projectile.velocity.Y += .2f;
				}
				Projectile.rotation += Projectile.velocity.X * .1f;
				if (Projectile.velocity.Y > 16)
					Projectile.velocity.Y = 16;
			}

			#region Visuals
			if (Projectile.frameCounter++ >= (int)(Projectile.timeLeft / 3.75f))
			{
				Projectile.frame = (Projectile.frame + 1) % 6;
				Projectile.frameCounter = 0;
			}
			Lighting.AddLight(Projectile.Center, light_scale, light_scale, light_scale);
			#endregion
		}

		public virtual bool? ProjectileCanCutTiles()
		{
			return true;
		}

		public virtual bool? ProjectileCanDamage()
		{
			return null;
		}

		public virtual void ProjectileCutTiles()
		{
			DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
			Utils.PlotTileLine(Projectile.position, Projectile.BottomRight, Projectile.width, DelegateMethods.CutTiles);
		}

		public virtual void ProjectileModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			if (Projectile.timeLeft > 0)
			{
				Projectile.timeLeft = 0;
			}
		}

		public virtual void ProjectileModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
		{
			if (Projectile.timeLeft > 0)
			{
				Projectile.timeLeft = 0;
			}
		}

		public virtual void ProjectileOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			producer.BombOnHitNPC(target, hit, damageDone);
		}

		public virtual void ProjectileOnHitPlayer(Player target, Player.HurtInfo info)
		{
			producer.BombOnHitPlayer(target, info);
		}

		public virtual void ProjectileOnKill(int timeLeft)
		{
			Projectile.position.X = Projectile.position.X + (Projectile.width / 2);
			Projectile.position.Y = Projectile.position.Y + (Projectile.height / 2);
			Projectile.width = (int)(BombRadius * 2f);
			Projectile.height = (int)(BombRadius * 2f);
			Projectile.position.X = Projectile.position.X - (Projectile.width / 2);
			Projectile.position.Y = Projectile.position.Y - (Projectile.height / 2);

			Projectile.Damage();
			//Projectile.ExplodeCrackedTiles(Projectile.position, (int)BombRadius, (int)(Projectile.position.X + Projectile.width - BombRadius), (int)(Projectile.position.X + Projectile.width), (int)(Projectile.height - BombRadius), (int)(Projectile.position.Y + Projectile.height));
			foreach (var npc in Main.ActiveNPCs)
			{
				NPC who = Main.npc[npc.whoAmI];
				if (who.active && !who.friendly && !who.dontTakeDamage && who.type != NPCID.TargetDummy)
				{
					Vector2 direction = who.Center - Projectile.Center;
					float distance = direction.Length();
					direction.Normalize();
					if (distance < BombRadius && !npc.dontTakeDamage)
					{
						//who.SimpleStrikeNPC(Projectile.damage, Projectile.direction, Main.rand.NextFloat() <= Main.player[Projectile.owner].GetCritChance<HunterDamageClass>(), Projectile.knockBack, ModContent.GetInstance<HunterDamageClass>(), true, Main.player[Projectile.owner].luck);
						//
						if (!who.boss)
						{
							who.velocity += direction * (BombRadius - distance);

							if (who.velocity.X > Xthreshold)
								who.velocity.X = Xthreshold;
							if (who.velocity.X < -Xthreshold)
								who.velocity.X = -Xthreshold;
							if (who.velocity.Y > Xthreshold)
								who.velocity.Y = Xthreshold;
							if (who.velocity.Y < -Xthreshold)
								who.velocity.Y = -Xthreshold;
						}
					}
				}
			}

			for (int i = 0; i < 255; ++i)
			{
				Player player = Main.player[i];
				if (player.active && ((player.hostile && player.team != Main.player[Projectile.owner].team) || player.whoAmI == Projectile.owner) && !player.dead)
				{
					Vector2 direction = player.Center - Projectile.Center;
					float distance = direction.Length();
					direction.Normalize();
					if (distance < BombRadius)
					{
						direction *= BombRadius - distance;
						if (player.whoAmI == Projectile.owner)
						{
							if (Math.Abs(player.Center.X - Projectile.Center.X) <= 2f)
							{
								direction.X = 0f;
							}
							direction.Y = -BombRadius;
						}
						player.velocity += direction;// * (BombRadius - distance);
						player.GetModPlayer<MPlayer>().spiderball = false;

						if (player.velocity.X > Xthreshold)
							player.velocity.X = Xthreshold;
						if (player.velocity.X < -Xthreshold)
							player.velocity.X = -Xthreshold;
						if (player.velocity.Y > Xthreshold)
							player.velocity.Y = Xthreshold;
						if (player.velocity.Y < -Xthreshold)
							player.velocity.Y = -Xthreshold;
					}
				}
			}
			SoundEngine.PlaySound(Sounds.Suit.BombExplode, Projectile.Center);

			int dustType = 59, dustType2 = 61;
			float dustScale = 5f, dustScale2 = 5f;

			producer.BombKill(ref dustType, ref dustType2, ref dustScale, ref dustScale2);

			for (int i = 0; i < 25; i++)
			{
				int newDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, 0f, 0f, 100, default(Color), dustScale);
				Main.dust[newDust].velocity *= 1.4f;
				Main.dust[newDust].noGravity = true;

				newDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType2, 0f, 0f, 100, default(Color), dustScale2);
				Main.dust[newDust].velocity *= 1.4f;
				Main.dust[newDust].noGravity = true;
			}
		}

		public virtual bool ProjectileOnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.ai[0] == 0)
			{
				if (Projectile.velocity.X != oldVelocity.X)
					Projectile.velocity.X = Projectile.velocity.X * -.5f;
				if (Projectile.velocity.Y != oldVelocity.X && Projectile.velocity.Y > 1f)
					Projectile.velocity.Y = Projectile.velocity.Y * -.5f;
			}
			return false;
		}

		public virtual bool ProjectilePreDraw(ref Color lightColor)
		{
			return true;
		}

		public virtual void ProjectileSetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;

			Projectile.light = 0.2f;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 1;
			Projectile.timeLeft = 40;

			Projectile.DamageType = ModContent.GetInstance<HunterDamageClass>();
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.ownerHitCheck = true;
		}

		public virtual void ProjectileSetStaticDefaults()
		{
			
		}

		public IGeneratesModProjectile Clone(GeneratedModProjectile newGeneratedModProjectile)
		{
			MorphBallBombProjectile inst = (MorphBallBombProjectile)MemberwiseClone();
			inst.GeneratedModProjectile = newGeneratedModProjectile;
			return inst;
		}
	}

	public abstract class ModMorphBallBomb : ModMorphBallAddon, IMorphBallBombAddon
	{
		public MorphBallBombProjectile BombProjectile { get; internal set; }
		public virtual string BombProjectileTexture => TexturePath + "_Bomb";

		public override MorphBallAddonSlot AddonSlot => MorphBallAddonSlot.Weapon;

		public override void Load()
		{
			base.Load();
			BombProjectile = new MorphBallBombProjectile(this);
			BombProjectile.GeneratedModProjectile = new GeneratedModProjectile(BombProjectile);
			Mod.AddContent(BombProjectile.GeneratedModProjectile);
			BombProjectile.ProjectileType = BombProjectile.Projectile.type;
		}


		public virtual void BombKill(ref int dustType, ref int dustType2, ref float dustScale, ref float dustScale2)
		{
			
		}

		public virtual void BombOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			
		}

		public virtual void BombOnHitPlayer(Player target, Player.HurtInfo info)
		{
			
		}

		public override IGeneratesModItem Clone(GeneratedModItem newGeneratedModItem)
		{
			ModMorphBallBomb inst = (ModMorphBallBomb)MemberwiseClone();
			inst.GeneratedModItem = newGeneratedModItem;
			return inst;
		}

	}
}
