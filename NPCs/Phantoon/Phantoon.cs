using Terraria;
using Terraria.ID;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroidMod.NPCs.Phantoon
{
    public class Phantoon : ModNPC
    {
		// Written by Empio

		// Order of Actions
		// ----------------
		// Spawn
		// Appear
		// Swing - hit = continue swing
		// Crash - 4
		// Whip - 8 (Right to Left first)
		// ------------------------------
		// Repeat Below
		// ------------------------------
		// Swing - hit = continue swing - if hit player on pulse skip to Crash
		// InvisSwing
		// Crash - 4
		// Whip - 8 (Right to Left first)
		public bool spawn = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phantoon");
		}
public override void SetDefaults()
		{
			npc.width = 28;
			npc.height = 27;
			npc.damage = 50;
			npc.defense = 25;
			npc.lifeMax = 1500;
			npc.dontTakeDamage = true;
			npc.alpha = 255;
			npc.scale = 1.6f;
			npc.boss = true;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath10;
			npc.noGravity = true;
			npc.value = Item.buyPrice(0, 0, 7, 0);
			npc.knockBackResist = 0;
			npc.lavaImmune = true;
			npc.noTileCollide = true;
			npc.behindTiles = true;
			npc.buffImmune[20] = true;
			npc.buffImmune[24] = true;
			npc.buffImmune[31] = true;
			npc.buffImmune[39] = true;
			npc.frameCounter = 0;
			npc.aiStyle = -1;
			npc.npcSlots = 0;
			music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Kraid");
			bossBag = mod.ItemType("PhantoonBag");
		}
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.7f * bossLifeScale + 1);
			npc.damage = (int)(npc.damage * 0.7f);
		}
		public override void BossLoot(ref string name, ref int potionType)
		{
			potionType = ItemID.GreaterHealingPotion;
		}
		public override void NPCLoot()
		{
			if (Main.expertMode)
			{
				npc.DropBossBags();
			}
			else
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("GravityGel"), Main.rand.Next(20, 51));
				if (Main.rand.Next(5) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("KraidPhantoonMusicBox"));
				}
				if (Main.rand.Next(7) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PhantoonMask"));
				}
				if (Main.rand.Next(10) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PhantoonTrophy"));
				}
			}
		}
		// enumeration for what action to perform
		public enum Actions
		{
			Initialise,				// Initialise variables, spawn Phantoons body parts and set their locations
			SpawnProjectiles,		// Create fireballs in a circle around Phantoon
			RotateProjectiles,		// Rotate the circle of fireballs
			Spawn,					// Set variables and destroy circle of fireballs
			SpawnAppear,			// Phantoon appears!
			AccelerateSwing,		// Phantoon will be in a figure 8 pattern and accelerate, he will also spawn fireballs
			DecelerateSwing,		// Phantoon will be in a figure 8 pattern and decelerate
			SwingPulse,				// Phantoon will send out a circle of fireballs
			SwingWaitDisappear,		// Phantoon will wait before disappearing
			SwingDisappear,			// Phantoon will disappear
			SetCrash,				// Sets variables for the Crash action
			Crash,					// Phantoon will drop a horizontal line of fireballs from his position
			CrashDisappear,			// Phantoon will disappear after crashing fireballs
			EndCrash,				// Resets variables after crashing phase has ended
			WaitWhip,				// Phantoon will wait before creating a whip of fireballs
			SetWhip,				// Sets variables for the Whip action
			WhipAppear,				// Phantoon will appear above the player
			Whip,					// Phantoon will create a whip of fireballs which goes across the screen
			WhipDisappear,			// Phantoon will disappear once he was whipped enough times
			DespawnDisappear,		// Phantoon will despawn if the summoner dies
			Despawn					// Phantoon will despawn if the summoner dies
		}

		// actions fireballs should perform
		public enum ProjActions
		{
			Spawn,			// fireball will stay where it is created until all projectiles are created - changes to SpawnRotate
			SpawnRotate,	// fireballs will orbit around Phantoon
			SwingPulse,		// fireballs will fire out from Phantoon in a spiral
			Bounce,			// fireballs will fall to the ground and bounce when they hit it
			Crash,			// fireballs will fall to the ground and disappear when they hit it
			Whip			// fireballs will orbit around their parent (previous fireball or Phantoon) to make a chain/whip
		}

		// Fireball projectile class - is actually an npc
		public class Fireball
		{
			public int ID;										// npc ID
			public int GroupNo;									// number of the projectile in it's group
			public ProjActions Action;							// fireball ai
			public float Timer;									// duration timer
			public float Duration;								// time to last before disappearing
			public float AnimTimer;								// animation timer
			public float AnimDuration;							// animation duration
			public int FrameCount;								// number of frames in animation
			public bool DestroyOnTile;							// if the fireball should be destroyed when it hits a tile
			public int BounceCount;								// number of times to bounce - bounce height reduces each bounce
			public const float BounceHeight = 8.0f;				// initial bounce height
			public const int MaxBounceCount = 5;				// max number of times to bounce
			
			// constructor
			public Fireball(int a_ID, int a_GroupNo, ProjActions a_Action, float a_Duration,
							float a_AnimDuration, int a_FrameCount, bool a_DestroyOnTile)
			{
				ID = a_ID;
				GroupNo = a_GroupNo;
				Action = a_Action;
				Timer = 0.0f;
				Duration = a_Duration;
				AnimTimer = 0.0f;
				AnimDuration = a_AnimDuration;
				FrameCount = a_FrameCount;
				DestroyOnTile = a_DestroyOnTile;
				BounceCount = 0;
			}
		}

		#region Variables

		// action to perform
		Actions Action = Actions.Initialise;

		// timer for performing actions
		float ActionTimer = 0.0f;

		// timer for swinging position
		float SwingingTimer = 0.0f;

		// time to accelerate and decelerate while swinging
		const float SwingAccelDuration = 3.0f;
		const float SwingDecelDuration = 1.0f;

		// swing speed
		float SwingSpeed = 0.0f;
		const float MinSwingSpeed = 0.075f;
		const float MaxSwingSpeed = 0.5f;
		const float SwingAccel = 0.3f;

		// max distance to travel from centre while swinging
		const int SwingExtension = 256;

		// time to wait after pulsing before disappearing
		const float SwingDisappearDelay = 1.0f;

		// number of times swung - changing to accelerate or to decelerate increases the count
		int SwingCount = 0;
		const int MaxSwingCount = 3;

		// time between creating projectiles on spawn
		const float SpawnProjectileDelay = 0.5f;

		// number of fireballs spawned while swinging
		int SwingFireballs = 0;

		// How far the swing pulse projectiles will travel before disappearing
		const int SwingPulseExtension = 384;

		// duration for rotating spawn projectiles
		const float RotateDuration = 4.0f;

		// when to start moving the spawn projectiles inwards while rotating
		const float InwardsDelay = 3.0f;

		// number of spawn projectiles created
		int ProjectilesSpawned = 0;

		// maximum projectiles to create at once
		const int MaxProjectiles = 8;

		// position during crashing
		float CrashX = 0.0f;
		float CrashY = 0.0f;

		// duration of one crash
		const float CrashDuration = 1.5f;

		// number of times crashed
		int CrashCount = 0;
		const int MaxCrashCount = 4;

		// max distance from centreX to appear or create projectiles at
		const int CrashExtension = 256;

		// acceleration speed for crash projectiles
		const float CrashProjectileAccel = 0.3f;

		// time between dropping projectiles while crashing
		const float CrashProjectileDropDelay = 0.1f;

		// time to wait after crashing and disappearing and before whipping
		const float WaitWhipDelay = 1.0f;

		// duration of one whip
		const float WhipDuration = 2.0f;

		// number of whips done
		int WhipCount = 0;
		const int MaxWhipCount = 4;

		// direction to whip
		int WhipDirection = 1;

		// final distance between each fireball in the whip chain
		int WhipSpread = 128;

		// durations for appearing and disappearing
		const float AppearDuration = 1.0f;
		const float DisappearDuration = 1.5f;

		// list of projectiles to animate and update
		List<Fireball> ActiveProjectiles = new List<Fireball>();

		// NPC IDs
		int PhantoonUpper = 0;
		int PhantoonMid = 0;
		int PhantoonLower = 0;

		int UpperType = 0;//NPCDef.byName["MetroidMod:PhantoonUpperPart"].type;
		int MidType = 0;//NPCDef.byName["MetroidMod:PhantoonMiddlePart"].type;
		int LowerType = 0;//NPCDef.byName["MetroidMod:PhantoonLowerPart"].type;

		// total size of Phantoon
		float TotalWidth = 168.0f;
		float TotalHeight = 118.0f;

		// player who summoned Phantoon
		Player Summoner = null;

		// 360 degrees in radians
		const float Revolution = 6.28308f;

		// the type the projectiles will use
		int FireballType = 0;//NPCDef.byName["MetroidMod:FireBall"].type;
		int ID2 = 0;

		// delta time
		float Delta = 0.0f;

		// first cycle of phases since Phantoon spawned
		bool FirstCycle = true;

		// Phantoon will be invisible during Swing action
		bool InvisSwing = false;

		#endregion

		// first time run - not using Initialize as Phantoon's target needs to be set first
		//public void CustomInit()
		public override bool PreAI()
		{
			if (!spawn)
			{
			UpperType = mod.NPCType("PhantoonUpperPart");
			MidType = mod.NPCType("PhantoonMiddlePart");
			LowerType = mod.NPCType("PhantoonLowerPart");
			FireballType = mod.NPCType("FireBall");
			Action = Actions.SpawnProjectiles;
			
			// spawn Phantoon's body parts and set body part sizes
			InitUpper();
			InitMid();
			InitLower();

			// target player
			npc.TargetClosest(true);

			// get target player
			Summoner = Main.player[npc.target];
			
			// get total size of Phantoon
			TotalWidth = Main.npc[PhantoonUpper].width;
			TotalHeight = Main.npc[PhantoonUpper].height + Main.npc[PhantoonMid].height + Main.npc[PhantoonLower].height;
			
			// set initial position
			npc.position.X = Summoner.position.X + (Summoner.width - npc.width) * 0.5f;
			npc.position.Y = Summoner.position.Y + (Summoner.height - npc.height) * 0.5f - TotalHeight;
			
			// set positions of Phantoon's body parts to their correct positions in relation to Phantoon
			SetLocations();

			spawn = true;
			}
			return true;
		}

		// main function
		public override void AI()
		{
			// target player
			npc.TargetClosest(true);

			// get target player
			Summoner = Main.player[npc.target];

			// get total size of Phantoon
			TotalWidth = Main.npc[PhantoonUpper].width;
			TotalHeight = Main.npc[PhantoonUpper].height + Main.npc[PhantoonMid].height + Main.npc[PhantoonLower].height;

			// buff immunities
			npc.buffImmune[20] = true;
			npc.buffImmune[24] = true;
			npc.buffImmune[31] = true;
			npc.buffImmune[39] = true;
			npc.buffImmune[mod.BuffType("PhazonDebuff")] = true;

			// Delta time - used for measuring time in milliseconds/seconds instead of Terraria's fixed updates
			Delta = 1.0f / 60f;//Main.frameRate;
			
			ActionTimer += Delta;
			
			if (!Main.npc[PhantoonUpper].active || Main.npc[PhantoonUpper].type != UpperType)
			{
				InitUpper();
			}
			if (!Main.npc[PhantoonMid].active || Main.npc[PhantoonMid].type != MidType)
			{
				InitMid();
			}
			if (!Main.npc[PhantoonLower].active || Main.npc[PhantoonLower].type != LowerType)
			{
				InitLower();
			}
			if (Main.player[npc.target] == null || !Main.player[npc.target].active || Main.player[npc.target].dead)
			{
				ActionTimer = 0.0f;
				Action = Actions.DespawnDisappear;
			}
			
			// Action switch - do function based on the value of Action
			switch(Action)
			{
				case Actions.SpawnProjectiles:
					DoSpawnProjectiles();
					break;
				case Actions.RotateProjectiles:
					DoRotateProjectiles();
					break;
				case Actions.Spawn:
					DoSpawn();
					break;
				case Actions.SpawnAppear:
					DoSpawnAppear();
					break;
				case Actions.AccelerateSwing:
					DoAccelerateSwing();
					break;
				case Actions.DecelerateSwing:
					DoDecelerateSwing();
					break;
				case Actions.SwingPulse:
					DoSwingPulse();
					break;
				case Actions.SwingWaitDisappear:
					DoSwingWaitDisappear();
					break;
				case Actions.SwingDisappear:
					DoSwingDisappear();
					break;
				case Actions.SetCrash:
					DoSetCrash();
					break;
				case Actions.Crash:
					DoCrash();
					break;
				case Actions.CrashDisappear:
					DoCrashDisappear();
					break;
				case Actions.EndCrash:
					DoEndCrash();
					break;
				case Actions.WaitWhip:
					DoWaitWhip();
					break;
				case Actions.SetWhip:
					DoSetWhip();
					break;
				case Actions.WhipAppear:
					DoWhipAppear();
					break;
				case Actions.Whip:
					DoWhip();
					break;
				case Actions.WhipDisappear:
					DoWhipDisappear();
					break;
				case Actions.DespawnDisappear:
					DoDespawnDisappear();
					break;
				case Actions.Despawn:
					DoDespawn();
					break;
				default:
					break;
			}
			
			// updates all active fireballs
			UpdateProjectiles();
			
			// sets the location of Phantoon's body parts
			SetLocations();
			
			// Phantoon is vulnerable if his eye is visible/open
			npc.dontTakeDamage = npc.alpha >= 127;
			
			// Phantoon will emit light while his eye is open and visible
			Lighting.AddLight((int)npc.position.X / 16, (int)npc.position.Y / 16, (255.0f - npc.alpha) / 255, (255.0f - npc.alpha) / 255, (255.0f - npc.alpha) / 255);
		}

		// runs when Phantoon is hit or killed
	public override void HitEffect(int hitDirection, double damage)
		{
			if(npc.life <= 0)
			{
				Main.npc[PhantoonUpper].active = false;
				Main.npc[PhantoonMid].active = false;
				Main.npc[PhantoonLower].active = false;
				Main.npc[ID2].active = false;
				for(int i = 0; i < Main.npc.Length; i++)
				{
					if(Main.npc[i].active && Main.npc[i].type == mod.NPCType("FireBall"))
					{
						Main.npc[i].life = 0;
					}
				}
				for (int num350 = 0; num350 < 20; num350++)
				{
					int num351 = Dust.NewDust(npc.position, npc.width, npc.height, 30, 0f, 0f, 50, default(Color), 1.5f);
					Main.dust[num351].velocity *= 2f;
					Main.dust[num351].noGravity = true;
				}
				int num352 = Gore.NewGore(new Vector2(npc.position.X, npc.position.Y - 10f), new Vector2((float)hitDirection, 0f), 61, npc.scale);
				Main.gore[num352].velocity *= 0.3f;
				num352 = Gore.NewGore(new Vector2(npc.position.X, npc.position.Y + (float)(npc.height / 2) - 15f), new Vector2((float)hitDirection, 0f), 62, npc.scale);
				Main.gore[num352].velocity *= 0.3f;
				num352 = Gore.NewGore(new Vector2(npc.position.X, npc.position.Y + (float)npc.height - 20f), new Vector2((float)hitDirection, 0f), 63, npc.scale);
				Main.gore[num352].velocity *= 0.3f;
			}
		}

		// initialise Phantoon's upper section
		public void InitUpper()
		{
			PhantoonUpper = NPC.NewNPC(0, 0, UpperType);
			
			Main.npc[PhantoonUpper].scale = npc.scale;
			Main.npc[PhantoonUpper].width = (int)(Main.npc[PhantoonUpper].width * npc.scale);
			Main.npc[PhantoonUpper].height = (int)(Main.npc[PhantoonUpper].height * npc.scale);
		}

		// initialise Phantoon's mid section
		public void InitMid()
		{
			PhantoonMid = NPC.NewNPC(0, 0, MidType);
			
			Main.npc[PhantoonMid].scale = npc.scale;
			Main.npc[PhantoonMid].width = (int)(Main.npc[PhantoonMid].width * npc.scale);
			Main.npc[PhantoonMid].height = (int)(Main.npc[PhantoonMid].height * npc.scale);
		}

		// initialise Phantoon's lower section
		public void InitLower()
		{
			PhantoonLower = NPC.NewNPC(0, 0, LowerType);
			
			Main.npc[PhantoonLower].scale = npc.scale;
			Main.npc[PhantoonLower].width = (int)(Main.npc[PhantoonLower].width * npc.scale);
			Main.npc[PhantoonLower].height = (int)(Main.npc[PhantoonLower].height * npc.scale);
		}

		// sets the location of Phantoon's body parts
		// the 2 is because there is a 1 transparent pixel line on the top and bottom of each frame
		// the reason for the transparent line is to stop frames bleeding over into each other
		public void SetLocations()
		{
			float UX = npc.position.X + (npc.width - Main.npc[PhantoonUpper].width) * 0.5f;
			float UY = npc.position.Y - TotalHeight * 0.5f + 11 * npc.scale;
			
			Main.npc[PhantoonUpper].position.X = UX;
			Main.npc[PhantoonUpper].position.Y = UY;
			
			float MX = npc.position.X + (npc.width - Main.npc[PhantoonMid].width) * 0.5f;
			float MY = UY + Main.npc[PhantoonUpper].height - 2 * npc.scale;
			
			Main.npc[PhantoonMid].position.X = MX;
			Main.npc[PhantoonMid].position.Y = MY;
			
			float LX = npc.position.X + (npc.width - Main.npc[PhantoonLower].width) * 0.5f;
			float LY = MY + Main.npc[PhantoonMid].height - 2 * npc.scale;
			
			Main.npc[PhantoonLower].position.X = LX;
			Main.npc[PhantoonLower].position.Y = LY;
		}

		// updates all active fireballs
		public void UpdateProjectiles()
		{
			// saves on typing
			Fireball FB;
			
			// do for every fireball
			for (int Proj = 0; Proj < ActiveProjectiles.Count; Proj++)
			{
				// get current fireball
				FB = ActiveProjectiles[Proj];
				
				// projectile is not active or not of the correct type
				if (!ProjectileSafe(FB))
					continue;
				
				// fireball will be destroyed when it hits a tile
				if (FB.DestroyOnTile)
					// using Terraria's in-built collision
					if (Collision.SolidCollision(Main.npc[FB.ID].position, Main.npc[FB.ID].width, Main.npc[FB.ID].height))
					{
						DropProjectile(Proj);
						Proj--;
						continue;
					}
				
				// if a duration for the fireball has been set
				// fireballs without a duration have to be destroyed manually
				// fireball has lasted it's full duration
				if (FB.Duration > 0.0f && FB.Timer >= FB.Duration)
				{
					DropProjectile(Proj);
					Proj--;
					continue;
				}
				
				// set the fireball's current frame in it's animation
				Main.npc[FB.ID].frame.Y = (int)(FB.AnimTimer / FB.AnimDuration * FB.FrameCount) * Main.npc[FB.ID].frame.Height;
				
				FB.AnimTimer += Delta;
				
				// reset animation
				if (FB.AnimTimer >= FB.AnimDuration)
					FB.AnimTimer -= FB.AnimDuration;
				
				// add light to the fireballs position - making it seem like it is glowing
				Lighting.AddLight((int)Main.npc[FB.ID].position.X / 16, (int)Main.npc[FB.ID].position.Y / 16, 1.0f, 1.0f, 1.0f);
				
				// update the fireball depending on it's assigned action
				switch(FB.Action)
				{
					case ProjActions.Bounce:
						ProjBounce(FB.ID, Proj);
						break;
					case ProjActions.Spawn:
						ProjSpawn(Proj);
						break;
					case ProjActions.SpawnRotate:
						ProjSpawnRotate(Proj, FB.GroupNo);
						break;
					case ProjActions.SwingPulse:
						ProjSwingPulse(FB.ID, Proj, FB.GroupNo);
						break;
					case ProjActions.Crash:
						ProjCrash(FB.ID);
						break;
					case ProjActions.Whip:
						ProjWhip(FB.ID, Proj, FB.GroupNo);
						break;
					default:
						break;
				}
				
				FB.Timer += Delta;
			}
		}

		// fireball will bounce when it hits the ground
		public void ProjBounce(int a_ID, int a_ArrayPos)
		{
			// tile positions below the fireball
			int XL = (int)(Main.npc[a_ID].position.X + Main.npc[a_ID].width * 0.25f) / 16;
			int XR = (int)(Main.npc[a_ID].position.X + Main.npc[a_ID].width * 0.75f) / 16;
			int Y = (int)(Main.npc[a_ID].position.Y + Main.npc[a_ID].height) / 16 + 1;
			
			// readability
			float BounceHeight = Fireball.BounceHeight;
			int BounceCount = ActiveProjectiles[a_ArrayPos].BounceCount;
			int MaxBounceCount = Fireball.MaxBounceCount;
			
			// bottom left tile
			if (Main.tile[XL, Y] != null && Main.tile[XL, Y].active() && Main.tileSolid[Main.tile[XL, Y].type])
			{
				// bounce up and to the left or right
				Main.npc[a_ID].velocity.X = (Main.rand.Next(2) * 2 - 1) * 2;
				Main.npc[a_ID].velocity.Y = -BounceHeight + BounceHeight * 0.5f * BounceCount / MaxBounceCount;
				ActiveProjectiles[a_ArrayPos].BounceCount++;
				return;
			}
			
			// bottom right tile
			if (Main.tile[XR, Y] != null && Main.tile[XR, Y].active() && Main.tileSolid[Main.tile[XR, Y].type])
			{
				// bounce up and to the left or right
				Main.npc[a_ID].velocity.X = (Main.rand.Next(2) * 2 - 1) * 2;
				Main.npc[a_ID].velocity.Y = -BounceHeight + BounceHeight * 0.5f * BounceCount / MaxBounceCount;
				ActiveProjectiles[a_ArrayPos].BounceCount++;
			}
		}

		// fireballs will do nothing until all projectiles are spawned
		public void ProjSpawn(int a_ArrayPos)
		{
			if (ProjectilesSpawned >= MaxProjectiles)
			{
				ActiveProjectiles[a_ArrayPos].Action = ProjActions.SpawnRotate;
			}
		}

		// fireballs will orbit around Phantoon and move inwards after a while
		public void ProjSpawnRotate(int a_ArrayPos, int a_GroupNo)
		{
			// destroy fireballs if phase is over
			if (Action != Actions.SpawnProjectiles && Action != Actions.RotateProjectiles)
			{
				DropProjectile(a_ArrayPos);
				
				// not updating this fireball
				return;
			}
			
			int ID = ActiveProjectiles[a_ArrayPos].ID;
			
			float Rot = ActionTimer * Revolution / RotateDuration;	// one full rotation over the rotate delay
			
			float BaseX = npc.position.X + npc.width * 0.5f;
			float BaseY = npc.position.Y + npc.height * 0.5f;
			
			float HalfHeight = TotalHeight * 0.5f;
			
			float X = BaseX - Main.npc[ID].width * 0.5f;
			float Y = BaseY - Main.npc[ID].height * 0.5f;
			
			Rot += a_GroupNo * Revolution / MaxProjectiles;		// add rotation to get this projectile's position
			
			float AddSin = (float)Math.Sin(Rot) * HalfHeight;
			float AddCos = (float)Math.Cos(Rot) * HalfHeight;
			
			// rotating and moving inwards
			if (ActionTimer >= InwardsDelay)
			{
				float Inwards = 0.0f;
				
				if (RotateDuration - InwardsDelay > 0.0f)
				{
					// value between 0.0f and 1.0f
					Inwards = (RotateDuration - ActionTimer) / (RotateDuration - InwardsDelay);
				}
				
				AddSin *= Inwards;
				AddCos *= Inwards;
			}
			
			X += AddSin;
			Y -= AddCos;
			
			Main.npc[ID].position.X = X;
			Main.npc[ID].position.Y = Y;
		}

		// Phantoon will shoot fireballs out in a spiral pattern from his position
		public void ProjSwingPulse(int a_ID, int a_ArrayPos, int a_GroupNo)
		{
			float MidX = npc.position.X + (npc.width - Main.npc[a_ID].width) * 0.5f;
			float MidY = npc.position.Y + (npc.height - Main.npc[a_ID].height) * 0.5f;
			
			float Timer = ActiveProjectiles[a_ArrayPos].Timer;
			
			float AddX = (float)Math.Cos(a_GroupNo * Revolution / MaxProjectiles + Timer * 2) * Timer * SwingPulseExtension;
			float AddY = (float)Math.Sin(a_GroupNo * Revolution / MaxProjectiles + Timer * 2) * Timer * SwingPulseExtension;
			
			Main.npc[a_ID].position.X = MidX + AddX;
			Main.npc[a_ID].position.Y = MidY + AddY;
		}

		// Phantoon will drop a horizontal line of projectiles from his position
		public void ProjCrash(int a_ID)
		{
			Main.npc[a_ID].velocity.Y += CrashProjectileAccel;
		}

		// Phantoon will create a line of fireballs - the first will orbit around him and
		// the rest will orbit around the previous fireball
		public void ProjWhip(int a_ID, int a_ArrayPos, int a_GroupNo)
		{
			// not in whip phase, don't update
			if (Action != Actions.Whip)
				return;
			
			// will do 75% of a full rotation
			float Rot = ActionTimer * 4.71239f / WhipDuration;
			
			// parent position to orbit around
			float ParentX = 0.0f;
			float ParentY = 0.0f;
			
			// distance from the parent
			float Distance = WhipSpread * ActionTimer / WhipDuration;
			
			// X and Y distances
			float CosRot = WhipDirection * Distance;
			float SinRot = Distance;
			
			// first fireball in the chain
			if (a_GroupNo == 0)
			{
				CosRot *= (float)Math.Cos(Rot);
				SinRot *= (float)Math.Sin(Rot);
				ParentX = npc.position.X;
				ParentY = npc.position.Y;
			}
			else
			{
				// get orbit position around the previous fireball in the chain
				float NewRot = Rot - (float)a_GroupNo / MaxProjectiles * 1.14159f;
				CosRot *= (float)Math.Cos(NewRot);
				SinRot *= (float)Math.Sin(NewRot);
				ParentX = Main.npc[ActiveProjectiles[a_ArrayPos - 1].ID].position.X;
				ParentY = Main.npc[ActiveProjectiles[a_ArrayPos - 1].ID].position.Y;
			}
			
			// set whip fireball location
			Main.npc[a_ID].position.X = ParentX + CosRot;
			Main.npc[a_ID].position.Y = ParentY + SinRot;
		}

		// create a new fireball
		public int SpawnFireball(int a_GroupNo, ProjActions a_Action, float a_Duration, bool a_DestroyOnTile)
		{
			ID2 = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("FireBall"), npc.whoAmI);
			Main.PlaySound(SoundLoader.customSoundType, (int)npc.Center.X, (int)npc.Center.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/PhantoonFire"));
			// set fireball size
			Main.npc[ID2].scale = npc.scale;
			Main.npc[ID2].width = (int)(Main.npc[ID2].width * npc.scale);
			Main.npc[ID2].height = (int)(Main.npc[ID2].height * npc.scale);
			
			// no inactive fireballs in the list, add a new fireball to the list
			ActiveProjectiles.Add(new Fireball(ID2, a_GroupNo, a_Action, a_Duration, 0.2f, 2, a_DestroyOnTile));
			
			// return fireball npc ID
			return ID2;
		}

		// spawn fireballs in a circle pattern around Phantoon
		public void DoSpawnProjectiles()
		{
			// time to spawn the next projectile
			if (ActionTimer >= SpawnProjectileDelay * ProjectilesSpawned)
			{
				// get rotation at which to spawn the projectile
				float Rot = ProjectilesSpawned * Revolution / MaxProjectiles;
				
				// create projectile
				int Proj = SpawnFireball(ProjectilesSpawned, ProjActions.Spawn, 0.0f, false);
				
				// get position from rotation
				float X = npc.position.X + (npc.width - Main.npc[Proj].width +
											(float)Math.Sin(Rot) * TotalHeight) * 0.5f;
				
				float Y = npc.position.Y + (npc.height - Main.npc[Proj].height -
											(float)Math.Cos(Rot) * TotalHeight) * 0.5f;
				
				// move projectile into position
				Main.npc[Proj].position.X = X;
				Main.npc[Proj].position.Y = Y;
				
				// increase projectile count
				ProjectilesSpawned++;
				
				// finished creating projectiles move to next spawn phase
				if (ProjectilesSpawned >= MaxProjectiles)
				{
					ActionTimer = 0.0f;
					Action = Actions.RotateProjectiles;
				}
			}
		}

		// if a fireball is active and of the correct type
		public bool ProjectileSafe(Fireball FB)
		{
			return FB != null && Main.npc[FB.ID] != null && Main.npc[FB.ID].active && Main.npc[FB.ID].type == FireballType;
		}

		// set a fireball to inactive
		public void DropProjectile(int ArrayPos)
		{
			if (ProjectileSafe(ActiveProjectiles[ArrayPos]))
				Main.npc[ActiveProjectiles[ArrayPos].ID].active = false;
			
			ActiveProjectiles.RemoveAt(ArrayPos);
		}

		// rotate fireball circle around Phantoon
		public void DoRotateProjectiles()
		{
			// finished rotating and moving inwards - phase has ended
			if (ActionTimer >= RotateDuration)
			{
				Action = Actions.Spawn;
			}
		}

		// spawn Phantoon's projectiles
		public void DoSpawn()
		{
			Action = Actions.SpawnAppear;
			
			ActionTimer = 0.0f;
			
			if (Main.netMode == 0)
			{
				Main.NewText("Phantoon has awoken!", 175, 75, 255, false);
			}
			/*else if (Main.netMode == 2)
			{
				NetMessage.SendData(25, -1, -1, "Phantoon " + Lang.misc[16], 255, 175f, 75f, 255f, 0, 0);				
			}*/
			npc.boss = true;
			// destroy all active projectiles
			for (int Proj = 0; Proj < ActiveProjectiles.Count; Proj++)
			{
				if (ActiveProjectiles[Proj].Action == ProjActions.SpawnRotate)
				{
					DropProjectile(Proj);
					Proj--;
				}
			}
		}

		// Phantoon appears!
		public void DoSpawnAppear()
		{
			Appear(Actions.AccelerateSwing);
		}

		// Phantoon has appeared!
		public void Appear(Actions NextAction)
		{
			// get alpha - reduce over time - 255 is invisible
			int Alpha = 255 - (int)(ActionTimer / AppearDuration * 255);
			
			Main.npc[PhantoonUpper].alpha = Alpha;
			Main.npc[PhantoonMid].alpha = Alpha;
			Main.npc[PhantoonLower].alpha = Alpha;
			
			// finished appearing
			if (ActionTimer >= AppearDuration)
			{
				Action = NextAction;
				ActionTimer = 0.0f;
				Main.npc[PhantoonUpper].alpha = 0;
				Main.npc[PhantoonMid].alpha = 0;
				Main.npc[PhantoonLower].alpha = 0;
			}
		}

		// move in a figure 8 pattern speeding up and dropping bouncing fireballs
		public void DoAccelerateSwing()
		{
			SwingingTimer += Revolution * SwingSpeed * Delta;
			
			// increase swinging speed
			SwingSpeed += SwingAccel * Delta;
			
			if (SwingSpeed > MaxSwingSpeed)
				SwingSpeed = MaxSwingSpeed;
			
			// drop half the fireballs in first accelerating swing and the rest in the second
			if (SwingCount < 2)
			{
				// time to spawn another
				if (SwingFireballs < MaxProjectiles * 0.5f && ActionTimer >= SwingAccelDuration * SwingFireballs / MaxProjectiles)
				{
					int Proj = SpawnFireball(0, ProjActions.Bounce, 3.0f, false);
					
					Main.npc[Proj].position.X = npc.position.X + (npc.width - Main.npc[Proj].width) * 0.5f;
					Main.npc[Proj].position.Y = npc.position.Y + (npc.height - Main.npc[Proj].height) * 0.5f;
					
					Main.npc[Proj].noGravity = false;
					
					SwingFireballs++;
				}
			}
			// time to spawn another
			else if (SwingFireballs < MaxProjectiles && ActionTimer >= SwingAccelDuration * SwingFireballs / MaxProjectiles)
			{
				int Proj = SpawnFireball(0, ProjActions.Bounce, 3.0f, false);
				
				Main.npc[Proj].position.X = npc.position.X + (npc.width - Main.npc[Proj].width) * 0.5f;
				Main.npc[Proj].position.Y = npc.position.Y + (npc.height - Main.npc[Proj].height) * 0.5f;
				
				Main.npc[Proj].noGravity = false;
				
				SwingFireballs++;
			} 
			
			// this swing is over, change to decelerate
			if (ActionTimer >= SwingAccelDuration)
			{
				ActionTimer = 0.0f;
				Action = Actions.DecelerateSwing;
				SwingCount++;
			}
			
			// update position
			SetSwingPosition();
			
			// cut out halfway through final swing or halfway through middle swing if invisible
			if ((SwingCount >= MaxSwingCount && ActionTimer >= SwingAccelDuration * 0.5f) ||
			   (SwingCount >= MaxSwingCount * 0.5f && InvisSwing))
			{
				ActionTimer = 0.0f;
				
				// if invisible
				if (InvisSwing)
					Action = Actions.SwingWaitDisappear;
				else
					Action = Actions.SwingPulse;
			}
		}

		// move in a figure 8 pattern slowing down - see DoAccelerateSwing() for comments
		public void DoDecelerateSwing()
		{
			SwingingTimer += Revolution * SwingSpeed * Delta;
			
			SwingSpeed -= SwingAccel * Delta;
			
			if (SwingSpeed < MinSwingSpeed)
				SwingSpeed = MinSwingSpeed;
			
			if (ActionTimer >= SwingDecelDuration)
			{
				ActionTimer = 0.0f;
				Action = Actions.AccelerateSwing;
				SwingCount++;
			}
			
			SetSwingPosition();
			
			if ((SwingCount >= MaxSwingCount && ActionTimer >= SwingDecelDuration * 0.5f) ||
			   (SwingCount >= MaxSwingCount * 0.5f && InvisSwing))
			{
				ActionTimer = 0.0f;
				
				if (InvisSwing)
					Action = Actions.SwingWaitDisappear;
				else
					Action = Actions.SwingPulse;
			}
		}

		// send out a group of fireballs in a spiral pattern
		public void DoSwingPulse()
		{
			SwingingTimer += Revolution * SwingSpeed * Delta;
			
			// continue updating position
			SetSwingPosition();
			
			// spawn fireballs
			for (int Proj = 0; Proj < MaxProjectiles; Proj++)
				SpawnFireball(Proj, ProjActions.SwingPulse, SwingDisappearDelay, false);
			
			ActionTimer = 0.0f;
			
			npc.alpha = 0;
			
			SwingCount = 0;
			SwingFireballs = 0;
			
			Action = Actions.SwingWaitDisappear;
		}

		// wait a moment before disappearing
		public void DoSwingWaitDisappear()
		{
			if (ActionTimer >= SwingDisappearDelay)
			{
				ActionTimer = 0.0f;
				Action = Actions.SwingDisappear;
			}
		}

		// disappear
		public void DoSwingDisappear()
		{
			SwingSpeed = MinSwingSpeed;
			
			SwingingTimer += Revolution * SwingSpeed * Delta;
			
			// continue updating position
			SetSwingPosition();
			
			// first cycle of ai
			if (FirstCycle)
			{
				Disappear(Actions.SetCrash);
			}
			// invisible
			else if (InvisSwing)
			{
				Action = Actions.SetCrash;
				InvisSwing = false;
			}
			else
			{
				// disappear and do invisible swing
				Disappear(Actions.AccelerateSwing);
				
				if (Action != Actions.SwingDisappear)
				{
					InvisSwing = true;
				}
			}
		}

		// disappear from sight
		public void Disappear(Actions NextAction)
		{
			int Alpha = (int)(ActionTimer / DisappearDuration * 255);
			
			npc.alpha = Alpha;
			Main.npc[PhantoonUpper].alpha = Alpha;
			Main.npc[PhantoonMid].alpha = Alpha;
			Main.npc[PhantoonLower].alpha = Alpha;
			
			if (ActionTimer >= DisappearDuration)
			{
				npc.alpha = 255;
				Main.npc[PhantoonUpper].alpha = 255;
				Main.npc[PhantoonMid].alpha = 255;
				Main.npc[PhantoonLower].alpha = 255;
				Action = NextAction;
			}
		}

		// set position for swinging
		public void SetSwingPosition()
		{
			npc.position.X = Summoner.position.X + (Summoner.width - npc.width) * 0.5f;
			npc.position.Y = Summoner.position.Y + (Summoner.height - npc.height) * 0.5f - TotalHeight;
			
			npc.position.X -= (float)Math.Sin(SwingingTimer) * SwingExtension;
			npc.position.Y += (float)Math.Sin(SwingingTimer * 2.0f) * SwingExtension * 0.35f;
		}

		// set initial position for crashing
		public void DoSetCrash()
		{
			int GridX = Main.rand.Next(MaxProjectiles);
			int GridY = Main.rand.Next(4);
			
			float GridSpread = (float)CrashExtension / MaxProjectiles * 2.0f;
			
			npc.position.X = Summoner.position.X + Summoner.width * 0.5f - CrashExtension - GridSpread * 0.5f + GridSpread * GridX;
			npc.position.Y = Summoner.position.Y + Summoner.height * 0.5f - TotalHeight * 0.75f - GridSpread * GridY;
			
			// create line of fireballs
			for (int Proj = 0; Proj < MaxProjectiles; Proj++)
			{
				// don't create a fireball at Phantoon's location
				if (Proj != GridX)
				{
					int ProjX = SpawnFireball(Proj, ProjActions.Crash, 0.0f, true);
					
					Main.npc[ProjX].position.X = npc.position.X + (npc.width - Main.npc[ProjX].width) * 0.5f -
												 (GridX - Proj) * GridSpread;
					
					Main.npc[ProjX].position.Y = npc.position.Y + (npc.height - Main.npc[ProjX].height) * 0.5f;
				}
			}
			
			npc.alpha = 0;
			Main.npc[PhantoonUpper].alpha = 0;
			Main.npc[PhantoonMid].alpha = 0;
			Main.npc[PhantoonLower].alpha = 0;
			
			Action = Actions.Crash;
			
			ActionTimer = 0.0f;
		}

		// Phantoon will appear in a semi-random position and drop a line of fireballs
		public void DoCrash()
		{
			int ProjectilesDropping = (int)(ActionTimer / CrashProjectileDropDelay);
			
			if (ActionTimer >= CrashDuration)
			{
				ActionTimer = 0.0f;
				CrashCount++;
				Action = Actions.CrashDisappear;
			}
			
			// Phantoon was hit, change to next action
			if (npc.justHit)
			{
				ActionTimer = 0.0f;
				CrashCount = MaxCrashCount;
				Action = Actions.CrashDisappear;
			}
		}

		// disappear after crash
		public void DoCrashDisappear()
		{
			if (CrashCount >= MaxCrashCount)
			{
				Disappear(Actions.EndCrash);
			}
			else
			{
				Disappear(Actions.SetCrash);
			}
		}

		// finished crashing
		public void DoEndCrash()
		{
			CrashCount = 0;
			ActionTimer = 0.0f;
			FirstCycle = false;
			WhipCount = 0;
			//Disappear(Actions.SpawnAppear);
			Action = Actions.SpawnAppear;
			//Action = Actions.WaitWhip;
		}

		// start whip when high damage is dealt
		public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
		{
			if(damage >= 150)
			{
				CrashCount = 0;
				
				ActionTimer = 0.0f;
				
				Action = Actions.WaitWhip;
				
				npc.alpha = 255;
			}
			return true;
		}
			public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
		{
			if(damage >= 150)
			{
				CrashCount = 0;
				
				ActionTimer = 0.0f;
				
				Action = Actions.WaitWhip;
				
				npc.alpha = 255;
			}
		}

		// short waiting period after recieving high damage and before whipping
		public void DoWaitWhip()
		{
			if (ActionTimer >= WaitWhipDelay)
			{
				npc.position.X = Summoner.position.X + (Summoner.width - npc.width) * 0.5f;
				npc.position.Y = Summoner.position.Y + (Summoner.height - npc.height) * 0.5f - TotalHeight;
				
				ActionTimer = 0.0f;
				Action = Actions.WhipAppear;
			}
		}

		// Phantoon will appear at the middle top of the fighting area
		// and will fling a curved line of fireballs across the fighting area
		public void DoSetWhip()
		{
			npc.position.X = Summoner.position.X + (Summoner.width - npc.width) * 0.5f;
			npc.position.Y = Summoner.position.Y + (Summoner.height - npc.height) * 0.5f - TotalHeight;
			
			float MidX = npc.position.X + npc.width * 0.5f;
			float MidY = npc.position.Y + npc.height * 0.5f;
			
			float HalfHeight = TotalHeight * 0.5f;
			
			float Start = -HalfHeight;
			float Addition = TotalHeight / MaxProjectiles;
			
			// create chain of fireballs
			for (int Proj = 0; Proj < MaxProjectiles; Proj++)
			{
				int ProjX = SpawnFireball(Proj, ProjActions.Whip, WhipDuration, false);
				
				Main.npc[ProjX].position.X = MidX - Main.npc[ProjX].width * 0.5f;
				Main.npc[ProjX].position.Y = MidY - (Main.npc[ProjX].height * (1.0f + Proj * 0.5f)) * 0.5f;
			}
			
			ActionTimer = 0.0f;
			
			Action = Actions.Whip;
		}

		// appear
		public void DoWhipAppear()
		{
			Appear(Actions.SetWhip);
			
			npc.alpha = 255;
		}

		// fling the chain of fireballs
		public void DoWhip()
		{
			// this whip is done
			if (ActionTimer >= WhipDuration)
			{
				WhipCount++;
				WhipDirection *= -1;
				
				// destroy whip
				for (int Proj = 0; Proj < ActiveProjectiles.Count; Proj++)
					if (ActiveProjectiles[Proj].Action == ProjActions.Whip)
					{
						DropProjectile(Proj);
						Proj--;
					}
				
				// finished whip action
				if (WhipCount >= MaxWhipCount)
				{
					ActionTimer = 0.0f;
					WhipCount = 0;
					Action = Actions.WhipDisappear;
					FirstCycle = false;
				}
				else
					Action = Actions.SetWhip;
			}
		}

		// disappear
		public void DoWhipDisappear()
		{
			Disappear(Actions.SpawnAppear);
			
			if (Action == Actions.SpawnAppear)
			{
				npc.position.X = Summoner.position.X + (Summoner.width - npc.width) * 0.5f;
				npc.position.Y = Summoner.position.Y + (Summoner.height - npc.height) * 0.5f - TotalHeight;
			}
		}

		// disappear before despawning
		public void DoDespawnDisappear()
		{
			Disappear(Actions.Despawn);
			npc.alpha = 255;
		}

		// destroy all fireballs and despawn
		public void DoDespawn()
		{
			for (int Proj = 0; Proj < ActiveProjectiles.Count; Proj++)
				if (!ActiveProjectiles[Proj].DestroyOnTile)
				{
					DropProjectile(Proj);
					Proj--;
				}
			
			Main.npc[PhantoonUpper].active = false;
			Main.npc[PhantoonMid].active = false;
			Main.npc[PhantoonLower].active = false;
			Main.npc[ID2].active = false;
			for(int i = 0; i < Main.npc.Length; i++)
			{
				if(Main.npc[i].active && Main.npc[i].type == mod.NPCType("FireBall"))
				{
					Main.npc[i].life = 0;
				}
			}
			npc.active = false;
		}
	}
}
