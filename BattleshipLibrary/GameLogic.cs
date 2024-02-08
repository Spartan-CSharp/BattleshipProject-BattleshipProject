using System.Collections.Generic;

using BattleshipLibrary.Models;

namespace BattleshipLibrary
{
	public static class GameLogic
	{
		public static void InitializeGrid(PlayerInfoModel model)
		{
			List<string> letters = new List<string> {
				"A",
				"B",
				"C",
				"D",
				"E"
			};

			List<int> numbers = new List<int> {
				1,
				2,
				3,
				4,
				5
			};

			foreach ( string letter in letters )
			{
				foreach ( int number in numbers )
				{
					AddGridSpot(model, letter, number);
				}
			}
		}

		private static void AddGridSpot(PlayerInfoModel model, string letter, int number)
		{
			GridSpotModel spot = new GridSpotModel
			{
				SpotLetter = letter,
				SpotNumber = number,
				Status = GRIDSPOTSTATUS.Empty
			};

			model.ShotGrid.Add(spot);
		}

		public static bool PlayerStillActive(PlayerInfoModel opponent)
		{
			bool stillhasships = false;

			foreach ( GridSpotModel gridspot in opponent.ShipLocations )
			{
				if ( gridspot.Status == GRIDSPOTSTATUS.Ship )
				{
					stillhasships = true;
				}
			}

			return stillhasships;
		}

		public static int GetShotCount(PlayerInfoModel winner)
		{
			int shotcount = 0;

			foreach ( GridSpotModel gridspot in winner.ShotGrid )
			{
				if ( gridspot.Status == GRIDSPOTSTATUS.Hit || gridspot.Status == GRIDSPOTSTATUS.Miss )
				{
					shotcount++;
				}
			}

			return shotcount;
		}

		public static bool PlaceShip(PlayerInfoModel model, string location)
		{
			bool isvalidlocation = true;
			string row = "";
			int column = 0;

			if ( location.Length > 1 )
			{
				row = location.Substring(0, 1).ToUpper();
				string columntext = location.Substring(1);

				_ = int.TryParse(columntext, out column);
			}

			if ( column < 1 || column > 5 )
			{
				isvalidlocation = false;
			}

			if ( row != "A" && row != "B" && row != "C" && row != "D" && row != "E" )
			{
				isvalidlocation = false;
			}

			foreach ( GridSpotModel gridspot in model.ShipLocations )
			{
				if ( gridspot.SpotLetter == row && gridspot.SpotNumber == column && gridspot.Status != GRIDSPOTSTATUS.Empty )
				{
					isvalidlocation = false;
				}
			}

			if ( isvalidlocation )
			{
				GridSpotModel spotmodel = new GridSpotModel
				{
					SpotLetter = row,
					SpotNumber = column,
					Status = GRIDSPOTSTATUS.Ship
				};
				model.ShipLocations.Add(spotmodel);
			}

			return isvalidlocation;
		}

		public static (string row, int column) SplitShotIntoRowAndColumn(string shot)
		{
			string row = "";
			int column = 0;

			if ( shot.Length > 1 )
			{
				row = shot.Substring(0, 1).ToUpper();
				string columntext = shot.Substring(1);

				_ = int.TryParse(columntext, out column);
			}

			return (row, column);
		}

		public static bool ValidateShot(PlayerInfoModel activePlayer, string row, int column)
		{
			bool isshotvalid = true;

			if ( column < 1 || column > 5 )
			{
				isshotvalid = false;
			}

			if ( row != "A" && row != "B" && row != "C" && row != "D" && row != "E" )
			{
				isshotvalid = false;
			}

			foreach ( GridSpotModel gridspot in activePlayer.ShotGrid )
			{
				if ( gridspot.SpotLetter == row && gridspot.SpotNumber == column && gridspot.Status != GRIDSPOTSTATUS.Empty )
				{
					isshotvalid = false;
				}
			}

			return isshotvalid;
		}

		public static bool IdentifyShotResult(PlayerInfoModel opponent, string row, int column)
		{
			bool isahit = false;

			foreach ( GridSpotModel gridspot in opponent.ShipLocations )
			{
				if ( gridspot.SpotLetter == row && gridspot.SpotNumber == column && gridspot.Status == GRIDSPOTSTATUS.Ship )
				{
					isahit = true;
					gridspot.Status = GRIDSPOTSTATUS.Sunk;
				}
			}

			return isahit;
		}

		public static void MarkShotResult(PlayerInfoModel activePlayer, string row, int column, bool isAHit)
		{
			foreach ( GridSpotModel gridspot in activePlayer.ShotGrid )
			{
				if ( gridspot.SpotLetter == row && gridspot.SpotNumber == column )
				{
					gridspot.Status = isAHit ? GRIDSPOTSTATUS.Hit : GRIDSPOTSTATUS.Miss;
				}
			}
		}
	}
}
