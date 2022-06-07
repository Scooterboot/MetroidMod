using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroidModPorted.Content.NPCs.Kraid
{
	public class KraidClaw : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Kraid");
			NPCID.Sets.MPAllowedEnemies[Type] = true;
		}

		public override void SetDefaults()
		{
			NPC.width = 26;
			NPC.height = 26;
			NPC.scale = 1f;
			NPC.damage = 30;
			NPC.defense = 0;
			NPC.lifeMax = 1;
			NPC.dontTakeDamage = true;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.noGravity = true;
			NPC.knockBackResist = 0;
			NPC.lavaImmune = true;
			NPC.noTileCollide = true;
			NPC.behindTiles = false;
			NPC.aiStyle = -1;
			NPC.npcSlots = 1;
		}
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			int associatedNPCType = ModContent.NPCType<Kraid_Head>();
			bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[associatedNPCType], quickUnlock: true);
			bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
			{
				new MoonLordPortraitBackgroundProviderBestiaryInfoElement(), // Plain black background
				new FlavorTextBestiaryInfoElement("One of Kraid's infinitely generating claws.")
			});
		}

		float rotation = 0f;
		public override void AI()
		{
			rotation += 0.25f*NPC.direction;
		}

		/*public override void NPCLoot()
		{
			Player player = Main.player[(int)Player.FindClosest(NPC.position, NPC.width, NPC.height)];
			MPlayer mp = player.GetModPlayer<MPlayer>();
			bool flag = false;
			for(int i = 0; i < player.inventory.Length; i++)
			{
				if(player.inventory[i].type == mod.ItemType("MissileLauncher"))
				{
					flag = true;
				}
			}
			if(flag)
			{
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, mod.ItemType("MissilePickup"), 1 + Main.rand.Next(5));
			}
		}*/

		public override bool PreDraw(SpriteBatch sb, Vector2 screenPos, Color drawColor)
		{
			Texture2D tex = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			SpriteEffects effects = SpriteEffects.None;
			if (NPC.direction == -1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}
			sb.Draw(tex, NPC.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), NPC.GetAlpha(Color.White), rotation, new Vector2((float)tex.Width/2f, (float)tex.Height/2f), NPC.scale, effects, 0f);
			return false;
		}
	}
}
