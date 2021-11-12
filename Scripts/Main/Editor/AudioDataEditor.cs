using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AudioData))]
public class AudioDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        AudioData audioData = (AudioData)target;
        if (DrawDefaultInspector())
        {
            #region frequency
            if (audioData.minFrequency > audioData.maxFrequency)
            {
                audioData.minFrequency = audioData.maxFrequency;
            }

            if (audioData.maxFrequency < audioData.minFrequency)
            {
                audioData.maxFrequency = audioData.minFrequency;
            }
            #endregion

            #region volume
            if (audioData.minVolume > audioData.maxVolume)
            {
                audioData.minVolume = audioData.maxVolume;
            }

            if (audioData.maxVolume < audioData.minVolume)
            {
                audioData.maxVolume = audioData.minVolume;
            }

            if (audioData.minVolume > audioData.baseVolume)
            {
                audioData.minVolume = audioData.baseVolume;
            }

            if (audioData.maxVolume < audioData.baseVolume)
            {
                audioData.maxVolume = audioData.baseVolume;
            }
            #endregion

            #region sound distance
            if (audioData.minSoundDistance > audioData.maxSoundDistance)
            {
                audioData.minSoundDistance = audioData.maxSoundDistance;
            }

            if (audioData.maxSoundDistance < audioData.minSoundDistance)
            {
                audioData.maxSoundDistance = audioData.minSoundDistance;
            }

            if (audioData.minSoundDistance > audioData.baseSoundDistance)
            {
                audioData.minSoundDistance = audioData.baseSoundDistance;
            }

            if (audioData.maxSoundDistance < audioData.baseSoundDistance)
            {
                audioData.maxSoundDistance = audioData.baseSoundDistance;
            }


            #endregion
        }

    }
}
