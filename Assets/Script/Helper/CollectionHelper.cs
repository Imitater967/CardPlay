using System.Collections.Generic;
using System.Collections.ObjectModel;
using Script.Player;

namespace Script.Helper {

    
    public static class CollectionHelper {

        public static void SwapElement<T>(this Collection<T> list, int index1, int index2) {
            var temp = list[index1];
            list[index1] = list[index2];
            list[index2] = temp;
        }
        public static void SwapElement<T>(this List<T> list, int index1, int index2) {
            var temp = list[index1];
            list[index1] = list[index2];
            list[index2] = temp;
        }
        public static int GetCardIndex(this List<PreparePlayerPrepareCardQueuePresenter.CardCompound> list,StaffCard card) {
            for (var i = 0; i < list.Count; i++) {
                if (list[i].Model == card) {
                    return i;
                }
            }
            return -1;
        }
    }
}