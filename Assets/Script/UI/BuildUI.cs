using System;
using UnityEngine;
using UnityEngine.UIElements;

public class BuildUI : MonoBehaviour
{
    [SerializeField] private ABuildClass[] prefabArray = null;

    [SerializeField] private UIDocument buildListUI = null;
    [SerializeField] private VisualTreeAsset buildSlot = null;
    private ScrollView buildGrid = null;

    private void OnEnable()
    {
        buildGrid = buildListUI.rootVisualElement.Q<ScrollView>("BuildGrid");
        RebuildGrid();

    }

    private void RebuildGrid()
    {
        buildGrid.Clear();

        foreach (ABuildClass prefab in prefabArray)
        {
            VisualElement slot = buildSlot.Instantiate();

            slot.Q<Image>("PrefabIcon").sprite = prefab.BuildData.BuildingIcon;
            slot.Q<Label>("PrefabName").text = prefab.BuildData.BuildName;

            slot.Q<Button>("PrefabButton").clicked += () => BuildSelected(prefab);

            buildGrid.Add(slot);
        }
    }

    public void BuildSelected(ABuildClass prefab)
    {
        PlayerInteraction.Instance.ChangePlayerState(EPlayerState.CONSTURCTION);

        BuilderSystemManager.Instance.SelectedBuild(prefab);
    }
}
