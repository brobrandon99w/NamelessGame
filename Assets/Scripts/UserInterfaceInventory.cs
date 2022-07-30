using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using TMPro;

public class UserInterfaceInventory : MonoBehaviour
{
    private Inventory inventory;
    private Transform itemSlotContainer;
    private Transform itemSlotTemplate;
    private Player player;

    private void Awake()
    {
        itemSlotContainer = transform.Find("itemSlotContainer");
        itemSlotTemplate = itemSlotContainer.Find("itemSlotTemplate");
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
    }

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;

        inventory.OnItemListChanged += Inventory_OnItemListChanged;

        RefreshInventoryItems();
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        RefreshInventoryItems();
    }

    private void RefreshInventoryItems()
    {
        foreach (Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplate) continue;
            Destroy(child.gameObject);
        }
        int y = 0;
        float itemSlotCellSize = 55f;
        foreach (Item item in inventory.GetItemList())
        {
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);

            itemSlotRectTransform.GetComponent<Button_UI>().ClickFunc = () =>
            {
                // Use item
                inventory.UseItem(item);
            };
            itemSlotRectTransform.GetComponent<Button_UI>().MouseRightClickFunc = () =>
            {
                // Drop item
                Item duplicateItem = new Item { itemType = item.itemType, amount = item.amount };
                inventory.RemoveItem(item);
                ItemWorld.DropItem(player.GetPosition(), duplicateItem);
            };
            // Set the tooltips
            switch (item.itemType)
            {
                case Item.ItemType.healthDrink:
                    itemSlotRectTransform.GetComponent<Button_UI>().MouseOverOnceFunc = () => TooltipScreenSpaceUI.ShowTooltip_Static("<color=#00ff00>Health Drink</color>\nRestores a small amount of health.");
                    itemSlotRectTransform.GetComponent<Button_UI>().MouseOutOnceFunc = () => TooltipScreenSpaceUI.HideTooltip_Static();
                    break;
                case Item.ItemType.pistol:
                    itemSlotRectTransform.GetComponent<Button_UI>().MouseOverOnceFunc = () => TooltipScreenSpaceUI.ShowTooltip_Static("<color=#00ff00>Pistol</color>\nStandard M1911A1 with a 7-round .45 ACP magazine.\n" +
                        "The weapon is easy to handle, and does a small amount of damage.");
                    itemSlotRectTransform.GetComponent<Button_UI>().MouseOutOnceFunc = () => TooltipScreenSpaceUI.HideTooltip_Static();
                    break;
                case Item.ItemType.flashlightOn:
                    itemSlotRectTransform.GetComponent<Button_UI>().MouseOverOnceFunc = () => TooltipScreenSpaceUI.ShowTooltip_Static("<color=#00ff00>Flashlight</color>\nI feel safe having this with me.");
                    itemSlotRectTransform.GetComponent<Button_UI>().MouseOutOnceFunc = () => TooltipScreenSpaceUI.HideTooltip_Static();
                    break;
                case Item.ItemType.flashlightOff:
                    itemSlotRectTransform.GetComponent<Button_UI>().MouseOverOnceFunc = () => TooltipScreenSpaceUI.ShowTooltip_Static("<color=#00ff00>Flashlight</color>\nIt's out of battery.");
                    itemSlotRectTransform.GetComponent<Button_UI>().MouseOutOnceFunc = () => TooltipScreenSpaceUI.HideTooltip_Static();
                    break;
                case Item.ItemType.pistolAmmo:
                    itemSlotRectTransform.GetComponent<Button_UI>().MouseOverOnceFunc = () => TooltipScreenSpaceUI.ShowTooltip_Static("<color=#00ff00>Pistol Ammo</color>\n.45 ACP FMJ");
                    itemSlotRectTransform.GetComponent<Button_UI>().MouseOutOnceFunc = () => TooltipScreenSpaceUI.HideTooltip_Static();
                    break;
            }

                    itemSlotRectTransform.anchoredPosition = new Vector2(0, y * itemSlotCellSize);
            Image image = itemSlotRectTransform.Find("Image").GetComponent<Image>();
            image.sprite = item.GetSprite();

            TextMeshProUGUI uiText = itemSlotRectTransform.Find("amountText").GetComponent<TextMeshProUGUI>();
            if (item.amount > 1)
            {
                uiText.SetText(item.amount.ToString());
            }
            else
            {
                uiText.SetText("");
            }
            y++;

        }

    }
}
