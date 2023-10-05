using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Jumpscare : MonoBehaviour
{
    public GameObject playerCamera;
    public GameObject scareCamera;
    public AudioSource scream;
    public Light scareLight;
    private float counter = -1;

    private GameObject gameStarter;

    private void Start()
    {
        scareCamera.SetActive(false);
        gameStarter = GameObject.Find("GameStarter");
    }

    private void Update()
    {
        if (counter >= 0)
        {
            counter ++;
            if (counter > 20)
            {
                counter = 0;
                scareLight.GetComponent<Light>().enabled = !scareLight.GetComponent<Light>().enabled;
            }
        }
    }
    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.CompareTag("Enemy"))
        {
            Debug.Log("YOU JUST GOT JUMPSCARED");
            scream.Play();
            playerCamera.SetActive(false);
            scareCamera.SetActive(true);
            
            GameObject enemy = collisionInfo.collider.gameObject;
            enemy.GetComponent<EnemyNavMesh>().isRunning = false;
            enemy.GetComponent<Rigidbody>().Sleep();
            enemy.GetComponent<Rigidbody>().useGravity = false;
            enemy.GetComponent<NavMeshAgent>().enabled = false;
            enemy.transform.SetPositionAndRotation(new Vector3(scareCamera.transform.position.x,
                scareCamera.transform.position.y - 2.5f, scareCamera.transform.position.z + 2),
                new Quaternion(0, 180   , 0, 0));
            enemy.GetComponent<Animator>().speed = 3;

            counter = 0;
            StartCoroutine(EndScare());
        }
    }
    IEnumerator EndScare()
    {
        yield return new WaitForSeconds(3.0f);
        playerCamera.SetActive(true);
        scareCamera.SetActive(false);
        counter = -1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene("Menu screen");
    }
}
