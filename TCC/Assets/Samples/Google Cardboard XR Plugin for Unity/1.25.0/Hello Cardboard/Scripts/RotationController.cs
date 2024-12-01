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
using UnityEngine.SceneManagement;

/// <summary>
/// Controls target objects behaviour.
/// </summary>
public class RotationController : MonoBehaviour
{
    /// <summary>
    /// The material to use when this object is inactive (not being gazed at).
    /// </summary>
    public Material InactiveMaterial;

    /// <summary>
    /// The material to use when this object is active (gazed at).
    /// </summary>
    public Material GazedAtMaterial;

    private Renderer _myRenderer;
    private Vector3 _startingPosition;
    
    private Renderer _renderCard;
    public GameObject objToRotate;
    public GameObject card;
    public Material textureBase;
    public Material textureClicked;
    [SerializeField] private bool left;
    [SerializeField] private bool right;
    [SerializeField] private float rotationSpeed;
    private bool shouldRotate = false;


    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    public void Start()
    {
        _startingPosition = transform.parent.localPosition;
        _myRenderer = GetComponent<Renderer>();
        _renderCard = card.GetComponent<Renderer>();
        SetMaterial(false);
    }

     private void OnValidate()
    {
        if (left == right)
        {
            right = !left;
            left = !right;
        }
    }

    public void Update() 
    {
        if (shouldRotate)
        {
            if(right)
                objToRotate.transform.Rotate(Vector3.up, rotationSpeed * -Time.deltaTime);
            else
                objToRotate.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }

    }

    // Propriedades para acessar os valores
    public bool IsLeftSelected => left;
    public bool IsRightSelected => right;

    /// <summary>
    /// This method is called by the Main Camera when it starts gazing at this GameObject.
    /// </summary>
    public void OnPointerEnter()
    {
        _renderCard.material = textureClicked;
        shouldRotate = true;
        SetMaterial(true);
    }

    /// <summary>
    /// This method is called by the Main Camera when it stops gazing at this GameObject.
    /// </summary>
    public void OnPointerExit()
    {
        _renderCard.material = textureBase;
        shouldRotate = false;
        SetMaterial(false);
    }

    /// <summary>
    /// This method is called by the Main Camera when it is gazing at this GameObject and the screen
    /// is touched.
    /// </summary>
    public void OnPointerClick()
    {
        
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
