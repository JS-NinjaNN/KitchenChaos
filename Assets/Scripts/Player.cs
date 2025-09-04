using System;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent {
    [SerializeField] private float _moveSpeed = 7.0f;
    [SerializeField] private float _rotateSpeed = 10.0f;
    [SerializeField] private GameInput _gameInput;
    [SerializeField] private LayerMask _countersLayerMask;
    [SerializeField] private Transform _kitchenObjectHoldPoint;

    private bool _isWalking;
    private Vector3 _lastInteractDir;
    private BaseCounter _selectedCounter;
    private KitchenObject _kitchenObject;

    public static Player Instance { get; private set; }

    public event EventHandler OnPickedSomething;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

    public class OnSelectedCounterChangedEventArgs : EventArgs {
        public BaseCounter SelectedCounter;
    }

    private void Awake() {
        if (Instance != null && Instance != this) Destroy(gameObject);

        Instance = this;
    }

    private void Start() {
        _gameInput.OnInteractAction += GameInput_OnInteractAction;
        _gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e) {
        if (!GameManager.Instance.IsGamePlaying()) return;
        if (_selectedCounter != null) _selectedCounter.InteractAlternate(this);
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e) {
        if (!GameManager.Instance.IsGamePlaying()) return;
        if (_selectedCounter != null) _selectedCounter.Interact(this);
    }

    private void Update() {
        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking() {
        return _isWalking;
    }

    private void HandleMovement() {
        Vector2 inputVector = _gameInput.GetNormalizedMovementVector();
        Vector3 moveDir = new(inputVector.x, 0f, inputVector.y);

        float moveDistance = _moveSpeed * Time.deltaTime;
        float playerRadius = 0.7f;
        float playerHeight = 2.0f;

        bool canMove = !Physics.CapsuleCast(
            transform.position,
            transform.position + Vector3.up * playerHeight,
            playerRadius,
            moveDir,
            moveDistance
        );

        if (!canMove) {
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;

            canMove = moveDir.x != 0 &&
                !Physics.CapsuleCast(
                    transform.position,
                    transform.position + Vector3.up * playerHeight,
                    playerRadius,
                    moveDirX,
                    moveDistance
                );

            if (canMove)
                moveDir = moveDirX;
            else {
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;

                canMove = moveDir.z != 0 &&
                    !Physics.CapsuleCast(
                        transform.position,
                        transform.position + Vector3.up * playerHeight,
                        playerRadius,
                        moveDirZ,
                        moveDistance
                    );

                if (canMove) moveDir = moveDirZ;
            }
        }

        if (canMove) transform.position += moveDir * moveDistance;

        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * _rotateSpeed);
        _isWalking = moveDir != Vector3.zero;
    }

    private void HandleInteractions() {
        Vector2 inputVector = _gameInput.GetNormalizedMovementVector();
        Vector3 moveDir = new(inputVector.x, 0f, inputVector.y);
        float interactDistance = 2.0f;

        if (moveDir != Vector3.zero) _lastInteractDir = moveDir;

        if (Physics.Raycast(
            transform.position,
            _lastInteractDir,
            out RaycastHit raycastHit,
            interactDistance,
            _countersLayerMask
        )) {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter)) {
                if (baseCounter != _selectedCounter) SetSelectedCounter(baseCounter);
            } else
                SetSelectedCounter(null);
        } else
            SetSelectedCounter(null);
    }

    private void SetSelectedCounter(BaseCounter selectedCounter) {
        _selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(
            this,
            new OnSelectedCounterChangedEventArgs { SelectedCounter = selectedCounter }
        );
    }

    public Transform GetKitchenObjectFollowTransform() {
        return _kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject) {
        _kitchenObject = kitchenObject;

        if (kitchenObject != null) OnPickedSomething?.Invoke(this, EventArgs.Empty);
    }

    public KitchenObject GetKitchenObject() {
        return _kitchenObject;
    }

    public void ClearKitchenObject() {
        _kitchenObject = null;
    }

    public bool HasKitchenObject() {
        return _kitchenObject != null;
    }
}