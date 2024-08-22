using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.NPCs.Kraid
{
	public class Kraid_ArmFront : ModNPC
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Kraid");
			Main.npcFrameCount[NPC.type] = 5;
			NPCID.Sets.MPAllowedEnemies[Type] = true;
			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
			{
				Hide = true // ExampleMod says: "Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry."
			};              // y'know, like this oaf
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
		}

		Vector2 swipeVec = Vector2.Zero;
		Vector2[] swipeDestVec = new Vector2[9];
		float swipeFrame = 0f;

		public override void SetDefaults()
		{
			NPC.width = 2;
			NPC.height = 2;
			NPC.scale = 1f;
			NPC.damage = 0;
			NPC.defense = 500;
			NPC.lifeMax = 1000;
			NPC.dontTakeDamage = true;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath5;
			NPC.noGravity = true;
			NPC.value = Item.buyPrice(0, 0, 4, 60);
			NPC.knockBackResist = 0;
			NPC.lavaImmune = true;
			NPC.noTileCollide = true;
			NPC.behindTiles = true;
			NPC.frameCounter = 0;
			NPC.aiStyle = -1;
			NPC.npcSlots = 1;
			NPC.boss = true;

			swipeDestVec[0] = new Vector2(-20f, -2f);
			swipeDestVec[1] = new Vector2(-50f, -6f);
			swipeDestVec[2] = new Vector2(-78f, -42f);
			swipeDestVec[3] = new Vector2(-70f, -116f);
			swipeDestVec[4] = new Vector2(0f, -100f);
			swipeDestVec[5] = new Vector2(36f, -54f);
			swipeDestVec[6] = new Vector2(42f, 0f);
			swipeDestVec[7] = new Vector2(6f, 22f);
			swipeDestVec[8] = Vector2.Zero;
		}
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			int associatedNPCType = ModContent.NPCType<Kraid_Head>();
			bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[associatedNPCType], quickUnlock: true);
			// Pretty sure we still need to have this method thing so the Scan Visor counts all of it as the one NPC
			/*bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>  //All this is to add page info, not necessary but kept it here just in case    -Z
			{
				new MoonLordPortraitBackgroundProviderBestiaryInfoElement(), // Plain black background
				new FlavorTextBestiaryInfoElement("This invasive species made its way on this planet after the Gizzard tribe had brought it to the Terrarian Planet to train young warriors. It is extremely bulky and slow, but can shoot projectiles from its stomach. It's hide is almost impenetrable save for even the hottest lava. But these creatures are not indestructible on the inside. Give it a taste of pain when the mouth opens!")
			});*/
		}

		int fArmAnim = 1;
		int num = 1;
		float anim = 0;
		Vector2 animVec = new Vector2(14f, -8f);

		int num2 = 220;

		int state = 0;
		public override void AI()
		{
			NPC Head = Main.npc[(int)NPC.ai[0]];
			bool despawn = (Head.ai[3] > 1);
			if (!Head.active)
			{
				NPC.life = 0;
				NPC.active = false;
				if (!despawn)
				{
					NPC.HitEffect(0, 10.0);
				}
				return;
			}

			state = 0;
			if (Head.life < (int)(Head.lifeMax * 0.75f))
			{
				state = 1;
			}
			if (Head.life < (int)(Head.lifeMax * 0.5f))
			{
				state = 2;
			}
			if (Head.life < (int)(Head.lifeMax * 0.25f))
			{
				state = 3;
			}

			NPC.TargetClosest(true);
			Player player = Main.player[NPC.target];
			if (!player.dead)
			{
				NPC.timeLeft = 60;
			}

			NPC.ai[1]++;
			if (NPC.ai[1] >= num2)
			{
				if (NPC.ai[1] <= num2 + 40)
				{
					swipeFrame = Math.Min(swipeFrame + 0.125f, 4f);
				}
				else if (NPC.ai[1] <= num2 + 75)
				{
					swipeFrame = Math.Min(swipeFrame + 0.25f, 9f);
				}
				swipeVec = LerpArray(Vector2.Zero, swipeDestVec, swipeFrame);
				if (NPC.ai[1] >= num2 + 80)
				{
					swipeFrame = 0f;
					swipeVec = Vector2.Zero;
					NPC.ai[1] = 0;
				}
			}
			else
			{
				num2 = 220;
				num2 -= 30 * state;
			}
			NPC.rotation = -(((float)Math.PI / 4) * (Math.Max((swipeVec.X + swipeVec.Y) * -1, 0) / 194));

			if (swipeFrame == 6f)
			{
				Terraria.Audio.SoundEngine.PlaySound(Sounds.NPCs.KraidSwipe, NPC.Center);
				Vector2 clawPos = NPC.Center + new Vector2(48 * Head.direction, -36);
				float trot = (float)Math.Atan2(player.Center.Y - clawPos.Y, player.Center.X - clawPos.X);
				float speed = 4f;
				Vector2 clawVel = new Vector2((float)Math.Cos(trot) * speed, (float)Math.Sin(trot) * speed);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					var entitySource = NPC.GetSource_FromAI();
					for (int i = 0; i < 2; i++)
					{
						int spread = 15;
						float spreadMult = 0.05f;
						float vX = clawVel.X + Main.rand.Next(-spread, spread + 1) * spreadMult;
						float vY = clawVel.Y + Main.rand.Next(-spread, spread + 1) * spreadMult;

						int c = NPC.NewNPC(entitySource, (int)clawPos.X, (int)clawPos.Y, ModContent.NPCType<KraidClaw>(), NPC.whoAmI);
						Main.npc[c].position.Y += (float)Main.npc[c].height / 2;
						Main.npc[c].velocity = new Vector2(vX, vY);
						Main.npc[c].direction = Head.direction;
						Main.npc[c].netUpdate = true;
					}
				}
			}

			NPC.frameCounter += 1;
			if (NPC.frameCounter >= 10)
			{
				NPC.frame.Y += fArmAnim;
				if (NPC.frame.Y >= 4 || NPC.frame.Y <= 0)
				{
					fArmAnim *= -1;
				}
				NPC.frameCounter = 0;
			}

			if (NPC.frame.Y >= 4)
			{
				num = -1;
			}
			if (NPC.frame.Y <= 0)
			{
				num = 1;
			}
			anim = (1f / 4) * NPC.frame.Y + ((float)NPC.frameCounter / 40) * num;
			anim = MathHelper.Clamp(anim, 0f, 1f);

			Vector2 vec = Vector2.Lerp(Vector2.Zero, new Vector2(animVec.X * Head.direction, animVec.Y), anim);
			NPC.Center = Head.Center + new Vector2(42 * Head.direction, 131) + vec + new Vector2(swipeVec.X * Head.direction, swipeVec.Y);
			NPC.velocity *= 0f;
		}
		/*public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode != 2)
			{
				if (NPC.life <= 0)
				{
					for (int m = 0; m < 20; m++)
					{
						int dustID = Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, NPC.velocity.X * 0.2f, NPC.velocity.Y * 0.2f, 100, Color.White, NPC.life <= 0 && m % 2 == 0 ? 3f : 1f);
						if (m % 2 == 0)
						{
							Main.dust[dustID].noGravity = true;
						}
					}
					int gore = Gore.NewGore(NPC.position,NPC.velocity,mod.GetGoreSlot("Gores/KraidGore4"),1f);
					Main.gore[gore].timeLeft = 60;
					gore = Gore.NewGore(NPC.position,NPC.velocity,mod.GetGoreSlot("Gores/KraidGore5"),1f);
					Main.gore[gore].timeLeft = 60;
					int gore = Gore.NewGore(NPC.position,NPC.velocity,mod.GetGoreSlot("Gores/KraidGore6"),1f);
					Main.gore[gore].timeLeft = 30;
					Main.PlaySound(4,(int)NPC.position.X,(int)NPC.position.Y,1);
					
					for (int num70 = 0; num70 < 15; num70++)
					{
						int num71 = Dust.NewDust(new Vector2(NPC.position.X-10,NPC.position.Y-10), NPC.width+10, NPC.height+10, 6, 0f, 0f, 100, default(Color), 5f);
						Main.dust[num71].velocity *= 1.4f;
						Main.dust[num71].noGravity = true;
						int num72 = Dust.NewDust(new Vector2(NPC.position.X-10,NPC.position.Y-10), NPC.width+10, NPC.height+10, 30, 0f, 0f, 100, default(Color), 3f);
						Main.dust[num72].velocity *= 1.4f;
						Main.dust[num72].noGravity = true;
					}
					Main.PlaySound(2,(int)NPC.position.X,(int)NPC.position.Y,14);
				}
			}
		}*/
		public override bool PreDraw(SpriteBatch sb, Vector2 screenPos, Color drawColor)
		{
			return false;
		}

		public static Vector2 LerpArray(Vector2 value1, Vector2[] value2, float amount)
		{
			Vector2 result = value1;
			for (int i = 0; i < value2.Length; i++)
			{
				if ((i + 1) >= amount)
				{
					Vector2 firstValue = value1;
					Vector2 secondValue = value2[i];
					if (i > 0)
					{
						firstValue = value2[i - 1];
					}
					float amt = amount - i;
					result = firstValue + (secondValue - firstValue) * amt;
					break;
				}
			}
			return result;
		}
	}
}
