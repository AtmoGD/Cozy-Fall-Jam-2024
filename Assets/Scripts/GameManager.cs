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
    private Vector3 rotationFactor = new Vector3(0, 0, 0);
    private float modelDistance = 0;
    private Vector3 initScale = Vector3.one;
    private float scale = 1;

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

            Quaternion resultingRotation = Quaternion.FromToRotation(Vector3.up, currentHit.normal) * Quaternion.Euler(rotationFactor);
            previewObject.Model.transform.rotation = resultingRotation;

            Vector3 position = previewObject.Model.transform.localPosition;
            position = currentHit.normal * modelDistance;
            previewObject.Model.transform.localPosition = position;

            previewObject.Model.transform.localScale = initScale * scale;
        }
        else
        {
            previewObject.transform.position = Vector3.one * 1000;
        }
    }


    public void SelectObject(BuildObjectData objData)
    {
        selectedObjectData = objData;

        if (previewObject != null)
        {
            Destroy(previewObject.gameObject);
        }

        if (!selectedObjectData) return;

        previewObject = Instantiate(selectedObjectData.previewPrefab).GetComponent<BuildObject>();
        previewObject.transform.position = Vector3.one * 1000;

        rotationFactor = Vector3.zero;
        modelDistance = 0;
        scale = 1;
        initScale = previewObject.Model.transform.localScale;
    }

    public void ClickStart()
    {
        if (CanInstantiateObject())
        {
            BuildObject instantietedObject = Instantiate(selectedObjectData.prefab, previewObject.transform.position, previewObject.transform.rotation).GetComponent<BuildObject>();

            instantietedObject.Model.transform.localScale = previewObject.Model.transform.localScale;
            instantietedObject.Model.transform.localPosition = previewObject.Model.transform.localPosition;
            instantietedObject.Model.transform.rotation = previewObject.Model.transform.rotation;
        }
    }

    public void RotateObjectX(int direction)
    {
        if (!selectedObjectData.CanRotateX) return;

        rotationFactor.x += direction * selectedObjectData.RotationXSteps;
        if (selectedObjectData.RotationXMax - selectedObjectData.RotationXMin >= 360)
        {
            rotationFactor.x = rotationFactor.x % 360;
        }
        rotationFactor.x = Mathf.Clamp(rotationFactor.x, selectedObjectData.RotationXMin, selectedObjectData.RotationXMax);
    }

    public void RotateObjectY(int direction)
    {
        if (!selectedObjectData.CanRotateY) return;

        rotationFactor.y += direction * selectedObjectData.RotationYSteps;
        if (selectedObjectData.RotationYMax - selectedObjectData.RotationYMin >= 360)
        {
            rotationFactor.y = rotationFactor.y % 360;
        }
        rotationFactor.y = Mathf.Clamp(rotationFactor.y, selectedObjectData.RotationYMin, selectedObjectData.RotationYMax);
    }

    public void RotateObjectZ(int direction)
    {
        if (!selectedObjectData.CanRotateZ) return;

        rotationFactor.z += direction * selectedObjectData.RotationZSteps;
        if (selectedObjectData.RotationZMax - selectedObjectData.RotationZMin >= 360)
        {
            rotationFactor.z = rotationFactor.z % 360;
        }
        rotationFactor.z = Mathf.Clamp(rotationFactor.z, selectedObjectData.RotationZMin, selectedObjectData.RotationZMax);
    }

    public void AddModelDistance(int direction)
    {
        if (!selectedObjectData.CanAddDistance) return;

        modelDistance += direction * selectedObjectData.distanceStep;
        modelDistance = Mathf.Clamp(modelDistance, selectedObjectData.MinDistance, selectedObjectData.MaxDistance);
    }

    public void ScaleObject(int direction)
    {
        if (!selectedObjectData.CanScale) return;

        scale += direction * selectedObjectData.scaleStep;
        scale = Mathf.Clamp(scale, selectedObjectData.MinScale, selectedObjectData.MaxScale);
    }


    private bool CanInstantiateObject()
    {
        // TODO: Check for collision of all objects not just the attached children
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
