namespace BattleshipLibrary.Models
{
	public class GridSpotModel
	{
		public string SpotLetter
		{
			get; set;
		}
		public int SpotNumber
		{
			get; set;
		}
		public GRIDSPOTSTATUS Status
		{
			get; set;
		} = GRIDSPOTSTATUS.Empty;
	}
}
