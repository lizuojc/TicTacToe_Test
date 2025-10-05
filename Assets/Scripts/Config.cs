using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Configuration/Game Config")]
public class Config : ScriptableObject
{
    [Header("AI����")]
    [Range(0f, 1f)]
    [Tooltip("�����̶ȿ��Ʋ�����0=��ȫ�����1=���Ų��ԣ�")]
    public float smartness = 0.5f;

    [Tooltip("AI�������̸���Ȩ��ʱ��������ʤ��Ȩ��")]
    public float winWeight = 100;
    [Tooltip("AI�������̸���Ȩ��ʱ����ֹ��һ�ʤ��Ȩ��")]
    public float defendWeight = 50;
    [Tooltip("AI�������̸���Ȩ��ʱ������λ�õ�Ȩ��")]
    public float centerLocWeight = 20;
    [Tooltip("AI�������̸���Ȩ��ʱ������λ�õ�Ȩ��")]
    public float cornerLocWeight = 10;
    [Tooltip("AI�������̸���Ȩ��ʱ����Եλ�õ�Ȩ��")]
    public float edgeLocWeight = 5;


    [Header("��������")]
    [Tooltip("X���ӵ���ɫ")]
    public Color xColor = Color.white;
    [Tooltip("O���ӵ���ɫ")]
    public Color oColor = Color.white;
    [Tooltip("��ʤ���ӵ���ɫ")]
    public Color highlightColor = Color.green;

    [Tooltip("X���ӵ���ʽ")]
    public Sprite xSprite;
    [Tooltip("O���ӵ���ʽ")]
    public Sprite oSprite;


    [Header("��Ϸ����")]
    [Tooltip("AI�غ�ʱ�ȴ���ʱ�䣬����ģ��AI˼��")]
    public float aiDelay = 0.5f;
    [Tooltip("��Ϸ��ʼʱ���Ƿ��������")]
    public bool randomFirstPlayer = true; // �������ѡ��

}
