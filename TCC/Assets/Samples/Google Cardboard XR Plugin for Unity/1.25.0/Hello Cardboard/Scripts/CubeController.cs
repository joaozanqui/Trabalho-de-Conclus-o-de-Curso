using System.Collections;
using UnityEngine;

/// <summary>
/// Controls the behavior of the cube.
/// </summary>
public class CubeController : MonoBehaviour
{
    /// <summary>
    /// The material to use when this object is inactive (not being gazed at).
    /// </summary>
    public Material InactiveMaterial;

    /// <summary>
    /// The material to use when this object is active (gazed at).
    /// </summary>
    public Material GazedAtMaterial;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private Vector3 targetPosition;
    private Quaternion targetRotation;

    private bool isInitialState = true;
    private Renderer _cubeRenderer;
    private float smoothTime = 0.5f;

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        _cubeRenderer = GetComponent<Renderer>();

        // Save the initial position and rotation of the cube
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;

        // Set the target position and rotation
        targetPosition = initialPosition + new Vector3(0, 0.5f, 0); // Move up by 0.5 units
        targetRotation = initialRotation * Quaternion.Euler(30f, 180f, 0f); // Rotate 30 degrees on X, 180 degrees on Y

        SetMaterial(false);
    }

    /// <summary>
    /// This method is called by the Main Camera when it starts gazing at this GameObject.
    /// </summary>
    public void OnPointerEnter()
    {
        SetMaterial(true);
    }

    /// <summary>
    /// This method is called by the Main Camera when it stops gazing at this GameObject.
    /// </summary>
    public void OnPointerExit()
    {
        SetMaterial(false);
    }

    /// <summary>
    /// This method is called by the Main Camera when it is gazing at this GameObject and the screen is touched.
    /// </summary>
    public void OnPointerClick()
    {
        // Toggle between the initial state and the target state
        isInitialState = !isInitialState;
        StartCoroutine(AnimateCube());
    }

    /// <summary>
    /// Animate the cube to the target position and rotation.
    /// </summary>
    private IEnumerator AnimateCube()
    {
        float elapsedTime = 0f;
        Vector3 startPosition = isInitialState ? targetPosition : initialPosition;
        Quaternion startRotation = isInitialState ? targetRotation : initialRotation;
        Vector3 endPosition = isInitialState ? initialPosition : targetPosition;
        Quaternion endRotation = isInitialState ? initialRotation : targetRotation;

        while (elapsedTime < smoothTime)
        {
            float t = elapsedTime / smoothTime;

            // Lerp position and Slerp rotation
            transform.localPosition = Vector3.Lerp(startPosition, endPosition, t);
            transform.localRotation = Quaternion.Slerp(startRotation, endRotation, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final position and rotation are set
        transform.localPosition = endPosition;
        transform.localRotation = endRotation;
    }

    /// <summary>
    /// Sets this instance's material according to gazedAt status.
    /// </summary>
    ///
    /// <param name="gazedAt">Value `true` if this object is being gazed at, `false` otherwise.</param>
    private void SetMaterial(bool gazedAt)
    {
        if (InactiveMaterial != null && GazedAtMaterial != null)
        {
            _cubeRenderer.material = gazedAt ? GazedAtMaterial : InactiveMaterial;
        }
    }
}
