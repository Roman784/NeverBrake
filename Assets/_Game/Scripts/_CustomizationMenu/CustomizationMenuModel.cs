using CMS;
using Currency;
using Cysharp.Threading.Tasks;
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
        public Wallet Wallet { get; private set; }
        public int SelectedCarId { get; set; }

        private List<int> _unlockedCarIds;
        private CarCMS[] _allCarsCMS;
        private Dictionary<SkinPreview, int> _carPreviewsMap = new();

        private CarsRepository Repository => G.Repository.Cars;

        public CustomizationMenuModel()
        {
            Wallet = G.Wallet;
            SelectedCarId = Repository.GetSelectedCarId();

            _unlockedCarIds = new List<int>(
                Repository.GetUnlockedCarIds());

            _allCarsCMS = G.RootCMS.CarsCMS.AllCarsCMS;
        }

        public IEnumerable<int> GetCarIds() => _allCarsCMS.Select(c => c.Id);
        public bool IsSelectedCarUnlocked() => IsUnlocked(SelectedCarId);
        public bool IsUnlocked(int carId) => _unlockedCarIds.Contains(carId);
        public async UniTask SaveSelectedCarId() => await Repository.SetSelectedCarId(SelectedCarId);

        public int GetCarIdByPreview(SkinPreview preview)
        {
            if (_carPreviewsMap.TryGetValue(preview, out var id)) return id;
            return 0;
        }

        public SkinPreview GetCarPreview(int carId)
        {
            return _carPreviewsMap.FirstOrDefault(p => p.Value == carId).Key;
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

        public SkinPreview GetCarPreviewPrefab(int carId)
        {
            var carCMS = _allCarsCMS.FirstOrDefault(c => c.Id == carId);
            if (carCMS != null) return carCMS.PreviewPrefab;
            return null;
        }

        public void AddCarPreview(SkinPreview preview, int carId)
        {
            _carPreviewsMap[preview] = carId;
        }
    }
}
