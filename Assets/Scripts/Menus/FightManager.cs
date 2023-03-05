using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �����, �������������� ��� �������� ����� �������� �� �����.
/// </summary>
public class FightManager : MonoBehaviour
{
    // ���������� UI.
    [SerializeField] GameObject fightUI;
    [SerializeField] TMP_Text districtAtStakeText;
    [SerializeField] TMP_Text admitDefeatHintText;
    [SerializeField] TMP_Text readyToFightHintText;

    [SerializeField] GameObject defendingUnitPlacement;
    [SerializeField] GameObject attackingUnitPlacement;

    [SerializeField] Button buttonReadyToFight;
    [SerializeField] Button buttonAdmitDefeat;

    [SerializeField] ArmyUI defenderArmyUI;
    [SerializeField] ArmyUI attackerArmyUI;

    // ����� ���������.
    [SerializeField] GameObject battleOverScreen;
    [SerializeField] TMP_Text whoWonHintText;

    // ����.
    Country attackingCountry;
    Country defendingCountry;
    DistrictInfo districtAtStake;

    UnitUI attackingUnit = null;
    UnitUI defendingUnit = null;

    /// <summary>
    /// ������������� ��������, ����� �����-������ ������ ��������� ����� ��� ���.
    /// </summary>
    /// <param name="byDefender">������ ��������� �����?</param>
    /// <param name="unitUIOfChoise">����, �������� ������� ������.</param>
    public void UnitChoosed(bool byDefender, UnitUI unitUIOfChoise)
    {
        Debug.Log("���� ������!");
        Debug.LogWarning("����� ����� ������� �� " + attackingCountry.Army.Units.Count + " ������");
        Debug.LogWarning("����� ������ ������� �� " + defendingCountry.Army.Units.Count + " ������");

        if (byDefender)
        {
            // ����������� �� �������� � unitUIOfChoise, ���������� � �����.
            unitUIOfChoise.gameObject.transform.SetParent(fightUI.transform);
            unitUIOfChoise.gameObject.transform.position = defendingUnitPlacement.transform.position;
            defendingUnit = unitUIOfChoise;

            // ���� ��� ��� �����, ��������� ����� ��� ������ �����.
            if (defendingCountry.Leader.IsPlayer)
            {
                defenderArmyUI.Open(defendingCountry, 2, this, true);
                // ���� �� ��� �� ��������, ����������.
                if (attackingUnit == null)
                {
                    MakeAILeaderToChooseUnit();
                    return;
                }
            }
        }
        else
        {
            // ����������� �� �������� � unitUIOfChoise, ���������� � �����.
            unitUIOfChoise.gameObject.transform.SetParent(fightUI.transform);
            unitUIOfChoise.gameObject.transform.position = attackingUnitPlacement.transform.position;
            attackingUnit = unitUIOfChoise;

            // ���� ��� ��� �����, ��������� ����� ��� ������ �����.
            if (attackingCountry.Leader.IsPlayer)
            {
                attackerArmyUI.Open(attackingCountry, 2, this, false);
                // ���� �� ��� �� ��������, ����������.
                if (defendingUnit == null)
                {
                    MakeAILeaderToChooseUnit();
                    return;
                }
            }
        }

        // ���������, ��� �� ����� ������ � ���.
        if (attackingUnit != null && defendingUnit != null)
        {
            // �������� ��� ����� �������.
            StartFightBetweenUnits();
        }
    }

