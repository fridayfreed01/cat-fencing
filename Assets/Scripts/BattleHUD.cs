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
        public TextMeshProUGUI hp;

        public void SetHUD(Unit unit)
        {
            nameText.text = unit.unitName;
            hp.text = unit.currentHP + "/" + unit.maxHP;
        }

        public void SetHP(int health, Unit unit)
        {
            hp.text = health + "/" + unit.maxHP;
        }

    }
}
