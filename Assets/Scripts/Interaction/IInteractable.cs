

using UnityEngine;

public interface IInteractable
{
    // �Ƿ�ɽ���
    bool IsInteractable { get; set; }

    bool IsInteracting { get; set; }
    // ������Ϣ
    string Description { get; }

    // ������������ڣ����ⲿ����
    public void TriggerInteraction()
    {
        if (!IsInteractable || IsInteracting)
        {
            Debug.Log("Interaction not allowed or already in progress.");
            return;
        }
        PerformInteraction();
    }

    // ִ�о���Ľ����߼������ڲ�����
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
