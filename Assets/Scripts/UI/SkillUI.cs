using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    //����ͼ��
    public Image skillImage;
    //��ʼ����ͼƬ����ȴʱ����ͼƬ
    public Sprite[] sprites;
    //��ȴ����ʱͼƬ
    public Image coolTimeImage;

    public bool isMPEnough = true;

    /// <summary>
    /// �л�����ͼƬ
    /// isCoolΪtrueʱ������ȴ
    /// </summary>
    /// <param name="isCool"></param>
    public void ChangeSkillImage(bool isCool)
    {
        if (isCool)
        {
            skillImage.sprite = sprites[1];
        }
        else
        {
            skillImage.sprite = sprites[0];
        }
    }

    /// <summary>
    /// ������ȴ����ʱ
    /// </summary>
    /// <param name="percent"></param>
    public void CoolTime(float percent)
    {
        coolTimeImage.fillAmount = percent;
        if(coolTimeImage.fillAmount <= 0.01f)
        {
            coolTimeImage.fillAmount = 0;
            if (isMPEnough)
            {
                ChangeSkillImage(false);
            }
        }
    }
}
