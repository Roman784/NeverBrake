using System.Collections.Generic;
using UnityEngine;
using R3;
using System.Linq;

namespace UI
{
    public class ToastsRoot : MonoBehaviour
    {
        [SerializeField] private Transform _container;

        private List<Toast> _toasts = new();

        public void AttachToast(Toast toast)
        {
            _toasts.Add(toast);
            toast.transform.SetParent(_container, false);
        }

        public void DestroyAllToasts()
        {
            foreach (var toast in _toasts)
                toast.Destroy();
            _toasts.Clear();
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
