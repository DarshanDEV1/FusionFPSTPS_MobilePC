using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterInputHandler : MonoBehaviour
{
    Vector2 moveInputVector = Vector2.zero;
    Vector2 viewInputVector = Vector2.zero;
    bool isJumpButtonPressed = false;
    bool isFireButtonPressed = false;
    bool isGrenadeFireButtonPressed = false;
    bool isRocketLauncherFireButtonPressed = false;

    //Other components
    LocalCameraHandler localCameraHandler;
    CharacterMovementHandler characterMovementHandler;

    [SerializeField] private Transform _touchArea;

    private void Awake()
    {
        localCameraHandler = GetComponentInChildren<LocalCameraHandler>();
        characterMovementHandler = GetComponent<CharacterMovementHandler>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _touchArea.GetChild(0).GetComponent<Button>().onClick.AddListener(() =>
        {
            //Jump
            isJumpButtonPressed = true;
            Debug.Log("Jump Clicked");
        });
        _touchArea.GetChild(1).GetComponent<Button>().onClick.AddListener(() =>
        {
            //Fire
            isFireButtonPressed = true;
        });
        _touchArea.GetChild(2).GetComponent<Button>().onClick.AddListener(() =>
        {
            //Rocket
            isRocketLauncherFireButtonPressed = true;
        });
        _touchArea.GetChild(3).GetComponent<Button>().onClick.AddListener(() =>
        {
            //Granade
            isGrenadeFireButtonPressed = true;
        });
        _touchArea.GetChild(4).GetComponent<Button>().onClick.AddListener(() =>
        {
            //Camera
            //Switch the variable on our "local version" of NetworkPlayer
            NetworkPlayer.Local.is3rdPersonCamera = !NetworkPlayer.Local.is3rdPersonCamera;

            //Tell the State Authority which camera mode we are using
            NetworkPlayer.Local.RPC_SetCameraMode(NetworkPlayer.Local.is3rdPersonCamera);
        });

    }


    // Update is called once per frame
    void Update()
    {
        if (!characterMovementHandler.Object.HasInputAuthority)
            return;

        if (SceneManager.GetActiveScene().name == "Ready")
            return;

        //View input
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        /*viewInputVector.x = Input.GetAxis("Mouse X");
        viewInputVector.y = Input.GetAxis("Mouse Y") * -1;*/ //Invert the mouse look
        LookAround _look = FindObjectOfType<LookAround>();
        viewInputVector.x = _look.Horizontal;
        viewInputVector.y = _look.Vertical * -1;

        //Move input
        /*moveInputVector.x = Input.GetAxis("Horizontal");
        moveInputVector.y = Input.GetAxis("Vertical");*/
        Joystick _joystick = FindObjectOfType<Joystick>();
        moveInputVector.x = _joystick.Horizontal;
        moveInputVector.y = _joystick.Vertical;

        /*//Jump
        if (Input.GetButtonDown("Jump"))
            isJumpButtonPressed = true;

        //Fire
        if (Input.GetButtonDown("Fire1"))
            isFireButtonPressed = true;

        //Fire
        if (Input.GetButtonDown("Fire2"))
            isRocketLauncherFireButtonPressed = true;

        //Throw grenade
        if (Input.GetKeyDown(KeyCode.G))
            isGrenadeFireButtonPressed = true;

        //Switch camera mode
        if (Input.GetKeyDown(KeyCode.C))
        {
            //Switch the variable on our "local version" of NetworkPlayer
            NetworkPlayer.Local.is3rdPersonCamera = !NetworkPlayer.Local.is3rdPersonCamera;

            //Tell the State Authority which camera mode we are using
            NetworkPlayer.Local.RPC_SetCameraMode(NetworkPlayer.Local.is3rdPersonCamera);
        }*/

        //Set view
        localCameraHandler.SetViewInputVector(viewInputVector);

    }

    public NetworkInputData GetNetworkInput()
    {
        NetworkInputData networkInputData = new NetworkInputData();

        //Aim data
        networkInputData.aimForwardVector = localCameraHandler.transform.forward;

        //Camera position
        networkInputData.cameraPosition = localCameraHandler.transform.position;

        //Move data
        networkInputData.movementInput = moveInputVector;

        //Jump data
        networkInputData.isJumpPressed = isJumpButtonPressed;

        //Fire data
        networkInputData.isFireButtonPressed = isFireButtonPressed;

        //Rocket data
        networkInputData.isRocketLauncherFireButtonPressed = isRocketLauncherFireButtonPressed;

        //Grenade fire data
        networkInputData.isGrenadeFireButtonPressed = isGrenadeFireButtonPressed;

        //Reset variables now that we have read their states
        isJumpButtonPressed = false;
        isFireButtonPressed = false;
        isGrenadeFireButtonPressed = false;
        isRocketLauncherFireButtonPressed = false;

        return networkInputData;
    }
}
