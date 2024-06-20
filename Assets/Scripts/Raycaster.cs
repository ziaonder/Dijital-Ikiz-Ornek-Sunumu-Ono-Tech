using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycaster : MonoBehaviour
{
    private Camera mainCam;
    private Ray ray;
    [SerializeField] private GameObject canvasModelName, canvasModelDetails;
    private Vector3 initialPos = new Vector3(-14.6f, 9.5f, -9.7f);
    private Vector3 initialDir = new Vector3(20.6f, 43.9f, 0f);
    private Vector3 zoomedPos = new Vector3(-9.7f, 6.7f, 18.7f);
    private Vector3 zoomedDir = new Vector3(20.6f, 130.6f, 0f);

    private void Awake()
    {
        mainCam = Camera.main;
    }

    private void Start()
    {
        canvasModelName.SetActive(true);
        canvasModelDetails.SetActive(false);
        mainCam.transform.position = initialPos;
        mainCam.transform.rotation = Quaternion.Euler(initialDir);
    }

    void Update()
    {
        // This is to block if the camera is not in the initial position.
        if (Vector3.Distance(transform.position, initialPos) > .1f)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            CastRay();
        }        
    }

    private void CastRay()
    {
        ray = mainCam.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            if(hit.collider.name == "Lab")
            {
                Debug.Log("Hit the lab");
                canvasModelName.SetActive(false);
                StartCoroutine(MoveAndRotateInTime());
            }
        }
    }

    private IEnumerator MoveAndRotateInTime()
    {
        float i = 0;
        while (i < 1f)
        {
            mainCam.transform.position = Vector3.Lerp(initialPos, zoomedPos, i);
            mainCam.transform.rotation = 
                Quaternion.Lerp(Quaternion.Euler(initialDir), Quaternion.Euler(zoomedDir), i);
            i += Time.deltaTime;
            yield return null;
        }

        canvasModelDetails.SetActive(true);
    }
}
