using UnityEngine;

public class Player_Death_Script : MonoBehaviour

{
    private Renderer characterRenderer;
    private float fadeDuration = 5f;
    private float elapsedTime = 0f;

    void Start()
    {
        // Get the renderer component of the character prefab
        characterRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        // Increment the elapsed time
        elapsedTime += Time.deltaTime;

        // Calculate the fade progress (from 0 to 1)
        float fadeProgress = elapsedTime / fadeDuration;

        // Set the new alpha value
        SetAlpha(1f - fadeProgress);

        // Check if the fade duration has passed
        if (elapsedTime >= fadeDuration)
        {
            // Disable the script to stop updating the opacity
            enabled = false;
        }
    }

    void SetAlpha(float alpha)
    {
        // Assuming the character prefab has a Renderer component
        if (characterRenderer != null)
        {
            // Get the material color
            Color materialColor = characterRenderer.material.color;

            // Set the new color with updated alpha value
            characterRenderer.material.color = new Color(materialColor.r, materialColor.g, materialColor.b, alpha);
        }
        else
        {
            Debug.LogWarning("Renderer component not found. Make sure the prefab has a Renderer component.");
        }
    }
}