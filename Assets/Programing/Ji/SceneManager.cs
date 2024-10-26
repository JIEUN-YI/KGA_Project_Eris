using UnityEngine;

public class SceneManager : MonoBehaviour
{
    // Scene�� ���¸� ����
    public enum SceneState { Start, Phase1, Phase2 };
    /* Start : Scene�� ���� ����, �������ڸ����� ����
     * Phase1 : ��縦 ���� ����� �� Phase1 ����
     *          - Phase1�̸鼭 ������ ���°� Phase2�� ���� �� Phase2�� ����
     *          - Boss�� Phase�� ������ �и�����, �ִϸ��̼ǰ� ���·θ� �и������� ���� �޶�������
     * Phase2 : Phase2�� �����ڸ��� ������ ��� ��ũ��Ʈ�� ���
     *          - Phase2�� ��, ������ ����ϸ� ��� ��ũ��Ʈ�� ����ؾ� ��
     */
    public SceneState nowSceneState; // ���� ���� ���¸� ����

    [SerializeField] DialogueSystem dialogueSystem; // DialogueSystem.cs�� �Ҵ�

    private void Awake()
    {
        nowSceneState = SceneState.Start; // ���� �������ڸ��� Start ���·� ����
    }

    private void Update()
    {
        // ���� ���� ���¿� ���� �����ϴ� �Լ��� ���� - ��������
        switch (nowSceneState)
        {
            case SceneState.Start:
                dialogueSystem.StartDialogue(); // DialogueSystem.cs���� ���۽� ����ϴ� ���
                break;
            case SceneState.Phase1:
                dialogueSystem.Phase1Dialogue(); // DialogueSystem.cs���� Phase1 ����� ����ϴ� ���
                break;
            case SceneState.Phase2:
                dialogueSystem.Phase2Dialogue(); // DialogueSystem.cs���� Phase2 ����� ����ϴ� ���
                break;
        }

    }
}
