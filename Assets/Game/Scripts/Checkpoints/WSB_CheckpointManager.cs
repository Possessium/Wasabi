using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WSB_CheckpointManager : MonoBehaviour
{
    public static WSB_CheckpointManager I { get; private set; }

    [SerializeField] WSB_Checkpoint checkpointLux = null;
    [SerializeField] WSB_Checkpoint checkPointBan = null;

    WSB_Ban ban = null;
    WSB_Lux lux = null;

    private void Awake()
    {
        I = this;
    }

    private void Start()
    {
        ban = WSB_Ban.I;
        lux = WSB_Lux.I;
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
        Respawn(ban.Player);
    }

    public void RespawnLux(InputAction.CallbackContext _ctx)
    {
        // Only goes through when player has hold the button enough
        if (!_ctx.performed)
            return;

        Respawn(lux.PlayerMovable);
    }

    public void Respawn(WSB_PlayerMovable _p)
    {
        if (_p.GetComponent<WSB_Ban>() && checkPointBan)
            _p.transform.position = checkPointBan.Position;

        else if (_p.GetComponent<WSB_Lux>() && checkpointLux)
            _p.transform.position = checkpointLux.Position;
    }
}
