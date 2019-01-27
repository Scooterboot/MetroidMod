using System;

using Microsoft.Xna.Framework;

namespace MetroidMod
{
	//imported from Example Mod because it's useful
	public struct Angle
	{
		public float Value;

		public Angle(float angle)
		{
			Value = angle;
			float remainder = Value % (2f * (float)Math.PI);
			float rotations = Value - remainder;
			Value -= rotations;
			if (Value < 0f)
			{
				Value += 2f * (float)Math.PI;
			}
		}

		public static Angle operator +(Angle a1, Angle a2)
		{
			return new Angle(a1.Value + a2.Value);
		}

		public static Angle operator -(Angle a1, Angle a2)
		{
			return new Angle(a1.Value - a2.Value);
		}

		public Angle Opposite()
		{
			return new Angle(Value + (float)Math.PI);
		}

		public bool ClockwiseFrom(Angle other)
		{
			if (other.Value >= (float)Math.PI)
			{
				return this.Value < other.Value && this.Value >= other.Opposite().Value;
			}
			return this.Value < other.Value || this.Value >= other.Opposite().Value;
		}

		public bool Between(Angle cLimit, Angle ccLimit)
		{
			if (cLimit.Value < ccLimit.Value)
			{
				return this.Value >= cLimit.Value && this.Value <= ccLimit.Value;
			}
			return this.Value >= cLimit.Value || this.Value <= ccLimit.Value;
		}
		
		public static float Vector2Angle(Vector2 Angle1, Vector2 Angle2, int dirX = 1, int dirY = 1, float mult = 1f, float min = 0f, float max = 0f)
		{
			float targetAngle = (float)Math.Atan2((Angle2.Y-Angle1.Y)*dirY,(Angle2.X-Angle1.X)*dirX);
			
			float angle = targetAngle * mult;
			if (min < 0f && angle < min)
			{
				angle = min;
			}
			if (max > 0f && angle > max)
			{
				angle = max;
			}
			return angle;
		}
		
		public static float AngleFlip(float angle, int dir)
		{
			float cos = (float)Math.Cos(angle);
			float sin = (float)Math.Sin(angle);
			return (float)Math.Atan2(sin,cos*dir);
		}
		
		public static float LerpArray(float value1, float[] value2, float amount)
		{
			float result = value1;
			for(int i = 0; i < value2.Length; i++)
			{
				if((i+1) >= amount)
				{
					float firstValue = value1;
					float secondValue = value2[i];
					if(i > 0)
					{
						firstValue = value2[i-1];
					}
					float amt = amount-i;
					result = firstValue + (secondValue-firstValue)*amt;
					break;
				}
			}
			return result;
		}
		
		public static double ConvertToRadians(double angle)
		{
			return (Math.PI / 180) * angle;
		}
		public static double ConvertToDegrees(double angle)
		{
			return angle * (180 / Math.PI);
		}
	}
}