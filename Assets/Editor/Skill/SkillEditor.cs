using EditorExtend;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Skill), true)]
public class SkillEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty icon, displayName, animationPrefab, pawnAnimationState, movementLatency, weaponAnimation;
    [AutoProperty]
    public SerializedProperty preConditions, buffPreConditions, specialPreconditions, actionTime, HPCost, parameterOnAgent, buffOnAgent, extraDescription;

    protected override void MyOnInspectorGUI()
    {
        displayName.TextField("展示技能名");
        if (GUILayout.Button("自动填充"))
        {
            displayName.stringValue = GenerateDisplayName();
        }
        icon.PropertyField("图标");
        animationPrefab.PropertyField("技能特效");
        pawnAnimationState.EnumField<EPawnAnimationState>("人物动作");
        movementLatency.FloatField("人物动作延迟");
        weaponAnimation.BoolField("启用武器动画");
        preConditions.ListField("参数前置条件");
        buffPreConditions.ListField("Buff前置条件");
        specialPreconditions.ListField("特殊前置条件");
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.EndHorizontal();
        actionTime.IntField("基本时间消耗");
        HPCost.IntField("血量消耗");
        parameterOnAgent.ListField("参数修改");
        buffOnAgent.ListField("对自身施加/移除Buff");
        extraDescription.TextArea("额外描述");
        EditorGUILayout.LabelField("技能描述");
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.TextArea((target as Skill).Description);
        EditorGUI.EndDisabledGroup();
    }

    public string GenerateDisplayName()
    {
        string fileName = target.name;
        string[] ss = fileName.Split('_');
        return ss[^1];
    }
}