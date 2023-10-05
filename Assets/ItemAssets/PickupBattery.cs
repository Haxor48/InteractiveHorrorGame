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
    public class PickupBattery : MonoBehaviour
    {
        // Start is called before the first frame update
        //Adjust pickupRange & movementDist to be 2 & 100 respectively
        public float pickupRange = 5;
        public Transform holdParent;
        public float movementDist = 250;
        private GameObject heldObject;
        private StarterAssetsInputs input;
        private float timeoutDelta;
        public float timeout = 0.15f;

        private void Start()
        {
            input = GetComponent<StarterAssetsInputs>();

        }
        // Update is called once per frame
        void Update()
        {

            if (input.pickup && timeoutDelta <= 0.0f)
            {
                Debug.Log("test");

                if (heldObject == null)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickupRange))
                    {
                        pickupObject(hit.transform.gameObject);
                    }
                }

                timeoutDelta = timeout;
                input.PickupInput(false);

            }

            if (timeoutDelta > 0.0f)
            {
                timeoutDelta -= Time.deltaTime;
            }

            if (heldObject != null)
            {
                moveObject();
            }
        }

        void moveObject()
        {
            if (Vector3.Distance(heldObject.transform.position, holdParent.position) > .1f)
            {
                Vector3 displacement = holdParent.position - heldObject.transform.position;
                heldObject.GetComponent<Rigidbody>().AddForce(displacement * movementDist);
            }
        }

        void pickupObject(GameObject obj)
        {
            Debug.Log(obj.name.ToString());
            if (obj.CompareTag("Non-Holdable"))
            {
                Debug.Log("got em!");
                //     Rigidbody rigid = obj.GetComponent<Rigidbody>();
                obj.SetActive(false);

                //     rigid.transform.parent = holdParent;
                //     heldObject = obj;
            }
        }
    }
}