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

    public GameObject desc;
    public GameObject back;

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
        augmentationPlane.transform.localScale = new Vector3(frustumWidth, frustumHeight, 1);
        augmentationPlane.transform.SetParent(museumEnvironment.transform);
        augmentationPlane.layer = 12;
        augmentationPlane.GetComponent<MeshCollider>().convex = true;
        augmentationPlane.GetComponent<Renderer>().enabled = false;
        //augmentationPlane.AddComponent<ReverseNormals>();

        StartCoroutine(DemonstrationInitialScreen());
    }

    private void InstantiateMO(List<MuseumObjectMetadata> mObjs, Transform parent)
    {
       foreach (MuseumObjectMetadata mObj in mObjs) if (!mObj.GO) { mObj.InstantiateGO(parent); }
    }
    private void OrganizeMuseumSpaces(List<MuseumObjectMetadata> mObjs, Transform parent)
    {
        foreach (MuseumObjectMetadata mObj in mObjs)
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

            foreach (MuseumObjectMetadata MO in CommandCenter.museumExhibits.Values)
            {
                MO.GO.GetComponent<MuseumObjectController>().aimedAt = false;
            }
            c.gameObject.GetComponentInParent<MuseumObjectController>().aimedAt = true;
            screens.transform.Find("Controls/Topic/Text").gameObject.GetComponent<TextMeshProUGUI>().text = c.gameObject.GetComponentInParent<MuseumObjectController>().metadata.name;
            return;
        }
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Physics.Raycast(Camera.main.ScreenPointToRay(touch.position), out RaycastHit hit);
            if (hit.collider.gameObject.GetComponentInParent<MuseumObjectController>())
            {
                MuseumObjectController MOC = hit.collider.gameObject.GetComponentInParent<MuseumObjectController>();
                desc.SetActive(true);
                desc.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = MOC.metadata.desc;
                back.SetActive(true);
            }
        }
    }

    public void ResetCamera()
    {
        cameraParent.transform.position = CommandCenter.DenormalizedMuseumVectors(Vector3.zero, true) - cameraParent.transform.Find("Main Camera").localPosition;
    }

    private IEnumerator DemonstrationInitialScreen()
    {
        screens.transform.Find("Controls/Status/Text").gameObject.GetComponent<TextMeshProUGUI>().text = "Acquiring location...";
        screens.transform.Find("Controls/Topic").gameObject.SetActive(false);
        screens.transform.Find("Controls/Alert/Text").gameObject.GetComponent<TextMeshProUGUI>().text = "Please slowly move your phone around...";
        screens.transform.Find("Controls/Controls").gameObject.SetActive(false);
        yield return new WaitForSeconds(5);
        screens.transform.Find("Controls/Status/Text").gameObject.GetComponent<TextMeshProUGUI>().text = "First floor, Foyer";
        screens.transform.Find("Controls/Topic").gameObject.SetActive(true);
        //canvas.transform.Find("Controls/Alert").gameObject.SetActive(false);
        screens.transform.Find("Controls/Controls").gameObject.SetActive(true);
        screens.transform.Find("Controls/Alert/Text").gameObject.GetComponent<TextMeshProUGUI>().text = "Please scan around with your phone";
        yield return new WaitForSeconds(3);
        screens.transform.Find("Controls/Alert").gameObject.SetActive(false);
        museumEnvironment.SetActive(true);
    }
    public void hide()
    {
        desc.SetActive(false);
        back.SetActive(false);
    }

}
