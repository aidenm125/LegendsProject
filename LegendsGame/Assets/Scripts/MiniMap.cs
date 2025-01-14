using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    Map1 map;
    List<Coords> roomCoords;
    List<Coords> visitedRooms = new List<Coords>();
    //Coords currPlayerCoords;
    public Transform minimapCenter;
    public GameObject minimapNode;
    int center;

    //GameObject[] nodes;

    Dictionary<Coords, GameObject> nodes = new Dictionary<Coords, GameObject>();

    float xmultiplier = 1.075f;
    float ymultiplier = 1.15f;

    Coords currentRoom = new Coords();

    //liams a movie buff
    public void SpawnMinimap(Coords currentPlayerCoordinate, Coords oldPlayerCoords)
    {
        map = GameObject.FindGameObjectWithTag("MapGenerator").GetComponent<Map1>();
        //currPlayerCoords = map.GetPlayerCoords();
        center = (int)Mathf.Ceil(map.numberOfRooms / 2) + 1;
        //nodes = new GameObject[map.numberOfRooms];
        roomCoords = map.getAllRoomCoords();

        //Keep the active cell in the center of the minimap at all times
        //Use the minimap background as a mask to cut out rest of minimap
        //currPlayerCoords = currentPlayerCoordinate;
        for (int i = 0; i < roomCoords.Count; i++)
        {
            //(x position - center x position) + minimapcenter.x
            ////(y position - center y position) + minimapcenter.y

            //nodes[i] =
            minimapNode.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
            nodes.Add(roomCoords[i], Instantiate(minimapNode, new Vector3(minimapCenter.position.x + minimapNode.transform.localScale.x * xmultiplier * (roomCoords[i].X - center), minimapCenter.position.y + minimapNode.transform.localScale.y * ymultiplier * (roomCoords[i].Y - center), 0), Quaternion.identity));
            nodes[roomCoords[i]].transform.SetParent(gameObject.transform);
            nodes[roomCoords[i]].SetActive(false);
        }

        currentRoom = new Coords(center, center);

        UpdateMiniMap(currentPlayerCoordinate);
        //UpdateMiniMap(currentPlayerCoordinate, oldPlayerCoords);
    }

    float flash = 0.3f;
    Color flashColour = new Color(0,0,0);

    float flashOverTime = 1f;
    float rate = 0.85f;

    private void FixedUpdate()
    {
        flash += flashOverTime * rate * Time.fixedDeltaTime;

        flashColour.r = flash;
        flashColour.g = flash;
        flashColour.b = flash;

        nodes[currentRoom].GetComponent<SpriteRenderer>().color = flashColour;

        if (flash < 0.3f || flash > 0.7f )
        {
            flashOverTime *= -1;
        }
    }

    public void UpdateMiniMap(Coords currentPlayerCoords)
    {
        //Create colour objects
        Color activeColour = new Color(0, 0, 0);
        Color inactiveColour = new Color(1, 1, 1);

        //set current rooms to inactive colour
        nodes[currentRoom].GetComponent<SpriteRenderer>().color = inactiveColour;

        //shift the map to have the current room in the center

        //get distance from currentplayer coord to int center variable
        float distanceX = (minimapNode.transform.localScale.x * xmultiplier) * (currentRoom.X - currentPlayerCoords.X);
        float distanceY = (minimapNode.transform.localScale.y * ymultiplier) * (currentRoom.Y - currentPlayerCoords.Y);

        //shift all nodes by this amount to trend the current room to center
        for (int i = 0; i < roomCoords.Count; i++)
        {
            Vector3 move = nodes[roomCoords[i]].transform.position;
            move.x += distanceX;
            move.y += distanceY;
            nodes[roomCoords[i]].transform.position = move;
        }

        currentRoom = currentPlayerCoords;

        //Set current room to active colour
        nodes[currentRoom].GetComponent<SpriteRenderer>().color = activeColour;

        // this is wrong for reasons i don't feel like explaining
        // commenting out for now

        /*int distanceX, distanceY;
        distanceX = currPlayerCoords.X - roomToMoveTo.X;
        distanceY = currPlayerCoords.Y - roomToMoveTo.Y;
        for(int i = 0; i < roomCoords.Count; i++)
        {
            Vector3 move = nodes[i].transform.position;
            move -= new Vector3(distanceX, distanceY, 0);
            nodes[i].transform.position = move;
        }*/
        //make the colours for the room not active white
        //make the colour of the current room yellow

        if (!visitedRooms.Contains(currentPlayerCoords))
        {
            visitedRooms.Add(currentPlayerCoords);
        }

        for(int i = 0; i < visitedRooms.Count; i++)
        {
            nodes[visitedRooms[i]].SetActive(true);
            for (int j = 0; j < 4; j++)
            {
                Coords coords = new Coords();
                switch(j)
                {
                    case 0://north
                        coords = new Coords(visitedRooms[i].X, visitedRooms[i].Y + 1);
                        break;
                    case 1://east
                        coords = new Coords(visitedRooms[i].X + 1, visitedRooms[i].Y);
                        break;
                    case 2://south
                        coords = new Coords(visitedRooms[i].X, visitedRooms[i].Y - 1);
                        break;
                    case 3://west
                        coords = new Coords(visitedRooms[i].X - 1, visitedRooms[i].Y);
                        break;
                }
                if (!visitedRooms.Contains(coords) && roomCoords.Contains(coords))
                {
//                    Debug.Log(coords);
                    nodes[coords].GetComponent<SpriteRenderer>().color = new Color(0.35f, 0.35f, 0.35f);
                    nodes[coords].SetActive(true);
                }

                // if the j location isn't in visitedRooms and is a valid room, make it grey
            }
        }
    }
}


