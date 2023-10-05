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
    public class Pickup : MonoBehaviour
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
        private GameObject bananaWall;
        private GameObject exit;
        private GameObject taser;
        public float zapRange = 5;


        private void Start()
        {
            input = GetComponent<StarterAssetsInputs>();
            bananaWall = GameObject.Find("Banana Wall");
            exit = GameObject.Find("Exit");
            taser = GameObject.Find("Taser");
        }
        // Update is called once per frame
        void Update()
        {
            if (input.taser && heldObject == taser && timeoutDelta <= 0.0f)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, zapRange))
                {
                    zapEnemy(hit.transform.gameObject);
                }

                timeoutDelta = timeout;
                input.TaserInput(false);
            }

            if (input.pickup && timeoutDelta <= 0.0f)
            {
                if (heldObject == null)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickupRange))
                    {
                        pickupObject(hit.transform.gameObject);
                    }
                }
                else
                {
                    dropObject();
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
            if (obj.GetComponent<Rigidbody>())
            {
                Rigidbody rigid = obj.GetComponent<Rigidbody>();
                rigid.useGravity = false;
                // Changes speed at which object moves
                rigid.drag = 10;

                rigid.transform.parent = holdParent;
                heldObject = obj;
            }

            Debug.Log(obj.name.ToString());
            if (obj.CompareTag("Non-Holdable"))
            {
                Debug.Log("got em!");
                //     Rigidbody rigid = obj.GetComponent<Rigidbody>();
                obj.SetActive(false);

                //     rigid.transform.parent = holdParent;
                //     heldObject = obj;
                if (obj.name.Equals("box"))
                {
                    bananaWall.SetActive(false);
                }
                if (obj.name.Equals("Card"))
                {
                    exit.SetActive(false);
                }
            }
        }

        void dropObject()
        {
            Rigidbody rigid = heldObject.GetComponent<Rigidbody>();
            rigid.useGravity = true;
            rigid.drag = 1;
            rigid.transform.parent = null;
            heldObject = null;
        }

        void zapEnemy(GameObject obj)
        {
            Debug.Log("hit something");
            if (obj.CompareTag("Enemy"))
            {
                Debug.Log("GOTEM");
                obj.GetComponent<Rigidbody>().AddExplosionForce(100, new Vector3(0, 0, 0), 10);
            }
        }
    }
}