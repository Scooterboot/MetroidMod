using Microsoft.Xna.Framework;
using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using static Terraria.ModLoader.ModContent;

namespace MetroidModPorted.Content.NPCs.Serris
{
	public abstract class Serris : ModNPC
	{
		int bodyLength = 10;

		protected float defSpeed = 8f;
		protected float defTurnSpeed = .07f;

		private bool isTileGround(int x, int y) => Main.tile[x, y] != null && (Main.tile[x, y].IsActuated && (Main.tileSolid[Main.tile[x, y].TileType] || Main.tileSolidTop[Main.tile[x, y].TileType] && Main.tile[x, y].TileFrameY == 0) || Main.tile[x, y].LiquidAmount > 64);
		
		protected void Update_Worm(bool head = false)
		{
			if (!head && NPC.timeLeft < 300)
				NPC.timeLeft = 300;
			if (NPC.target < 0 || NPC.target > 255 || Main.player[NPC.target].dead)
				NPC.TargetClosest();
			if (Main.player[NPC.target].dead && NPC.timeLeft > 300)
				NPC.timeLeft = 300;

			// Spawn the rest of Serris's body.
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				if (NPC.ai[0] == 0 && head)
				{
					int prev = NPC.whoAmI;
					NPC.realLife = NPC.whoAmI;
					var entitySource = NPC.GetSource_FromAI();
					for (int i = 0; i < bodyLength; i++)
					{
						int type = NPCType<Serris_Body>();
						if (i > 6)
							type = NPCType<Serris_Tail>();

						int srs = NPC.NewNPC(entitySource, (int)NPC.Center.X, (int)NPC.Center.Y, type, NPC.whoAmI);
						Main.npc[srs].ai[0] = srs;
						Main.npc[srs].ai[1] = prev;
						Main.npc[srs].ai[3] = NPC.whoAmI;
						Main.npc[srs].realLife = NPC.whoAmI;

						if (i > 7)
							Main.npc[srs].ai[2] = i - 7;
						if (prev != NPC.whoAmI)
							Main.npc[prev].ai[0] = srs;
						Main.npc[srs].netUpdate = true;
						prev = srs;
					}

					NPC.ai[0] = 1;
					NPC.netUpdate = true;
				}

				if (!NPC.active && Main.netMode == NetmodeID.Server)
					NetMessage.SendData(28, -1, -1, null, NPC.whoAmI, -1f);
			}

			int tileXMin = (int)MathHelper.Clamp((NPC.position.X / 16f) - 1, 0, Main.maxTilesX-1);
			int tileXMax = (int)MathHelper.Clamp(((NPC.position.X + (float)NPC.width) / 16f) + 2, 0, Main.maxTilesX-1);
			int tileYMin = (int)MathHelper.Clamp((NPC.position.Y / 16f) - 1, 0, Main.maxTilesY-1);
			int tileYMax = (int)MathHelper.Clamp(((NPC.position.Y + (float)NPC.height) / 16f) + 2, 0, Main.maxTilesY-1);
			
			Vector2 worldBounds = new Vector2(Main.maxTilesX * 16, Main.maxTilesY * 16);
			bool outOfBounds = (NPC.position.X + NPC.width < 0 || NPC.position.X > worldBounds.X + 16 || NPC.position.Y + NPC.height < 0 || NPC.position.Y > worldBounds.Y + 16);

			// Grounded check to see if the NPC is 'flying' or not.
			bool inGround = outOfBounds;
			for (int x = tileXMin; x < tileXMax; ++x)
			{
				for (int y = tileYMin; y < tileYMax; ++y)
				{
					if (isTileGround(x, y))
					{
						Vector2 worldPos = new Vector2(x * 16, y * 16);
						if (NPC.position.X + NPC.width > worldPos.X && NPC.position.X < worldPos.X + 16 && NPC.position.Y + NPC.height > worldPos.Y && NPC.position.Y < worldPos.Y + 16)
						{
							inGround = true;
							// 1% chance to generate a dust effect on a tile.
							if (Main.rand.NextBool(100) && NPC.behindTiles && Main.tile[x, y].IsActuated)
								WorldGen.KillTile(x, y, true, true, false);
						}
					}
				}
			}

			// Make sure the head of the NPC is flipped correctly vertically.
			NPC.spriteDirection = Math.Sign(NPC.velocity.X);

			float speed = defSpeed;
			float turnSpeed = defTurnSpeed;
			Vector2 npcCenter = new Vector2(NPC.position.X + NPC.width * .5f, NPC.position.Y + NPC.height * .5f);
			Vector2 targetDir = new Vector2(Main.player[NPC.target].position.X + Main.player[NPC.target].width * .5f, Main.player[NPC.target].position.Y + Main.player[NPC.target].height * .5f);

			// Round values down correctly to tile coordinates in worldspace and substract the current npc's position to get a directional vector.
			targetDir = ((targetDir / 16f) * 16) - ((npcCenter / 16f) * 16);

			float length = targetDir.Length();

