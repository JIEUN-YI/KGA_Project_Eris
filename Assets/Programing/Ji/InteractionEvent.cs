using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// DatabaseManager ���� ������ ��� �����͸� �ҷ����� ����
public class InteractionEvent : MonoBehaviour
{
    // Dialogue.cs�� DialogueEvent�� ����ϱ� ����
    [SerializeField] DialogueEvent dialogueEvent;




    /// <summary>
    /// ������ ��� �����͸� �������� ���� �Լ�
    /// </summary>
    /// <returns></returns>
    public Dialogue[] GetDialogue()
    {
        // DialogueEvent�� dialogues�� Dialogue[]�� ���·� ����ϱ����� ��縦 �����ϴ� ������
        // DatabaseManager�� GetDialogues�Լ����� �� �̺�Ʈ�� ���Ǵ� ��縦 ���������� �ϸ�
        // Dialogue.cs���� DialogueEvent�� ������� Vector2 line�� ���� int�� ���� ����ȯ �� ���
        dialogueEvent.dialogues = DatabaseManager.instance.GetDialogues((int)dialogueEvent.line.x, (int)dialogueEvent.line.y);
        return dialogueEvent.dialogues;
    }
}
