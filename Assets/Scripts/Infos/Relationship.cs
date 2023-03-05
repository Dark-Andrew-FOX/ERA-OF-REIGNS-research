using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����, �������������� ����� ��������� ����� ����� ��������.
/// </summary>
public class Relationship
{
    int firstCountryId;
    int secondCountryId;
    bool atWar;
    int numberOfMoveToUnlockPeace;
    int numberOfMoveToUnlockWar;

    public Relationship()
    {

    }

    public Relationship(int firstCountryId, int secondCountryId)
    {
        this.firstCountryId = firstCountryId;
        this.secondCountryId = secondCountryId;
        atWar = false;
        numberOfMoveToUnlockPeace = -1;
        numberOfMoveToUnlockWar = -1;
    }

    /// <summary>
    /// Id ������ ������ �� ���������.
    /// </summary>
    public int FirstCountryId { get => firstCountryId; set => firstCountryId = value; }
    /// <summary>
    /// Id ������ ������ �� ���������.
    /// </summary>
    public int SecondCountryId { get => secondCountryId; set => secondCountryId = value; }
    /// <summary>
    /// ��������� �� ��� ������ � ��������� �����?
    /// </summary>
    public bool AtWar { get => atWar; set => atWar = value; }
    /// <summary>
    /// �� ����� ���� ����� ����� ���������� ��������� ���.
    /// </summary>
    public int NumberOfMoveToUnlockPeace { get => numberOfMoveToUnlockPeace; set => numberOfMoveToUnlockPeace = value; }
    /// <summary>
    /// �� ����� ���� ����� ����� �������� �����.
    /// </summary>
    public int NumberOfMoveToUnlockWar { get => numberOfMoveToUnlockWar; set => numberOfMoveToUnlockWar = value; }
}
