using UnityEngine;

public class OrderData
{
    [SerializeField] private EOrderType orderType = EOrderType.NONE;
    [SerializeField] private Vector3 orderDestination;
    [SerializeField] private ITargetableObject orderTarget;

    //Consturcteur de déplacement
    public OrderData(EOrderType type, Vector3 destination)
    {
        orderType = type;
        orderDestination = destination;
    }
    //Constructeur de l'ordre d'attaque
    public OrderData(EOrderType type, ITargetableObject target)
    {
        orderType = type;
        orderTarget = target;
    }

    //Constructeur d'ordre stop
    public OrderData(EOrderType type)
    {
        orderType = type;
    }

    public EOrderType OrderType => orderType;
    public Vector3 OrderDestination => orderDestination;
    public ITargetableObject OrderTarget => orderTarget;
}

public enum EOrderType
{
    NONE,
    IDLE,
    MOVETO,
    ATTACK,
    AUTONOMOUS,
    STOP,
}