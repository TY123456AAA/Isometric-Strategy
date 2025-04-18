using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 在技能作用范围内各位置生成同样物体的动画
/// </summary>
public class AreaAnimation : AnimationObject
{
    public GameObject prefab;
    private readonly List<Vector3Int> area = new();

    public override void Initialize(IAnimationSource source)
    {
        base.Initialize(source);
        if (prefab != null)
        {
            PawnAction action = source as PawnAction;
            if(action.skill is AimSkill aimSkill)
                aimSkill.MockArea(igm, action.position, action.target, area);
            else
            {
                area.Clear();
                area.Add(action.target);
            }

            for (int i = 0; i < area.Count; i++)
            {
                Vector3 world = igm.CellToWorld(area[i]);
                objectManager.Activate(prefab.name, world, Vector3.zero, transform);
            }
        }
    }
}