    /// <summary>
    /// �������� ��� ����� ����� ������������� �������.
    /// </summary>
    private void StartFightBetweenUnits()
    {
        int damageOfDefender = defendingUnit.Unit.Damage;
        int damageOfAttacker = attackingUnit.Unit.Damage;

        // ������� ���� ���������� �����.
        // ���� ���� ��������, ������� ���.
        if (attackingUnit.Unit.TakeDamage(damageOfDefender))
        {
            Debug.Log("���� �������� � ����������!");
            //attackingCountry.Army.Units.Remove(attackingUnit.Unit);
            attackingCountry.Army.KillUnit(attackingUnit.Unit, attackingCountry);
            attackingUnit.Kill();
        }
        // ����� ���������� � ������ (������� UnitUI, �� ������ ��������� ��� ���������� ������).
        else
        {
            Destroy(attackingUnit.gameObject, 1);
        }
        attackingUnit = null;

        // ������� ���� ����������� �����.
        // ���� ���� ��������, ������� ���.
        if (defendingUnit.Unit.TakeDamage(damageOfAttacker))
        {
            Debug.Log("���� �������� � �������������!");
            //defendingCountry.Army.Units.Remove(attackingUnit.Unit);
            defendingCountry.Army.KillUnit(defendingUnit.Unit, defendingCountry);
            defendingUnit.Kill();
        }
        // ����� ���������� � ������ (������� UnitUI, �� ������ ��������� ��� ���������� ������).
        else
        {
            Destroy(defendingUnit.gameObject, 1);
        }
        defendingUnit = null;

        //Debug.LogWarning("����� ����� ������� �� " + attackingCountry.Army.Units.Count + " ������");
        //Debug.LogWarning("����� ������ ������� �� " + defendingCountry.Army.Units.Count + " ������");

        UpdatePanelsAfterFight();

        // ���������, ��� �� ���������. 
        // ��������� ������, ���� ���.
        if (!CheckIfSomebodyIsDefeated()) MakeAILeaderToChooseUnit();
    }

    /// <summary>
    /// ��������� ������ ����� ����� ��������.
    /// </summary>
    public void UpdatePanelsAfterFight()
    {
        ArmyUI playersArmyUI;
        ArmyUI computerLeaderArmyUI;
        Country playerCountry;
        Country computerLeaderCountry;

        if (attackingCountry.Leader.IsPlayer)
        {
            playersArmyUI = attackerArmyUI;
            playerCountry = attackingCountry;

            computerLeaderArmyUI = defenderArmyUI;
            computerLeaderCountry = defendingCountry;
        }
        else
        {
            playersArmyUI = defenderArmyUI;
            playerCountry = defendingCountry;

            computerLeaderArmyUI = attackerArmyUI;
            computerLeaderCountry = attackingCountry;
        }

        computerLeaderArmyUI.Open(computerLeaderCountry, 2, this, computerLeaderArmyUI.IsDefender);
        playersArmyUI.Open(playerCountry, 1, this, playersArmyUI.IsDefender);
    }

    /// <summary>
    /// ������ ��-������ ������� ����� ��� ����� �� ����� �����.
    /// </summary>
    public void MakeAILeaderToChooseUnit()
    {
        Unit unit;
        if (defendingCountry.Leader.IsPlayer)
        {
            unit = attackingCountry.Leader.ChooseUnitToFight();
            // ���� ����� � �������� � ��������.
            FindUnitUIinChildrenOf(attackerArmyUI.gameObject, unit);
        }
        else
        {
            unit = defendingCountry.Leader.ChooseUnitToFight();
            // ���� ����� � �������� � ��������.
            FindUnitUIinChildrenOf(defenderArmyUI.gameObject, unit);
        }
    }

    private void FindUnitUIinChildrenOf(GameObject armyUI, Unit unit)
    {
        UnitUI[] childs = armyUI.GetComponentsInChildren<UnitUI>();
        for (int i = 0; i < childs.Length; i++)
        {
            if (childs[i].Unit == unit)
            {
                if (defendingCountry.Leader.IsPlayer) UnitChoosed(false, childs[i]);
                else UnitChoosed(true, childs[i]);
                return;
            }
        }
    }

