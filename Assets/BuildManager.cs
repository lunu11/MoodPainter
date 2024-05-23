using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    [SerializeField] private AquariumManager _aquariumManager;
    [SerializeField] private GameObject _moveButton;
    private Vector3 offset;
    private Vector3 dragStartPosition;
    private Vector3 dragDelta;
    [SerializeField] private GameObject _sand;

    public GameObject _selectedDecor;
    public bool IsEligible;
    [SerializeField] private GameObject _tools;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LayerMask mask = LayerMask.GetMask("Decor");

            Vector3 mousePos = Input.mousePosition;
            Vector3 point = Camera.main.ScreenToWorldPoint(mousePos);
            point.z = 0;

            Collider2D hit = Physics2D.OverlapPoint(point, mask);
            if (hit != null)
            {
                _selectedDecor = hit.gameObject;
                _moveButton.transform.position = _selectedDecor.transform.position;
                _moveButton.SetActive(true);
            }
        }
        if (Input.GetMouseButtonDown(0) && _selectedDecor != null)
        {
            dragStartPosition = Input.mousePosition;
            offset = _selectedDecor.transform.position - Camera.main.ScreenToWorldPoint(dragStartPosition);
        }
        else if (Input.GetMouseButton(0) && _selectedDecor != null)
        {
            LayerMask mask = LayerMask.GetMask("Selection");

            Vector3 mousePos = Input.mousePosition;
            Vector3 point = Camera.main.ScreenToWorldPoint(mousePos);
            point.z = 0;

            Collider2D hit = Physics2D.OverlapPoint(point, mask);
            if (hit != null)
            {
                Vector3 currentMousePosition = Input.mousePosition;
                Vector3 newPosition = Camera.main.ScreenToWorldPoint(currentMousePosition) + offset;

                newPosition.z = 0;
                // Move object B with the drag delta
                _selectedDecor.transform.position = newPosition;
                _moveButton.transform.position = newPosition;
                if (!IsObjectWithinArea())
                {
                    _selectedDecor.GetComponent<SpriteRenderer>().color = Color.red;
                    IsEligible = false;
                }
                else
                {
                    _selectedDecor.GetComponent<SpriteRenderer>().color = Color.green;
                    IsEligible = true;

                }
            }
        }        
    }

    public void SaveExit()
    {
        if (IsEligible)
        {
            _moveButton.SetActive(false);
            _tools.SetActive(true);
            gameObject.SetActive(false);
            _aquariumManager.SavePositions();
        }
    }
    public void Exit()
    {
        _moveButton.SetActive(false);
        _tools.SetActive(true);
        gameObject.SetActive(false);
        _aquariumManager.RevertPositions();
    }

    private bool IsObjectWithinArea()
    {
        LayerMask mask = LayerMask.GetMask("Sand");
        Vector3 point = _selectedDecor.transform.position;
        Collider2D hit = Physics2D.OverlapPoint(point, mask);
        if (hit != null)
        {
            return true; 
        } else
        {
            return false;
        }
    }
}
