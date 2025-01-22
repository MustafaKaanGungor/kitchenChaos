using UnityEngine;

[CreateAssetMenu(fileName = "AudioClipRefsSO", menuName = "AudioClipRefsSO", order = 0)]
public class AudioClipRefsSO : ScriptableObject {
    public AudioClip[] chop;
    public AudioClip[] deliverySuccess;  
    public AudioClip[] deliveryFail;  
    public AudioClip[] footstep;  
    public AudioClip[] objectDrop;  
    public AudioClip[] objectPickup;  
    public AudioClip stoveSizzle;  
    public AudioClip[] trash;  
    public AudioClip[] warning;  

}