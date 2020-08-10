using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace GameCode.Misc
{
    /// <summary>
    /// Code from user 'ChevyRay' 19/4/20
    /// https://forums.tigsource.com/index.php?topic=32178.0
    /// </summary>
    static class Coroutines
    {
        static List<IEnumerator> routines = new List<IEnumerator>();

        public static void Start(IEnumerator routine)
        {
            routines.Add(routine);
        }

        public static void StopAll()
        {
            routines.Clear();
        }

        public static void Update()
        {
            for (int i = 0; i < routines.Count; i++)
            {
                if (routines[i].Current is IEnumerator)
                    if (MoveNext((IEnumerator)routines[i].Current))
                        continue;
                if (!routines[i].MoveNext())
                    routines.RemoveAt(i--);
            }
        }

        static bool MoveNext(IEnumerator routine)
        {
            if (routine.Current is IEnumerator)
                if (MoveNext((IEnumerator)routine.Current))
                    return true;
            return routine.MoveNext();
        }

        public static int Count
        {
            get { return routines.Count; }
        }

        public static bool Running
        {
            get { return routines.Count > 0; }
        }

        public static IEnumerator WaitForSeconds(float time)
        {
            var watch = Stopwatch.StartNew();
            while (watch.Elapsed.TotalSeconds < time)
                yield return 0;
        }
    }
}
