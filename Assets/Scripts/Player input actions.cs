//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.2.0
//     from Assets/Scripts/Player input actions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @Playerinputactions : IInputActionCollection2, IDisposable
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
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""AttackDirection"",
                    ""type"": ""PassThrough"",
                    ""id"": ""19de4a60-c9af-411c-b0a7-3bd1266f0d0c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Change weapon"",
                    ""type"": ""Button"",
                    ""id"": ""9387702d-67c5-4596-89e6-ade728be8ec4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""EquipShield"",
                    ""type"": ""Button"",
                    ""id"": ""b50bbe8a-e6c6-4e26-bd34-c645ecc0f144"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""f26cb684-f167-4b05-b0cf-4a58ed335411"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""EquipPickAxe"",
                    ""type"": ""Button"",
                    ""id"": ""f664b65b-9f2e-47ff-975a-2bcbdb4c708c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
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
                    ""name"": ""Gamepad joystick"",
                    ""id"": ""b7f6ed60-7175-4c80-9a87-22d829169a25"",
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
                    ""id"": ""1ce7d99a-dbf8-46c5-a0d4-b8b65f87088c"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""a6d7ff44-7c1d-4afe-b642-6529d8b3ac18"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""6d9cbb02-07ae-448b-afad-6ea596c8d539"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""29b4f8ab-a906-4527-b026-fa7c98e859a1"",
                    ""path"": ""<Gamepad>/leftStick/right"",
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
                    ""name"": ""right joystick"",
                    ""id"": ""d14b87cb-aeb8-453c-925e-e3581566f902"",
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
                    ""id"": ""4a6531b1-3d73-49a6-b929-ed26fdbdf872"",
                    ""path"": ""<Gamepad>/rightStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AttackDirection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""c8349a78-0823-4e80-8cde-6665674ac19a"",
                    ""path"": ""<Gamepad>/rightStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AttackDirection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""37d01a99-e5d0-4e0c-8ae9-6c3c2f0e900d"",
                    ""path"": ""<Gamepad>/rightStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AttackDirection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""8b75e950-9000-4a5a-8256-8050a1ebac1e"",
                    ""path"": ""<Gamepad>/rightStick/right"",
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
                },
                {
                    ""name"": """",
                    ""id"": ""a203b16e-bbee-4679-9bb7-35e714a189a0"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Change weapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""657fbd83-3e10-4a79-9c9e-c87c26c25401"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""EquipShield"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""31cf198b-62d3-477e-8bc5-af6da1c9f5f5"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""EquipShield"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""803eb45f-4e2f-43ba-b1d8-ff0b20788087"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""07c3eaa7-5dd5-4c87-b91c-43ab2bd65e6b"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b32d1151-83e6-4cd5-8627-176c6f3956b8"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""EquipPickAxe"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""40255cd2-fc33-49f3-a890-32dc75f09338"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""EquipPickAxe"",
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
        m_Playercontrols_EquipShield = m_Playercontrols.FindAction("EquipShield", throwIfNotFound: true);
        m_Playercontrols_Attack = m_Playercontrols.FindAction("Attack", throwIfNotFound: true);
        m_Playercontrols_EquipPickAxe = m_Playercontrols.FindAction("EquipPickAxe", throwIfNotFound: true);
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
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Player controls
    private readonly InputActionMap m_Playercontrols;
    private IPlayercontrolsActions m_PlayercontrolsActionsCallbackInterface;
    private readonly InputAction m_Playercontrols_Move;
    private readonly InputAction m_Playercontrols_AttackDirection;
    private readonly InputAction m_Playercontrols_Changeweapon;
    private readonly InputAction m_Playercontrols_EquipShield;
    private readonly InputAction m_Playercontrols_Attack;
    private readonly InputAction m_Playercontrols_EquipPickAxe;
    public struct PlayercontrolsActions
    {
        private @Playerinputactions m_Wrapper;
        public PlayercontrolsActions(@Playerinputactions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Playercontrols_Move;
        public InputAction @AttackDirection => m_Wrapper.m_Playercontrols_AttackDirection;
        public InputAction @Changeweapon => m_Wrapper.m_Playercontrols_Changeweapon;
        public InputAction @EquipShield => m_Wrapper.m_Playercontrols_EquipShield;
        public InputAction @Attack => m_Wrapper.m_Playercontrols_Attack;
        public InputAction @EquipPickAxe => m_Wrapper.m_Playercontrols_EquipPickAxe;
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
                @EquipShield.started -= m_Wrapper.m_PlayercontrolsActionsCallbackInterface.OnEquipShield;
                @EquipShield.performed -= m_Wrapper.m_PlayercontrolsActionsCallbackInterface.OnEquipShield;
                @EquipShield.canceled -= m_Wrapper.m_PlayercontrolsActionsCallbackInterface.OnEquipShield;
                @Attack.started -= m_Wrapper.m_PlayercontrolsActionsCallbackInterface.OnAttack;
                @Attack.performed -= m_Wrapper.m_PlayercontrolsActionsCallbackInterface.OnAttack;
                @Attack.canceled -= m_Wrapper.m_PlayercontrolsActionsCallbackInterface.OnAttack;
                @EquipPickAxe.started -= m_Wrapper.m_PlayercontrolsActionsCallbackInterface.OnEquipPickAxe;
                @EquipPickAxe.performed -= m_Wrapper.m_PlayercontrolsActionsCallbackInterface.OnEquipPickAxe;
                @EquipPickAxe.canceled -= m_Wrapper.m_PlayercontrolsActionsCallbackInterface.OnEquipPickAxe;
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
                @EquipShield.started += instance.OnEquipShield;
                @EquipShield.performed += instance.OnEquipShield;
                @EquipShield.canceled += instance.OnEquipShield;
                @Attack.started += instance.OnAttack;
                @Attack.performed += instance.OnAttack;
                @Attack.canceled += instance.OnAttack;
                @EquipPickAxe.started += instance.OnEquipPickAxe;
                @EquipPickAxe.performed += instance.OnEquipPickAxe;
                @EquipPickAxe.canceled += instance.OnEquipPickAxe;
            }
        }
    }
    public PlayercontrolsActions @Playercontrols => new PlayercontrolsActions(this);
    public interface IPlayercontrolsActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnAttackDirection(InputAction.CallbackContext context);
        void OnChangeweapon(InputAction.CallbackContext context);
        void OnEquipShield(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
        void OnEquipPickAxe(InputAction.CallbackContext context);
    }
}
