// GENERATED AUTOMATICALLY FROM 'Assets/Controls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Controls : IInputActionCollection, IDisposable
{
    private InputActionAsset asset;
    public @Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""Vehicle"",
            ""id"": ""f2cf127d-7125-4a99-ad13-4526d5704cea"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""b32da602-5443-423a-9037-b282c3301617"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""253c2004-7724-4779-a560-3299a62640e8"",
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
                    ""id"": ""69a269f3-8e08-4906-9328-f0eae9088d70"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""SinglePlayer"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""80a28f06-15c2-43f6-94ff-80cc62d0ad7c"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""SinglePlayer"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""10e373c9-dcd2-421e-8a16-a6621793dcad"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""SinglePlayer"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""16a851fc-36c9-4b31-a247-1ec49fd5a691"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""SinglePlayer"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""ArrowKeys"",
                    ""id"": ""15f24981-93dd-45bf-8918-af64afe28bf9"",
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
                    ""id"": ""de923a2e-47c5-440f-970c-2102b2241345"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""SinglePlayer"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""36a06994-6854-4a06-9ec8-f2571ad49b57"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""SinglePlayer"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""2aceefc4-dd81-4659-bcce-c9ba927b2ff1"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""SinglePlayer"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""bc81b928-2e44-4bc0-b46e-0643debd80c6"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""SinglePlayer"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Throttle"",
                    ""id"": ""9ed86b02-96c1-4756-b7fc-91eb2e3efa9e"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""83d62c27-1c49-41f1-bea2-a104a3ff4e54"",
                    ""path"": ""<Keyboard>/leftCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""SinglePlayer"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""1bddd1db-e84e-495a-81f4-bdbc51a6df73"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""SinglePlayer"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""SinglePlayer"",
            ""bindingGroup"": ""SinglePlayer"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""TwoPlayer"",
            ""bindingGroup"": ""TwoPlayer"",
            ""devices"": []
        }
    ]
}");
        // Vehicle
        m_Vehicle = asset.FindActionMap("Vehicle", throwIfNotFound: true);
        m_Vehicle_Move = m_Vehicle.FindAction("Move", throwIfNotFound: true);
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

    // Vehicle
    private readonly InputActionMap m_Vehicle;
    private IVehicleActions m_VehicleActionsCallbackInterface;
    private readonly InputAction m_Vehicle_Move;
    public struct VehicleActions
    {
        private @Controls m_Wrapper;
        public VehicleActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Vehicle_Move;
        public InputActionMap Get() { return m_Wrapper.m_Vehicle; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(VehicleActions set) { return set.Get(); }
        public void SetCallbacks(IVehicleActions instance)
        {
            if (m_Wrapper.m_VehicleActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_VehicleActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_VehicleActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_VehicleActionsCallbackInterface.OnMove;
            }
            m_Wrapper.m_VehicleActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
            }
        }
    }
    public VehicleActions @Vehicle => new VehicleActions(this);
    private int m_SinglePlayerSchemeIndex = -1;
    public InputControlScheme SinglePlayerScheme
    {
        get
        {
            if (m_SinglePlayerSchemeIndex == -1) m_SinglePlayerSchemeIndex = asset.FindControlSchemeIndex("SinglePlayer");
            return asset.controlSchemes[m_SinglePlayerSchemeIndex];
        }
    }
    private int m_TwoPlayerSchemeIndex = -1;
    public InputControlScheme TwoPlayerScheme
    {
        get
        {
            if (m_TwoPlayerSchemeIndex == -1) m_TwoPlayerSchemeIndex = asset.FindControlSchemeIndex("TwoPlayer");
            return asset.controlSchemes[m_TwoPlayerSchemeIndex];
        }
    }
    public interface IVehicleActions
    {
        void OnMove(InputAction.CallbackContext context);
    }
}
