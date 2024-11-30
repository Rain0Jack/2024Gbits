using System.Collections.Generic;
using UnityEngine;

public class DragAndFillGame
{
    // ��ʾһ�仰������
    public string Sentence { get; private set; }
    // ��ȱ�ĵط��ھ����е�����λ��
    public List<int> MissingIndexes { get; private set; }
    // ÿ����ȱλ�ö�Ӧ��ȷ�Ĵ��� ID
    public List<string> CorrectWordIDs { get; private set; }

    // ���캯�������ڳ�ʼ��
    public DragAndFillGame(string sentence, List<int> missingIndexes, List<string> correctWordIDs)
    {
        if (missingIndexes.Count != correctWordIDs.Count)
        {
            Debug.LogError("MissingIndexes �� CorrectWordIDs ��������ƥ�䣡");
            return;
        }

        Sentence = sentence;
        MissingIndexes = missingIndexes;
        CorrectWordIDs = correctWordIDs;
    }
}
