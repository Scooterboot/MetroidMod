using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.NPCs.GoldenTorizo
{
	public class OrbBomb_Golden : ModNPC
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Golden Torizo Orb");
			NPCID.Sets.MPAllowedEnemies[Type] = true;

			NPCID.Sets.SpecificDebuffImmunity[Type][20] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][24] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][31] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][39] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][44] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][ModContent.BuffType<Buffs.PhazonDebuff>()] = true;
		}
		public override void SetDefaults()
		{
			NPC.width = 26;
			NPC.height = 26;
			NPC.damage = 0;
			NPC.defense = 0;
			NPC.lifeMax = 5;
			NPC.knockBackResist = 0;
			NPC.HitSound = SoundID.NPCHit3;
			NPC.DeathSound = SoundID.NPCDeath3;
			NPC.value = 0;
			NPC.lavaImmune = true;
			NPC.behindTiles = true;
			NPC.aiStyle = -1;
			NPC.npcSlots = 0;

			//NPC.dontTakeDamage = true;
			//NPC.noTileCollide = true;
			NPC.noGravity = true;
		}

		int damage = 120;//30;//60;
		public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: bossLifeScale -> balance (bossAdjustment is different, see the docs for details) */
		{
			damage *= 2;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemID.Heart));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Miscellaneous.EnergyPickup>(), 1, 10, 25));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Miscellaneous.MissilePickup>(), 1, 5, 25));
		}

		public override void AI()
		{
			if (NPC.ai[0] >= 1)
			{
				if (NPC.velocity.X == 0 || NPC.velocity.Y == 0)
				{
					NPC.damage = damage;
					NPC.velocity *= 0f;

					NPC.position.X += NPC.width / 2f;
					NPC.position.Y += NPC.height;
					NPC.width = 30;
					NPC.height = 60;
					NPC.position.X -= NPC.width / 2f;
					NPC.position.Y -= NPC.height;

					if (NPC.ai[0] == 1)
					{
						for (int i = 0; i < 25; i++)
						{
							int newDust = Dust.NewDust(new Vector2(NPC.position.X + 2, NPC.position.Y + NPC.height - 4), NPC.width - 2, 8, 55, 0f, 0f, 100, default(Color), 2f);
							Main.dust[newDust].velocity *= 0.5f;
							Main.dust[newDust].velocity.Y -= 4f;
							Main.dust[newDust].noGravity = true;

							newDust = Dust.NewDust(new Vector2(NPC.position.X + 2, NPC.position.Y + NPC.height - 4), NPC.width - 2, 8, 30, 0f, 0f, 100, default(Color), 2f);
							Main.dust[newDust].velocity *= 0.5f;
							Main.dust[newDust].velocity.Y -= 4f;
							Main.dust[newDust].noGravity = true;

							newDust = Dust.NewDust(new Vector2(NPC.position.X + 4, NPC.position.Y + NPC.height - 10), NPC.width - 4, 10, 55, 0f, 0f, 100, default(Color), 0.5f);
							Main.dust[newDust].velocity.X *= 0.5f;
							Main.dust[newDust].velocity.Y = -Main.rand.Next(30) * 0.1f;
							Main.dust[newDust].noGravity = true;
						}
					}
					Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14, NPC.Center);

					NPC.ai[0]++;
				}
				else
				{
					NPC.velocity.Y += 0.1f;
				}
				if (NPC.ai[0] > 2)
				{
					NPC.life = 0;
					NPC.HitEffect(0, 10.0);
					NPC.active = false;
				}
			}
			else
			{
				NPC.ai[0] = 1;
			}
		}
		public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode == NetmodeID.Server) { return; }
			for (int i = 0; i < 15; i++)
			{
				int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, 30, 0f, -(Main.rand.Next(4) / 2), 100, Color.White, 1.5f);
				Main.dust[dust].noGravity = true;
			}
			if (NPC.life <= 0)
			{
				for (int i = 0; i < 10; i++)
				{
					int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, 55, 0f, -(Main.rand.Next(3) / 2), 100, Color.White, 2f);
					Main.dust[dust].noGravity = true;
				}
			}
		}
	}
}
