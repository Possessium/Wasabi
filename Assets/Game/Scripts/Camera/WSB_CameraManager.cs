using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WSB_CameraManager : MonoBehaviour
{
    public static WSB_CameraManager I { get; private set; }


    [SerializeField] CamType currentCamType = CamType.Dynamic;
        public CamType CurrentCamType { get { return currentCamType; } }

    [SerializeField] Transform lux = null;
    [SerializeField] Transform ban = null;

    #region Cameras
    //[SerializeField] bool isOrtho = false;
        //public bool IsOrtho { get { return isOrtho; } }

    [SerializeField] WSB_Camera camLux = null;
    [SerializeField] WSB_Camera camBan = null;

    [SerializeField] float camMoveSpeed = 20;
    public float CamMoveSpeed { get { return camMoveSpeed; } }
    [SerializeField] float camZoomSpeed = 20;
    public float CamZoomSpeed { get { return camZoomSpeed; } }

    [SerializeField] float minCamZoom = 5;
    public float MinCamZoom { get { return minCamZoom; } }
    [SerializeField] float maxCamZoom = 10;
    public float MaxCamZoom { get { return maxCamZoom; } }
    #endregion

    #region Split
    [SerializeField] RenderTexture cam2RenderTexture = null;
    //[SerializeField] Material readMaterial = null;
    [SerializeField] RawImage render = null;
    [SerializeField] GameObject mask = null;
    [SerializeField] GameObject bigSplit = null;
    [SerializeField] Animator leftMiddle = null;
    [SerializeField] Animator rightMiddle = null;
    public float SplitAngle { get; private set; } = 0;
    #endregion

    [SerializeField] List<WSB_TriggerCam> lastTriggered = new List<WSB_TriggerCam>();
    [SerializeField] Vector3 targetPositionCamBan = Vector3.zero;
    [SerializeField] Vector3 targetPositionCamLux = Vector3.zero;

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
        if (WSB_GameManager.Paused)
            return;

        // Check if all the needed components are here, throw error if not
        if (!IsReady)
        {
            Debug.LogError($"ban {ban}   lux {lux}   camBan {camBan}   camLux {camLux}   cam2RenderTexture {cam2RenderTexture}  render {render}   split {mask}   bigSplit {bigSplit}   ");
            Debug.LogError("Erreur, paramètres manquant sur WSB_CameraManager");
            return;
        }

        // Switch on the type to behave properly
        switch (currentCamType)
        {
            case CamType.Dynamic:
                Dynamic();
                break;
            case CamType.SplitFixe:
                SplitFixe();
                break;
            case CamType.SplitDynamic:
                SplitDynamic();
                break;
        }
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

    public void TriggerExit(WSB_TriggerCam _trigger)
    {
        if (lastTriggered.Contains(_trigger))
            lastTriggered.Remove(_trigger);
    }

    public void TriggerEntered(WSB_TriggerCam _trigger)
    {
        // Checks if the trigger is already used or any other triggers have a player inside
        if (lastTriggered.Contains(_trigger) || lastTriggered.Find(t => t != _trigger && t.PlayersIn > 0))
            return;

        lastTriggered.Add(_trigger);

        // Change the type of the camera to the trigger given type
        switch (_trigger.Type)
        {
            case CamType.Fixe:
                SwitchCamType(CamType.Fixe, _trigger.Position, _trigger.Zoom);
                break;
            case CamType.Dynamic:
                SwitchCamType(CamType.Dynamic, _trigger.Position);
                break;
            case CamType.SplitFixe:
                SwitchCamType(CamType.SplitFixe, _trigger.Angle, _trigger.Zoom);
                break;
            case CamType.SplitDynamic:
                SwitchCamType(CamType.SplitDynamic, _trigger.Position);
                break;
        }
    }

    // Dynamic && SplitDynamic
    public void SwitchCamType(CamType _t, Vector3 _position)
    {
        // If the changing type is not the correct one, exit
        if (_t != CamType.Dynamic && _t != CamType.SplitDynamic)
            return;

        // Set the new type after checking if the distance and type are correct
        float _dist = Vector2.Distance(ban.position, lux.position);
        if (_t == CamType.SplitDynamic && _dist < MaxCamZoom)
            _t = CamType.Dynamic;
        
        currentCamType = _t;

        leftMiddle.SetTrigger("Change");
        rightMiddle.SetTrigger("Change");
        leftMiddle.SetBool("Split", _dist >= MaxCamZoom);
        rightMiddle.SetBool("Split", _dist >= MaxCamZoom);

        camBan.SetInstantCam(new Vector3(camLux.transform.position.x, camLux.transform.position.y, camLux.Cam.orthographicSize));

        // Set the given position
        targetPositionCamBan = new Vector3(targetPositionCamBan.x, targetPositionCamBan.y, camBan.Cam.orthographicSize);
        targetPositionCamLux = new Vector3(targetPositionCamLux.x, targetPositionCamLux.y, camLux.Cam.orthographicSize);

        if (currentCamType == CamType.SplitDynamic)
        {
            // Activate the split
            ToggleSplit(true);

            // Sets the correct angle of the split
            Vector3 _dir = ban.position - lux.position;
            float _angle = Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg;
            mask.transform.eulerAngles = new Vector3(0, 0, _angle - 90);
            render.transform.localEulerAngles = new Vector3(0, 0, -_angle + 90);
            SplitDynamic();
        }

        // Disable the split it the type doesn't need it
        else if (currentCamType == CamType.Dynamic)
        {
            ToggleSplit(false);
            Dynamic();
        }
    }

    // SplitFixe
    public void SwitchCamType(CamType _t, float _angle, float _zoom)
    {
        // If the changing type is not the correct one, exit
        if (_t != CamType.SplitFixe)
            return;

        // Set the nexw type
        currentCamType = _t;

        // Set the given position
        targetPositionCamBan = new Vector3(targetPositionCamBan.x, targetPositionCamBan.y, _zoom);
        targetPositionCamLux = new Vector3(targetPositionCamLux.x, targetPositionCamLux.y, _zoom);

        // Sets the correct angle of the split
        SplitAngle = _angle;

        // Activate the split
        ToggleSplit(true);

        SplitFixe();
    }

    // Fixe
    public void SwitchCamType(CamType _t, Vector3 _position, float _zoom)
    {
        // If the changing type is not the correct one, exit
        if (_t != CamType.Fixe)
            return;

        // Set the nexw type
        currentCamType = _t;

        // Set the given position
        targetPositionCamBan = new Vector3(targetPositionCamBan.x, targetPositionCamBan.y, _position.z);
        targetPositionCamLux = new Vector3(targetPositionCamLux.x, targetPositionCamLux.y, _position.z);

        // Set the camera position with correct zoom
        camLux.SetCam(new Vector3(_position.x, _position.y, -_zoom));

        // Disable the split
        ToggleSplit(false);
    }

    void Dynamic()
    {
        // Get required variables for further calculs
        Vector3 _camPos = camLux.transform.position;
        Vector3 _dir = ban.position - lux.position;
        float _dist = (Vector2.Distance(ban.position, lux.position));
        float _zoom = 0;

        // Lock the zoom to the minimum given zoom
        if (_dist < MinCamZoom)
            _zoom = MinCamZoom;

        // Split the screen if the distance is higher than the maximum given zoom
        else if (_dist > MaxCamZoom)
        {
            SwitchCamType(CamType.SplitDynamic, new Vector3(_camPos.x, _camPos.y, camLux.Cam.orthographicSize));
            return;
        }

        // Sets the zoom on the distance between lux & ban
        else 
            _zoom = _dist;

        // Set the camera position to between lux & ban and with the appropriate zoom
        camLux.SetCam( new Vector3(lux.position.x + _dir.x / 2, lux.position.y + _dir.y / 2, _zoom));
        camBan.SetCam(new Vector3(camLux.transform.position.x, camLux.transform.position.y, _zoom));
    }


    void SplitFixe()
    {
        // Reset the angle to defined angle in case it would have moved
        mask.transform.eulerAngles = new Vector3(0, 0, SplitAngle - 90);
        render.transform.localEulerAngles = new Vector3(0, 0, -SplitAngle + 90);

        Vector3 _dir = lux.position - ban.position;

        // Get the position of both cameras and offset them by the zoom troward each other
        Vector3 _luxOffset = new Vector3(
            lux.position.x - (_dir.normalized.x * MaxCamZoom),
            lux.position.y - (_dir.normalized.y * MinCamZoom),
            camLux.transform.position.z);

        Vector3 _banOffset = new Vector3(
            ban.position.x + (_dir.normalized.x * MaxCamZoom),
            ban.position.y + (_dir.normalized.y * MinCamZoom),
            camBan.transform.position.z);

        // Set the correct cameras position and zoom
        camBan.SetCam(new Vector3(_banOffset.x, _banOffset.y, targetPositionCamBan.z));
        camLux.SetCam(new Vector3(_luxOffset.x, _luxOffset.y, targetPositionCamLux.z));
    }

    void SplitDynamic()
    {
        // Get required variables for further calculs
        float _dist = (Vector2.Distance(ban.position, lux.position));
        Vector3 _dir = lux.position - ban.position;

        // Get the position of both cameras and offset them by the zoom troward each other
        Vector3 _luxOffset = new Vector3(
            lux.position.x - (_dir.normalized.x * (MaxCamZoom)),
            (lux.position.y) - (_dir.normalized.y * (MinCamZoom)),
            camLux.transform.position.z);

        Vector3 _banOffset = new Vector3(
            ban.position.x + (_dir.normalized.x * (MaxCamZoom)),
            (ban.position.y) + (_dir.normalized.y * (MinCamZoom)),
            camBan.transform.position.z);

        if(leftMiddle.GetBool("Split") != _dist >= MaxCamZoom)
        {
            leftMiddle.SetTrigger("Change");
            rightMiddle.SetTrigger("Change");
        }

        leftMiddle.SetBool("Split", _dist >= MaxCamZoom);
        rightMiddle.SetBool("Split", _dist >= MaxCamZoom);


        // Loop until the distance between lux & ban is lower than the max zoom * 1.5
        if (_dist <= MaxCamZoom * 1.5f)
        {
            // Tells the cameras to merge towards each other if the distance is lower than the max zoom
            if (_dist < MaxCamZoom)
            {
                camBan.SetCam(new Vector3(camLux.transform.position.x, camLux.transform.position.y, _dist), true);
                camLux.SetCam(new Vector3(camBan.transform.position.x, camBan.transform.position.y, _dist), false);
                return;
            }

            // Get the direction between the offset positions of the cameras
            Vector3 _dirOffset = _luxOffset - _banOffset;

            // Calcul and set ban's cam position and zoom
            targetPositionCamBan = new Vector3(
                _luxOffset.x - (_dirOffset.x * (_dist / (maxCamZoom * 1.5f))),
                _luxOffset.y - (_dirOffset.y * (_dist /(maxCamZoom * 1.5f))),
                _dist);


            camBan.SetCam(targetPositionCamBan/*, targetPositionCamBan.z*/);

            // Calcul and set ban's cam position and zoom
            targetPositionCamLux = new Vector3(
                _banOffset.x + (_dirOffset.x * (_dist / (maxCamZoom * 1.5f))),
                _banOffset.y + (_dirOffset.y * (_dist /(maxCamZoom * 1.5f))),
                _dist);

            camLux.SetCam(targetPositionCamLux/*, targetPositionCamLux.z*/);
        }

        // If the distance is higher than the max zoom * 1.5
        else
        {
            camBan.SetCam(new Vector3(_banOffset.x, _banOffset.y, Mathf.Clamp(_dist, minCamZoom, maxCamZoom * 1.5f)));
            camLux.SetCam(new Vector3(_luxOffset.x, _luxOffset.y, Mathf.Clamp(_dist, minCamZoom, maxCamZoom * 1.5f)));
        }

        // Sets the correct angle of the split
        float _angle = Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg;
        mask.transform.eulerAngles = new Vector3(0, 0, _angle - 90);
        render.transform.localEulerAngles = new Vector3(0, 0, -_angle + 90);
    }

    public void ToggleSplit(bool _status)
    {
        bigSplit.SetActive(_status);
    }
}

public enum CamType
{
    Fixe,
    Dynamic,
    SplitFixe,
    SplitDynamic,
    None
}
