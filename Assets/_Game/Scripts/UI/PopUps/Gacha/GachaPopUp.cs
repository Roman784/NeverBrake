using DG.Tweening;
using Gameplay;
using GameRoot;
using R3;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Random = UnityEngine.Random;

namespace UI
{
    [RequireComponent(typeof(GachaCapsulesLayout))]
    public class GachaPopUp : PopUp
    {
        [Space]

        //[SerializeField] private WalletHUD _walletHUD;

        [Space]

        [SerializeField] private GachaCapsule _capsulePrefab;
        [SerializeField] private int _maxCapsulesCount;
        [SerializeField] private int _maxSpinCirclesCount;
        [SerializeField] private float _spinDuration;

        [Space]

        [SerializeField] private TMP_Text _spinCostView;
        [SerializeField] private Button _spinBtn;
        [SerializeField] private Button _closeBtn;

        [Space]

        [SerializeField] private GameObject _noMoreView;

        private GachaCapsulesLayout _capsulesLayout;
        private GachaCapsule[] _capsules;
        private Dictionary<int, GachaCapsule> _capsulesByIdMap = new();
        private Coroutine _spinRoutine;

        private Subject<int> _itemReceivedSignalSubj = new();
        public Observable<int> ItemReceivedSignal => _itemReceivedSignalSubj;

        public void Open(GachaPoolData poolData)
        {
            _capsulesLayout = GetComponent<GachaCapsulesLayout>();

            //_walletHUD.Init(G.Wallet);
            SetSpinCost();

            poolData.Shuffle();

            _capsules = CreateCapsules(poolData.Items).ToArray();

            _capsulesLayout.ClearContainer();
            _capsulesLayout.LayOutCapsules(_capsules);

            var hasAnyCapsules = _capsules.Length > 0;
            _spinBtn.gameObject.SetActive(hasAnyCapsules);
            _noMoreView.SetActive(!hasAnyCapsules);

            SetupSubscriptions(poolData.Items);

            base.Open();
        }

        public override void Close()
        {
            Coroutines.Stop(_spinRoutine);
            base.Close();
        }

        private void SetupSubscriptions(IEnumerable<GachaPoolItemData> itemsData)
        {
            _spinBtn.onClick.AddListener(() => TrySpin(itemsData));
            _closeBtn.onClick.AddListener(() => Close());
        }

        private void SetSpinCost()
        {
            var cost = G.RootCMS.GachaCMS.SpinCost;
            var formattetCost = NumberFormatter.FormatCoins(cost);
            _spinCostView.text = formattetCost;
        }

        private IEnumerable<GachaCapsule> CreateCapsules(IEnumerable<GachaPoolItemData> itemsData)
        {
            var capsules = new List<GachaCapsule>();
            foreach (var data in itemsData)
            {
                var createdCapsule = Instantiate(_capsulePrefab);
                createdCapsule.SetRarity(data.Rarity);
                createdCapsule.SetReward(data.Reward);

                createdCapsule.OpenedSignal
                    .Subscribe(_ => ReceiveItem(data.ItemId));

                capsules.Add(createdCapsule);
                _capsulesByIdMap[data.ItemId] = createdCapsule;
            }
            return capsules;
        }

        private void ReceiveItem(int itemId)
        {
            _itemReceivedSignalSubj.OnNext(itemId);
            _itemReceivedSignalSubj.OnCompleted();

            _closeBtn.gameObject.SetActive(true);
        }

        private void TrySpin(IEnumerable<GachaPoolItemData> itemsData)
        {
            var randomItemId = GetRandomItemId(itemsData);
            var targetCapsule = _capsulesByIdMap[randomItemId];

            // Wallet Check

            Spin(targetCapsule);
        }

        private int GetRandomItemId(IEnumerable<GachaPoolItemData> itemsData)
        {
            var itemIdByWeights = itemsData
                    .Select(i => (i.ItemId, RarityWeightMapper.GetWeight(i.Rarity)))
                    .ToArray();
            return WeightedRandom.Get(itemIdByWeights);
        }

        private void Spin(GachaCapsule targetCapsule)
        {
            _spinBtn.gameObject.SetActive(false);
            _closeBtn.gameObject.SetActive(false);

            Coroutines.Stop(_spinRoutine);
            _spinRoutine = Coroutines.Start(
                SpinRoutine(_capsules, targetCapsule, () =>
                {
                    HideCapsulesWithout(targetCapsule);
                    SelectCapsule(targetCapsule);
                }));
        }

        private IEnumerator SpinRoutine(IReadOnlyList<GachaCapsule> capsules, GachaCapsule targetCapsule, Action onComplete)
        {
            var targetCapsuleIdx = capsules.ToList().IndexOf(targetCapsule);
            var targetAngle = 360 * (_maxSpinCirclesCount - targetCapsuleIdx * (1f / capsules.Count()));
            var time = 0f;

            do
            {
                yield return null;

                time += Time.deltaTime;
                time = Mathf.Clamp(time, 0f, _spinDuration);

                var progress = 1f - Mathf.Pow(1 - (time / _spinDuration), 3);
                var offset = targetAngle * progress;

                _capsulesLayout.LayOutCapsules(capsules, offset);
            }
            while (time < _spinDuration);

            onComplete?.Invoke();
        }

        private void HideCapsulesWithout(GachaCapsule without)
        {
            foreach (var capsule in _capsules)
            {
                if (capsule != without)
                    capsule.transform.DOScale(0, 0.4f).SetEase(Ease.InBack);
            }
        }

        private void SelectCapsule(GachaCapsule capsule)
        {
            capsule.transform.DOScale(1.5f, 0.5f).SetEase(Ease.InOutBack);
            capsule.PressedSignal.Subscribe(_ => capsule.Open()).AddTo(capsule);
        }
    }
}