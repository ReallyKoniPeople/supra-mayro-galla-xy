using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Interfaces
{
    public interface IGravitySource
    {
        float GravityStrength { get; }
        List<GravityItem> ItemsInRange { get; }
        Collider[] GravityColliders { get; }
        bool EnableGravity { get; }
    }
}
