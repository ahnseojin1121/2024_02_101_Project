using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //�÷��̾��� ������ �ӵ��� �����ϴ� ����
    [Header("Player Movement")]
    public float moveSpeed = 5.0f;
    public float jumpforce = 5.0f;
    public float rotationSpeed = 10f;

    [Header("Camera Settings")]
    public Camera firstPersonCamera;
    public Camera thirdPersonCamera;

    public float cameraDistance = 5.0f;
    public float minDistance = 1.0f;
    public float maxDistance = 10.0f;

    private float CurrentX = 0.0f;
    private float CurrentY = 45.0f;

    private const float Y_ANGLE_MIN = 0.0f;
    private const float Y_ANGLE_MAX = 50.0f;
    public float mouseSensitivity = 200f;


    public float radius = 5.0f;
    public float minRadius = 1.0f;
    public float maxRadius = 10.0f;

    public float yMinLimit = 30;
    public float yMaxLimit = 90;

    private float theta = 0.0f;
    private float phi = 0.0f;
    private float targetVerticalRotation = 0f;
    private float verticalRotationSpeed = 240f;


    //���� ������
    public bool isFirstPerson = true;
    private bool isGrounded;
    private Rigidbody rb;

    public float fallingThreshold = -0.1f;

    [Header("Ground Check Setting")]
    public float groundCheckDistance = 0.3f;
    public float slopedLimit = 45f;
    public const int groundCheckPoints = 5;

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
        HandleRotation();
        HandleJump();
        HandleCameraToggle();

    }

    private void FixedUpdate()
    {
        HandleMovement();
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

        if(isFirstPerson)
        {
            transform.rotation = Quaternion.Euler(0.0f, theta, 0.0f);
            firstPersonCamera.transform.localRotation = Quaternion.Euler(phi, 0.0f, 0.0f);
        }
        else
        {
           CurrentX += mouseX;
           CurrentY -= mouseY;

           CurrentY = Mathf.Clamp(CurrentY, Y_ANGLE_MIN, Y_ANGLE_MAX);

           Vector3 dir = new Vector3(0, 0, cameraDistance);
           Quaternion rotation =Quaternion.Euler(CurrentY, CurrentX, 0.0f);
           thirdPersonCamera.transform.position = transform.position + rotation * dir;
           thirdPersonCamera.transform.LookAt(transform.position);

           cameraDistance = Mathf.Clamp(cameraDistance - Input.GetAxis("Mouse ScrollWheel") * 5, minDistance, maxDistance);
        }
    }

    //1��Ī
    public void HandleCameraToggle()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            isFirstPerson = !isFirstPerson;
            SetActiveCamera();

        }
    }

    //ī�޶� �ʱ� ��ġ �� ȸ���� �����ϴ� �Լ�
    void SetupCameras()
    {
        firstPersonCamera.transform.localPosition = new Vector3(0f, 0.6f, 0f);
        firstPersonCamera.transform.localRotation = Quaternion.identity;
    }
    void HandleJump()
    {
        //���� ��ư�� ������ ���� ���� ��
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
            isGrounded = false;
        }
    }


    public void HandleMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement;
        if(!isFirstPerson)
        {
            Vector3 cameraForward = thirdPersonCamera.transform.forward;
            cameraForward.y = 0f;
            cameraForward.Normalize();

            Vector3 cameraRight = thirdPersonCamera.transform.right;
            cameraRight.y = 0f;
            cameraRight.Normalize();

            //�̵� ���� ���
            movement = cameraForward * moveVertical + cameraRight * moveHorizontal;

        }
        else
        {
            //ĳ���� �������� �̵�
            movement = transform.right * moveHorizontal + transform.forward * moveVertical;
          
        }
        
        //�̵� �������� ĳ���� ȸ��
        if(movement.magnitude > 0.1f)
        {
            Quaternion torotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, torotation, rotationSpeed * Time.deltaTime);
        }

        rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);
    }

    public bool isFalling()
    {
        return Physics.Raycast(transform.position, Vector3.down, 2.0f);
    }

    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 2.0f);
    }

    public float GetVerticalVelocity()
    {
        return rb.velocity.y;
    }

    
}
