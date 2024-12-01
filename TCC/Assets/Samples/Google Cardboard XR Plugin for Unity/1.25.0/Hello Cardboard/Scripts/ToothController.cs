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
public class ToothController : MonoBehaviour
{
    /// <summary>
    /// The material to use when this object is inactive (not being gazed at).
    /// </summary>
    public Material InactiveMaterial;
    private Renderer _renderButton;
    private Renderer _renderMandible;
    public GameObject Button;
    public GameObject Mandible;
    /// <summary>
    /// The material to use when this object is active (gazed at).
    /// </summary>
    public Material GazedAtMaterial;

    private bool isInInitialPosition = false;
    public Material textureBase;
    public Material textureClicked;

    private Renderer _myRenderer;
    
    public Transform leftTooth; 
    public Transform rightTooth;
    public Transform left2Tooth;
    public Transform right2Tooth;

    public float toothOffset;

    private Vector3 initialLeftToothPos;
    private Vector3 initialRightToothPos;
    private Vector3 initialLeft2ToothPos;
    private Vector3 initialRight2ToothPos;

    private float smoothTime = 0.5f;
    private bool isClicked = false;

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    void Start()
    {
        _renderButton = Button.GetComponent<Renderer>();
        _renderMandible = Mandible.GetComponent<Renderer>();

        // Armazena as posições iniciais dos objetos
        initialLeftToothPos = leftTooth.localPosition;
        initialRightToothPos = rightTooth.localPosition;
        if(left2Tooth && right2Tooth) {
            initialLeft2ToothPos = left2Tooth.localPosition;
            initialRight2ToothPos = right2Tooth.localPosition;
        }
        _renderMandible.transform.rotation = Quaternion.Euler(-90, 0, -90);
    }

    private IEnumerator SetIfIsClicked()
    {
        isClicked = true;

        // Espera pelo tempo especificado
        yield return new WaitForSeconds(smoothTime);

        // Define a variável como false
        isClicked = false;
    }

    IEnumerator MoveTeeth()
    {
        float duration = 0.5f; // Tempo da transição
        float elapsedTime = 0f;

        // Posições de partida e alvo, dependendo do estado atual
        Vector3 startLeftToothPos, targetLeftToothPos;
        Vector3 startRightToothPos, targetRightToothPos;
        Quaternion startMandiblePos, targetMandiblePos;

        Vector3 startLeft2ToothPos = Vector3.zero;
        Vector3 targetLeft2ToothPos = Vector3.zero;
        Vector3 startRight2ToothPos = Vector3.zero;
        Vector3 targetRight2ToothPos = Vector3.zero;
        
        startLeftToothPos = leftTooth.localPosition;
        startRightToothPos = rightTooth.localPosition;

        if(left2Tooth && right2Tooth) {
            startLeft2ToothPos = left2Tooth.localPosition;
            startRight2ToothPos = right2Tooth.localPosition;
        }

        if (isInInitialPosition)
        {
            // Movendo para as novas posições
            targetLeftToothPos = initialLeftToothPos + new Vector3(0, 0, toothOffset);
            targetRightToothPos = initialRightToothPos + new Vector3(0, 0, toothOffset); 
            if(left2Tooth && right2Tooth) {
                targetLeft2ToothPos = initialLeft2ToothPos + new Vector3(0, 0, toothOffset);
                targetRight2ToothPos = initialRight2ToothPos + new Vector3(0, 0, toothOffset);   
            }  

            startMandiblePos = Quaternion.Euler(-90, 0, -90);
            targetMandiblePos = Quaternion.Euler(-80, 0, -90);
        }
        else
        {
            // Voltando para as posições iniciais
            targetLeftToothPos = initialLeftToothPos;
            targetRightToothPos = initialRightToothPos;  
            if(left2Tooth && right2Tooth) {
                targetLeft2ToothPos = initialLeft2ToothPos;
                targetRight2ToothPos = initialRight2ToothPos;  
            }

            startMandiblePos = Quaternion.Euler(-80, 0, -90);
            targetMandiblePos = Quaternion.Euler(-90, 0, -90);  
        }

        // Interpolação suave das posições
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            leftTooth.localPosition = Vector3.Lerp(startLeftToothPos, targetLeftToothPos, t);
            rightTooth.localPosition = Vector3.Lerp(startRightToothPos, targetRightToothPos, t);
            if(left2Tooth && right2Tooth) {
                left2Tooth.localPosition = Vector3.Lerp(startLeft2ToothPos, targetLeft2ToothPos, t);
                right2Tooth.localPosition = Vector3.Lerp(startRight2ToothPos, targetRight2ToothPos, t);
            }
            _renderMandible.transform.rotation = Quaternion.Lerp(startMandiblePos, targetMandiblePos, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Garante que as posições finais sejam atingidas
        leftTooth.localPosition = targetLeftToothPos;
        rightTooth.localPosition = targetRightToothPos;
        if(left2Tooth && right2Tooth) {
            left2Tooth.localPosition = targetLeft2ToothPos;
            right2Tooth.localPosition = targetRight2ToothPos;
        }

        // Alterna o estado
        if(isInInitialPosition) {
            _renderButton.transform.rotation = Quaternion.Euler(0, 90, 90);
        } else {
            _renderButton.transform.rotation = Quaternion.Euler(180, 90, 90);
        }
        isInInitialPosition = !isInInitialPosition;
    }


    /// <summary>
    /// This method is called by the Main Camera when it starts gazing at this GameObject.
    /// </summary>
    public void OnPointerEnter()
    {
        _renderButton.material = textureClicked;
        SetMaterial(true);
    }

    /// <summary>
    /// This method is called by the Main Camera when it stops gazing at this GameObject.
    /// </summary>
    public void OnPointerExit()
    {
        _renderButton.material = textureBase;
        SetMaterial(false);
    }

    /// <summary>
    /// This method is called by the Main Camera when it is gazing at this GameObject and the screen
    /// is touched.
    /// </summary>
    public void OnPointerClick()
    {
        if(!isClicked) {
            StartCoroutine(MoveTeeth());
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
