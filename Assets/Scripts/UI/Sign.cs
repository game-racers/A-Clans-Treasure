using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
    public class Sign : MonoBehaviour
    {
        [SerializeField] GameObject image; 
        [SerializeField] float maxSize = 1f;
        [SerializeField] float maxHeight = 10f;
        [SerializeField] float shiftTime = .5f;
        [SerializeField] float ratio = 1;
        float minSize = 0.001f;
        float minHeight;
        float shiftingTimer = Mathf.Infinity;
        float shiftingBuffer = 0;
        IEnumerator coroutine;

        private void Start() 
        {
            minHeight = image.transform.localPosition.y;
        }

        private void OnTriggerEnter(Collider other) 
        {
            if (other.gameObject.tag == "Player")
            {
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                }
                coroutine = Shifting(maxSize, maxHeight);
                StartCoroutine(coroutine);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                }
                coroutine = Shifting(minSize, minHeight);
                StartCoroutine(coroutine);
            }
        }

        private IEnumerator Shifting(float target, float pos)
        {
            Vector3 startScale = image.transform.localScale;
            Vector3 startPos = image.transform.localPosition;
            Vector3 endPos = new Vector3(0, pos, 0);
            Vector3 endScale = new Vector3(target, target, target * ratio);

            // Test if mid shift
            if (shiftingTimer < shiftTime)
            {
                shiftingBuffer = shiftTime - shiftingTimer;
            }
            else
            {
                shiftingBuffer = 0;
            }
            shiftingTimer = 0;

            do 
            {
                image.transform.localScale = Vector3.Lerp(startScale, endScale, shiftingTimer / (shiftTime - shiftingBuffer));
                image.transform.localPosition = Vector3.Lerp(startPos, endPos, shiftingTimer / (shiftTime - shiftingBuffer));
                shiftingTimer += Time.deltaTime;
                yield return null;
            } 
            while (shiftingTimer < shiftTime);

            shiftingTimer = Mathf.Infinity;
        }
    }
}
