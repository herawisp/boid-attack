using UnityEngine;
using UnityEngine.UI;

public class DungeonDoor : MonoBehaviour {

    //================================================================================================//
    //================================================================================================//

    [Header("Wall Sprites")]
    public Image Wall;
    public Sprite OpenWallSprite;
    public Sprite ClosedWallSprite;
    public GameObject Arrow;

    Image _image;
    Animator _animator;

    void Awake() {
        _image = GetComponent<Image>();
        _animator = GetComponent<Animator>();
    }

    public void Enable(bool isEnabled) {
        Wall.sprite = isEnabled ? OpenWallSprite : ClosedWallSprite;
        _image.enabled = isEnabled;

        if (isEnabled) {
            SetAnimationOpen();
            Arrow.SetActive(false);
        } else {
            Arrow.SetActive(false);
        }
    }
    
    public void OnOpenedFinished() {
        Arrow.SetActive(true);
        SetAnimationIdle();
    }

    //================================================================================================//
    //================================================================================================//
    
    void SetAnimationIdle() {
        _animator.SetBool("IsOpened", false);
        _animator.SetBool("IsClosed", false);
        _animator.SetBool("IsIdle", true);
    }

    void SetAnimationClose() {
        _animator.SetBool("IsOpened", false);
        _animator.SetBool("IsIdle", false);
        _animator.SetBool("IsClosed", true);
    }

    void SetAnimationOpen() {
        _animator.SetBool("IsClosed", false);
        _animator.SetBool("IsIdle", false);
        _animator.SetBool("IsOpened", true);
    }

    //================================================================================================//
    //================================================================================================//
}
