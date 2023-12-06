using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    //技能图标
    public Image skillImage;
    //初始技能图片和冷却时技能图片
    public Sprite[] sprites;
    //冷却倒计时图片
    public Image coolTimeImage;

    public bool isMPEnough = true;

    /// <summary>
    /// 切换技能图片
    /// isCool为true时进入冷却
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
    /// 技能冷却倒计时
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
