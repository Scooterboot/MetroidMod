using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using MetroidModPorted.Common.Players;

namespace MetroidModPorted.Common.GlobalItems
{
	public class MGlobalItem : GlobalItem
	{
		public AddonType AddonType = AddonType.None;
		public int statMissiles = 5;
		public int maxMissiles = 5;

		public int numSeekerTargets = 0;
		public int[] seekerTarget = new int[5];
		public int seekerCharge = 0;
		public static int seekerMaxCharge = 25;

		public Texture2D itemTexture;

		public override bool InstancePerEntity
		{
			get { return true; }
		}
		/*public override bool CloneNewInstances
		{
			get { return true; }
		}*/

		public override GlobalItem Clone(Item item, Item itemClone)
		{
			MGlobalItem other = (MGlobalItem)MemberwiseClone();
			other.maxMissiles = maxMissiles;
			other.statMissiles = statMissiles;

			return other;
		}
	}
	public class Grab : GlobalItem
	{
		public override void GrabRange(Item item, Player player, ref int grabRange)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			grabRange += (int)(mp.statCharge * 1.6f);
		}
	}
}
