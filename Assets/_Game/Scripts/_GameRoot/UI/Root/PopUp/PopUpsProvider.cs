using GameRoot;
using UnityEngine;

namespace UI
{
    public class PopUpsProvider
    {
        private PopUpFactory _popUpFactory;

        private UICMS CMS => G.RootCMS.UICMS;

        public GachaPopUp OpenGachaPopUp(GachaPoolData poolData)
        {
            var createdPopUp = G.PopUpFactory.Create(CMS.GachaPopUpPrefab);
            createdPopUp.Open(poolData);

            Debug.Log(createdPopUp);

            return createdPopUp;
        }
    }
}