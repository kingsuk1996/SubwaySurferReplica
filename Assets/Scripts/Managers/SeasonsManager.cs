using UnityEngine;

public class SeasonsManager : MonoBehaviour
{
    public Material oldSkybox;
    public Material newSkybox;
    public float blendTime = 2.0f; // Blend time in seconds

    private float blendTimer = 0.0f;
    private bool isBlending = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            StartBlend();
        }

        if (isBlending)
        {
            blendTimer += Time.deltaTime;
            float blendFactor = blendTimer / blendTime;

            RenderSettings.skybox.Lerp(oldSkybox, newSkybox, blendFactor);

            if (blendFactor >= 1.0f)
            {
                EndBlend();
            }
        }
    }

    void StartBlend()
    {
        isBlending = true;
        blendTimer = 0.0f;

        RenderSettings.skybox = oldSkybox;
    }

    void EndBlend()
    {
        isBlending = false;
        blendTimer = 0.0f;

        RenderSettings.skybox = newSkybox;
    }
}
