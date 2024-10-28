using TMPro;
using UnityEngine;
using static SceneManager;
/* ��� ��ũ��Ʈ ���� ��Ʃ�� ����
 * https://www.youtube.com/watch?v=DPWvoUlHbjg&list=PLUZ5gNInsv_NG_UKZoua8goQbtseAo8Ow&index=11
 * https://www.youtube.com/watch?v=1fRbGvQlIEQ
 * https://www.youtube.com/watch?v=_04sCWLHoXU
 * https://www.youtube.com/watch?v=qJjfYvEYKiE
 * 
 * ��縦 Ÿ���������� ����ϴ� �� ���� ��Ʃ�� ����
 * https://www.youtube.com/watch?v=OjcPuEVQT6s
 */
public class DialogueManager : MonoBehaviour
{
    // DatabaseManager.cs�� GetDialogues() �Լ��� ����ϱ� ���� ����
    [SerializeField] DatabaseManager databaseManager;

    [Header("UI")]
    [SerializeField] private GameObject imgBoss; // ���� ĳ������ ��ȭ �� ��� �̹���
    [SerializeField] private GameObject imgPlayer; // �÷��̾� ĳ������ ��ȭ �� ��� �̹���

    [Header("Test")]
    [SerializeField] private TextMeshProUGUI textName; // ��ȭ�ϴ� ĳ������ �̸� text
    [SerializeField] private TextMeshProUGUI textDialogue; // ĳ������ ��� text
    /*
        /// <summary>
        /// �̺�Ʈ�� �����ϴ� ��ȣ�� ������ ��ȣ�� ���� Text�� �˸��� ��ġ�� ����ϰ�
        /// �̹����� �ٲٴ� �Լ�
        /// </summary>
        /// <param name="startNum"></param>
        /// <param name="endNum"></param>
        public void NowDialogueArr(int startNum, int endNum)
        {
            // nowStart�� �����ͺ��̽����� ����� �迭�� ������ �ҷ���
            Dialogue[] nowStart = databaseManager.GetDialogues(startNum, endNum);

            int i = 0; // �迭�� ��������

            // do - while ������ ������ Ȯ���ϸ鼭 �ݺ�
            do // ������ ù��° �迭�� ���
            {
                Debug.Log(nowStart[i].eventName);
                Debug.Log(nowStart[i].unitId);
                Debug.Log(nowStart[i].name);
                int j = 0; // nowStart[i]�� �迭�� contexts[j]�� Ž���Ͽ� ����ϱ� ���� ������ ����
                Debug.Log(nowStart[i].contexts[j]);
                //ShowText(nowStart, i, j);
                if (++i < nowStart.Length) // �迭�� ���̰� ����� �ʴ� ������
                {
                    while (nowStart[i].name == "") // �̸��� ������ ��쿡�� ���� ĳ���Ͱ� ���� ��縦 �����
                    {
                        j++;
                        Debug.Log(nowStart[i].contexts[j]);
                        ShowText(nowStart, i, j);
                    }
                }
            } while (nowStart[i-1].eventName != "end"); // nowStart�迭�� �̺�Ʈ�̸����� end�� ������ ���� �� ��� �ݺ����� ����
        }

        private void Start()
        {
            NowDialogueArr(0, 4);
        }
    */


    /// <summary>
    /// SceneManager ���� ������ Scene�� ���¿� ���� DatabaseManager.cs�� GetDialogues�Լ��� �̿���
    /// �� ��ġ�� �˸��� �����͸� Dialogue[] nowDialogueArr�� ���·� �����Ͽ� ����
    /// </summary>
    /// <param name="nowSceneState"></param>
    /// <returns></returns>
    public Dialogue[] cheakEvent(SceneState nowSceneState)
    {
        Dialogue[] nowDialogueArr;
        switch (nowSceneState)
        {
            case SceneState.Start:
                nowDialogueArr = databaseManager.GetDialogues(0, 5);
                return nowDialogueArr;
            case SceneState.Phase1Talk:
                nowDialogueArr = databaseManager.GetDialogues(6, 10);
                return nowDialogueArr;
            case SceneState.MonsterDie:
                nowDialogueArr = databaseManager.GetDialogues(11, 17);
                return nowDialogueArr;
        }
        return null;
    }

    /// <summary>
    /// ���� �������� count�� �˸��� �̸��� ���
    /// ���⼭ count�� Ű�� �Է��Ͽ� ��縦 �ѱ� Ƚ��
    /// </summary>
    /// <param name="nowDialogue"></param>
    /// <param name="count"></param>
    private void ShowTextName(Dialogue[] nowDialogue, int count)
    {
        // ��簡 �� �� �̶� �̸��� ������ ��찡 �ֱ⿡ ������ �ƴϸ� �̸��� ���
        if (nowDialogue[count].name != "")
        {
            textName.text = nowDialogue[count].name.ToString();
        }
        // ������ ��쿡�� ������ �̸��� �״�� ����ϹǷ� ��¿� ������ ����
    }

    /// <summary>
    /// ���� �������� count�� �˸´� ĳ����Id�� ã�Ƽ� ID�� �´� ĳ���ʹ� �����ϰ�
    /// �ƴ� ĳ���ʹ� ��Ӱ� ǥ���ϱ�
    /// </summary>
    /// <param name="nowDialogue"></param>
    /// <param name="count"></param>
    private void ShowImgName(Dialogue[] nowDialogue, int count)
    {


    }
}
