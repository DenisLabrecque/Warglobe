namespace DGL.Math {
   /// <summary>
   /// Store a set of values and retrieve their average.
   /// </summary>
   public class RunningAverage<T>
   {
      private T[] _values;
      private bool _firstRun = true;

      public RunningAverage(uint length)
      {
         _values = new T[length];
      }

      /// <summary>
      /// Add a value to history. The oldest value will be discarded.
      /// </summary>
      /// <param name="value">New value.</param>
      public void Add(T value)
      {
         if(_firstRun)
            // Fill with the same starting value
            for (int i = 0; i < _values.Length; i++)
               _values[i] = value;
         else
         {
            ShiftRight();
            _values[0] = value;
         }
      }

      ///// <summary>
      ///// Average of all values in the history.
      ///// </summary>
      //public T Average
      //{
      //   get {
      //      dynamic total = 0;
      //      foreach (dynamic element in _values)
      //         total += element;
      //      return total / _values.Length;
      //   }
      //}

      /// <summary>
      /// Move all values right, discarding the last value in the list, and effectively duplicating the first.
      /// </summary>
      private void ShiftRight()
      {
         for (int i = _values.Length - 1; i > 0; i--)
            _values[i] = _values[i - 1];
      }
   }
}