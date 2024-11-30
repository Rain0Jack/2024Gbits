using System;
using System.Collections.Generic;
using System.IO;
using QFramework;
using UnityEngine.Events;


namespace Project
{

    public class WordsCollect : Singleton<WordsCollect>
    {
        private WordsCollect() { }

        // 字典用于存储所有词语
        public Dictionary<string, string> AllWords = new Dictionary<string, string>();

        /// <summary>
        /// 从指定路径加载词语集到字典中
        /// </summary>
        /// <param name="path">文件路径</param>
        public void LoadWords(string path)
        {
            try
            {
                // 判断文件是否存在
                if (!File.Exists(path))
                {
                    Console.WriteLine("文件不存在: " + path);
                    return;
                }

                // 按行读取文件内容
                string[] lines = File.ReadAllLines(path);

                // 遍历每一行，解析并添加到字典
                foreach (string line in lines)
                {
                    // 确保行不为空且包含分隔符 '*'
                    if (!string.IsNullOrWhiteSpace(line) && line.Contains('*'))
                    {
                        // 使用 '*' 分隔词语ID和词语内容
                        string[] parts = line.Split(new[] { '*' }, 2);
                        if (parts.Length == 2)
                        {
                            string id = parts[0].Trim();
                            string content = parts[1].Trim();

                            // 将解析的键值对加入字典
                            if (!AllWords.ContainsKey(id))
                            {
                                AllWords.Add(id, content);
                            }
                            else
                            {
                                Console.WriteLine($"警告: 词语ID重复 - {id}, 已跳过");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"无效行格式: {line}");
                    }
                }

                Console.WriteLine("词语集加载完成，共加载词语数: " + AllWords.Count);
                RegisterWordClickEvents();
            }
            catch (Exception ex)
            {
                Console.WriteLine("加载词语集时发生错误: " + ex.Message);
            }
        }
        private void RegisterWordClickEvents()
        {
            foreach (var word in AllWords)
            {
                string id = word.Key; // 词语ID
                UnityAction action = () =>
                {
                    // 将词语ID添加到Player.Default.PlayerWordsID列表中
                    if (Player.Default != null && !Player.Default.PlayerWordsID.Contains(id))
                    {
                        Player.Default.PlayerWordsID.Add(id);
                        Console.WriteLine($"已将词语ID '{id}' 添加到 PlayerWordsID");
                    }
                };

                // 使用 TextClickManager 注册点击事件
                TextClickManager.Instance.RegisterLinkAction(id, action);
            }

            Console.WriteLine("所有词语点击事件注册完成");
        }
    }
}
