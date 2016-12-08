﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Character_Script : MonoBehaviour {

    //Constants
    public int AURA_MULTIPLIER = 10;
    public int AP_MULTIPLIER = 5;
    public int AP_MAX = 20;
    public int AP_RECOVERY = 10;
    public int SPEED = 8;
    public int character_id;
    public enum States { Moving, Attacking, Idle, Dead, Blinking }
    public enum Actions { Move=-1, Attack=5, Wait = 0, Blink=-2, Channel = 10 }
    public enum Weapons { Sword, Rifle, Spear, Sniper, Pistol, Claws }
    public enum Armors { Light, Medium, Heavy }
    public enum Character_Stats { aura_max, action_max, canister_max, strength, coordination, spirit, dexterity, vitality, speed };

    //Variables
    public int character_num { get; set; }
    public string character_name { get; set; }
    public int aura_max { get; set; }
    public int aura_curr { get; set; }
    public int action_max { get; set; }
    public int action_curr { get; set; }
    public int action_cost { get; set; }
    public int canister_max { get; set; }
    public int canister_curr { get; set; }
    public int strength { get; set; }
    public int coordination { get; set; }
    public int spirit { get; set; }
    public int dexterity { get; set; }
    public int vitality { get; set; }
    public double speed { get; set; }
    public int level { get; set; }
    public Weapon weapon { get; set; }
    public Armor armor { get; set; }
    public Accessory[] accessories { get; set; }
    public List<Actions> actions { get; set; }
    public States state { get; set; }
    public Game_Controller controller { get; set; }
    public Transform curr_tile { get; set; }

    public class Equipment
    {
        public string name;
        public enum Equipment_Type { Weapon, Armor, Accessory };
        public Equipment_Type type;
        public Actions[] actions;
        public Effect[] effects; 
        public int durability;
        public double weight;
        public int armor;
        public SpriteRenderer sprite;

        public class Effect
        {
            public Character_Stats stat;
            public int effect;

            public Effect(Character_Stats st, int eff)
            {
                stat = st;
                effect = eff;
            }
        }
    }

    public class Armor: Equipment
    {
        public Armor(string str)
        {
            type = Equipment_Type.Armor;
            durability = 100;
            switch (str)
            {
                case "Light":
                    name = Armors.Light.ToString();
                    armor = -1;
                    weight = 0;
                    effects = new Effect[2];
                    effects[0] = new Effect(Character_Stats.speed, 1);
                    effects[1] = new Effect(Character_Stats.dexterity, 1);
                    actions = new Actions[1];
                    actions[0] = Actions.Blink;
                    break;
                case "Medium":
                    name = Armors.Medium.ToString();
                    armor = 2;
                    weight = 1;
                    effects = new Effect[2];
                    effects[0] = new Effect(Character_Stats.strength, 1);
                    effects[1] = new Effect(Character_Stats.coordination, 1);
                    break;
                case "Heavy":
                    name = Armors.Heavy.ToString();
                    armor = 5;
                    weight = 2;
                    effects = new Effect[2];
                    effects[0] = new Effect(Character_Stats.vitality, 1);
                    effects[1] = new Effect(Character_Stats.spirit, 1);
                    actions = new Actions[1];
                    actions[0] = Actions.Channel;
                    break;
            }
        }

        public Armor(Armors ar)
        {
            type = Equipment_Type.Armor;
            durability = 100;
            switch (ar)
            {
                case Armors.Light:
                    name = Armors.Light.ToString();
                    armor = -1;
                    weight = 0;
                    effects = new Effect[2];
                    effects[0] = new Effect(Character_Stats.speed, 1);
                    effects[1] = new Effect(Character_Stats.dexterity, 1);
                    actions = new Actions[1];
                    actions[0] = Actions.Blink;
                    break;
                case Armors.Medium:
                    name = Armors.Medium.ToString();
                    armor = 2;
                    weight = 1;
                    effects = new Effect[2];
                    effects[0] = new Effect(Character_Stats.strength, 1);
                    effects[1] = new Effect(Character_Stats.coordination, 1);
                    break;
                case Armors.Heavy:
                    name = Armors.Heavy.ToString();
                    armor = 5;
                    weight = 2;
                    effects = new Effect[2];
                    effects[0] = new Effect(Character_Stats.vitality, 1);
                    effects[1] = new Effect(Character_Stats.spirit, 1);
                    actions = new Actions[1];
                    actions[0] = Actions.Channel;
                    break;
            }
        }
    }

    public class Accessory : Equipment
    {
        public Accessory()
        {
            type = Equipment_Type.Accessory;
            durability = 100;
        }
    }

    public class Weapon: Equipment{
        public int range;
        public int attack;
        public bool ranged;

        public Weapon(string str)
        {
            type = Equipment_Type.Weapon;
            durability = 100;
            actions = new Actions[3];
            actions[0] = Actions.Move;
            actions[1] = Actions.Attack;
            actions[2] = Actions.Wait;
            switch (str)
            {
                case "Sword":
                    name = Weapons.Sword.ToString();
                    range = 1;
                    attack = 2;
                    weight = 0.5;
                    ranged = false;
                    break;
                case "Rifle":
                    name = Weapons.Rifle.ToString();
                    range = 4;
                    attack = 3;
                    ranged = true;
                    weight = 1;
                    break;
                case "Spear":
                    name = Weapons.Spear.ToString();
                    range = 2;
                    attack = 2;
                    ranged = false;
                    weight = 1;
                    break;
                case "Sniper":
                    name = Weapons.Sniper.ToString();
                    range = 6;
                    attack = 5;
                    ranged = true;
                    weight = 3;
                    break;
                case "Pistol":
                    name = Weapons.Pistol.ToString();
                    range = 3;
                    attack = 2;
                    ranged = true;
                    weight = 0.5;
                    break;
                case "Claws":
                    name = Weapons.Claws.ToString();
                    range = 1;
                    attack = 10;
                    ranged = false;
                    weight = 4;
                    break;
                default:
                    break;
            }
        }

        public Weapon(Weapons wep)
        {
            type = Equipment_Type.Weapon;
            durability = 100;
            actions = new Actions[3];
            actions[0] = Actions.Move;
            actions[1] = Actions.Attack;
            actions[2] = Actions.Wait;
            switch (wep)
            {
                case Weapons.Sword:
                    name = Weapons.Sword.ToString();
                    range = 1;
                    attack = 2;
                    weight = 0.5;
                    ranged = false;
                    break;
                case Weapons.Rifle:
                    name = Weapons.Rifle.ToString();
                    range = 4;
                    attack = 3;
                    ranged = true;
                    weight = 1;
                    break;
                case Weapons.Spear:
                    name = Weapons.Spear.ToString();
                    range = 2;
                    attack = 2;
                    ranged = false;
                    weight = 1;
                    break;
                case Weapons.Sniper:
                    name = Weapons.Sniper.ToString();
                    range = 6;
                    attack = 5;
                    ranged = true;
                    weight = 3;
                    break;
                case Weapons.Pistol:
                    name = Weapons.Pistol.ToString();
                    range = 3;
                    attack = 2;
                    ranged = true;
                    weight = 0.5;
                    break;
                case Weapons.Claws:
                    name = Weapons.Claws.ToString();
                    range = 1;
                    attack = 10;
                    ranged = false;
                    weight = 4;
                    break;
                default:
                    break;
            }
        }
    }

    //Methods
	// Use this for initialization
	void Start () {
        //gameObject.SetActive(true);
    }

    public Character_Script(string nm, int lvl, int str, int crd, int spt, int dex, int vit, int spd, int can, string wep, string arm)
    {
        controller = Game_Controller.controller;
        character_name = nm.TrimStart();
        level = lvl;
        strength = str;
        coordination = crd;
        spirit = spt;
        dexterity = dex;
        vitality = vit;
        speed = spd;
        aura_max = vitality * AURA_MULTIPLIER;
        aura_curr = aura_max;
        action_max = AP_MAX;
        action_curr = action_max;
        action_cost = 0;
        actions = new List<Actions>();
        canister_max = can;
        canister_curr = canister_max;
        state = States.Idle;
        foreach (Weapons weps in Enum.GetValues(typeof(Weapons)))
        {
            if (wep.TrimStart() == weps.ToString())
            {
                Weapon w = new Weapon(weps);
                Equip(w);
                break;
            }
        }
        foreach (Armors arms in Enum.GetValues(typeof(Armors)))
        {
            if (arm.TrimStart() == arms.ToString())
            {
                Armor a = new Armor(arms);
                Equip(a);
                break;
            }
        }
        //foreach (string s in acc)
        //{
        //    foreach (Weapons weps in Enum.GetValues(typeof(Weapons)))
        //    {
        //        if (wep == weps.ToString())
        //        {
        //            Weapon w = new Weapon(weps);
        //            Equip(w);
        //            break;
        //        }
        //    }
        //}
    }

    public void Equip(Equipment e)
    {
        switch (e.type)
        {
            case Equipment.Equipment_Type.Weapon:
                weapon = (Weapon)e;
                break;
            case Equipment.Equipment_Type.Accessory:
                accessories[0] = (Accessory)e;
                break;
            case Equipment.Equipment_Type.Armor:
                armor = (Armor)e;
                break;
            default:
                break;
        }
        if (e.effects != null)
        {
            foreach (Equipment.Effect eff in e.effects)
            {
                switch (eff.stat)
                {
                    case Character_Stats.action_max:
                        action_max += eff.effect * AP_MULTIPLIER;
                        action_curr += eff.effect * AP_MULTIPLIER;
                        break;
                    case Character_Stats.aura_max:
                        aura_max += eff.effect * AURA_MULTIPLIER;
                        aura_max += eff.effect * AURA_MULTIPLIER;
                        break;
                    case Character_Stats.canister_max:
                        canister_max += eff.effect;
                        break;
                    case Character_Stats.coordination:
                        coordination += eff.effect;
                        break;
                    case Character_Stats.dexterity:
                        dexterity += eff.effect;
                        break;
                    case Character_Stats.speed:
                        speed += eff.effect;
                        break;
                    case Character_Stats.spirit:
                        spirit += eff.effect;
                        action_max += eff.effect * AP_MULTIPLIER;
                        action_curr += eff.effect * AP_MULTIPLIER;
                        break;
                    case Character_Stats.strength:
                        strength += eff.effect;
                        break;
                    case Character_Stats.vitality:
                        vitality += eff.effect;
                        aura_max += eff.effect * AURA_MULTIPLIER;
                        aura_curr += eff.effect * AURA_MULTIPLIER;
                        break;
                }
            }
        }
        /*speed -= e.weight;
        if( speed <= 0)
        {
            speed = 1;
        }*/
        if (e.actions != null)
        {
            foreach(Actions a in e.actions)
            {
                actions.Add(a);
            }
        }
    }

    public int Calculate_Damage(Armor a)
    {
        if (weapon.ranged)
        {
            return weapon.attack + coordination - a.armor;
        }else
        {
            return weapon.attack + strength - a.armor;
        }
    }

    public void Randomize(){
        //Randomize stats
		strength = UnityEngine.Random.Range (1,7);
		coordination = UnityEngine.Random.Range (1, 7);
		spirit = UnityEngine.Random.Range (1, 7);
		dexterity = UnityEngine.Random.Range (1, 7);
		vitality = UnityEngine.Random.Range (1, 7);

        //Formulas
        speed = SPEED;
        aura_max = vitality * AURA_MULTIPLIER;
        aura_curr = aura_max;
        action_max = AP_MAX;// spirit * AP_MULTIPLIER;
        action_curr = action_max;
        actions = new List<Actions>();
        canister_max = UnityEngine.Random.Range(0, 3);
        canister_curr = canister_max;
		state = States.Idle;

        //Randomize Equipment
        int w = UnityEngine.Random.Range(0, 5);
        Weapon wep;
        if (w == 0)
        {
            wep = new Weapon(Weapons.Sword);
        } else if (w == 1)
        {
            wep = new Weapon(Weapons.Rifle);
        }
        else if (w == 2)
        {
            wep = new Weapon(Weapons.Spear);
        }
        else if (w == 3)
        {
            wep = new Weapon(Weapons.Sniper);
        }
        else if (w == 4)
        {
            wep = new Weapon(Weapons.Pistol);
        }
        else
        {
            wep = new Weapon(Weapons.Claws);
        }
        Equip(wep);
        int a = UnityEngine.Random.Range(0, 3);
        Armor ar;
        if (a == 0)
        {
            ar = new Armor(Armors.Light);
        }
        else if (a == 1)
        {
            ar = new Armor(Armors.Medium);
        }
        else
        {
            ar = new Armor(Armors.Heavy);
        }
        Equip(ar);
		level = 1;
		character_name = "Character " + character_num;
		controller = Game_Controller.controller;
        //FindReachable(controller.tile_grid, weapon.range);
        //FindReachable(controller.GetComponent<Game_Controller>().tile_grid,dexterity);
	}
	
	//public SpriteRenderer renderer;

	public void Action(Actions act){
        if ((int)act > 0)
        {
            action_cost = (int)act;
        }else if ((int)act == 0)
        {
            action_cost = -AP_RECOVERY;
        }
        if(action_curr-action_cost < 0)
        {
            action_cost = action_curr;
        }
        if (act == Actions.Move && state != States.Dead) {

            state = States.Moving;
			controller.curr_scenario.FindReachable(action_curr, SPEED);
			controller.curr_scenario.CleanReachable ();
			controller.curr_scenario.MarkReachable ();
		}
		if (act == Actions.Attack && state != States.Dead) {
			state = States.Attacking;
            controller.curr_scenario.FindReachable(action_curr, weapon.range);
            controller.curr_scenario.CleanReachable ();
            controller.curr_scenario.MarkReachable ();
		}
        if (act == Actions.Wait && state != States.Dead)
        {
            state = States.Idle;
            action_curr -= action_cost;
            print("Character " + character_name + " Waited, Recovering " + AP_RECOVERY + " AP.");
            action_cost = 0;
            if (action_curr > action_max)
            {
                action_curr = action_max;
            }
            controller.curr_scenario.CleanReachable();
            controller.curr_scenario.NextPlayer();
        }
        if (act == Actions.Blink && state != States.Dead)
        {
            state = States.Blinking;
            controller.curr_scenario.FindReachable(action_curr, weapon.range);
            controller.curr_scenario.CleanReachable();
            controller.curr_scenario.MarkReachable();
        }
        if (act == Actions.Channel && state != States.Dead)
        {
            Channel();
            controller.curr_scenario.CleanReachable();
        }


    }

    public void Die()
    {
        state = States.Dead;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
    }

    public IEnumerator MoveOverTime(Tile_Data.Node prev_tile, Tile_Data.Node temp_tile)
    {
        float elapsedTime = 0;
        float duration = 2;
        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(new Vector3(controller.curr_scenario.tile_grid.getTiles()[prev_tile.id[0], prev_tile.id[1]].position.x, 
                (float)(controller.curr_scenario.tile_grid.getTiles()[prev_tile.id[0], prev_tile.id[1]].position.y + 
                (controller.curr_scenario.tile_grid.getTiles()[prev_tile.id[0], prev_tile.id[1]].GetComponent<SpriteRenderer>().sprite.rect.height) / 100) + 0.15f, curr_tile.position.z), 
                new Vector3(controller.curr_scenario.tile_grid.getTiles()[temp_tile.id[0], temp_tile.id[1]].position.x, 
                (float)(controller.curr_scenario.tile_grid.getTiles()[temp_tile.id[0], temp_tile.id[1]].position.y + 
                (controller.curr_scenario.tile_grid.getTiles()[temp_tile.id[0], temp_tile.id[1]].GetComponent<SpriteRenderer>().sprite.rect.height) / 100) + 0.15f, curr_tile.position.z), 
                elapsedTime/duration);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator Move(Transform clicked_tile)
    {
        Stack<Tile_Data.Node> path = new Stack<Tile_Data.Node>();
        //path = controller.navmesh.shortestPath(curr_tile.GetComponent<Tile_Data>().node, clicked_tile.GetComponent<Tile_Data>().node, action_curr, SPEED);
        //Tile_Data.Node temp_tile;
        //temp_tile = path.Pop();
        //Tile_Data.Node prev_tile;
        Tile_Data.Node temp_tile = clicked_tile.GetComponent<Tile_Data>().node;
        Tile_Data.Node prev_tile = curr_tile.GetComponent<Tile_Data>().node;
        action_cost = temp_tile.weight;
        //action_cost = (int)clicked_tile.GetComponent<Tile_Data>().node.weight;//Math.Abs(curr_tile.GetComponent<Tile_Data>().x_index - clicked_tile.GetComponent<Tile_Data>().x_index) * (int)(armor.weight + weapon.weight) +Math.Abs(curr_tile.GetComponent<Tile_Data>().y_index - clicked_tile.GetComponent<Tile_Data>().y_index) * (int)(armor.weight + weapon.weight) + (clicked_tile.GetComponent<Tile_Data>().node.height - curr_tile.GetComponent<Tile_Data>().node.height)*2;
        if (action_cost < 1)
        {
            action_cost = 1;
        }
        if (action_cost > action_curr)
        {
            action_cost = action_curr;
        }
        print("Character " + character_name + " Moved from: " + curr_tile.GetComponent<Tile_Data>().x_index + "," + curr_tile.GetComponent<Tile_Data>().y_index + " to: " + clicked_tile.GetComponent<Tile_Data>().x_index + "," + clicked_tile.GetComponent<Tile_Data>().y_index + " Using " + action_cost + " AP");
        action_curr -= action_cost;
        action_cost = 0;

        if (gameObject.CompareTag("Player"))
        {
            //Tile_Data.Node temp_tile = clicked_tile.GetComponent<Tile_Data>().node;
            //Tile_Data.Node prev_tile = curr_tile.GetComponent<Tile_Data>().node;
            //Stack<Tile_Data.Node> path = new Stack<Tile_Data.Node>();

            //Construct a stack that is a path from the clicked tile to the source.
            while(!(temp_tile.id[0] == curr_tile.GetComponent<Tile_Data>().node.id[0] && temp_tile.id[1] == curr_tile.GetComponent<Tile_Data>().node.id[1]))
            {
                path.Push(temp_tile);
                //Look at the parent tile.
                temp_tile = temp_tile.parent;
            }
            
            //distances.Push(distance);
            //Debug.Log("temp_tile.id[0]: " + temp_tile.id[0]);
            //Debug.Log("temp_tile.id[1]: " + temp_tile.id[1]);
            //Debug.Log("curr_tile.id[0]: " + curr_tile.GetComponent<Tile_Data>().node.id[0]);
            //Debug.Log("curr_tile.id[0]: " + curr_tile.GetComponent<Tile_Data>().node.id[1]);
            //Navigate the path by popping tiles out of the stack.
            while (path.Count != 0)
            {

                temp_tile = path.Pop();
                //transform.position = new Vector3(controller.curr_scenario.tile_grid.getTiles()[temp_tile.id[0], temp_tile.id[1]].position.x, (float)(controller.curr_scenario.tile_grid.getTiles()[temp_tile.id[0], temp_tile.id[1]].position.y + (controller.curr_scenario.tile_grid.getTiles()[temp_tile.id[0], temp_tile.id[1]].GetComponent<SpriteRenderer>().sprite.rect.height) / 100) + 0.15f, curr_tile.position.z);
                //
                //yield return new WaitForSeconds(.3f);

                float elapsedTime = 0;
                float duration = .3f;
                //print("duration: " +duration);
                while (elapsedTime < duration)
                {
                    transform.position = Vector3.Lerp(new Vector3(controller.curr_scenario.tile_grid.getTiles()[prev_tile.id[0], prev_tile.id[1]].position.x, (float)(controller.curr_scenario.tile_grid.getTiles()[prev_tile.id[0], prev_tile.id[1]].position.y + (controller.curr_scenario.tile_grid.getTiles()[prev_tile.id[0], prev_tile.id[1]].GetComponent<SpriteRenderer>().sprite.rect.height) / 100) + 0.15f, curr_tile.position.z), new Vector3(controller.curr_scenario.tile_grid.getTiles()[temp_tile.id[0], temp_tile.id[1]].position.x, (float)(controller.curr_scenario.tile_grid.getTiles()[temp_tile.id[0], temp_tile.id[1]].position.y + (controller.curr_scenario.tile_grid.getTiles()[temp_tile.id[0], temp_tile.id[1]].GetComponent<SpriteRenderer>().sprite.rect.height) / 100) + 0.15f, curr_tile.position.z), elapsedTime/duration);
                    elapsedTime += Time.deltaTime;
                    if (controller.curr_scenario.tile_grid.getTiles()[temp_tile.id[0], temp_tile.id[1]].GetComponent<SpriteRenderer>().sortingOrder > curr_tile.GetComponent<SpriteRenderer>().sortingOrder)
                    {
                        gameObject.GetComponent<SpriteRenderer>().sortingOrder = controller.curr_scenario.tile_grid.getTiles()[temp_tile.id[0], temp_tile.id[1]].GetComponent<SpriteRenderer>().sortingOrder + 1;
                    }
                    else
                    {
                        gameObject.GetComponent<SpriteRenderer>().sortingOrder = curr_tile.GetComponent<SpriteRenderer>().sortingOrder+1;
                    }
                    yield return new WaitForEndOfFrame();
                }
                gameObject.GetComponent<SpriteRenderer>().sortingOrder = controller.curr_scenario.tile_grid.getTiles()[temp_tile.id[0], temp_tile.id[1]].GetComponent<SpriteRenderer>().sortingOrder + 1;
                prev_tile = temp_tile;



                //    new Vector3(controller.curr_scenario.tile_grid.getTiles()[temp_tile.id[0], temp_tile.id[1]].position.x, (float)(controller.curr_scenario.tile_grid.getTiles()[temp_tile.id[0], temp_tile.id[1]].position.y + (controller.curr_scenario.tile_grid.getTiles()[temp_tile.id[0], temp_tile.id[1]].GetComponent<SpriteRenderer>().sprite.rect.height) / 100) + 0.15f, curr_tile.position.z);
                //WaitForSeconds(1);
            }
            //transform.position = new Vector3(curr_tile.position.x, (float)(curr_tile.position.y + (curr_tile.GetComponent<SpriteRenderer>().sprite.rect.height) / 100) + 0.15f, curr_tile.position.z);
        }
        else
        {
            temp_tile = clicked_tile.GetComponent<Tile_Data>().node;
            prev_tile = curr_tile.GetComponent<Tile_Data>().node;
            path = new Stack<Tile_Data.Node>();
            //Construct a stack that is a path from the clicked tile to the source.
            while (!(temp_tile.id[0] == curr_tile.GetComponent<Tile_Data>().node.id[0] && temp_tile.id[1] == curr_tile.GetComponent<Tile_Data>().node.id[1]))
            {
                path.Push(temp_tile);
                temp_tile = temp_tile.parent;
            }
            //Debug.Log("temp_tile.id[0]: " + temp_tile.id[0]);
            //Debug.Log("temp_tile.id[1]: " + temp_tile.id[1]);
            //Debug.Log("curr_tile.id[0]: " + curr_tile.GetComponent<Tile_Data>().node.id[0]);
            //Debug.Log("curr_tile.id[0]: " + curr_tile.GetComponent<Tile_Data>().node.id[1]);
            //Navigate the path by popping tiles out of the stack.
            while (path.Count != 0)
            {

                temp_tile = path.Pop();
                float elapsedTime = 0;
                float duration = .3f;
                //print("duration: " + duration);
                while (elapsedTime < duration)
                {
                    transform.position = Vector3.Lerp(new Vector3(controller.curr_scenario.tile_grid.getTiles()[prev_tile.id[0], prev_tile.id[1]].position.x, (float)(controller.curr_scenario.tile_grid.getTiles()[prev_tile.id[0], prev_tile.id[1]].position.y + (controller.curr_scenario.tile_grid.getTiles()[prev_tile.id[0], prev_tile.id[1]].GetComponent<SpriteRenderer>().sprite.rect.height) / 100) + 0.15f, curr_tile.position.z), new Vector3(controller.curr_scenario.tile_grid.getTiles()[temp_tile.id[0], temp_tile.id[1]].position.x, (float)(controller.curr_scenario.tile_grid.getTiles()[temp_tile.id[0], temp_tile.id[1]].position.y + (controller.curr_scenario.tile_grid.getTiles()[temp_tile.id[0], temp_tile.id[1]].GetComponent<SpriteRenderer>().sprite.rect.height) / 100) + 0.15f, curr_tile.position.z), elapsedTime / duration);
                    elapsedTime += Time.deltaTime;
                    if (controller.curr_scenario.tile_grid.getTiles()[temp_tile.id[0], temp_tile.id[1]].GetComponent<SpriteRenderer>().sortingOrder > curr_tile.GetComponent<SpriteRenderer>().sortingOrder)
                    {
                        gameObject.GetComponent<SpriteRenderer>().sortingOrder = controller.curr_scenario.tile_grid.getTiles()[temp_tile.id[0], temp_tile.id[1]].GetComponent<SpriteRenderer>().sortingOrder + 1;
                    }
                    else
                    {
                        gameObject.GetComponent<SpriteRenderer>().sortingOrder = curr_tile.GetComponent<SpriteRenderer>().sortingOrder + 1;
                    }
                    yield return new WaitForEndOfFrame();
                }
                gameObject.GetComponent<SpriteRenderer>().sortingOrder = controller.curr_scenario.tile_grid.getTiles()[temp_tile.id[0], temp_tile.id[1]].GetComponent<SpriteRenderer>().sortingOrder + 1;
                prev_tile = temp_tile;
            }
            //transform.position = new Vector3(curr_tile.position.x, (float)(curr_tile.position.y + (curr_tile.GetComponent<SpriteRenderer>().sprite.rect.height) / 100) + 0.15f, curr_tile.position.z); 
        }
        //renderer = (SpriteRenderer)curr_tile.GetComponent<SpriteRenderer> ();
        curr_tile.GetComponent<Tile_Data>().node.traversible = true;
        curr_tile = clicked_tile;
        curr_tile.GetComponent<Tile_Data>().node.traversible = false;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = clicked_tile.GetComponent<SpriteRenderer>().sortingOrder + 1;
        state = States.Idle;
        controller.curr_scenario.ResetReachable();
        if (action_curr <= 0)
        {
            action_curr = 0;
            action_curr += AP_RECOVERY;
            controller.curr_scenario.NextPlayer();
        }
        if (action_curr > action_max)
        {
            action_curr = action_max;
        }
        controller.action_menu.GetComponent<Action_Menu_Script>().resetActions();
    }

    public void Attack(GameObject character)
    {
        print("Character " + character_name + " Attacked: " + character.GetComponent<Character_Script>().character_name + "; Dealing " + Calculate_Damage(character.GetComponent<Character_Script>().armor) + " damage and Using " + action_cost + " AP");
        action_curr -= action_cost;
        action_cost = 0;
        if (character.GetComponent<Character_Script>().aura_curr == 0 )
        {
            character.GetComponent<Character_Script>().Die();
        }else
        {
            character.GetComponent<Character_Script>().aura_curr -= Calculate_Damage(character.GetComponent<Character_Script>().armor);
            if(character.GetComponent<Character_Script>().aura_curr < 0)
            {
                character.GetComponent<Character_Script>().aura_curr = 0;
                character.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
            }
        }
        state = States.Idle;
        controller.curr_scenario.ResetReachable();
        if (action_curr <= 0)
        {
            action_curr = 0;
            action_curr += AP_RECOVERY;
            controller.curr_scenario.NextPlayer();
        }
        if (action_curr > action_max)
        {
            action_curr = action_max;
        }
        controller.action_menu.GetComponent<Action_Menu_Script>().resetActions();
    }

    public void Blink (Transform clicked_tile)
    {
        action_cost = Math.Abs(curr_tile.GetComponent<Tile_Data>().x_index - clicked_tile.GetComponent<Tile_Data>().x_index) + Math.Abs(curr_tile.GetComponent<Tile_Data>().y_index - clicked_tile.GetComponent<Tile_Data>().y_index);
        if (action_cost < 1)
        {
            action_cost = 1;
        }
        if (action_cost > action_curr)
        {
            action_cost = action_curr;
        }
        print("Character " + character_name + " Blinked from: " + curr_tile.GetComponent<Tile_Data>().x_index + "," + curr_tile.GetComponent<Tile_Data>().y_index + " to: " + clicked_tile.GetComponent<Tile_Data>().x_index + "," + clicked_tile.GetComponent<Tile_Data>().y_index + " Using " + action_cost + " AP");
        action_curr -= action_cost;
        action_cost = 0;
        curr_tile.GetComponent<Tile_Data>().node.traversible = true;
        curr_tile = clicked_tile;
        curr_tile.GetComponent<Tile_Data>().node.traversible = false;
        if (gameObject.CompareTag("Player"))
        {
            transform.position = new Vector3(curr_tile.position.x, (float)(curr_tile.position.y + (curr_tile.GetComponent<SpriteRenderer>().sprite.rect.height) / 100) + 0.15f, curr_tile.position.z);
        }
        else
        {
            transform.position = new Vector3(curr_tile.position.x, (float)(curr_tile.position.y + (curr_tile.GetComponent<SpriteRenderer>().sprite.rect.height) / 100) + 0.15f, curr_tile.position.z);
        }
        //renderer = (SpriteRenderer)curr_tile.GetComponent<SpriteRenderer> ();
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = clicked_tile.GetComponent<SpriteRenderer>().sortingOrder + 1;
        state = States.Idle;
        controller.curr_scenario.ResetReachable();
        if (action_curr <= 0)
        {
            action_curr = 0;
            action_curr += AP_RECOVERY;
            controller.curr_scenario.NextPlayer();
        }
        if (action_curr > action_max)
        {
            action_curr = action_max;
        }
        controller.action_menu.GetComponent<Action_Menu_Script>().resetActions();
    }

    public void Channel()
    {
        print("Character " + character_name + " Channeled from: " + aura_curr + ", Using " + action_cost + " AP");
        action_curr -= action_cost;
        action_cost = 0;
        aura_curr += 10;
        state = States.Idle;
        if (aura_curr > aura_max)
        {
            aura_curr = aura_max;
        }
        if (action_curr <= 0)
        {
            action_curr = 0;
            action_curr += AP_RECOVERY;
            controller.curr_scenario.NextPlayer();
        }
        if (action_curr > action_max)
        {
            action_curr = action_max;
        }
        controller.action_menu.GetComponent<Action_Menu_Script>().resetActions();
    }

    public Character_Script(){
	}
	
	// Update is called once per frame
	void Update () {
		if (aura_curr < 0) {
			aura_curr = 0;
		}
        if (aura_curr > aura_max)
        {
            aura_curr = aura_max;
        }
        if (action_curr <= 0)
        {
            action_curr = 0;
            action_curr += AP_RECOVERY;
            controller.curr_scenario.NextPlayer();
        }
        if (action_curr > action_max)
        {
            action_curr = action_max;
        }
    }
}