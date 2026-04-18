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
    public class GachaPopUp : PopUp
    {
        //[Space]

        //[SerializeField] private WalletHUD _walletHUD;

        //[Space]

        //[SerializeField] private GachaCapsulesLayout _capsulesLayout;
        //[SerializeField] private GachaCapsule _capsulePrefab;
        //[SerializeField] private int _maxCapsulesCount;
        //[SerializeField] private int _maxSpinCirclesCount;
        //[SerializeField] private float _spinDuration;

        //[Space]

        //[SerializeField] private TMP_Text _spinCostView;
        //[SerializeField] private Button _spinBtn;
        //[SerializeField] private Button _closeBtn;
        //[SerializeField] private Button _takeBtn;

        //[Space]

        //[SerializeField] private GameObject _noMoreWeaponsView;

        //private bool _canSpin;
        //private Coroutine _spinRoutine;

        //private GachaConfigs GachaConfigs => G.Configs.GachaConfigs;
        //private int SpinCost => GachaConfigs.SpinCost;

        //public override void Open()
        //{
        //    _walletHUD.Init(G.Wallet);
        //    SetSpinCost(SpinCost);
        //    SetUpRoulette();

        //    _takeBtn.onClick.AddListener(() =>
        //    {
        //        _capsulesLayout.ClearContainer();
        //        SetUpRoulette();
        //    });

        //    base.Open();
        //}

        //public override void Close()
        //{
        //    Coroutines.Stop(_spinRoutine);
        //    base.Close();
        //}

        //private void SetSpinCost(int cost)
        //{
        //    var formattetCost = NumberFormatter.FormatMoney(cost);
        //    _spinCostView.text = $"Spin {formattetCost}"; // TODO: Loc.
        //}

        //private void SetUpRoulette()
        //{
        //    var targetWeaponsMap = GetTargetWeaponsMap();

        //    var capsulesCount = targetWeaponsMap.Values.Sum(w => w?.Count ?? 0);
        //    var capsules = CreateCapsules(capsulesCount);
            
        //    EnableButton(_closeBtn);
        //    DisableButton(_takeBtn);

        //    _noMoreWeaponsView.SetActive(capsulesCount == 0);
        //    if (capsulesCount == 0)
        //    {
        //        DisableButton(_spinBtn);
        //        return;
        //    }
        //    else
        //        EnableButton(_spinBtn);
            
        //    SetCapsuleRarities(capsules, targetWeaponsMap);

        //    _capsulesLayout.LayOutCapsules(capsules);

        //    _spinBtn.onClick.AddListener(() =>
        //    {
        //        var targetRarity = CalculateRarity(targetWeaponsMap);
        //        Spin(capsules, targetWeaponsMap, targetRarity, SpinCost);
        //    });

        //    _canSpin = true;
        //}

        //private Dictionary<Rarity, List<WeaponConfigs>> GetTargetWeaponsMap()
        //{
        //    var allWeapons = G.Configs.WeaponsConfigs.Weapons;
        //    var unlockedWeaponIds = G.Repository.WeaponsCollection.GetUnlockedIds();

        //    var targetWeaponsMap = new Dictionary<Rarity, List<WeaponConfigs>>();

        //    foreach (var weapon in allWeapons)
        //    {
        //        if (!targetWeaponsMap.ContainsKey(weapon.Rarity))
        //            targetWeaponsMap[weapon.Rarity] = new List<WeaponConfigs>();

        //        if (weapon.UnlockType == WeaponUnlockType.ForGacha && 
        //            !unlockedWeaponIds.Contains(weapon.Id))
        //            targetWeaponsMap[weapon.Rarity].Add(weapon);
        //    }

        //    return targetWeaponsMap;
        //}

        //private List<GachaCapsule> CreateCapsules(int count)
        //{
        //    var capsules = new List<GachaCapsule>();

        //    for (int i = 0; i < count; i++)
        //    {
        //        var createdCapsule = Instantiate(_capsulePrefab);
        //        capsules.Add(createdCapsule);
        //    }

        //    return capsules;
        //}

        //private void SetCapsuleRarities(IReadOnlyList<GachaCapsule> capsules, 
        //                                IReadOnlyDictionary<Rarity, List<WeaponConfigs>> targetWeaponsMap)
        //{
        //    var rng = new System.Random();
        //    var shuffledCapsules = new List<GachaCapsule>(capsules)
        //        .OrderBy(x => rng.Next()).ToList();

        //    var currentCapsuleIdx = 0;
        //    foreach (var targetWeaponItem in targetWeaponsMap)
        //    {
        //        var rarity = targetWeaponItem.Key;
        //        var count = currentCapsuleIdx + targetWeaponItem.Value.Count;

        //        for (; currentCapsuleIdx < count; currentCapsuleIdx++)
        //        {
        //            shuffledCapsules[currentCapsuleIdx].SetRarity(rarity);
        //        }
        //    }
        //}

        //private void Spin(IReadOnlyList<GachaCapsule> capsules,
        //                  IReadOnlyDictionary<Rarity, List<WeaponConfigs>> targetWeaponsMap,
        //                  Rarity targetRarity,
        //                  int cost)
        //{
        //    if (!_canSpin) return;

        //    var targetCapsule = GetRandomCapsule(capsules, targetRarity);
        //    if (targetCapsule == null) return;

        //    var targetWeapon = GetRandomWeapon(targetWeaponsMap[targetRarity]);

        //    if (targetWeapon == null) return;
        //    if (!G.Wallet.TrySpendMoney(cost)) return;

        //    G.Repository.WeaponsCollection.AddUnlockedId(targetWeapon.Id);

        //    _canSpin = false;
        //    DisableButton(_spinBtn);
        //    DisableButton(_closeBtn);

        //    Coroutines.Stop(_spinRoutine);
        //    _spinRoutine = Coroutines.Start(SpinRoutine(capsules, targetCapsule, () =>
        //    {
        //        HideCapsulesWithout(capsules, targetCapsule);
        //        targetCapsule.Activate(targetWeapon);

        //        targetCapsule.OpenedSignal.Subscribe(_ =>
        //        {
        //            DOVirtual.DelayedCall(1, () =>
        //                EnableButton(_takeBtn));
        //        });
        //    }));
        //}

        //private GachaCapsule GetRandomCapsule(IReadOnlyList<GachaCapsule> capsules, Rarity rarity)
        //{
        //    var rng = new System.Random();
        //    var shuffledCapsules = new List<GachaCapsule>(capsules)
        //        .OrderBy(x => rng.Next()).ToList();

        //    foreach (var capsule in shuffledCapsules)
        //    {
        //        if (capsule.Rarity == rarity)
        //            return capsule;
        //    }
        //    return null;
        //}

        //private WeaponConfigs GetRandomWeapon(IReadOnlyList<WeaponConfigs> targetWeapons)
        //{
        //    return targetWeapons[Random.Range(0, targetWeapons.Count())];
        //}

        //private IEnumerator SpinRoutine(IReadOnlyList<GachaCapsule> capsules, GachaCapsule targetCapsule, Action onComplete)
        //{
        //    var targetCapsuleIdx = capsules.ToList().IndexOf(targetCapsule);
        //    var targetAngle = 360 * (_maxSpinCirclesCount - targetCapsuleIdx * (1f / capsules.Count()));
        //    var time = 0f;

        //    do
        //    {
        //        yield return null;

        //        time += Time.deltaTime;
        //        time = Mathf.Clamp(time, 0f, _spinDuration);

        //        var progress = 1f - Mathf.Pow(1 - (time / _spinDuration), 3);
        //        var offset = targetAngle * progress;

        //        _capsulesLayout.LayOutCapsules(capsules, offset);
        //    } 
        //    while (time < _spinDuration);

        //    onComplete?.Invoke();
        //}

        //private void HideCapsulesWithout(IReadOnlyList<GachaCapsule> capsules, GachaCapsule without)
        //{
        //    foreach (var capsule in capsules)
        //    {
        //        if (capsule != without)
        //            capsule.transform.DOScale(0, 0.4f).SetEase(Ease.InBack);
        //    }
        //}

        //private Rarity CalculateRarity(IReadOnlyDictionary<Rarity, List<WeaponConfigs>> targetWeaponsMap)
        //{
        //    var chancesMap = new Dictionary<Rarity, float>(GachaConfigs.GetChancesMap());
        //    var totalEmptyChance = 0f;

        //    foreach (var rarity in default(Rarity).ToArray())
        //    {
        //        if (targetWeaponsMap.ContainsKey(rarity) && targetWeaponsMap[rarity].Count != 0) continue;
        //        if (!chancesMap.ContainsKey(rarity)) continue;

        //        totalEmptyChance += chancesMap[rarity];
        //        chancesMap.Remove(rarity);
        //    }

        //    chancesMap = chancesMap.ToDictionary(
        //        pair => pair.Key,
        //        pair => pair.Value + totalEmptyChance / chancesMap.Values.Count);

        //    return GachaConfigs.CalculateRarity(chancesMap);
        //}

        //private void EnableButton(Button button)
        //{
        //    button.enabled = enabled;
        //    button.transform.DOScale(1, 0.2f).SetEase(Ease.OutQuad);
        //}

        //private void DisableButton(Button button)
        //{
        //    button.enabled = false;
        //    button.transform.DOScale(0, 0.2f).SetEase(Ease.OutQuad);
        //}
    }
}