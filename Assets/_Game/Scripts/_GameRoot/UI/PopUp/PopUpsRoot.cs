using System.Collections.Generic;
using UnityEngine;
using R3;
using System.Linq;
using DG.Tweening;

namespace UI
{
    public class PopUpsRoot : MonoBehaviour
    {
        [SerializeField] private Transform _popUpsContainer;

        private List<PopUp> _popUps = new();

        private PopUp LastPopUp => _popUps.Last();

        public void OpenLast()
        {
            if (_popUps.Count == 0) return;

            if (_popUps.Count > 1)
                _popUps[_popUps.Count - 2].Hide().OnComplete(() =>
                {
                    LastPopUp.Open();
                });
            else
                LastPopUp.Open();
        }

        public void AttachPopUp(PopUp popUp)
        {
            _popUps.Add(popUp);
            popUp.transform.SetParent(_popUpsContainer, false);

            popUp.CloseSignal
                .Subscribe(_ => DestroyTopPopUp())
                .AddTo(popUp);
        }

        public void DestroyTopPopUp()
        {
            var popUp = LastPopUp;
            _popUps.Remove(popUp);
            popUp.Destroy();

            if (_popUps.Count > 0)
                LastPopUp.Show();
        }

        public void ClearContainer()
        {
            foreach (var popUp in _popUps)
            {
                popUp.Destroy();
            }

            _popUps.Clear();
        }
    }
}