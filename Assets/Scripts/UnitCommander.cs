using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCommander : MonoBehaviour
{
    public GameObject selectionMarkerPrefab;
    public LayerMask layerMask;

    // components
    private UnitSelection unitSelection;
    private Camera cam;

    void Awake()
    {
        // get the components
        unitSelection = GetComponent<UnitSelection>();
        cam = Camera.main;
    }

    void Update()
    {
        // did we press down our right mouse button and do we have units selected?
        if (Input.GetMouseButtonDown(1) && unitSelection.HasUnitsSelected())
        {
            // shoot a raycast from our mouse, to see what we hit
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // cache the selected units in an array
            Unit[] selectedUnits = unitSelection.GetSelectedUnits();

            // shoot the raycast
            if (Physics.Raycast(ray, out hit, 100, layerMask))
            {
                // are we clicking on the ground?
                if (hit.collider.CompareTag("Ground"))
                {
                    UnitsMoveToPosition(hit.point, selectedUnits);
                }
            }
        }
    }

    void UnitsMoveToPosition(Vector3 movePos, Unit[] units)
    {
        for (int x = 0; x < units.Length; x++)
        {
            units[x].MoveToPosition(movePos);
        }
    }
}
