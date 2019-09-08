using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class ARController : MonoBehaviour
{

    public GameObject cameraParent;

    public GameObject museumEnvironment;
    public GameObject defaultCamLocation;
    
    private GameObject augmentationPlane;
    public GameObject museumLocationService;

    public GameObject screens;
    private TMP_Dropdown topicDropdown;

    public GameObject debugOut;
    private static GameObject debugOutStatic;

    private bool cameraMoving;

    // Start is called before the first frame update
    void Start()
    {
        museumEnvironment.SetActive(false);

        InstantiateMO(CommandCenter.museumObjects.Values.ToList(), museumEnvironment.transform);
        OrganizeMuseumSpaces(CommandCenter.museumObjects.Values.Where(m => m.type == "space" || m.type == "place").ToList(), museumEnvironment.transform);

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

        topicDropdown = screens.transform.Find("Topic").GetComponent<TMP_Dropdown>();
        topicDropdown.ClearOptions();
        topicDropdown.AddOptions(CommandCenter.museumThreads.Keys.ToList());

        museumLocationService.GetComponent<MuseumLocationService>().UpdateLocationEvent += UpdateLocation;

        debugOutStatic = debugOut;

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
    public void FilterMuseumObjects(int o)
    {
        string thread = topicDropdown.options[o].text;
        if (CommandCenter.museumThreads[thread] != null)
        {
            foreach (MuseumObjectRep m in CommandCenter.museumObjects.Values)
            {
                if (CommandCenter.museumThreads[thread].Contains(m))
                {
                    m.included = true;
                }
                else
                {
                    m.included = false;
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
            if (c.gameObject.name != "Selection") { continue; }

            foreach (MuseumObjectRep MO in CommandCenter.museumObjects.Values)
            {
                MO.GO.GetComponent<MuseumObjectController>().aimedAt = false;
            }
            c.gameObject.GetComponentInParent<MuseumObjectController>().aimedAt = true;
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

        if (cameraMoving)
        {
            UpdateScreenText("Status/Text", "Relocating...");
        }
        else
        {
            if (CommandCenter.currentLocation != "")
            {
                UpdateScreenText("Status/Text", CommandCenter.museumObjects[CommandCenter.currentLocation].desc);
            }
            else
            {
                UpdateScreenText("Status/Text", "Outside museum!");
            }
        }

        //ScreenDebug(gameObject, EnumerateTransform(museumLocationService.transform, museumLocationService.name + "/", 0, 3));
    }

    public void RelocateCamera(Transform t)
    {
        cameraParent.transform.position = t.position;
        Camera.main.transform.localPosition = Vector3.zero;
    }
    public void RelocateCamera(Vector3 v, bool normalized = false)
    {
        cameraParent.transform.position = normalized ? CommandCenter.DenormalizedMuseumVectors(v, true) : v;
        Camera.main.transform.localPosition = Vector3.zero;
    }
    public void MoveCamera(string direction)
    {
        Vector3 newPosition = cameraParent.transform.position;
        Vector3 oldPosition = cameraParent.transform.position;

        switch(direction)
        {
            case "W":
                newPosition += Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)) * 3;
                StartCoroutine(Alert("Moving 3m forward", 2));
                break;
            case "A":
                newPosition += Vector3.Scale(Camera.main.transform.right, new Vector3(1, 0, 1)) * -3;
                StartCoroutine(Alert("Moving 3m left", 2));
                break;
            case "S":
                newPosition += Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)) * -3;
                StartCoroutine(Alert("Moving 3m backward", 2));
                break;
            case "D":
                newPosition += Vector3.Scale(Camera.main.transform.right, new Vector3(1, 0, 1)) * 3;
                StartCoroutine(Alert("Moving 3m right", 2));
                break;
            case "E":
                newPosition += CommandCenter.DenormalizedMuseumVectors(new Vector3(0f, 0.25f, 0f), false);
                StartCoroutine(Alert("Moving up 1 floor", 2));
                break;
            case "C":
                newPosition += CommandCenter.DenormalizedMuseumVectors(new Vector3(0f, -0.25f, 0f), false);
                StartCoroutine(Alert("Moving down 1 floor", 2));
                break;
        }

        StartCoroutine(InterpolateCamera(oldPosition, newPosition, 60));
    }
    private IEnumerator InterpolateCamera(Vector3 start, Vector3 end, int frames)
    {
        cameraMoving = true;
        for (int i = 1; i <= frames; i++)
        {
            RelocateCamera(Vector3.Lerp(start, end, i / (float)frames));
            yield return null;
        }
        cameraMoving = false;
    }

    internal void UpdateLocation(object sender, EventMessage e)
    {
        string exhibitId = e.msg;
        if (!CommandCenter.museumObjects.TryGetValue(exhibitId, out MuseumObjectRep m)) return;

        Vector3 cameraHeading = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1));
        Vector3 newPosition = m.GO.transform.position - cameraHeading * 0.3f;

        StartCoroutine(InterpolateCamera(cameraParent.transform.position, newPosition, 60));
    }

    private void UpdateScreenText(string path, string text)
    {
        screens.transform.Find(path).gameObject.GetComponent<TextMeshProUGUI>().text = text;
    }
    private IEnumerator Alert(string msg, float t)
    {
        ToggleScreenElementVisible("Alert");
        UpdateScreenText("Alert/Text", msg);
        yield return new WaitForSeconds(t);
        ToggleScreenElementVisible("Alert", false);
    }
    private void ToggleScreenElementVisible(string path, bool visible = true)
    {
        screens.transform.Find(path).gameObject.SetActive(visible);
    }
    public void ShowMovementControl(bool visible)
    {
        ToggleScreenElementVisible("Controls/Simulation", visible);
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

        FilterMuseumObjects(0);
        topicDropdown.RefreshShownValue();
        yield return new WaitForSeconds(3);

        ToggleScreenElementVisible("Alert", false);
    }

    private string EnumerateTransform(Transform t, string basePath, int currentLevel = 0, int maxLevel = 1000)
    {
        if (currentLevel >= maxLevel) return "";
        string output = "";
        foreach (Transform c in t)
        {
            string newLn = c.gameObject.activeSelf + ", " + basePath + c.name + "/";
            output += newLn + "\n";
            output += EnumerateTransform(c, newLn, currentLevel + 1, maxLevel);
        }
        return output;
    }
    public static void ScreenDebug(GameObject o, string msg)
    {
        debugOutStatic.GetComponent<TextMeshProUGUI>().text = Mathf.Round(Time.time * 1000) / 1000 + (o != null ? " " + o.name + " " : "") + "\n";
        debugOutStatic.GetComponent<TextMeshProUGUI>().text += msg + "\n";
    }

}
