﻿using UnityEngine;
using System.Collections;
using UnityEditor;

public class Tile_Grid : MonoBehaviour{
	public float TILE_LENGTH = 43;
	public float TILE_WIDTH = 85;
	public float TILE_HEIGHT = 16;
	public static int MAX_TILES = 10;
	public static int tile_grid_width=20;
	public static int tile_grid_height=20;
	Transform tile;
	Sprite[] tile_sprite_sheet;
	Transform item;
	Sprite[] item_sprite_sheet;
	SpriteRenderer sprite;
	int[,] item_sprites = new int[tile_grid_width, tile_grid_height];
	Transform[,,] tiles = new Transform[tile_grid_width, tile_grid_height,MAX_TILES];
	int[,] tile_heights = new int[tile_grid_width, tile_grid_height];
	int[,,] tile_sprites = new int[tile_grid_width, tile_grid_height,MAX_TILES];

	public Tile_Grid(string[] lines, Transform tile_prefab, Sprite[] newTileSprites, Transform item_prefab, Sprite[] newItemSpriteSheet){
		tile = tile_prefab;
		tile_sprite_sheet = newTileSprites;
		item = item_prefab;
		item_sprite_sheet = newItemSpriteSheet;
		//read the height map
		int tile_sprite;
		int item_sprite;
		int tile_number = 0;
		int row_num = 0;
		int col_num = 0;
		int i = 0;
		double start_x_pos = 0;
		double start_y_pos = 3.5;
		foreach (string line in lines){
			string[] elements = line.Split(';');
			foreach (string e in elements){
				if (row_num == 1) { 
					if (col_num==0){
						//if (int.TryParse(e, out tile_grid_width)){
						//}
					}
					else if (col_num == 1) {
						//if (int.TryParse(e, out tile_grid_height)){
						//}
					}
				}
				else if (row_num >= 3 && row_num < tile_grid_height+3){
					//print ("string " + e);
					string[] str = e.Split (',');
					i = 0;
					foreach (string s in str) {
					    if (int.TryParse(s, out tile_sprite)){
							//print ("Tile " + (row_num-3) + "," + col_num + "," + i + " sprite = " + s);
							tile_sprites[row_num-3,col_num,i] = tile_sprite;
							//tile_sprites[row_num-tile_grid_height-4,col_num,i] = tile_sprite;
						    i++;
							if (i >= MAX_TILES){
								break;
							}
						}
					}
					//print ("i = " + i);
					tile_heights[row_num-3,col_num] = i;
				}
				else if(row_num >= tile_grid_height+4){
					if (int.TryParse(e, out item_sprite)){
						//TODO FIX TILE OBJECTS
						item_sprites[row_num-tile_grid_height-4,col_num] = item_sprite;
						//tile_sprites[row_num-tile_grid_height-4,col_num] = tile_sprite;
						//print("line_num:" + line_num);
						//print ("line_num - tile_grid_height - 4 = " + (line_num-tile_grid_height-4));
						//print ("num_num:" + num_num);
						//tiles[row_num-tile_grid_height-4,col_num].setTileSpriteIndex(tile_sprite);
					}
				}
				col_num++;
			}
			col_num = 0;
			row_num++;
		}

		for (int x = 0; x < tile_grid_width; x++){
			for (int y = 0; y < tile_grid_height; y++){
				for (int z=0; z < tile_heights[x,y]; z++){
					//Set the correct sprite for the tile
					sprite = tile.GetComponent<SpriteRenderer>();
					sprite.sprite = tile_sprite_sheet[tile_sprites[x,y,z]-1];
 
					//Instantiate the tile object. Destroy the collider on the prefab if the tile is not on top of the stack.
					Transform instance= (Transform)Instantiate(tile, new Vector3((float)(start_x_pos - (x) * (TILE_WIDTH/200) + (y) * (TILE_WIDTH/200)), (float)(start_y_pos - (x) * (TILE_LENGTH/200) - (y) * (TILE_LENGTH/200)+z*TILE_HEIGHT/100.0), 0), Quaternion.identity);
					if (z != tile_heights[x,y]-1){
						Destroy (instance.gameObject.GetComponent<PolygonCollider2D>());
					}
					if (tile_sprites[x,y,z]-1 != 2){
						Destroy (instance.gameObject.GetComponent<Animator>());
					}
					instance.GetComponent<Tile_Data>().instantiate(x,y,tile_heights[x,y],tile_sprites[x,y,z]);

					//else {
					//	instance = Instantiate(tile, new Vector3((float)(start_x_pos - (x) * (TILE_WIDTH/200) + (y) * (TILE_WIDTH/200)), (float)(start_y_pos - (x) * (TILE_LENGTH/200) - (y) * (TILE_LENGTH/200)+z*TILE_HEIGHT/100.0), 0), Quaternion.identity);
					//}
					//Increase the tile draw order so other tiles are drawn correctly.
					//if (sprite)
					//{
					tile_number++;
					sprite.sortingOrder = tile_number;
					//}
					tiles[x,y,z] = instance;

					if(item_sprites[x,y] == 0){
						tiles[x,y,z].GetComponent<Tile_Data>().traversible = true;
					}
					else{
						tiles[x,y,z].GetComponent<Tile_Data>().traversible = false;
					}
				}
				if (item_sprites[x,y] != 0) {
					sprite = item.GetComponent<SpriteRenderer>();
					sprite.sprite = item_sprite_sheet[item_sprites[x,y]-1];
					sprite.sortingOrder = tile_number + 1;
					Instantiate(item, new Vector3((float)(start_x_pos - (x) * (TILE_WIDTH/200) + (y) * (TILE_WIDTH/200)), (float)(start_y_pos - (x) * (TILE_LENGTH/200) - (y) * (TILE_LENGTH/200)+tile_heights[x,y]*TILE_HEIGHT/100.0+ 0.075f), 0), Quaternion.identity);

					print (sprite.sortingOrder);
					//Instantiate(new Transform())
				}
			}
		}
		sprite.sortingOrder = 0;

	}

