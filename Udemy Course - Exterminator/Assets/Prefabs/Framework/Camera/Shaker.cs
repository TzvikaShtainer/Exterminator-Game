using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Prefabs.Framework.Camera
{
    public class Shaker : MonoBehaviour
    {
        [SerializeField] private float shakeMag = 0.1f;
        [SerializeField] private float shakeDuration = 0.1f;
        [SerializeField] private Transform shakeTransform;
        [SerializeField] private float shakeRecoverySpeed = 10f;

        private Coroutine shakeCoroutine;
        private bool isShaking;
        
        
        private Vector3 originalPos;

        private void Start()
        {
            originalPos = transform.position;
        }

        public void StartShake()
        {
            if (shakeCoroutine == null)
            {
                isShaking = true;
                shakeCoroutine = StartCoroutine(ShakeStarted());
            }
        }

        IEnumerator ShakeStarted()
        {
            yield return new WaitForSeconds(shakeDuration);
            isShaking = false;
            shakeCoroutine = null;
        }

        private void LateUpdate()
        {
            ProcessShake();
        }

        private void ProcessShake()
        {
            if (isShaking)
            {
                Vector3 shakeAmt = new Vector3(Random.value, Random.value, Random.value) * shakeMag *
                                   (Random.value > 0.5 ? -1 : 1);

                shakeTransform.localPosition += shakeAmt;
            }
            else
            {
                shakeTransform.localPosition = Vector3.Lerp(shakeTransform.localPosition, originalPos, Time.deltaTime * shakeRecoverySpeed);
            }
        }
    }
}
