using GameRoot;
using UnityEngine;
using R3;
using System;

namespace LevelMenu
{
    public class LevelMenuPresenter : IDisposable
    {
        private LevelMenuView _view;
        private LevelMenuModel _model;

        private CompositeDisposable _disposables = new();

        public LevelMenuPresenter(
            LevelMenuView view,
            LevelMenuModel model) 
        { 
            _view = view;
            _model = model;

            SetupSubscriptions();
        }

        private void SetupSubscriptions()
        {
            _view.ExitButtonPressedSignal
                .Subscribe(_ => ExitFromMneu());
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }

        public void CreateLevelButtons()
        {
            for (int i = 0; i < _model.LevelsCount; i++)
            {
                var number = i + 1;
                var createdButton = _view.CreateLevelButton();
                _model.AddLevelButtonNumber(createdButton, number);

                createdButton.SetNumber(number);

                if (!_model.IsLevelUnlocked(number))
                    createdButton.Lock();
                else
                    createdButton.Unlock();

                if (number == _model.LastPassedLevelNumber + 1)
                    SelectLevelButton(createdButton);

                createdButton.PressedSignal
                    .Subscribe(_ =>
                    {
                        if (_model.IsLevelUnlocked(number))
                            SelectLevelButton(createdButton);
                    })
                    .AddTo(_disposables);
            }
        }

        public void CreatePathLine()
        {
            var positions = _model.GetLevelButtonPositions();
            _view.CreatePathLine(positions);
        }

        public void AdaptLevelButtonsContainer()
        {
            _view.AdaptLevelButtonsContainer(_model.LevelsCount);
        }

        public void ScrollCountainerToLastOpenedLevel()
        {
            _view.ScrollCountainerToLastOpenedLevel(_model.LastPassedLevelNumber + 1);
        }

        private void SelectLevelButton(LevelButton button)
        {
            _model.SelectedLevelButton?.Deselect();
            _model.SelectedLevelButton = button;
            button.Select();
        }

        private void OpenLevel(int number)
        {
            G.SceneProvider.OpenObstacleCourseMode(number);
        }

        private void ExitFromMneu()
        {

        }
    }
}