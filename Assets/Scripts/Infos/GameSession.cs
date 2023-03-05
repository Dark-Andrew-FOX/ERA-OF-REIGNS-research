using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������������ ����� ������� ������ (��� ����������, �������� � �������� ������� ������).
/// ���� ����� ����� �������� ����� ������, �� ����� ����� ������ ������� ����������.
/// </summary>
public class GameSession
{
    // ��� ���� ������.
    private string sessionName;
    // ���� ���������� ������� ���� ������.
    private string lastPlayingTime;
    // ����� ���� ������.
    private int mapId;

    // ������-���������� ���� ������.
    private List<Country> countries;
    // ������ �������-���������� ������ ����� ���� ������.
    private List<DistrictInfo> districtsInfos;
    // ������ ��������� ����� ��������.
    private List<Relationship> relationships;

    // ������� ���.
    private int currentMove;
    // ����� ������, ������� ��������� ������� ���.
    private int currentCountry = 0;
    // ������� ����������� ������� �������� �����, ��� ��� ������, �� ���� ���.
    private int conqueredNeutralDistrict;

    // ������� ������� ���� ������.
    private GameRules gameRules;

    // ������ �� � ���� ������ ����� ����?
    private bool gameOver;
    // ��������� � ����� ����.
    private string gameOverMessage;
    // ���������� ������.
    private int winnerCountryId;

    public string SessionName { get => sessionName; set => sessionName = value; }
    public string LastPlayingTime { get => lastPlayingTime; set => lastPlayingTime = value; }
    public int MapId { get => mapId; set => mapId = value; }
    public List<Country> Countries { get => countries; set => countries = value; }
    public List<DistrictInfo> DistrictsInfos { get => districtsInfos; set => districtsInfos = value; }
    public List<Relationship> Relationships { get => relationships; set => relationships = value; }
    public int CurrentMove { get => currentMove; set => currentMove = value; }
    public int CurrentCountry { get => currentCountry; set => currentCountry = value; }
    public int ConqueredNeutralDistrict { get => conqueredNeutralDistrict; set => conqueredNeutralDistrict = value; }
    public GameRules GameRules { get => gameRules; set => gameRules = value; }
    public bool GameOver { get => gameOver; set => gameOver = value; }
    public string GameOverMessage { get => gameOverMessage; set => gameOverMessage = value; }
    public int WinnerCountryId { get => winnerCountryId; set => winnerCountryId = value; }

    public GameSession()
    {

    }

    public GameSession(int mapId)
    {
        this.MapId = mapId;

        LastPlayingTime = DateTime.Now.ToString();
        SessionName = "Save " + LastPlayingTime;
        CurrentMove = 0;

        Countries = new List<Country>();
        DistrictsInfos = new List<DistrictInfo>();
        Relationships = new List<Relationship>();

        GameRules = new GameRules();
    }

    public GameSession(int mapId, GameRules gameRules) : this(mapId)
    {
        this.GameRules = gameRules;
    }

    public void EndGame(string gameOverMessage, int winnerCountryId)
    {
        gameOver = true;
        this.gameOverMessage = gameOverMessage;
        this.winnerCountryId = winnerCountryId;
    }

    public DistrictInfo FindDistrictById(int id)
    {
        if (id < 0 || id > DistrictsInfos.Count)
        {
            Debug.LogError("��� �������� ����� � id " + id + ", �� ������ ������ ���!");
            return null;
        }

        return DistrictsInfos[id];
    }

    /// <summary>
    /// ���� ��������� ����� �������� � ������� id.
    /// </summary>
    /// <param name="firstId">������ 1(2)</param>
    /// <param name="secondId">������ 2(1)</param>
    /// <returns>��������� ����� ����� ��������. Null, ���� ��������� ����� ����� �������� ���.</returns>
    public Relationship FindRelationship(int firstId, int secondId)
    {
        for (int i = 0; i < Relationships.Count; i++)
        {
            if (Relationships[i].FirstCountryId == firstId && Relationships[i].SecondCountryId == secondId ||
                Relationships[i].FirstCountryId == secondId && Relationships[i].SecondCountryId == firstId)
            {
                return Relationships[i];
            }
        }

        return null;
    }
    
    /// <summary>
    /// ��������� ��� ����� �� ����� � ������ �� ���������� ��������� � ������ ��.
    /// </summary>
    public void AddUnaddedRelationships()
    {
        for (int i = 0; i < Countries.Count; i++)
        {
            for (int j = 0; j < Countries.Count; j++)
            {
                // � ������ � ����� � ����� ��� ���������.
                if (i == j)
                    continue;
                // ���� ��������� ��� ����, ����������.
                if (FindRelationship(Countries[i].CountryId, Countries[j].CountryId) != null)
                {
                    continue;
                }
                // ����� - ������.
                else
                {
                    Relationships.Add(new Relationship(Countries[i].CountryId, Countries[j].CountryId));
                }
            }
        }
    }

    public void UpdateLastPlayingTime()
    {
        LastPlayingTime = DateTime.Now.ToString();
    }
}
