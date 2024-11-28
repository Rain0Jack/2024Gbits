using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
public class WordsCollect : Singleton<WordsCollect>
{
    private WordsCollect() { }
    // Start is called before the first frame update
    public Dictionary<string, string> AllWords;
}
