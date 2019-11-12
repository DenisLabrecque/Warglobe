﻿using System;



namespace Mathematics

{

   /// <summary>
   /// A (P)roportional, (I)ntegral, (D)erivative Controller
   /// </summary>
   /// <remarks>
   /// The controller should be able to control any process with a
   /// measureable value, a known ideal value and an input to the
   /// process that will affect the measured value.
   /// </remarks>
   /// <see cref="https://en.wikipedia.org/wiki/PID_controller"/>

   public sealed class PIDController
   {
      private double m_ProcessVariable = 0;



      public PIDController(double GainProportional, double GainIntegral, double GainDerivative, double OutputMax, double OutputMin)

      {

         this.GainDerivative = GainDerivative;

         this.GainIntegral = GainIntegral;

         this.GainProportional = GainProportional;

         this.OutputMax = OutputMax;

         this.OutputMin = OutputMin;

      }



      /// <summary>
      /// The controller output
      /// </summary>
      /// <param name="timeSinceLastUpdate">timespan of the elapsed time
      /// since the previous time that ControlVariable was called</param>
      /// <returns>Value of the variable that needs to be controlled</returns>
      public double ControlVariable(float timeSinceLastUpdate)
      {
         double error = SetPoint - ProcessVariable;

         // integral term calculation
         IntegralTerm += (GainIntegral * error * timeSinceLastUpdate);
         IntegralTerm = Clamp(IntegralTerm);

         // derivative term calculation
         double dInput = m_ProcessVariable - ProcessVariableLast;
         double derivativeTerm = GainDerivative * (dInput / timeSinceLastUpdate);

         // proportional term calculation
         double proportionalTerm = GainProportional * error;

         double output = proportionalTerm + IntegralTerm - derivativeTerm;

         output = Clamp(output);

         return output;
      }



      /// <summary>

      /// The derivative term is proportional to the rate of

      /// change of the error

      /// </summary>

      public double GainDerivative { get; set; }



      /// <summary>

      /// The integral term is proportional to both the magnitude

      /// of the error and the duration of the error

      /// </summary>

      public double GainIntegral { get; set; }



      /// <summary>

      /// The proportional term produces an output value that

      /// is proportional to the current error value

      /// </summary>

      /// <remarks>

      /// Tuning theory and industrial practice indicate that the

      /// proportional term should contribute the bulk of the output change.

      /// </remarks>

      public double GainProportional { get; set; }



      /// <summary>

      /// The max output value the control device can accept.

      /// </summary>

      public double OutputMax { get; private set; }



      /// <summary>

      /// The minimum ouput value the control device can accept.

      /// </summary>

      public double OutputMin { get; private set; }



      /// <summary>

      /// Adjustment made by considering the accumulated error over time

      /// </summary>

      /// <remarks>

      /// An alternative formulation of the integral action, is the

      /// proportional-summation-difference used in discrete-time systems

      /// </remarks>

      public double IntegralTerm { get; private set; }





      /// <summary>

      /// The current value

      /// </summary>

      public double ProcessVariable {

         get { return m_ProcessVariable; }

         set {

            ProcessVariableLast = m_ProcessVariable;

            m_ProcessVariable = value;

         }

      }



      /// <summary>

      /// The last reported value (used to calculate the rate of change)

      /// </summary>

      public double ProcessVariableLast { get; private set; }



      /// <summary>

      /// The desired value

      /// </summary>

      public double SetPoint { get; set; }



      /// <summary>

      /// Limit a variable to the set OutputMax and OutputMin properties

      /// </summary>

      /// <returns>

      /// A value that is between the OutputMax and OutputMin properties

      /// </returns>

      /// <remarks>

      /// Inspiration from http://stackoverflow.com/questions/3176602/how-to-force-a-number-to-be-in-a-range-in-c

      /// </remarks>

      private double Clamp(double variableToClamp)

      {

         if(variableToClamp <= OutputMin)
         { return OutputMin; }

         if(variableToClamp >= OutputMax)
         { return OutputMax; }

         return variableToClamp;

      }

   }

}