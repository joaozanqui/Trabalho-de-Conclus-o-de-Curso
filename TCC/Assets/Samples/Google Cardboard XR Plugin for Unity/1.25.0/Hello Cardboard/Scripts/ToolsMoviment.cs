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
public class ToolsMoviment : MonoBehaviour
{
    /// <summary>
    /// The material to use when this object is inactive (not being gazed at).
    /// </summary>
    public Material InactiveMaterial;
    private Renderer _renderMenu;
    private Renderer _renderSign;

    public Transform tool1;
    public Transform tool2;
    public Transform tool3;
    public Transform tool4;
    private bool hasTool4 = true;
    public float tagetMoviment;

    private Vector3 initialTool1Pos;
    private Vector3 initialTool2Pos;
    private Vector3 initialTool3Pos;
    private Vector3 initialTool4Pos;


    /// <summary>
    /// The material to use when this object is active (gazed at).
    /// </summary>
    public Material GazedAtMaterial;

    private Renderer _myRenderer;
    private float smoothTime = 0.5f;

    private int toolPosition = 0;
    private bool isClicked = false;

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    void Start()
    {
        initialTool1Pos = tool1.localPosition;
        initialTool2Pos = tool2.localPosition;
        initialTool3Pos = tool3.localPosition;
        if(tool4 == null) {
            hasTool4 = false;
        }
        if(hasTool4)
            initialTool4Pos = tool4.localPosition;
        
    }

    private IEnumerator SetIfIsClicked()
    {
        isClicked = true;

        // Espera pelo tempo especificado
        yield return new WaitForSeconds(smoothTime);

        // Define a variável como false
        isClicked = false;
    }

    IEnumerator MoveTools()
    {
        float duration = 0.5f;
        float elapsedTime = 0f;

        Vector3 startTool1Pos, targetTool1Pos;
        Vector3 startTool2Pos, targetTool2Pos;
        Vector3 startTool3Pos, targetTool3Pos;
        Vector3 startTool4Pos = Vector3.zero;
        Vector3 targetTool4Pos = Vector3.zero;

        startTool1Pos = tool1.localPosition;
        startTool2Pos = tool2.localPosition;
        startTool3Pos = tool3.localPosition;
        if(hasTool4)
            startTool4Pos = tool4.localPosition;

        targetTool1Pos = initialTool1Pos;
        targetTool2Pos = initialTool2Pos;
        targetTool3Pos = initialTool3Pos;
        if(hasTool4)
            targetTool4Pos = initialTool4Pos;

        if (toolPosition == 0) {          
            targetTool1Pos = initialTool1Pos + new Vector3(0, tagetMoviment, 0);
            toolPosition = 1;
        } else if (toolPosition == 1) {          
            targetTool2Pos = initialTool2Pos + new Vector3(0, tagetMoviment, 0);
            toolPosition = 2;
        } else if (toolPosition == 2) {          
            targetTool3Pos = initialTool3Pos + new Vector3(0, tagetMoviment, 0);
            toolPosition = 3;
        } else if (toolPosition == 3) {
            if(hasTool4) {
                toolPosition = 4;
                targetTool4Pos = initialTool4Pos + new Vector3(0, tagetMoviment, 0);
            }
            else
                toolPosition = 0;
        } else if (toolPosition == 4) {
            toolPosition = 0;
        }

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            tool1.localPosition = Vector3.Lerp(startTool1Pos, targetTool1Pos, t);
            tool2.localPosition = Vector3.Lerp(startTool2Pos, targetTool2Pos, t);
            tool3.localPosition = Vector3.Lerp(startTool3Pos, targetTool3Pos, t);
            if(hasTool4)
                tool4.localPosition = Vector3.Lerp(startTool4Pos, targetTool4Pos, t);
            

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        tool1.localPosition = targetTool1Pos;
        tool2.localPosition = targetTool2Pos;
        tool3.localPosition = targetTool3Pos;
        if(hasTool4)
            tool4.localPosition = targetTool4Pos;
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
            StartCoroutine(MoveTools());
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