    /// <summary>
    /// ��������� ��������� ���, ���������� �������� ���.
    /// </summary>
    /// <param name="attackingCountry">��������� ������.</param>
    /// <param name="defendingCountry">������������ ������</param>
    /// <param name="districtAtStake">�����, �� ������� ��� ���.</param>
    public void StartBattle(Country attackingCountry, Country defendingCountry, DistrictInfo districtAtStake)
    {
        buttonReadyToFight.gameObject.SetActive(true);
        readyToFightHintText.gameObject.SetActive(true);

        districtAtStakeText.text = districtAtStake.DistrictName;
        this.attackingCountry = attackingCountry;
        this.defendingCountry = defendingCountry;
        this.districtAtStake = districtAtStake;

        Debug.Log("����� ����� ������� �� " + attackingCountry.Army.Units.Count + " ������");
        Debug.Log("����� ������ ������� �� " + defendingCountry.Army.Units.Count + " ������");

        // � ����� ������ ��� ������������ ������� ��������� ������, ���� �����.
        if (LeaderMonoBehaviour.GameManager.gameSession.GameRules.GiveFreeUnitsForDefending) GiveFreeUnits();

        // ���� ����� ���������� � �����...
        if (attackingCountry.Leader.IsPlayer || defendingCountry.Leader.IsPlayer)
        {
            // �� ��������� ��������� � �������� ����������� ���.
            StartBattleWithPlayer();
        }
        else
        {
            // �� ��������� ��������� � �������� ���������� ���.
            StartBattleWithotPlayer();
        }
    }

    /// <summary>
    /// ������� ���-��.
    /// </summary>
    /// <param name="defenderWon">������� ������������?</param>
    public void EndBattle(bool defenderWon)
    {
        Debug.Log("���������� ���");
        Debug.LogWarning("����� ����� ������� �� " + attackingCountry.Army.Units.Count + " ������");
        Debug.LogWarning("����� ������ ������� �� " + defendingCountry.Army.Units.Count + " ������");

        GameManager gameManager = LeaderMonoBehaviour.GameManager;
        string whoWonMessage;
        Color whoWontTextColor;

        // ����� ����������� ����� ���������� ������, ������� ��������� ������.
        if (defenderWon)
        {
            Debug.Log("������� ������������!");
            whoWonMessage = "������������ ����� " + defendingCountry.Leader.LeaderName + " ������� ������!\n" +
                "����� " + districtAtStake.DistrictName + " ������� � ��� ��������. ";
            whoWontTextColor = defendingCountry.GetCountryColor();

            defendingCountry.Gold += gameManager.gameSession.GameRules.VictoryBonusInGold;
            defendingCountry.VictoryPoints += gameManager.gameSession.GameRules.VictoryBonusInVictoryPoints;

            attackingCountry.VictoryPoints += gameManager.gameSession.GameRules.DefeatFineInVictoryPoints;
        }
        else
        {
            Debug.Log("������� ���������!");
            whoWonMessage = "��������� ����� " + attackingCountry.Leader.LeaderName + " ������� ������!\n" +
                "����� " + districtAtStake.DistrictName + " ��������� � ��� ��������. ";
            whoWontTextColor = attackingCountry.GetCountryColor();

            attackingCountry.Gold += gameManager.gameSession.GameRules.VictoryBonusInGold;
            attackingCountry.VictoryPoints += gameManager.gameSession.GameRules.VictoryBonusInVictoryPoints;

            defendingCountry.VictoryPoints += gameManager.gameSession.GameRules.DefeatFineInVictoryPoints;

            // ������ ��������� ��������� ������� �����.
            defendingCountry.RemoveDistrict(districtAtStake.districtFunc, true, attackingCountry);

            // ���� ��� ������ ��� �������, �� ����� ��� �����.
            if (defendingCountry.DistrictsIds.Count == 0)
            {
                attackingCountry.VictoryPoints += gameManager.gameSession.GameRules.VictoryOverCountryBonusInVictoryPoints;
                whoWonMessage += "����� ����, ��� ������ ������������ ����� ����� ��������� ������ " + defendingCountry.Leader.LeaderName + "!";
            }
        }

        // ����������� �����, ������ ���� ����� �������� ������� � �����.
        if (defendingCountry.Leader.IsPlayer || attackingCountry.Leader.IsPlayer)
        {
            OpenBattleOverScreen(whoWonMessage, whoWontTextColor);
        }
        else
        {
            CloseTheWholeThing();
        }
    }

