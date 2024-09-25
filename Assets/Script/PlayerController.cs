using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //플레이어의 움직임 속도를 설정하는 변수
    [Header("Player Movement")]
    public float moveSpeed = 5.0f;
    public float jumpforce = 5.0f;

    [Header("Camera Settings")]
    public Camera firstPersonCamera;
    public Camera thirdPersonCamera;

    public float radius = 5.0f;
    public float minRadius = 1.0f;
    public float maxRadius = 10.0f;

    public float yMinLimit = 30;
    public float yMaxLimit = 90;

    private float theta = 0.0f;
    private float phi = 0.0f;
    private float targetVerticalRotation = 0f;
    private float verticalRotationSpeed = 240f;

    public float mouseSensitivity = 2f;

    //내부 변수들
    private bool isFirstPerson = true;
    private bool isGrounded;
    private Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        SetupCameras();

    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleJump();
        HandleCameraToggle();


    }

    void SetActiveCamera()
    {
        firstPersonCamera.gameObject.SetActive(isFirstPerson);
        thirdPersonCamera.gameObject.SetActive(isFirstPerson);
    }

    void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        //
        theta += mouseX;
        theta = Mathf.Repeat(theta, 360f);

        //수직 회전 처리
        targetVerticalRotation -= mouseY;
        targetVerticalRotation = Mathf.Clamp(targetVerticalRotation, yMinLimit, yMaxLimit);
        phi = Mathf.MoveTowards(phi, targetVerticalRotation, verticalRotationSpeed * Time.deltaTime);

        //
        transform.rotation = Quaternion.Euler(0.0f, theta, 0.0f);

        firstPersonCamera.transform.localRotation = Quaternion.Euler(phi,0.0f, 0.0f);

        if(isFirstPerson)
        {
            firstPersonCamera.transform.localRotation = Quaternion.Euler(phi, 0.0f, 0.0f);
        }
        else
        {
            //3인칭 카메라 구면 좌표계에서 위치 및 회전 계산
            float x = radius * Mathf.Sin(Mathf.Deg2Rad * phi) * Mathf.Cos(Mathf.Deg2Rad * theta);
            float y = radius * Mathf.Cos(Mathf.Deg2Rad * phi);
            float z = radius * Mathf.Sin(Mathf.Deg2Rad * phi) * Mathf.Cos(Mathf.Deg2Rad * theta);

            //마우스 스크롤을 사용하여 카메라 줌 조정
            radius = Mathf.Clamp(radius - Input.GetAxis("Mouse ScrollWheel") *5 , minRadius, maxRadius);

        }
    }

    //1인칭
    void HandleCameraToggle()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            isFirstPerson = !isFirstPerson;
            SetActiveCamera();

        }
    }

    //카메라 초기 위치 및 회전을 설정하는 함수
    void SetupCameras()
    {
        firstPersonCamera.transform.localPosition = new Vector3(0f, 0.6f, 0f);
        firstPersonCamera.transform.localRotation = Quaternion.identity;
    }
    void HandleJump()
    {

        //점프 버튼을 누르고 땅에 있을 때
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
            isGrounded = false;
        }
    }


    void HandleMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        if(!isFirstPerson)
        {
            Vector3 cameraForward = thirdPersonCamera.transform.forward;
            cameraForward.y = 0f;
            cameraForward.Normalize();

            Vector3 cameraRight = thirdPersonCamera.transform.right;
            cameraRight.y = 0f;
            cameraRight.Normalize();

            Vector3 movement =cameraForward * moveVertical + cameraRight * moveHorizontal;
            rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);

        }
        else
        {

            //캐릭터 기준으로 이동
            Vector3 movement = transform.right * moveHorizontal + transform.forward * moveVertical;
            rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);

        }
        
    }

    void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }
}
