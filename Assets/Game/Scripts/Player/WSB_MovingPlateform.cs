using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Rigidbody2D))]
public class WSB_MovingPlateform : MonoBehaviour
{
    //[SerializeField] List<GameObject> objectsOn = new List<GameObject>();
    //[SerializeField] Vector3 lastFramePos = Vector3.zero;
    //[SerializeField] LayerMask moveLayer = 0;
    //[SerializeField] BoxCollider2D hitbox = null;
    //public Vector3 Movement { get { return transform.position - lastFramePos; } }


    //private void Start()
    //{
    //    if (!hitbox)
    //        hitbox = GetComponent<BoxCollider2D>();

    //    lastFramePos = transform.position;
        
    //    GetComponent<Rigidbody2D>().isKinematic = true;
    //    GetComponent<Rigidbody2D>().useFullKinematicContacts = true;
    //}

    //void Update()
    //{
    //    Collider2D[] _cols = Physics2D.OverlapBoxAll(transform.position, hitbox.size * transform.localScale, 0, moveLayer);
    //    List<GameObject> _gos = new List<GameObject>();

    //    foreach (Collider2D col in _cols)
    //        _gos.Add(col.gameObject);


    //    for (int i = 0; i < _cols.Length; i++)
    //    {
    //        GameObject _go = _cols[i].gameObject;

    //        if(!objectsOn.Contains(_go))
    //        {
    //            if(_go.GetComponent<WSB_Player>())
    //            {
    //                _go.GetComponent<WSB_Player>().IsOnMovingPlateform = true;
    //                objectsOn.Add(_go.gameObject);
    //            }
    //            else if (_go.GetComponent<WSB_Movable>())
    //            {
    //                _go.GetComponent<WSB_Movable>().IsOnMovingPlateform = true;
    //                objectsOn.Add(_go.gameObject);
    //            }
    //        }
    //    }

    //    for (int i = 0; i < objectsOn.Count; i++)
    //    {
    //        if (!_gos.Contains(objectsOn[i]))
    //            StartCoroutine(DelayExit(objectsOn[i]));
    //    }


    //    for (int i = 0; i < objectsOn.Count; i++)
    //    {
    //        if (objectsOn[i].GetComponent<WSB_Player>())
    //            objectsOn[i].GetComponent<WSB_Player>().AddInstantForce(Movement);
    //        else if (objectsOn[i].GetComponent<WSB_Movable>())
    //            objectsOn[i].GetComponent<WSB_Movable>().AddInstantForce(Movement);
    //    }

    //    lastFramePos = transform.position;
    //}


    //IEnumerator DelayExit(GameObject _go)
    //{
    //    yield return new WaitForEndOfFrame();

    //    RaycastHit2D[] _hits = new RaycastHit2D[10];

    //    hitbox.Cast(Vector2.zero, _hits);

    //    List<GameObject> _gos = new List<GameObject>();

    //    for (int i = 0; i < _hits.Length; i++)
    //    {
    //        if (_hits[i])
    //            _gos.Add(_hits[i].transform.gameObject);
    //    }

    //    if(!_gos.Contains(_go) && objectsOn.Contains(_go))
    //    {
    //        if (_go.transform.GetComponent<WSB_Player>())
    //            _go.transform.GetComponent<WSB_Player>().IsOnMovingPlateform = false;
    //        else if (_go.transform.GetComponent<WSB_Movable>())
    //            _go.transform.GetComponent<WSB_Movable>().IsOnMovingPlateform = false;

    //        objectsOn.Remove(_go);
    //    }
    //}

}
