using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum WorkMode
{
    Build,
    Collect
}

[Serializable]
public class InventorySlot
{
    public BuildObjectData data;
    public int count;
}


[RequireComponent(typeof(CameraController))]
[RequireComponent(typeof(InputManger))]
[RequireComponent(typeof(UIController))]
public class GameManager : MonoBehaviour
{
    [SerializeField] WorkMode workMode = WorkMode.Build;
    [SerializeField] LayerMask objectLayer;
    [SerializeField] LayerMask mushroomLayer;
    [SerializeField] OtherVolumeController otherController;
    [SerializeField] float rayDistance = 1000f;
    [SerializeField] private MusicManager musicManager;
    [SerializeField] private UIController ui;
    [SerializeField] private Transform objectParent;
    [SerializeField] private List<AudioSource> placeSound;
    [SerializeField] private List<AudioSource> collectSound;
    [SerializeField] private List<AudioSource> switchModeToBuildSound;
    [SerializeField] private List<AudioSource> switchModeToCollectSound;
    [SerializeField] private List<AudioSource> confirmSound;
    [SerializeField] private List<AudioSource> selectSound;
    [SerializeField] private float outlineThickness = 0.05f;
    [SerializeField] private float distanceMultiplier = 0.1f;
    [SerializeField] private Color previewColor = new Color(1, 1, 1, 0.5f);
    [SerializeField] private Color previewColorInvalid = new Color(1, 0, 0, 0.5f);
    [SerializeField] private BuildObjectData selectedObjectData;
    [SerializeField] private List<BuildObject> buildObjects = new List<BuildObject>();
    [SerializeField] private List<InventorySlot> inventory = new List<InventorySlot>();
    [SerializeField] private bool infiniteInventory = false;
    public bool InfiniteInventory => infiniteInventory;
    [SerializeField] private bool canOverlap = false;
    public bool CanOverlap => canOverlap;
    [SerializeField] private bool canPlaceAnywhere = false;
    public bool CanPlaceAnywhere => canPlaceAnywhere;
    public List<InventorySlot> Inventory => inventory;
    [SerializeField] private bool isDay = true;
    public bool IsDay => isDay;
    [SerializeField] private bool isRain = false;
    public bool IsRain => isRain;
    private BuildObject previewObject;
    private BuildObject currentObject;
    private RaycastHit currentHit = new RaycastHit();
    private Vector3 rotationFactor = new Vector3(0, 0, 0);
    private float modelDistance = 0;
    private Vector3 initScale = Vector3.one;
    private float scale = 1;
    private BuildObject lastHighlightedObject;
    private bool isPaused = false;

    private void Start()
    {
        musicManager.StartCrossfade("Pause");

        if (!selectedObjectData || (Inventory.Find(x => x.data == selectedObjectData).count <= 0 && !infiniteInventory)) return;

        SelectObject(selectedObjectData);
    }

    private void Update()
    {
        currentObject = GetObjectUnderMouse();

        if (!currentObject)
        {
            HighlightObject(null);
            HidePreview();
        }

        if (workMode == WorkMode.Build)
        {
            if (currentObject)
            {
                UpdatePreviewObjectPosition();
            }

            if (CanInstantiateObject())
            {
                SetPreview(true);

                HighlightObject(currentObject);
            }
            else
            {
                SetPreview(false);
            }
        }
        else if (workMode == WorkMode.Collect)
        {
            HidePreview();

            if (CanCollectObject())
            {
                HighlightObject(currentObject);
            }
        }
    }


    public void SetIsDay(bool value)
    {
        isDay = value;
        UpdateEnvironment();

        if (confirmSound != null)
            confirmSound[UnityEngine.Random.Range(0, confirmSound.Count)].Play();
    }

    public void SetIsRain(bool value)
    {
        isRain = value;

        UpdateEnvironment();

        if (confirmSound != null)
            confirmSound[UnityEngine.Random.Range(0, confirmSound.Count)].Play();
    }

    public void UpdateEnvironment()
    {
        string environment = (isDay ? "Day" : "Night") + (isRain ? "Rain" : "");
        environment = isPaused ? "Pause" : environment;
        musicManager.StartCrossfade(environment);
    }

    public void PlayConfirmSound()
    {
        if (confirmSound != null)
            confirmSound[UnityEngine.Random.Range(0, confirmSound.Count)].Play();
    }

