using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����� ��� �������� �������� �������.
/// </summary>
public class DestroyAfter : MonoBehaviour
{
    /// <summary>
    /// ����������� �����, ����� �������� ������ ���������������.
    /// </summary>
    /// <param name="destroyTime">����� � ��������, ����� �������� ����� �����������. 
    /// ���� ��� <= 0, �� ��������������� ����������� �� �����.</param>
    public void SetDestructionTime(float destroyTime)
    {
        if (destroyTime > 0)
        {
            Destroy(gameObject, destroyTime);
        }
    }
}
