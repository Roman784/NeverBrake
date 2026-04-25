using UI;
using UnityEngine;

namespace GameRoot
{
    [CreateAssetMenu(fileName = "UICMS",
                     menuName = "CMS/UI/New UI",
                     order = 2)]
    public class UICMS : ScriptableObject
    {
        [Header("Root")]
        public UIRoot Root;

        [Header("Toasts")]
        public CoinsReceivedToast CoinsReceivedToastPrefab;
        public CoinsForAdToast CoinsForAdToastPrefab;
        public PrizeToast PrizeToastPrefab;
        public GiftToast GiftToastPrefab;
    }
}
