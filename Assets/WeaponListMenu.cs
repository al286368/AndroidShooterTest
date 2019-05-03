using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponListMenu : MonoBehaviour
{
    public List<WeaponInventoryElement> elementsInList;
    public Transform displaceTransform;

    private int currentIndex = 0;
    private List<WeaponData> listInDisplay;
    private bool currentlyScrolling = false;

    private const float MOVEMENT_DISTANCE_SMALL_ELEMENTS = 125f;
    private const float MOVEMENT_DISTANCE_CENTER_ELWEMENTS = 150f;

    // Start is called before the first frame update
    void Start()
    {
        listInDisplay = GlobalGameManager.currentInstance.GetGameData().GetWeaponList();
        UpdateAllElements();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            MoveDown();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveUp();
        }
    }
    public void MoveDown() {
        if (currentIndex < listInDisplay.Count - 1 && !currentlyScrolling)
            StartCoroutine("ScrollDownAnimation");
    }
    public void MoveUp() {
        if (currentIndex > 0 && !currentlyScrolling)
            StartCoroutine("ScrollUpAnimation");
        print(currentIndex);
    }
    public void UpdateAllElements() {
        for (int i = 0; i < elementsInList.Count; i++) {
            if (currentIndex + i - 4 > 0 && currentIndex + i - 4 < listInDisplay.Count)
            {
                elementsInList[i].gameObject.SetActive(true);
                elementsInList[i].SetFor(listInDisplay[currentIndex + i -4]);
            } else {
                elementsInList[i].gameObject.SetActive(false);
            }

        }
    }

    IEnumerator ScrollUpAnimation() {
        currentlyScrolling = true;
        float animSpeed = 5;
        float t = 0;
        while (t < 1) {
            t = Mathf.MoveTowards(t, 1, Time.deltaTime * animSpeed);
            displaceTransform.localPosition = new Vector3(0,-150 * t,0);
            yield return null;
        }
        currentIndex--;
        UpdateAllElements();
        displaceTransform.localPosition = Vector3.zero;
        currentlyScrolling = false;
    }
    IEnumerator ScrollDownAnimation()
    {
        currentlyScrolling = true;
        float animSpeed = 5;
        float t = 0;
        while (t < 1)
        {
            t = Mathf.MoveTowards(t, 1, Time.deltaTime * animSpeed);
            displaceTransform.localPosition = new Vector3(0,150 * t, 0);
            yield return null;
        }
        currentIndex++;
        UpdateAllElements();
        displaceTransform.localPosition = Vector3.zero;
        currentlyScrolling = false;
    }
}
