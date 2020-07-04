using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD: MonoBehaviour
{
    public Text nameText;
    public Text levelText;
    public Slider hpSlider;

    public void SetupHUD(Unit unit)
    {
        nameText.text = unit.unitName;
        levelText.text = "LV " + unit.unitLevel;
        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;
    }

    public void UpdateHP(int newCurrentHP)
    {
        hpSlider.value = newCurrentHP;
    }
}