    public void OpenBattleOverScreen(string whoWonHint, Color colorOfTheHintText)
    {
        battleOverScreen.SetActive(true);
        whoWonHintText.text = whoWonHint;
        whoWonHintText.color = colorOfTheHintText;
    }

    public void CloseTheWholeThing()
    {
        battleOverScreen.SetActive(false);

        GameManager gameManager = LeaderMonoBehaviour.GameManager;

        //Destroy(attackingUnit.gameObject);
        //Destroy(defendingUnit.gameObject);
        attackingUnit = null;
        defendingUnit = null;

        districtAtStake.districtFunc.HighlightDistrict(7);
        fightUI.SetActive(false);
        gameManager.CameraController.LockCamera(false);

        gameManager.UpdateStats();
        gameManager.districtUI.UpdateIfNeeded();
        defendingCountry.Army.DeleteTemporaryUnits();

        gameManager.gameSession.Countries[LeaderMonoBehaviour.GameManager.gameSession.CurrentCountry].Leader.BattleEnded();
    }

    /// <summary>
    /// ��������� ��� ������.
    /// </summary>
    public void EndBattle()
    {
        Debug.Log("��������� ��� ������!");
        defendingCountry.RemoveDistrict(districtAtStake.districtFunc, true, null);

        // ����������� �����, ������ ���� ����� �������� ������� � �����.
        if (defendingCountry.Leader.IsPlayer || attackingCountry.Leader.IsPlayer)
        {
            OpenBattleOverScreen("��� ������ �������� ��� ���� �����, ������� ����� ����������� �����������!", Color.black);
        }
        else
        {
            CloseTheWholeThing();
        }
        
    }

    private void StartBattleWithPlayer()
    {
        // ���������� ������ �� ����� � ��������.
        LeaderMonoBehaviour.GameManager.CameraController.FocusCamera(districtAtStake.districtFunc.gameObject);
        LeaderMonoBehaviour.GameManager.CameraController.LockCamera(true);
        fightUI.SetActive(true);

        // ����� ��-�������� ��� ���������� �����.
        if (!attackingCountry.Leader.IsPlayer) attackingCountry.Leader.ConsiderToImproveArmyBeforeFight(defendingCountry.Army.Power);
        else if (!defendingCountry.Leader.IsPlayer) defendingCountry.Leader.ConsiderToImproveArmyBeforeFight(attackingCountry.Army.Power);

        // �������� ����������� ������� ����� ��� ���������� � ���.
        // ��� ������.
        if (defendingCountry.Leader.IsPlayer)
        {
            defenderArmyUI.Open(defendingCountry, 0, this, true);
            admitDefeatHintText.text = "���� ��������, ��� ������������ ��� ��������� ���������� �� ���� ����� ���������, " +
                "�� ������ �������� �����. � ���� ������ ��� ����� ��������� ��������� � ���, �� ����� ����� ���������� � ����� �����.";
        }
        else
        {
            defenderArmyUI.Open(defendingCountry, 2, this, true);
        }

        // ��� ���������.
        if (attackingCountry.Leader.IsPlayer)
        {
            attackerArmyUI.Open(attackingCountry, 0, this, false);
            admitDefeatHintText.text = "���� ��������, ��� ������������, " +
                "�� ������ �������� �����. � ���� ������ ��� ����� ��������� ��������� � ���, �� ����� ����� ���������� � ����� �����.";
        }
        else
        {
            attackerArmyUI.Open(attackingCountry, 2, this, false);
        }
    }

