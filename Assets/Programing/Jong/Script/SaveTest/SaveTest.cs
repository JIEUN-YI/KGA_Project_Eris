using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveTest : MonoBehaviour
{
    
   

    static GameObject container;

    // ---�̱������� ����--- //
    static SaveTest instance;
    public static SaveTest Instance
    {
        get
        {
            if (!instance)
            {
                container = new GameObject();
                container.name = "DataManager";
                instance = container.AddComponent(typeof(SaveTest)) as SaveTest;
                DontDestroyOnLoad(container);
            }
            return instance;
        }
    }

    // --- ���� ������ �����̸� ���� ("���ϴ� �̸�(����).json") --- //
    string GameDataFileName = "GameData.json";

    // --- ����� Ŭ���� ���� --- //
    public Data data = new Data();


    // �ҷ�����
    public void LoadGameData()
    {
        string filePath = Application.persistentDataPath + "/" + GameDataFileName;

        // ����� ������ �ִٸ�
        if (File.Exists(filePath))
        {
            // ����� ���� �о���� Json�� Ŭ���� �������� ��ȯ�ؼ� �Ҵ�
            string FromJsonData = File.ReadAllText(filePath);
            data = JsonUtility.FromJson<Data>(FromJsonData);
            print("�ҷ����� �Ϸ�");
        }
    }


    // �����ϱ�
    public void SaveGameData()
    {
        // Ŭ������ Json �������� ��ȯ (true : ������ ���� �ۼ�)
        string ToJsonData = JsonUtility.ToJson(data, true);
        string filePath = Application.persistentDataPath + "/" + GameDataFileName;

        // �̹� ����� ������ �ִٸ� �����, ���ٸ� ���� ���� ����
        File.WriteAllText(filePath, ToJsonData);

        // �ùٸ��� ����ƴ��� Ȯ�� (�����Ӱ� ����)
        print("���� �Ϸ�");
        for (int i = 0; i < data.isUnlock.Length; i++)
        {
            print($"{i}�� é�� ��� ���� ���� : " + data.isUnlock[i]);
        }
        print($"����{data.vol}");
        print($"���ݷ�{data.atk}");
        print($"���ǵ�{data.speed}");
        print($"ȣ����{data.hover}");
        print($"�뽬{data.dash}");
    }
}