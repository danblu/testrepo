﻿using UnityEngine;
using System.Collections;

public class Camera_Controller : MonoBehaviour {

    int HOR_BORDER = 40;
    int VER_BORDER = 20;
	float HOR_SPD = .2f;
	float VER_SPD = .4f;
    int CAMERA_HEIGHT = 20;
    float rotationAmount = 0;

    Game_Controller controller;// = Game_Controller.controller;

	void OnGUI(){
		if (GUI.Button (new Rect (10, 100, 100, 30), "Save")) {
			Game_Controller.controller.Save();
		}
		if (GUI.Button (new Rect (10, 140, 100, 30), "Load")) {
			Game_Controller.controller.Save();
		}
		if (GUI.Button (new Rect (10, Screen.height-160, 50, 30), "Prev")) {
			Game_Controller.controller.curr_scenario.PrevPlayer();
		}
		if (GUI.Button (new Rect (70, Screen.height-160, 50, 30), "Next")) {
			Game_Controller.controller.curr_scenario.NextPlayer();
		}

        //print (controller.curr_scenario.curr_player.GetComponent<Character_Script> ().character_name);
        if (controller.curr_scenario.curr_player != null)
        {
            GUI.TextArea(new Rect(10, Screen.height - 120, 200, 110), controller.curr_scenario.curr_player.GetComponent<Character_Script>().character_name + "\n" +
                  "AU: " + controller.curr_scenario.curr_player.GetComponent<Character_Script>().aura_curr + " / " + controller.curr_scenario.curr_player.GetComponent<Character_Script>().aura_max + "\n" +
                  "AP: " + (controller.curr_scenario.curr_player.GetComponent<Character_Script>().action_curr - controller.curr_scenario.curr_player.GetComponent<Character_Script>().action_cost) + " / " + controller.curr_scenario.curr_player.GetComponent<Character_Script>().action_max + "\n" +
                  "Can: " + controller.curr_scenario.curr_player.GetComponent<Character_Script>().canister_curr + " / " + controller.curr_scenario.curr_player.GetComponent<Character_Script>().canister_max + "\n" +
                  "Str: " + controller.curr_scenario.curr_player.GetComponent<Character_Script>().strength + "   Crd: " + controller.curr_scenario.curr_player.GetComponent<Character_Script>().coordination + "    Spt: " + controller.curr_scenario.curr_player.GetComponent<Character_Script>().spirit + "\n" +
                  "Dex: " + controller.curr_scenario.curr_player.GetComponent<Character_Script>().dexterity + "   Vit: " + controller.curr_scenario.curr_player.GetComponent<Character_Script>().vitality + "\n" +
                  "Wep: " + controller.curr_scenario.curr_player.GetComponent<Character_Script>().weapon.name + "   Armor: " + controller.curr_scenario.curr_player.GetComponent<Character_Script>().armor.name);

            if (controller.curr_scenario.highlighted_player != null)
            {
                GUI.TextArea(new Rect(220, Screen.height - 120, 200, 110), controller.curr_scenario.highlighted_player.GetComponent<Character_Script>().character_name + "\n" +
                              "AU: " + controller.curr_scenario.highlighted_player.GetComponent<Character_Script>().aura_curr + " / " + controller.curr_scenario.highlighted_player.GetComponent<Character_Script>().aura_max + "\n" +
                              "AP: " + controller.curr_scenario.highlighted_player.GetComponent<Character_Script>().action_curr + " / " + controller.curr_scenario.highlighted_player.GetComponent<Character_Script>().action_max + "\n" +
                              "Can: " + controller.curr_scenario.highlighted_player.GetComponent<Character_Script>().canister_curr + " / " + controller.curr_scenario.highlighted_player.GetComponent<Character_Script>().canister_max + "\n" +
                              "Str: " + controller.curr_scenario.highlighted_player.GetComponent<Character_Script>().strength + "   Crd: " + controller.curr_scenario.highlighted_player.GetComponent<Character_Script>().coordination + "    Spt: " + controller.curr_scenario.curr_player.GetComponent<Character_Script>().spirit + "\n" +
                              "Dex: " + controller.curr_scenario.highlighted_player.GetComponent<Character_Script>().dexterity + "   Vit: " + controller.curr_scenario.highlighted_player.GetComponent<Character_Script>().vitality + "\n" +
                              "Wep: " + controller.curr_scenario.highlighted_player.GetComponent<Character_Script>().weapon.name + "   Armor: " + controller.curr_scenario.highlighted_player.GetComponent<Character_Script>().armor.name);
            }
        }



    }

