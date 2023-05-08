
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;

namespace Framework.Helper
{
    /// <summary>
    ///     Collection of static helper methods to do with randomness.
    /// </summary>
    public static class RandomHelper
    {
        #region Static fields and constants
        
        private static int _seed;

        private static readonly char[] Symbols =
        {
            '!', '"', '#', '$', '%', '&', '\'', '(', ')', '*', '+', ',', '-', '.', '/', ':',
            ';', '<', '=', '>', '?', '@', '[', '\\', ']', '^', '_', '`', '{', '|', '}', '~'
        };

        // ReSharper disable once InconsistentNaming
        private const float PI_2 = Mathf.PI * 2f;

        #endregion
        
        #region Static properties
        
        /// <summary>
        ///     Random number generator used by the helper methods.
        /// </summary>
        [PublicAPI, NotNull]
        // ReSharper disable once InconsistentNaming
        public static System.Random RNG { get; private set; }

        /// <summary>
        ///     Seed used by the random number generator instance.
        ///     Setting a new seed will initialise a new random number generator.
        /// </summary>
        [PublicAPI]
        public static int Seed
        {
            get => _seed;
            set
            {
                _seed = value;
                RNG = new System.Random(_seed);
            }
        }
        
        #endregion

        #region Static constructor

        static RandomHelper()
        {
            _seed = Environment.TickCount;
            RNG = new System.Random(_seed);
        }

        #endregion

        #region Static methods

