using UnityEngine;

public class ParticleSystemController : MonoBehaviour
{
    private Vector3 initialScale;

    void Start()
    {
        initialScale = transform.localScale;
    }

    public void SetParticleSystemActive(bool isActive)
    {
        transform.localScale = initialScale;
        gameObject.SetActive(isActive);

        if (isActive)
        {
            ParticleSystem ps = GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Clear();
                ps.Play();
            }
        }
    }
}
