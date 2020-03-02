using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextAnim : MonoBehaviour
{
    public AnimationCurve animCurve;        //кривая анимации

    float time;     //время анимации
    Text text;      //отображемый текст

    private void OnEnable()
    {
        text = GetComponent<Text>();
        time = 0;
    }

    private void FixedUpdate()
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, animCurve.Evaluate(time));
        if (text.color.a == 0)
            gameObject.SetActive(false);
        time += Time.fixedDeltaTime;
    }
}
