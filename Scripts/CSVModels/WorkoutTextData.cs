﻿// This code automatically generated by TableCodeGen
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorkoutTextData
{
	public class Row
	{
		public string id;
		public string voice_id;
		public string EN;
		public string CN;
		public string ZH;
		public string Text(string langKey)
		{
			switch (langKey)
			{
				case "en":
					return EN;
				case "cn":
					return CN;
				case "zh":
					return ZH;
			}

			return $"<color=red>[{langKey}]</color> NO Key !!!!";
		}
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
		for(int i = 1 ; i < grid.Length ; i++)
		{
			Row row = new Row();
			row.id = grid[i][0];
			row.voice_id = grid[i][1];
			row.EN = grid[i][2];
			row.CN = grid[i][3];
			row.ZH = grid[i][4];

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
	
	public string GetTextByVoiceID(string voice, string langID)
	{
		var find = rowList.Find(x => x.voice_id == voice);
		return find != null ? find.Text(langID.ToLower()) : $"Null voice ID: {voice}" ;
	}
}