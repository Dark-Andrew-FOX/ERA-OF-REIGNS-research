using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �����, ��������������� ��� ���������� UI �����.
/// </summary>
public class ArmyUI : MonoBehaviour
{
    [SerializeField] GameObject ContentForUnits;

    [SerializeField] Button button;
    [SerializeField] TMP_Text buttonText;

    [SerializeField] GameObject warriorPrefab;
    [SerializeField] GameObject enhancedWarrior;
    [SerializeField] GameObject horsemanPrefab;
    [SerializeField] GameObject agentPrefab;

    [SerializeField] TMP_Text leaderNameText;
    [SerializeField] TMP_Text capacityText;
    [SerializeField] TMP_Text powerText;

    // 0 - ����, 1 - ����. ����, 2- �������, 3 - �����.
    [SerializeField] List<Button> buttonsForBuying;
    [SerializeField] List<TMP_Text> buttonForBuyingTexts;
    [SerializeField] GameObject store;

    private Country country;
    // 0 - ��� ��� (����� �������� � �������� ������), 1 - ��� ��� (����� ������� �����), 2 - ��� ��� (������ ������� �����).
    private int state;

    private bool isDefender;
    private FightManager fightManager;

    public Country Country { get => country; set => country = value; }
    public FightManager FightManager { get => fightManager; set => fightManager = value; }
    public bool IsDefender { get => isDefender; set => isDefender = value; }

    /// <summary>
    /// ��������� ������ ����� ��� ������.
    /// </summary>
    /// <param name="country">������ ������, ��� ������� ����� ��������� �����������.</param>
    /// <param name="state">��������� ������ (0 - ��� ��� (����� �������� � �������� ������),
    /// 1 - ��� ��� (����� ������� �����), 2 - ��� ��� (������ ������� �����)).
    /// </param>
    public void Open(Country country, int state)
    {
        this.country = country;
        this.state = state;

        if (state != 0)
        {
            button.gameObject.SetActive(false);
            CloseStore();
        }
        else
        {
            button.gameObject.SetActive(true);
        }

        leaderNameText.text = country.Leader.LeaderName;
        leaderNameText.color = country.GetCountryColor();

        UpdateList();
    }

    public void Open(Country country, int state, FightManager fightManager, bool isDefender)
    {
        this.FightManager = fightManager;
        this.IsDefender = isDefender;

        Open(country, state);
    }

    public void UpdateList()
    {
        LeaderMonoBehaviour.GameManager.UpdateStats();

        Army army = Country.Army;

        capacityText.text = army.FilledCapacity + "/" + Country.CounterOfDistrictsEverHelded;
        powerText.text = army.Power.ToString();

        // ������� ����.
        ClearList();

        // �������� ����.
        for (int i = 0; i < army.Units.Count; i++)
        {
            UnitUI unitItem;
            switch (army.Units[i].Type)
            {
                case 0:
                    unitItem = Instantiate(warriorPrefab, ContentForUnits.transform).GetComponent<UnitUI>();
                    break;
                case 1:
                    unitItem = Instantiate(enhancedWarrior, ContentForUnits.transform).GetComponent<UnitUI>();
                    break;
                case 2:
                    unitItem = Instantiate(horsemanPrefab, ContentForUnits.transform).GetComponent<UnitUI>();
                    break;
                case 3:
                    unitItem = Instantiate(agentPrefab, ContentForUnits.transform).GetComponent<UnitUI>();
                    break;
                default:
                    unitItem = null;
                    Debug.LogError("����� ��� ����� �� ��������!");
                    break;
            }

            unitItem.Create(army.Units[i], this, state);
        }

        // ������ ��������, ����� ������ ����� ������ � ��������� ���.
        GameRules gameRules = LeaderMonoBehaviour.GameManager.gameSession.GameRules;
        for (int i = 0; i < 4; i++)
        {
            if (country.Army.CheckIfBuyngPossible(i, Country))
            {
                // �� �����.
                buttonForBuyingTexts[i].text = "������";
                buttonsForBuying[i].interactable = true;
            }
            else
            {
                // �� ������.
                buttonsForBuying[i].interactable = false;

                if (!(gameRules.CapacityRequirements[i] + country.Army.FilledCapacity <= country.CounterOfDistrictsEverHelded))
                {
                    buttonForBuyingTexts[i].text = "��� �����!";
                }
                else
                {
                    buttonForBuyingTexts[i].text = "�� ������� ��������!";
                }
            }
        }
    }

    public void Close()
    {
        CloseStore();
        // ������� ����.
        // ClearList();
        // Destroy(gameObject);
    }

    private void ClearList()
    {
        UnitUI[] childs = gameObject.GetComponentsInChildren<UnitUI>();
        for (int i = 0; i < childs.Length; i++)
        {
            Destroy(childs[i].gameObject);
        }
    }

    private void AddUnitToToTheArmy(Unit unit)
    {
        Country.Army.AddNewUnit(unit, country);
        UpdateList();
        //LeaderMonoBehaviour.GameManager.UpdateStats();
    }

    public void BuyUnitWarrior()
    {
        GameRules gameRules = LeaderMonoBehaviour.GameManager.gameSession.GameRules;
        //Unit unit = new Unit("����", 0, false, gameRules.Damages[0], gameRules.Healths[0]);
        Unit unit = country.Army.GetUnit(0, false);

        AddUnitToToTheArmy(unit);
    }

    public void BuyUnitCoolerWarrior()
    {
        GameRules gameRules = LeaderMonoBehaviour.GameManager.gameSession.GameRules;
        //Unit unit = new Unit("��������� ����", 1, false, gameRules.Damages[1], gameRules.Healths[1]);
        Unit unit = country.Army.GetUnit(1, false);

        AddUnitToToTheArmy(unit);
    }

    public void BuyUnitHorseman()
    {
        GameRules gameRules = LeaderMonoBehaviour.GameManager.gameSession.GameRules;
        //Unit unit = new Unit("�������", 2, false, gameRules.Damages[2], gameRules.Healths[2]);
        Unit unit = country.Army.GetUnit(2, false);

        AddUnitToToTheArmy(unit);
    }

    public void BuyUnitAgent()
    {
        GameRules gameRules = LeaderMonoBehaviour.GameManager.gameSession.GameRules;
        //Unit unit = new Unit("����� ������ �������", 3, false, gameRules.Damages[3], gameRules.Healths[3]);
        Unit unit = country.Army.GetUnit(3, false);

        AddUnitToToTheArmy(unit);
    }

    public void OpenStore()
    {
        store.SetActive(true);
    }

    public void CloseStore()
    {
        store.SetActive(false);
    }
}
