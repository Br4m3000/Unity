using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaycastManager : MonoBehaviour
{
    private GameObject raycastedObj;

    [Header("Raycast Settings")]
    [SerializeField] private float rayLength = 10;
    [SerializeField] private LayerMask newLayerMask;

    [Header("References")]
    [SerializeField] private Image crosshair;
    [SerializeField] private Text itemNameText;
    [SerializeField] private PlayerVitals playerVitals;

    void Update()
    {
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        
        if(Physics.Raycast(transform.position, fwd, out hit, rayLength, newLayerMask.value))
        {
            if (hit.collider.CompareTag("Consumable"))
            {
                CrosshairActive();
                raycastedObj = hit.collider.gameObject;
                ItemProperties properties = raycastedObj.GetComponent<ItemProperties>();
                itemNameText.text = properties.itemName;

                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("Interacting");
                    properties.Interaction(playerVitals);
                    //raycastedObj.SetActive(false);
                }
            }
        }

        else
        {
            CrosshairNormal();
            itemNameText.text = null;
        }
    }

    void CrosshairActive()
    {
        crosshair.color = Color.green;
    }

    void CrosshairNormal()
    {
        crosshair.color = Color.white;
    }
}