	// Use this for initialization
	void Start () {
		//Screen.SetResolution(resolution_x, resolution_y, true);
		controller = Game_Controller.controller;
	}

	// Update is called once per frame
	void Update () {
        //check mouse position and scroll camera if necessary
        //transform.Translate(horizontal_speed* Input.GetAxis("Mouse Y"),vertical_speed* Input.GetAxis("Mouse X"),0);

        //Camera Scrolling
        if (transform.position.x >= -80)
        {
            if (Input.mousePosition.x <= HOR_BORDER)
            {
                transform.Translate(-HOR_SPD, 0, 0);// (transform.position.x-1,transform.position.y, transform.position.z);
            }
        }
        if (transform.position.x <= -10)
        {
            if (Input.mousePosition.x >= Screen.width - HOR_BORDER)
            {
                transform.Translate(HOR_SPD, 0, 0);
            }
        }
        if (transform.position.z <= -10)
        {
            if (Input.mousePosition.y >= Screen.height - VER_BORDER)
            {
                transform.Translate(0, 0, VER_SPD);
            }
        }
        if (transform.position.z >= -80)
        {
            if (Input.mousePosition.y <= VER_BORDER)
            {
                transform.Translate(0, 0, -VER_SPD);// (transform.position.x-1,transform.position.y, transform.position.z);
            }
        }
        transform.position = new Vector3(transform.position.x, CAMERA_HEIGHT, transform.position.z);

        //Camera Zooming
        if (Input.GetAxis("Mouse ScrollWheel") > 0f && this.GetComponent<Camera>().orthographicSize > 5) // forward
        {
            this.GetComponent<Camera>().orthographicSize--;
        }
        if (Input.GetAxis("Mouse ScrollWheel")< 0f && this.GetComponent<Camera>().orthographicSize < 15) // backwards
        {
            this.GetComponent<Camera>().orthographicSize++;
        }

        //Camera Turning
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("x:" + transform.rotation.x + ", y:" + transform.rotation.y + "z:" + transform.rotation.z + ", w:" + transform.rotation.w);

            //transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
            //transform.RotateAround(transform.position, Vector3.up, -90f);
            rotationAmount -= 90;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("x:" + transform.rotation.x + ", y:" + transform.rotation.y + "z:" + transform.rotation.z + ", w:" + transform.rotation.w);
            //transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
            //transform.RotateAround(transform.position, Vector3.up, 90f);
            rotationAmount += 90;
        }
        Vector3 rot = transform.rotation.eulerAngles;
        rot.y = rot.y + rotationAmount * Time.deltaTime *2;
        rotationAmount = rotationAmount - rotationAmount * Time.deltaTime *2;
        if (rot.y > 360)
            rot.y -= 360;
        else if (rot.y < 360)
            rot.y += 360;
        transform.eulerAngles = rot;

        /*if (transform.position.x >= -4) {
            if (Input.mousePosition.x <= 20) {
                transform.Translate (-.2f, 0, 0);// (transform.position.x-1,transform.position.y, transform.position.z);
            }
        }
        if (transform.position.x <= 4) {
            if (Input.mousePosition.x >= Screen.width - 20) {
                transform.Translate (.2f, 0, 0);
            }
        }
        if (transform.position.y >= -5) {
            if (Input.mousePosition.y <= 10) {
                transform.Translate (0, -.2f, 0);// (transform.position.x-1,transform.position.y, transform.position.z);
            }
        }
        if (transform.position.y <= 1) {
            if (Input.mousePosition.y >= Screen.height - 10) {
                transform.Translate (0, .2f, 0);
            }
        }*/
    }
}
