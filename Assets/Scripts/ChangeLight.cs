using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
	[RequireComponent(typeof(PlayerInput))]
#endif
    public class ChangeLight : MonoBehaviour
    {
        private Light myLight;
        private StarterAssetsInputs input;
        private float timeoutDelta;
        public float timeout = 0.15f;
        // Start is called before the first frame update
        void Start()
        {
            myLight = GetComponent<Light>();
            input = GetComponent<StarterAssetsInputs>();
        }

        // Update is called once per frame
        void Update()
        {
            if (input.flashlight && timeoutDelta <= 0.0f)
            {
                myLight.enabled = !myLight.enabled;
                timeoutDelta = timeout;
                input.FlashlightInput(false);
            }
            if (timeoutDelta > 0.0f)
            {
                timeoutDelta -= Time.deltaTime;
            }
        }
    }
}
