using System.Collections;
using UnityEngine;

public class WSB_Camera : MonoBehaviour
{
    [SerializeField] Camera cam = null;
    public Camera Cam { get { return cam; } }

    private void Awake()
    {
        // Check if all the needed components are here, throw error and destroy itself if not
        if (!cam) cam = GetComponent<Camera>();
        if (!cam)
        {
            Debug.LogError($"Pas de component Camera trouvé sur {transform.name}");
            Destroy(this);
        }
    }
    [SerializeField] float coef = 2;
    public void SetCam(Vector3 _pos, System.Action _callBack = null)
    {
        // Call the callback if the position is already set to the given position
        if (transform.position == _pos && Cam.orthographicSize == _pos.z)
        {
            if (_callBack != null)
                _callBack.Invoke();
            return;
        }

        float _d = Vector2.Distance(_pos, transform.position);
        float _coef = 1;
        if (_d < 2)
            _coef = coef;

        transform.position = new Vector3(
               Mathf.Lerp(transform.position.x, _pos.x, Time.deltaTime * WSB_CameraManager.I.CamMoveSpeed * _coef),
               Mathf.Lerp(transform.position.y, _pos.y, Time.deltaTime * WSB_CameraManager.I.CamMoveSpeed * _coef),
               transform.position.z);
        Cam.orthographicSize = Mathf.MoveTowards(Cam.orthographicSize, _pos.z, Time.deltaTime * (WSB_CameraManager.I.CamZoomSpeed));

        if(_callBack != null && _d < .01f && Mathf.Abs(Cam.orthographicSize - _pos.z) < .01f)
        {
            _callBack.Invoke();
        }
    }

    public void SetInstantCam(Vector3 _pos)
    {
        transform.position = new Vector3(_pos.x, _pos.y, transform.position.z);
        cam.orthographicSize = _pos.z;
    }
}
