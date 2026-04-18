using GameRoot;
using System.Collections.Generic;
using UnityEngine;

namespace CustomizationMenu
{
    public class CustomizationMenuModel
    {
        private Dictionary<int, Sprite> _carPreviewsMap = new();
        private List<CarPreviewItem> _carPreviewItems = new();

        public CustomizationMenuModel()
        {
            FillCarPreviewsMap();
        }

        private void FillCarPreviewsMap()
        {
            var allCarsCMS = G.RootCMS.CarsCMS.AllCarsCMS;
            foreach (var carCMS in allCarsCMS)
            {
                _carPreviewsMap[carCMS.Id] = carCMS.Preview;
            }
        }

        public IEnumerable<int> GetCarIds() => _carPreviewsMap.Keys;
        
        public Sprite GetCarPreview(int id)
        {
            if (_carPreviewsMap.ContainsKey(id))
                return _carPreviewsMap[id];
            return null;
        }

        public void AddCarpreviewItem(CarPreviewItem carPreviewItem)
        {
            _carPreviewItems.Add(carPreviewItem);
        }
    }
}
