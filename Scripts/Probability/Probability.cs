using System;
using UnityEngine;

namespace HegaCore
{
    public static class Probability
    {
        private static readonly PseudoProbability _probability;

        static Probability()
        {
            _probability = new PseudoProbability(new PRDMath(), new PRDRandom());
        }

        public static void Generate()
            => _probability.Generate();

        public static bool DoIHaveLuck(float chance)
            => _probability.DoIHaveLuck(chance);

        public static bool DoIHaveLuckInHundred(float chance)
            => _probability.DoIHaveLuckInHundred(chance);

        public static bool DoIHaveLuckInHundred(int chance)
            => _probability.DoIHaveLuckInHundred(chance);

        public static bool DoIHaveLuck(float chance, int n, int n_0, out int n_1)
            => _probability.DoIHaveLuck(chance, n, n_0, out n_1);

        public static bool DoIHaveLuckInHundred(int chance, int n, int n_0, out int n_1)
            => _probability.DoIHaveLuckInHundred(chance, n, n_0, out n_1);

        public static bool DoIHaveLuckInHundred(float chance, int n, int n_0, out int n_1)
            => _probability.DoIHaveLuckInHundred(chance, n, n_0, out n_1);

        public static bool DoIHaveLuckInThousand(int chance, int n, int n_0, out int n_1)
            => _probability.DoIHaveLuckInThousand(chance, n, n_0, out n_1);

        public static bool DoIHaveLuckInThousand(float chance, int n, int n_0, out int n_1)
            => _probability.DoIHaveLuckInThousand(chance, n, n_0, out n_1);

        private readonly struct PRDMath : PseudoProbability.IMath
        {
            public float Abs(float f)
                => Mathf.Abs(f);

            public int CeilToInt(float f)
                => Mathf.CeilToInt(f);

            public float Clamp(float value, float min, float max)
                => Mathf.Clamp(value, min, max);

            public int Clamp(int value, int min, int max)
                => Mathf.Clamp(value, min, max);

            public float Min(float a, float b)
                => Mathf.Min(a, b);

            public int RoundToInt(float f)
                => Mathf.RoundToInt(f);
        }

        private readonly struct PRDRandom : PseudoProbability.IRandom
        {
            public float Value => UnityEngine.Random.value;

            public float Range(float min, float max)
                => UnityEngine.Random.Range(min, max);
        }
    }
}