using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

// Monobehaviour must implement this interface
// or replace "characterController.GetDirection()" 
public interface IFootprintsCharacter
{
    Vector3 GetDirection();
    
}
public class FootprintsSprite2D : MonoBehaviour
{
    public float FootPrintSpacing = 2f;
    public int MaxNumberOfFootprints = 20;
    private Vector3 _lastPosition = Vector3.zero;
    public GameObject FootstepPrefab;
    public float FootOffset;
    public bool UseTwoFootsteps = true;
    public GameObject SecondFootstepPrefab;
    private bool _secondfoot = false;
    public float SecondFootOffset;

    private Transform _transform;
    public List<GameObject> _footsteps;
    public List<Vector3> _positions;
    [SerializeField]
    public GameObject ObjectWithIFootprintsCharacter;
    private IFootprintsCharacter _charController;

    void Start()
    {
        if (!UseTwoFootsteps)
        {
            SecondFootstepPrefab = FootstepPrefab;
        }
        _transform = transform;
        _lastPosition = _transform.position;
        _positions = new List<Vector3>();
        _footsteps = new List<GameObject>();
        _charController = ObjectWithIFootprintsCharacter.GetComponent<IFootprintsCharacter>();
        AddFootstep(_transform.position);
    }
    void Update()
    {
        float distanceFromLastFootprint = (_lastPosition - _transform.position).sqrMagnitude;
        if (distanceFromLastFootprint > FootPrintSpacing * FootPrintSpacing)
        {
            AddFootstep(_transform.position);
            _lastPosition = _transform.position;
        }
    }

    void AddFootstep(Vector3 position)
    {
        GameObject footstep = Instantiate(_secondfoot?SecondFootstepPrefab: FootstepPrefab);
        Vector3 direction = _charController.GetDirection().normalized;
        Vector3 perpendicular = new Vector3(direction.y, -direction.x).normalized;
        if (UseTwoFootsteps && _secondfoot)
        {
            position += perpendicular * SecondFootOffset;
        }
        else
        {
            position += perpendicular * FootOffset;
        }
        footstep.transform.position = position;
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Debug.Log("Angle: " + angle);
        //footstep.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        footstep.transform.eulerAngles = new Vector3(0, 0, angle);
        if (_positions.Count > MaxNumberOfFootprints)
        {
            _positions.RemoveAt(0);
            Destroy(_footsteps[0]);
            _footsteps.RemoveAt(0);
        }

        _positions.Add(position);
        _footsteps.Add(footstep);

        _secondfoot = !_secondfoot;
    }
}


