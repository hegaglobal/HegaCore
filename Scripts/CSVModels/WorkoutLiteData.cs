// This code automatically generated by TableCodeGen
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorkoutLiteData
{
	public class Row
	{
		public string id;
		public string phase;
		public string command;
		public string sfx;
		public bool loop_sfx; // empty = no loop. 
		public float delay; // delay between 2 audioClip. Input 10000 to play once. 
		public List<string> voices;
	}

	List<Row> rowList = new List<Row>();
	bool isLoaded = false;

	public bool IsLoaded()
	{
		return isLoaded;
	}

	public List<Row> GetRowList()
	{
		return rowList;
	}
    public void Load(TextAsset csv)
	{
        Load(csv.text);
    }
	public void Load(string text)
	{
		rowList.Clear();
		string[][] grid = CsvParser2.Parse(text);
		for (int i = 1; i < grid.Length; i++)
		{
			Row row = new Row();
			row.id = grid[i][0];
			row.phase = grid[i][1];
			row.command = grid[i][2];
			row.sfx = grid[i][3];
			row.loop_sfx = !string.IsNullOrEmpty(grid[i][4]);
			
			if (string.IsNullOrEmpty(grid[i][5]))
				row.delay = 0;
			else
				row.delay = grid[i][5].ParseFloatUS();
			
			row.voices = new List<string>();
			if (!string.IsNullOrEmpty(grid[i][6]))
				row.voices.Add(grid[i][6]);
			if (!string.IsNullOrEmpty(grid[i][7]))
				row.voices.Add(grid[i][7]);
			if (!string.IsNullOrEmpty(grid[i][8]))
				row.voices.Add(grid[i][8]);
			if (!string.IsNullOrEmpty(grid[i][9]))
				row.voices.Add(grid[i][9]);
			if (!string.IsNullOrEmpty(grid[i][10]))
				row.voices.Add(grid[i][10]);
			rowList.Add(row);
		}

		isLoaded = true;
	}

	public int NumRows()
	{
		return rowList.Count;
	}

	public Row GetAt(int i)
	{
		if(rowList.Count <= i)
			return null;
		return rowList[i];
	}
}