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
public class infosController : MonoBehaviour
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
    
    private Renderer _renderNextImage;
    public GameObject objCard;
    public Material nextImage;

    public GameObject currentNextButton;
    public GameObject currentPreviousButton;
    public GameObject nextNextButton;
    public GameObject nextPreviousButton;

    private bool isClicked = false;
    private float smoothTime = 0.5f;


    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    public void Start()
    {
        StartCoroutine(SetIfIsClicked());
        _startingPosition = transform.parent.localPosition;
        _myRenderer = GetComponent<Renderer>();
        _renderNextImage = objCard.GetComponent<Renderer>();
        SetMaterial(false);
    }

    private IEnumerator SetIfIsClicked()
    {
        isClicked = true;

        // Espera pelo tempo especificado
        yield return new WaitForSeconds(smoothTime);

        // Define a variável como false
        isClicked = false;
    }

    public void changeCard() {
        _renderNextImage.material = nextImage;
        if(currentNextButton)
            currentNextButton.SetActive(false);
        if(currentPreviousButton)
            currentPreviousButton.SetActive(false);
        if(nextNextButton)
            nextNextButton.SetActive(true);
        if(nextPreviousButton)
            nextPreviousButton.SetActive(true);
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
    /// This method is called by the Main Camera when it is gazing at this GameObject and the screen
    /// is touched.
    /// </summary>
    public void OnPointerClick()
    {
        if(!isClicked) {
            changeCard();
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
