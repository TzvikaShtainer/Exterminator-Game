using System;
using System.Collections;
using UnityEngine;

namespace Prefabs.Framework.Damage
{
    public class Scanner : MonoBehaviour
    {
        [SerializeField] private Transform scannerPivot;
        
        public delegate void OnScanDetectionUpdated(GameObject newDetection);

        public event OnScanDetectionUpdated onScanDetectionUpdated;

        [SerializeField] private float scanRange;
        [SerializeField] private float scanDuration = 2f;
        
        internal void SetScanRange(float scanRange)
        {
            this.scanRange = scanRange;
        }

        internal void SetScanDuration(float duration)
        {
            scanDuration = duration;
        }

        public void AddChildAttached(Transform newChild)
        {
            newChild.parent = scannerPivot;
            newChild.localPosition = Vector3.zero;
        }

        public void StartScan()
        {
            scannerPivot.localScale = Vector3.zero;
            StartCoroutine(StartScanCoroutine());
        }

        private IEnumerator StartScanCoroutine()
        {
            float scanGrowthRate = scanRange / scanDuration;
            float startTime = 0;
            while (startTime < scanDuration)
            {
                startTime += Time.deltaTime;
                scannerPivot.localScale += Vector3.one * (scanGrowthRate * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            
            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            onScanDetectionUpdated?.Invoke(other.gameObject);
        }
    }
}
