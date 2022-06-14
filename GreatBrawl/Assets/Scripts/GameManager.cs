using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MobileTestProject
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        
        [SerializeField] private Core alliedCore;
        [SerializeField] private Core hostileCore;
        [SerializeField] private Transform alliedCorePlaceholder;
        [SerializeField] private Transform hostileCorePlaceholder;
        //For tests
        [SerializeField] private Transform banditPlaceholder;
        [SerializeField] private Transform knightPlaceholder;
        [SerializeField] private Transform monkPlaceholder;

        public static Core AlliedCore
        {
            get => _instance.alliedCore;
            private set => _instance.alliedCore = value;
        }
        
        public static Core HostileCore
        {
            get => _instance.hostileCore;
            private set => _instance.hostileCore = value;
        }

        private void Awake()
        {
            _instance = this;
        }

        private void Start()
        {
            if (!alliedCore)
                alliedCore = ResourceManager.CreatePrefabInstance<EUnits, Core>(EUnits.Core, alliedCorePlaceholder);
            Destroy(alliedCorePlaceholder.gameObject);
            alliedCore.Place(ETeam.Allied);

            if (!hostileCore)
                hostileCore = ResourceManager.CreatePrefabInstance<EUnits, Core>(EUnits.Core, hostileCorePlaceholder);
            Destroy(hostileCorePlaceholder.gameObject);
            hostileCore.Place(ETeam.Hostile);

            var bandit = ResourceManager.CreatePrefabInstance<EUnits, Bandit>(EUnits.Bandit, banditPlaceholder);
            Destroy(banditPlaceholder.gameObject);
            bandit.Place(ETeam.Hostile, alliedCore);
            var knight = ResourceManager.CreatePrefabInstance<EUnits, Knight>(EUnits.Knight, knightPlaceholder);
            Destroy(knightPlaceholder.gameObject);
            knight.Place(ETeam.Hostile, alliedCore);
            var monk = ResourceManager.CreatePrefabInstance<EUnits, Monk>(EUnits.Monk, monkPlaceholder);
            Destroy(monkPlaceholder.gameObject);
            monk.Place(ETeam.Hostile, alliedCore);
        }
    }
}

