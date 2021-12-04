// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Player input actions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Playerinputactions : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Playerinputactions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Player input actions"",
    ""maps"": [
        {
            ""name"": ""Player controls"",
            ""id"": ""c65edc16-3167-4af7-b390-692fd6453202"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""PassThrough"",
                    ""id"": ""83d909c3-74b9-482c-b144-e16648d2a2f2"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""AttackDirection"",
                    ""type"": ""PassThrough"",
                    ""id"": ""19de4a60-c9af-411c-b0a7-3bd1266f0d0c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Change weapon"",
                    ""type"": ""Button"",
                    ""id"": ""9387702d-67c5-4596-89e6-ade728be8ec4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""fb916df0-6378-4c51-89de-886498cae81d"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""68871feb-0120-425f-b9a9-5f4664e4c4de"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""df794529-02fd-4955-8c93-9526042c8bcc"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""169ce40a-de2d-4b9c-a94c-fbb7aa03ea37"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""5d05f2a2-4778-47f2-9d43-0d7c01e378a5"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Arrows"",
                    ""id"": ""8a95d18b-2957-407d-826f-3b14830693f4"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AttackDirection"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""097aee6a-3bf5-45fe-91e7-691bf5845c9c"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AttackDirection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""a6da6f7a-5aa6-4445-b59c-c484fb0cd8a9"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AttackDirection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""db707248-c78f-44d9-8e39-b09ab464cd2a"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AttackDirection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""50bf019c-7305-4f45-b164-f82ae92505c0"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AttackDirection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""36ab2d30-b13c-48ad-96f7-1bddfc8c35e0"",
                    ""path"": ""<Keyboard>/t"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Change weapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player controls
        m_Playercontrols = asset.FindActionMap("Player controls", throwIfNotFound: true);
        m_Playercontrols_Move = m_Playercontrols.FindAction("Move", throwIfNotFound: true);
        m_Playercontrols_AttackDirection = m_Playercontrols.FindAction("AttackDirection", throwIfNotFound: true);
        m_Playercontrols_Changeweapon = m_Playercontrols.FindAction("Change weapon", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Player controls
    private readonly InputActionMap m_Playercontrols;
    private IPlayercontrolsActions m_PlayercontrolsActionsCallbackInterface;
    private readonly InputAction m_Playercontrols_Move;
    private readonly InputAction m_Playercontrols_AttackDirection;
    private readonly InputAction m_Playercontrols_Changeweapon;
    public struct PlayercontrolsActions
    {
        private @Playerinputactions m_Wrapper;
        public PlayercontrolsActions(@Playerinputactions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Playercontrols_Move;
        public InputAction @AttackDirection => m_Wrapper.m_Playercontrols_AttackDirection;
        public InputAction @Changeweapon => m_Wrapper.m_Playercontrols_Changeweapon;
        public InputActionMap Get() { return m_Wrapper.m_Playercontrols; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayercontrolsActions set) { return set.Get(); }
        public void SetCallbacks(IPlayercontrolsActions instance)
        {
            if (m_Wrapper.m_PlayercontrolsActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_PlayercontrolsActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_PlayercontrolsActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_PlayercontrolsActionsCallbackInterface.OnMove;
                @AttackDirection.started -= m_Wrapper.m_PlayercontrolsActionsCallbackInterface.OnAttackDirection;
                @AttackDirection.performed -= m_Wrapper.m_PlayercontrolsActionsCallbackInterface.OnAttackDirection;
                @AttackDirection.canceled -= m_Wrapper.m_PlayercontrolsActionsCallbackInterface.OnAttackDirection;
                @Changeweapon.started -= m_Wrapper.m_PlayercontrolsActionsCallbackInterface.OnChangeweapon;
                @Changeweapon.performed -= m_Wrapper.m_PlayercontrolsActionsCallbackInterface.OnChangeweapon;
                @Changeweapon.canceled -= m_Wrapper.m_PlayercontrolsActionsCallbackInterface.OnChangeweapon;
            }
            m_Wrapper.m_PlayercontrolsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @AttackDirection.started += instance.OnAttackDirection;
                @AttackDirection.performed += instance.OnAttackDirection;
                @AttackDirection.canceled += instance.OnAttackDirection;
                @Changeweapon.started += instance.OnChangeweapon;
                @Changeweapon.performed += instance.OnChangeweapon;
                @Changeweapon.canceled += instance.OnChangeweapon;
            }
        }
    }
    public PlayercontrolsActions @Playercontrols => new PlayercontrolsActions(this);
    public interface IPlayercontrolsActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnAttackDirection(InputAction.CallbackContext context);
        void OnChangeweapon(InputAction.CallbackContext context);
    }
}
