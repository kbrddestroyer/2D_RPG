using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickable {
    public bool hint { get; set; }
    public void Pickup();
}
