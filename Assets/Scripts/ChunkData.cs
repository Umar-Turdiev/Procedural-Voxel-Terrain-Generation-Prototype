using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkData
{
    public BlockType[] Blocks;          /* Stores all the references to the blocks. */
    public int ChunkWidth = 16;         /* Chunk is a square when looking from the top, therefore X,Z length are the same. */
    public int ChunkHeight = 100;       /* Chunk height along the Y axis. */
    public World WorldReference;        /* Reference to the world that's referencing this chunk. Incase the edge voxels is 
                                           smodified, we'll also need to modify the neighboring chunk. */
    public Vector3Int WorldPosition;    /* Position where the chunk is placed. */

    public bool IsModifiedByPlayer = false;  /* Only need to save chunks that are modified, for the unmodified, we can 
                                                regenerate them. */

    public ChunkData(World worldReference, Vector3Int worldPosition)
    {
        this.WorldReference = worldReference;
        this.WorldPosition = worldPosition;

        this.Blocks = new BlockType[this.ChunkWidth * this.ChunkWidth * this.ChunkHeight];
    }
}
