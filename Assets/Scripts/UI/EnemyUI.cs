using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    public TextMeshProUGUI enemyNameText;
    //����ֵUI
    public Slider sliderHP;
    public Image sliderImage;
    public Image fillImage;
    //��ͨ���˺󵯳�������
    public TextMeshProUGUI damageText;
    //�������˺󵯳�������
    public TextMeshProUGUI criticalText;

    private bool nameTimerIsActive = false;
    private float nameDuration = 4f;
    private float fadeDuration = 1f;

    private bool damageTimerIsActive = false;
    private float damageDuration = 2f;
    private float fadingDuration = 1f;

    private void Start()
    {
        enemyNameText.CrossFadeAlpha(0, 0.01f, false);
    }

    private void Update()
    {
        if (damageTimerIsActive)
        {
            damageDuration -= Time.deltaTime;
            if (damageDuration < 0)
            {
                damageTimerIsActive = false;
                damageText.CrossFadeAlpha(0, fadingDuration, false);
                criticalText.CrossFadeAlpha(0, fadingDuration, false);
            }
        }

        if (nameTimerIsActive)
        {
            nameDuration -= Time.deltaTime;
            if(nameDuration < 0)
            {
                nameTimerIsActive = false;
                nameDuration = 4f;
                enemyNameText.CrossFadeAlpha(0, fadeDuration, false);
            }
        }
    }

    /// <summary>
    /// ��ʾ�������ƺ�Ѫ��
    /// </summary>
    /// <param name="show"></param>
    public void ShowInfo()
    {
        StopAllCoroutines();
        sliderImage.color = new Color(sliderImage.color.r, sliderImage.color.g, sliderImage.color.b, 1);
        fillImage.color = new Color(fillImage.color.r, fillImage.color.g, fillImage.color.b, 1);
        enemyNameText.CrossFadeAlpha(1, 0.01f, false);
        nameTimerIsActive = true;
        StartCoroutine(FadeOut(sliderImage));
        StartCoroutine(FadeOut(fillImage));
    }

    /// <summary>
    /// ���õ���Ѫ��
    /// </summary>
    /// <param name="currentHP"></param>
    /// <param name="MaxHP"></param>
    public void SetEnemyHP(int currentHP, int MaxHP)
    {
        float percent = (float)currentHP / (float)MaxHP;
        sliderHP.value = percent;
    }

    public void ShowDamageText(int damageValue)
    {
        float x = Random.Range(-0.6f, 0.6f);
        float y = Random.Range(0.5f, 1.5f);
        if (damageValue <= 20)
        {
            damageText.text = damageValue.ToString();
            damageText.GetComponent<UIFollowObject>().offset = new Vector3(x, y, 0);
            damageText.CrossFadeAlpha(1, 0.01f, false);
            damageText.gameObject.SetActive(true);
        }
        else if(damageValue > 20)
        {
            criticalText.text = damageValue.ToString();
            criticalText.GetComponent<UIFollowObject>().offset = new Vector3(x, y, 0);
            criticalText.CrossFadeAlpha(1, 0.01f, false);
            criticalText.gameObject.SetActive(true);
        }
        damageTimerIsActive = true;
        damageDuration = 2f;        
    }

    /// <summary>
    /// ʹͼƬ����͸��
    /// </summary>
    /// <param name="imageToFade"></param>
    /// <returns></returns>
    IEnumerator FadeOut(Image imageToFade)
    {
        yield return new WaitForSeconds(nameDuration);
        float elapsedTime = 0f;
        Color startingColor = imageToFade.color;

        while (elapsedTime < fadeDuration)
        {
            // ����͸���Ȳ�ֵ
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);

            // ����Image��͸����
            imageToFade.color = new Color(startingColor.r, startingColor.g, startingColor.b, alpha);

            // ����ʱ��
            elapsedTime += Time.deltaTime;

            // �ȴ���һ֡
            yield return null;
        }

        // ���������������������ִ�������߼�
    }
}
