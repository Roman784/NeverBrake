using Currency;
using DG.Tweening;
using GameRoot;
using R3;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UI;
using UnityEngine;
using Utils;

namespace GachaMenu
{
    [RequireComponent(typeof(GachaCapsulesLayout))]
    public class GachaMenuView : MonoBehaviour
    {
        [SerializeField] private WalletView _walletView;

        [Space]

        [SerializeField] private GachaCapsule _capsulePrefab;
        [SerializeField] private int _maxCapsulesCount;
        [SerializeField] private int _maxSpinCirclesCount;
        [SerializeField] private float _spinDuration;

        [Space]

        [SerializeField] private TMP_Text _spinCostView;
        [SerializeField] private GameObject _spinBtn;
        [SerializeField] private GameObject _equipBtn;
        [SerializeField] private GameObject _customizationBtn;
        [SerializeField] private GameObject _restartBtn;

        [Space]

        [SerializeField] private GameObject _noMoreView;

        private GachaCapsulesLayout _capsulesLayout;

        private Subject<Unit> _spinButtonPressedSignalSubj = new();
        private Subject<Unit> _equipButtonPressedSignalSubj = new();
        private Subject<Unit> _customizationButtonPressedSignalSubj = new();
        private Subject<Unit> _restartButtonPressedSignalSubj = new();

        public Observable<Unit> SpinButtonPressedSignal => _spinButtonPressedSignalSubj;
        public Observable<Unit> EquipButtonPressedSignal => _equipButtonPressedSignalSubj;
        public Observable<Unit> CustomizationButtonPressedSignal => _customizationButtonPressedSignalSubj;
        public Observable<Unit> RestartButtonPressedSignal => _restartButtonPressedSignalSubj;

        private void Awake()
        {
            _capsulesLayout = GetComponent<GachaCapsulesLayout>();
            
            _noMoreView.SetActive(false);
            _equipBtn.SetActive(false);
            _restartBtn.SetActive(false);
        }

        public void PressSpinButton() => _spinButtonPressedSignalSubj.OnNext(Unit.Default);
        public void PressEquipButton() => _equipButtonPressedSignalSubj.OnNext(Unit.Default);
        public void PressCustomizationButton() => _customizationButtonPressedSignalSubj.OnNext(Unit.Default);
        public void PressRestartButton() => _restartButtonPressedSignalSubj.OnNext(Unit.Default);

        public void BindWallet(Wallet wallet)
        {
            _walletView.Bind(wallet);
        }

        public void SetSpinCost(int cost)
        {
            _spinCostView.text = cost.ToCoinsFormat();
        }

        public GachaCapsule CreateCapsule(Rarity rarity, GameObject rewardPreviewPrefab)
        {
            var createdCapsule = Instantiate(_capsulePrefab);
            createdCapsule.SetRarity(rarity);
            createdCapsule.SetRewardPreviewPrefab(rewardPreviewPrefab);
            return createdCapsule;
        }

        public void ClearContainerAndLayOutCapsules(IEnumerable<GachaCapsule> capsules)
        {
            _capsulesLayout.ClearContainer();
            _capsulesLayout.LayOutCapsules(capsules);
        }

        public void ActiveNoMoreCapsulesLeftMode()
        {
            _noMoreView.SetActive(true);
            _customizationBtn.SetActive(true);

            _spinBtn.SetActive(false);
            _equipBtn.SetActive(false);
            _restartBtn.SetActive(false);
        }

        public Observable<Unit> Spin(
            GachaCapsule targetCapsule, 
            IEnumerable<GachaCapsule> capsules)
        {
            var onCompleted = new Subject<Unit>();

            _spinBtn.SetActive(false);
            _customizationBtn.SetActive(false);
            _restartBtn.SetActive(false);

            targetCapsule.OpenedSignal
                .Subscribe(_ =>
                {
                    _equipBtn.SetActive(true);
                    _customizationBtn.SetActive(true);
                    _restartBtn.SetActive(true);
                });

            StartCoroutine(
                SpinRoutine(targetCapsule, capsules, () =>
                {
                    onCompleted.OnNext(Unit.Default);
                    onCompleted.OnCompleted();
                }));

            return onCompleted;
        }

        private IEnumerator SpinRoutine(
            GachaCapsule targetCapsule,
            IEnumerable<GachaCapsule> capsules, 
            Action onComplete)
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

        public void HideCapsulesWithout( 
            GachaCapsule without,
            IEnumerable<GachaCapsule> capsules)
        {
            foreach (var capsule in capsules)
            {
                if (capsule != without)
                    capsule.transform.DOScale(0, 0.4f).SetEase(Ease.InBack);
            }
        }

        public void HighlightCapsule(GachaCapsule capsule)
        {
            capsule.transform.DOScale(1.5f, 0.5f).SetEase(Ease.InOutBack);
        }
    }
}
