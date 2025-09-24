using System;
using UnityEngine;


public class NewMonoBehaviourScript : MonoBehaviour
{
    public int _dimensions_x = 10;
    public int _dimensions_z = 10;
    
    public Vector2 _start = new Vector2(0,0);

    public int _critical_path_length = 10;

    public int _branch_length = 5;

    enum DIRECTIONS {RIGHT,UP,LEFT,DOWN};

    private Vector2 LEVEL_START_POS = new Vector2(1, 1);
    private Vector2 ROOM_SIZE = new Vector2(3000, 1500);

    private int[,] level = { { }, };
    private Array _branch_candidates;
    private Array room_nodes;

    //private bool room_instantiated = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        print(DIRECTIONS.RIGHT);
        _initialize_level();
        _place_entrance();
        _print_level();
    }

    void _initialize_level()
    {
        int[,] level = new int[_dimensions_x,_dimensions_z];
        for (int x = 0; x < _dimensions_x; x++)
        {
            for (int y = 0; y < _dimensions_z; y++)
            {
                level[x, y] = 0;
            }
        }
        
    }


    void _place_entrance()
    {
        level[_dimensions_x, _dimensions_z] = 1;
    }

    void _generate_path(Vector2Int from, int length, int end_of_path)
    {
        if(length == 0)
        {
            return;
        }
        Vector2Int current = from;
        int random = UnityEngine.Random.Range(0, 3);
        Vector2Int direction;
        switch (random)
        {
            case 0:
                direction = new Vector2Int(0,1);
            case 1: 
                direction = new Vector2Int(1,0);
            case 2:
                direction = new Vector2Int(0,-1);
            case 3:
                direction = new Vector2Int(1,-0);
                //
        }
    }

    void _print_level()
    {
        string level_as_str = "\n";
        
        for (int y = 0; y < _dimensions_z; y++)
        {
            //level_as_str += "[ ]";
            for (int x = 0; x < _dimensions_z; x++)
            {
                level_as_str += "[ ]";
            }
        level_as_str+= "\n";
        }
        print(level_as_str);
    }
}
