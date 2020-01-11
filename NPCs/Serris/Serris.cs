using Microsoft.Xna.Framework;
using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using static Terraria.ModLoader.ModContent;

namespace MetroidMod.NPCs.Serris
{
	public class Serris : ModNPC
	{
		int bodyLength = 10;

		protected float defSpeed = 8f;
		protected float defTurnSpeed = .07f;

		public override bool Autoload(ref string name)
		{
			return (false);
		}

		private bool isTileGround(int x, int y) => Main.tile[x, y] != null && (Main.tile[x, y].nactive() && (Main.tileSolid[Main.tile[x, y].type] || Main.tileSolidTop[Main.tile[x, y].type] && Main.tile[x, y].frameY == 0) || Main.tile[x, y].liquid > 64);
						
		protected void Update_Worm(bool head = false)
		{
			if (!head && npc.timeLeft < 300)
				npc.timeLeft = 300;
			if (npc.target < 0 || npc.target > 255 || Main.player[npc.target].dead)
				npc.TargetClosest();
			if (Main.player[npc.target].dead && npc.timeLeft > 300)
				npc.timeLeft = 300;

			// Spawn the rest of Serris's body.
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				if (npc.ai[0] == 0 && head)
				{
					int prev = npc.whoAmI;
					npc.realLife = npc.whoAmI;
					for (int i = 0; i < bodyLength; i++)
					{
						int type = mod.NPCType("Serris_Body");
						if (i > 6)
							type = mod.NPCType("Serris_Tail");

						int srs = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, type, npc.whoAmI);
						Main.npc[srs].ai[0] = srs;
						Main.npc[srs].ai[1] = prev;
						Main.npc[srs].ai[3] = npc.whoAmI;
						Main.npc[srs].realLife = npc.whoAmI;

						if (i > 7)
							Main.npc[srs].ai[2] = i - 7;
						if (prev != npc.whoAmI)
							Main.npc[prev].ai[0] = srs;
						Main.npc[srs].netUpdate = true;
						prev = srs;
					}

					npc.ai[0] = 1;
					npc.netUpdate = true;
				}

				if (!npc.active && Main.netMode == 2)
					NetMessage.SendData(28, -1, -1, null, npc.whoAmI, -1f);
			}

			int tileXMin = (int)MathHelper.Clamp((npc.position.X / 16f) - 1, 0, Main.maxTilesX);
			int tileXMax = (int)MathHelper.Clamp(((npc.position.X + (float)npc.width) / 16f) + 2, 0, Main.maxTilesX);
			int tileYMin = (int)MathHelper.Clamp((npc.position.Y / 16f) - 1, 0, Main.maxTilesY);
			int tileYMax = (int)MathHelper.Clamp(((npc.position.Y + (float)npc.height) / 16f) + 2, 0, Main.maxTilesY);

			// Grounded check to see if the NPC is 'flying' or not.
			bool inGround = false;
			for (int x = tileXMin; x < tileXMax; ++x)
			{
				for (int y = tileYMin; y < tileYMax; ++y)
				{
					if (isTileGround(x, y))
					{
						Vector2 worldPos = new Vector2(x * 16, y * 16);
						if (npc.position.X + npc.width > worldPos.X && npc.position.X < worldPos.X + 16 && npc.position.Y + npc.height > worldPos.Y && npc.position.Y < worldPos.Y + 16)
						{
							inGround = true;
							// 1% chance to generate a dust effect on a tile.
							if (Main.rand.NextBool(100) && npc.behindTiles && Main.tile[x, y].nactive())
								WorldGen.KillTile(x, y, true, true, false);
						}
					}
				}
			}

			// Make sure the head of the NPC is flipped correctly vertically.
			npc.spriteDirection = Math.Sign(npc.velocity.X);

			float speed = defSpeed;
			float turnSpeed = defTurnSpeed;
			Vector2 npcCenter = new Vector2(npc.position.X + npc.width * .5f, npc.position.Y + npc.height * .5f);
			Vector2 targetDir = new Vector2(Main.player[npc.target].position.X + Main.player[npc.target].width * .5f, Main.player[npc.target].position.Y + Main.player[npc.target].height * .5f);

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
					npc.TargetClosest();
					npc.velocity.Y += .11f;
					if (npc.velocity.Y > speed)
						npc.velocity.Y = speed;

