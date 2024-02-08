using System;
using System.Collections.Generic;
using System.Linq;

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

		public static bool PlayerStillActive(PlayerInfoModel player)
		{
			bool isactive = false;

			foreach ( GridSpotModel ship in player.ShipLocations )
			{
				if ( ship.Status != GRIDSPOTSTATUS.Sunk )
				{
					isactive = true;
				}
			}

			return isactive;
		}

		public static int GetShotCount(PlayerInfoModel player)
		{
			int shotcount = 0;

			foreach ( GridSpotModel shot in player.ShotGrid )
			{
				if ( shot.Status != GRIDSPOTSTATUS.Empty )
				{
					shotcount += 1;
				}
			}

			return shotcount;
		}

		public static bool PlaceShip(PlayerInfoModel model, string location)
		{
			bool output = false;
			(string row, int column) = SplitShotIntoRowAndColumn(location);

			bool isvalidlocation = ValidateGridLocation(model, row, column);
			bool isspotopen = ValidateShipLocation(model, row, column);

			if ( isvalidlocation && isspotopen )
			{
				model.ShipLocations.Add(new GridSpotModel
				{
					SpotLetter = row.ToUpper(),
					SpotNumber = column,
					Status = GRIDSPOTSTATUS.Ship
				});

				output = true;
			}

			return output;
		}

		private static bool ValidateShipLocation(PlayerInfoModel model, string row, int column)
		{
			bool isvalidlocation = true;

			foreach ( GridSpotModel ship in model.ShipLocations )
			{
				if ( ship.SpotLetter == row.ToUpper() && ship.SpotNumber == column )
				{
					isvalidlocation = false;
				}
			}

			return isvalidlocation;
		}

		private static bool ValidateGridLocation(PlayerInfoModel model, string row, int column)
		{
			bool isvalidlocation = false;

			foreach ( GridSpotModel ship in model.ShotGrid )
			{
				if ( ship.SpotLetter == row.ToUpper() && ship.SpotNumber == column )
				{
					isvalidlocation = true;
				}
			}

			return isvalidlocation;
		}

		public static (string row, int column) SplitShotIntoRowAndColumn(string shot)
		{
			if ( shot.Length != 2 )
			{
				throw new ArgumentException("This was an invalid shot type.", "shot");
			}

			char[] shotarray = shot.ToArray();

			string row = shotarray[0].ToString();
			int column = int.Parse(shotarray[1].ToString());
			return (row, column);
		}

		public static bool ValidateShot(PlayerInfoModel player, string row, int column)
		{
			bool isvalidshot = false;

			foreach ( GridSpotModel gridspot in player.ShotGrid )
			{
				if ( gridspot.SpotLetter == row.ToUpper() && gridspot.SpotNumber == column )
				{
					if ( gridspot.Status == GRIDSPOTSTATUS.Empty )
					{
						isvalidshot = true;
					}
				}
			}

			return isvalidshot;
		}

		public static bool IdentifyShotResult(PlayerInfoModel opponent, string row, int column)
		{
			bool isahit = false;

			foreach ( GridSpotModel ship in opponent.ShipLocations )
			{
				if ( ship.SpotLetter == row.ToUpper() && ship.SpotNumber == column )
				{
					isahit = true;
					ship.Status = GRIDSPOTSTATUS.Sunk;
				}
			}

			return isahit;
		}

		public static void MarkShotResult(PlayerInfoModel player, string row, int column, bool isAHit)
		{
			foreach ( GridSpotModel gridspot in player.ShotGrid )
			{
				if ( gridspot.SpotLetter == row.ToUpper() && gridspot.SpotNumber == column )
				{
					gridspot.Status = isAHit ? GRIDSPOTSTATUS.Hit : GRIDSPOTSTATUS.Miss;
				}
			}
		}
	}
}
