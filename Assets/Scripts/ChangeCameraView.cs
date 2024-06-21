using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCameraView : MonoBehaviour
{
    private Camera mainCam;
    private Ray ray;
    [SerializeField] private GameObject canvasModelName, canvasModelDetails, backButton;
    private Vector3 initialPos = new Vector3(-14.6f, 9.5f, -9.7f);
    private Vector3 initialDir = new Vector3(20.6f, 43.9f, 0f);
    private Vector3 zoomedPos = new Vector3(-9.7f, 6.7f, 18.7f);
    private Vector3 zoomedDir = new Vector3(20.6f, 130.6f, 0f);
    [SerializeField] private AudioClip clickSound, transitionSound;
    private AudioSource audioSource;

    private void Awake()
    {
        mainCam = Camera.main;
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        BackButton.OnBackButtonClicked += BackToInitial;
    }

    private void OnDisable()
    {
        BackButton.OnBackButtonClicked -= BackToInitial;
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
                StartCoroutine(MoveAndRotateInTime(true));
            }
        }
    }

    private IEnumerator MoveAndRotateInTime(bool isZooming)
    {
        float i = 0;
        i += Time.deltaTime;
        audioSource.PlayOneShot(clickSound);
        audioSource.PlayOneShot(transitionSound);

        if (isZooming)
        {
            canvasModelName.SetActive(false);
         
            while (i < 1f)
            {
                i += Time.deltaTime;
                mainCam.transform.position = Vector3.Lerp(initialPos, zoomedPos, i);
                mainCam.transform.rotation = 
                    Quaternion.Lerp(Quaternion.Euler(initialDir), Quaternion.Euler(zoomedDir), i);
                yield return null;
            }

            canvasModelDetails.SetActive(true);
            backButton.SetActive(true);
        }
        else
        {
            canvasModelDetails.SetActive(false);
            backButton.SetActive(false);

            while (i < 1f)
            {
                i += Time.deltaTime;
                mainCam.transform.position = Vector3.Lerp(zoomedPos, initialPos, i);
                mainCam.transform.rotation =
                    Quaternion.Lerp(Quaternion.Euler(zoomedDir), Quaternion.Euler(initialDir), i);
                yield return null;
            }

            canvasModelName.SetActive(true);
        }
    }

    private void BackToInitial()
    {
       StartCoroutine(MoveAndRotateInTime(false));
    }
}
