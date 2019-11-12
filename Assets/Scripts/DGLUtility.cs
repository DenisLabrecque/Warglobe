namespace DGL.Math
{
   public enum PercentMode { AnyPercent, ClampPositive, ClampNegative, Clamp0To1, ClampNegative1To1, ClampNegative1To0 }

   /// <summary>
   /// Common methods I notice I use often.
   /// 
   /// Denis Labrecque
   /// May 2019
   /// </summary>
   public static class Utility
   {
      /// <summary>
      /// Return the percent the fraction is of the base value.
      /// </summary>
      /// <returns>When the base value is zero, returns zero.</returns>
      public static float Percent(float fraction, float baseValue, PercentMode percentMode = PercentMode.AnyPercent)
      {
         if(baseValue == 0f)
            return 0f;
         else
         {
            float percentage = fraction / baseValue;

            switch(percentMode)
            {
               case PercentMode.ClampPositive:
                  return ClampPositive(percentage);
               case PercentMode.ClampNegative:
                  return ClampNegative(percentage);
               case PercentMode.Clamp0To1:
                  return Clamp0To1(percentage);
               case PercentMode.ClampNegative1To1:
                  return ClampNegative1To1(percentage);
               case PercentMode.ClampNegative1To0:
                  return ClampNegative1To0(percentage);
               default:
                  // Any percent
                  return percentage;
            }
         }
      }

      public static float Clamp(float value, float min, float max)
      {
         if(value < min)
            return min;
         else if(value > max)
            return max;
         else
            return value;
      }

      public static float Clamp0To1(float value)
      {
         return Clamp(value, 0f, 1f);
      }

      public static float ClampNegative1To1(float value)
      {
         return Clamp(value, -1f, 1f);
      }

      public static float ClampNegative1To0(float value)
      {
         return Clamp(value, -1f, 0f);
      }

      public static float ClampPositive(float value)
      {
         return Clamp(value, 0f, float.PositiveInfinity);
      }

      public static float ClampNegative(float value)
      {
         return Clamp(value, float.NegativeInfinity, 0f);
      }
   }
}