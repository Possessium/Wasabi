using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WSB_CheckpointManager : MonoBehaviour
{
    public static WSB_CheckpointManager I { get; private set; }

    [SerializeField] WSB_Checkpoint checkpointLux = null;
    [SerializeField] WSB_Checkpoint checkPointBan = null;

    private void Awake()
    {
        I = this;
    }

    public void SetNewCheckpoint(WSB_Checkpoint _cp, bool _ban)
    {
        if (_ban)
            checkPointBan = _cp;

        else
            checkpointLux = _cp;
    }

    public void RespawnBan(InputAction.CallbackContext _ctx)
    {
        // Only goes through when player has hold the button enough
        if (!_ctx.performed)
            return;
        //Debug.Log("in");
        Respawn(WSB_Ban.I);
    }

    public void RespawnLux(InputAction.CallbackContext _ctx)
    {
        // Only goes through when player has hold the button enough
        if (!_ctx.performed)
            return;

        Respawn(WSB_Lux.I);
    }

    public void Respawn(WSB_Player _p)
    {
        if (_p.GetComponent<WSB_Ban>() && checkPointBan)
            _p.transform.position = checkPointBan.Position;

        else if (_p.GetComponent<WSB_Lux>() && checkpointLux)
            _p.transform.position = checkpointLux.Position;
    }
}
