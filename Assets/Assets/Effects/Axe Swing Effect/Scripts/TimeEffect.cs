using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class TimeEffect : MonoBehaviour
    {
        private Light axeLight;        
        Animation m_Animation;

        void Awake()
        {
            m_Animation = GetComponent<Animation>();
            axeLight = GetComponentInParent<Light>();
            gameObject.SetActive(false);
        }
        public void Activate()
        {
            gameObject.SetActive(true);
            axeLight.enabled = true;

            if (m_Animation)
                m_Animation.Play();

            StartCoroutine(DisableAtEndOfAnimation());
        }
        IEnumerator DisableAtEndOfAnimation()
        {
            yield return new WaitForSeconds(m_Animation.clip.length);
            gameObject.SetActive(false);
            axeLight.enabled = false;
        }
    } 
