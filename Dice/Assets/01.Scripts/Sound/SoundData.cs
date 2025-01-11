using UnityEngine;

[CreateAssetMenu(fileName = "SoundData", menuName = "Scriptable Objects/SoundData")]
public class SoundData : ScriptableObject
{
    /// <summary>
    /// ���� �̸�
    /// </summary>
    public string soundName{
        get => this.name;
    }

    /// <summary>
    /// ����� AudioClip
    /// </summary>
    public AudioClip audioClip;

    /// <summary>
    /// ���� (0-1)
    /// </summary>
    [Range(0f, 2f)] public float volume = 1f;

    /// <summary>
    /// ��ġ
    /// </summary>
    [Range(0f, 2f)] public float pitch = 1f;

    /*private void OnValidate() // �����Ϳ��� soundName�� ���� �̸����� ���� (���Ǽ�)
    {
        if (string.IsNullOrEmpty(soundName))
        {
            soundName = name;
        }
    }
    
    [ContextMenu("Renew Name")]
    private void PrintDebugMessage()
    {
        if (string.IsNullOrEmpty(soundName))
        {
            soundName = name;
        }
    }*/
    
}
