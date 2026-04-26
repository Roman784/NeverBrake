using GameRoot;
using UnityEngine;

namespace UI
{
    public class PopUpsProvider
    {
        private PopUpFactory Factory => G.PopUpFactory;
        private UICMS CMS => G.RootCMS.UICMS;

        public LevelPassingPopUp OpenLevelPassingPopUp()
        {
            var createdPopUp = Factory.Create(CMS.LevelPassingPopUpPrefab);
            createdPopUp.Open();
            return createdPopUp;
        }

        public LevelFailurePopUp OpenLevelFailurePopUp()
        {
            var createdPopUp = Factory.Create(CMS.LevelFailurePopUpPrefab);
            createdPopUp.Open();
            return createdPopUp;
        }
    }
}