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
public class ImplantController : MonoBehaviour
{
    /// <summary>
    /// The material to use when this object is inactive (not being gazed at).
    /// </summary>
    public Material InactiveMaterial;
    private Renderer _renderMenu;
    public GameObject objCard;
    /// <summary>
    /// The material to use when this object is active (gazed at).
    /// </summary>
    public Material GazedAtMaterial;
    private bool isInInitialPosition = true;
    public Material textureBase;
    public Material textureClicked;

    private Renderer _myRenderer;
    
    public Transform upperPart; 
    public Transform middleUpperPart;    
    public Transform middleLowerPart;    
    public Transform lowerPart; 

    public Transform sign1; 
    public Transform sign2; 
    public Transform sign3; 
    public Transform sign4; 
    public Transform signGeneral; 

    public float initialUpperOffset;
    public float initialMiddleUpperOffset;
    public float initialMiddleLowerOffset;
    public float initialLowerOffset;

    public float initialSign1Offset;
    public float initialSign2Offset;
    public float initialSign3Offset;
    public float initialSign4Offset;

    private Vector3 initialUpperPos;
    private Vector3 initialMiddleUpperPos;
    private Vector3 initialMiddleLowerPos;
    private Vector3 initialLowerPos;

    private Vector3 initialSign1Pos;
    private Vector3 initialSign2Pos;
    private Vector3 initialSign3Pos;
    private Vector3 initialSign4Pos;

    private float smoothTime = 0.5f;
    private bool isClicked = false;

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    void Start()
    {
        _renderMenu = objCard.GetComponent<Renderer>();

        // Armazena as posições iniciais dos objetos
        initialUpperPos = upperPart.localPosition;
        initialMiddleUpperPos = middleUpperPart.localPosition;
        initialMiddleLowerPos = middleLowerPart.localPosition;
        initialLowerPos = lowerPart.localPosition;

        initialSign1Pos = sign1.localPosition;
        initialSign2Pos = sign2.localPosition;
        initialSign3Pos = sign3.localPosition;
        initialSign4Pos = sign4.localPosition;

        sign1.gameObject.SetActive(isInInitialPosition);
        sign2.gameObject.SetActive(isInInitialPosition);
        sign3.gameObject.SetActive(isInInitialPosition);
        sign4.gameObject.SetActive(isInInitialPosition);
        signGeneral.gameObject.SetActive(!isInInitialPosition);
    }

    private IEnumerator SetIfIsClicked()
    {
        isClicked = true;

        // Espera pelo tempo especificado
        yield return new WaitForSeconds(smoothTime);

        // Define a variável como false
        isClicked = false;
    }
    
    public void showSigns() {
        sign1.gameObject.SetActive(!isInInitialPosition);
        sign2.gameObject.SetActive(!isInInitialPosition);
        sign3.gameObject.SetActive(!isInInitialPosition);
        sign4.gameObject.SetActive(!isInInitialPosition);
        signGeneral.gameObject.SetActive(isInInitialPosition);
    } 