    public void PlaySelectSound()
    {
        if (selectSound != null)
            selectSound[UnityEngine.Random.Range(0, selectSound.Count)].Play();
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    public void ClickStart()
    {
        currentObject = GetObjectUnderMouse();

        if (workMode == WorkMode.Collect && DoRaycastForMushrooms())
        {
            otherController.SetEffectActive(true);
        }

        if (workMode == WorkMode.Build)
        {
            if (CanInstantiateObject())
            {
                InstantiateSelectedObject();

                SelectObject(selectedObjectData);
            }
        }
        else if (workMode == WorkMode.Collect)
        {
            if (CanCollectObject())
            {
                CollectObject();
            }
        }
    }

    public bool DoRaycastForMushrooms()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rayDistance, mushroomLayer))
        {
            return true;
        }
        return false;
    }

    public void StartBaseGame()
    {
        canOverlap = false;
        infiniteInventory = false;
        canPlaceAnywhere = false;
        isPaused = false;

        ui.SetMenuActive(false);

        UpdateEnvironment();
    }

    public void StartUnlimitedGame()
    {
        canOverlap = false;
        infiniteInventory = true;
        canPlaceAnywhere = false;
        isPaused = false;

        ui.SetMenuActive(false);

        UpdateEnvironment();
    }

    public void StartCreativeGame()
    {
        canOverlap = true;
        infiniteInventory = true;
        canPlaceAnywhere = true;
        isPaused = false;

        ui.SetMenuActive(false);

        UpdateEnvironment();
    }

    public void SetGamePaused(bool paused)
    {
        isPaused = paused;

        UpdateEnvironment();
    }

    private void CollectObject()
    {
        buildObjects.Remove(currentObject);
        if (currentObject.ParentObject != null)
            currentObject.ParentObject.ChildObjects.Remove(currentObject);
        Destroy(currentObject.gameObject);
        inventory.Find(item => item.data == currentObject.Data).count++;

        if (collectSound != null)
            collectSound[UnityEngine.Random.Range(0, collectSound.Count)].Play();
    }

    private void InstantiateSelectedObject()
    {
        int variantIndex = selectedObjectData.GetPrefabs().Count > 1 ? UnityEngine.Random.Range(0, selectedObjectData.GetPrefabs().Count) : 0;
        BuildObject instantietedObject = Instantiate(selectedObjectData.GetPrefabs()[variantIndex], previewObject.transform.position, previewObject.transform.rotation, objectParent).GetComponent<BuildObject>();

        instantietedObject.Model.transform.localScale = previewObject.Model.transform.localScale;
        instantietedObject.Model.transform.localPosition = previewObject.Model.transform.localPosition;
        instantietedObject.Model.transform.rotation = previewObject.Model.transform.rotation;
        instantietedObject.variantIndex = variantIndex;

        buildObjects.Add(instantietedObject);
        instantietedObject.SetParentObject(currentObject);
        currentObject.ChildObjects.Add(instantietedObject);

        if (placeSound != null)
            placeSound[UnityEngine.Random.Range(0, placeSound.Count)].Play();

        inventory.Find(item => item.data == selectedObjectData).count--;
        SelectObject(selectedObjectData);
    }

    public void SwitchPrefabVariant()
    {
        if (!currentObject) return;

        int newVariantIndex = (currentObject.variantIndex + 1) % currentObject.Data.GetPrefabs().Count;

        currentObject.variantIndex = (currentObject.variantIndex + 1) % currentObject.Data.GetPrefabs().Count;
        GameObject newPrefab = currentObject.Data.GetPrefabs()[newVariantIndex];

        GameObject newObj = Instantiate(newPrefab, currentObject.transform.position, currentObject.transform.rotation, objectParent);
        BuildObject newVariant = newObj.GetComponent<BuildObject>();

        newVariant.Model.transform.localScale = currentObject.Model.transform.localScale;
        newVariant.Model.transform.localPosition = currentObject.Model.transform.localPosition;
        newVariant.Model.transform.rotation = currentObject.Model.transform.rotation;
        newVariant.isBase = currentObject.isBase;
        newVariant.variantIndex = newVariantIndex;

        buildObjects.Add(newVariant);
        buildObjects.Remove(currentObject);

        if (currentObject.ParentObject != null)
        {
            newVariant.SetParentObject(currentObject.ParentObject);
            currentObject.ParentObject.ChildObjects.Add(newVariant);
            currentObject.ParentObject.ChildObjects.Remove(currentObject);
        }

        if (placeSound != null)
            placeSound[UnityEngine.Random.Range(0, placeSound.Count)].Play();

        Destroy(currentObject.gameObject);
    }

    private void HighlightObject(BuildObject obj)
    {
        float distance = 1;
        if (obj != null)
        {
            distance = Vector3.Distance(obj.transform.position, Camera.main.transform.position) * distanceMultiplier;
        }

        if (lastHighlightedObject != obj)
        {
            if (lastHighlightedObject != null)
            {
                lastHighlightedObject.OutlineController.SetOutlineThickness(0);
            }

            lastHighlightedObject = obj;
        }

        foreach (BuildObject buildObject in buildObjects)
        {
            if (buildObject == obj)
            {
                buildObject.OutlineController.SetOutlineThickness(outlineThickness * distance);
            }
            else
            {
                buildObject.OutlineController.SetOutlineThickness(0);
            }
        }

        if (obj != null)
            obj.OutlineController.SetOutlineThickness(outlineThickness * distance);
    }

    public void SetWorkModeToBuild()
    {
        workMode = WorkMode.Build;

        if (switchModeToBuildSound != null)
            switchModeToBuildSound[UnityEngine.Random.Range(0, switchModeToBuildSound.Count)].Play();
    }

    public void SetWorkModeToDestroy()
    {
        workMode = WorkMode.Collect;

        if (switchModeToCollectSound != null)
            switchModeToCollectSound[UnityEngine.Random.Range(0, switchModeToCollectSound.Count)].Play();
    }

    private void UpdatePreviewObjectPosition()
    {
        if (!previewObject) return;

        previewObject.transform.position = currentHit.point;
        previewObject.Model.transform.rotation = Quaternion.FromToRotation(Vector3.up, currentHit.normal) * Quaternion.Euler(rotationFactor);
        previewObject.Model.transform.localPosition = currentHit.normal * modelDistance;
        previewObject.Model.transform.localScale = initScale * scale;
    }

    public void SelectObject(BuildObjectData objData)
    {
        selectedObjectData = objData;

        if (previewObject != null)
        {
            Destroy(previewObject.gameObject);
        }

        if (!selectedObjectData || (Inventory.Find(x => x.data == selectedObjectData).count <= 0 && !infiniteInventory)) return;

        previewObject = Instantiate(selectedObjectData.previewPrefab).GetComponent<BuildObject>();
        SetPreview(true);

        rotationFactor = Vector3.zero;
        modelDistance = selectedObjectData.startDistance;
        scale = 1;
        initScale = previewObject.Model.transform.localScale;
    }

    private void SetPreview(bool valid)
    {
        if (!previewObject) return;

        previewObject.MeshRenderer.material.color = valid ? previewColor : previewColorInvalid;
    }

    private void HidePreview()
    {
        if (!previewObject) return;

        previewObject.MeshRenderer.material.color = previewColor;
        previewObject.transform.position = Vector3.one * 1000;
    }

    public void RotateObjectX(int direction)
    {
        if (!selectedObjectData || !selectedObjectData.CanRotateX) return;

        rotationFactor.x += direction * selectedObjectData.RotationXSteps;
        if (selectedObjectData.RotationXMax - selectedObjectData.RotationXMin >= 360)
        {
            rotationFactor.x %= 360;
        }
        rotationFactor.x = Mathf.Clamp(rotationFactor.x, selectedObjectData.RotationXMin, selectedObjectData.RotationXMax);
    }

    public void RotateObjectY(int direction)
    {
        if (!selectedObjectData || !selectedObjectData.CanRotateY) return;

        rotationFactor.y += direction * selectedObjectData.RotationYSteps;
        if (selectedObjectData.RotationYMax - selectedObjectData.RotationYMin >= 360)
        {
            rotationFactor.y %= 360;
        }
        rotationFactor.y = Mathf.Clamp(rotationFactor.y, selectedObjectData.RotationYMin, selectedObjectData.RotationYMax);
    }

    public void RotateObjectZ(int direction)
    {
        if (!selectedObjectData || !selectedObjectData.CanRotateZ) return;

        rotationFactor.z += direction * selectedObjectData.RotationZSteps;
        if (selectedObjectData.RotationZMax - selectedObjectData.RotationZMin >= 360)
        {
            rotationFactor.z %= 360;
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
        return
        previewObject &&
        currentObject &&
        (canPlaceAnywhere || previewObject.Data.CanBePlacedOn(currentObject.Data)) &&
        (canOverlap || !CollidesWithObjects(previewObject));
    }

    private bool CanCollectObject()
    {
        return currentObject && !currentObject.IsBase && currentObject.ChildObjects.Count == 0;
    }

    private BuildObject GetObjectUnderMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, objectLayer))
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

    private bool CollidesWithObjects(BuildObject obj)
    {
        foreach (BuildObject buildObject in buildObjects)
        {
            if (buildObject.ModelCollider.bounds.Intersects(obj.ModelCollider.bounds))
            {
                if (currentObject && buildObject == currentObject)
                {
                    continue;
                }

                return true;
            }
        }

        return false;
    }
}
