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
public class MouthBreathController : MonoBehaviour
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
    
    public Transform upperPart; 
    public Transform tongue;    
    public Transform lowerPart; 

    public float breathSpeed = 1f; // Velocidade da respiração
    public float breathAmount = 0.2f; // Quantidade de movimento da respiração

    private Vector3 initialUpperPos;
    private Vector3 initialLowerPos;
    private Vector3 initialTonguePos;


    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    void Start()
    {
        // Armazena as posições iniciais dos objetos
        initialUpperPos = upperPart.localPosition;
        initialLowerPos = lowerPart.localPosition;
        initialTonguePos = tongue.localPosition;
    }

    void Update()
    {
        float breathOffset = Mathf.Sin(Time.time * breathSpeed) * breathAmount;
        upperPart.localPosition = initialUpperPos + new Vector3(0, breathOffset, 0);
        lowerPart.localPosition = initialLowerPos - new Vector3(0, breathOffset, 0);
        tongue.localPosition = initialTonguePos + new Vector3(0, breathOffset / 2f, 0);
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
