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

        // �ֵ����ڴ洢���д���
        public Dictionary<string, string> AllWords = new Dictionary<string, string>();

        /// <summary>
        /// ��ָ��·�����ش��Ｏ���ֵ���
        /// </summary>
        /// <param name="path">�ļ�·��</param>
        public void LoadWords(string path)
        {
            try
            {
                // �ж��ļ��Ƿ����
                if (!File.Exists(path))
                {
                    Console.WriteLine("�ļ�������: " + path);
                    return;
                }

                // ���ж�ȡ�ļ�����
                string[] lines = File.ReadAllLines(path);

                // ����ÿһ�У���������ӵ��ֵ�
                foreach (string line in lines)
                {
                    // ȷ���в�Ϊ���Ұ����ָ��� '*'
                    if (!string.IsNullOrWhiteSpace(line) && line.Contains('*'))
                    {
                        // ʹ�� '*' �ָ�����ID�ʹ�������
                        string[] parts = line.Split(new[] { '*' }, 2);
                        if (parts.Length == 2)
                        {
                            string id = parts[0].Trim();
                            string content = parts[1].Trim();

                            // �������ļ�ֵ�Լ����ֵ�
                            if (!AllWords.ContainsKey(id))
                            {
                                AllWords.Add(id, content);
                            }
                            else
                            {
                                Console.WriteLine($"����: ����ID�ظ� - {id}, ������");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"��Ч�и�ʽ: {line}");
                    }
                }

                Console.WriteLine("���Ｏ������ɣ������ش�����: " + AllWords.Count);
                RegisterWordClickEvents();
            }
            catch (Exception ex)
            {
                Console.WriteLine("���ش��Ｏʱ��������: " + ex.Message);
            }
        }
        private void RegisterWordClickEvents()
        {
            foreach (var word in AllWords)
            {
                string id = word.Key; // ����ID
                UnityAction action = () =>
                {
                    // ������ID��ӵ�Player.Default.PlayerWordsID�б���
                    if (Player.Default != null && !Player.Default.PlayerWordsID.Contains(id))
                    {
                        Player.Default.PlayerWordsID.Add(id);
                        Console.WriteLine($"�ѽ�����ID '{id}' ��ӵ� PlayerWordsID");
                    }
                };

                // ʹ�� TextClickManager ע�����¼�
                TextClickManager.Instance.RegisterLinkAction(id, action);
            }

            Console.WriteLine("���д������¼�ע�����");
        }
    }
}
