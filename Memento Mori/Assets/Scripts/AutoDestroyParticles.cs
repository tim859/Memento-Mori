using UnityEngine;

// a bit inefficient to have a script for the purpose of literally one line of code
// but the blood particle systems refuse to be destroyed by any means
// other than having a script attached directly to them
// (thanks to buzzefall from the unity forums)
// https://answers.unity.com/questions/219609/auto-destroying-particle-system.html
public class AutoDestroyParticles : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, GetComponent<ParticleSystem>().main.duration);
    }
}
