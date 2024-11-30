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
        /// 读取文件
        /// </summary>
        /// <param name="fileName">默认路径为Assets/Texts/</param>
        /// <returns></returns>
        public SubtitleRequestInfo LoadSubtitles(string fileName, UnityAction callback = null)
        {
            string[] lines = File.ReadAllLines("Assets/Texts/" + fileName); // 读取字幕文件所有行
            Debug.Log(File.Exists("Assets/Texts/" + fileName));
            Debug.Log(string.Join("\n", lines));

            SubtitleRequestInfo currentInfo = new SubtitleRequestInfo();  // 当前字幕信息
            SubtitleRequestInfo firstInfo = currentInfo;   // 用于保存第一条字幕信息
            string previousParent = "";  // 保存上一行的父物体名称


            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue; // 跳过空行

                // 按照特殊分隔符分割每行
                var parts = line.Split(new[] { "*" }, StringSplitOptions.None);

                string parentName;  // 父物体名称
                string subtitleText;  // 对话内容
                if (parts.Length > 2) continue; // 如果格式不对，则跳过
                else if (parts.Length == 2)
                {
                    parentName = parts[0]; // 父物体名称
                    subtitleText = parts[1];
                    Transform parentTransform = GameObject.Find(parentName)?.transform;
                    if (parentTransform == null)
                    {
                        Debug.LogWarning($"没有找到父物体: {parentName}");
                        continue;
                    }

                    if (parentName != previousParent && firstInfo.subtitles.Count>0)
                    {
                        // 保存上一条字幕的信息
                        var previousInfo = currentInfo;
                        var nextInfo = new SubtitleRequestInfo
                        {
                            parent = parentTransform,
                            subtitles = new List<string> { subtitleText }
                        };
                        // 创建回调以保证链式执行
                        previousInfo.callBack = () =>
                        {
                            Player.Default.currentSubtitles = StartSubtitle(nextInfo);
                        };
                        currentInfo = nextInfo;
                        
                    }
                    else if (parentName==previousParent) 
                    {
                        // 如果父物体名称相同，追加字幕
                        currentInfo.subtitles.Add(subtitleText);
                    }
                    else
                    {
                        currentInfo.parent = parentTransform;
                        currentInfo.subtitles.Add(subtitleText);
                    }
                    previousParent = parentName; // 更新父物体名称
                }
                else
                {
                    if (currentInfo == null)
                    {
                        Debug.LogWarning("第一句话物体为空，字幕将为默认设置");
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
