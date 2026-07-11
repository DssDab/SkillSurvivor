using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Weapon", story: "try attack with [CurrentWeapon]", category: "Action", id: "0f2faba7661e7d5f9ab469559eb4df67")]
public partial class WeaponAction : Action
{
    [SerializeReference] public BlackboardVariable<WeaponBase> CurrentWeapon;

  
    protected override Status OnUpdate()
    {
        CurrentWeapon.Value.TryAttack();

        return Status.Success;
    }

  
}

