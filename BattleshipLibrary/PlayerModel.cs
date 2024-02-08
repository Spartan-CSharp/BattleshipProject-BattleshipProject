using System.Collections.Generic;

namespace BattleshipLibrary
{
	public class PlayerModel
	{
		public string PlayerName
		{
			get; set;
		}
		public List<GridSpot> ShipLocations
		{
			get; set;
		}
		public List<GridSpot> ShotGrid
		{
			get; set;
		}
		public PlayerModel()
		{
			ShipLocations = new List<GridSpot>();
			ShotGrid = new List<GridSpot>();
		}
	}
}
