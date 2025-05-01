using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class Tile : MonoBehaviour
{
    public int x;

    public int y;

    private Item _item;

    public Item Item
    {
        get => _item;

        set
        {
            if (_item == value) return;

            _item = value;

            icon.sprite = _item.sprite;
        }
    }

    public Image icon;

    public Button button;

    public Tile Left => x > 0 ? Board.Instance.Tiles[x - 1, y] : null;
    public Tile Top => y > 0 ? Board.Instance.Tiles[x, y - 1] : null;
    public Tile Right => x < Board.Instance.Width - 1 ? Board.Instance.Tiles[x + 1, y] : null;
    public Tile Bottom => y < Board.Instance.Height - 1 ? Board.Instance.Tiles[x, y + 1] : null;

    public Tile[] Neighbours => new[]
    {
        Left,
        Top,
        Right,
        Bottom
    }; 

    private void Start() => button.onClick.AddListener(() => Board.Instance.Select(this));

    /*public List<Tile> GetConnectedTiles(List<Tile> exclude = null)
    {
        var result = new List<Tile> { this, };

        if (exclude == null)
        {
            exclude = new List<Tile> { this, };
        }
        else
        {
            exclude.Add(this);
        }

        foreach (var neighbour in Neighbours)
        {
            if (neighbour == null || exclude.Contains(neighbour) || neighbour.Item != Item) continue; 

            result.AddRange(neighbour.GetConnectedTiles(exclude));
        }

        return result;
    }*/

    public List<Tile> GetConnectedTiles()
    {
        var connectedTiles = new List<Tile> { this };
        var queue = new Queue<Tile>();
        queue.Enqueue(this);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            foreach (var neighbor in GetNeighbors(current))
            {
                if (neighbor.Item == this.Item && !connectedTiles.Contains(neighbor))
                {
                    connectedTiles.Add(neighbor);
                    queue.Enqueue(neighbor);
                }
            }
        }
        return connectedTiles;
    }

    private IEnumerable<Tile> GetNeighbors(Tile tile)
    {
        var neighbors = new List<Tile>();

        if (tile.x > 0) neighbors.Add(Board.Instance.Tiles[tile.x - 1, tile.y]); // Left
        if (tile.x < Board.Instance.Width - 1) neighbors.Add(Board.Instance.Tiles[tile.x + 1, tile.y]); // Right
        if (tile.y > 0) neighbors.Add(Board.Instance.Tiles[tile.x, tile.y - 1]); // Below
        if (tile.y < Board.Instance.Height - 1) neighbors.Add(Board.Instance.Tiles[tile.x, tile.y + 1]); // Above

        return neighbors;
    }
    
}
