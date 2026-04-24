using Gameplay;
using System;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace CMS
{
    [CreateAssetMenu(fileName = "CarsCMS",
                     menuName = "CMS/Cars/New Cars CMS",
                     order = 0)]
    public class CarsCMS : ScriptableObject
    {
        public CarCMS[] AllCarsCMS;
        public CarInputsCMS InputsCMS;

        public bool IsCarExist(int id) => AllCarsCMS.Any(c => c.Id == id);

        public CarCMS GetCarCMS(int id)
        {
            return AllCarsCMS.FirstOrDefault(l => l.Id == id);
        }

        private void OnValidate()
        {
            SetCarIds();
        }

        [ContextMenu("Set Ids")]
        private void SetCarIds()
        {
            if (AllCarsCMS == null || AllCarsCMS.Length == 0) return;
            for (int i = 0; i < AllCarsCMS.Length; i++)
            {
                AllCarsCMS[i].Id = i;
#if UNITY_EDITOR
                EditorUtility.SetDirty(AllCarsCMS[i]);
#endif
            }
        }
    }
}
