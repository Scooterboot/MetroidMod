
using System;
using MetroidMod.Common.Systems;
using MetroidMod.Content.DamageClasses;
using MetroidMod.Content.Projectiles;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MetroidMod.Content.MorphBallAddons
{
	// TODO: write powerbombexplosion
	public interface IMorphBallPowerBombAddon
	{
		Mod Mod { get; }
		string Name { get; }
		SoundStyle? ExplosionSound { get; }

		MorphBallPowerBombProjectile PowerBombProjectile { get; }
		string PowerBombProjectileTexture { get; }

		MorphBallPowerBombExplosionProjectile PowerBombExplosionProjectile { get; }
		string PowerBombExplosionProjectileTexture { get; }

		/// <summary>
		/// Drawing field for the Explosion projetile. Return null to trigger default code; return false to draw your own; return true to let vanilla handle it.
		/// </summary>
		bool? ExplosionDraw(ref Color lightColor);

		/// <inheritdoc cref="MorphBallPowerBombExplosionProjectile.ProjectileSetDefaults" />
		void ExplosionDefaults();

		/// <summary>
		/// AI field for the Explosion projectile. Return false to trigger default code.
		/// </summary>
		bool ExplosionLogic();
	}

	[Autoload(false)]
	public class MorphBallPowerBombProjectile : IGeneratesModProjectile
	{
		public IMorphBallPowerBombAddon producer;

		public string Name => producer.Name + "PowerBomb";

		public GeneratedModProjectile GeneratedModProjectile { get; internal set; }

		public int ProjectileType { get; internal set; }

		public Projectile Projectile => GeneratedModProjectile.Projectile;

		public LocalizedText ProjectileDisplayName => producer.Mod.GetLocalization($"MorphBallAddons.{producer.Name}.PowerBomb.DisplayName");

		public string ProjectileTexture => producer.PowerBombProjectileTexture;

		public SoundStyle? ExplosionSound => producer.ExplosionSound;
		
		public MorphBallPowerBombProjectile(IMorphBallPowerBombAddon producer)
		{
			this.producer = producer;
		}

		public virtual void ProjectileAI()
		{
			float scalez = 0.2f;
			Lighting.AddLight(Projectile.Center, scalez, scalez, scalez);

			if (Projectile.frameCounter++ >= (int)(Projectile.timeLeft / 7.5f))
			{
				Projectile.frame = (Projectile.frame + 1) % 6;
				Projectile.frameCounter = 0;
			}
		}

		public virtual bool? ProjectileCanCutTiles()
		{
			return null;
		}

		public virtual bool? ProjectileCanDamage()
		{
			return false;
		}

		public virtual void ProjectileCutTiles()
		{
			
		}

		public virtual void ProjectileModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			
		}

		public virtual void ProjectileModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
		{
			
		}

		public void ProjectileOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			
		}

		public void ProjectileOnHitPlayer(Player target, Player.HurtInfo info)
		{
			
		}

		public virtual void ProjectileOnKill(int timeLeft)
		{
			SoundEngine.PlaySound(producer.ExplosionSound, Projectile.position);
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0, 0, producer.PowerBombExplosionProjectile.ProjectileType, Projectile.damage, Projectile.knockBack, Projectile.owner);
		}

		public virtual bool ProjectileOnTileCollide(Vector2 oldVelocity)
		{
			return true;
		}

		public virtual bool ProjectilePreDraw(ref Color lightColor)
		{
			return true;
		}

		public virtual void ProjectileSetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 60;//138;
			Projectile.ownerHitCheck = true;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.tileCollide = true;
			Projectile.penetrate = 1;
			Projectile.ignoreWater = true;
			Projectile.DamageType = ModContent.GetInstance<HunterDamageClass>();
			Projectile.light = 0.2f;
		}

		public virtual void ProjectileSetStaticDefaults()
		{
			Main.projFrames[ProjectileType] = 6;
		}

		public IGeneratesModProjectile Clone(GeneratedModProjectile newGeneratedModProjectile)
		{
			MorphBallPowerBombProjectile inst = (MorphBallPowerBombProjectile)MemberwiseClone();
			inst.GeneratedModProjectile = newGeneratedModProjectile;
			return inst;
		}
	}

	[Autoload(false)]
	public class MorphBallPowerBombExplosionProjectile : IGeneratesModProjectile
	{
		public IMorphBallPowerBombAddon producer;

		public string Name => producer.Name	+ "PowerBombExplosion";

		public GeneratedModProjectile GeneratedModProjectile { get; internal set; }

		public int ProjectileType { get; internal set; }

		public Projectile Projectile => GeneratedModProjectile.Projectile;

		public LocalizedText ProjectileDisplayName => producer.Mod.GetLocalization($"MorphBallAddons.{producer.Name}.PowerBombExplosion.DisplayName");

		public string ProjectileTexture => producer.PowerBombExplosionProjectileTexture;
		
		public MorphBallPowerBombExplosionProjectile(IMorphBallPowerBombAddon producer)
		{
			this.producer = producer;
		}

		private float scaleSize = 1f;
		private Color colory = Color.Gold;
		private const int width = 1000;
		private const int height = 750;
		private const int maxDistance = 55;
		public virtual void ProjectileAI()
		{
			if (producer.ExplosionLogic())
				return;
			
			// producer told us to run our own code by returning false, RATTLE EM
			float speed = 2f;
			colory = Color.Yellow; Projectile.timeLeft = 60;
			Projectile.frameCounter++;

			if (Projectile.frameCounter < maxDistance)
			{
				scaleSize += speed;

				int num = (int)(50f * Projectile.scale);
				for (int i = 0; i < num; i++)
				{
					float angle = (float)(Math.PI * 2 / num) * i;
					Vector2 position = Projectile.Center - new Vector2(10, 10);
					position.X += (float)Math.Cos(angle) * (Projectile.width / 2f);
					position.Y += (float)Math.Sin(angle) * (Projectile.height / 2f);
					int num20 = Dust.NewDust(position, 20, 20, 57, 0f, 0f, 100, default(Color), 3f);
					Dust dust = Main.dust[num20];
					dust.velocity += Vector2.Normalize(Projectile.Center - dust.position) * 5f * Projectile.scale;
					dust.noGravity = true;
				}
			}
			else
			{
				scaleSize -= speed;
				colory = Color.Black;
				Projectile.damage = 0;
				for (int i = 0; i < Main.item.Length; i++)
				{
					if (!Main.item[i].active) continue;

					Item I = Main.item[i];
					if (Projectile.Hitbox.Intersects(I.Hitbox))
					{
						Vector2 center = new Vector2(Projectile.Center.X, Projectile.Center.Y - (I.height / 2f));
						Vector2 velocity = Vector2.Normalize(center - I.Center) * Math.Min(20f, Vector2.Distance(center, I.Center));
						if (Vector2.Distance(center, I.Center) > 1f)
						{
							I.position += velocity;
							I.velocity *= 0f;
						}
					}
				}
			}
			if (Projectile.frameCounter >= (maxDistance * 2))
			{
				scaleSize = 1f;
				colory = Color.Gold;
				Projectile.frameCounter = 0;
				Projectile.Kill();
			}

			Projectile.scale = scaleSize * 0.02f;
			Projectile.Resize((int)Math.Floor(width * Projectile.scale), (int)Math.Floor(height * Projectile.scale));
			Projectile.netUpdate = true;

			if (Projectile.frameCounter == maxDistance)
			{
				Rectangle tileRect = new Rectangle((int)(Projectile.position.X / 16), (int)(Projectile.position.Y / 16), Projectile.width / 16, Projectile.height / 16);
				for (int x = tileRect.X; x < tileRect.X + tileRect.Width; x++)
				{
					for (int y = tileRect.Y; y < tileRect.Y + tileRect.Height; y++)
					{
						if (MSystem.mBlockType[x, y] != BreakableTileID.None)
						{
							MSystem.hit[x, y] = true;
						}
						if (MSystem.mBlockType[x, y] == BreakableTileID.Bomb)
						{
							MSystem.AddRegenBlock(x, y);
						}
						if (MSystem.mBlockType[x, y] == BreakableTileID.Fake)
						{
							MSystem.AddRegenBlock(x, y);
						}
						if (MSystem.mBlockType[x, y] == BreakableTileID.PowerBomb)
						{
							MSystem.AddRegenBlock(x, y);
						}
						if (MSystem.mBlockType[x, y] == BreakableTileID.FakeHint)
						{
							MSystem.AddRegenBlock(x, y);
						}
					}
				}
			}
		}

		public virtual bool? ProjectileCanCutTiles()
		{
			return true;
		}

		public virtual bool? ProjectileCanDamage()
		{
			return true;
		}

		public virtual void ProjectileCutTiles()
		{
			
		}

		public void ProjectileModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			modifiers.ArmorPenetration += 999;
		}

		public virtual void ProjectileModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
		{
			
		}

		public void ProjectileOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			
		}

		public void ProjectileOnHitPlayer(Player target, Player.HurtInfo info)
		{
			
		}

		public virtual void ProjectileOnKill(int timeLeft)
		{
			
		}

		public bool ProjectileOnTileCollide(Vector2 oldVelocity)
		{
			return false;
		}

		public bool ProjectilePreDraw(ref Color lightColor)
		{
			bool? value = producer.ExplosionDraw(ref lightColor);
			if (value != null)
				return (bool)value;
			// producer gave us null, so here's the default code to run.
			Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[ProjectileType].Value;
			Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), colory, Projectile.rotation, new Vector2(tex.Width / 2, tex.Height / 2), Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}

		public void ProjectileSetDefaults()
		{
			Projectile.width = 1000;
			Projectile.height = 750;
			Projectile.scale = 0.02f;
			Projectile.localNPCHitCooldown = 1;
			Projectile.timeLeft = 200;
			Projectile.penetrate = -1;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.usesLocalNPCImmunity = true;
			producer.ExplosionDefaults();
		}

		public void ProjectileSetStaticDefaults()
		{
			
		}

		public IGeneratesModProjectile Clone(GeneratedModProjectile newGeneratedModProjectile)
		{
			MorphBallPowerBombExplosionProjectile inst = (MorphBallPowerBombExplosionProjectile)MemberwiseClone();
			inst.GeneratedModProjectile = newGeneratedModProjectile;
			return inst;
		}
	}

	public abstract class ModMorphBallSpecial : ModMorphBallAddon, IMorphBallPowerBombAddon
	{
		public virtual SoundStyle? ExplosionSound => null;

		public MorphBallPowerBombProjectile PowerBombProjectile { get; internal set; }

		public virtual string PowerBombProjectileTexture => TexturePath + "_PowerBomb";

		public MorphBallPowerBombExplosionProjectile PowerBombExplosionProjectile { get; internal set; }

		public virtual string PowerBombExplosionProjectileTexture => TexturePath + "_PowerBombExplosion";

		public override void Load()
		{
			base.Load();
			PowerBombProjectile = new MorphBallPowerBombProjectile(this);
			PowerBombExplosionProjectile = new MorphBallPowerBombExplosionProjectile(this);
			PowerBombProjectile.GeneratedModProjectile = new GeneratedModProjectile(PowerBombProjectile);
			PowerBombExplosionProjectile.GeneratedModProjectile = new GeneratedModProjectile(PowerBombExplosionProjectile);
			Mod.AddContent(PowerBombProjectile.GeneratedModProjectile);
			Mod.AddContent(PowerBombExplosionProjectile.GeneratedModProjectile);
			PowerBombProjectile.ProjectileType = PowerBombProjectile.Projectile.type;
			PowerBombExplosionProjectile.ProjectileType = PowerBombExplosionProjectile.Projectile.type;
		}

		public virtual void ExplosionDefaults()
		{
			
		}

		public virtual bool? ExplosionDraw(ref Color lightColor)
		{
			return null;
		}

		public virtual bool ExplosionLogic()
		{
			return false;
		}
	}
}