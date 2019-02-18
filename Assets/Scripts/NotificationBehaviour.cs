using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationBehaviour : MonoBehaviour {

    public Text notificationText;

    private bool isCrit;
    private float horizontalSpeed = 0;
    private float verticalSpeed = 0;

    public void SetAsDamage(float dmg, bool crit, Enums.DamageType type, Vector3 worldpos)
    {
        isCrit = crit;
        notificationText.text = ((int)dmg).ToString();
        transform.position = Camera.main.WorldToScreenPoint(worldpos);
        SetHorizontalAndVerticalSpeed();
        SetColor(type);
        gameObject.SetActive(true);
        gameObject.transform.SetAsLastSibling();

        if (isCrit)
            StartCoroutine("AnimationCrit");
        else
            StartCoroutine("AnimationNoCrit");
    }
    public void SetAsStatus(Enums.StatusEffect status, Vector3 worldpos)
    {
        notificationText.text = "Frozen";
        transform.position = Camera.main.WorldToScreenPoint(worldpos);
        SetHorizontalAndVerticalSpeed();
        SetColor(Enums.DamageType.cryo);
        gameObject.SetActive(true);
        gameObject.transform.SetAsLastSibling();
        StartCoroutine("AnimationCrit");
    }
    private void SetHorizontalAndVerticalSpeed() {
        horizontalSpeed = Random.Range(-2f, 2f) * Screen.width / 50;
        if (isCrit)
            verticalSpeed = Random.Range(0.65f, 1f) * Screen.height / 50;
        else
            verticalSpeed = Random.Range(1f, 2.5f) * Screen.height / 50;
    }
    private void SetColor(Enums.DamageType type)
    {
        switch (type)
        {
            case Enums.DamageType.normal:
                {
                    notificationText.color = Color.white;
                    break;
                }
            case Enums.DamageType.electricDamage:
                {
                    notificationText.color = Color.yellow;
                    break;
                }
            case Enums.DamageType.cryo:
                {
                    notificationText.color = Color.cyan;
                    break;
                }
            case Enums.DamageType.nuclearDamage:
                {
                    notificationText.color = Color.blue;
                    break;
                }
            case Enums.DamageType.photon:
                {
                    notificationText.color = Color.red;
                    break;
                }
            case Enums.DamageType.plasma:
                {
                    notificationText.color = new Color(1, 0, 0.5f);
                    break;
                }
        }
    }
	// Update is called once per frame
	void Update () {
        transform.Translate(horizontalSpeed * Time.deltaTime, verticalSpeed * Time.deltaTime, 0);
        if (!isCrit)
		    verticalSpeed -= 0.1f * Screen.height * Time.deltaTime;
	}
    IEnumerator AnimationNoCrit()
    {
        float t = 0; float animspeed = 7.5f;
        transform.localScale = Vector3.one * (t + 1f);
        while (t < 1)
        {
            t += Time.deltaTime * animspeed;
            transform.localScale = Vector3.one * (t + 1f);
            yield return null;
        }
        while (t > -0.25f)
        {
            t -= Time.deltaTime * animspeed;
            transform.localScale = Vector3.one * (t + 1f);
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
    IEnumerator AnimationCrit()
    {
        float t = 0; float animspeed = 7.5f;
        transform.localScale = Vector3.one * (t + 1f);
        while (t < 1)
        {
            t += Time.deltaTime * animspeed;
            transform.localScale = Vector3.one * (t*2.5f);
            yield return null;
        }
        while (t > 0.5f)
        {
            t -= Time.deltaTime * animspeed;
            transform.localScale = Vector3.one * (t * 2.5f);
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}
