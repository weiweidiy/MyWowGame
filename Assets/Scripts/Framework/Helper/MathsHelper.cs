
using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;

namespace Framework.Helper
{
    /// <summary>
    ///     Collection of static maths helper methods.
    /// </summary>
    public static class MathsHelper
    {
        #region Static methods

        /// <summary>
        ///     Shift the start value towards the end value without exceeding.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="shift"></param>
        /// <returns></returns>
        [PublicAPI, Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Approach(float start, float end, float shift)
        {
            return start < end ? Mathf.Min(start + shift, end) : Mathf.Max(start - shift, end);
        }

        /// <summary>
        ///     Shift the start angle towards the end angle (degrees).
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="shift"></param>
        /// <returns></returns>
        [PublicAPI, Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ApproachAngle(float start, float end, float shift)
        {
            var deltaAngle = Mathf.DeltaAngle(start, end);
            if (-shift < deltaAngle && deltaAngle < shift)
            {
                return end;
            }

            return Mathf.Repeat(Approach(start, start + deltaAngle, shift), 360f);
        }

        /// <summary>
        ///     Shift the start value towards zero without exceeding.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="shift"></param>
        /// <returns></returns>
        [PublicAPI, Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Reduce(float start, float shift)
        {
            return Approach(start, 0f, shift);
        }

        /// <summary>
        ///     Shift the start angle towards zero (degrees).
        /// </summary>
        /// <param name="start"></param>
        /// <param name="shift"></param>
        /// <returns></returns>
        [PublicAPI, Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ReduceAngle(float start, float shift)
        {
            return ApproachAngle(start, 0f, shift);
        }

        /// <summary>
        ///     Pulse between min and max using a sine wave.
        /// </summary>
        /// <param name="time">Increasing value (e.g. Time.time).</param>
        /// <param name="frequency">How many min..max..min per second (lower is slower).</param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        [PublicAPI, Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Pulse(float time, float frequency, float min, float max)
        {
            var half = (max - min) * 0.5f;
            return min + half + Mathf.Sin(2f * Mathf.PI * frequency * time) * half;
        }

        /// <summary>
        ///     Alternate between true and false using a sine wave.
        /// </summary>
        /// <param name="time">Increasing value (e.g. Time.time).</param>
        /// <param name="frequency">How many false..true..false per second (lower is slower).</param>
        /// <returns></returns>
        [PublicAPI, Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool FlipFlop(float time, float frequency)
        {
            return Pulse(time, frequency, 0f, 1f) >= 0.5f;
        }

        /// <summary>
        ///     Alternate between true and false using a sine wave, only returning true when it's at its peak.
        /// </summary>
        /// <param name="time">Increasing value (e.g. Time.time).</param>
        /// <param name="frequency">Blip speed (lower is slower).</param>
        /// <returns></returns>
        [PublicAPI, Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Blip(float time, float frequency)
        {
            return Math.Abs(Pulse(time, frequency, 0f, 1f) - 1f) < Mathf.Epsilon;
        }

        /// <summary>
        ///     Sign method that returns -1, 0 or +1 depending on the sign of the given value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [PublicAPI, Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Sign(int value)
        {
            return value < 0 ? -1 : value > 0 ? 1 : 0;
        }
        
        /// <summary>
        ///     Sign method that returns -1, 0 or +1 depending on the sign of a given value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [PublicAPI, Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Sign(float value)
        {
            return value < 0f ? -1f : value > 0f ? 1f : 0f;
        }

        /// <summary>
        ///     Retrieve the sign of a value if it's over the threshold, otherwise return zero.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        [PublicAPI, Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SignThreshold(int value, int threshold)
        {
            return Mathf.Abs(value) >= threshold ? Sign(value) : 0;
        }
        
        /// <summary>
        ///     Retrieve the sign of a value if it's over the threshold, otherwise return zero.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        [PublicAPI, Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SignThreshold(float value, float threshold)
        {
            return Mathf.Abs(value) >= threshold ? Sign(value) : 0f;
        }

        /// <summary>
        ///     Get a vector with its original components passed through Sign().
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [PublicAPI, Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Sign(Vector2 value)
        {
            return new Vector2(Sign(value.x), Sign(value.y));
        }

        /// <summary>
        ///     Get a vector with its original components passed through Sign().
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [PublicAPI, Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Sign(Vector3 value)
        {
            return new Vector3(Sign(value.x), Sign(value.y), Sign(value.z));
        }

        /// <summary>
        ///     Check if a value is between min and max (inclusive).
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        [PublicAPI, Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Between(float value, float min, float max)
        {
            return value >= min && value <= max;
        }

        /// <summary>
        ///     Check if a value is between min and max (inclusive).
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        [PublicAPI, Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Between(int value, int min, int max)
        {
            return value >= min && value <= max;
        }

        /// <summary>
        ///     Check if a value is even.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [PublicAPI, Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEven(int value)
        {
            return value % 2 == 0;
        }

        /// <summary>
        ///     Check if a value is odd.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [PublicAPI, Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOdd(int value)
        {
            return !IsEven(value);
        }

        /// <summary>
        ///     Map a value from some arbitrary range to the 0 to 1 range.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        [PublicAPI, Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Map01(float value, float min, float max)
        {
            return (value - min) * 1f / (max - min);
        }

        /// <summary>
        ///     Map a value from some arbitrary range to the 1 to 0 range.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        [PublicAPI, Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Map10(float value, float min, float max)
        {
            return 1f - Map01(value, min, max);
        }

        /// <summary>
        ///     Map a value from one range to another range.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="from1"></param>
        /// <param name="from2"></param>
        /// <param name="to1"></param>
        /// <param name="to2"></param>
        /// <returns></returns>
        [PublicAPI, Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Map(float value, float from1, float from2, float to1, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        /// <summary>
        ///     Convert from an angle (degrees) to a 2d direction.
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns></returns>
        [PublicAPI, Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 GetDirection(float degrees)
        {
            return new Vector2(Mathf.Cos(degrees * Mathf.Deg2Rad), Mathf.Sin(degrees * Mathf.Deg2Rad));
        }

        /// <summary>
        ///     Convert from a 2d direction to an angle (degrees).
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        [PublicAPI, Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetAngle(Vector2 direction)
        {
            return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }

        /// <summary>
        ///     Retrieve the angle (degrees) between two 2d directions.
        /// </summary>
        /// <param name="directionA"></param>
        /// <param name="directionB"></param>
        /// <returns></returns>
        [PublicAPI, Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetAngleBetween(Vector2 directionA, Vector2 directionB)
        {
            return GetAngle(directionA - directionB);
        }

        /// <summary>
        ///     Check if a target 2d position is within a cone projected from the source 2d position.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="coneDirection"></param>
        /// <param name="degrees"></param>
        /// <returns></returns>
        [PublicAPI, Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WithinAngle(Vector2 source, Vector2 target, Vector2 coneDirection, float degrees)
        {
            var direction = (target - source).normalized;
            var cosAngle = Vector2.Dot(direction, coneDirection);
            var angle = Mathf.Acos(cosAngle) * Mathf.Rad2Deg;
            return angle < degrees;
        }

        /// <summary>
        ///     Rotate a vector by an angle (degrees).
        /// </summary>
        /// <param name="value"></param>
        /// <param name="degrees"></param>
        /// <returns></returns>
        [PublicAPI, Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 RotateVector(Vector2 value, float degrees)
        {
            return GetDirection(GetAngle(value) + degrees);
        }

        /// <summary>
        ///     Rotate a vector around a pivot point by an angle (degrees).
        /// </summary>
        /// <param name="value"></param>
        /// <param name="degrees"></param>
        /// <param name="pivot"></param>
        /// <returns></returns>
        [PublicAPI, Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 RotateVector(Vector2 value, float degrees, Vector2 pivot)
        {
            var direction = value - pivot;
            direction = Quaternion.Euler(0f, 0f, degrees) * direction;
            return direction + pivot;
        }

        [PublicAPI, Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Clamp(float value, float one, float two)
        {
            if (one > two)
                return Mathf.Clamp(value, two, one);
            return Mathf.Clamp(value, one, two);
        }
        #endregion
    }
}