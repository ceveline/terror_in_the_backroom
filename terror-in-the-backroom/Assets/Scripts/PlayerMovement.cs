using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float mouseSensitivity = 2f;
    public float jumpForce = 5f;
    public float gravity = 9.8f;
    private Camera playerCamera;
    private CharacterController playerController;
    private Vector3 direction;
    private float verticalVelocity;
    private float verticalRotation = 0f;


    bool isCollidingWithSkeleton = false;
    Collider colliderOfObjectToCollect;

    bool isRunning = false;
    float runSpeed = 10f;

   public  GameObject inventory;
   bool inventoryStatus = false;

    public float carryDistance = 2.0f;
    public GameObject mallet;
    GameObject newMallet;
    SkeletonComponent skeleton;
    GameObject skeletonInstance;

    bool isCarrying = false;
    bool atDropoffLocation = false;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<CharacterController>();
        playerCamera = Camera.main;

        //lock cursor to center of screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        HandlePlayerMovement();
       
        //we only allow the mouse to rotate the camera is the inventory s not active
        if(!inventoryStatus)
        {
         LookAround();
        }

        //Check if player is trying to attack a skeleton
        CheckForMallet();

        //Open the inventory if the player is pressing I
        OpenCloseInventory();

        //Drop Off Items if player is pressing V
        DropOffItems();


    }

    void HandlePlayerMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (playerController.isGrounded)
        {
            isRunning = Input.GetButton("Run");

            direction = new Vector3(horizontal, 0, vertical);
           
            direction = transform.TransformDirection(direction) * ((isRunning) ? runSpeed : speed);

            if (Input.GetButton("Jump"))
            {
                verticalVelocity = jumpForce;
            }
        }

        verticalVelocity = verticalVelocity - (gravity * Time.deltaTime);
        direction.y = verticalVelocity;

        playerController.Move(direction * Time.deltaTime);
    }


    void LookAround()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate the player around the Y-axis
        transform.Rotate(0, mouseX, 0);

        // Keep track of the camera's vertical rotation
        verticalRotation -= mouseY; // Decrease vertical rotation based on mouse input
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 45f); // Clamp vertical rotation

        // Apply the vertical rotation to the camera
        playerCamera.transform.localEulerAngles = new Vector3(verticalRotation, 0f, 0f);
    }


    public void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Skeleton"))
        {
            Debug.Log("trigger skeleton");
            isCollidingWithSkeleton = true;
            skeleton = other.gameObject.GetComponent<SkeletonComponent>();
            skeletonInstance = other.gameObject;
        }
        if (other.gameObject.CompareTag("Dropoff"))
        {
            Debug.Log("entered dropoff location");
            atDropoffLocation = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Skeleton"))
        {
            isCollidingWithSkeleton = false;
        }
        if (other.gameObject.CompareTag("Dropoff"))
        {
            atDropoffLocation = false;
        }
    }

    void CheckForMallet()
    {
        //check if player has pressing the left mouse button and has a mallet in their inventory
        if  (Input.GetMouseButtonDown(0))
        {
            Item mallet = InventoryManager.Instance.inInventory("Mallet");
            if (mallet != null && !isCollidingWithSkeleton)
            {
                if (!isCarrying)
                {
                    Carry (mallet);
                }
                else
                {
                    Drop(newMallet);
                }
            }
            if (isCollidingWithSkeleton && isCarrying)
            {
                Attack(newMallet);
            }
        }
    }

    void Carry (Item item)
    { 
        //carry mallet around 
        // Calculate position to place mallet in front of player
        Vector3 carryWeaponPosition = transform.position + transform.forward * carryDistance;

        //rotate mallet by 90 degrees so it looks like player is holding it facing it away from her
        Quaternion holdRotation = Quaternion.Euler(90, 0, 90);

        newMallet = Instantiate(mallet, carryWeaponPosition, holdRotation);

        // Set the player as the parent of the mallet so that it is attached to the player
        newMallet.transform.SetParent(transform);

        isCarrying = true;
    }

    void Drop(GameObject item)
    {
        //stop carrying item around
        Destroy(item);

        //set isCarrying back to false
        isCarrying = false;
    }


    void Attack(GameObject item)
    {
        Debug.Log("attack method called");

    //remove mallet from Inventory
    //InventoryManager.Instance.Remove(item);

        //swing mallet and reset back to upright position
        //using coroutine to add a small delay, without delay it looks like the mallet does not move at all
        StartCoroutine(SwingAndReset());

        //decrease skeleton health
        skeleton.healthbar.takeDamage(34);
        Debug.Log(skeleton.healthbar.health);

        //if skeleton health == 0
        //remove skeleton
        if (skeleton.healthbar.health < 0)
        { 
            Destroy(skeletonInstance);
            //set isColliding bool back to false
            isCollidingWithSkeleton = false;

            //stop carrying mallet
            item.SetActive(false);
        }

    }

    private IEnumerator SwingAndReset()
    {
        // Swing mallet down
        newMallet.transform.Rotate(0, -90, 0);

        // Wait for 1/2 a second
        yield return new WaitForSeconds(0.5f);

        // revert mallet back to original position after slight delay
        newMallet.transform.Rotate(0, 90, 0);
    }

    void OpenCloseInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            //set the inventory status to the opposite of what it currently is
            //allows the player to open and close the inventory by pressing the I key
            inventory.SetActive(!inventoryStatus);
            inventoryStatus = !inventoryStatus;

            //update inventory with items that have been collected
            InventoryManager.Instance.ListItems();
        }
    }

    void DropOffItems()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log("V being pressed");

            List<Item> items = InventoryManager.Instance.GetItems();
            float spaceOutDistance = 5.0f;

            //create a copy of itemList to safely iterate through
            var itemsCopy = new List<Item>(items);

            foreach (Item item in itemsCopy)
            {
                //save the amount of items being dropped off
                GameManager.Instance.UpdateItemsDroppedOff();

                //update items list
                InventoryManager.Instance.ListItems();

                //get the transforms of the items so that we can instantiate the game objects
                //List<Transform> itemTransforms = InventoryManager.Instance.getItemTransforms();
               // Debug.Log("tranforms : "  + itemTransforms.Count);

               

                //Instantiate GameObjects
                /*foreach(Transform itemTransform in itemTransforms)
                {*/
                    //Debug.Log("tranform loop entered");
                    Vector3 itemLocation = transform.position + transform.forward * spaceOutDistance;
                    itemLocation. y = 0.5f;

                    Instantiate(item.prefab, itemLocation, Quaternion.identity);
                    spaceOutDistance += 2;
                    Debug.Log("item instantiated");
                //}
                
                //remove these items from the inventory
                InventoryManager.Instance.Remove(item);

            }
        }
    }

}
