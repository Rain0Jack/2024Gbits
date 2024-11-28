

using UnityEngine;

public interface IInteractable
{
    // 是否可交互
    bool IsInteractable { get; set; }

    bool IsInteracting { get; set; }
    // 描述信息
    string Description { get; }

    // 触发交互的入口，由外部调用
    public void TriggerInteraction()
    {
        if (!IsInteractable || IsInteracting)
        {
            Debug.Log("Interaction not allowed or already in progress.");
            return;
        }
        PerformInteraction();
    }

    // 执行具体的交互逻辑，由内部调用
    public void PerformInteraction();

    public void OnInteractionStart()
    {
        IsInteracting = true;
    }
    public void OnInteractionEnd() 
    {
        IsInteracting = false;
    }
}
