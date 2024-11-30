using UnityEngine;
using QFramework;
using Unity.VisualScripting;
using System.Drawing;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine.Events;

namespace Project
{
	public partial class SubtitleSystem : ViewController
	{
		public static SubtitleSystem Default;
        private void Awake()
        {
            Default = this;
        }
        private void OnDestroy()
        {
			Default = null;
        }
        public Subtitle StartSubtitle(SubtitleRequestInfo info)
		{
			var subtitle = Subtitle
				.Instantiate(Subtitle,this.transform)
                .Position(Camera.main.WorldToScreenPoint(info.parent.position) + new Vector3(info.offset.x, info.offset.y, 0))
                .Show()
				.GetComponent<Subtitle>();
			var RT = subtitle.gameObject.GetComponent<RectTransform>();
			var textBox = subtitle.textBox;

			RT.sizeDelta = info.size;
			if(info.parent == null)
                RT.anchoredPosition = info.position;
            textBox.fontSize =info.fontSize;

			subtitle.parent = info.parent;
			subtitle.Delay = info.subtitlesDelay;
			subtitle.typeSound = info.typeSound;
			subtitle.callBack = info.callBack;
			subtitle.subtitles = info.subtitles;
            subtitle.offset = info.offset;

            Debug.Log(string.Join("\n", subtitle.subtitles));

            Player.Default.currentSubtitles= subtitle;
            
			return subtitle;
		}

        /// <summary>
        /// ��ȡ�ļ�
        /// </summary>
        /// <param name="fileName">Ĭ��·��ΪAssets/Texts/</param>
        /// <returns></returns>
        public SubtitleRequestInfo LoadSubtitles(string fileName, UnityAction callback = null)
        {
            string[] lines = File.ReadAllLines("Assets/Texts/" + fileName); // ��ȡ��Ļ�ļ�������
            Debug.Log(File.Exists("Assets/Texts/" + fileName));
            Debug.Log(string.Join("\n", lines));

            SubtitleRequestInfo currentInfo = new SubtitleRequestInfo();  // ��ǰ��Ļ��Ϣ
            SubtitleRequestInfo firstInfo = currentInfo;   // ���ڱ����һ����Ļ��Ϣ
            string previousParent = "";  // ������һ�еĸ���������


            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue; // ��������

                // ��������ָ����ָ�ÿ��
                var parts = line.Split(new[] { "*" }, StringSplitOptions.None);

                string parentName;  // ����������
                string subtitleText;  // �Ի�����
                if (parts.Length > 2) continue; // �����ʽ���ԣ�������
                else if (parts.Length == 2)
                {
                    parentName = parts[0]; // ����������
                    subtitleText = parts[1];
                    Transform parentTransform = GameObject.Find(parentName)?.transform;
                    if (parentTransform == null)
                    {
                        Debug.LogWarning($"û���ҵ�������: {parentName}");
                        continue;
                    }

                    if (parentName != previousParent && firstInfo.subtitles.Count>0)
                    {
                        // ������һ����Ļ����Ϣ
                        var previousInfo = currentInfo;
                        var nextInfo = new SubtitleRequestInfo
                        {
                            parent = parentTransform,
                            subtitles = new List<string> { subtitleText }
                        };
                        // �����ص��Ա�֤��ʽִ��
                        previousInfo.callBack = () =>
                        {
                            Player.Default.currentSubtitles = StartSubtitle(nextInfo);
                        };
                        currentInfo = nextInfo;
                        
                    }
                    else if (parentName==previousParent) 
                    {
                        // ���������������ͬ��׷����Ļ
                        currentInfo.subtitles.Add(subtitleText);
                    }
                    else
                    {
                        currentInfo.parent = parentTransform;
                        currentInfo.subtitles.Add(subtitleText);
                    }
                    previousParent = parentName; // ���¸���������
                }
                else
                {
                    if (currentInfo == null)
                    {
                        Debug.LogWarning("��һ�仰����Ϊ�գ���Ļ��ΪĬ������");
                    }
                    else
                    {
                        currentInfo.subtitles.Add(line);
                    }
                }
            }
            if (currentInfo != null)
            {
                currentInfo.callBack += callback;
            }

            return firstInfo;
        }

    }
}
