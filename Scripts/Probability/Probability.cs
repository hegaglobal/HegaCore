using System.Collections.Generic;
using UnityEngine;

namespace HegaCore
{
    public static class Probability
    {
        private static readonly Dictionary<int, float> _cValues;

        static Probability()
        {
            _cValues = new Dictionary<int, float>();
        }

        public static void Initialize()
        {
            _cValues.Clear();

            for (var p = 1; p < 1000; p++)
            {
                var c = PRD.GetCFromP(p / 1000f);
                _cValues.Add(p, c * 100f);
            }
        }

        public static bool DoIHaveASimpleLuck(float chance)
        {
            var r = Random.Range(0f, 100f);
            return r <= chance;
        }

        public static bool DoIHaveASimpleLuck(int chance)
        {
            var r = Random.Range(0, 100);
            return r <= chance;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="chance">In the range of [0, 1]</param>
        /// <param name="n"></param>
        /// <param name="n_0">Number of failed attempts</param>
        /// <param name="n_1">n_0 if succeeded or n+1 if failed</param>
        /// <returns></returns>
        public static bool DoIHaveAPercentLuck(float chance, int n, int n_0, out int n_1)
        {
            var thousand = Mathf.RoundToInt(Mathf.Clamp(chance, 0f, 1f) * 1000);

            if (thousand <= 0)
                thousand = 1;

            return DoIHaveLuck(thousand, n, n_0, out n_1);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="chance">In the range of [1, 100]</param>
        /// <param name="n"></param>
        /// <param name="n_0">Number of failed attempts</param>
        /// <param name="n_1">n_0 if succeeded or n+1 if failed</param>
        /// <returns></returns>
        public static bool DoIHaveAHundredLuck(int chance, int n, int n_0, out int n_1)
        {
            var thousand = Mathf.Clamp(chance, 1, 100) * 10;
            return DoIHaveLuck(thousand, n, n_0, out n_1);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="chance">In the range of [1, 100]</param>
        /// <param name="n"></param>
        /// <param name="n_0">Number of failed attempts</param>
        /// <param name="n_1">n_0 if succeeded or n+1 if failed</param>
        /// <returns></returns>
        public static bool DoIHaveAHundredLuck(float chance, int n, int n_0, out int n_1)
        {
            var thousand = Mathf.RoundToInt(Mathf.Clamp(chance, 1f, 100f) * 10);
            return DoIHaveLuck(thousand, n, n_0, out n_1);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="chance">In the range of [1, 1000]</param>
        /// <param name="n"></param>
        /// <param name="n_0">Number of failed attempts</param>
        /// <param name="n_1">n_0 if succeeded or n+1 if failed</param>
        /// <returns></returns>
        public static bool DoIHaveAThousandLuck(int chance, int n, int n_0, out int n_1)
        {
            var thousand = Mathf.Clamp(chance, 1, 1000);
            return DoIHaveLuck(thousand, n, n_0, out n_1);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="chance">In the range of [1, 1000]</param>
        /// <param name="n"></param>
        /// <param name="n_0">Number of failed attempts</param>
        /// <param name="n_1">n_0 if succeeded or n+1 if failed</param>
        /// <returns></returns>
        public static bool DoIHaveAThousandLuck(float chance, int n, int n_0, out int n_1)
        {
            var thousand = Mathf.RoundToInt(Mathf.Clamp(chance, 1f, 1000f));
            return DoIHaveLuck(thousand, n, n_0, out n_1);
        }

        private static bool DoIHaveLuck(int thousand, int n, int n_0, out int n_1)
        {
            var c = _cValues[thousand];
            var p = c * n;
            var r = Random.value * 100f;

            if (r > p)
            {
                n_1 = n + 1;
                return false;
            }

            n_1 = n_0;
            return true;
        }
    }
}