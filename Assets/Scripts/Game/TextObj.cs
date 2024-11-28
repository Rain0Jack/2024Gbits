using UnityEngine;
using QFramework;

namespace Project
{
	public partial class TextObj : ViewController ,IInteractable
	{
        public string Description => "textobj";
        public bool IsInteractable { get; set; } = true;
        public bool IsInteracting { get; set; } = false;

        void IInteractable.PerformInteraction()
        {
            var i = this.GetComponent<IInteractable>();
            i.OnInteractionStart();
            var ss = SubtitleSystem.Default;
            ss.StartSubtitle(ss.LoadSubtitles("1.txt", () =>
            {
                i.OnInteractionEnd();
            }));
        }


    }
}
