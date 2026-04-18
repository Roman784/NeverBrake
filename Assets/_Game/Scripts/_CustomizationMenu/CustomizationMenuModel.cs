using CMS;
using GameRoot;
using GameState;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace CustomizationMenu
{
    public class CustomizationMenuModel
    {
        public int SelectedCarId { get; set; }

        private List<int> _unlockedCarIds;
        private CarCMS[] _allCarsCMS;
        private Dictionary<CarPreviewItem, int> _carPreviewItemsMap = new();

        private CarsRepository Repository => G.Repository.Cars;

        public CustomizationMenuModel()
        {
            SelectedCarId = Repository.GetSelectedCarId();

            _unlockedCarIds = new List<int>(
                Repository.GetUnlockedCarIds());

            _allCarsCMS = G.RootCMS.CarsCMS.AllCarsCMS;
        }

        public IEnumerable<int> GetCarIds() => _allCarsCMS.Select(c => c.Id);
        public bool IsSelectedCarUnlocked() => IsUnlocked(SelectedCarId);
        public bool IsUnlocked(int carId) => _unlockedCarIds.Contains(carId);
        public void SaveSelectedCarId() => Repository.SetSelectedCarId(SelectedCarId);

        public int GetCarIdByPreviewItem(CarPreviewItem item)
        {
            if (_carPreviewItemsMap.TryGetValue(item, out var id)) return id;
            return 0;
        }

        public CarPreviewItem GetCarPreviewItem(int carId)
        {
            return _carPreviewItemsMap.FirstOrDefault(p => p.Value == carId).Key;
        }

        public IEnumerable<int> GetLockedCarIds()
        {
            return _allCarsCMS
                .Where(c => !IsUnlocked(c.Id))
                .Select(c => c.Id);
        }

        public Rarity GetCarRarity(int carId)
        {
            var carCMS = _allCarsCMS.FirstOrDefault(c => c.Id == carId);
            if (carCMS != null) return carCMS.Rarity;
            return Rarity.Common;
        }

        public Sprite GetCarPreview(int carId)
        {
            var carCMS = _allCarsCMS.FirstOrDefault(c => c.Id == carId);
            if (carCMS != null) return carCMS.Preview;
            return null;
        }

        public void AddCarPreviewItem(CarPreviewItem item, int carId)
        {
            _carPreviewItemsMap[item] = carId;
        }
    }
}
