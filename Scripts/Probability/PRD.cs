using UnityEngine;

namespace HegaCore
{
    public static class PRD
    {
        public static float GetCFromP(float p)
        {
            var upperC = p;
            var lowerC = 0f;
            var p2 = 1f;
            float midC;
            float p1;

            while (true)
            {
                midC = (upperC + lowerC) / 2f;
                p1 = GetPFromC(midC);

                if (Mathf.Abs(p1 - p2) <= 0f)
                    break;

                if (p1 > p)
                    upperC = midC;
                else
                    lowerC = midC;

                p2 = p1;
            }

            return midC;
        }

        public static float GetPFromC(float c)
        {
            var pProcByN = 0f;
            var sumNpProcOnN = 0f;

            var maxFails = Mathf.CeilToInt(1f / c);

            for (var N = 1; N <= maxFails; ++N)
            {
                var pProcOnN = Mathf.Min(1f, N * c) * (1f - pProcByN);
                pProcByN += pProcOnN;
                sumNpProcOnN += N * pProcOnN;
            }

            return 1f / sumNpProcOnN;
        }
    }
}