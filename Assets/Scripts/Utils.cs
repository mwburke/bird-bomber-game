using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils {
    public static class Utils {
        public static T GetRandom<T>(this T[] array) {
            return array[Random.Range(0, array.Length)];
        }
    }

}

