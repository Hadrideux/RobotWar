public interface ISelectable
{
    public ESelectableType SelectableType { get; }

    public void Select();
    public void Deselect();

}

public enum ESelectableType
{
    NONE,
    UNIT,
    BUILDING,
}