    /// <summary>
    /// ���������, �� ���� �� ����������� ��������.
    /// </summary>
    /// <returns>bool, ���� ����, ����� false.</returns>
    private bool CheckIfSomebodyIsDefeated()
    {
        // ���� ��������� ���.
        if (attackingCountry.Army.Units.Count == 0 && defendingCountry.Army.Units.Count == 0)
        {
            EndBattle();
            return true;
        }
        // ���� �������� ������������.
        else if (attackingCountry.Army.Units.Count != 0 && defendingCountry.Army.Units.Count == 0)
        {
            EndBattle(false);
            return true;
        }
        // ���� �������� ���������.
        else if (attackingCountry.Army.Units.Count == 0 && defendingCountry.Army.Units.Count != 0)
        {
            EndBattle(true);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ConfirmReady()
    {
        // ���������� ���.
        buttonReadyToFight.gameObject.SetActive(false);
        readyToFightHintText.gameObject.SetActive(false);

        // ������� ����� ������ ����� ������ �� ���.
        if (defendingCountry.Leader.IsPlayer)
        {
            defenderArmyUI.Open(defendingCountry, 1, this, true);
        }
        else
        {
            attackerArmyUI.Open(attackingCountry, 1, this, false);
        }

        // ���������, ��� �� ���������.
        if (!CheckIfSomebodyIsDefeated()) MakeAILeaderToChooseUnit();
    }

    /// <summary>
    /// ����� ������ ���������, ���������� ��� ���������.
    /// </summary>
    public void AdmitDefeat()
    {
        if (defendingCountry.Leader.IsPlayer)
        {
            EndBattle(false);
        }
        else
        {
            EndBattle(true);
        }
    }

    private void GiveFreeUnits()
    {
        defendingCountry.Army.GiveTemporaryUnits(districtAtStake);
    }

    private void StartBattleWithotPlayer()
    {
        // ���������� ���, � �������� ���� ����� ���� ������.
        // � ������������ ������� ��� �����. � ����������� - ���������� ������ �������.

        // ���� � ����� ������.
        if (attackingCountry.Army.Power > defendingCountry.Army.Power)
        {
            // ������� ���������.
            Debug.Log("� ���� ������������ ���� ��-�������, ������� ���������.");
            killUnitsInAIArmy(attackingCountry, System.Math.Abs(attackingCountry.Army.Units.Count - defendingCountry.Army.Units.Count));
            killUnitsInAIArmy(defendingCountry, defendingCountry.Army.Units.Count);

            EndBattle(false);
        }
        // ���� � ������ ������.
        else if (defendingCountry.Army.Power > attackingCountry.Army.Power)
        {
            // ������� ������������.
            Debug.Log("� ���� ������������ ���� ��-�������, ������� ������������.");
            killUnitsInAIArmy(defendingCountry, System.Math.Abs(defendingCountry.Army.Units.Count - attackingCountry.Army.Units.Count));
            killUnitsInAIArmy(attackingCountry, attackingCountry.Army.Units.Count);

            EndBattle(true);
        }
        // ���� �������.
        else
        {
            // ������� ������������.
            Debug.Log("� ���� ������������ ���� ��-�������, �� ���� �� �������.");
            killUnitsInAIArmy(attackingCountry, attackingCountry.Army.Units.Count);
            killUnitsInAIArmy(defendingCountry, defendingCountry.Army.Units.Count);

            EndBattle();
        }

        //if (Leader.MakeProbabilityWeightedDecision(50))
        //{
        //    
        //    EndBattle(true);
        //}
        //else
        //{
        //    // ������� ���������.
        //    Debug.Log("� ���� ������������ ���� ��-�������, ������� ���������.");
        //    EndBattle(false);
        //}
    }

    private void killUnitsInAIArmy(Country country, int numberOfUnitsToKill)
    {
        for (int i = 0; i < numberOfUnitsToKill; i++)
        {
            if (numberOfUnitsToKill < country.Army.Units.Count)
            {
                country.Army.Units[numberOfUnitsToKill].Health = -10;
            }
        }

        country.Army.DeleteDeadUnits(country);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
