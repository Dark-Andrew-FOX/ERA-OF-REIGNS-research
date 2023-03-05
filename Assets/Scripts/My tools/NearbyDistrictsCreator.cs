using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NearbyDistrictsCreator : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] TMP_Text currentText;
    [SerializeField] TMP_Text nearbyCountText;
    [SerializeField] [Range(0, 56)] int currentDistrictIndex = 0;
    public bool readyToStart;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (readyToStart)
        {
            currentText.text = currentDistrictIndex.ToString() + ") " + GameManager.districts[currentDistrictIndex].name;

            // ������������ ��� �������� ������ ��������.
            if (GameManager.districts[currentDistrictIndex].nearbyDistricts != null)
            {
                nearbyCountText.text = "Nearby count: " + GameManager.districts[currentDistrictIndex].nearbyDistricts.Count.ToString();
                for (int i = 0; i < GameManager.districts[currentDistrictIndex].nearbyDistricts.Count; i++)
                {
                    GameManager.districts[currentDistrictIndex].nearbyDistricts[i].HighlightDistrict(0.06f);
                }
            }
        }
    }

    public void HiglightCurrent()
    {
        GameManager.districts[currentDistrictIndex].HighlightDistrict(2f);
    }

    public void DistrictAction(District district)
    {
        // ������, ������ �� �� ���� ��.
        if (GameManager.districts[currentDistrictIndex] == district)
        {
            Debug.LogWarning("���� ����� ����� ������������. �� �� ����� ���� ������� � ����� � �����!");
        }
        else
        {
            // �������� �� ���� �������� ������� � ������, �������� �� ��� district.
            if (GameManager.districts[currentDistrictIndex].nearbyDistricts.Count == 0)
            {
                Debug.LogWarning("����� ��� �� ��� ��������. ���������...");
                AddDistrict(district);
            }
            else
            {
                Debug.LogWarning("����� ��� �� ��� ��������. ���������...");
                AddDistrict(district);
                //for (int i = 0; i < GameManager.districts[currentDistrictIndex].nearbyDistricts.Count; i++)
                //{
                //    
                //    //if (GameManager.districts[currentDistrictIndex].nearbyDistricts[i] == district)
                //    //{
                //    //    Debug.LogWarning("����� ��� ��� ��������. �������...");
                //    //    RemoveDistrict(district);
                //    //}
                //    //else
                //    //{
                //    //    Debug.LogWarning("����� ��� �� ��� ��������. ���������...");
                //    //    AddDistrict(district);
                //    //}
                //}
            }
        }
    }

    public void AddDistrict(District districtToAdd)
    {
        if (GameManager.districts[currentDistrictIndex].nearbyDistricts == null)
        {
            GameManager.districts[currentDistrictIndex].nearbyDistricts = new List<District>();
        }

        GameManager.districts[currentDistrictIndex].nearbyDistricts.Add(districtToAdd);
    }

    public void RemoveDistrict(District districtToRemove)
    {
        GameManager.districts[currentDistrictIndex].nearbyDistricts.Remove(districtToRemove);
    }

    public void NextDistrictToSetup()
    {
        if (currentDistrictIndex < GameManager.districts.Count - 1)
        {
            ++currentDistrictIndex;
        }
        else
        {
            Debug.LogWarning("����� ������!");
        }

        gameManager.CameraController.FocusCamera(GameManager.districts[currentDistrictIndex].gameObject);
    }

    public void PrevDistrictToSetup()
    {
        if (currentDistrictIndex != 0)
        {
            --currentDistrictIndex;
        }
        else
        {
            Debug.LogWarning("����� ������ � ������!");
        }

        gameManager.CameraController.FocusCamera(GameManager.districts[currentDistrictIndex].gameObject);
    }
}
