using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �����, ��������������� ��� ����������� �������� ������ (����) � UI �����.
/// </summary>
public class UnitUI : MonoBehaviour
{
    private Unit unit;
    // 0 - �������, 1 - � �����, 2 - ����.
    private int buttonState;
    private ArmyUI armyUI;

    [SerializeField] TMP_Text unitName;
    [SerializeField] TMP_Text damage;
    [SerializeField] TMP_Text health;
    [SerializeField] TMP_Text buttonText;
    [SerializeField] Button button;
    [SerializeField] GameObject deadImage;

    public Unit Unit { get => unit; set => unit = value; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="armyUI"></param>
    /// <param name="buttonState">0 - ��� ��� (����� �������� � �������� ������),
    /// 1 - ��� ��� (����� ������� �����), 2 - ��� ��� (������ ������� �����).</param>
    public void Create(Unit unit, ArmyUI armyUI, int buttonState)
    {
        unitName.text = unit.UnitName;
        damage.text = "�����: " + unit.Damage;
        health.text = "��������: " + unit.Health + "/" + unit.MaxHealth;

        this.unit = unit;
        this.armyUI = armyUI;
        this.buttonState = buttonState;

        switch (buttonState)
        {
            case 0:
                button.gameObject.SetActive(true);
                buttonText.text = "�������";
                if (unit.IsTemporary) button.interactable = false;
                break;
            case 1:
                button.gameObject.SetActive(true);
                buttonText.text = "��������� � �����";
                break;
            case 2:
                button.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// ���������� ������������ ������ ����� (��-�� ��� ������).
    /// </summary>
    public void Kill()
    {
        // ��������� ������.
        gameObject.AddComponent<Rigidbody2D>();
        Rigidbody2D rigidbody2d = gameObject.GetComponent<Rigidbody2D>();
        rigidbody2d.gravityScale = 50;
        rigidbody2d.interpolation = RigidbodyInterpolation2D.Extrapolate;
        if (armyUI.IsDefender) gameObject.transform.rotation = Quaternion.Euler(0, 0, Random.Range(-30f, -10f));
        else gameObject.transform.rotation = Quaternion.Euler(0, 0, Random.Range(30f, 10f));
        deadImage.gameObject.SetActive(true);

        // ������ ������.
        Destroy(gameObject, 4);
    }

    public void ButtonAction()
    {
        switch (buttonState)
        {
            case 0:
                armyUI.Country.Army.DeleteUnit(unit, armyUI.Country);
                armyUI.UpdateList();
                break;
            case 1:
                button.gameObject.SetActive(false);
                armyUI.FightManager.UnitChoosed(armyUI.IsDefender, this);
                break;
            default:
                break;
        }
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
