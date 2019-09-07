using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class ARController : MonoBehaviour
{

    public GameObject cameraParent;

    public GameObject museumEnvironment;
    public GameObject screens;
    public GameObject defaultCamLocation;
    
    private GameObject augmentationPlane;

    // Start is called before the first frame update
    void Start()
    {
        museumEnvironment.SetActive(false);

        InstantiateMO(CommandCenter.museumSpaces.Values.ToList(), museumEnvironment.transform);
        InstantiateMO(CommandCenter.museumExhibits.Values.ToList(), museumEnvironment.transform);
        OrganizeMuseumSpaces(CommandCenter.museumSpaces.Values.ToList(), museumEnvironment.transform);

        var frustumHeight = 2.0f * 10 * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad) + 1f;
        var frustumWidth = frustumHeight * Camera.main.aspect;
        augmentationPlane = GameObject.CreatePrimitive(PrimitiveType.Quad);
        augmentationPlane.name = "AugmentationPlane";
        augmentationPlane.transform.localScale = new Vector3(frustumWidth * 1.1f, frustumHeight * 1.1f, 1);
        augmentationPlane.transform.SetParent(museumEnvironment.transform);
        augmentationPlane.layer = 12;
        augmentationPlane.GetComponent<MeshCollider>().convex = true;
        augmentationPlane.GetComponent<Renderer>().enabled = false;
        //augmentationPlane.AddComponent<ReverseNormals>();

        StartCoroutine(DemonstrationInitialScreen());
    }

    private void InstantiateMO(List<MuseumObjectRep> mObjs, Transform parent)
    {
       foreach (MuseumObjectRep mObj in mObjs) if (!mObj.GO) { mObj.InstantiateGO(parent); }
    }
    private void OrganizeMuseumSpaces(List<MuseumObjectRep> mObjs, Transform parent)
    {
        foreach (MuseumObjectRep mObj in mObjs)
        {
            foreach (string child in mObj.relationship["children"].AsArray.Values)
            {
                var childTransform = parent.Find(child);
                if (childTransform)
                {
                    childTransform.parent = mObj.GO.transform;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //augmentationPlane.transform.LookAt(Camera.main.transform.position);
        augmentationPlane.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 10;
        augmentationPlane.transform.rotation = Camera.main.transform.rotation;
        foreach (Collider c in Physics.OverlapCapsule(Camera.main.transform.position, Camera.main.transform.position + Camera.main.transform.forward * 20, 0.1f, 1 << 15))
        {
            screens.transform.Find("Controls/Topic/Text").gameObject.GetComponent<TextMeshProUGUI>().text = "-";
            if (c.gameObject.name != "Selection") { continue; }

            foreach (MuseumObjectRep MO in CommandCenter.museumExhibits.Values)
            {
                MO.GO.GetComponent<MuseumObjectController>().aimedAt = false;
            }
            c.gameObject.GetComponentInParent<MuseumObjectController>().aimedAt = true;
            UpdateScreenText("Topic/Text", c.gameObject.GetComponentInParent<MuseumObjectController>().metadata.name);
            return;
        }
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Physics.Raycast(Camera.main.ScreenPointToRay(touch.position), out RaycastHit hit);
            if (hit.collider.gameObject.GetComponentInParent<MuseumObjectController>())
            {
                MuseumObjectController MOC = hit.collider.gameObject.GetComponentInParent<MuseumObjectController>();
                ToggleScreenElementVisible("Desc");
                UpdateScreenText("Desc/Text", MOC.metadata.desc);
            }
        }

        if (CommandCenter.museumSpaces.Count > 0)
        {
            string currentLocationId = "";
            foreach (string zoneId in CommandCenter.currentZones)
            {
                if (string.Compare(zoneId, currentLocationId) > 0)
                {
                    currentLocationId = zoneId;
                }
            }
            if (currentLocationId != "")
            {
                UpdateScreenText("Status/Text", CommandCenter.museumSpaces[currentLocationId].desc);
            } else
            {
                UpdateScreenText("Status/Text", "Outside museum!");
            }
        }
    }

    public void RelocateCamera(Transform t)
    {
        cameraParent.transform.position = CommandCenter.DenormalizedMuseumVectors(t.position, true) - Camera.main.transform.localPosition;
    }
    public void RelocateCamera(Vector3 v, bool normalized = false)
    {
        cameraParent.transform.position = (normalized ? CommandCenter.DenormalizedMuseumVectors(v, true) : v) - Camera.main.transform.localPosition;
    }
    public void MoveCamera(string direction)
    {
        Vector3 newPosition = cameraParent.transform.position;
        Vector3 oldPosition = cameraParent.transform.position;

        switch(direction)
        {
            case "W":
                newPosition += Camera.main.transform.forward * 3;
                break;
            case "A":
                newPosition += Camera.main.transform.right * -3;
                break;
            case "S":
                newPosition += Camera.main.transform.forward * -3;
                break;
            case "D":
                newPosition += Camera.main.transform.right * 3;
                break;
            case "E":
                newPosition += CommandCenter.DenormalizedMuseumVectors(new Vector3(0f, 0.25f, 0f), false);
                break;
            case "C":
                newPosition += CommandCenter.DenormalizedMuseumVectors(new Vector3(0f, -0.25f, 0f), false);
                break;
        }

        StartCoroutine(InterpolateCamera(oldPosition, newPosition, 60));
    }
    private IEnumerator InterpolateCamera(Vector3 start, Vector3 end, int frames)
    {
        for (int i = 1; i <= frames; i++)
        {
            RelocateCamera(Vector3.Lerp(start, end, i / (float)frames));
            yield return null;
        }
    }

    private void UpdateScreenText(string path, string text)
    {
        screens.transform.Find(path).gameObject.GetComponent<TextMeshProUGUI>().text = text;
    }
    private void ToggleScreenElementVisible(string path, bool visible = true)
    {
        screens.transform.Find(path).gameObject.SetActive(visible);
    }
    private IEnumerator DemonstrationInitialScreen()
    {
        UpdateScreenText("Status/Text", "Acquiring location...");
        ToggleScreenElementVisible("Topic", false);
        UpdateScreenText("Alert/Text", "Please slowly move your phone around...");
        ToggleScreenElementVisible("Controls/Options", false);
        RelocateCamera(defaultCamLocation.transform.position);

        yield return new WaitForSeconds(5);

        museumEnvironment.SetActive(true);
        ToggleScreenElementVisible("Topic");
        ToggleScreenElementVisible("Controls/Options");
        UpdateScreenText("Alert/Text", "Location acquired");
        yield return new WaitForSeconds(3);

        ToggleScreenElementVisible("Alert", false);

    }
    public void ShowMovementControl(bool visible)
    {
        ToggleScreenElementVisible("Controls/Simulation", visible);
    }

}
