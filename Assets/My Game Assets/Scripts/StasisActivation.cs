using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StasisActivation : MonoBehaviour
{
    [SerializeField] MeshRenderer renderer;
    [SerializeField] AnimationCurve radarCurve;
    [SerializeField] float duration = 1;
    [SerializeField] float activationLuminosity = 2;
    [SerializeField] float deactivationLuminosity = 0;

    // debug Stats
    [SerializeField] float targetLuminosity = 0;
    [SerializeField] float currentCachedLuminosity;
    [SerializeField] float elapsedTime = 0;
    [SerializeField] bool activated = false;



    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            activated = activated ? false : true;
            elapsedTime = 0;
            targetLuminosity = activated ? activationLuminosity : deactivationLuminosity;
            currentCachedLuminosity = renderer.material.GetFloat("_Activation");
        }

        if (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            t = radarCurve.Evaluate(t);
            float activationLerp = Mathf.Lerp(currentCachedLuminosity, targetLuminosity, t);
            renderer.material.SetFloat("_Activation", activationLerp);
        }
    }
}
