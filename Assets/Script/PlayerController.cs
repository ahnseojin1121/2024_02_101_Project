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

    //���� ������
    public bool isFirstPerson = true;
    //private bool isGrounded;
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
        HandleCameraToggle();

        if(Input.GetKeyDown(KeyCode.Space))
        {
            HandleJump();
        }

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

    public void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        //
        theta += mouseX;
        theta = Mathf.Repeat(theta, 360f);

        //���� ȸ�� ó��
        targetVerticalRotation -= mouseY;
        targetVerticalRotation = Mathf.Clamp(targetVerticalRotation, yMinLimit, yMaxLimit);
        phi = Mathf.MoveTowards(phi, targetVerticalRotation, verticalRotationSpeed * Time.deltaTime);

        if(isFirstPerson)
        {
            transform.rotation = Quaternion.Euler(0.0f, theta, 0.0f);
            firstPersonCamera.transform.localRotation = Quaternion.Euler(phi, 0.0f, 0.0f);
        }
        else
        {
            //3��Ī ī�޶� ���� ��ǥ�迡�� ��ġ �� ȸ�� ���
            float x = radius * Mathf.Sin(Mathf.Deg2Rad * phi) * Mathf.Cos(Mathf.Deg2Rad * theta);
            float y = radius * Mathf.Cos(Mathf.Deg2Rad * phi);
            float z = radius * Mathf.Sin(Mathf.Deg2Rad * phi) * Mathf.Cos(Mathf.Deg2Rad * theta);

            //���콺 ��ũ���� ����Ͽ� ī�޶� �� ����
            radius = Mathf.Clamp(radius - Input.GetAxis("Mouse ScrollWheel") *5 , minRadius, maxRadius);

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
        if (IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
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
