using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyCard/Card/Prototype")]
public class StaffPrototype : ScriptableObject {
    public int Health;
    public int Attack;
    public Sprite CardImage;
    public String Name;
}
