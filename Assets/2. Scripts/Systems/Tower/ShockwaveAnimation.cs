using UnityEngine;

public class ShockwaveAnimation : MonoBehaviour
{
    private Animator _anim;
    
    public void PlayShockWave()
    {
        _anim ??= transform.GetChild(0).GetComponent<Animator>();
        _anim.SetTrigger("Shockwave");
    }
}
