using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������������ ����� �����-����������.
/// </summary>
public class DistrictInfo
{
    // �������� ������.
    private string districtName;
    // ������� �������� ������.
    [JsonIgnore] public Country holder;
    // ������ �� �����-����������.
    [JsonIgnore] public District districtFunc;

    public DistrictInfo()
    {

    }

    public DistrictInfo(District district)
    {
        DistrictName = district.name;
        GenerateBonus();
        DestroyAllBuildings();
    }

    /// <summary>
    /// �������� ������.
    /// </summary>
    public string DistrictName { get => districtName; set => districtName = value; }

    #region ����� ������
    /// <summary>
    /// ����� ������. 0 - ��� ������, 1 - ������, 2 - ������, 3 - ��������������, 4 - ���� ������.
    /// </summary>
    public int DistrictBonus { get => districtBonus; set => districtBonus = value; }
    int districtBonus;
    public void GenerateBonus()
    {
        districtBonus = Random.Range(0, 5);
    }
    #endregion

    #region ��������� ������..
    /// <summary>
    /// ��������� "���������� ������ ������".
    /// </summary>
    public bool HasBonusProduction { get => hasBonusProduction; set => hasBonusProduction = value; }
    bool hasBonusProduction;
    public void BuildBonusProduction()
    {
        holder.Gold += District.gameManager.gameSession.GameRules.CostOfBonusProduction;
        hasBonusProduction = true;
        switch (DistrictBonus)
        {
            case 2:
                holder.Iron += District.gameManager.gameSession.GameRules.IronBonus;
                break;
            case 3:
                holder.Horses += District.gameManager.gameSession.GameRules.HorsesBonus;
                break;
        }

        District.gameManager.UpdateStats();
        District.gameManager.districtUI.UpdateIfNeeded();

        Debug.Log("����� " + holder.Leader.LeaderName + " �������� ��������� \"���������� ������ ������\" � ������ " + DistrictName);
    }

    /// <summary>
    /// ��������� "������ ������".
    /// </summary>
    public bool HasGoldProduction { get => hasGoldProduction; set => hasGoldProduction = value; }
    bool hasGoldProduction;
    public void BuildGoldProduction()
    {
        holder.Gold += District.gameManager.gameSession.GameRules.CostOfGoldProduction;
        hasGoldProduction = true;

        District.gameManager.UpdateStats();
        District.gameManager.districtUI.UpdateIfNeeded();

        Debug.Log("����� " + holder.Leader.LeaderName + " �������� ��������� \"������ ������\" � ������ " + DistrictName);
    }

    /// <summary>
    /// ��������� "���� ������������".
    /// </summary>
    public bool HasSecurityBureau { get => hasSecurityBureau; set => hasSecurityBureau = value; }
    bool hasSecurityBureau;
    public void BuildSecurityBureau()
    {
        holder.Gold += District.gameManager.gameSession.GameRules.CostOfAgentsProduction;
        holder.Agents += District.gameManager.gameSession.GameRules.AgentsProductionBonus;
        hasSecurityBureau = true;

        District.gameManager.UpdateStats();
        District.gameManager.districtUI.UpdateIfNeeded();

        Debug.Log("����� " + holder.Leader.LeaderName + " �������� ��������� \"���� ������������\" � ������ " + DistrictName);

    }

    /// <summary>
    /// ��������� "��������".
    /// </summary>
    public bool HasMonument { get => hasMonument; set => hasMonument = value; }
    bool hasMonument;
    public void BuildMonument()
    {
        holder.Gold += District.gameManager.gameSession.GameRules.CostOfMonument;
        holder.VictoryPoints += District.gameManager.gameSession.GameRules.MonumentBonus;
        hasMonument = true;

        District.gameManager.UpdateStats();
        District.gameManager.districtUI.UpdateIfNeeded();

        Debug.Log("����� " + holder.Leader.LeaderName + " �������� ��������� \"��������\" � ������ " + DistrictName);
    }

    /// <summary>
    /// ���������� ��� ��������� (���������) � ���� ������.
    /// </summary>
    public void DestroyAllBuildings()
    {
        hasBonusProduction = false;
        hasGoldProduction = false;
        hasSecurityBureau = false;
        hasMonument = false;
    }

    #endregion
}
