using TMPro;
using UnityEngine;

//switches the Player between Hero and Mage.
public class PlayerClassSelector : MonoBehaviour
{
    [Header("Classes")]
    [SerializeField] private Hero hero;
    [SerializeField] private Mage mage;

    [Header("UI")]
    //dropdown options must be in this order: 0 = Hero, 1 = Mage.
    [SerializeField] private TMP_Dropdown classDropdown;

    private void Awake()
    {
        if (hero == null) hero = GetComponent<Hero>();
        if (mage == null) mage = GetComponent<Mage>();
    }

    private void Start()
    {
        //default class is the Hero. SetValueWithoutNotify updates the dropdown label
        classDropdown.SetValueWithoutNotify(0);
        SetClass(0);
    }

    private void OnEnable()
    {
        classDropdown.onValueChanged.AddListener(SetClass);
    }

    private void OnDisable()
    {
        classDropdown.onValueChanged.RemoveListener(SetClass);
    }

    //enables exactly one class component and disables the other.
    //both components stay on the GameObject, so each keeps its own HP, damage and cooldown.
    private void SetClass(int index)
    {
        hero.enabled = index == 0;
        mage.enabled = index == 1;
    }
}