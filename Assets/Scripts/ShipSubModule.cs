using UnityEngine;

public abstract class ShipSubModule : MonoBehaviour
{
    protected bool wasGrabbed = false;
    
    public virtual void HandleModuleGrabbed()
    {
        wasGrabbed = true;
    }
}
