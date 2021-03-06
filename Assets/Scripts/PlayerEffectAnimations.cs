using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerEffectAnimations : MonoBehaviour
{
    private Animator _anim;
    private PlayerController _playerControllerScript;
    public Button nextLevelButton;
    private float _fillAmount = 0;
    [SerializeField] private Image mask = default;
    public GameObject progress;
    void Awake()
    {
        _anim = GetComponent<Animator>();
        _playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
    }
    private void Update()
    {
        if (_playerControllerScript.jumpValue == 0)
        {
            progress.SetActive(false);
        }
        else
        {
            progress.SetActive(true);
        }
        if (_playerControllerScript.touchDelta.x != 0 && _playerControllerScript.touchDelta.y==0)
        {
            _anim.SetBool("isWalking", true);
        }
        else { _anim.SetBool("isWalking", false); }

        if (_playerControllerScript.canJump == false)
        {
            _anim.SetTrigger("jump");
        }
        if (CrossPlatformInputManager.GetButton("Jump"))
        {
            _anim.SetBool("isWalking", false);
        }
        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            _anim.SetBool("isWalking", false);
        }
        ProgressBar();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Finish"))
        {
            _playerControllerScript.canMove = false;
            nextLevelButton.gameObject.SetActive(true);
        }
    }
    public void ProgressBar()
    {
        _fillAmount = (float)_playerControllerScript.jumpValue / (float)48;
        mask.fillAmount = _fillAmount;
    }
}