using Suntail;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    public TextMeshProUGUI nickNameText;
    //生命值UI
    public Slider sliderHP;
    public TextMeshProUGUI hpText;
    //魔法值UI
    public Slider sliderMP;
    public TextMeshProUGUI mpText;
    //近战物理技能
    public SkillUI skill1;
    //远程魔法技能
    public SkillUI skill2;
    //增益Buff技能
    public SkillUI skill3;

    public void Init()
    {
        SkillRelease(0, false);
        SkillRelease(1, false);
        SkillRelease(2, false);

        SkillCD(0, 0);
        SkillCD(1, 0);
        SkillCD(2, 0);
    }

    /// <summary>
    /// 魔法值消耗
    /// </summary>
    /// <param name="currentMP"></param>
    /// <param name="MaxMP"></param>
    public void MPCost(int currentMP, int MaxMP)
    {
        float percent = (float)currentMP / (float)MaxMP;
        sliderMP.value = percent;
        mpText.text = $"{currentMP}/{MaxMP}";
    }

    /// <summary>
    /// 根据传入技能编号修改技能图片
    /// </summary>
    /// <param name="skillID"></param>
    /// <param name="isCool"></param>
    public void SkillRelease(int skillID, bool isCool)
    {
        switch (skillID)
        {
            case 0:
                skill1.ChangeSkillImage(isCool);
                break;
            case 1:
                skill2.ChangeSkillImage(isCool);
                break;
            case 2:
                skill3.ChangeSkillImage(isCool);
                break;
        }
    }

    /// <summary>
    /// 技能冷却
    /// </summary>
    /// <param name="skillID"></param>
    /// <param name="percent"></param>
    public void SkillCD(int skillID, float percent)
    {
        switch (skillID)
        {
            case 0:
                skill1.CoolTime(percent);
                break;
            case 1:
                skill2.CoolTime(percent);
                break;
            case 2:
                skill3.CoolTime(percent);
                break;
        }
    }

    /// <summary>
    /// 设置技能消耗魔法值是否足够
    /// </summary>
    /// <param name="skillID"></param>
    /// <param name="enough"></param>
    public void SetISMPEnough(int skillID, bool enough)
    {
        switch (skillID)
        {
            //case 0:
            //    skill1.isMPEnough = enough;
            //    break;
            case 1:
                skill2.isMPEnough = enough;
                if (!enough)
                {
                    skill2.ChangeSkillImage(true);
                }
                break;
            case 2:
                skill3.isMPEnough = enough;
                if (!enough)
                {
                    skill3.ChangeSkillImage(true);
                }
                break;
        }
    }
}
