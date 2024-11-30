using UnityEngine;
using QFramework;
using UnityEngine.InputSystem;
using System;
using System.Collections.Generic;

namespace Project
{
	public partial class Player : ViewController
	{
		public static Player Default;

        private void Awake()
        {
            Default = this;
        }

        private void OnDestroy()
        {
            Default=null;
        }
        public float movementSpeed=5;
		public Vector2 inputDirection;
        List<IInteractable> interactablesInBox = new List<IInteractable>();
        IInteractable currentInteractObj = null;
        public Subtitle currentSubtitles;
		PlayerInputControl inputControl;

        public List<string> PlayerWordsID = new List<string>();                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        

		void Start()
		{
			// Code Here
			inputControl = InputController.Instance.playerInputControl;


			ActionKit.OnUpdate.Register(() =>
			{
				MovementHandle();
			}).UnRegisterWhenGameObjectDestroyed(gameObject);

            #region ��������
            InteractBox.OnTriggerEnter2DEvent(c =>
            {
                var interactable = c.gameObject.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    // ��ӵ��б���
                    interactablesInBox.Add(interactable);
                    // ���µ�ǰ��������Ϊ�����������
                    currentInteractObj = interactable;
                }
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            InteractBox.OnTriggerExit2DEvent(c =>
            {
                var interactable = c.gameObject.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    // ���б����Ƴ��˳�������
                    interactablesInBox.Remove(interactable);

                    // ����˳��������ǵ�ǰ�Ľ�������
                    if (currentInteractObj == interactable)
                    {
                        // ���µ�ǰ��������Ϊ�б��е����һ�����壨������ڣ�
                        currentInteractObj = interactablesInBox.Count > 0 ? interactablesInBox[interactablesInBox.Count - 1] : null;
                    }
                }
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
            #endregion

            inputControl.GamePlay.Interact.started += InteractInput;
        }

        private void InteractInput(InputAction.CallbackContext context)
        {
            Debug.Log("InteractInput");
            if (currentSubtitles == null && currentInteractObj !=null)
			{
				InteractHandle(currentInteractObj);
                
			}
			else if(currentSubtitles != null)
			{
				SubtitleHandle();
			}
        }

        void Update()
		{
        }
		void MovementHandle()
		{
            inputDirection = inputControl
				.GamePlay
				.Move
				.ReadValue<Vector2>();
            SelfRigidbody2D.velocity = new Vector2(inputDirection.x, inputDirection.y / 2) * movementSpeed;
        }
		void InteractHandle(IInteractable interactableObj)
		{
			interactableObj.TriggerInteraction();
		}
		void SubtitleHandle()
		{
			currentSubtitles.Next();
		}
    }
}
