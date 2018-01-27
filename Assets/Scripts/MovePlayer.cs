﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour {

    public float speed;
    public float rotSpeed;

    public float angle=0;

    private Vector3 dPos;
    private float hitCountdown = 0;
    private LevelController lvlController;
    private int level;
    private Sprite levelBitmap;
    private int bitmapWidth;
    private int bitmapHeight;
    private Color32[] bitmapColors;
    private Color32 cFloor = new Color32(255, 255, 255, 255);


    private bool enableMovement = true;
    private float trapHitCountdown;

    // Use this for initialization
    void Start () {
        lvlController = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelController>();
        level = lvlController.level;
        levelBitmap = lvlController.levelBitmaps[level];
        bitmapWidth = levelBitmap.texture.width;
        bitmapHeight = levelBitmap.texture.height;
        bitmapColors = levelBitmap.texture.GetPixels32();

        // Now the player object is available, get a reference to the gameobject in Traps-script
        GameObject.FindGameObjectWithTag("Traps").GetComponent<Traps>().getPlayerGO();

    }

    // Update is called once per frame
    void FixedUpdate () {

        float horiz = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");

        angle = horiz * rotSpeed * Time.deltaTime;

        transform.Rotate(new Vector3(0f, 1f, 0f), angle);

        dPos = transform.forward * speed * Time.deltaTime * vert;
        Vector3 pos = transform.position + dPos;

        int ix = (int)pos.x;
        int iz = (int)pos.z;

        if (enableMovement)
        {
            Vector3 rot = transform.rotation.eulerAngles;
            if (rot.y >= 0 && rot.y < 90)
            {
                if (isFloor(ix + 1, iz) && isFloor(ix + 1, iz + 1) && isFloor(ix, iz + 1))
                {
                    transform.position = pos;
                }
            }
            if (rot.y >= 90 && rot.y < 180)
            {
                if (isFloor(ix - 1, iz) && isFloor(ix - 1, iz + 1) && isFloor(ix, iz + 1))
                {
                    transform.position = pos;
                }
            }
            if (rot.y >= 180 && rot.y < 270)
            {
                if (isFloor(ix - 1, iz) && isFloor(ix - 1, iz - 1) && isFloor(ix, iz - 1))
                {
                    transform.position = pos;
                }
            }
            if (rot.y >= 270 && rot.y < 360)
            {
                if (isFloor(ix + 1, iz) && isFloor(ix + 1, iz - 1) && isFloor(ix, iz - 1))
                {
                    transform.position = pos;
                }
            }
        } else
        {
            // movement not enabled.
            trapHitCountdown -= Time.deltaTime;
            if(trapHitCountdown<0)
            {
                enableMovement = true;
                GameObject.FindGameObjectWithTag("PlayerStopMarker").GetComponent<MeshRenderer>().enabled = false;
            }
        }

    }

    void OnTriggerEnter(Collider other) {
        string tag = other.tag;
        if(tag=="Wall") {
            Vector3 pos = transform.position - dPos;
            transform.position = pos;
            hitCountdown = .1f;
        }
    }

    bool isFloor(int x, int z)
    {
        if(x<0 || z<0 || x>=bitmapWidth || z>=bitmapHeight)
        {
            return false;
        }
        return cFloor.Equals(bitmapColors[x + z * bitmapWidth]);
    }

    public void hitByTrap(Trap t, float dist)
    {
        Debug.Log("REMOVE-MovePlayer");
        // get properties of the trap for use!!!
        enableMovement = false;
        GameObject.FindGameObjectWithTag("PlayerStopMarker").GetComponent<MeshRenderer>().enabled = true;
        trapHitCountdown = .5f; // dist; // is this right??? - propably not...

    }
}
