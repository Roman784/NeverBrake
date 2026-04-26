using UnityEngine;

namespace CMS
{
    [CreateAssetMenu(fileName = "CurrencyCMS",
                     menuName = "CMS/New Currency CMS")]
    public class CurrencyCMS : ScriptableObject
    {
        public Vector2Int CoinsScatterForGift;
        public Vector2Int CoinsScatterForAd;
    }
}
