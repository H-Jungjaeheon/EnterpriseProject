using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleRemove : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem ParticleSystem;

    // Start is called before the first frame update
    void Start()
    {
        ParticleSystem = this.GetComponent<ParticleSystem>();

        Destroy(this.gameObject, ParticleSystem.duration);
    }
}
