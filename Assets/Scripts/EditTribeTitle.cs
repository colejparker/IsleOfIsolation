using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditTribeTitle : MonoBehaviour
{
    [SerializeField] TribeChangeCanvas tribeChangeCanvas;
    [SerializeField] Transform tribeParent;
    public TextMesh text;

    public void OnMouseDown()
    {
        ClickTribe();
    }

    public void ClickTribe()
    {
        Tribe[] tribes = FindObjectsOfType<Tribe>();
        Tribe tribeToReturn = null;
        foreach(Tribe tribe in tribes)
        {
            if (tribe.name == text.text) tribeToReturn = tribe;
        }
        if (tribeToReturn != null)
        {
            TribeChangeCanvas tcc = Instantiate(tribeChangeCanvas, Vector3.zero, Quaternion.identity);
            tcc.CreateFromTribe(tribeToReturn, this);
        }
    }

}
