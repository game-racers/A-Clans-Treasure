using System.Collections;
using UnityEngine;

namespace RPG.UI 
{
    public class ImageSize : MonoBehaviour
    {
        [SerializeField] float maxSize = 700f;
        [SerializeField] float shiftTime = .5f;
        [SerializeField] float shiftedXPos = -200f;
        [SerializeField] float shiftedYPos = -50f;
        [SerializeField] float xPos = -35f;
        [SerializeField] float yPos = -35f;
        [SerializeField] bool isOpen = false;
        static bool hasOpened = false;
        Vector2 smallPos;
        Vector2 largePos;
        RectTransform rect;
        float minSize = .1f;
        float shiftingTimer = Mathf.Infinity;
        float shiftingBuffer = 0;

        IEnumerator coroutine;


        private void Start() 
        {
            rect = GetComponent<RectTransform>(); 
            smallPos = rect.anchoredPosition;
            largePos = new Vector2(shiftedXPos, shiftedYPos);
            if (isOpen && !hasOpened)
            {
                WillOpen();
                Open();
                isOpen = true;
                hasOpened = true;
            }
        }

        private IEnumerator WillOpen()
        {
            yield return new WaitForSeconds(3);
        }

        public void Open()
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            coroutine = Shifting(maxSize, largePos);
            StartCoroutine(coroutine);
        }

        public void Close() 
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            coroutine = Shifting(minSize, smallPos);
            StartCoroutine(coroutine);
        }

        private IEnumerator Shifting(float target, Vector2 pos)
        {
            Vector2 startScale = rect.sizeDelta;
            Vector2 startPos = rect.anchoredPosition;
            // endPos = pos
            Vector2 endScale = new Vector2(target, target);

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
                rect.anchoredPosition = Vector2.Lerp(startPos, pos, shiftingTimer / (shiftTime - shiftingBuffer));
                rect.sizeDelta = Vector2.Lerp(startScale, endScale, shiftingTimer / (shiftTime - shiftingBuffer));
                shiftingTimer += Time.deltaTime;
                yield return null;
            }
            while (shiftingTimer < shiftTime);

            shiftingTimer = Mathf.Infinity;
        }
    }
}