	public Transform[,,] getTiles(){
		return tiles;
	}

	public Transform getTile(int x, int y, int z){
		return tiles [x, y, z];
	}

	public Transform getTopTile(int x, int y){
		if (tiles [x, y, 0].GetComponent<Tile_Data>().tile_height == 0)
			return tiles [x, y, 0];
		else
			return tiles [x, y, tiles [x, y, 0].GetComponent<Tile_Data>().tile_height - 1];
	}

	public double get_TILE_HEIGHT(){
		return TILE_HEIGHT;
	}
}

public class Draw_Tile_Grid : MonoBehaviour {

	public Transform tile;
	public Sprite[] tile_sprite_sheet;
	public Transform item;
	public Sprite[] item_sprite_sheet;
	public Tile_Grid tile_grid;
	public GameObject controller;
	public string curr_map;

	// Use this for initialization
	void Start () {
		//string[] lines = System.IO.File.ReadAllLines(@"Assets/Maps/falls_map.txt");
		curr_map = controller.GetComponent<Game_Controller>().curr_map;
		string[] lines = System.IO.File.ReadAllLines(curr_map);
		tile_grid = new Tile_Grid(lines, tile, tile_sprite_sheet, item, item_sprite_sheet);
		tile.GetComponent<SpriteRenderer> ().color = new Color(255f, 255f, 255f, 1f);
	}
	
	// Update is called once per frame
	void Update () {
		print ("curr_map  " + curr_map);
		print ("currMap " + controller.GetComponent<Game_Controller> ().curr_map);
		if (curr_map != controller.GetComponent<Game_Controller> ().curr_map) {
			curr_map = controller.GetComponent<Game_Controller> ().curr_map;
			//destroy the old map
			GameObject[] objects = GameObject.FindGameObjectsWithTag ("Tile");
			foreach (GameObject game_object in objects) {
				Destroy (game_object);
			}
			objects = GameObject.FindGameObjectsWithTag ("Object");
			foreach (GameObject game_object in objects) {
				Destroy (game_object);
			}
			string[] lines = System.IO.File.ReadAllLines(curr_map);
			tile_grid = new Tile_Grid(lines, tile, tile_sprite_sheet, item, item_sprite_sheet);
			tile.GetComponent<SpriteRenderer> ().color = new Color(255f, 255f, 255f, 1f);
		}

	}
	
	void OnApplicationQuit() {
		tile.GetComponent<SpriteRenderer> ().color = new Color(255f, 255f, 255f, 1f);
	}
}
