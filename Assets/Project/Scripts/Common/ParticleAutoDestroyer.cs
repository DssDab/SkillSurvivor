using UnityEngine;

public class ParticleAutoDestroyer : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem particle;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if( particle.isPlaying == false )
        {
            Destroy(gameObject);
        }
    }
}
