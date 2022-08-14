using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DictionarySpecialComparer : IEqualityComparer<Key3D>
{
    public bool Equals(Key3D keyBoxOne, Key3D keyBoxTwo)
    {
        return keyBoxOne.X == keyBoxTwo.X && keyBoxOne.Y == keyBoxTwo.Y && keyBoxOne.Z == keyBoxTwo.Z;
    }

    public int GetHashCode(Key3D keyBox)
    {
        return keyBox.X.GetHashCode() + keyBox.Y.GetHashCode() + keyBox.Z.GetHashCode();
    }


}