using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CameraController))]
[RequireComponent(typeof(InputManger))]
public class GameManager : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    [SerializeField] float rayDistance = 1000f;
    [SerializeField] private BuildObjectData selectedObjectData;
    private BuildObject previewObject;
    private BuildObject currentObject;
    private RaycastHit currentHit = new RaycastHit();

    private void Start()
    {
        if (selectedObjectData)
        {
            SelectObject(selectedObjectData);
        }
    }

    private void Update()
    {
        currentObject = GetObjectUnderMouse();

        if (CanInstantiateObject())
        {
            previewObject.transform.position = currentHit.point;
            previewObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, currentHit.normal);

        }
    }


    public void SelectObject(BuildObjectData objData)
    {
        selectedObjectData = objData;

        if (previewObject)
        {
            Destroy(previewObject);
        }

        previewObject = Instantiate(selectedObjectData.previewPrefab).GetComponent<BuildObject>();
    }

    public void ClickStart()
    {
        if (CanInstantiateObject())
        {
            Instantiate(selectedObjectData.prefab, previewObject.transform.position, previewObject.transform.rotation);
        }
    }

    private bool CanInstantiateObject()
    {
        return previewObject && currentObject && previewObject.CanBePlacedOn(currentObject);
    }

    private BuildObject GetObjectUnderMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance, layerMask))
        {
            BuildObjectModel buildObjectModel = hit.collider.GetComponent<BuildObjectModel>();
            if (buildObjectModel)
            {
                currentHit = hit;
                return buildObjectModel.buildObject;
            }
        }

        return null;
    }
}
