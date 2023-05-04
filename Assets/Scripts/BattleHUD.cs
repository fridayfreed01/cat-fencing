using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace Assets.Scripts
{
    public class BattleHUD : MonoBehaviour
    {
        public TextMeshProUGUI nameText;

        public Slider hp;

        public Unit unit;

        public void SetHUD(Unit unit)
        {
            nameText.text = unit.unitName;
            hp.maxValue = unit.maxHP;
            hp.value = unit.currentHP;
        }

        public void SetHP(Unit unit)
        {
            hp.value = unit.currentHP;
        }

    }
}
