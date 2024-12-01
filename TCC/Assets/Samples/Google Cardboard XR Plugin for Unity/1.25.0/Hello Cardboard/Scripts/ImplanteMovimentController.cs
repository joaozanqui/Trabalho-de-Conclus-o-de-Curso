//-----------------------------------------------------------------------
// <copyright file="ObjectController.cs" company="Google LLC">
// Copyright 2020 Google LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//-----------------------------------------------------------------------

using System.Collections;
using UnityEngine;

/// <summary>
/// Controls target objects behaviour.
/// </summary>
public class ImplanteMovimentController : MonoBehaviour
{
    /// <summary>
    /// The material to use when this object is inactive (not being gazed at).
    /// </summary>
    public Material InactiveMaterial;

    /// <summary>
    /// The material to use when this object is active (gazed at).
    /// </summary>
    public Material GazedAtMaterial;

    private bool shouldRotate = false;

    public float moveDistance = 1f; // Distância para mover para frente
    public float tiltAngle = 10f; // Ângulo de inclinação para frente
    public float duration = 0.5f; // Duração da animação

    public float horizontalSpeed = 100f;
    public float verticalSpeed = 100f;

    public Transform objectToTransformHorizontal; 
    public Transform objectToTransformVertical; 
    
    Vector3 initialPosition;
    Quaternion initialRotation;
    Vector3 targetPosition;
    Quaternion targetRotation;
    private bool isAtTarget = false;
    
    private Renderer _myRenderer;
    private Vector3 _startingPosition;
    private float smoothTime = 0.5f;
    private bool isClicked = false;
    
    public void StartTransformation()
    {
        StartCoroutine(MoveAndTilt());
    }

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    public void Start()
    {
        _startingPosition = transform.parent.localPosition;
        _myRenderer = GetComponent<Renderer>();
        
        initialPosition = objectToTransformVertical.position;
        initialRotation = objectToTransformVertical.rotation;
        targetPosition = initialPosition + objectToTransformVertical.forward * moveDistance;
        targetRotation = initialRotation * Quaternion.Euler(tiltAngle, 0, 0);
        SetMaterial(false);
    }

    void Update()
    {
        if (shouldRotate)
        {
            objectToTransformHorizontal.Rotate(Vector3.up, horizontalSpeed * Time.deltaTime);
            objectToTransformVertical.Rotate(Vector3.right, verticalSpeed * Time.deltaTime);
        }
    }

    private IEnumerator SetIfIsClicked()
    {
        isClicked = true;

        // Espera pelo tempo especificado
        yield return new WaitForSeconds(smoothTime);

        // Define a variável como false
        isClicked = false;
    }

    private IEnumerator MoveAndTilt()
    {
        Vector3 startPosition = isAtTarget ? targetPosition : initialPosition;
        Quaternion startRotation = isAtTarget ? targetRotation : initialRotation;

        Vector3 endPosition = isAtTarget ? initialPosition : targetPosition;
        Quaternion endRotation = isAtTarget ? initialRotation : targetRotation;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // Interpolação linear para a posição e rotação
            objectToTransformVertical.position = Vector3.Lerp(startPosition, endPosition, t);
            objectToTransformVertical.rotation = Quaternion.Lerp(startRotation, endRotation, t);

            yield return null;
        }

        // Certificar que termina na posição e rotação finais
        objectToTransformVertical.position = endPosition;
        objectToTransformVertical.rotation = endRotation;

        // Alterna o estado do objeto
        isAtTarget = !isAtTarget;
    }

    /// <summary>
    /// This method is called by the Main Camera when it starts gazing at this GameObject.
    /// </summary>
    public void OnPointerEnter()
    {
        SetMaterial(true);
        
        shouldRotate = true;
    }

    /// <summary>
    /// This method is called by the Main Camera when it stops gazing at this GameObject.
    /// </summary>
    public void OnPointerExit()
    {
        SetMaterial(false);
        shouldRotate = false;
    }

    /// <summary>
    /// This method is called by the Main Camera when it is gazing at this GameObject and the screen
    /// is touched.
    /// </summary>
    public void OnPointerClick()
    {
        if(!isClicked) {
            StartTransformation();
        }
        StartCoroutine(SetIfIsClicked());
    }

    /// <summary>
    /// Sets this instance's material according to gazedAt status.
    /// </summary>
    ///
    /// <param name="gazedAt">
    /// Value `true` if this object is being gazed at, `false` otherwise.
    /// </param>
    private void SetMaterial(bool gazedAt)
    {
        if (InactiveMaterial != null && GazedAtMaterial != null)
        {
            _myRenderer.material = gazedAt ? GazedAtMaterial : InactiveMaterial;
        }
    }
}
