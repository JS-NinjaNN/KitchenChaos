using System;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-100)] // гарантируем, что Awake сработает пораньше
public class GameInput : MonoBehaviour {
    private PlayerInputActions _playerInputActions;

    private const string PLAYER_PREFS_BINDINGS = "InputBindings";

    public static GameInput Instance { get; private set; }

    public event Action<Binding, bool> OnRebindStateChanged;
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event Action OnPauseAction;

    public enum Binding {
        MoveUp,
        MoveDown,
        MoveLeft,
        MoveRight,
        Interact,
        InteractAlternate
    }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _playerInputActions = new PlayerInputActions();

        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
            _playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));

        _playerInputActions.Player.Enable();
        _playerInputActions.Player.Interact.performed += Interact_performed;
        _playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
        _playerInputActions.Player.Pause.performed += Pause_performed;
    }

    private void OnDestroy() {
        _playerInputActions.Player.Interact.performed -= Interact_performed;
        _playerInputActions.Player.InteractAlternate.performed -= InteractAlternate_performed;
        _playerInputActions.Player.Pause.performed -= Pause_performed;

        _playerInputActions.Dispose();
    }

    private void Pause_performed(InputAction.CallbackContext obj) {
        OnPauseAction?.Invoke();
    }

    private void Interact_performed(InputAction.CallbackContext obj) {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternate_performed(InputAction.CallbackContext obj) {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetNormalizedMovementVector() {
        Vector2 inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;
        return inputVector;
    }

    public string GetBindingText(Binding binding) {
        return binding switch {
            Binding.Interact => _playerInputActions.Player.Interact.bindings[0].ToDisplayString(),
            Binding.MoveUp => _playerInputActions.Player.Move.bindings[1].ToDisplayString(),
            Binding.MoveDown => _playerInputActions.Player.Move.bindings[2].ToDisplayString(),
            Binding.MoveLeft => _playerInputActions.Player.Move.bindings[3].ToDisplayString(),
            Binding.MoveRight => _playerInputActions.Player.Move.bindings[4].ToDisplayString(),
            Binding.InteractAlternate => _playerInputActions.Player.InteractAlternate.bindings[0].ToDisplayString(),
            _ => throw new ArgumentOutOfRangeException(nameof(binding), binding, null)
        };
    }

    public void RebindBinding(Binding binding) {
        OnRebindStateChanged?.Invoke(binding, true);

        _playerInputActions.Player.Disable();

        _playerInputActions.Player
                           .Move
                           .PerformInteractiveRebinding(1)
                           .WithCancelingThrough("<Keyboard>/escape")
                           .WithControlsExcluding("Mouse")
                           .OnComplete(callback => {
                                   callback.Dispose();
                                   _playerInputActions.Player.Enable();
                                   OnRebindStateChanged?.Invoke(binding, false);

                                   PlayerPrefs.SetString(
                                       PLAYER_PREFS_BINDINGS,
                                       _playerInputActions.SaveBindingOverridesAsJson()
                                   );

                                   PlayerPrefs.Save();
                               }
                           )
                           .OnCancel(callback => {
                                   callback.Dispose();
                                   _playerInputActions.Player.Enable();
                                   OnRebindStateChanged?.Invoke(binding, false);
                               }
                           )
                           .Start();
    }
}