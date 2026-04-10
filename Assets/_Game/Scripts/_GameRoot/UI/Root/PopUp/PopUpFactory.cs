using GameRoot;
using UnityEngine;

namespace UI
{
    public class PopUpFactory
    {
        private PopUpsRoot Root => G.UIRoot.PopUpsRoot;

        public T Create<T>(T prefab) where T : PopUp
        {
            T createdPopUp = Object.Instantiate<T>(prefab);
            Root.AttachPopUp(createdPopUp);

            createdPopUp
                .SetInitialViewState();

            return createdPopUp;
        }
    }
}
