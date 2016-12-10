using UnityEngine;
using System.Collections;
using DigitalRuby.Tween;
using System.Collections.Generic;

public class TweenAnimationsManager : MonoBehaviour
{

    private static TweenAnimationsManager _instance;
    public static TweenAnimationsManager Instance
    {
        get
        {
            if (_instance)
            {
                return _instance;
            }
            else
            {
                GameObject go = new GameObject();
                _instance = go.AddComponent<TweenAnimationsManager>();
                go.name = "AnimationsManager";
                return _instance;
            }
        }
    }

    public List<SimpleTweenAnimation> CoinAnimations = new List<SimpleTweenAnimation>();
    private int _coinAnimationsCounter = 0;
    private int _activeIndex = 0;
    private int _maxCounter = 0;

    void Awake()
    {
        if (!_instance)
            _instance = this;
    }

    public static void Add(SimpleTweenAnimation animation)
    {
        Instance.CoinAnimations.Add(animation);
    }
    public static void Remove(SimpleTweenAnimation animation)
    {
        if (_instance)
            Instance.CoinAnimations.Remove(animation);
    }

    public static void SetActiveNext()
    {
        if (_instance)
        {
            Instance._coinAnimationsCounter++;
            if (Instance.CoinAnimations.Count > 0 && Instance._coinAnimationsCounter >= Instance._maxCounter)
            {
                Instance._coinAnimationsCounter = 0;
                SetActive(Instance._activeIndex++ % Instance.CoinAnimations.Count);
            }
        }
    }
    public static bool IsActive(SimpleTweenAnimation animation)
    {
        return Instance.CoinAnimations.IndexOf(animation) == Instance._activeIndex;
    }
    public static void SetActive(int index)
    {
        for (int i = 0; i < Instance.CoinAnimations.Count; i++)
        {
            Instance.CoinAnimations[i].SetActive(i == index);
        }
    }
    public static void SetMaxCounter(int counter)
    {
        Instance._maxCounter = counter;
    }
}
