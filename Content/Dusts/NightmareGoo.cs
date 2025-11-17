using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Dusts
{
	public abstract class NightmareGoo : ModDust
	{
		public override bool Update(Dust dust)
		{
			dust.position += dust.velocity;
			dust.velocity.X = 0;
			if (dust.velocity.Y < 10)
			{
				dust.velocity.Y += 0.2f;
			}
			dust.rotation = 0;
			dust.alpha += 2;
			if (dust.scale >= 255)
			{
				dust.active = false;
			}
			return false;
		}
	}
	public class NightmareGoo1 : NightmareGoo
	{
		public override void OnSpawn(Dust dust)
		{
			dust.frame = new Rectangle(0, 0, 8, 16);
		}
	}
	public class NightmareGoo2 : NightmareGoo
	{
		public override void OnSpawn(Dust dust)
		{
			dust.frame = new Rectangle(0, 0, 14, 20);
		}
	}
	public class NightmareGoo3 : NightmareGoo
	{
		public override void OnSpawn(Dust dust)
		{
			dust.frame = new Rectangle(0, 0, 10, 12);
		}
	}
	public class NightmareGoo4 : NightmareGoo
	{
		public override void OnSpawn(Dust dust)
		{
			dust.frame = new Rectangle(0, 0, 12, 18);
		}
	}
	public class NightmareGoo5 : NightmareGoo
	{
		public override void OnSpawn(Dust dust)
		{
			dust.frame = new Rectangle(0, 0, 12, 18);
		}
	}
}
