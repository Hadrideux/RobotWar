using UnityEngine;

public class OrderData
{
    private EOrderType orderType = EOrderType.NONE;
    private Vector3 orderDestination;
    private ISelectable orderTarget;

    //Consturcteur de déplacement
    public OrderData(EOrderType type, Vector3 destination)
    {
        orderType = type;
        orderDestination = destination;
    }
    //Constructeur de l'ordre d'attaque
    public OrderData(EOrderType type, ISelectable target)
    {
        orderType = type;
        orderTarget = target;
    }

    //Constructeur d'ordre stop
    public OrderData()
    {
        orderType = EOrderType.STOP;
    }

    public EOrderType OrderType => orderType;
    public Vector3 OrderDestination => orderDestination;
    public ISelectable OrderTarget => orderTarget;
}

public enum EOrderType
{
    NONE,
    MOVETO,
    ATTACK,
    STOP,
}