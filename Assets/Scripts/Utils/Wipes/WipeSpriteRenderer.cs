using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Utils.Wipes
{
    public class WipeSpriteRenderer : MonoBehaviour
    {
        [Tooltip("The sprite renderer to which the WipeMaterial is attached. WipeMaterial should be the first instantiated material (the one returned by spriteRenderer.material)")]
        [SerializeField] SpriteRenderer spriteRenderer;
        [SerializeField] Texture2D wipeSprite;

        [Tooltip("The time it takes to play the full transition, in seconds. A negative value makes it instantaneous")]
        [SerializeField] float transitionDuration;

        [Header("Play on start")]
        [SerializeField] bool fadeInOnStart;
        [SerializeField] bool fadeOutOnStart;
        [SerializeField] bool disableOnDoneFading;

        void Start()
        {
            if (fadeInOnStart)
            {
                if (fadeOutOnStart)
                    Debug.LogWarning("Fade in/out on start both set to true, fading in.");
                FadeIn(disableOnDoneFading);
            }
            else if (fadeOutOnStart)
            {
                FadeOut(disableOnDoneFading);
            }
        }

        public void FadeIn(bool disableOnOver = false)
        {
            StartCoroutine(PlayCoroutine(0, 1, disableOnOver));
        }

        public void FadeOut(bool disableOnOver = true)
        {
            StartCoroutine(PlayCoroutine(1, 0, disableOnOver));
        }

        IEnumerator PlayCoroutine(float cutoffFrom, float cutoffTo, bool diableOnOver = false)
        {
            spriteRenderer.gameObject.SetActive(true);
            spriteRenderer.material.SetTexture("_WipeTex", wipeSprite);

            if (transitionDuration > 0)
            {
                spriteRenderer.material.SetFloat("_Cutoff", cutoffFrom);
                float elapsed = 0;
                while (elapsed < transitionDuration)
                {
                    float t = elapsed / transitionDuration;
                    float cutoff = Mathf.Lerp(cutoffFrom, cutoffTo, t);
                    spriteRenderer.material.SetFloat("_Cutoff", cutoff);

                    elapsed += Time.deltaTime;
                    yield return null;
                }

            }

            spriteRenderer.material.SetFloat("_Cutoff", cutoffTo);


            if (diableOnOver)
            {
                yield return null;
                spriteRenderer.gameObject.SetActive(false);
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(WipeSpriteRenderer))]
    public class WipeSpriteRendererEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            WipeSpriteRenderer renderer = target as WipeSpriteRenderer;

            EditorGUILayout.Space();

            if (GUILayout.Button("Fade In"))
                renderer.FadeIn();
            if (GUILayout.Button("Fade Out"))
                renderer.FadeOut();
        }
    }
#endif
}