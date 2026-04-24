using GameRoot;
using UnityEngine;

namespace UI
{
    public class ToastFactory
    {
        private ToastsRoot Root => G.UIRoot.ToastsRoot;

        public T Create<T>(T prefab) where T : Toast
        {
            T createdPopUp = Object.Instantiate<T>(prefab);
            Root.AttachToast(createdPopUp);

            createdPopUp
                .SetInitialViewState();

            return createdPopUp;
        }
    }
}
