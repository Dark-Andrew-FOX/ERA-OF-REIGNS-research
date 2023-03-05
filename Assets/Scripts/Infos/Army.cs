using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Army
{
    private List<Unit> units = new List<Unit>();
    private int filledCapacity;
    private int power;

    public List<Unit> Units { get => units; set => units = value; }
    public int FilledCapacity { get => filledCapacity; set => filledCapacity = value; }
    public int Power { get => power; set => power = value; }

    public void AddNewUnit(Unit unitToAdd, Country country)
    {
        Units.Add(unitToAdd);
        Power += LeaderMonoBehaviour.GameManager.gameSession.GameRules.Damages[unitToAdd.Type];

        if (!unitToAdd.IsTemporary)
        {
            GameRules gameRules = LeaderMonoBehaviour.GameManager.gameSession.GameRules;

            filledCapacity += gameRules.CapacityRequirements[unitToAdd.Type];
            country.Gold -= gameRules.GoldRequirements[unitToAdd.Type];
            country.Iron -= gameRules.IronRequirements[unitToAdd.Type];
            country.Horses -= gameRules.HorsesRequirements[unitToAdd.Type];
            country.Agents -= gameRules.AgentsRequirements[unitToAdd.Type];
        }
    }

    public void DeleteUnit(Unit unitToDelete, Country country)
    {
        Units.Remove(unitToDelete);
        Power -= LeaderMonoBehaviour.GameManager.gameSession.GameRules.Damages[unitToDelete.Type];

        if (!unitToDelete.IsTemporary)
        {
            // ������ ����� ������ � �������.
            GameRules gameRules = LeaderMonoBehaviour.GameManager.gameSession.GameRules;

            filledCapacity -= gameRules.CapacityRequirements[unitToDelete.Type];
            country.Gold += gameRules.GoldRequirements[unitToDelete.Type];
            country.Iron += gameRules.IronRequirements[unitToDelete.Type];
            country.Horses += gameRules.HorsesRequirements[unitToDelete.Type];
            country.Agents += gameRules.AgentsRequirements[unitToDelete.Type];
        }
    }

    public void KillUnit(Unit unitToKill, Country country)
    {
        Units.Remove(unitToKill);
        Power -= LeaderMonoBehaviour.GameManager.gameSession.GameRules.Damages[unitToKill.Type];

        if (!unitToKill.IsTemporary)
        {
            // ������ ����� �������.
            GameRules gameRules = LeaderMonoBehaviour.GameManager.gameSession.GameRules;

            filledCapacity -= gameRules.CapacityRequirements[unitToKill.Type];
            country.Iron += gameRules.IronRequirements[unitToKill.Type];
            country.Horses += gameRules.HorsesRequirements[unitToKill.Type];
            country.Agents += gameRules.AgentsRequirements[unitToKill.Type];
        }
    }

    /// <summary>
    /// ���������, �������� �� ���������� ����� � �����.
    /// </summary>
    /// <param name="id">id �����, �������� ����� ���������.</param>
    /// <param name="country">������, � ����� ������� ����� ��������� ����������� ����������.</param>
    /// <returns>true, ���� ��������, ����� false.</returns>
    public bool CheckIfBuyngPossible(int id, Country country)
    {
        GameRules gameRules = LeaderMonoBehaviour.GameManager.gameSession.GameRules;

        return (gameRules.CapacityRequirements[id] + country.Army.FilledCapacity <= country.CounterOfDistrictsEverHelded &&
                country.Gold - gameRules.GoldRequirements[id] >= 0 &&
                country.Iron - gameRules.IronRequirements[id] >= 0 &&
                country.Horses - gameRules.HorsesRequirements[id] >= 0 &&
                country.Agents - gameRules.AgentsRequirements[id] >= 0);
    }

    /// <summary>
    /// ������������ ������ ����� ���� id.
    /// </summary>
    /// <param name="id">0 - ����, 1 - ��������� ����, 2 - �������, 3 - ����� ������ �������.</param>
    /// <param name="isTemp">��������� ����?</param>
    /// <returns></returns>
    public Unit GetUnit(int id, bool isTemp)
    {
        GameRules gameRules = LeaderMonoBehaviour.GameManager.gameSession.GameRules;

        switch (id)
        {
            case 0:
                return new Unit("����", 0, isTemp, gameRules.Damages[0], gameRules.Healths[0]);
            case 1:
                return new Unit("��������� ����", 1, isTemp, gameRules.Damages[1], gameRules.Healths[1]);
            case 2:
                return new Unit("�������", 2, isTemp, gameRules.Damages[2], gameRules.Healths[2]);
            case 3:
                return new Unit("����� ������ �������", 3, isTemp, gameRules.Damages[3], gameRules.Healths[3]);
            default:
                Debug.LogError("������ ��������� ����� �� id " + id);
                return null;
        }
    }

    public void DeleteTemporaryUnits()
    {
        //Units.RemoveAll((Unit) => Unit.IsTemporary);

        //Debug.LogError("��������� ����� ���� �������. ������ ����� ������� �� " + Units.Count + " ������.");
        int i = -1;
        while(i < units.Count)
        {
            //Debug.LogError(i);
            ++i;
            if (i < units.Count && units[i].IsTemporary)
            {
                DeleteUnit(units[i], null);
                Debug.Log("����� ��������� ����");
                i = -1;
            }
        }
    }

    public void DeleteDeadUnits(Country country)
    {
        //Units.RemoveAll((Unit) => Unit.Health <= 0);
        //
        //Debug.LogError("̸����� ����� ���� �������. ������ ����� ������� �� " + Units.Count + " ������.");

        int i = -1;
        while (i < units.Count)
        {
            //Debug.LogError(i);
            ++i;
            if (i < units.Count && units[i].Health <= 0)
            {
                KillUnit(units[i], country);
                Debug.Log("����� ������ ����");
                i = -1;
            }
        }
    }

    public void GiveTemporaryUnits(DistrictInfo protectingDistrict)
    {
        // ����� ���� "����" � ����� ������.
        AddNewUnit(GetUnit(0, true), protectingDistrict.holder);
        AddNewUnit(GetUnit(0, true), protectingDistrict.holder);

        // ����� ��� ��������� "���������� ������ ������".
        if (protectingDistrict.HasBonusProduction)
        {
            if (protectingDistrict.DistrictBonus == 3) AddNewUnit(GetUnit(2, true), protectingDistrict.holder);
            else if (protectingDistrict.DistrictBonus == 2) AddNewUnit(GetUnit(1 , true), protectingDistrict.holder);
        }

        // ����� ��� ��������� "���� ������������".
        if (protectingDistrict.HasSecurityBureau) AddNewUnit(GetUnit(3, true), protectingDistrict.holder);
    }
}