        /// <summary>
        ///     Retrieve a random bool value. By 50% chance, this will return true.
        /// </summary>
        /// <returns></returns>
        [PublicAPI, Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool NextBool()
        {
            return RNG.Next(100) >= 50;
        }

        /// <summary>
        ///     Retrieve a random lowercase letter.
        /// </summary>
        /// <returns></returns>
        [PublicAPI, Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static char NextLetter()
        {
            return (char)Range(97, 123);
        }

        /// <summary>
        ///     Retrieve a random symbol.
        /// </summary>
        /// <returns></returns>
        [PublicAPI, Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static char NextSymbol()
        {
            return Choose(Symbols);
        }

        /// <summary>
        ///     Retrieve a random digit.
        /// </summary>
        /// <returns></returns>
        [PublicAPI, Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int NextDigit()
        {
            return RNG.Next(10);
        }

        /// <summary>
        ///     Retrieve a random float value between 0 [inclusive] and 1 [exclusive].
        /// </summary>
        /// <returns></returns>
        [PublicAPI, Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float NextFloat()
        {
            return (float)RNG.NextDouble();
        }

        /// <summary>
        ///     Retrieve a random float between 0 [inclusive] and a max value [exclusive].
        /// </summary>
        /// <param name="max"></param>
        /// <returns></returns>
        [PublicAPI, Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float NextFloat(float max)
        {
            return NextFloat() * max;
        }

        /// <summary>
        ///     Retrieve a random double value between 0 [inclusive] and 1 [exclusive].
        /// </summary>
        /// <returns></returns>
        [PublicAPI, Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double NextDouble()
        {
            return RNG.NextDouble();
        }

        /// <summary>
        ///     Retrieve a random double between 0 [inclusive] and a max value [exclusive].
        /// </summary>
        /// <param name="max"></param>
        /// <returns></returns>
        [PublicAPI, Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double NextDouble(double max)
        {
            return NextDouble() * max;
        }

        /// <summary>
        ///     Retrieve a random int between 0 [inclusive] and a max value [exclusive].
        /// </summary>
        /// <param name="max"></param>
        /// <returns></returns>
        [PublicAPI, Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int NextInt(int max)
        {
            return RNG.Next(max);
        }

        /// <summary>
        ///     Retrieve a random angle between 0 and 2 PI (radians).
        /// </summary>
        /// <returns></returns>
        [PublicAPI, Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float NextAngleRadians()
        {
            return NextFloat(PI_2);
        }

        /// <summary>
        ///     Retrieve a random angle between 0 and 360 (degrees).
        /// </summary>
        /// <returns></returns>
        [PublicAPI, Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float NextAngleDegrees()
        {
            return NextFloat(360f);
        }

        /// <summary>
        ///     Retrieve a colour with random component values (rgb).
        /// </summary>
        /// <returns></returns>
        [PublicAPI, Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color NextColour()
        {
            return new Color(NextFloat(), NextFloat(), NextFloat());
        }

        /// <summary>
        ///     Retrieve a random unit vector.
        /// </summary>
        /// <param name="magnitude"></param>
        /// <returns></returns>
        [PublicAPI, Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 NextVector2(float magnitude = 1f)
        {
            return MathsHelper.GetDirection(NextAngleRadians() * Mathf.Rad2Deg) * magnitude;
        }

        /// <summary>
        ///     Retrieve a random unit vector.
        /// </summary>
        /// <param name="magnitude"></param>
        /// <returns></returns>
        [PublicAPI, Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 NextVector3(float magnitude = 1f)
        {
            return MathsHelper.GetDirection(NextAngleRadians() * Mathf.Rad2Deg) * magnitude;
        }

        /// <summary>
        ///     Retrieve a random int between a min value [inclusive] and a max value [exclusive].
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        [PublicAPI, Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Range(int min, int max)
        {
            return min + NextInt(max - min);
        }
        
        /// <summary>
        ///     Retrieve a random float between a min value [inclusive] and a max value [exclusive].
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        [PublicAPI, Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Range(float min, float max)
        {
            return min + NextFloat(max - min);
        }

        /// <summary>
        ///     Retrieve a random double between a min value [inclusive] and a max value [exclusive].
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        [PublicAPI, Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Range(double min, double max)
        {
            return min + NextDouble(max - min);
        }

        /// <summary>
        ///     Retrieve a random vector with components between a min value [inclusive] and a max value [exclusive].
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        [PublicAPI, Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Range(Vector2 min, Vector2 max)
        {
            return min + new Vector2(NextFloat(max.x - min.x), NextFloat(max.y - min.y));
        }

        /// <summary>
        ///     Retrieve a random vector with components between a min value [inclusive] and a max value [exclusive].
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        [PublicAPI, Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Range(Vector3 min, Vector3 max)
        {
            return min + new Vector3(NextFloat(max.x - min.x), NextFloat(max.y - min.y), NextFloat(max.z - min.z));
        }

        /// <summary>
        ///     Retrieve a random float between -1 and 1.
        /// </summary>
        /// <returns></returns>
        [PublicAPI, Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float MinusOneToOne()
        {
            return NextFloat(2f) - 1f;
        }

        /// <summary>
        ///     Retrieve a random integer, either -1 or 1.
        /// </summary>
        /// <returns></returns>
        [PublicAPI, Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int MinusOneOrOne()
        {
            return NextBool() ? -1 : 1;
        }

        /// <summary>
        ///     Roll a random chance.
        /// </summary>
        /// <param name="percent">0.0 - 1.0.</param>
        /// <returns></returns>
        [PublicAPI, Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Chance(float percent)
        {
            return NextFloat() < percent;
        }

        /// <summary>
        ///     Roll a random chance.
        /// </summary>
        /// <param name="percent">0.0 - 1.0.</param>
        /// <returns></returns>
        [PublicAPI, Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Chance(double percent)
        {
            return NextDouble() < percent;
        }

        /// <summary>
        ///     Roll a random chance.
        /// </summary>
        /// <param name="percent">0 - 100.</param>
        /// <returns></returns>
        [PublicAPI, Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Chance(int percent)
        {
            return NextInt(100) < percent;
        }

        /// <summary>
        ///     Retrieve a random element from an enumerable, or default if empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        [PublicAPI, Pure, CanBeNull, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Choose<T>(this IEnumerable<T> enumerable)
        {
            T current = default;
            var count = 0;
            foreach (var element in enumerable)
            {
                count += 1;
                if (NextInt(count) == 0)
                {
                    current = element;
                }
            }

            return current;
        }
        
        /// <summary>
        /// Random a element from List
        /// </summary>
        [PublicAPI, Pure, CanBeNull, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Next<T>(this List<T> _List)
        {
            return _List[NextInt(_List.Count)];
        }
        
        /// <summary>
        /// Random a element from Array
        /// </summary>
        [PublicAPI, Pure, CanBeNull, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Next<T>(this T[] _Array)
        {
            return _Array[NextInt(_Array.Length)];
        }

        #endregion
    }
}