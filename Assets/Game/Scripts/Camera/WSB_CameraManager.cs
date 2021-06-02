using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WSB_CameraManager : MonoBehaviour
{
    public static WSB_CameraManager I { get; private set; }


    [SerializeField] Transform lux = null;
    [SerializeField] Transform ban = null;

    #region Cameras
    [SerializeField] WSB_Camera camLux = null;
    public WSB_Camera CamLux { get { return camLux; } }
    [SerializeField] WSB_Camera camBan = null;

    [SerializeField] float camMoveSpeed = 20;
    public float CamMoveSpeed { get { return camMoveSpeed; } }
    [SerializeField] float camZoomSpeed = 20;
    public float CamZoomSpeed { get { return camZoomSpeed; } }

    [SerializeField] float minCamZoom = 5;
    public float MinCamZoom { get { return minCamZoom; } }
    [SerializeField] float maxCamZoom = 10;
    public float MaxCamZoom { get { return maxCamZoom; } }

    [SerializeField] private Vector3 offset = Vector3.zero;
    [SerializeField] private Vector3 calculatedOffset = Vector3.zero;
    #endregion

    #region Split
    [SerializeField] RenderTexture cam2RenderTexture = null;
    //[SerializeField] Material readMaterial = null;
    [SerializeField] RawImage render = null;
    [SerializeField] GameObject mask = null;
    [SerializeField] GameObject bigSplit = null;
    [SerializeField] Animator leftMiddle = null;
    [SerializeField] Animator rightMiddle = null;
    [SerializeField] GameObject splitMiddleParent = null;
    public float SplitAngle { get; private set; } = 0;
    #endregion

    [SerializeField] Vector3 targetPositionCamBan = Vector3.zero;
    [SerializeField] Vector3 targetPositionCamLux = Vector3.zero;

    public bool IsSplit { get; private set; } = false;
    public bool IsActive = true;

    public bool IsReady => ban && lux && camBan && camLux && cam2RenderTexture && render && mask && bigSplit;



    private void Awake()
    {
        // Populate the instance of the manager
        I = this;

        // Check that only one instance is in the scene
        if(FindObjectsOfType<WSB_CameraManager>().Length > 1)
        {
            Debug.LogError("Plusieurs component WSB_CameraManager sont présents dans la scène. Il ne doit y en avoir qu'un.");
            Destroy(this);
        }
    }

    private void Start()
    {
        // Check if all the needed components are here, throw error and destroy itself if not
        if(!IsReady)
        {
            Debug.LogError($"ban {ban}   lux {lux}   camBan {camBan}   camLux {camLux}   cam2RenderTexture {cam2RenderTexture}  render {render}   split {mask}   bigSplit {bigSplit}   ");
            Debug.LogError("Erreur, paramètres manquant sur WSB_CameraManager");
            Destroy(this);
        }

        // Initiliaze target position of ban & lux cam's
        Vector3 _camPos = camBan.transform.position;
        targetPositionCamBan = new Vector3(_camPos.x, _camPos.y, camBan.Cam.orthographicSize);

        _camPos = camLux.transform.position;
        targetPositionCamLux = new Vector3(_camPos.x, _camPos.y, camLux.Cam.orthographicSize);

        SetResolution();
    }


    public void LateUpdate()
    {
        // Hold if the game is paused
        if (WSB_GameManager.Paused || !IsActive)
            return;

        // Check if all the needed components are here, throw error if not
        if (!IsReady)
        {
            Debug.LogError($"ban {ban}   lux {lux}   camBan {camBan}   camLux {camLux}   cam2RenderTexture {cam2RenderTexture}  render {render}   split {mask}   bigSplit {bigSplit}   ");
            Debug.LogError("Erreur, paramètres manquant sur WSB_CameraManager");
            return;
        }

        splitMiddleParent.transform.rotation = mask.transform.rotation;

        Vector3 _dir = Vector3.zero;
        float _angle = 0;

        CalculateOffset();

        // Switch on the type to behave properly
        if (!IsSplit)
        {
            // Sets the correct angle of the split
            _dir = ban.position - lux.position;
            _angle = Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg;
            mask.transform.eulerAngles = new Vector3(0, 0, _angle - 90);
            render.transform.localEulerAngles = new Vector3(0, 0, -_angle + 90);
            ToggleSplit(false);

            Dynamic();
        }
        else
        {
            ToggleSplit(true);

            SplitDynamic();
        }


        if (Vector2.Distance(ban.position, lux.position) > MaxCamZoom / 1.25f && !leftMiddle.GetBool("Split") && !rightMiddle.GetBool("Split"))
        {
            leftMiddle.SetTrigger("Change");
            rightMiddle.SetTrigger("Change");
            leftMiddle.SetBool("Split", true);
            rightMiddle.SetBool("Split", true);
        }
        if (Vector2.Distance(ban.position, lux.position) <= MaxCamZoom / 1.25f && leftMiddle.GetBool("Split") && rightMiddle.GetBool("Split"))
        {
            leftMiddle.SetTrigger("Change");
            rightMiddle.SetTrigger("Change");
            leftMiddle.SetBool("Split", false);
            rightMiddle.SetBool("Split", false);
        }

    }

    private void CalculateOffset()
    {
        Vector3 _dir = ban.position - lux.position;
        float _angle = Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg;
        _angle = Mathf.Abs(_angle);

        if (_angle > 90)
            _angle -= (_angle - 90) * 2;

        float _normalizedAngle = _angle / 90;

        _normalizedAngle -= (_normalizedAngle * 2 - 1);

        calculatedOffset = offset * _normalizedAngle;
    }

    public void SetResolution()
    {
        // Get the current resolution of the screen
        int _width = Screen.width;
        int _height = Screen.height;

        // If the resolution has chanched
        if (cam2RenderTexture.width != _width || cam2RenderTexture.height != _height)
        {
            // Reset the render texture
            cam2RenderTexture.Release();

            // Setup a new render texture with the correct parameters and the new resolution
            cam2RenderTexture.Create();
            cam2RenderTexture = new RenderTexture(_width, _height, 24);
            camBan.Cam.targetTexture = cam2RenderTexture;
            render.texture = cam2RenderTexture;
        }

        // Was supposed to remove the flicker of the screen when the split begins but kinda work, kinda don't
        ToggleSplit(!bigSplit.activeSelf);
        ToggleSplit(!bigSplit.activeSelf);

        // Get the direction between lux & ban
        Vector3 _dir = ban.position - lux.position;

        // Set the cameras between lux & ban
        camBan.SetInstantCam(new Vector3(lux.position.x + _dir.x / 2, lux.position.y + _dir.y / 2, Vector2.Distance(ban.position, lux.position)));
        camLux.SetInstantCam(new Vector3(lux.position.x + _dir.x / 2, lux.position.y + _dir.y / 2, Vector2.Distance(ban.position, lux.position)));
    }

    // Dynamic && SplitDynamic
    public void SwitchCamType()
    {
        // Set the new type after checking if the distance and type are correct
        float _dist = Vector2.Distance(ban.position, lux.position);

        camBan.SetInstantCam(new Vector3(camLux.transform.position.x, camLux.transform.position.y, camLux.Cam.orthographicSize));

        // Set the given position
        targetPositionCamBan = new Vector3(targetPositionCamBan.x, targetPositionCamBan.y, camBan.Cam.orthographicSize);
        targetPositionCamLux = new Vector3(targetPositionCamLux.x, targetPositionCamLux.y, camLux.Cam.orthographicSize);

        // Disable the split it the type doesn't need it
        if (_dist < MaxCamZoom)
        {
            ToggleSplit(false);

            Dynamic();
        }

        else
        {
            // Activate the split
            ToggleSplit(true);

            SplitDynamic();
        }

    }


    void Dynamic()
    {
        //Debug.LogError("Dyna");
        
        Vector3 _camPos = camLux.transform.position;


        // Split the screen if the distance is higher than the maximum given zoom
        if (Vector2.Distance(ban.position, lux.position) > MaxCamZoom)
        {
            SwitchCamType();
            return;
        }

        _camPos = GetDynamicMiddlePosition();

        // Set the camera position to between lux & ban and with the appropriate zoom
        camLux.SetCam(_camPos + calculatedOffset);
        camBan.SetCam(_camPos + calculatedOffset);
    }


    Vector3 GetDynamicMiddlePosition()
    {
        // Get required variables for further calculs
        Vector3 _dir = ban.position - lux.position;
        float _dist = (Vector2.Distance(ban.position, lux.position));
        float _zoom = 0;

        // Lock the zoom to the minimum given zoom
        if (_dist < MinCamZoom)
            _zoom = MinCamZoom;

        // Sets the zoom on the distance between lux & ban
        else
            _zoom = _dist;

        return new Vector3(lux.position.x + _dir.x / 2, lux.position.y + _dir.y / 2, _zoom);
    }

    void SplitDynamic()
    {
        //Debug.LogError("Split");
        // Get required variables for further calculs
        float _dist = (Vector2.Distance(ban.position, lux.position));
        Vector3 _dir = lux.position - ban.position;

        // Get the position of both cameras and offset them by the zoom troward each other
        Vector3 _luxOffset = new Vector3(
            lux.position.x - (_dir.normalized.x * (MaxCamZoom)),
            (lux.position.y) - (_dir.normalized.y * (MaxCamZoom / 2)),
            camLux.transform.position.z);

        Vector3 _banOffset = new Vector3(
            ban.position.x + (_dir.normalized.x * (MaxCamZoom)),
            (ban.position.y) + (_dir.normalized.y * (MaxCamZoom / 2)),
            camBan.transform.position.z);

        // Sets the correct angle of the split
        float _angle = Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg;
        mask.transform.eulerAngles = new Vector3(0, 0, _angle - 90);
        render.transform.localEulerAngles = new Vector3(0, 0, -_angle + 90);

        // Loop until the distance between lux & ban is lower than the max zoom * 1.5
        if (_dist <= MaxCamZoom * 1.1f)
        {
            // Tells the cameras to merge towards each other if the distance is lower than the max zoom
            if (_dist < MaxCamZoom)
            {
                camBan.SetCam(GetDynamicMiddlePosition() + calculatedOffset, SwitchCamType);
                camLux.SetCam(GetDynamicMiddlePosition() + calculatedOffset);
                return;
            }

            // Get the direction between the offset positions of the cameras
            Vector3 _dirOffset = _luxOffset - _banOffset;

            // Calcul and set ban's cam position and zoom
            targetPositionCamBan = new Vector3(
                _luxOffset.x - (_dirOffset.x * (_dist / (maxCamZoom * 1.1f))),
                _luxOffset.y - (_dirOffset.y * (_dist /(maxCamZoom * 1.1f))),
                _dist);


            camBan.SetCam(targetPositionCamBan + calculatedOffset/*, targetPositionCamBan.z*/);

            // Calcul and set ban's cam position and zoom
            targetPositionCamLux = new Vector3(
                _banOffset.x + (_dirOffset.x * (_dist / (maxCamZoom * 1.1f))),
                _banOffset.y + (_dirOffset.y * (_dist /(maxCamZoom * 1.1f))),
                _dist);

            camLux.SetCam(targetPositionCamLux + calculatedOffset/*, targetPositionCamLux.z*/);
        }

        // If the distance is higher than the max zoom * 1.5
        else
        {
            camBan.SetCam(new Vector3(_banOffset.x, _banOffset.y, Mathf.Clamp(_dist, MaxCamZoom / 2, maxCamZoom * 1.1f)) + calculatedOffset);
            camLux.SetCam(new Vector3(_luxOffset.x, _luxOffset.y, Mathf.Clamp(_dist, MaxCamZoom / 2, maxCamZoom * 1.1f)) + calculatedOffset);
        }

    }

    public void ToggleSplit(bool _status)
    {
        IsSplit = _status;

        bigSplit.SetActive(_status);

        camBan.transform.gameObject.SetActive(_status);
    }

    public void ChangeZoom(float _z)
    {
        minCamZoom = _z;
        maxCamZoom = _z;
    }
}
