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
    //����ֵUI
    public Slider sliderHP;
    public TextMeshProUGUI hpText;
    //ħ��ֵUI
    public Slider sliderMP;
    public TextMeshProUGUI mpText;
    //��ս������
    public SkillUI skill1;
    //Զ��ħ������
    public SkillUI skill2;
    //����Buff����
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
    /// ħ��ֵ����
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
    /// ���ݴ��뼼�ܱ���޸ļ���ͼƬ
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
    /// ������ȴ
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
    /// ���ü�������ħ��ֵ�Ƿ��㹻
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
