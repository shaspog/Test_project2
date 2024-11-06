using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class UnscaledTimeParticleSystem : MonoBehaviour
{
    private ParticleSystem particleSystem;
    private float lastUpdateTime;

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        particleSystem.Stop(); // Ensure the particle system does not play on awake
        lastUpdateTime = Time.unscaledTime;
    }

    void Update()
    {
        if (Time.timeScale == 0f) // Only update manually when the game is paused
        {
            float unscaledDeltaTime = Time.unscaledTime - lastUpdateTime;
            particleSystem.Simulate(unscaledDeltaTime, true, false);
        }

        lastUpdateTime = Time.unscaledTime;
    }
}
