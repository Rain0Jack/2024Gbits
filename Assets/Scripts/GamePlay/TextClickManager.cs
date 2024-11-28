using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using QFramework;
public class TextClickManager : Singleton<TextClickManager>
{
    private TextClickManager() { }
    public Dictionary<string, UnityAction> clickActions = new Dictionary<string, UnityAction>();

}
