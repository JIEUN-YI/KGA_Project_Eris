using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestChara : MonoBehaviour
{
    [SerializeField] float moveSpeed;
   
    [SerializeField] float Atk;

    [SerializeField] bool firstStage;
    [SerializeField] bool secondStage;
    [SerializeField] bool thridStage;

    [SerializeField] bool ondash;
    [SerializeField] bool onhover;
    [Range(0, 1)]
    [SerializeField] float BgmVol;


    // ������ ���� �� �ҷ����� ���� , �����͸� ���� �� �ҷ����⸦ �Ϸ��� LoadGameData() , SaveGameData();  �� ���� �Ǿ�� �� 
    // ������ �ʿ��� Ÿ�ֿ̹� �Ʒ��� ����ϰ� �ۼ��ؼ� ���� �� �ҷ����⸦ �ؾ��� 
    private void Start()
    {
        SaveTest.Instance.LoadGameData(); // �ҷ�����
        Loading();
    }

    private void OnApplicationQuit()
    {
        Saving();
        SaveTest.Instance.SaveGameData(); // �����ϱ� 
    }

    public void Saving() // ���� �ִ� �ν��Ͻ��� ������ ���� 
    {
        SaveTest.Instance.data.isUnlock[0] = firstStage;
        SaveTest.Instance.data.isUnlock[1] = secondStage;
        SaveTest.Instance.data.isUnlock[2] = thridStage;


        SaveTest.Instance.data.vol = BgmVol;
   

        SaveTest.Instance.data.dash = ondash;
        SaveTest.Instance.data.hover = onhover;
    }
    public void Loading() // json ������ ���Ͽ��� �ҷ��� �����͸� �̾ƿ� 
    {
        firstStage = SaveTest.Instance.data.isUnlock[0];
        secondStage = SaveTest.Instance.data.isUnlock[1];
        thridStage = SaveTest.Instance.data.isUnlock[2];


        BgmVol = SaveTest.Instance.data.vol;
     

        ondash = SaveTest.Instance.data.dash;
        onhover = SaveTest.Instance.data.hover;
    }

}
