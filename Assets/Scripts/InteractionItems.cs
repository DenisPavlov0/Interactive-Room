using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionItems : MonoBehaviour
{
    private Door _door;
    private InteractableItem _interactableItem;
    private InteractableItem _lastInteractableItem;
    private InteractableItem _lastPickedUpItem;
    private GameObject _inventoryHolder;

    private void Awake()
    {
        _inventoryHolder = GameObject.Find("InventoryHolder");
    }

    private void Update()
    {
        Identify();
        Actions();
    }

    private void Identify()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, 2f))
        {
            DoorsOpening(ray,hitInfo);
            IlluminationItem(ray,hitInfo);
            
        }
    }

    private void DoorsOpening(Ray ray, RaycastHit hitInfo)
    {
        if (hitInfo.collider.name == "door")
        {
            _door = hitInfo.collider.GetComponent<Door>();
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (_door != null)
                {
                    _door.SwitchDoorState();
                }
            }
        }
    }

    private void IlluminationItem(Ray ray, RaycastHit hitInfo)
    {
        if (hitInfo.collider.tag == "InteractableItem")
        {
            _interactableItem = hitInfo.collider.GetComponent<InteractableItem>();
            if (_interactableItem != null)
            {
                _interactableItem.SetFocus();
            }
        }
        else
        {
            if (_interactableItem != null)
            {
                _interactableItem.RemoveFocus();
            }
        }
    }
    
    private void TryPickUpItem()
    {
        var interactableItem = _interactableItem;
        
        if (interactableItem != _lastPickedUpItem)
        {
            if (_lastPickedUpItem != null)
            {
                _lastPickedUpItem.Drop();
            }
            
            if (_interactableItem != null)
            {
                _interactableItem.PickUp(_inventoryHolder.transform);
            }

            _lastPickedUpItem = _interactableItem;
        }
    }

    private void Actions()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _lastPickedUpItem != null)
        {
            _lastPickedUpItem.ThrowAway(transform.forward);
            _lastPickedUpItem = null;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryPickUpItem();
        }
    }
    

    

}
