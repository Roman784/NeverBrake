using Gameplay;
using System;
using System.Linq;
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
#if UNITY_EDITOR
            SetCarIds();
#endif
        }

        private void SetCarIds()
        {
            if (AllCarsCMS == null || AllCarsCMS.Length == 0) return;
            for (int i = 0; i < AllCarsCMS.Length; i++)
            {
                AllCarsCMS[i].SetId(i);
                EditorUtility.SetDirty(AllCarsCMS[i]);
            }
        }
    }
}
