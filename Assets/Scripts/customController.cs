using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class customController : MonoBehaviour {

    //TODO apply physics to small objects, desvio.cs, detroy objects. 
       
    [Header("Character Controller Parameters")]
    [SerializeField] float speed = 6.0f;
    [SerializeField] float jumpSpeed = 8.0f;
    [SerializeField] float gravity = 20.0f;
    [SerializeField] float sensitivity = 4f;
    [SerializeField] float smoothness = 2f;  

    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;

    //Camera controller
    Vector2 mouseLook;
    Vector2 smoothLook;
    

    void Start () {
        controller =GetComponentInChildren<CharacterController>();        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;        
    }
   
    void Update () {

        CameraControl();
        CustomMovement();     
    }  

   void CustomMovement() {
      if (controller.isGrounded)
        {
            
            moveDirection = new Vector3((Input.GetAxis("Horizontal")), 0.0f, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection.normalized); //to walk always in facing direction
                     
            moveDirection = moveDirection * speed;
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }

        // Apply gravity
        moveDirection.y = moveDirection.y - (gravity * Time.deltaTime);            
                    controller.Move(moveDirection * Time.deltaTime);        
    }
   
    void CameraControl()  //Look around with the camera using the mouse
    {
        Vector2 mouseDir = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        mouseDir = Vector2.Scale(mouseDir, new Vector2(sensitivity * smoothness, sensitivity * smoothness));
        smoothLook.x = Mathf.Lerp(smoothLook.x, mouseDir.x, 1f / smoothness);
        smoothLook.y = Mathf.Lerp(smoothLook.y, mouseDir.y, 1f / smoothness);
        mouseLook += smoothLook;
        mouseLook.y = Mathf.Clamp(mouseLook.y, -90f, 90f);
        transform.GetChild(0).transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
        transform.rotation = Quaternion.AngleAxis(mouseLook.x, transform.up);
        
    }

   
           

    


  
}
