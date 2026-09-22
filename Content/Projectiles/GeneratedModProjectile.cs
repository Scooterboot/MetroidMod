
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles
{
	// This is half-advanced, see /Documentation/AutomaticContentGeneration.md for a wordy explanation. - Armipotent
	/// <summary>
	/// A standard for generating and interacting with <see cref="Terraria.ModLoader.ModProjectile">s.
	/// </summary>
	public interface IGeneratesModProjectile
	{
		string Name { get; }

		GeneratedModProjectile GeneratedModProjectile { get; }

		int ProjectileType { get; }

		LocalizedText ProjectileDisplayName { get; }

		string ProjectileTexture { get; }

		/// <inheritdoc cref="GeneratedModProjectile.AI" />
		void ProjectileAI();

		/// <inheritdoc cref="GeneratedModProjectile.CanCutTiles" />
		bool? ProjectileCanCutTiles();

		/// <inheritdoc cref="GeneratedModProjectile.CanDamage" />
		bool? ProjectileCanDamage();

		/// <inheritdoc cref="GeneratedModProjectile.CutTiles" />
		void ProjectileCutTiles();

		/// <inheritdoc cref="GeneratedModProjectile.ModifyHitNPC(NPC, ref NPC.HitModifiers)" />
		void ProjectileModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers);

		/// <inheritdoc cref="GeneratedModProjectile.ModifyHitPlayer(Player, ref Player.HurtModifiers)" />
		void ProjectileModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers);

		/// <inheritdoc cref="GeneratedModProjectile.OnHitNPC(NPC, NPC.HitInfo, int)">
		void ProjectileOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone);

		/// <inheritdoc cref="GeneratedModProjectile.OnHitPlayer(Player, Player.HurtInfo)">
		void ProjectileOnHitPlayer(Player target, Player.HurtInfo info);

		/// <inheritdoc cref="GeneratedModProjectile.OnKill(int)" />
		void ProjectileOnKill(int timeLeft);

		/// <inheritdoc cref="GeneratedModProjectile.OnTileCollide(Vector2)" />
		bool ProjectileOnTileCollide(Vector2 oldVelocity);

		/// <inheritdoc cref="GeneratedModProjectile.PreDraw(ref Color)" />
		bool ProjectilePreDraw(ref Color lightColor);

		/// <inheritdoc cref="GeneratedModProjectile.SetDefaults" />
		void ProjectileSetDefaults();

		/// <inheritdoc cref="GeneratedModProjectile.SetStaticDefaults" />
		void ProjectileSetStaticDefaults();

		IGeneratesModProjectile Clone(GeneratedModProjectile newGeneratedModProjectile);
	}

	/// <summary>
	/// An automatically generated ModProjectile. See <see cref="IGeneratesModProjectile"/>.
	/// </summary>
	[Autoload(false)]
	public class GeneratedModProjectile : ModProjectile
	{
		public IGeneratesModProjectile producer;


		public override string Name => producer.Name + "Projectile";

		public override LocalizedText DisplayName => producer.ProjectileDisplayName;

		public override string Texture => producer.ProjectileTexture;


		public GeneratedModProjectile(IGeneratesModProjectile producer)
		{
			this.producer = producer;
		}

		public override ModProjectile Clone(Projectile newEntity)
		{
			GeneratedModProjectile obj = (GeneratedModProjectile)base.Clone(newEntity);
			obj.producer = producer.Clone(obj);
			return obj;
		}

		public override ModProjectile NewInstance(Projectile entity)
		{
			ModProjectile inst = Clone(entity);
			return inst;
		}


		public override void AI()
		{
			producer.ProjectileAI();
		}

		public override bool? CanCutTiles()
		{
			return producer.ProjectileCanCutTiles();
		}

		public override bool? CanDamage()
		{
			return producer.ProjectileCanDamage();
		}


		public override void CutTiles()
		{
			producer.ProjectileCutTiles();
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			producer.ProjectileModifyHitNPC(target, ref modifiers);
		}

		public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
		{
			producer.ProjectileModifyHitPlayer(target, ref modifiers);
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			producer.ProjectileOnHitNPC(target, hit, damageDone);
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			producer.ProjectileOnHitPlayer(target, info);
		}

		public override void OnKill(int timeLeft)
		{
			producer.ProjectileOnKill(timeLeft);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			return producer.ProjectilePreDraw(ref lightColor);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return producer.ProjectileOnTileCollide(oldVelocity);
		}


		public override void SetDefaults()
		{
			producer.ProjectileSetDefaults();
		}

		public override void SetStaticDefaults()
		{
			producer.ProjectileSetStaticDefaults();
		}

	}
}
