using System.Collections.Generic;
using UnityEngine;
using R3;

namespace UI
{
    public class PopUpsRoot : MonoBehaviour
    {
        [SerializeField] private Transform _popUpsContainer;

        private Queue<PopUp> _popUps = new();

        public void AttachPopUp(PopUp popUp)
        {
            _popUps.Enqueue(popUp);
            popUp.transform.SetParent(_popUpsContainer, false);

            popUp.CloseSignal.Subscribe(_ => DestroyTopPopUp());
        }

        public void DestroyTopPopUp()
        {
            var popUp = _popUps.Dequeue();
            popUp.Destroy();

            if (_popUps.Count > 0)
                _popUps.Peek().Open();
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