    IEnumerator SetUp()
    {
        float duration = 0.5f; // Tempo da transição
        float elapsedTime = 0f;

        // Posições de partida e alvo, dependendo do estado atual
        Vector3 startUpperPos, targetUpperPos;
        Vector3 startMiddleUpperPos, targetMiddleUpperPos;
        Vector3 startMiddleLowerPos, targetMiddleLowerPos;
        Vector3 startLowerPos, targetLowerPos;

        Vector3 startSign1Pos, targetSign1Pos;
        Vector3 startSign2Pos, targetSign2Pos;
        Vector3 startSign3Pos, targetSign3Pos;
        Vector3 startSign4Pos, targetSign4Pos;

        if (isInInitialPosition)
        {
            // Movendo para as novas posições
            startUpperPos = upperPart.localPosition;
            targetUpperPos = initialUpperPos + new Vector3(0, initialUpperOffset, 0);

            startMiddleUpperPos = middleUpperPart.localPosition;
            targetMiddleUpperPos = initialMiddleUpperPos + new Vector3(0, initialMiddleUpperOffset / 2f, 0);

            startMiddleLowerPos = middleLowerPart.localPosition;
            targetMiddleLowerPos = initialMiddleLowerPos + new Vector3(0, initialMiddleLowerOffset / 2f, 0);

            startLowerPos = lowerPart.localPosition;
            targetLowerPos = initialLowerPos + new Vector3(0, initialLowerOffset, 0);

            
            startSign1Pos = sign1.localPosition;
            targetSign1Pos = initialSign1Pos + new Vector3(0, initialSign1Offset, 0);
            startSign2Pos = sign2.localPosition;
            targetSign2Pos = initialSign2Pos + new Vector3(0, initialSign2Offset, 0);
            startSign3Pos = sign3.localPosition;
            targetSign3Pos = initialSign3Pos + new Vector3(0, initialSign3Offset, 0);
            startSign4Pos = sign4.localPosition;
            targetSign4Pos = initialSign4Pos + new Vector3(0, initialSign4Offset, 0);
        }
        else
        {
            showSigns();
            // Voltando para as posições iniciais
            startUpperPos = upperPart.localPosition;
            targetUpperPos = initialUpperPos;

            startMiddleUpperPos = middleUpperPart.localPosition;
            targetMiddleUpperPos = initialMiddleUpperPos;

            startMiddleLowerPos = middleLowerPart.localPosition;
            targetMiddleLowerPos = initialMiddleLowerPos;

            startLowerPos = lowerPart.localPosition;
            targetLowerPos = initialLowerPos;


            startSign1Pos = sign1.localPosition;
            targetSign1Pos = initialSign1Pos;

            startSign2Pos = sign2.localPosition;
            targetSign2Pos = initialSign2Pos;
            
            startSign3Pos = sign3.localPosition;
            targetSign3Pos = initialSign3Pos;

            startSign4Pos = sign4.localPosition;
            targetSign4Pos = initialSign4Pos;
        }

        // Interpolação suave das posições
        while (elapsedTime < duration)
        {
            
            float t = elapsedTime / duration;
            upperPart.localPosition = Vector3.Lerp(startUpperPos, targetUpperPos, t);
            middleUpperPart.localPosition = Vector3.Lerp(startMiddleUpperPos, targetMiddleUpperPos, t);
            middleLowerPart.localPosition = Vector3.Lerp(startMiddleLowerPos, targetMiddleLowerPos, t);
            lowerPart.localPosition = Vector3.Lerp(startLowerPos, targetLowerPos, t);

            sign1.localPosition = Vector3.Lerp(startSign1Pos, targetSign1Pos, t);
            sign2.localPosition = Vector3.Lerp(startSign2Pos, targetSign2Pos, t);
            sign3.localPosition = Vector3.Lerp(startSign3Pos, targetSign3Pos, t);
            sign4.localPosition = Vector3.Lerp(startSign4Pos, targetSign4Pos, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Garante que as posições finais sejam atingidas
        upperPart.localPosition = targetUpperPos;
        middleUpperPart.localPosition = targetMiddleUpperPos;
        middleLowerPart.localPosition = targetMiddleLowerPos;
        lowerPart.localPosition = targetLowerPos;

        sign1.localPosition = targetSign1Pos;
        sign2.localPosition = targetSign2Pos;
        sign3.localPosition = targetSign3Pos;
        sign4.localPosition = targetSign4Pos;

        // Alterna o estado
        if(isInInitialPosition) {
            showSigns();
            _renderMenu.transform.rotation = Quaternion.Euler(0, 180, -90);
        } else {
            _renderMenu.transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        isInInitialPosition = !isInInitialPosition;
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
            StartCoroutine(SetUp());
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
