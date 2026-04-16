using R3;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace LevelMenu
{
    public class LevelMenuView : MonoBehaviour
    {
        [SerializeField] private LevelButtonsLayout _levelButtonsLayout;
        [SerializeField] private LevelButton _levelButtonPrefab;
        [SerializeField] private LineRenderer _pathLine;

        private Subject<Unit> _exitButtonPressedSignalSubj = new();

        public Observable<Unit> ExitButtonPressedSignal => _exitButtonPressedSignalSubj;

        public void PressExitButton() => _exitButtonPressedSignalSubj.OnNext(Unit.Default);

        public LevelButton CreateLevelButton()
        {
            var createdButton = Instantiate(_levelButtonPrefab);
            _levelButtonsLayout.LayOut(createdButton.RectTransform);

            return createdButton;
        }

        public void CreatePathLine(IEnumerable<Vector3> positions)
        {
            var parent = _levelButtonsLayout.Container;
            _pathLine.transform.SetParent(parent, false);

            for (int i = 0; i < positions.Count(); i++)
            {
                _pathLine.positionCount = i + 1;
                _pathLine.SetPosition(i, _levelButtonsLayout.GetPosition(i) - parent.anchoredPosition);
            }
        }

        public void AdaptLevelButtonsContainer(int buttonsCount)
        {
            _levelButtonsLayout.ResizeContainer(buttonsCount);
        }

        public void ScrollCountainerToLastOpenedLevel(int levelNumber)
        {
            _levelButtonsLayout.ScrollTo(levelNumber - 1);
        }
    }
}