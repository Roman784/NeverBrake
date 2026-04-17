using Cysharp.Threading.Tasks;
using Gameplay;
using GameRoot;
using ObstacleCourseMode;
using System.Collections;
using UnityEngine;

namespace LevelMenu
{
    public class LevelMenuEntryPoint : SceneEntryPoint<LevelMenuEnterParams>
    {
        [SerializeField] private LevelMenuView _view;

        private LevelMenuPresenter _presenter;

        protected override async UniTask Run(LevelMenuEnterParams enterParams)
        {
            var model = new LevelMenuModel(
                levelsCount: 100,
                lastPassedLevelNumber: 0);
            _presenter = new LevelMenuPresenter(_view, model);

            _presenter.CreateLevelButtons();
            _presenter.CreatePathLine();
            _presenter.AdaptLevelButtonsContainer();
            _presenter.ScrollCountainerToLastOpenedLevel();

            await UniTask.Yield();
        }

        private void Update()
        {
            Canvas.ForceUpdateCanvases();

        }

        private void OnDestroy()
        {
            _presenter?.Dispose();
        }
    }
}