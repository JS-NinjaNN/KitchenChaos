public class DeliveryCounter : BaseCounter {
    public static DeliveryCounter Instance { get; private set; }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public override void Interact(Player player) {
        if (!player.HasKitchenObject()) return;

        if (!player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) return;
        DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);
        player.GetKitchenObject().DestroySelf();
    }
}