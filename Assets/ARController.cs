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

    private GameObject augmentationPlane;

    // Start is called before the first frame update
    void Start()
    {
        museumEnvironment.SetActive(false);

        InstantiateMO(CommandCenter.museumSpaces.Values.ToList(), museumEnvironment.transform);
        InstantiateMO(CommandCenter.museumExhibits.Values.ToList(), museumEnvironment.transform);
        OrganizeMuseumSpaces(CommandCenter.museumSpaces.Values.ToList(), museumEnvironment.transform);

        var frustumHeight = 2.0f * 30 * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
        var frustumWidth = frustumHeight * Camera.main.aspect;
        augmentationPlane = GameObject.CreatePrimitive(PrimitiveType.Quad);
        augmentationPlane.name = "AugmentationPlane";
        augmentationPlane.transform.localScale = new Vector3(frustumWidth, frustumHeight, 1);
        augmentationPlane.transform.SetParent(museumEnvironment.transform);
        augmentationPlane.layer = 12;
        augmentationPlane.GetComponent<MeshCollider>().convex = true;
        augmentationPlane.GetComponent<Renderer>().enabled = false;

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
        augmentationPlane.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 30;
        augmentationPlane.transform.rotation = Camera.main.transform.rotation;
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

}
