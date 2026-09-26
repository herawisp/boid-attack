using UnityEngine;
using UnityEngine.UI;

public class DungeonDoor : MonoBehaviour {

    //================================================================================================//
    //================================================================================================//

    [Header("Wall Sprites")]
    public DungeonWall Wall;
    public GameObject Arrow;

    Image _image;
    Animator _animator;

    void Awake() {
        _image = GetComponent<Image>();
        _animator = GetComponent<Animator>();
    }

    public void Enable(bool isEnabled) {
        Wall.SetOpen(isEnabled);
        _image.enabled = isEnabled;
        Arrow.SetActive(false);
        _animator.SetBool("IsOpen", isEnabled);
    }
    
    public void OnOpenedFinished() {
        _animator.SetBool("IsOpen", false);
        Arrow.SetActive(true);
    }

    //================================================================================================//
    //================================================================================================//
}
