using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CellularAutomata
{
	public static void IterateHexaSphere(ref HexaSphere hexaSpehre, Vector2 bx, Vector2 sy, int iterationCount){
		for (int i = 0; i < iterationCount; i++) {
			foreach (Cell c1 in hexaSpehre.Cells) {
				int aliveNeighbourCount = CheckAliveNeighbourCount (c1);
				if(aliveNeighbourCount >= bx.x && aliveNeighbourCount <= bx.y){
					c1.CellState = true;

				}
				if(aliveNeighbourCount < sy.x && aliveNeighbourCount > sy.y){
					c1.CellState = false;
				}
			}
		}
	
	}

	static int CheckAliveNeighbourCount(Cell c){
		int aliveNeighbourCount = 0;
		foreach(Cell c1 in c.Neighbours){
			if(c1.CellState){
				aliveNeighbourCount += 1;
			}
		}
		return aliveNeighbourCount;
	}
}
