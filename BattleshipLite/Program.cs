﻿using System;

using BattleshipLibrary;
using BattleshipLibrary.Models;

namespace BattleshipLite
{
	internal class Program
	{
		private static void Main()
		{
			WelcomeMessage();

			PlayerInfoModel activeplayer = CreatePlayer("Player 1");
			PlayerInfoModel opponent = CreatePlayer("Player 2");
			PlayerInfoModel winner = null;

			do
			{
				DisplayShotGrid(activeplayer);

				RecordPlayerShot(activeplayer, opponent);

				bool doesgamecontinue = GameLogic.PlayerStillActive(opponent);

				if ( doesgamecontinue == true )
				{
					// Swap positions
					(activeplayer, opponent) = (opponent, activeplayer);
				}
				else
				{
					winner = activeplayer;
				}
			} while ( winner == null );

			IdentifyWinner(winner);

			_ = Console.ReadLine();
		}

		private static void IdentifyWinner(PlayerInfoModel winner)
		{
			Console.WriteLine($"Congratulations to {winner.UsersName} for winning!");
			Console.WriteLine($"{winner.UsersName} took {GameLogic.GetShotCount(winner)} shots.");
		}

		private static void RecordPlayerShot(PlayerInfoModel activePlayer, PlayerInfoModel opponent)
		{
			bool isvalidshot;
			string row;
			int column;

			do
			{
				string shot = AskForShot();
				(row, column) = GameLogic.SplitShotIntoRowAndColumn(shot);
				isvalidshot = GameLogic.ValidateShot(activePlayer, row, column);

				if ( isvalidshot == false )
				{
					Console.WriteLine("Invalid Shot Location. Please try again.");
				}
			} while ( isvalidshot == false );

			bool isahit = GameLogic.IdentifyShotResult(opponent, row, column);

			if ( isahit )
			{
				Console.WriteLine("That was a hit!");
				_ = Console.ReadLine();
			}
			else
			{
				Console.WriteLine("That was a miss.");
				_ = Console.ReadLine();
			}

			GameLogic.MarkShotResult(activePlayer, row, column, isahit);
		}

		private static string AskForShot()
		{
			Console.Write("Please enter your shot selection: ");
			string output = Console.ReadLine();

			return output;
		}

		private static void DisplayShotGrid(PlayerInfoModel activePlayer)
		{
			Console.Clear();

			string currentrow = activePlayer.ShotGrid[0].SpotLetter;

			foreach ( GridSpotModel gridspot in activePlayer.ShotGrid )
			{
				if ( gridspot.SpotLetter != currentrow )
				{
					Console.WriteLine();
					currentrow = gridspot.SpotLetter;
				}

				if ( gridspot.Status == GRIDSPOTSTATUS.Empty )
				{
					Console.Write($" {gridspot.SpotLetter}{gridspot.SpotNumber} ");
				}
				else if ( gridspot.Status == GRIDSPOTSTATUS.Hit )
				{
					Console.Write(" X  ");
				}
				else if ( gridspot.Status == GRIDSPOTSTATUS.Miss )
				{
					Console.Write(" O  ");
				}
				else
				{
					Console.Write(" ?  ");
				}
			}

			Console.WriteLine();
		}

		private static void WelcomeMessage()
		{
			Console.WriteLine("Welcome to Battleship Lite");
			Console.WriteLine("created by Tim Corey");
			Console.WriteLine();
		}

		private static PlayerInfoModel CreatePlayer(string playerTitle)
		{
			PlayerInfoModel output = new PlayerInfoModel();

			Console.WriteLine($"Player information for {playerTitle}");

			// Ask the user for their name
			output.UsersName = AskForUsersName();

			// Load up the shot grid
			GameLogic.InitializeGrid(output);

			// Ask the user for their 5 ship placements
			PlaceShips(output);

			// Clear
			Console.Clear();

			return output;
		}

		private static string AskForUsersName()
		{
			Console.Write("What is your name: ");
			string output = Console.ReadLine();

			return output;
		}

		private static void PlaceShips(PlayerInfoModel model)
		{
			do
			{
				Console.Write($"Where do you want to place ship number {model.ShipLocations.Count + 1}: ");
				string location = Console.ReadLine();

				bool isvalidlocation = GameLogic.PlaceShip(model, location);

				if ( isvalidlocation == false )
				{
					Console.WriteLine("That was not a valid location. Please try again.");
				}
			} while ( model.ShipLocations.Count < 5 );
		}
	}
}
