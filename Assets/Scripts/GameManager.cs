using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }

    public enum GoodsType { None, Cashier, ShoppingBasket, 
        Plate, Yogurt, CoffeeBox, Chips, Coke, 
        Tomato, Carrot, Banana, Apple, Eggplant,
        Donut, Sandwich, Milk, Fanta,
        Salmon, Fish, Syrup, PeanutButter,
        Exit
    }

    [System.Serializable]
    public struct ShoppingListItem {
        public GoodsType goodsType;
        public int quantity;
    }

    public List<ShoppingListItem> shoppingList;
    private ShoppingListUI shoppingListUI;
    private GuideManager guideManager;
    private StocksManager[] stockManagers;

    [SerializeField] private GameObject player;

    [SerializeField] private GameObject dollarSign;
    [SerializeField] private AudioClip doorBellSFX, successCheckoutSFX, failureCheckoutSFX;
    [SerializeField] private AudioSource storeAudioSource;

    private bool isCheckoutAllowed = true;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    void Start() {
        guideManager = FindObjectOfType<GuideManager>(); // Find GuideManager in the scene
        shoppingListUI = FindObjectOfType<ShoppingListUI>(); // Find the ShoppingListUI in the scene   
        stockManagers = FindObjectsOfType<StocksManager>(); // Find all StocksManager in the scene

        ResetScene();
    }

    private void GenerateShoppingList() {
        shoppingList = new List<ShoppingListItem>();
        List<GoodsType> usedGoodsTypes = new List<GoodsType>();

        // Generate 1 to 3 goods types to be shopped
        int numGoodsTypes = Random.Range(1, 4);
        for (int i = 0; i < numGoodsTypes; i++) {
            GoodsType goodsType;
            do {
                goodsType = (GoodsType)Random.Range(1, System.Enum.GetNames(typeof(GoodsType)).Length - 1);
            } while (goodsType == GoodsType.None || goodsType == GoodsType.Cashier || goodsType == GoodsType.ShoppingBasket || usedGoodsTypes.Contains(goodsType));

            // Add the new goods type to the list of used goods types
            usedGoodsTypes.Add(goodsType);

            // Generate 1 to 3 items of the selected goods type
            int numItems = Random.Range(1, 4);
            shoppingList.Add(new ShoppingListItem { goodsType = goodsType, quantity = numItems });
        }

        // Debug the shopping list
        foreach (var item in shoppingList) {
            Debug.Log($"Shopping List: {item.goodsType} x {item.quantity}");
        }
    }

    public GameManager.ShoppingListItem GetNextShoppingListItem() {
        foreach (GameManager.ShoppingListItem item in shoppingList) {
            if (item.quantity > 0) {
                return item;
            }
        }

        // If the shopping list is clear, return a new ShoppingListItem with goodsType None
        return new GameManager.ShoppingListItem { goodsType = GameManager.GoodsType.None, quantity = 0 };
    }

    public bool DecrementShoppingListItem(Goods goods) {
        GameManager.GoodsType collisionType = goods.goodsType;
        for (int i = 0; i < shoppingList.Count; i++) {
            if (shoppingList[i].goodsType == collisionType && shoppingList[i].quantity > 0) {
                shoppingList[i] = new ShoppingListItem { goodsType = shoppingList[i].goodsType, quantity = shoppingList[i].quantity - 1 };
                Debug.Log($"Added {shoppingList[i].goodsType} to the basket. {shoppingList[i].quantity} left to collect.");
                Destroy(goods.gameObject);
                shoppingListUI.UpdateShoppingListUI(shoppingList.ToArray()); // update shopping list UI

                if (guideManager.needGuides) // guidance for player
                    guideManager.UpdateGuidanceFlags();

                return true;
            }
        }
        Debug.Log($"Cannot add {collisionType} to the basket. It's not on the shopping list.");
        return false;
    }

    public void CheckOut() {
        if (!isCheckoutAllowed){
            // Checkout is not allowed, return without doing anything
            return;
        }

        bool allItemsShopped = true;
        foreach (var item in shoppingList) { 
            if (item.quantity > 0) { // if quantity larger than 0, fail in checkout
                allItemsShopped = false;
                break;
            }
        }

        if (allItemsShopped) {
            Debug.Log("You did it!");
            isCheckoutAllowed = false; // Disable checkout
            StartCoroutine(WaitAndResetScene(7f)); // Wait for a few seconds

        } else {
            Debug.Log("You still need to shop for:");
            storeAudioSource.PlayOneShot(failureCheckoutSFX); // SFX
            foreach (var item in shoppingList) {
                if (item.quantity > 0) {
                    Debug.Log($"{item.goodsType} x {item.quantity}");
                }
            }
        }
    }

    private IEnumerator WaitAndResetScene(float waitTime) {
        storeAudioSource.PlayOneShot(successCheckoutSFX); // SFX
        dollarSign.SetActive(true); // VFX

        yield return new WaitForSeconds(waitTime);
        ResetScene();
        isCheckoutAllowed = true; // Re-enable checkout
    }

    private void ResetScene() { // Reset Everything in scene 
        dollarSign.SetActive(false); // Hide the dollar sign

        foreach (StocksManager sm in stockManagers) // RestoreGoods
            sm.RestoreGoods();
        
        player.transform.position = Vector3.zero; // Reset player position to world 0,0,0
        player.transform.rotation = Quaternion.identity; // Reset player rotation to 0,0,0
        // Reset shoppingBasket position
        ShoppingBasket basket = FindObjectOfType<ShoppingBasket>();
        basket.ResetShoppingBasketPosition(); 

        GenerateShoppingList(); // generate new shopping list
        shoppingListUI.UpdateShoppingListUI(shoppingList.ToArray()); // update shopping list UI

        storeAudioSource.PlayOneShot(doorBellSFX); // SFX
        if (guideManager.needGuides) // guidance for player
            guideManager.UpdateGuidanceFlags();
    }

    public void SkipCurrentTask() {
        GameManager.ShoppingListItem nextItem = GetNextShoppingListItem();
        if (nextItem.goodsType != GameManager.GoodsType.None) {
            // skip the next item to be shopped
            nextItem = new GameManager.ShoppingListItem { goodsType = nextItem.goodsType, quantity = nextItem.quantity - 1 };

            // Update the shopping list
            for (int i = 0; i < shoppingList.Count; i++) {
                if (shoppingList[i].goodsType == nextItem.goodsType) {
                    shoppingList[i] = nextItem;
                    break;
                }
            }
            shoppingListUI.UpdateShoppingListUI(shoppingList.ToArray()); // Update the shopping list UI

            // positive feedback from shoppingBasket
            ShoppingBasket basket = FindObjectOfType<ShoppingBasket>();
            basket.ProvideFeedback(true);

            if (guideManager.needGuides) // next guidance for player
                guideManager.UpdateGuidanceFlags();
        } else { // If the shopping list is clear, call CheckOut()
            CheckOut();
        }
    }
}