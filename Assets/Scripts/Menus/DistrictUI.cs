using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �����, ��������������� ��� ���������� UI ������.
/// </summary>
public class DistrictUI : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    int buttonAction;
    DistrictInfo district;

    [SerializeField] TMP_Text districtName;
    [SerializeField] TMP_Text leaderName;
    [SerializeField] TMP_Text bonusName;
    [SerializeField] TMP_Text hint;
    [SerializeField] TMP_Text hintForButtonText;

    public Image bonusImage;
    public Button button;
    public TMP_Text buttonText;

    public List<Sprite> bonusImages;

    public RectTransform panel;

    [SerializeField] List<DistrictProductionItem> districtProductionItems;

    public void Open(DistrictInfo district)
    {
        panel.gameObject.SetActive(true);
        button.interactable = true;
        buttonAction = 0;
        this.district = district;
        hintForButtonText.text = "";

        // ��������� ��� ������-���������.
        ShowLeader();

        // ��������� ����� ������ � ��������� �� ����.
        ShowDistrictBonus();

        // ��������� ��� ������ �����.
        ShowButton();


        // ��������� � ������ ���������.
        ShowDistrictProductions();
    }

    public void Close()
    {
        this.district = null;
        panel.gameObject.SetActive(false);
    }

    public void UpdateIfNeeded()
    {
        if (district != null)
        {
            Open(district);
        }
    }

    public void LockButton(string text, string hintText)
    {
        buttonText.text = text;
        buttonAction = 0;
        button.interactable = false;
        hintForButtonText.text = hintText;
    }

    public void UnlockButton(string text, string hintText)
    {
        buttonText.text = text;
        button.interactable = true;
        hintForButtonText.text = hintText;
    }

    public void MakeButtonAction()
    {
        switch (buttonAction)
        {
            // �������� ����� �����.
            case 1:
                gameManager.gameSession.Countries[gameManager.gameSession.CurrentCountry].AddNewDistrict(district.districtFunc, true);
                ++gameManager.gameSession.ConqueredNeutralDistrict;
                // ������� ��� ������.
                Open(district);
                break;
            // ������ ��� �� �����.
            case 2:
                gameManager.FightManager.StartBattle(gameManager.gameSession.Countries[gameManager.gameSession.CurrentCountry],
                    district.holder, district);
                break;
            // �������� �����.
            case 3:
                gameManager.gameSession.Countries[gameManager.gameSession.CurrentCountry].RemoveDistrict(district.districtFunc, true, null);
                // ������� ��� ������.
                Open(district);
                break;
            // ������ �� ������.
            default:
                break;
        }

        gameManager.UpdateStats();
    }

    void ShowLeader()
    {
        districtName.text = district.DistrictName;
        if (district.holder != null)
        {
            leaderName.text = district.holder.Leader.LeaderName;
            leaderName.color = district.holder.GetCountryColor();
        }
        else
        {
            leaderName.text = "�����������";
            leaderName.color = Color.black;
        }
    }

    void ShowDistrictBonus()
    {
        bool hintNeeded = true;
        switch (district.DistrictBonus)
        {
            case 1:
                bonusName.text = "   ������";
                bonusImage.sprite = bonusImages[1];
                break;
            case 2:
                bonusName.text = "   ������";
                bonusImage.sprite = bonusImages[2];
                break;
            case 3:
                bonusName.text = "��������������";
                bonusImage.sprite = bonusImages[3];
                break;
            case 4:
                bonusName.text = "���� ������";
                bonusImage.sprite = bonusImages[4];
                hint.text = $"��� {gameManager.gameSession.GameRules.VictoryPointsBonus} ����� ������ �� ������. " +
                    $"��� ����� ����� ������, {gameManager.gameSession.GameRules.VictoryPointsBonus} ����� ������ ����� ����� �������.";
                hintNeeded = false;
                break;
            default:
                bonusName.text = "�����������";
                bonusImage.sprite = bonusImages[0];
                hint.text = "���� ����� �� �������� ������� ����������...";
                hintNeeded = false;
                break;
        }
        if (hintNeeded)
        {
            if (district.HasBonusProduction)
            {
                hint.text = "�������!";
            }
            else
            {
                hint.text = "�� �������! ���������� ��������� ��������� \"���������� ������ ������\", ����� ������������ ���...";
            }
        }
    }

    void ShowButton()
    {
        // ���� ���� ����� - �����������.
        if (district.holder == null)
        {
            // ��������, �������� �� ��.
            if (district.districtFunc.CheckForBeingNearCountry(gameManager.gameSession.Countries[gameManager.gameSession.CurrentCountry]))
            {
                // ��������, ������� ������� �� �����������.
                if (gameManager.gameSession.GameRules.NumberOfAllowedDistrictsToConquerPerMove > gameManager.gameSession.ConqueredNeutralDistrict)
                {
                    UnlockButton("�������� ����� �����", "�������, ��� ������ ���� ����������� ����� ����������� ���. " +
                        "������ ������� ������� �������� ��� �����, ���� ��� ������� ��������� ���� �������.");
                    buttonAction = 1;
                }
                else
                {
                    LockButton("�������� ����� �����", "�� ������ �� ������ ��������� �������� �������� �� ���� ����!");
                }
            }
            else
            {
                LockButton("�������� ����� �����", "����� �� �������� ��������!");
            }
        }
        // ���� ��� ����� ����������� ������ ��� ������:
        else if (gameManager.gameSession.CurrentCountry == district.holder.CountryId)
        {
            // ��������, ��������� �� ��.
            if (district.holder.DistrictsIds.Count == 1)
            {
                LockButton("�������� �����", "������ �������� ��������� ����� � ������!");
            }
            else
            {
                UnlockButton("�������� �����", "�������, ��� ������ �� �������� ���� �������. ����� �����, �� ������ �����������.");
                buttonAction = 3;
            }
        }
        // ���� ��� ����� �� ����������� ����������� ������ ��� ������:
        else if (gameManager.gameSession.CurrentCountry != district.holder.CountryId)
        {
            // ��������, �������� �� ��.
            if (district.districtFunc.CheckForBeingNearCountry(gameManager.gameSession.Countries[gameManager.gameSession.CurrentCountry]))
            {
                Relationship relationship = gameManager.gameSession.FindRelationship(gameManager.gameSession.CurrentCountry, district.holder.CountryId);
                // �������� � ��������� ����� �� ��� ������.
                if (relationship.AtWar)
                {
                    UnlockButton("������ ��� �� �����", "������� �� ���� �����. � ������ ������, �� ������� ��� ����������.");
                    buttonAction = 2;
                }
                else
                {
                    LockButton("������ ��� �� �����", "����� ��������� ���� �����, �� ������ �������� ����� ��� ���������!");
                }
            }
            else
            {
                LockButton("������ ��� �� �����", "����� �� �������� ��������!");
            }
        }
    }

    string buttonForDistrictProductionItemText;
    bool lockButtonForDistrictProductionItem;
    void ShowDistrictProductions()
    {
        for (int i = 0; i < districtProductionItems.Count; i++)
        {
            districtProductionItems[i].gameObject.SetActive(true);
        }
        // ���� ����� �� ����������� ���������� ������, ����������� ������� ������ � ������ ������� ���������.
        if (district.holder == null || gameManager.gameSession.CurrentCountry != district.holder.CountryId)
        {
            if (district.HasBonusProduction) districtProductionItems[0].SetItem("���������� ������ ������", "", "", "���������", true);
            else districtProductionItems[0].gameObject.SetActive(false);

            if (district.HasGoldProduction) districtProductionItems[1].SetItem("������ ������", "", "", "���������", true);
            else districtProductionItems[1].gameObject.SetActive(false);

            if (district.HasSecurityBureau) districtProductionItems[2].SetItem("���� ������������", "", "", "���������", true); 
            else districtProductionItems[2].gameObject.SetActive(false); 

            if (district.HasMonument) districtProductionItems[3].SetItem("��������", "", "", "���������", true);
            else districtProductionItems[3].gameObject.SetActive(false);
        }
        else
        {
            buttonForDistrictProductionItemText = "";
            lockButtonForDistrictProductionItem = false;
            // ������� ���������� ��������� "���������� ������ ������".
            // ���� ��� ������ ��� ���� ��� ���� ������, ��� ��������� �� ����� ����������.
            if (district.DistrictBonus == 0 || district.DistrictBonus == 4)
            {
                districtProductionItems[0].gameObject.SetActive(false);
            }
            else
            {
                districtProductionItems[0].gameObject.SetActive(true);
                string productionName = "���������� ������ ������";
                string cost = $"����: {-gameManager.gameSession.GameRules.CostOfBonusProduction} ������";

                if (district.HasBonusProduction)
                {
                    buttonForDistrictProductionItemText = "���������";
                    lockButtonForDistrictProductionItem = true;

                }
                else
                {
                    // �������� ����������� ������.
                    CheckBuyingPossibility(gameManager.gameSession.GameRules.CostOfBonusProduction);

                }

                switch (district.DistrictBonus)
                {
                    // ������.
                    case 1:
                        districtProductionItems[0].SetItem(productionName + " (������)",
                            $"��� {gameManager.gameSession.GameRules.GoldBonus} ������ �� ���",
                            cost, buttonForDistrictProductionItemText, lockButtonForDistrictProductionItem);
                        break;
                    // ������.
                    case 2:
                        districtProductionItems[0].SetItem(productionName + " (������)",
                            $"��� {gameManager.gameSession.GameRules.IronBonus} ������ �� ���",
                            cost, buttonForDistrictProductionItemText, lockButtonForDistrictProductionItem);
                        break;
                    // ��������������.
                    case 3:
                        districtProductionItems[0].SetItem(productionName + " (��������������)",
                            $"��� {gameManager.gameSession.GameRules.HorsesBonus} �������������� �� ���",
                            cost, buttonForDistrictProductionItemText, lockButtonForDistrictProductionItem);
                        break;
                    default:
                        break;
                }
                districtProductionItems[0].Button.onClick.RemoveAllListeners();
                districtProductionItems[0].Button.onClick.AddListener(district.BuildBonusProduction);
            }


            // ������ � ���������� "������ ������".
            if (district.HasGoldProduction)
            {
                buttonForDistrictProductionItemText = "���������";
                lockButtonForDistrictProductionItem = true;

                districtProductionItems[1].SetItem("������ ������",
                                $"��� {gameManager.gameSession.GameRules.GoldProductionBonus} ������ �� ���",
                                $"����: {-gameManager.gameSession.GameRules.CostOfGoldProduction} ������",
                                buttonForDistrictProductionItemText,
                                lockButtonForDistrictProductionItem);
            }
            else
            {
                CheckBuyingPossibility(gameManager.gameSession.GameRules.CostOfGoldProduction);
                districtProductionItems[1].SetItem("������ ������",
                                $"��� {gameManager.gameSession.GameRules.GoldProductionBonus} ������ �� ���",
                                $"����: {-gameManager.gameSession.GameRules.CostOfGoldProduction} ������",
                                buttonForDistrictProductionItemText,
                                lockButtonForDistrictProductionItem);
                districtProductionItems[1].Button.onClick.RemoveAllListeners();
                districtProductionItems[1].Button.onClick.AddListener(district.BuildGoldProduction);
            }

            // ������ � ���������� "���� ������������".
            if (district.HasSecurityBureau)
            {
                buttonForDistrictProductionItemText = "���������";
                lockButtonForDistrictProductionItem = true;

                districtProductionItems[2].SetItem("���� ������������",
                                $"����������� ��������������� �� {gameManager.gameSession.GameRules.AgentsProductionBonus} ��� ������ ���� \"������ �������\"",
                                $"����: {-gameManager.gameSession.GameRules.CostOfAgentsProduction} ������",
                                buttonForDistrictProductionItemText,
                                lockButtonForDistrictProductionItem);
            }
            else
            {
                CheckBuyingPossibility(gameManager.gameSession.GameRules.CostOfAgentsProduction);
                districtProductionItems[2].SetItem("���� ������������",
                                $"����������� ��������������� �� {gameManager.gameSession.GameRules.AgentsProductionBonus} ��� ������ ���� \"������ �������\"",
                                $"����: {-gameManager.gameSession.GameRules.CostOfAgentsProduction} ������",
                                buttonForDistrictProductionItemText,
                                lockButtonForDistrictProductionItem);
                districtProductionItems[2].Button.onClick.RemoveAllListeners();
                districtProductionItems[2].Button.onClick.AddListener(district.BuildSecurityBureau);
            }

            // ������ � ���������� "��������".
            if (district.HasMonument)
            {
                buttonForDistrictProductionItemText = "���������";
                lockButtonForDistrictProductionItem = true;

                districtProductionItems[3].SetItem("��������",
                                $"��� {gameManager.gameSession.GameRules.MonumentBonus} ����� ������ �� ���������. ��� ����� ������, ��� ���� ������ ����� ������, � ��������� ����������.",
                                $"����: {-gameManager.gameSession.GameRules.CostOfMonument} ������",
                                buttonForDistrictProductionItemText,
                                lockButtonForDistrictProductionItem);

            }
            else
            {
                CheckBuyingPossibility(gameManager.gameSession.GameRules.CostOfMonument);
                districtProductionItems[3].SetItem("��������",
                                $"��� {gameManager.gameSession.GameRules.MonumentBonus} ����� ������ �� ���������. ��� ����� ������, ��� ���� ������ ����� ������, � ��������� ����������.",
                                $"����: {-gameManager.gameSession.GameRules.CostOfMonument} ������",
                                buttonForDistrictProductionItemText,
                                lockButtonForDistrictProductionItem);
                districtProductionItems[3].Button.onClick.RemoveAllListeners();
                districtProductionItems[3].Button.onClick.AddListener(district.BuildMonument);
            }
        }
    }

    void CheckBuyingPossibility(int cost)
    {
        // �������� ����������� ������. ���� �����:
        if (gameManager.gameSession.Countries[gameManager.gameSession.CurrentCountry].Gold + cost >= 0)
        {
            buttonForDistrictProductionItemText = "���������";
            lockButtonForDistrictProductionItem = false;
        }
        else
        {
            buttonForDistrictProductionItemText = "�� �������!";
            lockButtonForDistrictProductionItem = true;
        }
    }
}