					if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < speed * .4f)
					{
						if (npc.velocity.X < 0f)
							npc.velocity.X -= turnSpeed * 1.1f;
						else
							npc.velocity.X += turnSpeed * 1.1f;
					}
					else if (npc.velocity.Y == speed)
						npc.velocity.X += Math.Sign(npc.velocity.X - targetDir.X) * turnSpeed * .9f;
					else if (npc.velocity.Y > 4f)
					{
						if (npc.velocity.X < 0f)
							npc.velocity.X += turnSpeed * .9f;
						else
							npc.velocity.X -= turnSpeed * .9f;
					}
				}
				else
				{
					if (npc.behindTiles && npc.soundDelay == 0)
					{
						float delay = length / 40f;
						delay = MathHelper.Clamp(delay, 10f, 20f);
						npc.soundDelay = (int)delay;
						Main.PlaySound(SoundID.Roar, npc.position, 1);
					}

					float absX = Math.Abs(targetDir.X);
					float absY = Math.Abs(targetDir.Y);
					float speedModifier = speed / length;
					targetDir *= speedModifier;

					if (Main.player[npc.target].dead)
					{
						// Run?
					}

					// Actual target movement calculation.
					if (npc.velocity.X > 0f && targetDir.X > 0f || npc.velocity.X < 0f && targetDir.X < 0f || npc.velocity.Y > 0f && targetDir.Y > 0f || npc.velocity.Y < 0f && targetDir.Y < 0f)
					{
						if (npc.velocity.X < targetDir.X)
							npc.velocity.X += turnSpeed;
						else if (npc.velocity.X > targetDir.X)
							npc.velocity.X -= turnSpeed;
						if (npc.velocity.Y < targetDir.Y)
							npc.velocity.Y += turnSpeed;
						else if (npc.velocity.Y > targetDir.Y)
							npc.velocity.Y -= turnSpeed;

						if (Math.Abs(targetDir.Y) < speed * .2f && (npc.velocity.X > 0f && targetDir.X < 0 || npc.velocity.X < 0f && targetDir.X > 0f))
						{
							if (npc.velocity.Y > 0f)
								npc.velocity.Y += turnSpeed * 2f;
							else
								npc.velocity.Y -= turnSpeed * 2f;
						}
						if (Math.Abs(targetDir.X) < speed * .2f && (npc.velocity.Y > 0f && targetDir.Y < 0f || npc.velocity.Y < 0f && targetDir.Y > 0f))
						{
							if (npc.velocity.X > 0f)
								npc.velocity.X += turnSpeed * 2f;
							else
								npc.velocity.X -= turnSpeed * 2f;
						}
					}
					else
					{
						if (absX > absY)
						{
							if (npc.velocity.X < targetDir.X)
								npc.velocity.X += turnSpeed * 1.1f;
							else if (npc.velocity.X > targetDir.X)
								npc.velocity.X -= turnSpeed * 1.1f;

							if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < speed * 0.5)
							{
								if (npc.velocity.Y > 0f)
									npc.velocity.Y += turnSpeed;
								else
									npc.velocity.Y -= turnSpeed;
							}
						}
						else
						{
							if (npc.velocity.Y < targetDir.Y)
								npc.velocity.Y += turnSpeed * 1.1f;
							else if (npc.velocity.Y > targetDir.Y)
								npc.velocity.Y -= turnSpeed * 1.1f;
							if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < speed * 0.5)
							{
								if (npc.velocity.X > 0f)
									npc.velocity.X += turnSpeed;
								else
									npc.velocity.X -= turnSpeed;
							}
						}
					}
				}

				npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) + 1.57f;

				// Lastly, check if we need to netUpdate the NPC.
				if (inGround)
				{
					if (npc.localAI[0] != 1f)
						npc.netUpdate = true;
					npc.localAI[0] = 1f;
				}
				else
				{
					if (npc.localAI[0] != 0f)
						npc.netUpdate = true;
					npc.localAI[0] = 0f;
				}

				if (!npc.justHit && (npc.velocity.X > 0f && npc.oldVelocity.X < 0f || npc.velocity.X < 0f && npc.oldVelocity.X > 0f || npc.velocity.Y > 0f && npc.oldVelocity.Y < 0f || npc.velocity.Y < 0f && npc.oldVelocity.Y > 0f))
				{
					npc.netUpdate = true;
					return;
				}
			}
			else if (npc.ai[1] > 0 && npc.ai[1] < Main.npc.Length)
			{
				if (npc.ai[3] > 0f)
					npc.realLife = (int)npc.ai[3];

				NPC targetNPC = Main.npc[(int)npc.ai[1]];
				targetDir = new Vector2(targetNPC.position.X + (targetNPC.width / 2) - npcCenter.X, targetNPC.position.Y + (targetNPC.height / 2) - npcCenter.Y);

				npc.rotation = (float)Math.Atan2(targetDir.Y, targetDir.X) + 1.57f;
				length = targetDir.Length();

				int width = npc.width;
				length = (length - width) / length;
				targetDir *= length;
				npc.velocity = Vector2.Zero;
				npc.position += targetDir;
				npc.spriteDirection = Math.Sign(targetDir.X);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					// Check if head is alive, active and not in its second stage...
					NPC headNPC = Main.npc[(int)npc.ai[3]];
					NPC prevNPC = Main.npc[(int)npc.ai[1]];

					if (headNPC == null || !headNPC.active || headNPC.ai[0] > 1 ||
						prevNPC == null || !prevNPC.active ||
						(prevNPC.type != NPCType<Serris_Head>() && prevNPC.type != NPCType<Serris_Body>() && prevNPC.type != NPCType<Serris_Tail>()))
					{
						npc.life = 0;
						npc.HitEffect(0, 10.0);
						npc.active = false;
					}
				}
			}
		}
	}
}
