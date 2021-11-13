using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SilhouetteStandState
{
    Up,
    Down,
}

public interface IMovableSilhouette
{
    public void StandSilhouette(SilhouetteStandState standState, float rotateTime);
}
