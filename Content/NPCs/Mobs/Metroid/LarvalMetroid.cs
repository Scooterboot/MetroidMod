using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace MetroidModPorted.Content.NPCs.Mobs.Metroid
{
	public class LarvalMetroid : MNPC
	{
		private float newScale = -1;
		public float movingSpeed = 0;
		public bool movingUp = false;
		public bool grappled = false;
		public bool frozen = false;
		public bool spawn = false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Larval Metroid");
			Main.npcFrameCount[Type] = 4;
		}
		public override void SetDefaults()
		{
			NPC.width = 38;
			NPC.height = 38;
			NPC.damage = 20;
			NPC.defense = 23;
			NPC.lifeMax = 100;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.noGravity = true;
			NPC.value = Item.buyPrice(0, 0, 1, 60);
			NPC.knockBackResist = 0.75f;
			NPC.aiStyle = -1;
			NPC.npcSlots = 1;
			//banner = npc.type;
			//bannerItem = mod.ItemType("MetroidBanner");
			NPC.buffImmune[BuffID.Poisoned] = true;
			NPC.buffImmune[BuffID.OnFire] = true;
			NPC.buffImmune[BuffID.CursedInferno] = true;

			/* NPC scale networking fix. */
			if (Main.rand != null && Main.netMode != NetmodeID.MultiplayerClient)
				newScale = (Main.rand.Next(5, 10) * 0.1f);
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if(Main.hardMode || NPC.downedBoss2)
			{
				float chance1 = 0.03f;
				float chance2 = 0.5f;
				if(Main.hardMode)
				{
					chance1 = 0.5f;
					chance2 = 0.75f;
				}
				return (SpawnCondition.Corruption.Chance + SpawnCondition.Crimson.Chance)*chance1 + SpawnCondition.DungeonNormal.Chance*chance2;
			}
			return SpawnCondition.DungeonNormal.Chance*0.5f;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			// We can use AddRange instead of calling Add multiple times in order to add multiple items at once
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("Latching onto enemies to drain energy is what the Larval Metroid does best.")
			});
		}

		public override bool PreAI()
		{
			if (!spawn && newScale != -1)
			{
				SetStats();
				spawn = true;
				NPC.netUpdate = true;
			}
			return true;
		}
		public override void AI()
		{
			frozen = NPC.GetGlobalNPC<Common.GlobalNPCs.MGlobalNPC>().froze;
			if (grappled)
			{
				if (Main.player[NPC.target].dead || !Main.player[NPC.target].active || frozen)
				{
					grappled = false;
					return;
				}
				NPC.rotation = 0;
				NPC.position = new Vector2(Main.player[NPC.target].Center.X - (NPC.width / 2), Main.player[NPC.target].Center.Y - (NPC.height / 2) - 16);
				Main.player[NPC.target].velocity.X *= 0.95f;
			}
			else if (!frozen)
			{
				NPC.TargetClosest();

				if (Main.player[NPC.target].Center.X < NPC.Center.X)
				{
					if (NPC.velocity.X > -2) { NPC.velocity.X -= 0.2f; }
				}
				else if (Main.player[NPC.target].Center.X > NPC.Center.X)
				{
					if (NPC.velocity.X < 2) { NPC.velocity.X += 0.2f; }
				}
				if (Main.player[NPC.target].Center.Y < NPC.Center.Y)
				{
					if (NPC.velocity.Y > -2) NPC.velocity.Y -= 0.2f;
				}
				else if (Main.player[NPC.target].Center.Y > NPC.Center.Y)
				{
					if (NPC.velocity.Y < 2) NPC.velocity.Y += 0.2f;
				}

				if (movingUp)
				{
					movingSpeed -= 0.02f;
				}
				else
				{
					movingSpeed += 0.02f;
				}
				if (movingSpeed <= -0.20f)
				{
					movingUp = false;
				}
				if (movingSpeed >= 0.20f)
				{
					movingUp = true;
				}
				NPC.velocity.Y += movingSpeed;

				Vector2 vector = NPC.velocity;
				NPC.velocity = Collision.TileCollision(NPC.position, NPC.velocity, NPC.width, NPC.height, false, false);
				if (NPC.velocity.X != vector.X)
				{
					NPC.velocity.X = -vector.X;
				}
				if (NPC.velocity.Y != vector.Y)
				{
					NPC.velocity.Y = -vector.Y;
				}

				Player player = Main.player[NPC.target];
				if (Vector2.Distance(NPC.Center, player.Center) <= 25f)
				{
					grappled = true;
				}
				NPC.noGravity = true;
			}
			if (frozen)
			{
				NPC.damage = 0;
				NPC.frame.Y = 0;
				NPC.noGravity = false;
				NPC.rotation += NPC.velocity.X * 0.1f;
				if (NPC.velocity.Y == 0f)
				{
					NPC.velocity.X = NPC.velocity.X * 0.98f;
					if ((double)NPC.velocity.X > -0.01 && (double)NPC.velocity.X < 0.01)
					{
						NPC.velocity.X = 0f;
					}
				}
			}

			if (Main.netMode == NetmodeID.Server && NPC.whoAmI < 200)
			{
				NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, NPC.whoAmI, 0f, 0f, 0f, 0);
			}
		}

		/*public override void OnHitPlayer(Player player, int damage, bool crit)
		{
			if(grappled)
			{
				hitDir = 0;
				player.knockbackResist = 0f;
			}
		}*/
		public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
		{
			if (frozen && damage >= 20)
			{
				damage = (int)((double)(damage * (2 - (double)NPC.scale)) + (double)NPC.defense * 0.5);
			}
			return true;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0 && Main.netMode != NetmodeID.Server)
			{
				Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("MetroidGore1").Type, NPC.scale);
				Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("MetroidGore1").Type, NPC.scale);
				Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("MetroidGore2").Type, NPC.scale);
				Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("MetroidGore2").Type, NPC.scale);
			}
		}
		public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
		{
			if (projectile.type == ((ModMBWeapon)MBAddonLoader.GetAddon<MorphBallAddons.Bomb>()).ProjectileType || projectile.type == ((ModMBSpecial)MBAddonLoader.GetAddon<MorphBallAddons.PowerBomb>()).ProjectileType)
			{
				grappled = false;
			}
		}
		public override void FindFrame(int frameHeight)
		{
			int num = 1;
			if (!Main.dedServ)
			{
				num = Terraria.GameContent.TextureAssets.Npc[Type].Value.Height / Main.npcFrameCount[Type];//Main.npcTexture[Type].Height / Main.npcFrameCount[NPC.type];
			}
			if (!frozen)
			{
				if (!grappled) NPC.rotation = NPC.velocity.X * 0.1f;
				NPC.frameCounter += 1.0;
				if (NPC.frameCounter >= 10.0)
				{
					NPC.frame.Y = NPC.frame.Y + num;
					NPC.frameCounter = 0.0;
				}
				if (NPC.frame.Y >= num * Main.npcFrameCount[Type])
				{
					NPC.frame.Y = 0;
				}
			}
			else
			{
				NPC.frame.Y = num;
			}
		}

		private void SetStats()
		{
			NPC.scale = newScale;
			NPC.defense = NPC.defDefense = (int)(NPC.defense * NPC.scale);
			NPC.damage = NPC.defDamage = (int)(NPC.damage * NPC.scale);
			NPC.life = NPC.lifeMax = (int)(NPC.life * NPC.scale);
			NPC.value = ((int)(NPC.value * NPC.scale));
			NPC.npcSlots *= NPC.scale;
			NPC.knockBackResist *= 2f - NPC.scale;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write((double)newScale);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			newScale = (float)reader.ReadDouble();
		}
	}
}
