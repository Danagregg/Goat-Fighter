using UnityEngine;

public class CanTakeDamageComponent : MonoBehaviour
{
	public ParticleSystem _particleSystem;
	public Transform _transform;

    void TakeDamage()
    {
        gameObject.SetActive(false);
        Object.Instantiate(_particleSystem, _transform.position, Quaternion.identity);

    }
}
