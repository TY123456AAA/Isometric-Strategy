using AStar;
using Character;
using EditorExtend.GridEditor;
using System;

public class MovableGridObject : GridObject
{
    public static bool ObjectCheck_AllObject(PawnEntity pawn, GridObject gridObject)
    {
        return true;
    }
    public static bool ObjectCheck_IgnoreAlly(PawnEntity pawn, GridObject gridObject)
    {
        Entity entity = gridObject.GetComponentInParent<Entity>();
        if (entity != null && pawn.FactionCheck(entity) > 0)
            return false;
        return true;
    }

    public IsometricGridManager Igm => IsometricGridManager.Instance;
    public PawnEntity Pawn { get; protected set; }
    public AMover Mover_Default { get; protected set; }
    public AMover Mover_Ranging { get; protected set; }
    public GridObjectMoveController MoveController { get; protected set; }

    public CharacterProperty climbAbility;
    public CharacterProperty dropAbility;
    public CharacterProperty moveAbility;

    protected override void Awake()
    {
        base.Awake();
        Pawn = GetComponentInParent<PawnEntity>();
        MoveController = GetComponentInChildren<GridObjectMoveController>();
        Mover_Default = new AMover_Default(this)
        {
            GetMoveAbility = () => moveAbility.IntValue
        };
        Mover_Ranging = new AMover_Ranging(this)
        {
            GetMoveAbility = () => 5 * moveAbility.IntValue
        };
    }

    public void RefreshProperty()
    {
        climbAbility.Refresh();
        dropAbility.Refresh();
        moveAbility.Refresh();
    }

    public virtual bool JumpCheck(Node from, Node to, Func<PawnEntity, GridObject, bool> ObjectCheck = null)
    {
        JumpSkill jumpSkill = Pawn.SkillManager.FindSkill<JumpSkill>();
        if (jumpSkill == null)
            return false;
        return jumpSkill.JumpCheck(Pawn, Pawn.Igm, from.Position, to.Position, ObjectCheck);
    }

    /// <summary>
    /// 判断entity是否为友方单位
    /// </summary>
    public virtual bool FactionCheck(Entity entity)
    {
        return Pawn.FactionCheck(entity) == 1;
    }

    public virtual bool HeightCheck(Node from, Node to)
    {
        int toLayer = Igm.AboveGroundLayer(to.Position);
        int fromLayer = Igm.AboveGroundLayer(from.Position);
        return HeightCheck(fromLayer, toLayer);
    }

    public virtual bool HeightCheck(int fromLayer, int toLayer)
    {
        return toLayer <= fromLayer + climbAbility.IntValue && toLayer >= fromLayer - dropAbility.IntValue;
    }

    public virtual bool ClimbCheck(Node from, Node to)
    {
        return Igm.ClimbCheck(Igm.AboveGroundPosition(from.Position), Igm.AboveGroundPosition(to.Position));
    }
}
