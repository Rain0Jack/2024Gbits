using System.Collections.Generic;
using UnityEngine;

public class DragAndFillGame
{
    // 表示一句话的内容
    public string Sentence { get; private set; }
    // 空缺的地方在句子中的索引位置
    public List<int> MissingIndexes { get; private set; }
    // 每个空缺位置对应正确的词语 ID
    public List<string> CorrectWordIDs { get; private set; }

    // 构造函数，用于初始化
    public DragAndFillGame(string sentence, List<int> missingIndexes, List<string> correctWordIDs)
    {
        if (missingIndexes.Count != correctWordIDs.Count)
        {
            Debug.LogError("MissingIndexes 和 CorrectWordIDs 的数量不匹配！");
            return;
        }

        Sentence = sentence;
        MissingIndexes = missingIndexes;
        CorrectWordIDs = correctWordIDs;
    }
}
