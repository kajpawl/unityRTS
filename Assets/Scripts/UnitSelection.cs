using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelection : MonoBehaviour
{
    public RectTransform selectionBox;
    public LayerMask unitLayerMask;

    private List<Unit> selectedUnits = new List<Unit>();
    private Vector2 startPos;

    // components
    private Camera cam;
    private Player player;

    void Awake()
    {
        // get the componenets
        cam = Camera.main;
        player = GetComponent<Player>();
    }

    void Update()
    {
        // mouse down
        if (Input.GetMouseButtonDown(0))
        {
            ToggleSelectionVisual(false);
            selectedUnits = new List<Unit>();

            TrySelect(Input.mousePosition);
            startPos = Input.mousePosition;

        }

        // mouse up
        if (Input.GetMouseButtonUp(0))
        {

        }

        // mouse held down
        if (Input.GetMouseButton(0))
        {
            UpdateSelectionBox(Input.mousePosition);
        }
    }

    // called when we click on a unit
    void TrySelect(Vector2 screenPos)
    {
        Ray ray = cam.ScreenPointToRay(screenPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, unitLayerMask))
        {
            Unit unit = hit.collider.GetComponent<Unit>();

            if (player.IsMyUnit(unit))
            {
                selectedUnits.Add(unit);
                unit.ToggleSelectionVisual(true);
            }
        }
    }

    // called when we are creating a selection box
    void UpdateSelectionBox(Vector2 curMousePos)
    {
        if (!selectionBox.gameObject.activeInHierarchy)
            selectionBox.gameObject.SetActive(true);

        float width = curMousePos.x - startPos.x;
        float height = curMousePos.y - startPos.y;

        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
        selectionBox.anchoredPosition = startPos + new Vector2(width / 2, height / 2);
    }

    // toggles the selected units selection visual
    void ToggleSelectionVisual(bool selected)
    {
        foreach(Unit unit in selectedUnits)
        {
            unit.ToggleSelectionVisual(selected);
        }
    }
}