			// If the current NPC is the head of Serris, it needs to follow the player.
			// Otherwise it needs to follow its target/previous body part.
			if (head)
			{
				// If Serris is in the air.
				if (!inGround)
				{
					NPC.TargetClosest();
					NPC.velocity.Y += .11f;
					if (NPC.velocity.Y > speed)
						NPC.velocity.Y = speed;

					if (Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y) < speed * .4f)
					{
						if (NPC.velocity.X < 0f)
							NPC.velocity.X -= turnSpeed * 1.1f;
						else
							NPC.velocity.X += turnSpeed * 1.1f;
					}
					else if (NPC.velocity.Y == speed)
						NPC.velocity.X += Math.Sign(NPC.velocity.X - targetDir.X) * turnSpeed * .9f;
					else if (NPC.velocity.Y > 4f)
					{
						if (NPC.velocity.X < 0f)
							NPC.velocity.X += turnSpeed * .9f;
						else
							NPC.velocity.X -= turnSpeed * .9f;
					}
				}
				else
				{
					if (NPC.behindTiles && NPC.soundDelay == 0)
					{
						float delay = length / 40f;
						delay = MathHelper.Clamp(delay, 10f, 20f);
						NPC.soundDelay = (int)delay;
						Terraria.Audio.SoundEngine.PlaySound(SoundID.Roar, NPC.position, 1);
					}

					float absX = Math.Abs(targetDir.X);
					float absY = Math.Abs(targetDir.Y);
					float speedModifier = speed / length;
					targetDir *= speedModifier;

					if (Main.player[NPC.target].dead)
					{
						// Run?
					}

					// Actual target movement calculation.
					if (NPC.velocity.X > 0f && targetDir.X > 0f || NPC.velocity.X < 0f && targetDir.X < 0f || NPC.velocity.Y > 0f && targetDir.Y > 0f || NPC.velocity.Y < 0f && targetDir.Y < 0f)
					{
						if (NPC.velocity.X < targetDir.X)
							NPC.velocity.X += turnSpeed;
						else if (NPC.velocity.X > targetDir.X)
							NPC.velocity.X -= turnSpeed;
						if (NPC.velocity.Y < targetDir.Y)
							NPC.velocity.Y += turnSpeed;
						else if (NPC.velocity.Y > targetDir.Y)
							NPC.velocity.Y -= turnSpeed;

						if (Math.Abs(targetDir.Y) < speed * .2f && (NPC.velocity.X > 0f && targetDir.X < 0 || NPC.velocity.X < 0f && targetDir.X > 0f))
						{
							if (NPC.velocity.Y > 0f)
								NPC.velocity.Y += turnSpeed * 2f;
							else
								NPC.velocity.Y -= turnSpeed * 2f;
						}
						if (Math.Abs(targetDir.X) < speed * .2f && (NPC.velocity.Y > 0f && targetDir.Y < 0f || NPC.velocity.Y < 0f && targetDir.Y > 0f))
						{
							if (NPC.velocity.X > 0f)
								NPC.velocity.X += turnSpeed * 2f;
							else
								NPC.velocity.X -= turnSpeed * 2f;
						}
					}
					else
					{
						if (absX > absY)
						{
							if (NPC.velocity.X < targetDir.X)
								NPC.velocity.X += turnSpeed * 1.1f;
							else if (NPC.velocity.X > targetDir.X)
								NPC.velocity.X -= turnSpeed * 1.1f;

							if (Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y) < speed * 0.5)
							{
								if (NPC.velocity.Y > 0f)
									NPC.velocity.Y += turnSpeed;
								else
									NPC.velocity.Y -= turnSpeed;
							}
						}
						else
						{
							if (NPC.velocity.Y < targetDir.Y)
								NPC.velocity.Y += turnSpeed * 1.1f;
							else if (NPC.velocity.Y > targetDir.Y)
								NPC.velocity.Y -= turnSpeed * 1.1f;
							if (Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y) < speed * 0.5)
							{
								if (NPC.velocity.X > 0f)
									NPC.velocity.X += turnSpeed;
								else
									NPC.velocity.X -= turnSpeed;
							}
						}
					}
				}

				NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X) + 1.57f;

				// Lastly, check if we need to netUpdate the NPC.
				if (inGround)
				{
					if (NPC.localAI[0] != 1f)
						NPC.netUpdate = true;
					NPC.localAI[0] = 1f;
				}
				else
				{
					if (NPC.localAI[0] != 0f)
						NPC.netUpdate = true;
					NPC.localAI[0] = 0f;
				}

				if (!NPC.justHit && (NPC.velocity.X > 0f && NPC.oldVelocity.X < 0f || NPC.velocity.X < 0f && NPC.oldVelocity.X > 0f || NPC.velocity.Y > 0f && NPC.oldVelocity.Y < 0f || NPC.velocity.Y < 0f && NPC.oldVelocity.Y > 0f))
				{
					NPC.netUpdate = true;
					return;
				}
			}
			else if (NPC.ai[1] > 0 && NPC.ai[1] < Main.npc.Length)
			{
				if (NPC.ai[3] > 0f)
					NPC.realLife = (int)NPC.ai[3];

				NPC targetNPC = Main.npc[(int)NPC.ai[1]];
				targetDir = new Vector2(targetNPC.position.X + (targetNPC.width / 2) - npcCenter.X, targetNPC.position.Y + (targetNPC.height / 2) - npcCenter.Y);

				NPC.rotation = (float)Math.Atan2(targetDir.Y, targetDir.X) + 1.57f;
				length = targetDir.Length();

				int width = NPC.width;
				length = (length - width) / length;
				targetDir *= length;
				NPC.velocity = Vector2.Zero;
				NPC.position += targetDir;
				NPC.spriteDirection = Math.Sign(targetDir.X);

				//if (Main.netMode != NetmodeID.MultiplayerClient)
				//{
					// Check if head is alive, active and not in its second stage...
					NPC headNPC = Main.npc[(int)NPC.ai[3]];
					NPC prevNPC = Main.npc[(int)NPC.ai[1]];

					if (headNPC == null || !headNPC.active || headNPC.ai[0] > 1 ||
						prevNPC == null || !prevNPC.active ||
						(prevNPC.type != NPCType<Serris_Head>() && prevNPC.type != NPCType<Serris_Body>() && prevNPC.type != NPCType<Serris_Tail>()))
					{
						NPC.life = 0;
						NPC.HitEffect(0, 10.0);
						NPC.active = false;
					}
				//}
			}
		}
	}
}
