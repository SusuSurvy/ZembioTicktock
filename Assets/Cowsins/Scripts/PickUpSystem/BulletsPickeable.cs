using System.Collections;
using UnityEngine;
namespace cowsins {
public class BulletsPickeable : Pickeable
{
    [Tooltip("How many bullets you will get"),SerializeField] private int amountOfBullets;

    [SerializeField] private Sprite bulletsIcon;

    [SerializeField] private GameObject bulletsGraphics;

    [HideInInspector] public int currentBullets, totalBullets;

    private GameObject render;

    public override void Start()
    {
        image.sprite = bulletsIcon;
        Destroy(graphics.transform.GetChild(0).gameObject);
        render = Instantiate(bulletsGraphics, graphics);
        base.Start(); 
    }
    public override void Interact()
    {
        if (player.GetComponent<WeaponController>().weapon == null) return;
        base.Interact(); 
        player.GetComponent<WeaponController>().id.totalBullets += amountOfBullets;
        //Destroy(this.gameObject);
        StartCoroutine(DelayedProgressUpdate());
    }
    
    IEnumerator DelayedProgressUpdate()
    {
        graphics.gameObject.SetActive(false);
        render.gameObject.SetActive(false);
        image.gameObject.SetActive(false);
        GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(300f);
        graphics.gameObject.SetActive(true);
        render.gameObject.SetActive(true);
        image.gameObject.SetActive(true);
        GetComponent<BoxCollider>().enabled = true;
    }

    
}
}