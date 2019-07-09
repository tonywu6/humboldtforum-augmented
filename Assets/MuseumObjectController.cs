using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MuseumObjectController : MonoBehaviour
{
    public static GameObject museumExhibitPrefab;

    private GameObject museumObjRep;

    private GameObject indicator;
    private GameObject indicatorAnchor;
    private GameObject indicatorPin;
    private GameObject indicatorArrow;
    private int indicatorNeighborCount;

    private GameObject sign;
    private GameObject signPlate;

    internal Color accentColor;
    internal Color textColor;

    internal bool included = true;

    internal MuseumObjectMetadata metadata;

    // Start is called before the first frame update
    void Start()
    {
        museumExhibitPrefab = Resources.Load<GameObject>("Prefabs/MuseumExhibit");
        museumObjRep = Instantiate(museumExhibitPrefab);
        museumObjRep.name = "MuseumObject";
        museumObjRep.transform.SetParent(transform);
        museumObjRep.transform.localPosition = Vector3.zero;

        indicator = museumObjRep.transform.Find("Indicator").gameObject;
        indicatorAnchor = indicator.transform.Find("ScreenAnchor").gameObject;
        indicatorPin = indicator.transform.Find("Pin").gameObject;
        indicatorArrow = indicator.transform.Find("Pin/Arrow").gameObject;

        sign = indicator.transform.Find("Sign").gameObject;
        signPlate = sign.transform.Find("Plate").gameObject;
        TextOf(sign).text = metadata.name;

        sign.transform.localPosition = Vector3.zero;

        accentColor = new Color32((byte)metadata.curatorial["colors"][0][0].AsInt, (byte)metadata.curatorial["colors"][0][1].AsInt, (byte)metadata.curatorial["colors"][0][2].AsInt, 255);
        textColor = accentColor.r * .2126 + accentColor.g * .7152 + accentColor.b * .0722 > 0.75 ? Color.black : Color.white;
        ChangeColor(indicatorPin, accentColor);
        ChangeColor(signPlate, accentColor);
        foreach (Transform t in indicatorArrow.transform) ChangeColor(t.gameObject, accentColor);
        TextOf(sign).color = textColor;

        MakeVisible(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!included) return;

        if ((transform.position - Camera.main.transform.position).sqrMagnitude < 144)
        {
            indicator.transform.localPosition = Vector3.zero;
            indicatorArrow.SetActive(true);
        }
        else if (Physics.Raycast(Camera.main.transform.position, (transform.position - Camera.main.transform.position).normalized, out RaycastHit hitInfo, Mathf.Infinity, 1 << 12))
        {
            indicator.transform.position = hitInfo.point;
            indicatorArrow.SetActive(false);

            Debug.DrawLine(Camera.main.transform.position, hitInfo.point, new Color(1, 0, 0, 1));
            Debug.DrawLine(indicatorAnchor.transform.position, indicatorPin.transform.position, new Color(1, 1, 0, 1));
            Debug.DrawLine(Camera.main.transform.position, transform.position, new Color(1, 0, 0, 0.5f));
        }
        else
        {
            MakeVisible(false);
            return;
        }

        indicatorPin.transform.rotation = LeveledCameraRotation();
        sign.transform.rotation = LeveledCameraRotation();
        sign.transform.position = indicatorPin.transform.position;
        signPlate.transform.localScale = new Vector3(TextOf(sign).GetRenderedValues().x / 85, TextOf(sign).GetRenderedValues().y / 95, 0.02f);

        indicatorNeighborCount = signPlate.GetComponent<OverlappingDetection>().overlappingCount;
        if (indicatorNeighborCount == 0)
        {
            StartCoroutine(ShowSignOnStabilize());
        } else
        {
            foreach (Renderer r in sign.GetComponentsInChildren<Renderer>()) { r.enabled = false; }
            indicatorPin.GetComponent<Renderer>().enabled = true;
        }

        MakeVisible(true);

    }

    private void ChangeColor(GameObject GO, Color col)
    {
        MaterialPropertyBlock propBlock = new MaterialPropertyBlock();
        Renderer r = GO.GetComponent<Renderer>();
        r.GetPropertyBlock(propBlock);
        propBlock.SetColor("_Color", col);
        r.SetPropertyBlock(propBlock);
    }
    private TextMeshPro TextOf(GameObject GO)
    {
        return GO.transform.Find("Text").gameObject.GetComponent<TextMeshPro>();
    }
    private void MakeVisible(bool visible)
    {
        sign.SetActive(visible);
        indicator.SetActive(visible);
    }
    private Quaternion LeveledCameraRotation()
    {
        return Quaternion.Euler(Camera.main.transform.rotation.eulerAngles.x, Camera.main.transform.rotation.eulerAngles.y, 0);
    }
    private IEnumerator ShowSignOnStabilize()
    {
        for (int i = 0; i < 60; i++) 
        {
            if (indicatorNeighborCount > 0)
            {
                yield break;
            }
            yield return null;
        }
        foreach (Renderer r in sign.GetComponentsInChildren<Renderer>()) { r.enabled = true; }
        indicatorPin.GetComponent<Renderer>().enabled = false;
    }

}
