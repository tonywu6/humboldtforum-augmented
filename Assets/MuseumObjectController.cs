using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuseumObjectController : MonoBehaviour
{
    public static GameObject museumExhibitPrefab;

    private GameObject museumObjRep;
    private GameObject screenAnchor;
    private GameObject endpoint;
    private bool outOfView = true;

    internal MuseumObjectMetadata metadata;

    // Start is called before the first frame update
    void Start()
    {
        museumExhibitPrefab = Resources.Load<GameObject>("Prefabs/MuseumExhibit");
        museumObjRep = Instantiate(museumExhibitPrefab);
        museumObjRep.name = "Indicator";
        museumObjRep.transform.SetParent(transform);
        museumObjRep.transform.localPosition = Vector3.zero;

        screenAnchor = museumObjRep.transform.Find("ScreenAnchor").gameObject;
        endpoint = museumObjRep.transform.Find("Endpoint").gameObject;

        ChangeColor(endpoint, new Color32((byte)metadata.curatorial["colors"][0][0].AsInt, (byte)metadata.curatorial["colors"][0][1].AsInt, (byte)metadata.curatorial["colors"][0][2].AsInt, 255));
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.DrawLine(transform.position, Camera.main.transform.position);
        if (Physics.Linecast(transform.position, Camera.main.transform.position, out RaycastHit hitInfo, 1 << 12))
        {
            print(metadata.name);
            Debug.DrawLine(Camera.main.transform.position, hitInfo.point, new Color(1, 0, 0, 1));

            screenAnchor.transform.position = hitInfo.point;
            if (outOfView) { endpoint.transform.position = hitInfo.point; endpoint.SetActive(true); outOfView = false; }
        }
        else
        {
            screenAnchor.transform.localPosition = Vector3.zero;
            if (!outOfView) { endpoint.transform.position = Vector3.zero; endpoint.SetActive(false); outOfView = true; }
        }
    }

    private void ChangeColor(GameObject GO, Color col)
    {
        MaterialPropertyBlock propBlock = new MaterialPropertyBlock();
        Renderer renderer = GO.GetComponent<Renderer>();
        renderer.GetPropertyBlock(propBlock);
        propBlock.SetColor("_Color", col);
        renderer.SetPropertyBlock(propBlock);
    }

}
