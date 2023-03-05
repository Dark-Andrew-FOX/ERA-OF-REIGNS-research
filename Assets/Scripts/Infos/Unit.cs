using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����, �������������� ����� ����� � �����.
/// </summary>
public class Unit
{
    /// <summary>
    /// ��� �����.
    /// </summary>
    public string UnitName { get => unitName; set => unitName = value; }
    /// <summary>
    /// ��� �����.
    /// 0 - ����; 1 - ��������� ����; 2 - �������; 3 - ������ �������.
    /// </summary>
    public int Type { get => type; set => type = value; }
    /// <summary>
    /// �������� �� ���� ���������?
    /// </summary>
    public bool IsTemporary { get => isTemporary; set => isTemporary = value; }
    /// <summary>
    /// ����, ������� ������� ���� ����.
    /// </summary>
    public int Damage { get => damage; set => damage = value; }
    /// <summary>
    /// ��������, ������� ���� � ����� �����.
    /// </summary>
    public int Health { get => health; set => health = value; }
    /// <summary>
    /// ������������ ����, ������� ���� ���� ����� ��������.
    /// </summary>
    public int MaxDamage { get => maxDamage; set => maxDamage = value; }
    /// <summary>
    /// �������� ��������, ������� ����� ���� � ����� �����.
    /// </summary>
    public int MaxHealth { get => maxHealth; set => maxHealth = value; }

    private string unitName;
    private int type;
    private bool isTemporary;

    private int damage;
    private int maxDamage;

    private int health;
    private int maxHealth;

    public Unit() { }

    public Unit(string unitName, int type, bool isTemporary, int maxDamage, int maxHealth)
    {
        this.unitName = unitName;
        this.type = type;
        this.isTemporary = isTemporary;
        if (isTemporary) this.unitName += " (����.)";

        this.MaxDamage = maxDamage;
        damage = maxDamage;
        this.MaxHealth = maxHealth;
        health = maxHealth;
    }

    /// <summary>
    /// ��������������� ��� �������������� �����.
    /// </summary>
    public void Heal()
    {
        damage = MaxDamage;
        health = MaxHealth;
    }

    /// <summary>
    /// ������� ����.
    /// </summary>
    /// <param name="damageToDealWith">����, ������� ���������� ����� ����� (�������� ������ ���� �� ������ ����).</param>
    /// <returns>true, ���� ���� �������� (�������� ����� ������ ��� ����� 0), ����� false.</returns>
    public bool TakeDamage(int damageToDealWith)
    {
        health -= damageToDealWith;

        return health <= 0;
    }
}
