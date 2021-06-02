using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour
{
    private Player player;
    private PlayerWeapon weapon;
    private ProjectileSwitch projectileSwitch;
    public static bool jump = false;
    public bool fire = false;
    public bool nextProjectile = false;
    public bool prevProjectile = false;
    private Animator anim;
    public bool usesObject = false;
    bool nextScene = false;
    bool firstScene = false;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        player = GetComponent<Player>();
        weapon = player.transform.Find("Body").GetChild(0).GetComponent<PlayerWeapon>();
        projectileSwitch = player.transform.Find("Body").GetChild(0).GetComponent<ProjectileSwitch>();
    }

    private void Update()
    {
        if (!GamePause.isPaused)
        {
            nextScene = Input.GetKeyDown(KeyCode.N);
            if (nextScene)
            {
                SceneLoader.LoadNScene();
            }

            usesObject = Input.GetKeyDown(KeyCode.F);
            jump = Input.GetButtonDown("Jump");
            if (jump)
            {
                player.Jump();
                anim.SetBool("isJumping", true);
            }
            jump = false;
            fire = Input.GetMouseButton(0);
            weapon.Shoot(fire);
            nextProjectile = Input.GetKeyDown(KeyCode.E);
            if (nextProjectile)
            {
                projectileSwitch.SwitchToNext();
            }
            else
            {
                prevProjectile = Input.GetKeyDown(KeyCode.Q);
                if (prevProjectile)
                {
                    projectileSwitch.SwitchToPrev();
                }
            }

        }
    }


    private void FixedUpdate()
    {
        if (!GamePause.isPaused)
        {
            float h = Input.GetAxis("Horizontal");
            if (h != 0)
            {
                anim.SetBool("isRunning", true);
            }
            else
            {
                anim.SetBool("isRunning", false);
            }
            player.Move(h);
        }
    }
}
