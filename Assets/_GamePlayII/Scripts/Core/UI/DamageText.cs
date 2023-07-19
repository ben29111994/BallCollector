using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    public TextMeshProUGUI numberText;
    public AnimationCurve animCurve;
    public Color colorBlue;
    public Color colorRed;

    public void Active(Vector3 _pos,int _damage,eNameTeam _eNameTeam)
    {
        transform.position = _pos + Vector3.up * 5.0f;
        numberText.text = "-" + _damage;
        numberText.color = _eNameTeam == eNameTeam.Blue ? colorBlue : colorRed;
        gameObject.SetActive(true);
        StartCoroutine(C_Anim());
    }

    private IEnumerator C_Anim()
    {
        float t = 0.0f;
        Vector3 pos = transform.position;
        Vector3 tarPos = pos + Vector3.forward * Random.Range(10.0f, 15.0f);
        tarPos.x += Random.Range(-2.0f, 2.0f);

        while(t < 1.0f)
        {
            Vector3 lerp = Vector3.Lerp(pos, tarPos, t);
            lerp.y += animCurve.Evaluate(t);
            transform.position = lerp;
            t += Time.deltaTime * 1.2f;
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
