using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MobileTestProject
{
    public class PlayerResources : MonoBehaviour
    {
        [SerializeField] private int startCoins = 50;
        [SerializeField] private TextMeshProUGUI coinsUI;

        private int _coins = 0;

        private void Start()
        {
            GainCoins(startCoins);
        }

        public bool TrySpendCoins(int amount)
        {
            if (_coins >= amount)
            {
                _coins -= amount;
                UpdateUI(_coins);
                return true;
            }

            return false;
        }

        public void GainCoins(int amount)
        {
            _coins += amount;
            UpdateUI(_coins);
        }

        private void UpdateUI(int amount)
        {
            coinsUI.text = amount.ToString();
        }
    }
}

