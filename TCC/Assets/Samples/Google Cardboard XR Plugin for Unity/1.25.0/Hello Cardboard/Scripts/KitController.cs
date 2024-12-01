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
public class KitController : MonoBehaviour
{
    /// <summary>
    /// The material to use when this object is inactive (not being gazed at).
    /// </summary>
    public Material InactiveMaterial;
    private Renderer _renderMenu;
    private Renderer _renderSign;
    public GameObject objCard;
    public GameObject signMessage;
    /// <summary>
    /// The material to use when this object is active (gazed at).
    /// </summary>
    public Material GazedAtMaterial;
    private int toolPosition = 1;

    public Material textureBase;
    public Material textureClicked;
    public Material textureSigned1;
    public Material textureSigned2;
    public Material textureSigned3;
    public Material textureSigned4;

    private Renderer _myRenderer;
    private float smoothTime = 0.5f;

    public Transform tool1; 
    public Transform tool2; 
    public Transform tool3; 
    public Transform tool4; 

    private bool isClicked = false;

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    void Start()
    {
        _renderMenu = objCard.GetComponent<Renderer>();
        _renderSign = signMessage.GetComponent<Renderer>();
        tool1.gameObject.SetActive(true);
        tool2.gameObject.SetActive(false);
        tool3.gameObject.SetActive(false);
        tool4.gameObject.SetActive(false);
    }

    private IEnumerator SetIfIsClicked()
    {
        isClicked = true;

        // Espera pelo tempo especificado
        yield return new WaitForSeconds(smoothTime);

        // Define a variável como false
        isClicked = false;
    }

    IEnumerator ChangeTool()
    {
        if (toolPosition == 1) {
            tool1.gameObject.SetActive(false);
            tool2.gameObject.SetActive(true);
            tool3.gameObject.SetActive(false);            
            tool4.gameObject.SetActive(false);            
            _renderSign.material = textureSigned2;
            toolPosition = 2;
        } else if (toolPosition == 2) {
            tool1.gameObject.SetActive(false);
            tool2.gameObject.SetActive(false);
            tool3.gameObject.SetActive(true);
            tool4.gameObject.SetActive(false);
            _renderSign.material = textureSigned3;
            toolPosition = 3;
        } else if (toolPosition == 3) {
            tool1.gameObject.SetActive(false);
            tool2.gameObject.SetActive(false);
            tool3.gameObject.SetActive(false);
            tool4.gameObject.SetActive(true);
            _renderSign.material = textureSigned4;
            toolPosition = 4;
        } else if (toolPosition == 4) {
            tool1.gameObject.SetActive(true);
            tool2.gameObject.SetActive(false);
            tool3.gameObject.SetActive(false);
            tool4.gameObject.SetActive(false);
            _renderSign.material = textureSigned1;
            toolPosition = 1;
        }

        yield return null;

    }


    /// <summary>
    /// This method is called by the Main Camera when it starts gazing at this GameObject.
    /// </summary>
    public void OnPointerEnter()
    {
        _renderMenu.material = textureClicked;
        SetMaterial(true);
    }

    /// <summary>
    /// This method is called by the Main Camera when it stops gazing at this GameObject.
    /// </summary>
    public void OnPointerExit()
    {
        _renderMenu.material = textureBase;
        SetMaterial(false);
    }

    /// <summary>
    /// This method is called by the Main Camera when it is gazing at this GameObject and the screen
    /// is touched.
    /// </summary>
    public void OnPointerClick()
    {
        if(!isClicked) {
            StartCoroutine(ChangeTool());
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
