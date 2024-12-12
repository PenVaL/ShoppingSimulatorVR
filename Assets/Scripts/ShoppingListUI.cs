using UnityEngine;
using TMPro;

public class ShoppingListUI : MonoBehaviour {
    [System.Serializable]
    public struct ShoppingListUISlot {
        public GameObject[] goodsImages;
        public TMP_Text quantityText;
    }

    [SerializeField] private ShoppingListUISlot[] uiSlots;

    public void UpdateShoppingListUI(GameManager.ShoppingListItem[] shoppingList) {
        // Iterate through the UI slots
        for (int i = 0; i < uiSlots.Length; i++) {
            // If we have a shopping list item for this slot
            if (i < shoppingList.Length) {
                // Set the corresponding goods objects active based on the goods type
                for (int j = 0; j < uiSlots[i].goodsImages.Length; j++) {
                    string goodsImageName = uiSlots[i].goodsImages[j].name;
                    GameManager.GoodsType goodsType = (GameManager.GoodsType)System.Enum.Parse(typeof(GameManager.GoodsType), goodsImageName);
                    if (goodsType == shoppingList[i].goodsType) {
                        uiSlots[i].goodsImages[j].SetActive(true);
                    } else {
                        uiSlots[i].goodsImages[j].SetActive(false);
                    }
                }

                // Update the quantity text
                uiSlots[i].quantityText.text = shoppingList[i].quantity.ToString();
            }
            // If we don't have a shopping list item for this slot
            else {
                // Deactivate all goods objects and clear the quantity text
                for (int j = 0; j < uiSlots[i].goodsImages.Length; j++)
                {
                    uiSlots[i].goodsImages[j].SetActive(false);
                }
                uiSlots[i].quantityText.text = string.Empty;
            }
        }
    }
}