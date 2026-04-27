using GameRoot;
using UnityEngine;

namespace UI
{
    public class PopUpsProvider
    {
        private PopUpFactory _factory;

        private PopUpsRoot Root => G.UIRoot.PopUpsRoot;
        private UICMS CMS => G.RootCMS.UICMS;

        public PopUpsProvider()
        {
            _factory = new PopUpFactory();
        }

        public SettingsPopUp OpenSettingsPopUp()
        {
            var createdPopUp = _factory.Create(CMS.SettingsPopUpPrefab);
            Root.OpenLast();
            return createdPopUp;
        }

        public PausePopUp OpenPausePopUp()
        {
            var createdPopUp = _factory.Create(CMS.PausePopUpPrefab);
            Root.OpenLast();
            return createdPopUp;
        }

        public LevelPassingPopUp OpenLevelPassingPopUp()
        {
            var createdPopUp = _factory.Create(CMS.LevelPassingPopUpPrefab);
            Root.OpenLast();
            return createdPopUp;
        }

        public LevelFailurePopUp OpenLevelFailurePopUp()
        {
            var createdPopUp = _factory.Create(CMS.LevelFailurePopUpPrefab);
            Root.OpenLast();
            return createdPopUp;
        }
    }
}