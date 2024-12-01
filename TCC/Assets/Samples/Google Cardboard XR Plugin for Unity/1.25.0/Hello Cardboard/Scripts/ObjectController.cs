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
public class ObjectController : MonoBehaviour
{
    /// <summary>
    /// The material to use when this object is inactive (not being gazed at).
    /// </summary>
    public Material InactiveMaterial;

    /// <summary>
    /// The material to use when this object is active (gazed at).
    /// </summary>
    public Material GazedAtMaterial;



    Vector3 initialUpPos, initialMidPos, initialDownPos, upPos, midPos, downPos;
    Quaternion initialRot =  Quaternion.Euler(0f, 180f, 0f);

    private bool isInitial = false;
    private float smoothTime = 0.5f;

    private float upDistance = -0.15f;
    private float midDistance = 0.4f;
    private float downDistance = 0.2f;

    private Quaternion upRot = Quaternion.Euler(-20f, 0f, 0f);
    private Quaternion downRot = Quaternion.Euler(30f, 0f, 0f);


    private Renderer _myRenderer;
    private Vector3 _startingPosition;

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    public void Start()
    {
        _startingPosition = transform.parent.localPosition;
        _myRenderer = GetComponent<Renderer>();
        getInitialPosition();
        SetMaterial(false);
    }

    public void getInitialPosition()
    {
        GameObject upSide = transform.GetChild(0).gameObject;
        GameObject midSide = transform.GetChild(1).gameObject;
        GameObject downSide = transform.GetChild(2).gameObject;

        initialUpPos = upSide.transform.localPosition;
        initialMidPos = midSide.transform.localPosition;
        initialDownPos = downSide.transform.localPosition;

        upPos = transform.forward * upDistance;
        midPos = transform.up * midDistance;
        downPos = transform.up * downDistance;
    }

    private IEnumerator rotateObject()
    {   
        float duration = smoothTime;

        GameObject upSide = transform.GetChild(0).gameObject;
        GameObject midSide = transform.GetChild(1).gameObject;
        GameObject downSide = transform.GetChild(2).gameObject;
        
        Vector3 startUpPos, targetUpPos, startMidPos, targetMidPos, startDownPos, targetDownPos;
        Quaternion startUpRot, targetUpRot, startDownRot, targetDownRot;

        if(isInitial)
        {
            startUpPos = initialUpPos;
            targetUpPos = initialUpPos + upPos;
            
            startMidPos = initialMidPos;
            targetMidPos = initialMidPos - midPos;

            startDownPos = initialDownPos;
            targetDownPos = initialDownPos - downPos;

            startUpRot = initialRot;
            targetUpRot = initialRot * upRot;

            startDownRot = initialRot;
            targetDownRot = initialRot * downRot;
        }

        else 
        {
            startUpPos = initialUpPos + upPos;
            targetUpPos = initialUpPos;
            
            startMidPos = initialMidPos - midPos;
            targetMidPos = initialMidPos;

            startDownPos = initialDownPos - downPos;
            targetDownPos = initialDownPos;

            startUpRot = initialRot * upRot;
            targetUpRot = initialRot;

            startDownRot = initialRot * downRot;
            targetDownRot = initialRot;
        }


        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            upSide.transform.localPosition = Vector3.Lerp(startUpPos, targetUpPos, t);
            midSide.transform.localPosition = Vector3.Lerp(startMidPos, targetMidPos, t);
            downSide.transform.localPosition = Vector3.Lerp(startDownPos, targetDownPos, t);
            
            upSide.transform.localRotation = Quaternion.Slerp(startUpRot, targetUpRot, t);
            downSide.transform.localRotation = Quaternion.Slerp(startDownRot, targetDownRot, t);

            elapsedTime += Time.deltaTime;
            yield return null; 
        }

        // Garante que os objetos cheguem às posições finais
        upSide.transform.localPosition = targetUpPos;
        midSide.transform.localPosition = targetMidPos;
        downSide.transform.localPosition = targetDownPos;
        
        upSide.transform.localRotation = targetUpRot;
        downSide.transform.localRotation = targetDownRot;
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
        // TeleportRandomly();
        isInitial = !isInitial;
        StartCoroutine(rotateObject());
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
