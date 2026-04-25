using System.Collections.Generic;
using UnityEngine;
using R3;
using System.Linq;

namespace UI
{
    public class ToastsRoot : MonoBehaviour
    {
        [SerializeField] private Transform _containerForAttaching;
        [SerializeField] private Transform _containerForDetaching;

        private List<Toast> _toasts = new();

        public void AttachToast(Toast toast)
        {
            _toasts.Add(toast);
            toast.transform.SetParent(_containerForAttaching, false);

            toast.CloseSignal.Subscribe(t => DestroyToast(t));
        }

        public void Detach(Toast toast)
        {
            toast.transform.SetParent(_containerForDetaching, false);
        }

        public void DestroyToast(Toast toast)
        {
            _toasts.Remove(toast);
            toast.Destroy();
        }

        public void ClearContainer()
        {
            foreach (var toast in _toasts)
            {
                toast.Destroy();
            }

            _toasts.Clear();
        }
    }
}
