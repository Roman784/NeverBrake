using GameRoot;
using GameState;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CustomizationMenu
{
    public class CustomizationMenuModel
    {
        public int SelectedCarId { get; set; }

        private List<int> _unlockedCarIds;

        private Dictionary<int, Sprite> _carPreviewsMap = new();
        private Dictionary<CarPreviewItem, int> _carPreviewItemsMap = new();

        private CarsRepository Repository => G.Repository.Cars;

        public CustomizationMenuModel()
        {
            SelectedCarId = Repository.GetSelectedCarId();

            _unlockedCarIds = new List<int>(
                Repository.GetUnlockedCarIds());

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
        public bool IsSelectedCarUnlocked() => IsUnlocked(SelectedCarId);
        public bool IsUnlocked(int carId) => _unlockedCarIds.Contains(carId);
        public void SaveSelectedCarId() => Repository.SetSelectedCarId(SelectedCarId);
        
        public Sprite GetCarPreview(int carId)
        {
            if (_carPreviewsMap.ContainsKey(carId))
                return _carPreviewsMap[carId];
            return null;
        }

        public int GetCarIdByPreviewItem(CarPreviewItem item)
        {
            if (_carPreviewItemsMap.TryGetValue(item, out var id)) return id;
            return 0;
        }

        public CarPreviewItem GetCarPreviewItem(int carId)
        {
            return _carPreviewItemsMap.FirstOrDefault(p => p.Value == carId).Key;
        }

        public void AddCarPreviewItem(CarPreviewItem item, int carId)
        {
            _carPreviewItemsMap[item] = carId;
        }
    }
}
