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
        public TotalCoinsToast TotalCoinsToastPrefab;
        public CoinsReceivedToast CoinsReceivedToastPrefab;
        public CoinsForAdToast CoinsForAdToastPrefab;
        public PrizeToast PrizeToastPrefab;
        public GiftToast GiftToastPrefab;

        [Header("Pop Ups")]
        public PausePopUp PausePopUpPrefab;
        public LevelPassingPopUp LevelPassingPopUpPrefab;
        public LevelFailurePopUp LevelFailurePopUpPrefab;
    }
}
